using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200008B RID: 139
	public sealed class SingleSerializer : Serializer<float>
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x0001E9D8 File Offset: 0x0001CBD8
		public override float ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.FloatingPoint || entryType == EntryType.Integer)
			{
				float result;
				if (!reader.ReadSingle(out result))
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
				}
				return result;
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Expected entry of type ",
				EntryType.FloatingPoint.ToString(),
				" or ",
				EntryType.Integer.ToString(),
				", but got entry '",
				text,
				"' of type ",
				entryType.ToString()
			}));
			reader.SkipEntry();
			return 0f;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001EAB7 File Offset: 0x0001CCB7
		public override void WriteValue(string name, float value, IDataWriter writer)
		{
			writer.WriteSingle(name, value);
		}
	}
}
