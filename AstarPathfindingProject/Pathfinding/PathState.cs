using System;

namespace Pathfinding
{
	// Token: 0x0200003F RID: 63
	public enum PathState
	{
		// Token: 0x0400019E RID: 414
		Created,
		// Token: 0x0400019F RID: 415
		PathQueue,
		// Token: 0x040001A0 RID: 416
		Processing,
		// Token: 0x040001A1 RID: 417
		ReturnQueue,
		// Token: 0x040001A2 RID: 418
		Returning,
		// Token: 0x040001A3 RID: 419
		Returned
	}
}
