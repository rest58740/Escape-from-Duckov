using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200008D RID: 141
	public sealed class UInt16Serializer : Serializer<ushort>
	{
		// Token: 0x06000454 RID: 1108 RVA: 0x0001EC00 File Offset: 0x0001CE00
		public override ushort ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				ushort result;
				if (!reader.ReadUInt16(out result))
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

		// Token: 0x06000455 RID: 1109 RVA: 0x0001ECBD File Offset: 0x0001CEBD
		public override void WriteValue(string name, ushort value, IDataWriter writer)
		{
			writer.WriteUInt16(name, value);
		}
	}
}
