using System;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x0200004E RID: 78
	internal sealed class SheetRecord
	{
		// Token: 0x06000260 RID: 608 RVA: 0x0000ACB8 File Offset: 0x00008EB8
		public SheetRecord(string name, string state, uint id, string rid)
		{
			this.Name = name;
			this.State = state;
			this.Id = id;
			this.Rid = rid;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000ACDD File Offset: 0x00008EDD
		public string Name { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000ACE5 File Offset: 0x00008EE5
		// (set) Token: 0x06000263 RID: 611 RVA: 0x0000ACED File Offset: 0x00008EED
		public string State { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000ACF6 File Offset: 0x00008EF6
		public uint Id { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000ACFE File Offset: 0x00008EFE
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000AD06 File Offset: 0x00008F06
		public string Rid { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000AD0F File Offset: 0x00008F0F
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000AD17 File Offset: 0x00008F17
		public string Path { get; set; }

		// Token: 0x06000269 RID: 617 RVA: 0x0000AD20 File Offset: 0x00008F20
		public SheetInfo ToSheetInfo(uint index)
		{
			if (string.IsNullOrEmpty(this.State))
			{
				return new SheetInfo(this.Id, index, this.Name, SheetState.Visible);
			}
			SheetState sheetState;
			if (Enum.TryParse<SheetState>(this.State, true, out sheetState))
			{
				return new SheetInfo(this.Id, index, this.Name, sheetState);
			}
			throw new ArgumentException("Unable to parse sheet state. Sheet name: " + this.Name);
		}
	}
}
