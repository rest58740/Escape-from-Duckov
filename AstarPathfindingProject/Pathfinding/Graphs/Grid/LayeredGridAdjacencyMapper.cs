using System;
using Unity.Collections;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x020001F4 RID: 500
	public struct LayeredGridAdjacencyMapper : GridAdjacencyMapper
	{
		// Token: 0x06000C96 RID: 3222 RVA: 0x0004E885 File Offset: 0x0004CA85
		public int LayerCount(IntBounds bounds)
		{
			return bounds.size.y;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0004E893 File Offset: 0x0004CA93
		public int GetNeighbourIndex(int nodeIndexXZ, int nodeIndex, int direction, NativeArray<ulong> nodeConnections, NativeArray<int> neighbourOffsets, int layerStride)
		{
			return nodeIndexXZ + neighbourOffsets[direction] + (int)(nodeConnections[nodeIndex] >> 4 * direction & 15UL) * layerStride;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0004E8B7 File Offset: 0x0004CAB7
		public bool HasConnection(int nodeIndex, int direction, NativeArray<ulong> nodeConnections)
		{
			return (nodeConnections[nodeIndex] >> 4 * direction & 15UL) != 15UL;
		}
	}
}
