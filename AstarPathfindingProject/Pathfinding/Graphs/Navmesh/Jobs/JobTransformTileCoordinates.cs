using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001ED RID: 493
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobTransformTileCoordinates : IJob
	{
		// Token: 0x06000C83 RID: 3203 RVA: 0x0004DF8C File Offset: 0x0004C18C
		public unsafe void Execute()
		{
			for (uint num = 0U; num < this.vertices.length; num += 1U)
			{
				Int3 @int = *this.vertices[num];
				*this.vertices[num] = (Int3)this.matrix.MultiplyPoint3x4(new Vector3((float)@int.x, (float)@int.y, (float)@int.z));
			}
		}

		// Token: 0x04000922 RID: 2338
		public UnsafeSpan<Int3> vertices;

		// Token: 0x04000923 RID: 2339
		public Matrix4x4 matrix;
	}
}
