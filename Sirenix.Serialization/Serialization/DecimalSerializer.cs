using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000080 RID: 128
	public sealed class DecimalSerializer : Serializer<decimal>
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x0001DD0C File Offset: 0x0001BF0C
		public override decimal ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.FloatingPoint || entryType == EntryType.Integer)
			{
				decimal result;
				if (!reader.ReadDecimal(out result))
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry of type " + entryType.ToString());
				}
				return result;
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Expected entry of type ",
				EntryType.FloatingPoint.ToString(),
				" or ",
				EntryType.Integer.ToString(),
				", but got entry of type ",
				entryType.ToString()
			}));
			reader.SkipEntry();
			return 0m;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001DDD9 File Offset: 0x0001BFD9
		public override void WriteValue(string name, decimal value, IDataWriter writer)
		{
			writer.WriteDecimal(name, value);
		}
	}
}
