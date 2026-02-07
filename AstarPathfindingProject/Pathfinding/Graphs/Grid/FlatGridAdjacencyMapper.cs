using System;
using Unity.Collections;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x020001F3 RID: 499
	public struct FlatGridAdjacencyMapper : GridAdjacencyMapper
	{
		// Token: 0x06000C93 RID: 3219 RVA: 0x0001797A File Offset: 0x00015B7A
		public int LayerCount(IntBounds bounds)
		{
			return 1;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0004E863 File Offset: 0x0004CA63
		public int GetNeighbourIndex(int nodeIndexXZ, int nodeIndex, int direction, NativeArray<ulong> nodeConnections, NativeArray<int> neighbourOffsets, int layerStride)
		{
			return nodeIndex + neighbourOffsets[direction];
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0004E86F File Offset: 0x0004CA6F
		public bool HasConnection(int nodeIndex, int direction, NativeArray<ulong> nodeConnections)
		{
			return (nodeConnections[nodeIndex] >> direction & 1UL) > 0UL;
		}
	}
}
