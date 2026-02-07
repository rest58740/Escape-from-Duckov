using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200008F RID: 143
	public sealed class UInt64Serializer : Serializer<ulong>
	{
		// Token: 0x0600045A RID: 1114 RVA: 0x0001EDA0 File Offset: 0x0001CFA0
		public override ulong ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				ulong result;
				if (!reader.ReadUInt64(out result))
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
			return 0UL;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001EE5E File Offset: 0x0001D05E
		public override void WriteValue(string name, ulong value, IDataWriter writer)
		{
			writer.WriteUInt64(name, value);
		}
	}
}
