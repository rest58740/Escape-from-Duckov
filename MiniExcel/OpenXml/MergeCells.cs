using System;
using System.Collections.Generic;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x02000047 RID: 71
	internal class MergeCells
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000A52D File Offset: 0x0000872D
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000A535 File Offset: 0x00008735
		public Dictionary<string, object> MergesValues { get; set; } = new Dictionary<string, object>();

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000A53E File Offset: 0x0000873E
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000A546 File Offset: 0x00008746
		public Dictionary<string, string> MergesMap { get; set; } = new Dictionary<string, string>();
	}
}
