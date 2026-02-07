using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000089 RID: 137
	public abstract class Serializer
	{
		// Token: 0x0600043B RID: 1083 RVA: 0x000021B8 File Offset: 0x000003B8
		[Conditional("UNITY_EDITOR")]
		protected static void FireOnSerializedType(Type type)
		{
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001E53B File Offset: 0x0001C73B
		public static Serializer GetForValue(object value)
		{
			if (value == null)
			{
				return Serializer.Get(typeof(object));
			}
			return Serializer.Get(value.GetType());
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001E55B File Offset: 0x0001C75B
		public static Serializer<T> Get<T>()
		{
			return (Serializer<T>)Serializer.Get(typeof(T), false);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001E572 File Offset: 0x0001C772
		public static Serializer Get(Type type)
		{
			return Serializer.Get(type, true);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001E57C File Offset: 0x0001C77C
		private static Serializer Get(Type type, bool allowWeakFallback)
		{
			if (type == null)
			{
				throw new ArgumentNullException();
			}
			Dictionary<Type, Serializer> dictionary = allowWeakFallback ? Serializer.Weak_ReaderWriterCache : Serializer.Strong_ReaderWriterCache;
			object @lock = Serializer.LOCK;
			Serializer serializer;
			lock (@lock)
			{
				if (!dictionary.TryGetValue(type, ref serializer))
				{
					serializer = Serializer.Create(type, allowWeakFallback);
					dictionary.Add(type, serializer);
				}
			}
			return serializer;
		}

		// Token: 0x06000440 RID: 1088
		public abstract object ReadValueWeak(IDataReader reader);

		// Token: 0x06000441 RID: 1089 RVA: 0x0001E5F0 File Offset: 0x0001C7F0
		public void WriteValueWeak(object value, IDataWriter writer)
		{
			this.WriteValueWeak(null, value, writer);
		}

		// Token: 0x06000442 RID: 1090
		public abstract void WriteValueWeak(string name, object value, IDataWriter writer);

		// Token: 0x06000443 RID: 1091 RVA: 0x0001E5FC File Offset: 0x0001C7FC
		private static Serializer Create(Type type, bool allowWeakfallback)
		{
			ExecutionEngineException ex = null;
			try
			{
				Type type2 = null;
				if (type.IsEnum)
				{
					if (allowWeakfallback && !EmitUtilities.CanEmit)
					{
						return new AnySerializer(type);
					}
					type2 = typeof(EnumSerializer<>).MakeGenericType(new Type[]
					{
						type
					});
				}
				else
				{
					if (FormatterUtilities.IsPrimitiveType(type))
					{
						try
						{
							type2 = Serializer.PrimitiveReaderWriterTypes[type];
							goto IL_9C;
						}
						catch (KeyNotFoundException)
						{
							UnityEngine.Debug.LogError("Failed to find primitive serializer for " + type.Name);
							goto IL_9C;
						}
					}
					if (allowWeakfallback && !EmitUtilities.CanEmit)
					{
						return new AnySerializer(type);
					}
					type2 = typeof(ComplexTypeSerializer<>).MakeGenericType(new Type[]
					{
						type
					});
				}
				IL_9C:
				return (Serializer)Activator.CreateInstance(type2);
			}
			catch (TargetInvocationException ex2)
			{
				if (!(ex2.GetBaseException() is ExecutionEngineException))
				{
					throw ex2;
				}
				ex = (ex2.GetBaseException() as ExecutionEngineException);
			}
			catch (TypeInitializationException ex3)
			{
				if (!(ex3.GetBaseException() is ExecutionEngineException))
				{
					throw ex3;
				}
				ex = (ex3.GetBaseException() as ExecutionEngineException);
			}
			catch (ExecutionEngineException ex4)
			{
				ex = ex4;
			}
			if (allowWeakfallback)
			{
				return new AnySerializer(type);
			}
			Serializer.LogAOTError(type, ex);
			throw ex;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0001E748 File Offset: 0x0001C948
		private static void LogAOTError(Type type, ExecutionEngineException ex)
		{
			UnityEngine.Debug.LogError(string.Concat(new string[]
			{
				"No AOT serializer was pre-generated for the type '",
				type.GetNiceFullName(),
				"'. Please use Odin's AOT generation feature to generate an AOT dll before building, and ensure that '",
				type.GetNiceFullName(),
				"' is in the list of supported types after a scan. If it is not, please report an issue and add it to the list manually."
			}));
			throw new SerializationAbortException("AOT serializer was missing for type '" + type.GetNiceFullName() + "'.");
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001E7AC File Offset: 0x0001C9AC
		// Note: this type is marked as 'beforefieldinit'.
		static Serializer()
		{
			Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
			dictionary.Add(typeof(char), typeof(CharSerializer));
			dictionary.Add(typeof(string), typeof(StringSerializer));
			dictionary.Add(typeof(sbyte), typeof(SByteSerializer));
			dictionary.Add(typeof(short), typeof(Int16Serializer));
			dictionary.Add(typeof(int), typeof(Int32Serializer));
			dictionary.Add(typeof(long), typeof(Int64Serializer));
			dictionary.Add(typeof(byte), typeof(ByteSerializer));
			dictionary.Add(typeof(ushort), typeof(UInt16Serializer));
			dictionary.Add(typeof(uint), typeof(UInt32Serializer));
			dictionary.Add(typeof(ulong), typeof(UInt64Serializer));
			dictionary.Add(typeof(decimal), typeof(DecimalSerializer));
			dictionary.Add(typeof(bool), typeof(BooleanSerializer));
			dictionary.Add(typeof(float), typeof(SingleSerializer));
			dictionary.Add(typeof(double), typeof(DoubleSerializer));
			dictionary.Add(typeof(IntPtr), typeof(IntPtrSerializer));
			dictionary.Add(typeof(UIntPtr), typeof(UIntPtrSerializer));
			dictionary.Add(typeof(Guid), typeof(GuidSerializer));
			Serializer.PrimitiveReaderWriterTypes = dictionary;
			Serializer.LOCK = new object();
			Serializer.Weak_ReaderWriterCache = new Dictionary<Type, Serializer>(FastTypeComparer.Instance);
			Serializer.Strong_ReaderWriterCache = new Dictionary<Type, Serializer>(FastTypeComparer.Instance);
		}

		// Token: 0x04000182 RID: 386
		private static readonly Dictionary<Type, Type> PrimitiveReaderWriterTypes;

		// Token: 0x04000183 RID: 387
		private static readonly object LOCK;

		// Token: 0x04000184 RID: 388
		private static readonly Dictionary<Type, Serializer> Weak_ReaderWriterCache;

		// Token: 0x04000185 RID: 389
		private static readonly Dictionary<Type, Serializer> Strong_ReaderWriterCache;
	}
}
