using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200008C RID: 140
	public sealed class StringSerializer : Serializer<string>
	{
		// Token: 0x06000451 RID: 1105 RVA: 0x0001EACC File Offset: 0x0001CCCC
		public override string ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.String)
			{
				string result;
				if (!reader.ReadString(out result))
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
				}
				return result;
			}
			if (entryType == EntryType.Null)
			{
				if (!reader.ReadNull())
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
				}
				return null;
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Expected entry of type ",
				EntryType.String.ToString(),
				" or ",
				EntryType.Null.ToString(),
				", but got entry '",
				text,
				"' of type ",
				entryType.ToString()
			}));
			reader.SkipEntry();
			return null;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001EBE3 File Offset: 0x0001CDE3
		public override void WriteValue(string name, string value, IDataWriter writer)
		{
			if (value == null)
			{
				writer.WriteNull(name);
				return;
			}
			writer.WriteString(name, value);
		}
	}
}
