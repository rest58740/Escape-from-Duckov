using System;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh.Jobs;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001A1 RID: 417
	public struct RecastBuilder
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x0004000F File Offset: 0x0003E20F
		public static TileBuilder BuildTileMeshes(RecastGraph graph, TileLayout tileLayout, IntRect tileRect)
		{
			return new TileBuilder(graph, tileLayout, tileRect);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00040019 File Offset: 0x0003E219
		public static JobBuildNodes BuildNodeTiles(RecastGraph graph, TileLayout tileLayout)
		{
			return new JobBuildNodes(graph, tileLayout);
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00040022 File Offset: 0x0003E222
		public static TileCutter CutTiles(NavmeshBase graph, GridLookup<NavmeshClipper> cuts, TileLayout tileLayout)
		{
			return new TileCutter(graph, cuts, tileLayout);
		}
	}
}
