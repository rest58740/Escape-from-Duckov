using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200007E RID: 126
	public sealed class CharSerializer : Serializer<char>
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x0001CC20 File Offset: 0x0001AE20
		public override char ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.String)
			{
				char result;
				if (!reader.ReadChar(out result))
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
				}
				return result;
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Expected entry of type ",
				EntryType.String.ToString(),
				", but got entry '",
				text,
				"' of type ",
				entryType.ToString()
			}));
			reader.SkipEntry();
			return '\0';
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001CCDD File Offset: 0x0001AEDD
		public override void WriteValue(string name, char value, IDataWriter writer)
		{
			writer.WriteChar(name, value);
		}
	}
}
