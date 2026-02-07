using System;
using System.Globalization;

namespace Sirenix.Serialization
{
	// Token: 0x02000027 RID: 39
	public sealed class DateTimeOffsetFormatter : MinimalBaseFormatter<DateTimeOffset>
	{
		// Token: 0x0600024A RID: 586 RVA: 0x0000FE58 File Offset: 0x0000E058
		protected override void Read(ref DateTimeOffset value, IDataReader reader)
		{
			string text;
			if (reader.PeekEntry(out text) == EntryType.String)
			{
				string text2;
				reader.ReadString(out text2);
				DateTimeOffset.TryParse(text2, ref value);
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000FE81 File Offset: 0x0000E081
		protected override void Write(ref DateTimeOffset value, IDataWriter writer)
		{
			writer.WriteString(null, value.ToString("O", CultureInfo.InvariantCulture));
		}
	}
}
