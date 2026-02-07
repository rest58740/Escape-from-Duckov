using System;

namespace Pathfinding
{
	// Token: 0x0200003A RID: 58
	[Flags]
	public enum GraphUpdateThreading
	{
		// Token: 0x04000179 RID: 377
		UnityThread = 0,
		// Token: 0x0400017A RID: 378
		UnityInit = 2,
		// Token: 0x0400017B RID: 379
		UnityPost = 4
	}
}
