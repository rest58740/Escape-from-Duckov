using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000088 RID: 136
	public sealed class SByteSerializer : Serializer<sbyte>
	{
		// Token: 0x06000438 RID: 1080 RVA: 0x0001E46C File Offset: 0x0001C66C
		public override sbyte ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				sbyte result;
				if (!reader.ReadSByte(out result))
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

		// Token: 0x06000439 RID: 1081 RVA: 0x0001E529 File Offset: 0x0001C729
		public override void WriteValue(string name, sbyte value, IDataWriter writer)
		{
			writer.WriteSByte(name, value);
		}
	}
}
