using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x02000049 RID: 73
	public class WeakQueueFormatter : WeakBaseFormatter
	{
		// Token: 0x060002E4 RID: 740 RVA: 0x00015318 File Offset: 0x00013518
		public WeakQueueFormatter(Type serializedType) : base(serializedType)
		{
			Type[] argumentsOfInheritedOpenGenericClass = serializedType.GetArgumentsOfInheritedOpenGenericClass(typeof(Queue));
			this.ElementSerializer = Serializer.Get(argumentsOfInheritedOpenGenericClass[0]);
			this.IsPlainQueue = (serializedType.IsGenericType && serializedType.GetGenericTypeDefinition() == typeof(Queue));
			this.EnqueueMethod = serializedType.GetMethod("Enqueue", 52, null, new Type[]
			{
				argumentsOfInheritedOpenGenericClass[0]
			}, null);
			if (this.EnqueueMethod == null)
			{
				throw new SerializationAbortException("Can't serialize type '" + serializedType.GetNiceFullName() + "' because no proper Enqueue method was found.");
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000153BC File Offset: 0x000135BC
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					if (this.IsPlainQueue)
					{
						value = Activator.CreateInstance(this.SerializedType, new object[]
						{
							(int)num
						});
					}
					else
					{
						value = Activator.CreateInstance(this.SerializedType);
					}
					ICollection collection = (ICollection)value;
					base.RegisterReferenceID(value, reader);
					object[] array = new object[1];
					int num2 = 0;
					while ((long)num2 < num)
					{
						if (reader.PeekEntry(out text) == EntryType.EndOfArray)
						{
							reader.Context.Config.DebugContext.LogError(string.Concat(new string[]
							{
								"Reached end of array after ",
								num2.ToString(),
								" elements, when ",
								num.ToString(),
								" elements were expected."
							}));
							break;
						}
						array[0] = this.ElementSerializer.ReadValueWeak(reader);
						this.EnqueueMethod.Invoke(value, array);
						if (!reader.IsInArrayNode)
						{
							reader.Context.Config.DebugContext.LogError("Reading array went wrong. Data dump: " + reader.GetDataDump());
							break;
						}
						num2++;
					}
					return;
				}
				finally
				{
					reader.ExitArray();
				}
			}
			reader.SkipEntry();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001551C File Offset: 0x0001371C
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			try
			{
				ICollection collection = (ICollection)value;
				writer.BeginArrayNode((long)collection.Count);
				foreach (object value2 in collection)
				{
					try
					{
						this.ElementSerializer.WriteValueWeak(value2, writer);
					}
					catch (Exception exception)
					{
						writer.Context.Config.DebugContext.LogException(exception);
					}
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x040000EB RID: 235
		private readonly Serializer ElementSerializer;

		// Token: 0x040000EC RID: 236
		private readonly bool IsPlainQueue;

		// Token: 0x040000ED RID: 237
		private MethodInfo EnqueueMethod;
	}
}
