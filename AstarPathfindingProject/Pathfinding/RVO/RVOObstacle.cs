using System;

namespace Pathfinding.RVO
{
	// Token: 0x020002C8 RID: 712
	public abstract class RVOObstacle : VersionedMonoBehaviour
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060010FB RID: 4347
		protected abstract bool ExecuteInEditor { get; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060010FC RID: 4348
		protected abstract bool LocalCoordinates { get; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060010FD RID: 4349
		protected abstract bool StaticObstacle { get; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060010FE RID: 4350
		protected abstract float Height { get; }

		// Token: 0x04000CC9 RID: 3273
		public RVOObstacle.ObstacleVertexWinding obstacleMode;

		// Token: 0x04000CCA RID: 3274
		public RVOLayer layer = RVOLayer.DefaultObstacle;

		// Token: 0x020002C9 RID: 713
		public enum ObstacleVertexWinding
		{
			// Token: 0x04000CCC RID: 3276
			KeepOut,
			// Token: 0x04000CCD RID: 3277
			KeepIn
		}
	}
}
