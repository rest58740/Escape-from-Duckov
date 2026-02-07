using System;
using Unity.Burst;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001D8 RID: 472
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobBuildCompactField : IJob
	{
		// Token: 0x06000C5E RID: 3166 RVA: 0x0004ACF9 File Offset: 0x00048EF9
		public void Execute()
		{
			this.output.BuildFromLinkedField(this.input);
		}

		// Token: 0x0400089F RID: 2207
		public LinkedVoxelField input;

		// Token: 0x040008A0 RID: 2208
		public CompactVoxelField output;
	}
}
