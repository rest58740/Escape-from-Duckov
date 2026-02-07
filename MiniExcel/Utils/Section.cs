using System;
using System.Collections.Generic;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000030 RID: 48
	internal class Section
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000055FC File Offset: 0x000037FC
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00005604 File Offset: 0x00003804
		public int SectionIndex { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000560D File Offset: 0x0000380D
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00005615 File Offset: 0x00003815
		public SectionType Type { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000561E File Offset: 0x0000381E
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00005626 File Offset: 0x00003826
		public List<string> GeneralTextDateDurationParts { get; set; }
	}
}
