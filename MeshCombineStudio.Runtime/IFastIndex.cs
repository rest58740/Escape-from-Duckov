using System;

namespace MeshCombineStudio
{
	// Token: 0x02000027 RID: 39
	public interface IFastIndex
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000B7 RID: 183
		// (set) Token: 0x060000B8 RID: 184
		IFastIndexList List { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000B9 RID: 185
		// (set) Token: 0x060000BA RID: 186
		int ListIndex { get; set; }
	}
}
