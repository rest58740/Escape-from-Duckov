using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001BE RID: 446
	public struct TileLayout
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0004596B File Offset: 0x00043B6B
		public float CellHeight
		{
			get
			{
				return Mathf.Max(this.graphSpaceSize.y / 64000f, 0.001f);
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00045988 File Offset: 0x00043B88
		public Vector2 TileWorldSize
		{
			get
			{
				return new Vector2(this.TileWorldSizeX, this.TileWorldSizeZ);
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x0004599B File Offset: 0x00043B9B
		public float TileWorldSizeX
		{
			get
			{
				return (float)this.tileSizeInVoxels.x * this.cellSize;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x000459B0 File Offset: 0x00043BB0
		public float TileWorldSizeZ
		{
			get
			{
				return (float)this.tileSizeInVoxels.y * this.cellSize;
			}
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x000459C8 File Offset: 0x00043BC8
		public Bounds GetTileBoundsInGraphSpace(int x, int z, int width = 1, int depth = 1)
		{
			Bounds result = default(Bounds);
			result.SetMinMax(new Vector3((float)x * this.TileWorldSizeX, 0f, (float)z * this.TileWorldSizeZ), new Vector3((float)(x + width) * this.TileWorldSizeX, this.graphSpaceSize.y, (float)(z + depth) * this.TileWorldSizeZ));
			return result;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00045A28 File Offset: 0x00043C28
		public IntRect GetTouchingTiles(Bounds bounds, float margin = 0f)
		{
			bounds = this.transform.InverseTransform(bounds);
			return new IntRect(Mathf.FloorToInt((bounds.min.x - margin) / this.TileWorldSizeX), Mathf.FloorToInt((bounds.min.z - margin) / this.TileWorldSizeZ), Mathf.FloorToInt((bounds.max.x + margin) / this.TileWorldSizeX), Mathf.FloorToInt((bounds.max.z + margin) / this.TileWorldSizeZ));
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00045AB0 File Offset: 0x00043CB0
		public IntRect GetTouchingTilesInGraphSpace(Rect rect)
		{
			return IntRect.Intersection(new IntRect(Mathf.FloorToInt(rect.xMin / this.TileWorldSizeX), Mathf.FloorToInt(rect.yMin / this.TileWorldSizeZ), Mathf.FloorToInt(rect.xMax / this.TileWorldSizeX), Mathf.FloorToInt(rect.yMax / this.TileWorldSizeZ)), new IntRect(0, 0, this.tileCount.x - 1, this.tileCount.y - 1));
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00045B34 File Offset: 0x00043D34
		public TileLayout(RecastGraph graph)
		{
			this = new TileLayout(new Bounds(graph.forcedBoundsCenter, graph.forcedBoundsSize), Quaternion.Euler(graph.rotation), graph.cellSize, graph.editorTileSize, graph.useTiles);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00045B6A File Offset: 0x00043D6A
		public TileLayout(NavMeshGraph graph)
		{
			this = new TileLayout(new Bounds(graph.transform.Transform(graph.forcedBoundsSize * 0.5f), graph.forcedBoundsSize), Quaternion.Euler(graph.rotation), 0.001f, 0, false);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00045BAC File Offset: 0x00043DAC
		public TileLayout(Bounds bounds, Quaternion rotation, float cellSize, int tileSizeInVoxels, bool useTiles)
		{
			this.transform = RecastGraph.CalculateTransform(bounds, rotation);
			this.cellSize = cellSize;
			Vector3 size = bounds.size;
			this.graphSpaceSize = size;
			int num = (int)(size.x / cellSize + 0.5f);
			int num2 = (int)(size.z / cellSize + 0.5f);
			if (!useTiles)
			{
				this.tileSizeInVoxels = new Vector2Int(num, num2);
				this.tileCount = new Vector2Int(1, 1);
			}
			else
			{
				this.tileSizeInVoxels = new Vector2Int(tileSizeInVoxels, tileSizeInVoxels);
				this.tileCount = new Vector2Int(Mathf.Max(0, (num + this.tileSizeInVoxels.x - 1) / this.tileSizeInVoxels.x), Mathf.Max(0, (num2 + this.tileSizeInVoxels.y - 1) / this.tileSizeInVoxels.y));
			}
			if (this.tileCount.x * this.tileCount.y > 524288)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Too many tiles (",
					(this.tileCount.x * this.tileCount.y).ToString(),
					") maximum is ",
					524288.ToString(),
					"\nTry disabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* inspector."
				}));
			}
		}

		// Token: 0x0400082F RID: 2095
		public Vector2Int tileCount;

		// Token: 0x04000830 RID: 2096
		public GraphTransform transform;

		// Token: 0x04000831 RID: 2097
		public Vector2Int tileSizeInVoxels;

		// Token: 0x04000832 RID: 2098
		public Vector3 graphSpaceSize;

		// Token: 0x04000833 RID: 2099
		public float cellSize;
	}
}
