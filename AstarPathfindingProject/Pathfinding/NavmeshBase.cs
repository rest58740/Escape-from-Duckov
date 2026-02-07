using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000D8 RID: 216
	[BurstCompile]
	public abstract class NavmeshBase : NavGraph, INavmeshHolder, ITransformedGraph, IRaycastableGraph
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060006BC RID: 1724
		public abstract float NavmeshCuttingCharacterRadius { get; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060006BD RID: 1725
		public abstract float TileWorldSizeX { get; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060006BE RID: 1726
		public abstract float TileWorldSizeZ { get; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060006BF RID: 1727
		public abstract float MaxTileConnectionEdgeDistance { get; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x00022D8A File Offset: 0x00020F8A
		GraphTransform ITransformedGraph.transform
		{
			get
			{
				return this.transform;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060006C1 RID: 1729
		public abstract bool RecalculateNormals { get; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00022D92 File Offset: 0x00020F92
		public override bool isScanned
		{
			get
			{
				return this.tiles != null;
			}
		}

		// Token: 0x060006C3 RID: 1731
		public abstract GraphTransform CalculateTransform();

		// Token: 0x060006C4 RID: 1732 RVA: 0x00022D9D File Offset: 0x00020F9D
		public NavmeshTile GetTile(int x, int z)
		{
			return this.tiles[x + z * this.tileXCount];
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00022DB0 File Offset: 0x00020FB0
		public Int3 GetVertex(int index)
		{
			int num = index >> 12 & 524287;
			return this.tiles[num].GetVertex(index);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00022DD8 File Offset: 0x00020FD8
		public Int3 GetVertexInGraphSpace(int index)
		{
			int num = index >> 12 & 524287;
			return this.tiles[num].GetVertexInGraphSpace(index);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00022DFE File Offset: 0x00020FFE
		public static int GetTileIndex(int index)
		{
			return index >> 12 & 524287;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00022E0A File Offset: 0x0002100A
		public int GetVertexArrayIndex(int index)
		{
			return index & 4095;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00022E13 File Offset: 0x00021013
		public void GetTileCoordinates(int tileIndex, out int x, out int z)
		{
			z = tileIndex / this.tileXCount;
			x = tileIndex - z * this.tileXCount;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00022E2C File Offset: 0x0002102C
		public NavmeshTile[] GetTiles()
		{
			return this.tiles;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00022E34 File Offset: 0x00021034
		public Bounds GetTileBounds(IntRect rect)
		{
			return this.GetTileBounds(rect.xmin, rect.ymin, rect.Width, rect.Height);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00022E56 File Offset: 0x00021056
		public Bounds GetTileBounds(int x, int z, int width = 1, int depth = 1)
		{
			return this.transform.Transform(this.GetTileBoundsInGraphSpace(x, z, width, depth));
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00022E6E File Offset: 0x0002106E
		public Bounds GetTileBoundsInGraphSpace(IntRect rect)
		{
			return this.GetTileBoundsInGraphSpace(rect.xmin, rect.ymin, rect.Width, rect.Height);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00022E90 File Offset: 0x00021090
		public Bounds GetTileBoundsInGraphSpace(int x, int z, int width = 1, int depth = 1)
		{
			Bounds result = default(Bounds);
			result.SetMinMax(new Vector3((float)x * this.TileWorldSizeX, 0f, (float)z * this.TileWorldSizeZ), new Vector3((float)(x + width) * this.TileWorldSizeX, this.forcedBoundsSize.y, (float)(z + depth) * this.TileWorldSizeZ));
			return result;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00022EF0 File Offset: 0x000210F0
		public Vector2Int GetTileCoordinates(Vector3 position)
		{
			position = this.transform.InverseTransform(position);
			position.x /= this.TileWorldSizeX;
			position.z /= this.TileWorldSizeZ;
			return new Vector2Int((int)position.x, (int)position.z);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00022F40 File Offset: 0x00021140
		protected override void OnDestroy()
		{
			base.OnDestroy();
			TriangleMeshNode.ClearNavmeshHolder((int)this.graphIndex, this);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00022F54 File Offset: 0x00021154
		protected override void DisposeUnmanagedData()
		{
			base.DisposeUnmanagedData();
			this.navmeshUpdateData.Dispose();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00022F68 File Offset: 0x00021168
		protected override void DestroyAllNodes()
		{
			this.GetNodes(delegate(GraphNode node)
			{
				node.GetConnections(delegate(GraphNode other)
				{
					if (node.GraphIndex != other.GraphIndex)
					{
						other.RemovePartialConnection(node);
					}
				}, 32);
			});
			this.GetNodes(delegate(GraphNode node)
			{
				node.Destroy();
			});
			if (this.tiles != null)
			{
				for (int i = 0; i < this.tiles.Length; i++)
				{
					this.tiles[i].Dispose();
				}
				this.tiles = null;
			}
			this.navmeshUpdateData.Dispose();
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00022FF9 File Offset: 0x000211F9
		public override void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			throw new Exception("This method cannot be used for navmesh or recast graphs. Please use the other overload of RelocateNodes instead");
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00023008 File Offset: 0x00021208
		public void RelocateNodes(GraphTransform newTransform)
		{
			base.AssertSafeToUpdateGraph();
			base.DirtyBounds(this.bounds);
			this.transform = newTransform;
			if (this.tiles != null)
			{
				for (int i = 0; i < this.tiles.Length; i++)
				{
					NavmeshTile navmeshTile = this.tiles[i];
					if (navmeshTile != null)
					{
						navmeshTile.vertsInGraphSpace.CopyTo(navmeshTile.verts);
						this.transform.Transform(navmeshTile.verts);
						for (int j = 0; j < navmeshTile.nodes.Length; j++)
						{
							navmeshTile.nodes[j].UpdatePositionFromVertices();
						}
					}
				}
				base.DirtyBounds(this.bounds);
				this.navmeshUpdateData.UpdateLayoutFromGraph();
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000230B0 File Offset: 0x000212B0
		protected NavmeshTile NewEmptyTile(int x, int z)
		{
			return new NavmeshTile
			{
				x = x,
				z = z,
				w = 1,
				d = 1,
				verts = default(UnsafeSpan<Int3>),
				vertsInGraphSpace = default(UnsafeSpan<Int3>),
				tris = default(UnsafeSpan<int>),
				nodes = new TriangleMeshNode[0],
				bbTree = default(BBTree),
				graph = this
			};
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00023124 File Offset: 0x00021324
		public override void GetNodes(Action<GraphNode> action)
		{
			if (this.tiles == null)
			{
				return;
			}
			for (int i = 0; i < this.tiles.Length; i++)
			{
				if (this.tiles[i] != null && this.tiles[i].x + this.tiles[i].z * this.tileXCount == i)
				{
					TriangleMeshNode[] nodes = this.tiles[i].nodes;
					if (nodes != null)
					{
						for (int j = 0; j < nodes.Length; j++)
						{
							action(nodes[j]);
						}
					}
				}
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000231A4 File Offset: 0x000213A4
		public IntRect GetTouchingTiles(Bounds bounds, float margin = 0f)
		{
			bounds = this.transform.InverseTransform(bounds);
			return IntRect.Intersection(new IntRect(Mathf.FloorToInt((bounds.min.x - margin) / this.TileWorldSizeX), Mathf.FloorToInt((bounds.min.z - margin) / this.TileWorldSizeZ), Mathf.FloorToInt((bounds.max.x + margin) / this.TileWorldSizeX), Mathf.FloorToInt((bounds.max.z + margin) / this.TileWorldSizeZ)), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00023248 File Offset: 0x00021448
		public IntRect GetTouchingTilesInGraphSpace(Rect rect)
		{
			return IntRect.Intersection(new IntRect(Mathf.FloorToInt(rect.xMin / this.TileWorldSizeX), Mathf.FloorToInt(rect.yMin / this.TileWorldSizeZ), Mathf.FloorToInt(rect.xMax / this.TileWorldSizeX), Mathf.FloorToInt(rect.yMax / this.TileWorldSizeZ)), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000232C4 File Offset: 0x000214C4
		protected void ConnectTileWithNeighbours(NavmeshTile tile, bool onlyUnflagged = false)
		{
			if (tile.w != 1 || tile.d != 1)
			{
				throw new ArgumentException("Tile widths or depths other than 1 are not supported. The fields exist mainly for possible future expansions.");
			}
			for (int i = -1; i <= 1; i++)
			{
				int num = tile.z + i;
				if (num >= 0 && num < this.tileZCount)
				{
					for (int j = -1; j <= 1; j++)
					{
						int num2 = tile.x + j;
						if (num2 >= 0 && num2 < this.tileXCount && j == 0 != (i == 0))
						{
							NavmeshTile navmeshTile = this.tiles[num2 + num * this.tileXCount];
							if (!onlyUnflagged || !navmeshTile.flag)
							{
								NavmeshBase.ConnectTiles(navmeshTile, tile, this.TileWorldSizeX, this.TileWorldSizeZ, this.MaxTileConnectionEdgeDistance);
							}
						}
					}
				}
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00023378 File Offset: 0x00021578
		public override float NearestNodeDistanceSqrLowerBound(Vector3 position, NNConstraint constraint)
		{
			if (this.tiles == null)
			{
				return float.PositiveInfinity;
			}
			float3 p = this.transform.InverseTransform(position);
			BBTree.ProjectionParams projectionParams = new BBTree.ProjectionParams(constraint, this.transform);
			return projectionParams.SquaredRectPointDistanceOnPlane(new IntRect(0, 0, (int)((float)(1000 * this.tileXCount) * this.TileWorldSizeX), (int)((float)(1000 * this.tileZCount) * this.TileWorldSizeZ)), p);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x000233EC File Offset: 0x000215EC
		public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, float maxDistanceSqr)
		{
			if (this.tiles == null)
			{
				return NNInfo.Empty;
			}
			float3 @float = this.transform.InverseTransform(position);
			int num = (int)(@float.x / this.TileWorldSizeX);
			int num2 = (int)(@float.z / this.TileWorldSizeZ);
			num = Mathf.Clamp(num, 0, this.tileXCount - 1);
			num2 = Mathf.Clamp(num2, 0, this.tileZCount - 1);
			int num3 = Math.Max(this.tileXCount, this.tileZCount);
			NNInfo empty = NNInfo.Empty;
			float num4 = maxDistanceSqr;
			BBTree.ProjectionParams projectionParams = new BBTree.ProjectionParams(constraint, this.transform);
			float num5 = Math.Min(this.TileWorldSizeX, this.TileWorldSizeX);
			for (int i = 0; i < num3; i++)
			{
				int num6 = Math.Min(i + num2 + 1, this.tileZCount);
				for (int j = Math.Max(-i + num2, 0); j < num6; j++)
				{
					int num7 = Math.Abs(i - Math.Abs(j - num2));
					int num8 = num7;
					do
					{
						int num9 = -num8 + num;
						if (num9 >= 0 && num9 < this.tileXCount)
						{
							NavmeshTile navmeshTile = this.tiles[num9 + j * this.tileXCount];
							if (navmeshTile != null && navmeshTile.bbTree.DistanceSqrLowerBound(@float, projectionParams) <= num4)
							{
								NavmeshTile navmeshTile2 = navmeshTile;
								float3 p = @float;
								GraphNode[] nodes = navmeshTile.nodes;
								navmeshTile2.bbTree.QueryClosest(p, constraint, projectionParams, ref num4, ref empty, nodes, navmeshTile.tris, navmeshTile.vertsInGraphSpace);
							}
						}
						num8 = -num8;
					}
					while (num8 != num7);
				}
				int num10 = i + 1;
				float num11 = (float)math.max(0, num10 - 2) * num5;
				if (projectionParams.alignedWithXZPlane && num4 - 1E-05f <= num11 * num11)
				{
					break;
				}
			}
			if (empty.node != null)
			{
				empty = new NNInfo(empty.node, this.transform.Transform(empty.position), empty.distanceCostSqr);
			}
			return empty;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x000235D0 File Offset: 0x000217D0
		public override NNInfo RandomPointOnSurface(NNConstraint nnConstraint = null, bool highQuality = true)
		{
			if (highQuality)
			{
				return base.RandomPointOnSurface(nnConstraint, highQuality);
			}
			if (!this.isScanned || this.tiles.Length == 0)
			{
				return NNInfo.Empty;
			}
			int num = UnityEngine.Random.Range(0, this.tiles.Length);
			for (int i = 0; i < this.tiles.Length; i++)
			{
				NavmeshTile navmeshTile = this.tiles[(num + i) % this.tiles.Length];
				if (navmeshTile.nodes.Length != 0)
				{
					TriangleMeshNode triangleMeshNode = navmeshTile.nodes[UnityEngine.Random.Range(0, navmeshTile.nodes.Length)];
					if (nnConstraint == null || nnConstraint.Suitable(triangleMeshNode))
					{
						return new NNInfo(triangleMeshNode, triangleMeshNode.RandomPointOnSurface(), 0f);
					}
				}
			}
			return NNInfo.Empty;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00023678 File Offset: 0x00021878
		public GraphNode PointOnNavmesh(Vector3 position, NNConstraint constraint)
		{
			if (this.tiles == null)
			{
				return null;
			}
			constraint = (constraint ?? NNConstraint.None);
			DistanceMetric distanceMetric = constraint.distanceMetric;
			if (!constraint.distanceMetric.isProjectedDistance)
			{
				constraint.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
			}
			constraint.distanceMetric.distanceScaleAlongProjectionDirection = 0f;
			GraphNode node = this.GetNearest(position, constraint, 0f).node;
			constraint.distanceMetric = distanceMetric;
			return node;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x000236E4 File Offset: 0x000218E4
		protected void FillWithEmptyTiles()
		{
			this.tiles = new NavmeshTile[this.tileXCount * this.tileZCount];
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					this.tiles[i * this.tileXCount + j] = this.NewEmptyTile(j, i);
				}
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00023744 File Offset: 0x00021944
		protected static void CreateNodeConnections(TriangleMeshNode[] nodes, bool keepExistingConnections)
		{
			List<Connection> list = ListPool<Connection>.Claim();
			Dictionary<Vector2Int, int> dictionary = ObjectPoolSimple<Dictionary<Vector2Int, int>>.Claim();
			dictionary.Clear();
			for (int i = 0; i < nodes.Length; i++)
			{
				TriangleMeshNode triangleMeshNode = nodes[i];
				int vertexCount = triangleMeshNode.GetVertexCount();
				for (int j = 0; j < vertexCount; j++)
				{
					Vector2Int key = new Vector2Int(triangleMeshNode.GetVertexIndex(j), triangleMeshNode.GetVertexIndex((j + 1) % vertexCount));
					dictionary.TryAdd(key, i);
				}
			}
			foreach (TriangleMeshNode triangleMeshNode2 in nodes)
			{
				list.Clear();
				if (keepExistingConnections && triangleMeshNode2.connections != null)
				{
					list.AddRange(triangleMeshNode2.connections);
				}
				int vertexCount2 = triangleMeshNode2.GetVertexCount();
				for (int l = 0; l < vertexCount2; l++)
				{
					int vertexIndex = triangleMeshNode2.GetVertexIndex(l);
					int vertexIndex2 = triangleMeshNode2.GetVertexIndex((l + 1) % vertexCount2);
					int num;
					if (dictionary.TryGetValue(new Vector2Int(vertexIndex2, vertexIndex), out num))
					{
						TriangleMeshNode triangleMeshNode3 = nodes[num];
						int vertexCount3 = triangleMeshNode3.GetVertexCount();
						for (int m = 0; m < vertexCount3; m++)
						{
							if (triangleMeshNode3.GetVertexIndex(m) == vertexIndex2 && triangleMeshNode3.GetVertexIndex((m + 1) % vertexCount3) == vertexIndex)
							{
								list.Add(new Connection(triangleMeshNode3, (uint)(triangleMeshNode2.position - triangleMeshNode3.position).costMagnitude, Connection.PackShapeEdgeInfo((byte)l, (byte)m, true, true, true)));
								break;
							}
						}
					}
				}
				triangleMeshNode2.connections = list.ToArrayFromPool<Connection>();
				triangleMeshNode2.SetConnectivityDirty();
			}
			dictionary.Clear();
			ObjectPoolSimple<Dictionary<Vector2Int, int>>.Release(ref dictionary);
			ListPool<Connection>.Release(ref list);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x000238E0 File Offset: 0x00021AE0
		internal static void ConnectTiles(NavmeshTile tile1, NavmeshTile tile2, float tileWorldSizeX, float tileWorldSizeZ, float maxTileConnectionEdgeDistance)
		{
			if (tile1 == null || tile2 == null)
			{
				return;
			}
			if (tile1.nodes == null)
			{
				throw new ArgumentException("tile1 does not contain any nodes");
			}
			if (tile2.nodes == null)
			{
				throw new ArgumentException("tile2 does not contain any nodes");
			}
			int num = Mathf.Clamp(tile2.x, tile1.x, tile1.x + tile1.w - 1);
			int num2 = Mathf.Clamp(tile1.x, tile2.x, tile2.x + tile2.w - 1);
			int num3 = Mathf.Clamp(tile2.z, tile1.z, tile1.z + tile1.d - 1);
			int num4 = Mathf.Clamp(tile1.z, tile2.z, tile2.z + tile2.d - 1);
			int i;
			int i2;
			int num5;
			int num6;
			float num7;
			if (num == num2)
			{
				i = 2;
				i2 = 0;
				num5 = num3;
				num6 = num4;
				num7 = tileWorldSizeZ;
			}
			else
			{
				if (num3 != num4)
				{
					throw new ArgumentException("Tiles are not adjacent (neither x or z coordinates match)");
				}
				i = 0;
				i2 = 2;
				num5 = num;
				num6 = num2;
				num7 = tileWorldSizeX;
			}
			if (Math.Abs(num5 - num6) != 1)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Tiles are not adjacent (tile coordinates must differ by exactly 1. Got '",
					num5.ToString(),
					"' and '",
					num6.ToString(),
					"')"
				}));
			}
			int num8 = (int)Math.Round((double)((float)Math.Max(num5, num6) * num7 * 1000f));
			TriangleMeshNode[] nodes = tile1.nodes;
			TriangleMeshNode[] nodes2 = tile2.nodes;
			TriangleMeshNode[] array = ArrayPool<TriangleMeshNode>.Claim(nodes2.Length);
			int num9 = 0;
			for (int j = 0; j < nodes2.Length; j++)
			{
				TriangleMeshNode triangleMeshNode = nodes2[j];
				int vertexCount = triangleMeshNode.GetVertexCount();
				for (int k = 0; k < vertexCount; k++)
				{
					Int3 vertexInGraphSpace = tile2.GetVertexInGraphSpace(triangleMeshNode.GetVertexIndex(k));
					Int3 vertexInGraphSpace2 = tile2.GetVertexInGraphSpace(triangleMeshNode.GetVertexIndex((k + 1) % vertexCount));
					if (Math.Abs(vertexInGraphSpace[i] - num8) < 2 && Math.Abs(vertexInGraphSpace2[i] - num8) < 2)
					{
						array[num9] = nodes2[j];
						num9++;
						break;
					}
				}
			}
			foreach (TriangleMeshNode triangleMeshNode2 in nodes)
			{
				int vertexCount2 = triangleMeshNode2.GetVertexCount();
				for (int m = 0; m < vertexCount2; m++)
				{
					Int3 vertexInGraphSpace3 = tile1.GetVertexInGraphSpace(triangleMeshNode2.GetVertexIndex(m));
					Int3 vertexInGraphSpace4 = tile1.GetVertexInGraphSpace(triangleMeshNode2.GetVertexIndex((m + 1) % vertexCount2));
					if (Math.Abs(vertexInGraphSpace3[i] - num8) < 2 && Math.Abs(vertexInGraphSpace4[i] - num8) < 2)
					{
						int num10 = Math.Min(vertexInGraphSpace3[i2], vertexInGraphSpace4[i2]);
						int num11 = Math.Max(vertexInGraphSpace3[i2], vertexInGraphSpace4[i2]);
						if (num10 != num11)
						{
							for (int n = 0; n < num9; n++)
							{
								TriangleMeshNode triangleMeshNode3 = array[n];
								int vertexCount3 = triangleMeshNode3.GetVertexCount();
								for (int num12 = 0; num12 < vertexCount3; num12++)
								{
									Int3 vertexInGraphSpace5 = tile2.GetVertexInGraphSpace(triangleMeshNode3.GetVertexIndex(num12));
									Int3 vertexInGraphSpace6 = tile2.GetVertexInGraphSpace(triangleMeshNode3.GetVertexIndex((num12 + 1) % vertexCount3));
									if (Math.Abs(vertexInGraphSpace5[i] - num8) < 2 && Math.Abs(vertexInGraphSpace6[i] - num8) < 2)
									{
										int num13 = Math.Min(vertexInGraphSpace5[i2], vertexInGraphSpace6[i2]);
										int num14 = Math.Max(vertexInGraphSpace5[i2], vertexInGraphSpace6[i2]);
										if (num13 != num14 && num11 > num13 && num10 < num14)
										{
											bool flag = (vertexInGraphSpace3 == vertexInGraphSpace5 && vertexInGraphSpace4 == vertexInGraphSpace6) || (vertexInGraphSpace3 == vertexInGraphSpace6 && vertexInGraphSpace4 == vertexInGraphSpace5);
											if (flag || VectorMath.SqrDistanceSegmentSegment((Vector3)vertexInGraphSpace3, (Vector3)vertexInGraphSpace4, (Vector3)vertexInGraphSpace5, (Vector3)vertexInGraphSpace6) < maxTileConnectionEdgeDistance * maxTileConnectionEdgeDistance)
											{
												uint costMagnitude = (uint)(triangleMeshNode2.position - triangleMeshNode3.position).costMagnitude;
												triangleMeshNode2.AddPartialConnection(triangleMeshNode3, costMagnitude, Connection.PackShapeEdgeInfo((byte)m, (byte)num12, flag, true, true));
												triangleMeshNode3.AddPartialConnection(triangleMeshNode2, costMagnitude, Connection.PackShapeEdgeInfo((byte)num12, (byte)m, flag, true, true));
											}
										}
									}
								}
							}
						}
					}
				}
			}
			ArrayPool<TriangleMeshNode>.Release(ref array, false);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00023D5A File Offset: 0x00021F5A
		public void StartBatchTileUpdate(bool exclusive = false)
		{
			base.AssertSafeToUpdateGraph();
			if (exclusive && this.batchTileUpdate > 0)
			{
				throw new InvalidOperationException("Calling StartBatchTileUpdate when batching is already enabled");
			}
			this.batchTileUpdate++;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00023D88 File Offset: 0x00021F88
		private static void DestroyNodes(List<MeshNode> nodes)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i].TemporaryFlag1 = true;
			}
			for (int j = 0; j < nodes.Count; j++)
			{
				MeshNode meshNode = nodes[j];
				if (meshNode.connections != null)
				{
					for (int k = 0; k < meshNode.connections.Length; k++)
					{
						GraphNode node = meshNode.connections[k].node;
						if (!node.TemporaryFlag1)
						{
							node.RemovePartialConnection(meshNode);
						}
					}
					ArrayPool<Connection>.Release(ref meshNode.connections, true);
				}
				meshNode.Destroy();
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00023E20 File Offset: 0x00022020
		private void TryConnect(int tileIdx1, int tileIdx2)
		{
			if (this.tiles[tileIdx1].flag && this.tiles[tileIdx2].flag && tileIdx1 >= tileIdx2)
			{
				return;
			}
			NavmeshBase.ConnectTiles(this.tiles[tileIdx1], this.tiles[tileIdx2], this.TileWorldSizeX, this.TileWorldSizeZ, this.MaxTileConnectionEdgeDistance);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00023E78 File Offset: 0x00022078
		public void EndBatchTileUpdate()
		{
			if (this.batchTileUpdate < 0)
			{
				throw new InvalidOperationException("Calling EndBatchTileUpdate when batching had not yet been started");
			}
			if (this.batchTileUpdate > 1)
			{
				this.batchTileUpdate--;
				return;
			}
			if (this.batchPendingNavmeshCutting)
			{
				this.batchPendingNavmeshCutting = false;
				this.navmeshUpdateData.ReloadDirtyTilesImmediately();
			}
			this.batchTileUpdate--;
			NavmeshBase.DestroyNodes(this.batchNodesToDestroy);
			this.batchNodesToDestroy.ClearFast<MeshNode>();
			if (this.batchUpdatedTiles.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.batchUpdatedTiles.Count; i++)
			{
				this.tiles[this.batchUpdatedTiles[i]].flag = true;
			}
			IntRect rect = default(IntRect);
			for (int j = 0; j < this.batchUpdatedTiles.Count; j++)
			{
				int num = this.batchUpdatedTiles[j] % this.tileXCount;
				int num2 = this.batchUpdatedTiles[j] / this.tileXCount;
				if (j == 0)
				{
					rect = new IntRect(num, num2, num, num2);
				}
				else
				{
					rect = rect.ExpandToContain(num, num2);
				}
				if (num > 0)
				{
					this.TryConnect(this.batchUpdatedTiles[j], this.batchUpdatedTiles[j] - 1);
				}
				if (num < this.tileXCount - 1)
				{
					this.TryConnect(this.batchUpdatedTiles[j], this.batchUpdatedTiles[j] + 1);
				}
				if (num2 > 0)
				{
					this.TryConnect(this.batchUpdatedTiles[j], this.batchUpdatedTiles[j] - this.tileXCount);
				}
				if (num2 < this.tileZCount - 1)
				{
					this.TryConnect(this.batchUpdatedTiles[j], this.batchUpdatedTiles[j] + this.tileXCount);
				}
			}
			for (int k = 0; k < this.batchUpdatedTiles.Count; k++)
			{
				this.tiles[this.batchUpdatedTiles[k]].flag = false;
			}
			this.batchUpdatedTiles.ClearFast<int>();
			base.DirtyBounds(this.GetTileBounds(rect));
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002408C File Offset: 0x0002228C
		public void ClearTiles(IntRect tileRect)
		{
			base.AssertSafeToUpdateGraph();
			this.StartBatchTileUpdate(false);
			IntRect b = new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1);
			tileRect = IntRect.Intersection(tileRect, b);
			for (int i = tileRect.ymin; i <= tileRect.ymax; i++)
			{
				for (int j = tileRect.xmin; j <= tileRect.xmax; j++)
				{
					this.ClearTile(j, i, this.NewEmptyTile(j, i));
				}
			}
			this.EndBatchTileUpdate();
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0002410C File Offset: 0x0002230C
		protected void ClearTile(int x, int z, NavmeshTile replacement)
		{
			if (this.batchTileUpdate == 0)
			{
				throw new Exception("Must be called during a batch update. See StartBatchTileUpdate");
			}
			NavmeshTile tile = this.GetTile(x, z);
			if (tile == null)
			{
				return;
			}
			TriangleMeshNode[] nodes = tile.nodes;
			for (int i = 0; i < nodes.Length; i++)
			{
				if (nodes[i] != null)
				{
					this.batchNodesToDestroy.Add(nodes[i]);
				}
			}
			tile.Dispose();
			this.tiles[x + z * this.tileXCount] = replacement;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002417C File Offset: 0x0002237C
		private unsafe void PrepareNodeRecycling(int x, int z, UnsafeSpan<Int3> verts, UnsafeSpan<int> tris, TriangleMeshNode[] recycledNodeBuffer)
		{
			NavmeshTile tile = this.GetTile(x, z);
			if (tile == null || tile.nodes.Length == 0)
			{
				return;
			}
			TriangleMeshNode[] nodes = tile.nodes;
			Dictionary<int, int> dictionary = this.nodeRecyclingHashBuffer;
			int i = 0;
			int num = 0;
			while (i < tris.Length)
			{
				dictionary[verts[*tris[i]].GetHashCode() * 31 ^ verts[*tris[i + 1]].GetHashCode() * 196613 ^ verts[*tris[i + 2]].GetHashCode() * 3145739] = num;
				i += 3;
				num++;
			}
			List<Connection> list = ListPool<Connection>.Claim();
			for (int j = 0; j < nodes.Length; j++)
			{
				TriangleMeshNode triangleMeshNode = nodes[j];
				Int3 rhs;
				Int3 rhs2;
				Int3 rhs3;
				triangleMeshNode.GetVerticesInGraphSpace(out rhs, out rhs2, out rhs3);
				int key = rhs.GetHashCode() * 31 ^ rhs2.GetHashCode() * 196613 ^ rhs3.GetHashCode() * 3145739;
				int num2;
				if (dictionary.TryGetValue(key, out num2) && *verts[*tris[3 * num2]] == rhs && *verts[*tris[3 * num2 + 1]] == rhs2 && *verts[*tris[3 * num2 + 2]] == rhs3)
				{
					recycledNodeBuffer[num2] = triangleMeshNode;
					nodes[j] = null;
					if (triangleMeshNode.connections != null)
					{
						for (int k = 0; k < triangleMeshNode.connections.Length; k++)
						{
							if (triangleMeshNode.connections[k].node.GraphIndex != triangleMeshNode.GraphIndex)
							{
								list.Add(triangleMeshNode.connections[k]);
							}
						}
						ArrayPool<Connection>.Release(ref triangleMeshNode.connections, true);
					}
					if (list.Count > 0)
					{
						triangleMeshNode.connections = list.ToArrayFromPool<Connection>();
						triangleMeshNode.SetConnectivityDirty();
						list.Clear();
					}
				}
			}
			dictionary.Clear();
			ListPool<Connection>.Release(ref list);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x000243CC File Offset: 0x000225CC
		public void ReplaceTile(int x, int z, Int3[] verts, int[] tris, uint[] tags = null, bool tryPreserveExistingTagsAndPenalties = true)
		{
			ulong gcHandle;
			UnsafeSpan<Int3> verts2 = new UnsafeSpan<Int3>(verts, ref gcHandle);
			ulong gcHandle2;
			UnsafeSpan<int> tris2 = new UnsafeSpan<int>(tris, ref gcHandle2);
			ulong gcHandle3 = 0UL;
			UnsafeSpan<uint> tags2 = (tags != null) ? new UnsafeSpan<uint>(tags, ref gcHandle3) : default(UnsafeSpan<uint>);
			try
			{
				this.ReplaceTile(x, z, verts2, tris2, tags2, tryPreserveExistingTagsAndPenalties);
			}
			finally
			{
				UnsafeUtility.ReleaseGCObject(gcHandle);
				UnsafeUtility.ReleaseGCObject(gcHandle2);
				if (tags != null)
				{
					UnsafeUtility.ReleaseGCObject(gcHandle3);
				}
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00024444 File Offset: 0x00022644
		public void ReplaceTile(int x, int z, UnsafeSpan<Int3> verts, UnsafeSpan<int> tris, UnsafeSpan<uint> tags, bool tryPreserveExistingTagsAndPenalties = true)
		{
			base.AssertSafeToUpdateGraph();
			if (tris.Length % 3 != 0)
			{
				throw new ArgumentException("Triangle array's length must be a multiple of 3 (tris)");
			}
			if (tags.Length > 0 && tags.Length != tris.Length / 3)
			{
				throw new ArgumentException("Triangle array must be 3 times the size of the tags array");
			}
			NavmeshTile tile = this.GetTile(x, z);
			if (tile.isCut)
			{
				this.StartBatchTileUpdate(false);
				tile.preCutTags.Free(Allocator.Persistent);
				tile.preCutTris.Free(Allocator.Persistent);
				tile.preCutVertsInTileSpace.Free(Allocator.Persistent);
				if (tags.Length == 0)
				{
					tile.preCutTags = new UnsafeSpan<uint>(Allocator.Persistent, tris.Length / 3);
					tile.preCutTags.FillZeros<uint>();
				}
				else
				{
					tile.preCutTags = tags.Clone(Allocator.Persistent);
				}
				tile.preCutTris = tris.Clone(Allocator.Persistent);
				tile.preCutVertsInTileSpace = verts.Clone(Allocator.Persistent);
				tile.isCut = true;
				this.navmeshUpdateData.MarkTilesDirty(new IntRect(x, z, x, z));
				this.batchPendingNavmeshCutting = true;
				this.EndBatchTileUpdate();
				return;
			}
			this.ReplaceTilePostCut(x, z, verts, tris, tags, tryPreserveExistingTagsAndPenalties, false);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00024564 File Offset: 0x00022764
		internal unsafe void ReplaceTilePostCut(int x, int z, UnsafeSpan<Int3> verts, UnsafeSpan<int> tris, UnsafeSpan<uint> tags, bool tryPreserveExistingTagsAndPenalties = true, bool preservePreCutData = false)
		{
			base.AssertSafeToUpdateGraph();
			int num = 1;
			int num2 = 1;
			if (x + num > this.tileXCount || z + num2 > this.tileZCount || x < 0 || z < 0)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Tile is placed at an out of bounds position or extends out of the graph bounds (",
					x.ToString(),
					", ",
					z.ToString(),
					" [",
					num.ToString(),
					", ",
					num2.ToString(),
					"] ",
					this.tileXCount.ToString(),
					" ",
					this.tileZCount.ToString(),
					")"
				}));
			}
			if (tris.Length % 3 != 0)
			{
				throw new ArgumentException("Triangle array's length must be a multiple of 3 (tris)");
			}
			if (tags.Length > 0 && tags.Length != tris.Length / 3)
			{
				throw new ArgumentException("Triangle array must be 3 times the size of the tags array");
			}
			if (verts.Length > 4095)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Too many vertices in the tile (",
					verts.Length.ToString(),
					" > ",
					4095.ToString(),
					")\nYou can enable ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector to raise this limit. Or you can use a smaller tile size to reduce the likelihood of this happening."
				}));
				verts = default(UnsafeSpan<Int3>);
				tris = default(UnsafeSpan<int>);
			}
			if (tris.Length == 0 && this.GetTile(x, z).nodes.Length == 0 && preservePreCutData)
			{
				return;
			}
			this.StartBatchTileUpdate(false);
			UnsafeSpan<int> unsafeSpan = tris.Clone(Allocator.Persistent);
			unsafeSpan.CopyFrom(tris);
			UnsafeSpan<Int3> unsafeSpan2 = verts.Clone(Allocator.Persistent);
			Int3 rhs = (Int3)new Vector3((float)x * this.TileWorldSizeX, 0f, (float)z * this.TileWorldSizeZ);
			for (int i = 0; i < verts.Length; i++)
			{
				*unsafeSpan2[i] += rhs;
			}
			UnsafeSpan<Int3> unsafeSpan3 = unsafeSpan2.Clone(Allocator.Persistent);
			this.transform.Transform(unsafeSpan3);
			BBTree bbTree = new BBTree(unsafeSpan, unsafeSpan2);
			NavmeshTile navmeshTile = new NavmeshTile
			{
				x = x,
				z = z,
				w = num,
				d = num2,
				tris = unsafeSpan,
				vertsInGraphSpace = unsafeSpan2,
				verts = unsafeSpan3,
				bbTree = bbTree,
				graph = this
			};
			if (!Mathf.Approximately((float)x * this.TileWorldSizeX * 1000f, (float)Math.Round((double)((float)x * this.TileWorldSizeX * 1000f))))
			{
				Debug.LogWarning("Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
			}
			if (!Mathf.Approximately((float)z * this.TileWorldSizeZ * 1000f, (float)Math.Round((double)((float)z * this.TileWorldSizeZ * 1000f))))
			{
				Debug.LogWarning("Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
			}
			navmeshTile.nodes = new TriangleMeshNode[unsafeSpan.Length / 3];
			this.PrepareNodeRecycling(x, z, unsafeSpan2, unsafeSpan, navmeshTile.nodes);
			if (this.RecalculateNormals)
			{
				MeshUtility.MakeTrianglesClockwise(ref navmeshTile.vertsInGraphSpace, ref navmeshTile.tris);
			}
			NavmeshBase.CreateNodes(navmeshTile, navmeshTile.tris, x + z * this.tileXCount, (uint)this.active.data.GetGraphIndex(this), tags, true, this.active, this.initialPenalty, tryPreserveExistingTagsAndPenalties);
			NavmeshBase.CreateNodeConnections(navmeshTile.nodes, true);
			if (preservePreCutData)
			{
				NavmeshTile tile = this.GetTile(x, z);
				navmeshTile.preCutVertsInTileSpace = tile.preCutVertsInTileSpace;
				navmeshTile.preCutTris = tile.preCutTris;
				navmeshTile.preCutTags = tile.preCutTags;
				navmeshTile.isCut = tile.isCut;
				tile.preCutVertsInTileSpace = default(UnsafeSpan<Int3>);
				tile.preCutTris = default(UnsafeSpan<int>);
				tile.preCutTags = default(UnsafeSpan<uint>);
			}
			this.ClearTile(x, z, navmeshTile);
			this.batchUpdatedTiles.Add(x + z * this.tileXCount);
			this.EndBatchTileUpdate();
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0002495C File Offset: 0x00022B5C
		internal unsafe static void CreateNodes(NavmeshTile tile, UnsafeSpan<int> tris, int tileIndex, uint graphIndex, UnsafeSpan<uint> tags, bool initializeNodes, AstarPath astar, uint initialPenalty, bool tryPreserveExistingTagsAndPenalties)
		{
			TriangleMeshNode[] nodes = tile.nodes;
			if (nodes == null || nodes.Length < tris.Length / 3)
			{
				throw new ArgumentException("nodes must be non null and at least as large as tris.Length/3");
			}
			tileIndex <<= 12;
			for (int i = 0; i < nodes.Length; i++)
			{
				TriangleMeshNode triangleMeshNode = nodes[i];
				bool flag = false;
				if (triangleMeshNode == null)
				{
					flag = true;
					if (initializeNodes)
					{
						triangleMeshNode = (nodes[i] = new TriangleMeshNode(astar));
					}
					else
					{
						triangleMeshNode = (nodes[i] = new TriangleMeshNode());
					}
				}
				if (!tryPreserveExistingTagsAndPenalties || flag)
				{
					if (tags.Length > 0)
					{
						triangleMeshNode.Tag = *tags[i];
					}
					triangleMeshNode.Penalty = initialPenalty;
				}
				triangleMeshNode.Walkable = true;
				triangleMeshNode.GraphIndex = graphIndex;
				triangleMeshNode.v0 = (*tris[i * 3] | tileIndex);
				triangleMeshNode.v1 = (*tris[i * 3 + 1] | tileIndex);
				triangleMeshNode.v2 = (*tris[i * 3 + 2] | tileIndex);
				triangleMeshNode.position = (tile.GetVertex(triangleMeshNode.v0) + tile.GetVertex(triangleMeshNode.v1) + tile.GetVertex(triangleMeshNode.v2)) * 0.33333334f;
			}
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00024A88 File Offset: 0x00022C88
		public NavmeshBase()
		{
			new NavmeshUpdates.NavmeshUpdateSettings(this).AttachToGraph();
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00024B01 File Offset: 0x00022D01
		public bool Linecast(Vector3 start, Vector3 end)
		{
			return this.Linecast(start, end, null);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00024B0C File Offset: 0x00022D0C
		public bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit)
		{
			return NavmeshBase.Linecast(this, start, end, hint, out hit, null, null);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00024B1C File Offset: 0x00022D1C
		public bool Linecast(Vector3 start, Vector3 end, GraphNode hint)
		{
			GraphHitInfo graphHitInfo;
			return NavmeshBase.Linecast(this, start, end, hint, out graphHitInfo, null, null);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00024B36 File Offset: 0x00022D36
		public bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
		{
			return NavmeshBase.Linecast(this, start, end, hint, out hit, trace, null);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00024B46 File Offset: 0x00022D46
		public bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter)
		{
			return NavmeshBase.Linecast(this, start, end, null, out hit, trace, filter);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00024B56 File Offset: 0x00022D56
		public bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter)
		{
			return NavmeshBase.Linecast(this, start, end, hint, out hit, trace, filter);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00024B0C File Offset: 0x00022D0C
		public static bool Linecast(NavmeshBase graph, Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit)
		{
			return NavmeshBase.Linecast(graph, start, end, hint, out hit, null, null);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00024B68 File Offset: 0x00022D68
		static NavmeshBase()
		{
			Side[] array = new Side[3];
			for (int i = 0; i < NavmeshBase.LinecastShapeEdgeLookup.Length; i++)
			{
				array[0] = (Side)(i & 3);
				array[1] = (Side)(i >> 2 & 3);
				array[2] = (Side)(i >> 4 & 3);
				NavmeshBase.LinecastShapeEdgeLookup[i] = byte.MaxValue;
				if (array[0] != (Side)3 && array[1] != (Side)3 && array[2] != (Side)3)
				{
					int num = int.MaxValue;
					for (int j = 0; j < 3; j++)
					{
						if ((array[j] == Side.Left || array[j] == Side.Colinear) && (array[(j + 1) % 3] == Side.Right || array[(j + 1) % 3] == Side.Colinear))
						{
							int num2 = ((array[j] == Side.Colinear) ? 1 : 0) + ((array[(j + 1) % 3] == Side.Colinear) ? 1 : 0);
							if (num2 < num)
							{
								NavmeshBase.LinecastShapeEdgeLookup[i] = (byte)j;
								num = num2;
							}
						}
					}
				}
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00024C64 File Offset: 0x00022E64
		public static bool Linecast(NavmeshBase graph, Vector3 origin, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter = null)
		{
			if (!graph.RecalculateNormals)
			{
				throw new InvalidOperationException("The graph is configured to not recalculate normals. This is typically used for spherical navmeshes or other non-planar ones. Linecasts cannot be done on such navmeshes. Enable 'Recalculate Normals' on the navmesh graph if you want to use linecasts.");
			}
			hit = default(GraphHitInfo);
			if (float.IsNaN(origin.x + origin.y + origin.z))
			{
				throw new ArgumentException("origin is NaN");
			}
			if (float.IsNaN(end.x + end.y + end.z))
			{
				throw new ArgumentException("end is NaN");
			}
			TriangleMeshNode triangleMeshNode = hint as TriangleMeshNode;
			NavmeshBase.NNConstraintNoneXZ.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
			if (triangleMeshNode == null)
			{
				NNInfo nearest = graph.GetNearest(origin, NavmeshBase.NNConstraintNoneXZ);
				triangleMeshNode = (nearest.node as TriangleMeshNode);
				if (triangleMeshNode == null || nearest.distanceCostSqr > 1.0000001E-06f)
				{
					hit.origin = origin;
					hit.point = origin;
					return true;
				}
			}
			Int3 @int = triangleMeshNode.ClosestPointOnNodeXZInGraphSpace(origin);
			hit.origin = graph.transform.Transform((Vector3)@int);
			if (!triangleMeshNode.Walkable || (filter != null && !filter(triangleMeshNode)))
			{
				hit.node = triangleMeshNode;
				hit.point = hit.origin;
				hit.tangentOrigin = hit.origin;
				return true;
			}
			Int3 int2 = (Int3)graph.transform.InverseTransform(end);
			if (@int == int2)
			{
				hit.point = hit.origin;
				hit.node = triangleMeshNode;
				if (trace != null)
				{
					trace.Add(triangleMeshNode);
				}
				return false;
			}
			int num = 0;
			Int3 int3;
			Int3 int4;
			Int3 int5;
			int num3;
			TriangleMeshNode triangleMeshNode2;
			for (;;)
			{
				num++;
				if (num > 2000)
				{
					break;
				}
				if (trace != null)
				{
					trace.Add(triangleMeshNode);
				}
				triangleMeshNode.GetVerticesInGraphSpace(out int3, out int4, out int5);
				int num2 = (int)VectorMath.SideXZ(@int, int2, int3);
				num2 |= (int)((int)VectorMath.SideXZ(@int, int2, int4) << 2);
				num2 |= (int)((int)VectorMath.SideXZ(@int, int2, int5) << 4);
				num3 = (int)NavmeshBase.LinecastShapeEdgeLookup[num2];
				if (num3 == 255)
				{
					goto Block_12;
				}
				Side side = VectorMath.SideXZ((num3 == 0) ? int3 : ((num3 == 1) ? int4 : int5), (num3 == 0) ? int4 : ((num3 == 1) ? int5 : int3), int2);
				if (side != Side.Left)
				{
					hit.point = end;
					hit.node = triangleMeshNode;
					triangleMeshNode2 = (graph.GetNearest(end, NavmeshBase.NNConstraintNoneXZ).node as TriangleMeshNode);
					if (triangleMeshNode2 == triangleMeshNode || triangleMeshNode2 == null)
					{
						return false;
					}
					if (side != Side.Colinear)
					{
						return true;
					}
					if (int3 == int2 || int4 == int2 || int5 == int2)
					{
						goto IL_27F;
					}
				}
				bool flag = false;
				Connection[] connections = triangleMeshNode.connections;
				for (int i = 0; i < connections.Length; i++)
				{
					if (connections[i].isEdgeShared && connections[i].isOutgoing && connections[i].shapeEdge == num3)
					{
						TriangleMeshNode triangleMeshNode3 = connections[i].node as TriangleMeshNode;
						if (triangleMeshNode3 != null && triangleMeshNode3.Walkable && (filter == null || filter(triangleMeshNode3)))
						{
							int adjacentShapeEdge = connections[i].adjacentShapeEdge;
							Side side2 = VectorMath.SideXZ(@int, int2, triangleMeshNode3.GetVertexInGraphSpace(adjacentShapeEdge));
							Side side3 = VectorMath.SideXZ(@int, int2, triangleMeshNode3.GetVertexInGraphSpace((adjacentShapeEdge + 1) % 3));
							flag = ((side2 == Side.Right || side2 == Side.Colinear) && (side3 == Side.Left || side3 == Side.Colinear));
							if (flag)
							{
								triangleMeshNode = triangleMeshNode3;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					goto Block_32;
				}
			}
			Debug.LogError("Linecast was stuck in infinite loop. Breaking.");
			return true;
			Block_12:
			Debug.LogError("Line does not intersect node at all");
			hit.node = triangleMeshNode;
			hit.point = (hit.tangentOrigin = hit.origin);
			return true;
			IL_27F:
			return !NavmeshBase.FindNodeAroundVertex(triangleMeshNode, triangleMeshNode2, int2, false) && !NavmeshBase.FindNodeAroundVertex(triangleMeshNode, triangleMeshNode2, int2, true);
			Block_32:
			Vector3 vector = (Vector3)((num3 == 0) ? int3 : ((num3 == 1) ? int4 : int5));
			Vector3 vector2 = (Vector3)((num3 == 0) ? int4 : ((num3 == 1) ? int5 : int3));
			Vector3 point = VectorMath.LineIntersectionPointXZ(vector, vector2, (Vector3)@int, (Vector3)int2);
			hit.point = graph.transform.Transform(point);
			hit.node = triangleMeshNode;
			Vector3 vector3 = graph.transform.Transform(vector);
			Vector3 a = graph.transform.Transform(vector2);
			hit.tangent = a - vector3;
			hit.tangentOrigin = vector3;
			return true;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000250A0 File Offset: 0x000232A0
		private static bool FindNodeAroundVertex(TriangleMeshNode node, TriangleMeshNode targetNode, Int3 vertexInGraphSpace, bool oppositeDirection)
		{
			TriangleMeshNode triangleMeshNode = node;
			bool flag = true;
			while (flag)
			{
				flag = false;
				int num = 0;
				while (num < 3 && !flag)
				{
					if (node.GetVertexInGraphSpace(num) == vertexInGraphSpace)
					{
						int num2 = oppositeDirection ? ((num - 1 + 3) % 3) : num;
						int i = 0;
						while (i < node.connections.Length)
						{
							Connection connection = node.connections[i];
							if (connection.isEdgeShared && connection.edgesAreIdentical && connection.shapeEdge == num2)
							{
								node = (connection.node as TriangleMeshNode);
								flag = true;
								if (node == targetNode)
								{
									return true;
								}
								if (node == triangleMeshNode)
								{
									return false;
								}
								break;
							}
							else
							{
								i++;
							}
						}
					}
					num++;
				}
			}
			return false;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002514C File Offset: 0x0002334C
		public override void OnDrawGizmos(DrawingData gizmos, bool drawNodes, RedrawScope redrawScope)
		{
			if (!drawNodes)
			{
				return;
			}
			using (CommandBuilder builder = gizmos.GetBuilder(redrawScope, false))
			{
				Bounds bounds = default(Bounds);
				bounds.SetMinMax(Vector3.zero, this.forcedBoundsSize);
				using (builder.WithMatrix(this.CalculateTransform().matrix))
				{
					builder.WireBox(bounds, Color.white);
				}
			}
			if (this.tiles != null && (this.showMeshSurface || this.showMeshOutline || this.showNodeConnections))
			{
				NodeHasher nodeHasher = new NodeHasher(this.active);
				nodeHasher.Add<int>(this.showMeshOutline ? 1 : 0);
				nodeHasher.Add<int>(this.showMeshSurface ? 1 : 0);
				nodeHasher.Add<int>(this.showNodeConnections ? 1 : 0);
				int num = 0;
				NodeHasher hasher = nodeHasher;
				int num2 = 0;
				for (int i = 0; i < this.tiles.Length; i++)
				{
					if (this.tiles[i] != null)
					{
						TriangleMeshNode[] nodes = this.tiles[i].nodes;
						for (int j = 0; j < nodes.Length; j++)
						{
							hasher.HashNode(nodes[j]);
						}
						num2 += nodes.Length;
						if (num2 > 1024 || i % this.tileXCount == this.tileXCount - 1 || i == this.tiles.Length - 1)
						{
							if (!gizmos.Draw(hasher, redrawScope))
							{
								using (GraphGizmoHelper gizmoHelper = GraphGizmoHelper.GetGizmoHelper(gizmos, this.active, hasher, redrawScope))
								{
									if (this.showMeshSurface || this.showMeshOutline)
									{
										this.CreateNavmeshSurfaceVisualization(this.tiles, num, i + 1, gizmoHelper);
										NavmeshBase.CreateNavmeshOutlineVisualization(this.tiles, num, i + 1, gizmoHelper);
									}
									if (this.showNodeConnections)
									{
										if (gizmoHelper.showSearchTree)
										{
											gizmoHelper.builder.PushLineWidth(2f, true);
										}
										for (int k = num; k <= i; k++)
										{
											if (this.tiles[k] != null)
											{
												TriangleMeshNode[] nodes2 = this.tiles[k].nodes;
												for (int l = 0; l < nodes2.Length; l++)
												{
													gizmoHelper.DrawConnections(nodes2[l]);
												}
											}
										}
										if (gizmoHelper.showSearchTree)
										{
											gizmoHelper.builder.PopLineWidth();
										}
									}
								}
							}
							num = i + 1;
							hasher = nodeHasher;
							num2 = 0;
						}
					}
				}
			}
			if (this.active.showUnwalkableNodes)
			{
				base.DrawUnwalkableNodes(gizmos, this.active.unwalkableNodeDebugSize, redrawScope);
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002540C File Offset: 0x0002360C
		private void CreateNavmeshSurfaceVisualization(NavmeshTile[] tiles, int startTile, int endTile, GraphGizmoHelper helper)
		{
			int num = 0;
			for (int i = startTile; i < endTile; i++)
			{
				if (tiles[i] != null)
				{
					num += tiles[i].nodes.Length;
				}
			}
			Vector3[] array = ArrayPool<Vector3>.Claim(num * 3);
			Color[] array2 = ArrayPool<Color>.Claim(num * 3);
			int num2 = 0;
			for (int j = startTile; j < endTile; j++)
			{
				NavmeshTile navmeshTile = tiles[j];
				if (navmeshTile != null)
				{
					for (int k = 0; k < navmeshTile.nodes.Length; k++)
					{
						TriangleMeshNode triangleMeshNode = navmeshTile.nodes[k];
						Int3 ob;
						Int3 ob2;
						Int3 ob3;
						triangleMeshNode.GetVertices(out ob, out ob2, out ob3);
						int num3 = num2 + k * 3;
						array[num3] = (Vector3)ob;
						array[num3 + 1] = (Vector3)ob2;
						array[num3 + 2] = (Vector3)ob3;
						Color color = helper.NodeColor(triangleMeshNode);
						array2[num3] = (array2[num3 + 1] = (array2[num3 + 2] = color));
					}
					num2 += navmeshTile.nodes.Length * 3;
				}
			}
			if (this.showMeshSurface)
			{
				helper.DrawTriangles(array, array2, num);
			}
			if (this.showMeshOutline)
			{
				helper.DrawWireTriangles(array, array2, num);
			}
			ArrayPool<Vector3>.Release(ref array, false);
			ArrayPool<Color>.Release(ref array2, false);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002555C File Offset: 0x0002375C
		private static void CreateNavmeshOutlineVisualization(NavmeshTile[] tiles, int startTile, int endTile, GraphGizmoHelper helper)
		{
			bool[] array = new bool[3];
			for (int i = startTile; i < endTile; i++)
			{
				NavmeshTile navmeshTile = tiles[i];
				if (navmeshTile != null)
				{
					for (int j = 0; j < navmeshTile.nodes.Length; j++)
					{
						array[0] = (array[1] = (array[2] = false));
						TriangleMeshNode triangleMeshNode = navmeshTile.nodes[j];
						if (triangleMeshNode.connections != null)
						{
							for (int k = 0; k < triangleMeshNode.connections.Length; k++)
							{
								TriangleMeshNode triangleMeshNode2 = triangleMeshNode.connections[k].node as TriangleMeshNode;
								if (triangleMeshNode2 != null && triangleMeshNode2.GraphIndex == triangleMeshNode.GraphIndex)
								{
									for (int l = 0; l < 3; l++)
									{
										for (int m = 0; m < 3; m++)
										{
											if (triangleMeshNode.GetVertexIndex(l) == triangleMeshNode2.GetVertexIndex((m + 1) % 3) && triangleMeshNode.GetVertexIndex((l + 1) % 3) == triangleMeshNode2.GetVertexIndex(m))
											{
												array[l] = true;
												l = 3;
												break;
											}
										}
									}
								}
							}
						}
						Color color = helper.NodeColor(triangleMeshNode);
						for (int n = 0; n < 3; n++)
						{
							if (!array[n])
							{
								helper.builder.Line((Vector3)triangleMeshNode.GetVertex(n), (Vector3)triangleMeshNode.GetVertex((n + 1) % 3), color);
							}
						}
					}
				}
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000256C4 File Offset: 0x000238C4
		protected unsafe override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			BinaryWriter writer = ctx.writer;
			if (this.tiles == null)
			{
				writer.Write(-1);
				return;
			}
			writer.Write(this.tileXCount);
			writer.Write(this.tileZCount);
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					NavmeshTile navmeshTile = this.tiles[j + i * this.tileXCount];
					if (navmeshTile == null)
					{
						throw new NullReferenceException();
					}
					writer.Write(navmeshTile.x);
					writer.Write(navmeshTile.z);
					if (navmeshTile.x == j && navmeshTile.z == i)
					{
						writer.Write(navmeshTile.w);
						writer.Write(navmeshTile.d);
						writer.Write(navmeshTile.tris.Length);
						for (int k = 0; k < navmeshTile.tris.Length; k++)
						{
							writer.Write(*navmeshTile.tris[k]);
						}
						writer.Write(navmeshTile.verts.Length);
						for (int l = 0; l < navmeshTile.verts.Length; l++)
						{
							ctx.SerializeInt3(*navmeshTile.verts[l]);
						}
						writer.Write(navmeshTile.vertsInGraphSpace.Length);
						for (int m = 0; m < navmeshTile.vertsInGraphSpace.Length; m++)
						{
							ctx.SerializeInt3(*navmeshTile.vertsInGraphSpace[m]);
						}
						if (navmeshTile.isCut)
						{
							writer.Write(true);
							writer.Write(navmeshTile.preCutTags.Length);
							for (int n = 0; n < navmeshTile.preCutTags.Length; n++)
							{
								writer.Write(*navmeshTile.preCutTags[n]);
							}
							writer.Write(navmeshTile.preCutVertsInTileSpace.Length);
							for (int num = 0; num < navmeshTile.preCutVertsInTileSpace.Length; num++)
							{
								ctx.SerializeInt3(*navmeshTile.preCutVertsInTileSpace[num]);
							}
							writer.Write(navmeshTile.preCutTris.Length);
							for (int num2 = 0; num2 < navmeshTile.preCutTris.Length; num2++)
							{
								writer.Write(*navmeshTile.preCutTris[num2]);
							}
						}
						else
						{
							writer.Write(false);
						}
						writer.Write(navmeshTile.nodes.Length);
						for (int num3 = 0; num3 < navmeshTile.nodes.Length; num3++)
						{
							navmeshTile.nodes[num3].SerializeNode(ctx);
						}
					}
				}
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00025968 File Offset: 0x00023B68
		protected unsafe override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			BinaryReader reader = ctx.reader;
			this.tileXCount = reader.ReadInt32();
			if (this.tileXCount < 0)
			{
				return;
			}
			this.tileZCount = reader.ReadInt32();
			this.transform = this.CalculateTransform();
			this.tiles = new NavmeshTile[this.tileXCount * this.tileZCount];
			TriangleMeshNode.SetNavmeshHolder((int)ctx.graphIndex, this);
			for (int i = 0; i < this.tileZCount; i++)
			{
				for (int j = 0; j < this.tileXCount; j++)
				{
					int num = j + i * this.tileXCount;
					int num2 = reader.ReadInt32();
					if (num2 < 0)
					{
						throw new Exception("Invalid tile coordinates (x < 0)");
					}
					int num3 = reader.ReadInt32();
					if (num3 < 0)
					{
						throw new Exception("Invalid tile coordinates (z < 0)");
					}
					if (num2 != j || num3 != i)
					{
						this.tiles[num] = this.tiles[num3 * this.tileXCount + num2];
					}
					else
					{
						NavmeshTile[] array = this.tiles;
						int num4 = num;
						NavmeshTile navmeshTile = new NavmeshTile();
						navmeshTile.x = num2;
						navmeshTile.z = num3;
						navmeshTile.w = reader.ReadInt32();
						navmeshTile.d = reader.ReadInt32();
						navmeshTile.bbTree = default(BBTree);
						navmeshTile.graph = this;
						navmeshTile.isCut = false;
						NavmeshTile navmeshTile2 = navmeshTile;
						array[num4] = navmeshTile;
						NavmeshTile navmeshTile3 = navmeshTile2;
						navmeshTile3.tris = ctx.ReadSpan<int>(Allocator.Persistent);
						if (navmeshTile3.tris.Length % 3 != 0)
						{
							throw new Exception("Corrupt data. Triangle indices count must be divisable by 3. Read " + navmeshTile3.tris.Length.ToString());
						}
						navmeshTile3.verts = ctx.ReadSpan<Int3>(Allocator.Persistent);
						if (ctx.meta.version.Major >= 4)
						{
							navmeshTile3.vertsInGraphSpace = ctx.ReadSpan<Int3>(Allocator.Persistent);
							if (navmeshTile3.vertsInGraphSpace.Length != navmeshTile3.verts.Length)
							{
								throw new Exception("Corrupt data. Array lengths did not match");
							}
						}
						else
						{
							navmeshTile3.vertsInGraphSpace = new UnsafeSpan<Int3>(Allocator.Persistent, navmeshTile3.verts.Length);
							navmeshTile3.verts.CopyTo(navmeshTile3.vertsInGraphSpace);
							this.transform.InverseTransform(navmeshTile3.vertsInGraphSpace);
						}
						if (ctx.meta.version >= AstarSerializer.V5_2_0)
						{
							navmeshTile3.isCut = reader.ReadBoolean();
							if (navmeshTile3.isCut)
							{
								navmeshTile3.preCutTags = ctx.ReadSpan<uint>(Allocator.Persistent);
								navmeshTile3.preCutVertsInTileSpace = ctx.ReadSpan<Int3>(Allocator.Persistent);
								navmeshTile3.preCutTris = ctx.ReadSpan<int>(Allocator.Persistent);
							}
						}
						int num5 = reader.ReadInt32();
						navmeshTile3.nodes = new TriangleMeshNode[num5];
						num <<= 12;
						for (int k = 0; k < navmeshTile3.nodes.Length; k++)
						{
							TriangleMeshNode triangleMeshNode = new TriangleMeshNode(this.active);
							navmeshTile3.nodes[k] = triangleMeshNode;
							triangleMeshNode.DeserializeNode(ctx);
							triangleMeshNode.v0 = (*navmeshTile3.tris[k * 3] | num);
							triangleMeshNode.v1 = (*navmeshTile3.tris[k * 3 + 1] | num);
							triangleMeshNode.v2 = (*navmeshTile3.tris[k * 3 + 2] | num);
							triangleMeshNode.UpdatePositionFromVertices();
						}
						navmeshTile3.bbTree = new BBTree(navmeshTile3.tris, navmeshTile3.vertsInGraphSpace);
					}
				}
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00025CB0 File Offset: 0x00023EB0
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			if (ctx.meta.version < AstarSerializer.V4_3_68 && this.tiles != null)
			{
				Dictionary<TriangleMeshNode, Connection[]> conns = this.tiles.SelectMany((NavmeshTile s) => s.nodes).ToDictionary((TriangleMeshNode n) => n, (TriangleMeshNode n) => n.connections ?? new Connection[0]);
				NavmeshTile[] array = this.tiles;
				for (int i = 0; i < array.Length; i++)
				{
					NavmeshBase.CreateNodeConnections(array[i].nodes, false);
				}
				foreach (NavmeshTile tile in this.tiles)
				{
					this.ConnectTileWithNeighbours(tile, false);
				}
				this.GetNodes(delegate(GraphNode node)
				{
					TriangleMeshNode triNode = node as TriangleMeshNode;
					IEnumerable<Connection> source = conns[triNode];
					Func<Connection, bool> <>9__4;
					Func<Connection, bool> predicate;
					if ((predicate = <>9__4) == null)
					{
						predicate = (<>9__4 = ((Connection conn) => !triNode.ContainsOutgoingConnection(conn.node)));
					}
					foreach (Connection connection in source.Where(predicate).ToList<Connection>())
					{
						triNode.AddPartialConnection(connection.node, connection.cost, connection.shapeEdgeInfo);
					}
				});
			}
			this.transform = this.CalculateTransform();
			if (this.enableNavmeshCutting && this.isScanned)
			{
				this.navmeshUpdateData.Enable();
			}
		}

		// Token: 0x04000486 RID: 1158
		public const int VertexIndexMask = 4095;

		// Token: 0x04000487 RID: 1159
		public const int TileIndexMask = 524287;

		// Token: 0x04000488 RID: 1160
		public const int TileIndexOffset = 12;

		// Token: 0x04000489 RID: 1161
		[JsonMember]
		public Vector3 forcedBoundsSize = new Vector3(100f, 40f, 100f);

		// Token: 0x0400048A RID: 1162
		[JsonMember]
		public bool showMeshOutline = true;

		// Token: 0x0400048B RID: 1163
		[JsonMember]
		public bool showNodeConnections;

		// Token: 0x0400048C RID: 1164
		[JsonMember]
		public bool showMeshSurface = true;

		// Token: 0x0400048D RID: 1165
		public int tileXCount;

		// Token: 0x0400048E RID: 1166
		public int tileZCount;

		// Token: 0x0400048F RID: 1167
		protected NavmeshTile[] tiles;

		// Token: 0x04000490 RID: 1168
		[JsonMember]
		[Obsolete("Set the appropriate fields on the NNConstraint instead")]
		public bool nearestSearchOnlyXZ;

		// Token: 0x04000491 RID: 1169
		[JsonMember]
		public bool enableNavmeshCutting = true;

		// Token: 0x04000492 RID: 1170
		public NavmeshUpdates.NavmeshUpdateSettings navmeshUpdateData;

		// Token: 0x04000493 RID: 1171
		private int batchTileUpdate;

		// Token: 0x04000494 RID: 1172
		private bool batchPendingNavmeshCutting;

		// Token: 0x04000495 RID: 1173
		private List<int> batchUpdatedTiles = new List<int>();

		// Token: 0x04000496 RID: 1174
		private List<MeshNode> batchNodesToDestroy = new List<MeshNode>();

		// Token: 0x04000497 RID: 1175
		public GraphTransform transform = GraphTransform.identityTransform;

		// Token: 0x04000498 RID: 1176
		public Action<NavmeshTile[]> OnRecalculatedTiles;

		// Token: 0x04000499 RID: 1177
		private Dictionary<int, int> nodeRecyclingHashBuffer = new Dictionary<int, int>();

		// Token: 0x0400049A RID: 1178
		private static readonly NNConstraint NNConstraintNoneXZ = new NNConstraint
		{
			constrainWalkability = false,
			constrainArea = false,
			constrainTags = false,
			constrainDistance = false,
			graphMask = -1
		};

		// Token: 0x0400049B RID: 1179
		private static readonly byte[] LinecastShapeEdgeLookup = new byte[64];
	}
}
