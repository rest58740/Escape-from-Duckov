using System;
using Pathfinding.Clipper2Lib;
using Unity.Jobs.LowLevel.Unsafe;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001B2 RID: 434
	internal static class TileHandlerCache
	{
		// Token: 0x0400080D RID: 2061
		internal static Clipper64[] cachedClippers = new Clipper64[JobsUtility.ThreadIndexCount];
	}
}
