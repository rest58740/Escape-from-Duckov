using System;
using System.Collections.Generic;

namespace MiniExcelLibs.Utils
{
	// Token: 0x0200002D RID: 45
	internal class ExcelNumberFormat
	{
		// Token: 0x06000136 RID: 310 RVA: 0x00005444 File Offset: 0x00003644
		public ExcelNumberFormat(string formatString)
		{
			bool flag;
			List<Section> list = Parser.ParseSections(formatString, out flag);
			this.IsValid = !flag;
			this.FormatString = formatString;
			if (this.IsValid)
			{
				this.Sections = list;
				this.IsDateTimeFormat = (Evaluator.GetFirstSection(this.Sections, SectionType.Date) != null);
				this.IsTimeSpanFormat = (Evaluator.GetFirstSection(this.Sections, SectionType.Duration) != null);
				return;
			}
			this.Sections = new List<Section>();
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000054B6 File Offset: 0x000036B6
		public bool IsValid { get; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000138 RID: 312 RVA: 0x000054BE File Offset: 0x000036BE
		public string FormatString { get; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000054C6 File Offset: 0x000036C6
		public bool IsDateTimeFormat { get; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600013A RID: 314 RVA: 0x000054CE File Offset: 0x000036CE
		public bool IsTimeSpanFormat { get; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600013B RID: 315 RVA: 0x000054D6 File Offset: 0x000036D6
		internal List<Section> Sections { get; }
	}
}
