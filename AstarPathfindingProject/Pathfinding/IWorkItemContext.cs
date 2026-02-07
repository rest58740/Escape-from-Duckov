using System;

namespace Pathfinding
{
	// Token: 0x0200008F RID: 143
	public interface IWorkItemContext : IGraphUpdateContext
	{
		// Token: 0x0600045B RID: 1115
		[Obsolete("You no longer need to call this method. Connectivity data is automatically kept up-to-date.")]
		void QueueFloodFill();

		// Token: 0x0600045C RID: 1116
		void EnsureValidFloodFill();

		// Token: 0x0600045D RID: 1117
		void PreUpdate();

		// Token: 0x0600045E RID: 1118
		void SetGraphDirty(NavGraph graph);
	}
}
