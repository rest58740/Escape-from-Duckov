using System;

namespace MeshCombineStudio
{
	// Token: 0x02000028 RID: 40
	public class FastIndex : IFastIndex
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000BB RID: 187 RVA: 0x0000818F File Offset: 0x0000638F
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00008197 File Offset: 0x00006397
		public IFastIndexList List { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000081A0 File Offset: 0x000063A0
		// (set) Token: 0x060000BE RID: 190 RVA: 0x000081A8 File Offset: 0x000063A8
		public int ListIndex { get; set; }

		// Token: 0x060000BF RID: 191 RVA: 0x000081B1 File Offset: 0x000063B1
		public FastIndex()
		{
			this.ListIndex = -1;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000081C0 File Offset: 0x000063C0
		public void RemoveFromList()
		{
			if (this.List != null)
			{
				this.List.Remove(this);
			}
		}
	}
}
