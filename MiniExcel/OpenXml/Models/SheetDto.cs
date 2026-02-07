using System;

namespace MiniExcelLibs.OpenXml.Models
{
	// Token: 0x0200005D RID: 93
	internal class SheetDto
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00012491 File Offset: 0x00010691
		// (set) Token: 0x06000319 RID: 793 RVA: 0x00012499 File Offset: 0x00010699
		internal string ID { get; set; } = string.Format("R{0:N}", Guid.NewGuid());

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600031A RID: 794 RVA: 0x000124A2 File Offset: 0x000106A2
		// (set) Token: 0x0600031B RID: 795 RVA: 0x000124AA File Offset: 0x000106AA
		internal string Name { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600031C RID: 796 RVA: 0x000124B3 File Offset: 0x000106B3
		// (set) Token: 0x0600031D RID: 797 RVA: 0x000124BB File Offset: 0x000106BB
		internal int SheetIdx { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600031E RID: 798 RVA: 0x000124C4 File Offset: 0x000106C4
		internal string Path
		{
			get
			{
				return string.Format("xl/worksheets/sheet{0}.xml", this.SheetIdx);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000124DB File Offset: 0x000106DB
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000124E3 File Offset: 0x000106E3
		internal string State { get; set; }
	}
}
