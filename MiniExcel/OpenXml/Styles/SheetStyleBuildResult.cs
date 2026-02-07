using System;
using System.Collections.Generic;

namespace MiniExcelLibs.OpenXml.Styles
{
	// Token: 0x02000059 RID: 89
	internal class SheetStyleBuildResult
	{
		// Token: 0x060002E8 RID: 744 RVA: 0x00012297 File Offset: 0x00010497
		public SheetStyleBuildResult(Dictionary<string, string> cellXfIdMap)
		{
			this.CellXfIdMap = cellXfIdMap;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x000122A6 File Offset: 0x000104A6
		// (set) Token: 0x060002EA RID: 746 RVA: 0x000122AE File Offset: 0x000104AE
		public Dictionary<string, string> CellXfIdMap { get; set; }
	}
}
