using System;

namespace Pathfinding
{
	// Token: 0x020000E6 RID: 230
	public interface INavmeshHolder : ITransformedGraph
	{
		// Token: 0x06000792 RID: 1938
		void GetNodes(Action<GraphNode> del);

		// Token: 0x06000793 RID: 1939
		Int3 GetVertex(int i);

		// Token: 0x06000794 RID: 1940
		Int3 GetVertexInGraphSpace(int i);

		// Token: 0x06000795 RID: 1941
		int GetVertexArrayIndex(int index);

		// Token: 0x06000796 RID: 1942
		void GetTileCoordinates(int tileIndex, out int x, out int z);
	}
}
