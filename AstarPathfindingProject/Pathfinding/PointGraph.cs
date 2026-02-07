using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000F3 RID: 243
	[JsonOptIn]
	[Preserve]
	public class PointGraph : NavGraph, IUpdatableGraph
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00029F33 File Offset: 0x00028133
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x00029F3B File Offset: 0x0002813B
		public int nodeCount { get; protected set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x00029F44 File Offset: 0x00028144
		public override bool isScanned
		{
			get
			{
				return this.nodes != null;
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00029F4F File Offset: 0x0002814F
		public override int CountNodes()
		{
			return this.nodeCount;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00029F58 File Offset: 0x00028158
		public override void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			int nodeCount = this.nodeCount;
			for (int i = 0; i < nodeCount; i++)
			{
				action(this.nodes[i]);
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00029F90 File Offset: 0x00028190
		public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, float maxDistanceSqr)
		{
			if (this.nodes == null)
			{
				return NNInfo.Empty;
			}
			Int3 @int = (Int3)position;
			if (this.lookupTree != null != this.optimizeForSparseGraph)
			{
				Debug.LogWarning("Lookup tree is not in the correct state. Have you changed PointGraph.optimizeForSparseGraph without calling RebuildNodeLookup?");
			}
			if (this.lookupTree != null)
			{
				if (this.nearestNodeDistanceMode == PointGraph.NodeDistanceMode.Node)
				{
					float distanceCostSqr = maxDistanceSqr;
					GraphNode nearest = this.lookupTree.GetNearest(@int, constraint, ref distanceCostSqr);
					if (nearest != null)
					{
						return new NNInfo(nearest, (Vector3)nearest.position, distanceCostSqr);
					}
					return NNInfo.Empty;
				}
				else
				{
					GraphNode nearestConnection = this.lookupTree.GetNearestConnection(@int, constraint, this.maximumConnectionLength);
					if (nearestConnection != null)
					{
						return this.FindClosestConnectionPoint(nearestConnection as PointNode, position, maxDistanceSqr);
					}
					return NNInfo.Empty;
				}
			}
			else
			{
				PointNode pointNode = null;
				long num = AstarMath.SaturatingConvertFloatToLong(maxDistanceSqr * 1000f * 1000f);
				for (int i = 0; i < this.nodeCount; i++)
				{
					PointNode pointNode2 = this.nodes[i];
					long sqrMagnitudeLong = (@int - pointNode2.position).sqrMagnitudeLong;
					if (sqrMagnitudeLong < num && (constraint == null || constraint.Suitable(pointNode2)))
					{
						num = sqrMagnitudeLong;
						pointNode = pointNode2;
					}
				}
				if (1.0000001E-06f * (float)num >= maxDistanceSqr || pointNode == null)
				{
					return NNInfo.Empty;
				}
				return new NNInfo(pointNode, (Vector3)pointNode.position, 1.0000001E-06f * (float)num);
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0002A0D0 File Offset: 0x000282D0
		private NNInfo FindClosestConnectionPoint(PointNode node, Vector3 position, float maxDistanceSqr)
		{
			Vector3 position2 = (Vector3)node.position;
			Connection[] connections = node.connections;
			Vector3 vector = (Vector3)node.position;
			if (connections != null)
			{
				for (int i = 0; i < connections.Length; i++)
				{
					Vector3 lineEnd = ((Vector3)connections[i].node.position + vector) * 0.5f;
					Vector3 vector2 = VectorMath.ClosestPointOnSegment(vector, lineEnd, position);
					float sqrMagnitude = (vector2 - position).sqrMagnitude;
					if (sqrMagnitude < maxDistanceSqr)
					{
						maxDistanceSqr = sqrMagnitude;
						position2 = vector2;
					}
				}
			}
			return new NNInfo(node, position2, maxDistanceSqr);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002A168 File Offset: 0x00028368
		public override NNInfo RandomPointOnSurface(NNConstraint nnConstraint = null, bool highQuality = true)
		{
			if (!this.isScanned || this.nodes.Length == 0)
			{
				return NNInfo.Empty;
			}
			for (int i = 0; i < 10; i++)
			{
				PointNode pointNode = this.nodes[UnityEngine.Random.Range(0, this.nodes.Length)];
				if (pointNode != null && (nnConstraint == null || nnConstraint.Suitable(pointNode)))
				{
					return new NNInfo(pointNode, pointNode.RandomPointOnSurface(), 0f);
				}
			}
			return base.RandomPointOnSurface(nnConstraint, highQuality);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002A1D8 File Offset: 0x000283D8
		public PointNode AddNode(Int3 position)
		{
			return this.AddNode<PointNode>(new PointNode(this.active), position);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002A1EC File Offset: 0x000283EC
		public T AddNode<T>(T node, Int3 position) where T : PointNode
		{
			base.AssertSafeToUpdateGraph();
			if (this.nodes == null || this.nodeCount == this.nodes.Length)
			{
				PointNode[] array = new PointNode[(this.nodes != null) ? Math.Max(this.nodes.Length + 4, this.nodes.Length * 2) : 4];
				if (this.nodes != null)
				{
					this.nodes.CopyTo(array, 0);
				}
				this.nodes = array;
				this.RebuildNodeLookup();
			}
			node.position = position;
			node.GraphIndex = this.graphIndex;
			node.Walkable = true;
			this.nodes[this.nodeCount] = node;
			int nodeCount = this.nodeCount;
			this.nodeCount = nodeCount + 1;
			if (this.lookupTree != null)
			{
				this.lookupTree.Add(node);
			}
			return node;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002A2CC File Offset: 0x000284CC
		public void RemoveNode(PointNode node)
		{
			base.AssertSafeToUpdateGraph();
			if (node.Destroyed)
			{
				throw new ArgumentException("The node has already been destroyed");
			}
			if (node.GraphIndex != this.graphIndex)
			{
				throw new ArgumentException("The node does not belong to this graph");
			}
			if (!this.isScanned)
			{
				throw new InvalidOperationException("Graph contains no nodes");
			}
			int num = Array.IndexOf<PointNode>(this.nodes, node);
			if (num == -1)
			{
				throw new ArgumentException("The node is not in the graph");
			}
			int nodeCount = this.nodeCount;
			this.nodeCount = nodeCount - 1;
			this.nodes[num] = this.nodes[this.nodeCount];
			this.nodes[this.nodeCount] = null;
			node.Destroy();
			if (this.lookupTree != null)
			{
				this.lookupTree.Remove(node);
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0002A388 File Offset: 0x00028588
		protected static int CountChildren(Transform tr)
		{
			int num = 0;
			foreach (object obj in tr)
			{
				Transform tr2 = (Transform)obj;
				num++;
				num += PointGraph.CountChildren(tr2);
			}
			return num;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002A3E8 File Offset: 0x000285E8
		protected static void AddChildren(PointNode[] nodes, ref int c, Transform tr)
		{
			foreach (object obj in tr)
			{
				Transform transform = (Transform)obj;
				nodes[c].position = (Int3)transform.position;
				nodes[c].Walkable = true;
				nodes[c].gameObject = transform.gameObject;
				c++;
				PointGraph.AddChildren(nodes, ref c, transform);
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002A470 File Offset: 0x00028670
		public void RebuildNodeLookup()
		{
			this.lookupTree = PointGraph.BuildNodeLookup(this.nodes, this.nodeCount, this.optimizeForSparseGraph);
			this.RebuildConnectionDistanceLookup();
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002A498 File Offset: 0x00028698
		private static PointKDTree BuildNodeLookup(PointNode[] nodes, int nodeCount, bool optimizeForSparseGraph)
		{
			if (optimizeForSparseGraph && nodes != null)
			{
				PointKDTree pointKDTree = new PointKDTree();
				pointKDTree.Rebuild(nodes, 0, nodeCount);
				return pointKDTree;
			}
			return null;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0002A4BD File Offset: 0x000286BD
		public void RebuildConnectionDistanceLookup()
		{
			if (this.nearestNodeDistanceMode == PointGraph.NodeDistanceMode.Connection)
			{
				this.maximumConnectionLength = PointGraph.LongestConnectionLength(this.nodes, this.nodeCount);
				return;
			}
			this.maximumConnectionLength = 0L;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002A4E8 File Offset: 0x000286E8
		private static long LongestConnectionLength(PointNode[] nodes, int nodeCount)
		{
			long num = 0L;
			for (int i = 0; i < nodeCount; i++)
			{
				PointNode pointNode = nodes[i];
				Connection[] connections = pointNode.connections;
				if (connections != null)
				{
					for (int j = 0; j < connections.Length; j++)
					{
						long sqrMagnitudeLong = (pointNode.position - connections[j].node.position).sqrMagnitudeLong;
						num = Math.Max(num, sqrMagnitudeLong);
					}
				}
			}
			return num;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002A556 File Offset: 0x00028756
		public void RegisterConnectionLength(long sqrLength)
		{
			this.maximumConnectionLength = Math.Max(this.maximumConnectionLength, sqrLength);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0002A56C File Offset: 0x0002876C
		protected virtual PointNode[] CreateNodes(int count)
		{
			PointNode[] array = new PointNode[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new PointNode(this.active);
			}
			return array;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0002A59B File Offset: 0x0002879B
		protected override void DestroyAllNodes()
		{
			base.DestroyAllNodes();
			this.nodes = null;
			this.lookupTree = null;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0002A5B1 File Offset: 0x000287B1
		protected override IGraphUpdatePromise ScanInternal()
		{
			return new PointGraph.PointGraphScanPromise
			{
				graph = this
			};
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0002A5C0 File Offset: 0x000287C0
		public void ConnectNodes()
		{
			base.AssertSafeToUpdateGraph();
			IEnumerator<float> enumerator = PointGraph.ConnectNodesAsync(this.nodes, this.nodeCount, this.lookupTree, this.maxDistance, this.limits, this).GetEnumerator();
			while (enumerator.MoveNext())
			{
			}
			this.RebuildConnectionDistanceLookup();
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0002A60B File Offset: 0x0002880B
		private static IEnumerable<float> ConnectNodesAsync(PointNode[] nodes, int nodeCount, PointKDTree lookupTree, float maxDistance, Vector3 limits, PointGraph graph)
		{
			if (maxDistance >= 0f)
			{
				List<Connection> connections = new List<Connection>();
				List<GraphNode> candidateConnections = new List<GraphNode>();
				long maxSquaredRange;
				if (maxDistance == 0f && (limits.x == 0f || limits.y == 0f || limits.z == 0f))
				{
					maxSquaredRange = long.MaxValue;
				}
				else
				{
					maxSquaredRange = (long)(Mathf.Max(limits.x, Mathf.Max(limits.y, Mathf.Max(limits.z, maxDistance))) * 1000f) + 1L;
					maxSquaredRange *= maxSquaredRange;
				}
				int num3;
				for (int i = 0; i < nodeCount; i = num3 + 1)
				{
					if (i % 512 == 0)
					{
						yield return (float)i / (float)nodeCount;
					}
					connections.Clear();
					PointNode pointNode = nodes[i];
					if (lookupTree != null)
					{
						candidateConnections.Clear();
						lookupTree.GetInRange(pointNode.position, maxSquaredRange, candidateConnections);
						for (int j = 0; j < candidateConnections.Count; j++)
						{
							PointNode pointNode2 = candidateConnections[j] as PointNode;
							float num;
							if (pointNode2 != pointNode && graph.IsValidConnection(pointNode, pointNode2, out num))
							{
								connections.Add(new Connection(pointNode2, (uint)Mathf.RoundToInt(num * 1000f), true, true));
							}
						}
					}
					else
					{
						for (int k = 0; k < nodeCount; k++)
						{
							if (i != k)
							{
								PointNode pointNode3 = nodes[k];
								float num2;
								if (graph.IsValidConnection(pointNode, pointNode3, out num2))
								{
									connections.Add(new Connection(pointNode3, (uint)Mathf.RoundToInt(num2 * 1000f), true, true));
								}
							}
						}
					}
					pointNode.connections = connections.ToArray();
					pointNode.SetConnectivityDirty();
					num3 = i;
				}
				connections = null;
				candidateConnections = null;
			}
			yield break;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0002A640 File Offset: 0x00028840
		public virtual bool IsValidConnection(GraphNode a, GraphNode b, out float dist)
		{
			dist = 0f;
			if (!a.Walkable || !b.Walkable)
			{
				return false;
			}
			Vector3 vector = (Vector3)(b.position - a.position);
			if ((!Mathf.Approximately(this.limits.x, 0f) && Mathf.Abs(vector.x) > this.limits.x) || (!Mathf.Approximately(this.limits.y, 0f) && Mathf.Abs(vector.y) > this.limits.y) || (!Mathf.Approximately(this.limits.z, 0f) && Mathf.Abs(vector.z) > this.limits.z))
			{
				return false;
			}
			dist = vector.magnitude;
			if (this.maxDistance != 0f && dist >= this.maxDistance)
			{
				return false;
			}
			if (!this.raycast)
			{
				return true;
			}
			Ray ray = new Ray((Vector3)a.position, vector);
			Ray ray2 = new Ray((Vector3)b.position, -vector);
			if (this.use2DPhysics)
			{
				if (this.thickRaycast)
				{
					return !Physics2D.CircleCast(ray.origin, this.thickRaycastRadius, ray.direction, dist, this.mask) && !Physics2D.CircleCast(ray2.origin, this.thickRaycastRadius, ray2.direction, dist, this.mask);
				}
				return !Physics2D.Linecast((Vector3)a.position, (Vector3)b.position, this.mask) && !Physics2D.Linecast((Vector3)b.position, (Vector3)a.position, this.mask);
			}
			else
			{
				if (this.thickRaycast)
				{
					return !Physics.SphereCast(ray, this.thickRaycastRadius, dist, this.mask) && !Physics.SphereCast(ray2, this.thickRaycastRadius, dist, this.mask);
				}
				return !Physics.Linecast((Vector3)a.position, (Vector3)b.position, this.mask) && !Physics.Linecast((Vector3)b.position, (Vector3)a.position, this.mask);
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0002A8F5 File Offset: 0x00028AF5
		IGraphUpdatePromise IUpdatableGraph.ScheduleGraphUpdates(List<GraphUpdateObject> graphUpdates)
		{
			if (!this.isScanned)
			{
				return null;
			}
			return new PointGraph.PointGraphUpdatePromise
			{
				graph = this,
				graphUpdates = graphUpdates
			};
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0002A914 File Offset: 0x00028B14
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			this.RebuildNodeLookup();
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0002A91C File Offset: 0x00028B1C
		public override void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			base.RelocateNodes(deltaMatrix);
			this.RebuildNodeLookup();
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0002A92C File Offset: 0x00028B2C
		protected override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			if (this.nodes == null)
			{
				ctx.writer.Write(-1);
			}
			ctx.writer.Write(this.nodeCount);
			for (int i = 0; i < this.nodeCount; i++)
			{
				if (this.nodes[i] == null)
				{
					ctx.writer.Write(-1);
				}
				else
				{
					ctx.writer.Write(0);
					this.nodes[i].SerializeNode(ctx);
				}
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0002A9A4 File Offset: 0x00028BA4
		protected override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = null;
				return;
			}
			this.nodes = new PointNode[num];
			this.nodeCount = num;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					this.nodes[i] = new PointNode(this.active);
					this.nodes[i].DeserializeNode(ctx);
				}
			}
		}

		// Token: 0x040004FE RID: 1278
		[JsonMember]
		public Transform root;

		// Token: 0x040004FF RID: 1279
		[JsonMember]
		public string searchTag;

		// Token: 0x04000500 RID: 1280
		[JsonMember]
		public float maxDistance;

		// Token: 0x04000501 RID: 1281
		[JsonMember]
		public Vector3 limits;

		// Token: 0x04000502 RID: 1282
		[JsonMember]
		public bool raycast = true;

		// Token: 0x04000503 RID: 1283
		[JsonMember]
		public bool use2DPhysics;

		// Token: 0x04000504 RID: 1284
		[JsonMember]
		public bool thickRaycast;

		// Token: 0x04000505 RID: 1285
		[JsonMember]
		public float thickRaycastRadius = 1f;

		// Token: 0x04000506 RID: 1286
		[JsonMember]
		public bool recursive = true;

		// Token: 0x04000507 RID: 1287
		[JsonMember]
		public LayerMask mask;

		// Token: 0x04000508 RID: 1288
		[JsonMember]
		public bool optimizeForSparseGraph;

		// Token: 0x04000509 RID: 1289
		private PointKDTree lookupTree = new PointKDTree();

		// Token: 0x0400050A RID: 1290
		private long maximumConnectionLength;

		// Token: 0x0400050B RID: 1291
		public PointNode[] nodes;

		// Token: 0x0400050C RID: 1292
		[JsonMember]
		public PointGraph.NodeDistanceMode nearestNodeDistanceMode;

		// Token: 0x020000F4 RID: 244
		public enum NodeDistanceMode
		{
			// Token: 0x0400050F RID: 1295
			Node,
			// Token: 0x04000510 RID: 1296
			Connection
		}

		// Token: 0x020000F5 RID: 245
		private class PointGraphScanPromise : IGraphUpdatePromise
		{
			// Token: 0x06000817 RID: 2071 RVA: 0x0002AA4A File Offset: 0x00028C4A
			public IEnumerator<JobHandle> Prepare()
			{
				Transform root = this.graph.root;
				if (root == null)
				{
					GameObject[] array = (this.graph.searchTag != null) ? GameObject.FindGameObjectsWithTag(this.graph.searchTag) : null;
					if (array == null)
					{
						this.nodes = new PointNode[0];
					}
					else
					{
						this.nodes = this.graph.CreateNodes(array.Length);
						for (int i = 0; i < array.Length; i++)
						{
							PointNode pointNode = this.nodes[i];
							pointNode.position = (Int3)array[i].transform.position;
							pointNode.Walkable = true;
							pointNode.gameObject = array[i].gameObject;
						}
					}
				}
				else
				{
					if (!this.graph.recursive)
					{
						int childCount = root.childCount;
						this.nodes = this.graph.CreateNodes(childCount);
						int num = 0;
						using (IEnumerator enumerator = root.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Transform transform = (Transform)obj;
								PointNode pointNode2 = this.nodes[num];
								pointNode2.position = (Int3)transform.position;
								pointNode2.Walkable = true;
								pointNode2.gameObject = transform.gameObject;
								num++;
							}
							goto IL_1A9;
						}
					}
					int count = PointGraph.CountChildren(root);
					this.nodes = this.graph.CreateNodes(count);
					int num2 = 0;
					PointGraph.AddChildren(this.nodes, ref num2, root);
				}
				IL_1A9:
				JobHandle jobHandle = default(JobHandle);
				this.lookupTree = PointGraph.BuildNodeLookup(this.nodes, this.nodes.Length, this.graph.optimizeForSparseGraph);
				foreach (float num3 in PointGraph.ConnectNodesAsync(this.nodes, this.nodes.Length, this.lookupTree, this.graph.maxDistance, this.graph.limits, this.graph))
				{
					jobHandle = default(JobHandle);
				}
				IEnumerator<float> enumerator2 = null;
				yield break;
				yield break;
			}

			// Token: 0x06000818 RID: 2072 RVA: 0x0002AA5C File Offset: 0x00028C5C
			public void Apply(IGraphUpdateContext ctx)
			{
				this.graph.DestroyAllNodes();
				this.graph.lookupTree = this.lookupTree;
				this.graph.nodes = this.nodes;
				this.graph.nodeCount = this.nodes.Length;
				this.graph.maximumConnectionLength = ((this.graph.nearestNodeDistanceMode == PointGraph.NodeDistanceMode.Connection) ? PointGraph.LongestConnectionLength(this.nodes, this.nodes.Length) : 0L);
			}

			// Token: 0x04000511 RID: 1297
			public PointGraph graph;

			// Token: 0x04000512 RID: 1298
			private PointKDTree lookupTree;

			// Token: 0x04000513 RID: 1299
			private PointNode[] nodes;
		}

		// Token: 0x020000F7 RID: 247
		private class PointGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x06000821 RID: 2081 RVA: 0x0002AE20 File Offset: 0x00029020
			public void Apply(IGraphUpdateContext ctx)
			{
				PointNode[] nodes = this.graph.nodes;
				for (int i = 0; i < this.graphUpdates.Count; i++)
				{
					GraphUpdateObject graphUpdateObject = this.graphUpdates[i];
					for (int j = 0; j < this.graph.nodeCount; j++)
					{
						PointNode pointNode = nodes[j];
						if (graphUpdateObject.bounds.Contains((Vector3)pointNode.position))
						{
							graphUpdateObject.WillUpdateNode(pointNode);
							graphUpdateObject.Apply(pointNode);
						}
					}
					if (graphUpdateObject.updatePhysics)
					{
						Bounds bounds = graphUpdateObject.bounds;
						if (this.graph.thickRaycast)
						{
							bounds.Expand(this.graph.thickRaycastRadius * 2f);
						}
						List<Connection> list = ListPool<Connection>.Claim();
						for (int k = 0; k < this.graph.nodeCount; k++)
						{
							PointNode pointNode2 = this.graph.nodes[k];
							Vector3 a = (Vector3)pointNode2.position;
							List<Connection> list2 = null;
							for (int l = 0; l < this.graph.nodeCount; l++)
							{
								if (l != k)
								{
									Vector3 b = (Vector3)nodes[l].position;
									if (VectorMath.SegmentIntersectsBounds(bounds, a, b))
									{
										PointNode pointNode3 = nodes[l];
										bool flag = pointNode2.ContainsOutgoingConnection(pointNode3);
										float num;
										bool flag2 = this.graph.IsValidConnection(pointNode2, pointNode3, out num);
										if (list2 == null && flag != flag2)
										{
											list.Clear();
											list2 = list;
											if (pointNode2.connections != null)
											{
												list2.AddRange(pointNode2.connections);
											}
										}
										if (!flag && flag2)
										{
											uint cost = (uint)Mathf.RoundToInt(num * 1000f);
											list2.Add(new Connection(pointNode3, cost, true, true));
											this.graph.RegisterConnectionLength((pointNode3.position - pointNode2.position).sqrMagnitudeLong);
										}
										else if (flag && !flag2)
										{
											for (int m = 0; m < list2.Count; m++)
											{
												if (list2[m].node == pointNode3)
												{
													list2.RemoveAt(m);
													break;
												}
											}
										}
									}
								}
							}
							if (list2 != null)
							{
								pointNode2.connections = list2.ToArray();
								pointNode2.SetConnectivityDirty();
							}
						}
						ListPool<Connection>.Release(ref list);
						ctx.DirtyBounds(graphUpdateObject.bounds);
					}
				}
				ListPool<GraphUpdateObject>.Release(ref this.graphUpdates);
			}

			// Token: 0x04000518 RID: 1304
			public PointGraph graph;

			// Token: 0x04000519 RID: 1305
			public List<GraphUpdateObject> graphUpdates;
		}
	}
}
