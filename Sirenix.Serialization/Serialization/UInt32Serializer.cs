using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200008E RID: 142
	public sealed class UInt32Serializer : Serializer<uint>
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x0001ECD0 File Offset: 0x0001CED0
		public override uint ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				uint result;
				if (!reader.ReadUInt32(out result))
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
			return 0U;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001ED8D File Offset: 0x0001CF8D
		public override void WriteValue(string name, uint value, IDataWriter writer)
		{
			writer.WriteUInt32(name, value);
		}
	}
}
