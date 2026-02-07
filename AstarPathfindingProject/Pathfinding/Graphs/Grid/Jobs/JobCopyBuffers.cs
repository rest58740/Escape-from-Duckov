using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000217 RID: 535
	[BurstCompile]
	public struct JobCopyBuffers : IJob
	{
		// Token: 0x06000D08 RID: 3336 RVA: 0x00051EA4 File Offset: 0x000500A4
		public void Execute()
		{
			Slice3D inputSlice = new Slice3D(this.input.bounds, this.bounds);
			Slice3D outputSlice = new Slice3D(this.output.bounds, this.bounds);
			JobCopyRectangle<Vector3>.Copy(this.input.positions, this.output.positions, inputSlice, outputSlice);
			JobCopyRectangle<float4>.Copy(this.input.normals, this.output.normals, inputSlice, outputSlice);
			JobCopyRectangle<ulong>.Copy(this.input.connections, this.output.connections, inputSlice, outputSlice);
			if (this.copyPenaltyAndTags)
			{
				JobCopyRectangle<uint>.Copy(this.input.penalties, this.output.penalties, inputSlice, outputSlice);
				JobCopyRectangle<int>.Copy(this.input.tags, this.output.tags, inputSlice, outputSlice);
			}
			JobCopyRectangle<bool>.Copy(this.input.walkable, this.output.walkable, inputSlice, outputSlice);
			JobCopyRectangle<bool>.Copy(this.input.walkableWithErosion, this.output.walkableWithErosion, inputSlice, outputSlice);
		}

		// Token: 0x040009C8 RID: 2504
		[ReadOnly]
		[DisableUninitializedReadCheck]
		public GridGraphNodeData input;

		// Token: 0x040009C9 RID: 2505
		[WriteOnly]
		public GridGraphNodeData output;

		// Token: 0x040009CA RID: 2506
		public IntBounds bounds;

		// Token: 0x040009CB RID: 2507
		public bool copyPenaltyAndTags;
	}
}
