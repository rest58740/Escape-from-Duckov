using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000081 RID: 129
	public sealed class DoubleSerializer : Serializer<double>
	{
		// Token: 0x06000422 RID: 1058 RVA: 0x0001DDEC File Offset: 0x0001BFEC
		public override double ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.FloatingPoint || entryType == EntryType.Integer)
			{
				double result;
				if (!reader.ReadDouble(out result))
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
			return 0.0;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001DECF File Offset: 0x0001C0CF
		public override void WriteValue(string name, double value, IDataWriter writer)
		{
			writer.WriteDouble(name, value);
		}
	}
}
