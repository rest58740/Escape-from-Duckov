using System;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x02000042 RID: 66
	internal class NumberFormatString
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000A11E File Offset: 0x0000831E
		public string FormatCode { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000A126 File Offset: 0x00008326
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0000A12E File Offset: 0x0000832E
		public Type Type { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000A137 File Offset: 0x00008337
		public bool NeedConvertToString { get; }

		// Token: 0x060001F6 RID: 502 RVA: 0x0000A13F File Offset: 0x0000833F
		public NumberFormatString(string formatCode, Type type, bool needConvertToString = false)
		{
			this.FormatCode = formatCode;
			this.Type = type;
			this.NeedConvertToString = needConvertToString;
		}
	}
}
