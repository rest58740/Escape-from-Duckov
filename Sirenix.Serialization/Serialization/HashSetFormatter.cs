using System;
using System.Collections.Generic;

namespace Sirenix.Serialization
{
	// Token: 0x02000036 RID: 54
	public class HashSetFormatter<T> : BaseFormatter<HashSet<T>>
	{
		// Token: 0x06000286 RID: 646 RVA: 0x00013144 File Offset: 0x00011344
		static HashSetFormatter()
		{
			new HashSetFormatter<int>();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override HashSet<T> GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00013160 File Offset: 0x00011360
		protected override void DeserializeImplementation(ref HashSet<T> value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					value = new HashSet<T>();
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
						value.Add(HashSetFormatter<T>.TSerializer.ReadValue(reader));
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

		// Token: 0x0600028A RID: 650 RVA: 0x00013268 File Offset: 0x00011468
		protected override void SerializeImplementation(ref HashSet<T> value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)value.Count);
				foreach (T value2 in value)
				{
					try
					{
						HashSetFormatter<T>.TSerializer.WriteValue(value2, writer);
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

		// Token: 0x040000C9 RID: 201
		private static readonly Serializer<T> TSerializer = Serializer.Get<T>();
	}
}
