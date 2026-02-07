using System;
using System.Collections;

namespace Sirenix.Serialization
{
	// Token: 0x02000023 RID: 35
	public class ArrayListFormatter : BaseFormatter<ArrayList>
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override ArrayList GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000F108 File Offset: 0x0000D308
		protected override void DeserializeImplementation(ref ArrayList value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					value = new ArrayList((int)num);
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
						value.Add(ArrayListFormatter.ObjectSerializer.ReadValue(reader));
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

		// Token: 0x0600022A RID: 554 RVA: 0x0000F210 File Offset: 0x0000D410
		protected override void SerializeImplementation(ref ArrayList value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)value.Count);
				for (int i = 0; i < value.Count; i++)
				{
					try
					{
						ArrayListFormatter.ObjectSerializer.WriteValue(value[i], writer);
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

		// Token: 0x0400008F RID: 143
		private static readonly Serializer<object> ObjectSerializer = Serializer.Get<object>();
	}
}
