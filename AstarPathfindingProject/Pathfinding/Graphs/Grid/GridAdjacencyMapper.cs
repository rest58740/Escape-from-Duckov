using System;
using Unity.Collections;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x020001F2 RID: 498
	public interface GridAdjacencyMapper
	{
		// Token: 0x06000C90 RID: 3216
		int LayerCount(IntBounds bounds);

		// Token: 0x06000C91 RID: 3217
		int GetNeighbourIndex(int nodeIndexXZ, int nodeIndex, int direction, NativeArray<ulong> nodeConnections, NativeArray<int> neighbourOffsets, int layerStride);

		// Token: 0x06000C92 RID: 3218
		bool HasConnection(int nodeIndex, int direction, NativeArray<ulong> nodeConnections);
	}
}
