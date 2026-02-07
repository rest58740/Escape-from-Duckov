using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001E7 RID: 487
	[BurstCompile]
	public struct JobCalculateTriangleConnections : IJob
	{
		// Token: 0x06000C7B RID: 3195 RVA: 0x0004D758 File Offset: 0x0004B958
		public unsafe void Execute()
		{
			NativeParallelHashMap<int2, uint> nativeParallelHashMap = new NativeParallelHashMap<int2, uint>(128, Allocator.Temp);
			bool flag = false;
			for (int i = 0; i < this.tileMeshes.Length; i++)
			{
				nativeParallelHashMap.Clear();
				TileMesh.TileMeshUnsafe tileMeshUnsafe = this.tileMeshes[i];
				int length = tileMeshUnsafe.triangles.Length;
				UnsafeAppendBuffer neighbours = new UnsafeAppendBuffer(length * 2 * 4, 4, Allocator.Persistent);
				UnsafeAppendBuffer neighbourCounts = new UnsafeAppendBuffer(length * 4, 4, Allocator.Persistent);
				int* ptr = tileMeshUnsafe.triangles.ptr;
				int j = 0;
				int num = 0;
				while (j < length)
				{
					flag |= !nativeParallelHashMap.TryAdd(new int2(ptr[j], ptr[j + 1]), (uint)(num | 0));
					flag |= !nativeParallelHashMap.TryAdd(new int2(ptr[j + 1], ptr[j + 2]), (uint)(num | 268435456));
					flag |= !nativeParallelHashMap.TryAdd(new int2(ptr[j + 2], ptr[j]), (uint)(num | 536870912));
					j += 3;
					num++;
				}
				for (int k = 0; k < length; k += 3)
				{
					int num2 = 0;
					for (int l = 0; l < 3; l++)
					{
						uint num3;
						if (nativeParallelHashMap.TryGetValue(new int2(ptr[k + (l + 1) % 3], ptr[k + l]), out num3))
						{
							uint value = num3 & 268435455U;
							int num4 = (int)(num3 >> 28);
							neighbours.Add<uint>(value);
							byte value2 = Connection.PackShapeEdgeInfo((byte)l, (byte)num4, true, true, true);
							neighbours.Add<int>((int)value2);
							num2++;
						}
					}
					neighbourCounts.Add<int>(num2);
				}
				this.nodeConnections[i] = new JobCalculateTriangleConnections.TileNodeConnectionsUnsafe
				{
					neighbours = neighbours,
					neighbourCounts = neighbourCounts
				};
			}
			if (flag)
			{
				Debug.LogWarning("Duplicate triangle edges were found in the input mesh. These have been removed. Are you sure your mesh is suitable for being used as a navmesh directly?\nThis could be caused by the mesh's normals not being consistent.");
			}
		}

		// Token: 0x04000905 RID: 2309
		[ReadOnly]
		public NativeArray<TileMesh.TileMeshUnsafe> tileMeshes;

		// Token: 0x04000906 RID: 2310
		[WriteOnly]
		public NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe> nodeConnections;

		// Token: 0x020001E8 RID: 488
		public struct TileNodeConnectionsUnsafe
		{
			// Token: 0x04000907 RID: 2311
			public UnsafeAppendBuffer neighbours;

			// Token: 0x04000908 RID: 2312
			public UnsafeAppendBuffer neighbourCounts;
		}
	}
}
