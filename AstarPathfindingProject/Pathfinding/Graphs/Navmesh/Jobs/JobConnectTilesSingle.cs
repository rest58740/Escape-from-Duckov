using System;
using System.Runtime.InteropServices;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001EA RID: 490
	internal struct JobConnectTilesSingle : IJob
	{
		// Token: 0x06000C80 RID: 3200 RVA: 0x0004DC5C File Offset: 0x0004BE5C
		public void Execute()
		{
			NavmeshTile[] array = (NavmeshTile[])this.tiles.Target;
			NavmeshBase.ConnectTiles(array[this.tileIndex1], array[this.tileIndex2], this.tileWorldSize.x, this.tileWorldSize.y, this.maxTileConnectionEdgeDistance);
		}

		// Token: 0x04000912 RID: 2322
		public GCHandle tiles;

		// Token: 0x04000913 RID: 2323
		public int tileIndex1;

		// Token: 0x04000914 RID: 2324
		public int tileIndex2;

		// Token: 0x04000915 RID: 2325
		public Vector2 tileWorldSize;

		// Token: 0x04000916 RID: 2326
		public float maxTileConnectionEdgeDistance;
	}
}
