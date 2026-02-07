using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001DA RID: 474
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobErodeWalkableArea : IJob
	{
		// Token: 0x06000C60 RID: 3168 RVA: 0x0004AF20 File Offset: 0x00049120
		public void Execute()
		{
			NativeArray<ushort> output = new NativeArray<ushort>(this.field.spans.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			VoxelUtilityBurst.CalculateDistanceField(this.field, output);
			for (int i = 0; i < output.Length; i++)
			{
				if ((int)output[i] < this.radius * 2)
				{
					this.field.areaTypes[i] = 0;
				}
			}
		}

		// Token: 0x040008A4 RID: 2212
		public CompactVoxelField field;

		// Token: 0x040008A5 RID: 2213
		public int radius;
	}
}
