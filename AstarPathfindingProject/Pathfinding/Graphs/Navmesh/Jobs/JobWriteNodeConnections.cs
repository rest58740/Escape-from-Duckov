using System;
using System.Runtime.InteropServices;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001EE RID: 494
	public struct JobWriteNodeConnections : IJob
	{
		// Token: 0x06000C84 RID: 3204 RVA: 0x0004DFFC File Offset: 0x0004C1FC
		public void Execute()
		{
			NavmeshTile[] array = (NavmeshTile[])this.tiles.Target;
			for (int i = 0; i < array.Length; i++)
			{
				JobCalculateTriangleConnections.TileNodeConnectionsUnsafe connections = this.nodeConnections[i];
				this.Apply(array[i].nodes, connections);
				connections.neighbourCounts.Dispose();
				connections.neighbours.Dispose();
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0004E05C File Offset: 0x0004C25C
		private void Apply(TriangleMeshNode[] nodes, JobCalculateTriangleConnections.TileNodeConnectionsUnsafe connections)
		{
			UnsafeAppendBuffer.Reader reader = connections.neighbourCounts.AsReader();
			UnsafeAppendBuffer.Reader reader2 = connections.neighbours.AsReader();
			foreach (TriangleMeshNode triangleMeshNode in nodes)
			{
				int num = reader.ReadNext<int>();
				Connection[] array = triangleMeshNode.connections = ArrayPool<Connection>.ClaimWithExactLength(num);
				for (int j = 0; j < num; j++)
				{
					int num2 = reader2.ReadNext<int>();
					byte shapeEdgeInfo = (byte)reader2.ReadNext<int>();
					TriangleMeshNode triangleMeshNode2 = nodes[num2];
					int costMagnitude = (triangleMeshNode.position - triangleMeshNode2.position).costMagnitude;
					array[j] = new Connection(triangleMeshNode2, (uint)costMagnitude, shapeEdgeInfo);
				}
			}
		}

		// Token: 0x04000924 RID: 2340
		[ReadOnly]
		public NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe> nodeConnections;

		// Token: 0x04000925 RID: 2341
		public GCHandle tiles;
	}
}
