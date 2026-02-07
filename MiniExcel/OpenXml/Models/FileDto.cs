using System;

namespace MiniExcelLibs.OpenXml.Models
{
	// Token: 0x0200005C RID: 92
	internal class FileDto
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000307 RID: 775 RVA: 0x000123BE File Offset: 0x000105BE
		// (set) Token: 0x06000308 RID: 776 RVA: 0x000123C6 File Offset: 0x000105C6
		internal string ID { get; set; } = string.Format("R{0:N}", Guid.NewGuid());

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000309 RID: 777 RVA: 0x000123CF File Offset: 0x000105CF
		// (set) Token: 0x0600030A RID: 778 RVA: 0x000123D7 File Offset: 0x000105D7
		internal string Extension { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600030B RID: 779 RVA: 0x000123E0 File Offset: 0x000105E0
		internal string Path
		{
			get
			{
				return "xl/media/" + this.ID + "." + this.Extension;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600030C RID: 780 RVA: 0x000123FD File Offset: 0x000105FD
		internal string Path2
		{
			get
			{
				return "/xl/media/" + this.ID + "." + this.Extension;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0001241A File Offset: 0x0001061A
		// (set) Token: 0x0600030E RID: 782 RVA: 0x00012422 File Offset: 0x00010622
		internal byte[] Byte { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0001242B File Offset: 0x0001062B
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00012433 File Offset: 0x00010633
		internal int RowIndex { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0001243C File Offset: 0x0001063C
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00012444 File Offset: 0x00010644
		internal int CellIndex { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0001244D File Offset: 0x0001064D
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00012455 File Offset: 0x00010655
		internal bool IsImage { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0001245E File Offset: 0x0001065E
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00012466 File Offset: 0x00010666
		internal int SheetId { get; set; }
	}
}
