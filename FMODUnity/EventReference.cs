using System;
using FMOD;

namespace FMODUnity
{
	// Token: 0x02000102 RID: 258
	[Serializable]
	public struct EventReference
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x00007554 File Offset: 0x00005754
		public override string ToString()
		{
			return this.Guid.ToString();
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x00007567 File Offset: 0x00005767
		public bool IsNull
		{
			get
			{
				return this.Guid.IsNull;
			}
		}

		// Token: 0x04000565 RID: 1381
		public GUID Guid;
	}
}
