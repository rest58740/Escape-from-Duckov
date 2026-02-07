using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Graphs.Navmesh.Jobs;
using Pathfinding.Jobs;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000F9 RID: 249
	[JsonOptIn]
	[Preserve]
	public class RecastGraph : NavmeshBase, IUpdatableGraph
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0002B3C7 File Offset: 0x000295C7
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x0002B3D4 File Offset: 0x000295D4
		[Obsolete("Use collectionSettings.rasterizeColliders instead")]
		public bool rasterizeColliders
		{
			get
			{
				return this.collectionSettings.rasterizeColliders;
			}
			set
			{
				this.collectionSettings.rasterizeColliders = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0002B3E2 File Offset: 0x000295E2
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x0002B3EF File Offset: 0x000295EF
		[Obsolete("Use collectionSettings.rasterizeMeshes instead")]
		public bool rasterizeMeshes
		{
			get
			{
				return this.collectionSettings.rasterizeMeshes;
			}
			set
			{
				this.collectionSettings.rasterizeMeshes = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0002B3FD File Offset: 0x000295FD
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0002B40A File Offset: 0x0002960A
		[Obsolete("Use collectionSettings.rasterizeTerrain instead")]
		public bool rasterizeTerrain
		{
			get
			{
				return this.collectionSettings.rasterizeTerrain;
			}
			set
			{
				this.collectionSettings.rasterizeTerrain = value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0002B418 File Offset: 0x00029618
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0002B425 File Offset: 0x00029625
		[Obsolete("Use collectionSettings.rasterizeTrees instead")]
		public bool rasterizeTrees
		{
			get
			{
				return this.collectionSettings.rasterizeTrees;
			}
			set
			{
				this.collectionSettings.rasterizeTrees = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0002B433 File Offset: 0x00029633
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0002B440 File Offset: 0x00029640
		[Obsolete("Use collectionSettings.colliderRasterizeDetail instead")]
		public float colliderRasterizeDetail
		{
			get
			{
				return this.collectionSettings.colliderRasterizeDetail;
			}
			set
			{
				this.collectionSettings.colliderRasterizeDetail = value;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0002B44E File Offset: 0x0002964E
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x0002B45B File Offset: 0x0002965B
		[Obsolete("Use collectionSettings.layerMask instead")]
		public LayerMask mask
		{
			get
			{
				return this.collectionSettings.layerMask;
			}
			set
			{
				this.collectionSettings.layerMask = value;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x0002B469 File Offset: 0x00029669
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x0002B476 File Offset: 0x00029676
		[Obsolete("Use collectionSettings.tagMask instead")]
		public List<string> tagMask
		{
			get
			{
				return this.collectionSettings.tagMask;
			}
			set
			{
				this.collectionSettings.tagMask = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x0002B484 File Offset: 0x00029684
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x0002B491 File Offset: 0x00029691
		[Obsolete("Use collectionSettings.terrainHeightmapDownsamplingFactor instead")]
		public int terrainSampleSize
		{
			get
			{
				return this.collectionSettings.terrainHeightmapDownsamplingFactor;
			}
			set
			{
				this.collectionSettings.terrainHeightmapDownsamplingFactor = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x0002B49F File Offset: 0x0002969F
		public override float NavmeshCuttingCharacterRadius
		{
			get
			{
				return this.characterRadius;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0001797A File Offset: 0x00015B7A
		public override bool RecalculateNormals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0002B4A7 File Offset: 0x000296A7
		public override float TileWorldSizeX
		{
			get
			{
				return (float)this.tileSizeX * this.cellSize;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0002B4B7 File Offset: 0x000296B7
		public override float TileWorldSizeZ
		{
			get
			{
				return (float)this.tileSizeZ * this.cellSize;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0002B4C7 File Offset: 0x000296C7
		public override float MaxTileConnectionEdgeDistance
		{
			get
			{
				return this.walkableClimb;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0002B4D0 File Offset: 0x000296D0
		public override Bounds bounds
		{
			get
			{
				float4x4 float4x = this.CalculateTransform().matrix;
				Bounds result = new ToWorldMatrix(new float3x3(float4x.c0.xyz, float4x.c1.xyz, float4x.c2.xyz)).ToWorld(new Bounds(Vector3.zero, this.forcedBoundsSize));
				result.center += this.forcedBoundsCenter;
				return result;
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0002B550 File Offset: 0x00029750
		public override bool IsInsideBounds(Vector3 point)
		{
			if (this.tiles == null || this.tiles.Length == 0)
			{
				return false;
			}
			float3 @float = this.transform.InverseTransform(point);
			if (this.dimensionMode == RecastGraph.DimensionMode.Dimension2D)
			{
				return @float.x >= 0f && @float.z >= 0f && @float.x <= this.forcedBoundsSize.x && @float.z <= this.forcedBoundsSize.z;
			}
			return math.all(@float >= 0f) && math.all(@float <= this.forcedBoundsSize);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002B5FA File Offset: 0x000297FA
		[Obsolete("Use SnapBoundsToScene instead")]
		public void SnapForceBoundsToScene()
		{
			this.SnapBoundsToScene();
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0002B604 File Offset: 0x00029804
		public void SnapBoundsToScene()
		{
			DisposeArena disposeArena = new DisposeArena();
			RecastMeshGatherer.MeshCollection data = new TileBuilder(this, new TileLayout(this), default(IntRect)).CollectMeshes(new Bounds(Vector3.zero, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)));
			if (data.meshes.Length > 0)
			{
				ToWorldMatrix toWorldMatrix = new ToWorldMatrix(new float3x3(Quaternion.Inverse(Quaternion.Euler(this.rotation))));
				Bounds bounds = toWorldMatrix.ToWorld(data.meshes[0].bounds);
				for (int i = 1; i < data.meshes.Length; i++)
				{
					bounds.Encapsulate(toWorldMatrix.ToWorld(data.meshes[i].bounds));
				}
				bounds.max += Vector3.up * (this.walkableHeight * 1.1f);
				bounds.min -= Vector3.up * 0.01f;
				this.forcedBoundsCenter = Quaternion.Euler(this.rotation) * bounds.center;
				this.forcedBoundsSize = Vector3.Max(bounds.size, Vector3.one * 0.01f);
			}
			disposeArena.Add<RecastMeshGatherer.MeshCollection>(data);
			disposeArena.DisposeAll();
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0002B771 File Offset: 0x00029971
		IGraphUpdatePromise IUpdatableGraph.ScheduleGraphUpdates(List<GraphUpdateObject> graphUpdates)
		{
			return new RecastGraph.RecastGraphUpdatePromise(this, graphUpdates);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002B77A File Offset: 0x0002997A
		public IGraphUpdatePromise TranslateInDirection(int dx, int dz)
		{
			return new RecastGraph.RecastMovePromise(this, new Vector2Int(dx, dz));
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0002B789 File Offset: 0x00029989
		protected override IGraphUpdatePromise ScanInternal(bool async)
		{
			return new RecastGraph.RecastGraphScanPromise
			{
				graph = this
			};
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0002B797 File Offset: 0x00029997
		public override GraphTransform CalculateTransform()
		{
			return RecastGraph.CalculateTransform(new Bounds(this.forcedBoundsCenter, this.forcedBoundsSize), Quaternion.Euler(this.rotation));
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0002B7BA File Offset: 0x000299BA
		public static GraphTransform CalculateTransform(Bounds bounds, Quaternion rotation)
		{
			return new GraphTransform(Matrix4x4.TRS(bounds.center, rotation, Vector3.one) * Matrix4x4.TRS(-bounds.extents, Quaternion.identity, Vector3.one));
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0002B7F4 File Offset: 0x000299F4
		protected void SetLayout(TileLayout info)
		{
			this.tileXCount = info.tileCount.x;
			this.tileZCount = info.tileCount.y;
			this.tileSizeX = info.tileSizeInVoxels.x;
			this.tileSizeZ = info.tileSizeInVoxels.y;
			this.transform = info.transform;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x0002B855 File Offset: 0x00029A55
		internal int CharacterRadiusInVoxels
		{
			get
			{
				return Mathf.CeilToInt(this.characterRadius / this.cellSize - 0.1f);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0002B86F File Offset: 0x00029A6F
		internal int TileBorderSizeInVoxels
		{
			get
			{
				return this.CharacterRadiusInVoxels + 3;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x0002B879 File Offset: 0x00029A79
		internal float TileBorderSizeInWorldUnits
		{
			get
			{
				return (float)this.TileBorderSizeInVoxels * this.cellSize;
			}
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0002B88C File Offset: 0x00029A8C
		public unsafe virtual void Resize(IntRect newTileBounds)
		{
			base.AssertSafeToUpdateGraph();
			if (!newTileBounds.IsValid())
			{
				throw new ArgumentException("Invalid tile bounds");
			}
			if (newTileBounds == new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1))
			{
				return;
			}
			if (newTileBounds.Area == 0)
			{
				throw new ArgumentException("Tile count must at least 1x1");
			}
			if (!this.useTiles)
			{
				throw new InvalidOperationException("Cannot resize graph when tiles are not enabled");
			}
			base.StartBatchTileUpdate(true);
			NavmeshTile[] array = new NavmeshTile[newTileBounds.Area];
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					if (newTileBounds.Contains(j, i))
					{
						NavmeshTile navmeshTile = this.tiles[j + i * this.tileXCount];
						array[j - newTileBounds.xmin + (i - newTileBounds.ymin) * newTileBounds.Width] = navmeshTile;
					}
					else
					{
						base.ClearTile(j, i, null);
						base.DirtyBounds(base.GetTileBounds(j, i, 1, 1));
					}
				}
			}
			this.forcedBoundsSize = new Vector3((float)newTileBounds.Width * this.TileWorldSizeX, this.forcedBoundsSize.y, (float)newTileBounds.Height * this.TileWorldSizeZ);
			this.forcedBoundsCenter = this.transform.Transform(new Vector3((float)(newTileBounds.xmin + newTileBounds.xmax + 1) * 0.5f * this.TileWorldSizeX, this.forcedBoundsSize.y * 0.5f, (float)(newTileBounds.ymin + newTileBounds.ymax + 1) * 0.5f * this.TileWorldSizeZ));
			this.transform = this.CalculateTransform();
			Int3 rhs = -(Int3)new Vector3(this.TileWorldSizeX * (float)newTileBounds.xmin, 0f, this.TileWorldSizeZ * (float)newTileBounds.ymin);
			for (int k = 0; k < newTileBounds.Height; k++)
			{
				for (int l = 0; l < newTileBounds.Width; l++)
				{
					int num = l + k * newTileBounds.Width;
					NavmeshTile navmeshTile2 = array[num];
					if (navmeshTile2 == null)
					{
						array[num] = base.NewEmptyTile(l, k);
					}
					else
					{
						navmeshTile2.x = l;
						navmeshTile2.z = k;
						for (int m = 0; m < navmeshTile2.nodes.Length; m++)
						{
							TriangleMeshNode triangleMeshNode = navmeshTile2.nodes[m];
							triangleMeshNode.v0 = ((triangleMeshNode.v0 & 4095) | num << 12);
							triangleMeshNode.v1 = ((triangleMeshNode.v1 & 4095) | num << 12);
							triangleMeshNode.v2 = ((triangleMeshNode.v2 & 4095) | num << 12);
						}
						for (int n = 0; n < navmeshTile2.vertsInGraphSpace.Length; n++)
						{
							*navmeshTile2.vertsInGraphSpace[n] += rhs;
						}
						navmeshTile2.vertsInGraphSpace.CopyTo(navmeshTile2.verts);
						this.transform.Transform(navmeshTile2.verts);
						navmeshTile2.bbTree.Dispose();
						navmeshTile2.bbTree = new BBTree(navmeshTile2.tris, navmeshTile2.vertsInGraphSpace);
					}
				}
			}
			this.tiles = array;
			this.tileXCount = newTileBounds.Width;
			this.tileZCount = newTileBounds.Height;
			base.EndBatchTileUpdate();
			this.navmeshUpdateData.OnResized(newTileBounds, new TileLayout(this));
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0002BBF5 File Offset: 0x00029DF5
		public void EnsureInitialized()
		{
			base.AssertSafeToUpdateGraph();
			if (this.tiles == null)
			{
				TriangleMeshNode.SetNavmeshHolder(AstarPath.active.data.GetGraphIndex(this), this);
				this.SetLayout(new TileLayout(this));
				base.FillWithEmptyTiles();
			}
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0002BC30 File Offset: 0x00029E30
		public void ReplaceTiles(TileMeshes tileMeshes, float yOffset = 0f)
		{
			base.AssertSafeToUpdateGraph();
			this.EnsureInitialized();
			if (tileMeshes.tileWorldSize.x != this.TileWorldSizeX || tileMeshes.tileWorldSize.y != this.TileWorldSizeZ)
			{
				string[] array = new string[7];
				array[0] = "Loaded tile size does not match this graph's tile size.\nThe source tiles have a world-space tile size of ";
				int num = 1;
				Vector2 tileWorldSize = tileMeshes.tileWorldSize;
				array[num] = tileWorldSize.ToString();
				array[2] = " while this graph's tile size is (";
				array[3] = this.TileWorldSizeX.ToString();
				array[4] = ",";
				array[5] = this.TileWorldSizeZ.ToString();
				array[6] = ").\nFor a recast graph, the world-space tile size is defined as the cell size * the tile size in voxels";
				throw new Exception(string.Concat(array));
			}
			int width = tileMeshes.tileRect.Width;
			int height = tileMeshes.tileRect.Height;
			IntRect newTileBounds = IntRect.Union(new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1), tileMeshes.tileRect);
			this.Resize(newTileBounds);
			tileMeshes.tileRect = tileMeshes.tileRect.Offset(-newTileBounds.Min);
			base.StartBatchTileUpdate(false);
			NavmeshTile[] array2 = new NavmeshTile[width * height];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					TileMesh tileMesh = tileMeshes.tileMeshes[j + i * width];
					Int3 rhs = (Int3)new Vector3(0f, yOffset, 0f);
					for (int k = 0; k < tileMesh.verticesInTileSpace.Length; k++)
					{
						tileMesh.verticesInTileSpace[k] += rhs;
					}
					Vector2Int vector2Int = new Vector2Int(j, i) + tileMeshes.tileRect.Min;
					base.ReplaceTile(vector2Int.x, vector2Int.y, tileMesh.verticesInTileSpace, tileMesh.triangles, null, true);
					array2[j + i * width] = base.GetTile(vector2Int.x, vector2Int.y);
				}
			}
			base.EndBatchTileUpdate();
			if (this.OnRecalculatedTiles != null)
			{
				this.OnRecalculatedTiles(array2);
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002BE58 File Offset: 0x0002A058
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			base.PostDeserialization(ctx);
			if (ctx.meta.version < AstarSerializer.V4_3_80)
			{
				this.collectionSettings.colliderRasterizeDetail = 2f * this.cellSize * this.collectionSettings.colliderRasterizeDetail * this.collectionSettings.colliderRasterizeDetail / 9.869605f;
			}
			if (ctx.meta.version < AstarSerializer.V5_1_0)
			{
				if (this.collectionSettings.tagMask.Count > 0 && this.collectionSettings.layerMask != -1)
				{
					Debug.LogError("In version 5.1.0 or higher of the A* Pathfinding Project you can no longer include objects both using a tag mask and a layer mask. Please choose in the recast graph inspector which one you want to use.");
					return;
				}
				if (this.collectionSettings.tagMask.Count > 0)
				{
					this.collectionSettings.collectionMode = RecastGraph.CollectionSettings.FilterMode.Tags;
				}
			}
		}

		// Token: 0x0400052D RID: 1325
		[JsonMember]
		public float characterRadius = 0.5f;

		// Token: 0x0400052E RID: 1326
		[JsonMember]
		public float contourMaxError = 2f;

		// Token: 0x0400052F RID: 1327
		[JsonMember]
		public float cellSize = 0.25f;

		// Token: 0x04000530 RID: 1328
		[JsonMember]
		public float walkableHeight = 2f;

		// Token: 0x04000531 RID: 1329
		[JsonMember]
		public float walkableClimb = 0.5f;

		// Token: 0x04000532 RID: 1330
		[JsonMember]
		public float maxSlope = 30f;

		// Token: 0x04000533 RID: 1331
		[JsonMember]
		public float maxEdgeLength = 20f;

		// Token: 0x04000534 RID: 1332
		[JsonMember]
		public float minRegionSize = 3f;

		// Token: 0x04000535 RID: 1333
		[JsonMember]
		public int editorTileSize = 128;

		// Token: 0x04000536 RID: 1334
		[JsonMember]
		public int tileSizeX = 128;

		// Token: 0x04000537 RID: 1335
		[JsonMember]
		public int tileSizeZ = 128;

		// Token: 0x04000538 RID: 1336
		[JsonMember]
		public bool useTiles = true;

		// Token: 0x04000539 RID: 1337
		public bool scanEmptyGraph;

		// Token: 0x0400053A RID: 1338
		[JsonMember]
		public List<RecastGraph.PerLayerModification> perLayerModifications = new List<RecastGraph.PerLayerModification>();

		// Token: 0x0400053B RID: 1339
		[JsonMember]
		public RecastGraph.DimensionMode dimensionMode = RecastGraph.DimensionMode.Dimension3D;

		// Token: 0x0400053C RID: 1340
		[JsonMember]
		public RecastGraph.BackgroundTraversability backgroundTraversability;

		// Token: 0x0400053D RID: 1341
		[JsonMember]
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x0400053E RID: 1342
		[JsonMember]
		public RecastGraph.CollectionSettings collectionSettings = new RecastGraph.CollectionSettings();

		// Token: 0x0400053F RID: 1343
		[JsonMember]
		public Vector3 rotation;

		// Token: 0x04000540 RID: 1344
		[JsonMember]
		public Vector3 forcedBoundsCenter;

		// Token: 0x04000541 RID: 1345
		private DisposeArena pendingGraphUpdateArena = new DisposeArena();

		// Token: 0x04000542 RID: 1346
		private bool hasExtendedInX;

		// Token: 0x04000543 RID: 1347
		private bool hasExtendedInZ;

		// Token: 0x020000FA RID: 250
		public enum RelevantGraphSurfaceMode
		{
			// Token: 0x04000545 RID: 1349
			DoNotRequire,
			// Token: 0x04000546 RID: 1350
			OnlyForCompletelyInsideTile,
			// Token: 0x04000547 RID: 1351
			RequireForAll
		}

		// Token: 0x020000FB RID: 251
		public enum DimensionMode
		{
			// Token: 0x04000549 RID: 1353
			Dimension2D,
			// Token: 0x0400054A RID: 1354
			Dimension3D
		}

		// Token: 0x020000FC RID: 252
		public enum BackgroundTraversability
		{
			// Token: 0x0400054C RID: 1356
			Walkable,
			// Token: 0x0400054D RID: 1357
			Unwalkable
		}

		// Token: 0x020000FD RID: 253
		[Serializable]
		public struct PerLayerModification
		{
			// Token: 0x1700015A RID: 346
			// (get) Token: 0x06000852 RID: 2130 RVA: 0x0002BFDC File Offset: 0x0002A1DC
			public static RecastGraph.PerLayerModification Default
			{
				get
				{
					return new RecastGraph.PerLayerModification
					{
						layer = 0,
						mode = RecastNavmeshModifier.Mode.WalkableSurface,
						surfaceID = 1
					};
				}
			}

			// Token: 0x06000853 RID: 2131 RVA: 0x0002C00C File Offset: 0x0002A20C
			public static RecastGraph.PerLayerModification[] ToLayerLookup(List<RecastGraph.PerLayerModification> perLayerModifications, RecastGraph.PerLayerModification defaultValue)
			{
				RecastGraph.PerLayerModification[] array = new RecastGraph.PerLayerModification[32];
				int num = 0;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = defaultValue;
					array[i].layer = i;
				}
				for (int j = 0; j < perLayerModifications.Count; j++)
				{
					if (perLayerModifications[j].layer < 0 || perLayerModifications[j].layer >= 32)
					{
						Debug.LogError("Layer " + perLayerModifications[j].layer.ToString() + " is out of range. Layers must be in the range [0...31]");
					}
					else if ((num & 1 << perLayerModifications[j].layer) != 0)
					{
						Debug.LogError("Several per layer modifications refer to the same layer '" + LayerMask.LayerToName(perLayerModifications[j].layer) + "'");
					}
					else
					{
						num |= 1 << perLayerModifications[j].layer;
						array[perLayerModifications[j].layer] = perLayerModifications[j];
					}
				}
				return array;
			}

			// Token: 0x0400054E RID: 1358
			public int layer;

			// Token: 0x0400054F RID: 1359
			public RecastNavmeshModifier.Mode mode;

			// Token: 0x04000550 RID: 1360
			public int surfaceID;
		}

		// Token: 0x020000FE RID: 254
		[Serializable]
		public class CollectionSettings
		{
			// Token: 0x04000551 RID: 1361
			public RecastGraph.CollectionSettings.FilterMode collectionMode;

			// Token: 0x04000552 RID: 1362
			[NonSerialized]
			public PhysicsScene? physicsScene;

			// Token: 0x04000553 RID: 1363
			[NonSerialized]
			public PhysicsScene2D? physicsScene2D;

			// Token: 0x04000554 RID: 1364
			public LayerMask layerMask = -1;

			// Token: 0x04000555 RID: 1365
			public List<string> tagMask = new List<string>();

			// Token: 0x04000556 RID: 1366
			public bool rasterizeColliders = true;

			// Token: 0x04000557 RID: 1367
			public bool rasterizeMeshes;

			// Token: 0x04000558 RID: 1368
			public bool rasterizeTerrain = true;

			// Token: 0x04000559 RID: 1369
			public bool rasterizeTrees = true;

			// Token: 0x0400055A RID: 1370
			public int terrainHeightmapDownsamplingFactor = 3;

			// Token: 0x0400055B RID: 1371
			public float colliderRasterizeDetail = 1f;

			// Token: 0x0400055C RID: 1372
			public Action<RecastMeshGatherer> onCollectMeshes;

			// Token: 0x020000FF RID: 255
			public enum FilterMode
			{
				// Token: 0x0400055E RID: 1374
				Layers,
				// Token: 0x0400055F RID: 1375
				Tags
			}
		}

		// Token: 0x02000100 RID: 256
		private class RecastGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x06000855 RID: 2133 RVA: 0x0002C168 File Offset: 0x0002A368
			public RecastGraphUpdatePromise(RecastGraph graph, List<GraphUpdateObject> graphUpdates)
			{
				this.promises = ListPool<ValueTuple<Promise<TileBuilder.TileBuilderOutput>, Promise<TileCutter.TileCutterOutput>, Promise<JobBuildNodes.BuildNodeTilesOutput>>>.Claim();
				this.graph = graph;
				this.graphHash = RecastGraph.RecastGraphUpdatePromise.HashSettings(graph);
				List<ValueTuple<IntRect, GraphUpdateObject>> list = ListPool<ValueTuple<IntRect, GraphUpdateObject>>.Claim();
				for (int i = graphUpdates.Count - 1; i >= 0; i--)
				{
					GraphUpdateObject graphUpdateObject = graphUpdates[i];
					if (graphUpdateObject.updatePhysics)
					{
						graphUpdates.RemoveAt(i);
						IntRect touchingTiles = graph.GetTouchingTiles(graphUpdateObject.bounds, graph.TileBorderSizeInWorldUnits);
						if (touchingTiles.IsValid())
						{
							list.Add(new ValueTuple<IntRect, GraphUpdateObject>(touchingTiles, graphUpdateObject));
						}
					}
				}
				this.graphUpdates = graphUpdates;
				if (list.Count > 1)
				{
					list.Sort((ValueTuple<IntRect, GraphUpdateObject> a, ValueTuple<IntRect, GraphUpdateObject> b) => b.Item1.Area.CompareTo(a.Item1.Area));
				}
				int j = 0;
				while (j < list.Count)
				{
					IntRect item = list[j].Item1;
					if (list.Count <= 1)
					{
						goto IL_136;
					}
					bool flag = false;
					for (int k = item.ymin; k <= item.ymax; k++)
					{
						for (int l = item.xmin; l <= item.xmax; l++)
						{
							NavmeshTile tile = graph.GetTile(l, k);
							flag |= !tile.flag;
							tile.flag = true;
						}
					}
					if (flag)
					{
						goto IL_136;
					}
					IL_1AC:
					j++;
					continue;
					IL_136:
					TileLayout tileLayout = new TileLayout(graph);
					Promise<TileBuilder.TileBuilderOutput> promise = RecastBuilder.BuildTileMeshes(graph, tileLayout, item).Schedule(graph.pendingGraphUpdateArena);
					Promise<TileCutter.TileCutterOutput> promise2 = RecastBuilder.CutTiles(graph, graph.navmeshUpdateData.clipperLookup, tileLayout).Schedule(promise);
					Promise<JobBuildNodes.BuildNodeTilesOutput> item2 = RecastBuilder.BuildNodeTiles(graph, tileLayout).Schedule(graph.pendingGraphUpdateArena, promise, promise2);
					this.promises.Add(new ValueTuple<Promise<TileBuilder.TileBuilderOutput>, Promise<TileCutter.TileCutterOutput>, Promise<JobBuildNodes.BuildNodeTilesOutput>>(promise, promise2, item2));
					goto IL_1AC;
				}
				if (list.Count > 1)
				{
					for (int m = 0; m < list.Count; m++)
					{
						IntRect item3 = list[m].Item1;
						for (int n = item3.ymin; n <= item3.ymax; n++)
						{
							for (int num = item3.xmin; num <= item3.xmax; num++)
							{
								graph.GetTile(num, n).flag = false;
							}
						}
					}
				}
				ListPool<ValueTuple<IntRect, GraphUpdateObject>>.Release(ref list);
			}

			// Token: 0x06000856 RID: 2134 RVA: 0x0002C3B0 File Offset: 0x0002A5B0
			public IEnumerator<JobHandle> Prepare()
			{
				int num;
				for (int i = 0; i < this.promises.Count; i = num + 1)
				{
					yield return this.promises[i].Item2.handle;
					yield return this.promises[i].Item1.handle;
					num = i;
				}
				yield break;
			}

			// Token: 0x06000857 RID: 2135 RVA: 0x0002C3C0 File Offset: 0x0002A5C0
			private static int HashSettings(RecastGraph graph)
			{
				return (graph.tileXCount * 31 ^ graph.tileZCount) * 31 ^ graph.TileWorldSizeX.GetHashCode() * 31 ^ graph.TileWorldSizeZ.GetHashCode();
			}

			// Token: 0x06000858 RID: 2136 RVA: 0x0002C404 File Offset: 0x0002A604
			public void Apply(IGraphUpdateContext ctx)
			{
				if (RecastGraph.RecastGraphUpdatePromise.HashSettings(this.graph) != this.graphHash)
				{
					throw new InvalidOperationException("Recast graph changed while a graph update was in progress. This is not allowed. Use AstarPath.active.AddWorkItem if you need to update graphs.");
				}
				for (int i = 0; i < this.promises.Count; i++)
				{
					Promise<TileBuilder.TileBuilderOutput> item = this.promises[i].Item1;
					Promise<TileCutter.TileCutterOutput> item2 = this.promises[i].Item2;
					Promise<JobBuildNodes.BuildNodeTilesOutput> item3 = this.promises[i].Item3;
					JobBuildNodes.BuildNodeTilesOutput buildNodeTilesOutput = item3.Complete();
					IntRect tileRect = buildNodeTilesOutput.progressSource.tileMeshes.tileRect;
					NavmeshTile[] tiles = buildNodeTilesOutput.tiles;
					item.Dispose();
					item2.Dispose();
					item3.Dispose();
					for (int j = 0; j < tiles.Length; j++)
					{
						AstarPath active = AstarPath.active;
						GraphNode[] nodes = tiles[j].nodes;
						active.InitializeNodes(nodes);
					}
					this.graph.StartBatchTileUpdate(true);
					for (int k = 0; k < tileRect.Height; k++)
					{
						for (int l = 0; l < tileRect.Width; l++)
						{
							NavmeshTile navmeshTile = tiles[k * tileRect.Width + l];
							navmeshTile.graph = this.graph;
							this.graph.ClearTile(l + tileRect.xmin, k + tileRect.ymin, navmeshTile);
						}
					}
					this.graph.EndBatchTileUpdate();
					GCHandle tilesHandle = GCHandle.Alloc(this.graph.tiles);
					IntRect tileRect2 = new IntRect(0, 0, this.graph.tileXCount - 1, this.graph.tileZCount - 1);
					JobConnectTiles.ScheduleRecalculateBorders(tilesHandle, default(JobHandle), tileRect2, tileRect, new Vector2(this.graph.TileWorldSizeX, this.graph.TileWorldSizeZ), this.graph.MaxTileConnectionEdgeDistance).Complete();
					tilesHandle.Free();
					if (this.graph.OnRecalculatedTiles != null)
					{
						this.graph.OnRecalculatedTiles(tiles);
					}
					ctx.DirtyBounds(this.graph.GetTileBounds(tileRect));
				}
				this.graph.pendingGraphUpdateArena.DisposeAll();
				if (this.graphUpdates != null)
				{
					for (int m = 0; m < this.graphUpdates.Count; m++)
					{
						GraphUpdateObject graphUpdateObject = this.graphUpdates[m];
						IntRect touchingTiles = this.graph.GetTouchingTiles(graphUpdateObject.bounds, this.graph.TileBorderSizeInWorldUnits);
						if (touchingTiles.IsValid())
						{
							for (int n = touchingTiles.ymin; n <= touchingTiles.ymax; n++)
							{
								for (int num = touchingTiles.xmin; num <= touchingTiles.xmax; num++)
								{
									NavmeshTile navmeshTile2 = this.graph.tiles[n * this.graph.tileXCount + num];
									NavMeshGraph.UpdateArea(graphUpdateObject, navmeshTile2);
								}
							}
							ctx.DirtyBounds(this.graph.GetTileBounds(touchingTiles));
						}
					}
				}
			}

			// Token: 0x04000560 RID: 1376
			public List<ValueTuple<Promise<TileBuilder.TileBuilderOutput>, Promise<TileCutter.TileCutterOutput>, Promise<JobBuildNodes.BuildNodeTilesOutput>>> promises;

			// Token: 0x04000561 RID: 1377
			public List<GraphUpdateObject> graphUpdates;

			// Token: 0x04000562 RID: 1378
			public RecastGraph graph;

			// Token: 0x04000563 RID: 1379
			private int graphHash;
		}

		// Token: 0x02000103 RID: 259
		private class RecastGraphScanPromise : IGraphUpdatePromise
		{
			// Token: 0x1700015D RID: 349
			// (get) Token: 0x06000862 RID: 2146 RVA: 0x0002C821 File Offset: 0x0002AA21
			public float Progress
			{
				get
				{
					if (this.progressSource == null)
					{
						return 1f;
					}
					return this.progressSource.Progress;
				}
			}

			// Token: 0x06000863 RID: 2147 RVA: 0x0002C83C File Offset: 0x0002AA3C
			public IEnumerator<JobHandle> Prepare()
			{
				TriangleMeshNode.SetNavmeshHolder(AstarPath.active.data.GetGraphIndex(this.graph), this.graph);
				if (!Application.isPlaying)
				{
					RelevantGraphSurface.FindAllGraphSurfaces();
				}
				RelevantGraphSurface.UpdateAllPositions();
				this.tileLayout = new TileLayout(this.graph);
				if (this.graph.scanEmptyGraph || this.tileLayout.tileCount.x * this.tileLayout.tileCount.y <= 0)
				{
					this.emptyGraph = true;
					yield break;
				}
				DisposeArena arena = new DisposeArena();
				IntRect tileRect = new IntRect(0, 0, this.tileLayout.tileCount.x - 1, this.tileLayout.tileCount.y - 1);
				Promise<TileBuilder.TileBuilderOutput> tileMeshesPromise = RecastBuilder.BuildTileMeshes(this.graph, this.tileLayout, tileRect).Schedule(arena);
				this.cutSettings = new NavmeshUpdates.NavmeshUpdateSettings(this.graph, this.tileLayout);
				Promise<TileCutter.TileCutterOutput> cutPromise = RecastBuilder.CutTiles(this.graph, this.cutSettings.clipperLookup, this.tileLayout).Schedule(tileMeshesPromise);
				this.cutSettings.DiscardPending();
				Promise<JobBuildNodes.BuildNodeTilesOutput> tilesPromise = RecastBuilder.BuildNodeTiles(this.graph, this.tileLayout).Schedule(arena, tileMeshesPromise, cutPromise);
				this.progressSource = tilesPromise;
				yield return tilesPromise.handle;
				this.progressSource = null;
				JobBuildNodes.BuildNodeTilesOutput buildNodeTilesOutput = tilesPromise.Complete();
				TileBuilder.TileBuilderOutput tileBuilderOutput = tileMeshesPromise.Complete();
				TileCutter.TileCutterOutput tileCutterOutput = cutPromise.Complete();
				this.tiles = buildNodeTilesOutput.tiles;
				tileBuilderOutput.Dispose();
				tileCutterOutput.Dispose();
				buildNodeTilesOutput.Dispose();
				arena.DisposeAll();
				yield break;
			}

			// Token: 0x06000864 RID: 2148 RVA: 0x0002C84C File Offset: 0x0002AA4C
			public void Apply(IGraphUpdateContext ctx)
			{
				this.graph.DestroyAllNodes();
				this.graph.hasExtendedInZ = false;
				this.graph.hasExtendedInX = false;
				this.cutSettings.AttachToGraph();
				if (this.emptyGraph)
				{
					this.graph.SetLayout(this.tileLayout);
					this.graph.FillWithEmptyTiles();
				}
				else
				{
					for (int i = 0; i < this.tiles.Length; i++)
					{
						AstarPath active = AstarPath.active;
						GraphNode[] nodes = this.tiles[i].nodes;
						active.InitializeNodes(nodes);
					}
					this.graph.SetLayout(this.tileLayout);
					this.graph.tiles = this.tiles;
					for (int j = 0; j < this.tiles.Length; j++)
					{
						this.tiles[j].graph = this.graph;
					}
				}
				if (this.graph.OnRecalculatedTiles != null)
				{
					this.graph.OnRecalculatedTiles(this.graph.tiles.Clone() as NavmeshTile[]);
				}
			}

			// Token: 0x0400056A RID: 1386
			public RecastGraph graph;

			// Token: 0x0400056B RID: 1387
			private TileLayout tileLayout;

			// Token: 0x0400056C RID: 1388
			private bool emptyGraph;

			// Token: 0x0400056D RID: 1389
			private NavmeshTile[] tiles;

			// Token: 0x0400056E RID: 1390
			private IProgress progressSource;

			// Token: 0x0400056F RID: 1391
			private NavmeshUpdates.NavmeshUpdateSettings cutSettings;
		}

		// Token: 0x02000105 RID: 261
		private class RecastMovePromise : IGraphUpdatePromise
		{
			// Token: 0x0600086C RID: 2156 RVA: 0x0002CB90 File Offset: 0x0002AD90
			public RecastMovePromise(RecastGraph graph, Vector2Int delta)
			{
				this.graph = graph;
				this.delta = delta;
				if (delta.x != 0 && delta.y != 0)
				{
					throw new ArgumentException("Only translation in a single direction is supported. delta.x == 0 || delta.y == 0 must hold.");
				}
			}

			// Token: 0x0600086D RID: 2157 RVA: 0x0002CBC3 File Offset: 0x0002ADC3
			public IEnumerator<JobHandle> Prepare()
			{
				if (this.delta.x == 0 && this.delta.y == 0)
				{
					yield break;
				}
				IntRect b = new IntRect(0, 0, this.graph.tileXCount - 1, this.graph.tileZCount - 1);
				this.newTileRect = b.Offset(this.delta);
				IntRect createdTiles = IntRect.Exclude(this.newTileRect, b);
				if (!this.graph.hasExtendedInX && this.delta.x != 0)
				{
					if (this.delta.x > 0)
					{
						createdTiles.xmin--;
					}
					this.graph.hasExtendedInX = true;
				}
				if (!this.graph.hasExtendedInZ && this.delta.y != 0)
				{
					if (this.delta.y > 0)
					{
						createdTiles.ymin--;
					}
					this.graph.hasExtendedInZ = true;
				}
				DisposeArena disposeArena = new DisposeArena();
				TileLayout tileLayout = new TileLayout(this.graph);
				tileLayout.graphSpaceSize.x = float.PositiveInfinity;
				tileLayout.graphSpaceSize.z = float.PositiveInfinity;
				Promise<TileBuilder.TileBuilderOutput> pendingPromise = RecastBuilder.BuildTileMeshes(this.graph, tileLayout, createdTiles).Schedule(disposeArena);
				yield return pendingPromise.handle;
				TileBuilder.TileBuilderOutput value = pendingPromise.GetValue();
				this.tileMeshes = value.tileMeshes.ToManaged();
				pendingPromise.Dispose();
				disposeArena.DisposeAll();
				this.tileMeshes.tileRect = createdTiles.Offset(-this.delta);
				yield break;
			}

			// Token: 0x0600086E RID: 2158 RVA: 0x0002CBD4 File Offset: 0x0002ADD4
			public void Apply(IGraphUpdateContext ctx)
			{
				if (this.delta.x == 0 && this.delta.y == 0)
				{
					return;
				}
				this.graph.Resize(this.newTileRect);
				this.graph.ReplaceTiles(this.tileMeshes, 0f);
			}

			// Token: 0x04000577 RID: 1399
			private RecastGraph graph;

			// Token: 0x04000578 RID: 1400
			private TileMeshes tileMeshes;

			// Token: 0x04000579 RID: 1401
			private Vector2Int delta;

			// Token: 0x0400057A RID: 1402
			private IntRect newTileRect;
		}
	}
}
