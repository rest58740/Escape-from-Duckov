using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x02000037 RID: 55
	public class WeakHashSetFormatter : WeakBaseFormatter
	{
		// Token: 0x0600028B RID: 651 RVA: 0x00013304 File Offset: 0x00011504
		public WeakHashSetFormatter(Type serializedType) : base(serializedType)
		{
			Type[] argumentsOfInheritedOpenGenericClass = serializedType.GetArgumentsOfInheritedOpenGenericClass(typeof(HashSet<>));
			this.ElementSerializer = Serializer.Get(argumentsOfInheritedOpenGenericClass[0]);
			this.AddMethod = serializedType.GetMethod("Add", 52, null, new Type[]
			{
				argumentsOfInheritedOpenGenericClass[0]
			}, null);
			this.CountProperty = serializedType.GetProperty("Count", 52);
			if (this.AddMethod == null)
			{
				throw new SerializationAbortException("Can't serialize/deserialize hashset of type '" + serializedType.GetNiceFullName() + "' since a proper Add method wasn't found.");
			}
			if (this.CountProperty == null)
			{
				throw new SerializationAbortException("Can't serialize/deserialize hashset of type '" + serializedType.GetNiceFullName() + "' since a proper Count property wasn't found.");
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x000133C0 File Offset: 0x000115C0
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
					value = Activator.CreateInstance(this.SerializedType);
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
						this.AddMethod.Invoke(value, array);
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

		// Token: 0x0600028E RID: 654 RVA: 0x000134E4 File Offset: 0x000116E4
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)((int)this.CountProperty.GetValue(value, null)));
				foreach (object value2 in ((IEnumerable)value))
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

		// Token: 0x040000CA RID: 202
		private readonly Serializer ElementSerializer;

		// Token: 0x040000CB RID: 203
		private readonly MethodInfo AddMethod;

		// Token: 0x040000CC RID: 204
		private readonly PropertyInfo CountProperty;
	}
}
