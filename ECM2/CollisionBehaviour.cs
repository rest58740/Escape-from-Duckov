using System;

namespace ECM2
{
	// Token: 0x02000008 RID: 8
	[Flags]
	public enum CollisionBehaviour
	{
		// Token: 0x04000062 RID: 98
		Default = 0,
		// Token: 0x04000063 RID: 99
		Walkable = 1,
		// Token: 0x04000064 RID: 100
		NotWalkable = 2,
		// Token: 0x04000065 RID: 101
		CanPerchOn = 4,
		// Token: 0x04000066 RID: 102
		CanNotPerchOn = 8,
		// Token: 0x04000067 RID: 103
		CanStepOn = 16,
		// Token: 0x04000068 RID: 104
		CanNotStepOn = 32,
		// Token: 0x04000069 RID: 105
		CanRideOn = 64,
		// Token: 0x0400006A RID: 106
		CanNotRideOn = 128
	}
}
