using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000026 RID: 38
	public sealed class DateTimeFormatter : MinimalBaseFormatter<DateTime>
	{
		// Token: 0x06000247 RID: 583 RVA: 0x0000FE14 File Offset: 0x0000E014
		protected override void Read(ref DateTime value, IDataReader reader)
		{
			string text;
			if (reader.PeekEntry(out text) == EntryType.Integer)
			{
				long num;
				reader.ReadInt64(out num);
				value = DateTime.FromBinary(num);
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000FE41 File Offset: 0x0000E041
		protected override void Write(ref DateTime value, IDataWriter writer)
		{
			writer.WriteInt64(null, value.ToBinary());
		}
	}
}
