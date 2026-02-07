using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200007D RID: 125
	public sealed class ByteSerializer : Serializer<byte>
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x0001CB50 File Offset: 0x0001AD50
		public override byte ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				byte result;
				if (!reader.ReadByte(out result))
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

		// Token: 0x06000415 RID: 1045 RVA: 0x0001CC0D File Offset: 0x0001AE0D
		public override void WriteValue(string name, byte value, IDataWriter writer)
		{
			writer.WriteByte(name, value);
		}
	}
}
