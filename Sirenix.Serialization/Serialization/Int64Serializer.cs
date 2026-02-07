using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000086 RID: 134
	public sealed class Int64Serializer : Serializer<long>
	{
		// Token: 0x06000432 RID: 1074 RVA: 0x0001E2C0 File Offset: 0x0001C4C0
		public override long ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				long result;
				if (!reader.ReadInt64(out result))
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
				}
				return result;
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Expected entry of type ",
				EntryType.Integer.ToString(),
				", but got entry '",
				text,
				"' of type ",
				entryType.ToString()
			}));
			reader.SkipEntry();
			return 0L;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001E37E File Offset: 0x0001C57E
		public override void WriteValue(string name, long value, IDataWriter writer)
		{
			writer.WriteInt64(name, value);
		}
	}
}
