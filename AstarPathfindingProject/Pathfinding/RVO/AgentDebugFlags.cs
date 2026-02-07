using System;

namespace Pathfinding.RVO
{
	// Token: 0x020002AD RID: 685
	[Flags]
	public enum AgentDebugFlags : byte
	{
		// Token: 0x04000C04 RID: 3076
		Nothing = 0,
		// Token: 0x04000C05 RID: 3077
		ObstacleVOs = 1,
		// Token: 0x04000C06 RID: 3078
		AgentVOs = 2,
		// Token: 0x04000C07 RID: 3079
		ReachedState = 4,
		// Token: 0x04000C08 RID: 3080
		DesiredVelocity = 8,
		// Token: 0x04000C09 RID: 3081
		ChosenVelocity = 16,
		// Token: 0x04000C0A RID: 3082
		Obstacles = 32,
		// Token: 0x04000C0B RID: 3083
		ForwardClearance = 64
	}
}
