using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000084 RID: 132
	public sealed class Int16Serializer : Serializer<short>
	{
		// Token: 0x0600042C RID: 1068 RVA: 0x0001E120 File Offset: 0x0001C320
		public override short ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				short result;
				if (!reader.ReadInt16(out result))
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

		// Token: 0x0600042D RID: 1069 RVA: 0x0001E1DD File Offset: 0x0001C3DD
		public override void WriteValue(string name, short value, IDataWriter writer)
		{
			writer.WriteInt16(name, value);
		}
	}
}
