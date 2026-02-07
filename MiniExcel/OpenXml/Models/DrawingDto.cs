using System;

namespace MiniExcelLibs.OpenXml.Models
{
	// Token: 0x0200005B RID: 91
	internal class DrawingDto
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0001238B File Offset: 0x0001058B
		// (set) Token: 0x06000305 RID: 773 RVA: 0x00012393 File Offset: 0x00010593
		internal string ID { get; set; } = string.Format("R{0:N}", Guid.NewGuid());
	}
}
