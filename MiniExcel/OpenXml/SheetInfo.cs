using System;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x0200004C RID: 76
	public class SheetInfo
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0000AC73 File Offset: 0x00008E73
		public SheetInfo(uint id, uint index, string name, SheetState sheetState)
		{
			this.Id = id;
			this.Index = index;
			this.Name = name;
			this.State = sheetState;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000AC98 File Offset: 0x00008E98
		public uint Id { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000ACA0 File Offset: 0x00008EA0
		public uint Index { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000ACA8 File Offset: 0x00008EA8
		public string Name { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000ACB0 File Offset: 0x00008EB0
		public SheetState State { get; }
	}
}
