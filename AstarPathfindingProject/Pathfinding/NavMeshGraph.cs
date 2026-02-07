using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.Graphs.Navmesh.Jobs;
using Pathfinding.Serialization;
using Pathfinding.Sync;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000DD RID: 221
	[JsonOptIn]
	[Preserve]
	public class NavMeshGraph : NavmeshBase, IUpdatableGraph
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x00025F1E File Offset: 0x0002411E
		public override float NavmeshCuttingCharacterRadius
		{
			get
			{
				return this.navmeshCuttingCharacterRadius;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00025F26 File Offset: 0x00024126
		public override bool RecalculateNormals
		{
			get
			{
				return this.recalculateNormals;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00025F2E File Offset: 0x0002412E
		public override float TileWorldSizeX
		{
			get
			{
				return this.forcedBoundsSize.x;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00025F3B File Offset: 0x0002413B
		public override float TileWorldSizeZ
		{
			get
			{
				return this.forcedBoundsSize.z;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x000059E1 File Offset: 0x00003BE1
		public override float MaxTileConnectionEdgeDistance
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00025F48 File Offset: 0x00024148
		public override bool IsInsideBounds(Vector3 point)
		{
			if (this.tiles == null || this.tiles.Length == 0 || this.sourceMesh == null)
			{
				return false;
			}
			Vector3 vector = this.transform.InverseTransform(point);
			Vector3 vector2 = this.sourceMesh.bounds.size * this.scale;
			return vector.x >= -0.0001f && vector.y >= -0.0001f && vector.z >= -0.0001f && vector.x <= vector2.x + 0.0001f && vector.y <= vector2.y + 0.0001f && vector.z <= vector2.z + 0.0001f;
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x0002600C File Offset: 0x0002420C
		public override Bounds bounds
		{
			get
			{
				if (this.sourceMesh == null)
				{
					return default(Bounds);
				}
				float4x4 float4x = this.CalculateTransform().matrix;
				return new ToWorldMatrix(new float3x3(float4x.c0.xyz, float4x.c1.xyz, float4x.c2.xyz)).ToWorld(new Bounds(Vector3.zero, this.sourceMesh.bounds.size * this.scale));
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000260A0 File Offset: 0x000242A0
		public override GraphTransform CalculateTransform()
		{
			return new GraphTransform(Matrix4x4.TRS(this.offset, Quaternion.Euler(this.rotation), Vector3.one) * Matrix4x4.TRS((this.sourceMesh != null) ? (this.sourceMesh.bounds.min * this.scale) : (this.cachedSourceMeshBoundsMin * this.scale), Quaternion.identity, Vector3.one));
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00026120 File Offset: 0x00024320
		IGraphUpdatePromise IUpdatableGraph.ScheduleGraphUpdates(List<GraphUpdateObject> graphUpdates)
		{
			return new NavMeshGraph.NavMeshGraphUpdatePromise
			{
				graph = this,
				graphUpdates = graphUpdates
			};
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00026138 File Offset: 0x00024338
		public static void UpdateArea(GraphUpdateObject o, INavmeshHolder graph)
		{
			Bounds bounds = graph.transform.InverseTransform(o.bounds);
			IntRect irect = new IntRect(Mathf.FloorToInt(bounds.min.x * 1000f), Mathf.FloorToInt(bounds.min.z * 1000f), Mathf.CeilToInt(bounds.max.x * 1000f), Mathf.CeilToInt(bounds.max.z * 1000f));
			Int3 a = new Int3(irect.xmin, 0, irect.ymin);
			Int3 b = new Int3(irect.xmin, 0, irect.ymax);
			Int3 c = new Int3(irect.xmax, 0, irect.ymin);
			Int3 d = new Int3(irect.xmax, 0, irect.ymax);
			int ymin = ((Int3)bounds.min).y;
			int ymax = ((Int3)bounds.max).y;
			graph.GetNodes(delegate(GraphNode _node)
			{
				TriangleMeshNode triangleMeshNode = _node as TriangleMeshNode;
				bool flag = false;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < 3; i++)
				{
					Int3 vertexInGraphSpace = triangleMeshNode.GetVertexInGraphSpace(i);
					if (irect.Contains(vertexInGraphSpace.x, vertexInGraphSpace.z))
					{
						flag = true;
						break;
					}
					if (vertexInGraphSpace.x < irect.xmin)
					{
						num++;
					}
					if (vertexInGraphSpace.x > irect.xmax)
					{
						num2++;
					}
					if (vertexInGraphSpace.z < irect.ymin)
					{
						num3++;
					}
					if (vertexInGraphSpace.z > irect.ymax)
					{
						num4++;
					}
				}
				if (!flag && (num == 3 || num2 == 3 || num3 == 3 || num4 == 3))
				{
					return;
				}
				for (int j = 0; j < 3; j++)
				{
					int i2 = (j > 1) ? 0 : (j + 1);
					Int3 vertexInGraphSpace2 = triangleMeshNode.GetVertexInGraphSpace(j);
					Int3 vertexInGraphSpace3 = triangleMeshNode.GetVertexInGraphSpace(i2);
					if (VectorMath.SegmentsIntersectXZ(a, b, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
					if (VectorMath.SegmentsIntersectXZ(a, c, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
					if (VectorMath.SegmentsIntersectXZ(c, d, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
					if (VectorMath.SegmentsIntersectXZ(d, b, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
				}
				if (flag || triangleMeshNode.ContainsPointInGraphSpace(a) || triangleMeshNode.ContainsPointInGraphSpace(b) || triangleMeshNode.ContainsPointInGraphSpace(c) || triangleMeshNode.ContainsPointInGraphSpace(d))
				{
					flag = true;
				}
				if (!flag)
				{
					return;
				}
				int num5 = 0;
				int num6 = 0;
				for (int k = 0; k < 3; k++)
				{
					Int3 vertexInGraphSpace4 = triangleMeshNode.GetVertexInGraphSpace(k);
					if (vertexInGraphSpace4.y < ymin)
					{
						num6++;
					}
					if (vertexInGraphSpace4.y > ymax)
					{
						num5++;
					}
				}
				if (num6 == 3 || num5 == 3)
				{
					return;
				}
				o.WillUpdateNode(triangleMeshNode);
				o.Apply(triangleMeshNode);
			});
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00026298 File Offset: 0x00024498
		protected override IGraphUpdatePromise ScanInternal(bool async)
		{
			return new NavMeshGraph.NavMeshGraphScanPromise
			{
				graph = this
			};
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x000262A6 File Offset: 0x000244A6
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			if (ctx.meta.version < AstarSerializer.V4_3_74)
			{
				this.navmeshCuttingCharacterRadius = 0f;
			}
			base.PostDeserialization(ctx);
		}

		// Token: 0x040004A6 RID: 1190
		[JsonMember]
		public Mesh sourceMesh;

		// Token: 0x040004A7 RID: 1191
		[JsonMember]
		public Vector3 offset;

		// Token: 0x040004A8 RID: 1192
		[JsonMember]
		public Vector3 rotation;

		// Token: 0x040004A9 RID: 1193
		[JsonMember]
		public float scale = 1f;

		// Token: 0x040004AA RID: 1194
		[JsonMember]
		public bool recalculateNormals = true;

		// Token: 0x040004AB RID: 1195
		[JsonMember]
		private Vector3 cachedSourceMeshBoundsMin;

		// Token: 0x040004AC RID: 1196
		[JsonMember]
		public float navmeshCuttingCharacterRadius = 0.5f;

		// Token: 0x020000DE RID: 222
		private class NavMeshGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x06000717 RID: 1815 RVA: 0x000262F8 File Offset: 0x000244F8
			public void Apply(IGraphUpdateContext ctx)
			{
				for (int i = 0; i < this.graphUpdates.Count; i++)
				{
					GraphUpdateObject graphUpdateObject = this.graphUpdates[i];
					NavMeshGraph.UpdateArea(graphUpdateObject, this.graph);
					ctx.DirtyBounds(graphUpdateObject.bounds);
				}
			}

			// Token: 0x040004AD RID: 1197
			public NavMeshGraph graph;

			// Token: 0x040004AE RID: 1198
			public List<GraphUpdateObject> graphUpdates;
		}

		// Token: 0x020000DF RID: 223
		private class NavMeshGraphScanPromise : IGraphUpdatePromise
		{
			// Token: 0x06000719 RID: 1817 RVA: 0x00026340 File Offset: 0x00024540
			public IEnumerator<JobHandle> Prepare()
			{
				Mesh sourceMesh = this.graph.sourceMesh;
				this.graph.cachedSourceMeshBoundsMin = ((sourceMesh != null) ? sourceMesh.bounds.min : Vector3.zero);
				this.transform = this.graph.CalculateTransform();
				if (sourceMesh == null)
				{
					this.emptyGraph = true;
					yield break;
				}
				if (!sourceMesh.isReadable)
				{
					Debug.LogError("The source mesh " + sourceMesh.name + " is not readable. Enable Read/Write in the mesh's import settings.", sourceMesh);
					this.emptyGraph = true;
					yield break;
				}
				Mesh.MeshDataArray meshData = Mesh.AcquireReadOnlyMeshData(sourceMesh);
				NativeArray<Vector3> vertices;
				NativeArray<int> indices;
				MeshUtility.GetMeshData(meshData, 0, out vertices, out indices);
				meshData.Dispose();
				float scale = this.graph.scale;
				Matrix4x4 meshToGraph = Matrix4x4.TRS(-sourceMesh.bounds.min * scale, Quaternion.identity, Vector3.one * scale);
				Promise<TileBuilder.TileBuilderOutput> promise = JobBuildTileMeshFromVertices.Schedule(vertices, indices, meshToGraph, this.graph.RecalculateNormals);
				this.forcedBoundsSize = sourceMesh.bounds.size * scale;
				this.tileRect = new IntRect(0, 0, 0, 0);
				this.tiles = new NavmeshTile[this.tileRect.Area];
				GCHandle tilesGCHandle = GCHandle.Alloc(this.tiles);
				TileLayout tileLayout = new TileLayout(new Bounds(this.transform.Transform(this.forcedBoundsSize * 0.5f), this.forcedBoundsSize), Quaternion.Euler(this.graph.rotation), 0.001f, 0, false);
				this.cutSettings = new NavmeshUpdates.NavmeshUpdateSettings(this.graph, tileLayout);
				Promise<TileCutter.TileCutterOutput> cutPromise = RecastBuilder.CutTiles(this.graph, this.cutSettings.clipperLookup, tileLayout).Schedule(promise);
				NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe> tileNodeConnections = new NativeArray<JobCalculateTriangleConnections.TileNodeConnectionsUnsafe>(this.tiles.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				TileCutter.TileCutterOutput value = cutPromise.GetValue();
				TileBuilder.TileBuilderOutput value2 = promise.GetValue();
				NativeArray<TileMesh.TileMeshUnsafe> tileMeshes;
				if (value.tileMeshes.tileMeshes.IsCreated)
				{
					tileMeshes = value.tileMeshes.tileMeshes;
				}
				else
				{
					tileMeshes = value2.tileMeshes.tileMeshes;
				}
				JobHandle job = new JobCalculateTriangleConnections
				{
					tileMeshes = tileMeshes,
					nodeConnections = tileNodeConnections
				}.Schedule(cutPromise.handle);
				JobHandle job2 = new JobCreateTiles
				{
					preCutTileMeshes = (value.tileMeshes.tileMeshes.IsCreated ? value2.tileMeshes.tileMeshes : default(NativeArray<TileMesh.TileMeshUnsafe>)),
					tileMeshes = tileMeshes,
					tiles = tilesGCHandle,
					tileRect = this.tileRect,
					graphTileCount = new Vector2Int(this.tileRect.Width, this.tileRect.Height),
					graphIndex = this.graph.graphIndex,
					initialPenalty = this.graph.initialPenalty,
					recalculateNormals = this.graph.recalculateNormals,
					graphToWorldSpace = this.transform.matrix,
					tileWorldSize = tileLayout.TileWorldSize
				}.Schedule(cutPromise.handle);
				JobHandle jobHandle = new JobWriteNodeConnections
				{
					tiles = tilesGCHandle,
					nodeConnections = tileNodeConnections
				}.Schedule(JobHandle.CombineDependencies(job2, job));
				yield return jobHandle;
				promise.Complete().Dispose();
				cutPromise.Complete().Dispose();
				tileNodeConnections.Dispose();
				vertices.Dispose();
				indices.Dispose();
				tilesGCHandle.Free();
				yield break;
			}

			// Token: 0x0600071A RID: 1818 RVA: 0x00026350 File Offset: 0x00024550
			public void Apply(IGraphUpdateContext ctx)
			{
				if (this.emptyGraph)
				{
					this.graph.forcedBoundsSize = Vector3.zero;
					this.graph.transform = this.transform;
					this.graph.tileZCount = (this.graph.tileXCount = 1);
					TriangleMeshNode.SetNavmeshHolder(AstarPath.active.data.GetGraphIndex(this.graph), this.graph);
					this.graph.FillWithEmptyTiles();
					this.graph.navmeshUpdateData.Dispose();
					return;
				}
				this.graph.DestroyAllNodes();
				for (int i = 0; i < this.tiles.Length; i++)
				{
					AstarPath active = AstarPath.active;
					GraphNode[] nodes = this.tiles[i].nodes;
					active.InitializeNodes(nodes);
				}
				this.graph.forcedBoundsSize = this.forcedBoundsSize;
				this.graph.transform = this.transform;
				this.graph.tileXCount = this.tileRect.Width;
				this.graph.tileZCount = this.tileRect.Height;
				this.graph.tiles = this.tiles;
				TriangleMeshNode.SetNavmeshHolder(this.graph.active.data.GetGraphIndex(this.graph), this.graph);
				this.cutSettings.AttachToGraph();
				if (this.graph.OnRecalculatedTiles != null)
				{
					this.graph.OnRecalculatedTiles(this.tiles.Clone() as NavmeshTile[]);
				}
			}

			// Token: 0x040004AF RID: 1199
			public NavMeshGraph graph;

			// Token: 0x040004B0 RID: 1200
			private bool emptyGraph;

			// Token: 0x040004B1 RID: 1201
			private GraphTransform transform;

			// Token: 0x040004B2 RID: 1202
			private NavmeshTile[] tiles;

			// Token: 0x040004B3 RID: 1203
			private Vector3 forcedBoundsSize;

			// Token: 0x040004B4 RID: 1204
			private IntRect tileRect;

			// Token: 0x040004B5 RID: 1205
			private NavmeshUpdates.NavmeshUpdateSettings cutSettings;
		}
	}
}
