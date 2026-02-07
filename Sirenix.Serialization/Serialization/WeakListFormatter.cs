using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200003D RID: 61
	public class WeakListFormatter : WeakBaseFormatter
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x00013864 File Offset: 0x00011A64
		public WeakListFormatter(Type serializedType) : base(serializedType)
		{
			Type[] argumentsOfInheritedOpenGenericClass = serializedType.GetArgumentsOfInheritedOpenGenericClass(typeof(List));
			this.ElementSerializer = Serializer.Get(argumentsOfInheritedOpenGenericClass[0]);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00013898 File Offset: 0x00011A98
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
					value = Activator.CreateInstance(this.SerializedType, new object[]
					{
						(int)num
					});
					IList list = (IList)value;
					base.RegisterReferenceID(value, reader);
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
						list.Add(this.ElementSerializer.ReadValueWeak(reader));
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

		// Token: 0x060002A3 RID: 675 RVA: 0x000139C0 File Offset: 0x00011BC0
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			try
			{
				IList list = (IList)value;
				writer.BeginArrayNode((long)list.Count);
				for (int i = 0; i < list.Count; i++)
				{
					try
					{
						this.ElementSerializer.WriteValueWeak(list[i], writer);
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

		// Token: 0x040000D4 RID: 212
		private readonly Serializer ElementSerializer;
	}
}
