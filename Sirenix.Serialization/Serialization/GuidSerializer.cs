using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000083 RID: 131
	public sealed class GuidSerializer : Serializer<Guid>
	{
		// Token: 0x06000429 RID: 1065 RVA: 0x0001E048 File Offset: 0x0001C248
		public override Guid ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Guid)
			{
				Guid result;
				if (!reader.ReadGuid(out result))
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
				}
				return result;
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Expected entry of type ",
				EntryType.Guid.ToString(),
				", but got entry '",
				text,
				"' of type ",
				entryType.ToString()
			}));
			reader.SkipEntry();
			return default(Guid);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001E10E File Offset: 0x0001C30E
		public override void WriteValue(string name, Guid value, IDataWriter writer)
		{
			writer.WriteGuid(name, value);
		}
	}
}
