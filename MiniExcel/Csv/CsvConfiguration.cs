using System;
using System.IO;
using System.Text;

namespace MiniExcelLibs.Csv
{
	// Token: 0x02000064 RID: 100
	public class CsvConfiguration : Configuration
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00012B5C File Offset: 0x00010D5C
		// (set) Token: 0x0600034E RID: 846 RVA: 0x00012B64 File Offset: 0x00010D64
		public char Seperator { get; set; } = ',';

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00012B6D File Offset: 0x00010D6D
		// (set) Token: 0x06000350 RID: 848 RVA: 0x00012B75 File Offset: 0x00010D75
		public string NewLine { get; set; } = "\r\n";

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000351 RID: 849 RVA: 0x00012B7E File Offset: 0x00010D7E
		// (set) Token: 0x06000352 RID: 850 RVA: 0x00012B86 File Offset: 0x00010D86
		public bool ReadLineBreaksWithinQuotes { get; set; } = true;

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00012B8F File Offset: 0x00010D8F
		// (set) Token: 0x06000354 RID: 852 RVA: 0x00012B97 File Offset: 0x00010D97
		public bool ReadEmptyStringAsNull { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00012BA0 File Offset: 0x00010DA0
		// (set) Token: 0x06000356 RID: 854 RVA: 0x00012BA8 File Offset: 0x00010DA8
		public bool AlwaysQuote { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00012BB1 File Offset: 0x00010DB1
		// (set) Token: 0x06000358 RID: 856 RVA: 0x00012BB9 File Offset: 0x00010DB9
		public Func<string, string[]> SplitFn { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00012BC2 File Offset: 0x00010DC2
		// (set) Token: 0x0600035A RID: 858 RVA: 0x00012BCA File Offset: 0x00010DCA
		public Func<Stream, StreamReader> StreamReaderFunc { get; set; } = (Stream stream) => new StreamReader(stream, CsvConfiguration._defaultEncoding);

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00012BD3 File Offset: 0x00010DD3
		// (set) Token: 0x0600035C RID: 860 RVA: 0x00012BDB File Offset: 0x00010DDB
		public Func<Stream, StreamWriter> StreamWriterFunc { get; set; } = (Stream stream) => new StreamWriter(stream, CsvConfiguration._defaultEncoding);

		// Token: 0x04000151 RID: 337
		private static Encoding _defaultEncoding = new UTF8Encoding(true);

		// Token: 0x0400015A RID: 346
		internal static readonly CsvConfiguration DefaultConfiguration = new CsvConfiguration();
	}
}
