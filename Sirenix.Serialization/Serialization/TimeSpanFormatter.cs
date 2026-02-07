using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000053 RID: 83
	public sealed class TimeSpanFormatter : MinimalBaseFormatter<TimeSpan>
	{
		// Token: 0x06000310 RID: 784 RVA: 0x00016924 File Offset: 0x00014B24
		protected override void Read(ref TimeSpan value, IDataReader reader)
		{
			string text;
			if (reader.PeekEntry(out text) == EntryType.Integer)
			{
				long num;
				reader.ReadInt64(out num);
				value = new TimeSpan(num);
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00016951 File Offset: 0x00014B51
		protected override void Write(ref TimeSpan value, IDataWriter writer)
		{
			writer.WriteInt64(null, value.Ticks);
		}
	}
}
