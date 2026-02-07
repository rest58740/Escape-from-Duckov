using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001DB RID: 475
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobBuildDistanceField : IJob
	{
		// Token: 0x06000C61 RID: 3169 RVA: 0x0004AF88 File Offset: 0x00049188
		public void Execute()
		{
			NativeArray<ushort> src = new NativeArray<ushort>(this.field.spans.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			VoxelUtilityBurst.CalculateDistanceField(this.field, src);
			this.output.ResizeUninitialized(this.field.spans.Length);
			VoxelUtilityBurst.BoxBlur(this.field, src, this.output.AsArray());
		}

		// Token: 0x040008A6 RID: 2214
		public CompactVoxelField field;

		// Token: 0x040008A7 RID: 2215
		public NativeList<ushort> output;
	}
}
