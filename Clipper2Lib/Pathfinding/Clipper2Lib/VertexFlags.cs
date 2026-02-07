using System;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000014 RID: 20
	[Flags]
	internal enum VertexFlags
	{
		// Token: 0x04000031 RID: 49
		None = 0,
		// Token: 0x04000032 RID: 50
		OpenStart = 1,
		// Token: 0x04000033 RID: 51
		OpenEnd = 2,
		// Token: 0x04000034 RID: 52
		LocalMax = 4,
		// Token: 0x04000035 RID: 53
		LocalMin = 8
	}
}
