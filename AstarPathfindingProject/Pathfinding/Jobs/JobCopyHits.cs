using System;
using Pathfinding.Graphs.Grid;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x02000178 RID: 376
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobCopyHits : IJob, GridIterationUtilities.ISliceAction
	{
		// Token: 0x06000ACD RID: 2765 RVA: 0x0003D1D0 File Offset: 0x0003B3D0
		public void Execute()
		{
			this.slice.AssertMatchesOuter<Vector3>(this.points);
			this.slice.AssertMatchesOuter<float4>(this.normals);
			GridIterationUtilities.ForEachCellIn3DSlice<JobCopyHits>(this.slice, ref this);
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0003D200 File Offset: 0x0003B400
		public void Execute(uint outerIdx, uint innerIdx)
		{
			Aliasing.ExpectNotAliased<NativeArray<Vector3>, NativeArray<float4>>(this.points, this.normals);
			Vector3 normal = this.hits[(int)innerIdx].normal;
			float4 x = new float4(normal.x, normal.y, normal.z, 0f);
			this.normals[(int)outerIdx] = math.normalizesafe(x, default(float4));
			if (math.lengthsq(x) > 1.1754944E-38f)
			{
				this.points[(int)outerIdx] = this.hits[(int)innerIdx].point;
			}
		}

		// Token: 0x04000746 RID: 1862
		[ReadOnly]
		public NativeArray<RaycastHit> hits;

		// Token: 0x04000747 RID: 1863
		[WriteOnly]
		public NativeArray<Vector3> points;

		// Token: 0x04000748 RID: 1864
		[WriteOnly]
		public NativeArray<float4> normals;

		// Token: 0x04000749 RID: 1865
		public Slice3D slice;
	}
}
