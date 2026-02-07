using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x02000052 RID: 82
	public class WeakStackFormatter : WeakBaseFormatter
	{
		// Token: 0x0600030C RID: 780 RVA: 0x0001664C File Offset: 0x0001484C
		public WeakStackFormatter(Type serializedType) : base(serializedType)
		{
			Type[] argumentsOfInheritedOpenGenericClass = serializedType.GetArgumentsOfInheritedOpenGenericClass(typeof(Stack));
			this.ElementSerializer = Serializer.Get(argumentsOfInheritedOpenGenericClass[0]);
			this.IsPlainStack = (serializedType.IsGenericType && serializedType.GetGenericTypeDefinition() == typeof(Stack));
			if (this.PushMethod == null)
			{
				throw new SerializationAbortException("Can't serialize type '" + serializedType.GetNiceFullName() + "' because no proper Push method was found.");
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x000166D0 File Offset: 0x000148D0
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
					if (this.IsPlainStack)
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
						this.PushMethod.Invoke(value, array);
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

		// Token: 0x0600030F RID: 783 RVA: 0x00016828 File Offset: 0x00014A28
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			try
			{
				ICollection collection = (ICollection)value;
				writer.BeginArrayNode((long)collection.Count);
				using (Cache<List<object>> cache = Cache<List<object>>.Claim())
				{
					List<object> value2 = cache.Value;
					value2.Clear();
					foreach (object obj in collection)
					{
						value2.Add(obj);
					}
					for (int i = value2.Count - 1; i >= 0; i--)
					{
						try
						{
							this.ElementSerializer.WriteValueWeak(value2[i], writer);
						}
						catch (Exception exception)
						{
							writer.Context.Config.DebugContext.LogException(exception);
						}
					}
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x040000F5 RID: 245
		private readonly Serializer ElementSerializer;

		// Token: 0x040000F6 RID: 246
		private readonly bool IsPlainStack;

		// Token: 0x040000F7 RID: 247
		private readonly MethodInfo PushMethod;
	}
}
