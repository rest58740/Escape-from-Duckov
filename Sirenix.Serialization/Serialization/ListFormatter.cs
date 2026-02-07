using System;
using System.Collections.Generic;

namespace Sirenix.Serialization
{
	// Token: 0x0200003C RID: 60
	public class ListFormatter<T> : BaseFormatter<List<T>>
	{
		// Token: 0x0600029B RID: 667 RVA: 0x000136BE File Offset: 0x000118BE
		static ListFormatter()
		{
			new ListFormatter<int>();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override List<T> GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000136D8 File Offset: 0x000118D8
		protected override void DeserializeImplementation(ref List<T> value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					value = new List<T>((int)num);
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
						value.Add(ListFormatter<T>.TSerializer.ReadValue(reader));
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

		// Token: 0x0600029F RID: 671 RVA: 0x000137E0 File Offset: 0x000119E0
		protected override void SerializeImplementation(ref List<T> value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)value.Count);
				for (int i = 0; i < value.Count; i++)
				{
					try
					{
						ListFormatter<T>.TSerializer.WriteValue(value[i], writer);
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

		// Token: 0x040000D3 RID: 211
		private static readonly Serializer<T> TSerializer = Serializer.Get<T>();
	}
}
