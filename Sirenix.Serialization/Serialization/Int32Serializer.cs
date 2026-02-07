using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000085 RID: 133
	public sealed class Int32Serializer : Serializer<int>
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x0001E1F0 File Offset: 0x0001C3F0
		public override int ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				int result;
				if (!reader.ReadInt32(out result))
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
			return 0;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001E2AD File Offset: 0x0001C4AD
		public override void WriteValue(string name, int value, IDataWriter writer)
		{
			writer.WriteInt32(name, value);
		}
	}
}
