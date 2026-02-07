using System;
using Pathfinding.Drawing;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000CE RID: 206
	public abstract class NavGraph : IGraphInternals
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x000226FA File Offset: 0x000208FA
		internal bool exists
		{
			get
			{
				return this.active != null;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000682 RID: 1666
		public abstract bool isScanned { get; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0001797A File Offset: 0x00015B7A
		public virtual bool persistent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x0001797A File Offset: 0x00015B7A
		public virtual bool showInInspector
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00022708 File Offset: 0x00020908
		public virtual Bounds bounds
		{
			get
			{
				return new Bounds(Vector3.zero, Vector3.positiveInfinity);
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0002271C File Offset: 0x0002091C
		public virtual int CountNodes()
		{
			int count = 0;
			this.GetNodes(delegate(GraphNode _)
			{
				int count = count;
				count++;
			});
			return count;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00022750 File Offset: 0x00020950
		public void GetNodes(Func<GraphNode, bool> action)
		{
			bool cont = true;
			this.GetNodes(delegate(GraphNode node)
			{
				if (cont)
				{
					cont &= action(node);
				}
			});
		}

		// Token: 0x06000688 RID: 1672
		public abstract void GetNodes(Action<GraphNode> action);

		// Token: 0x06000689 RID: 1673 RVA: 0x00022784 File Offset: 0x00020984
		public virtual bool IsPointOnNavmesh(Vector3 position)
		{
			NNInfo nearest = this.GetNearest(position, AstarPath.NNConstraintClosestAsSeenFromAbove, 0.0001f);
			return nearest.node != null && nearest.node.Walkable && nearest.distanceCostSqr < 0.0001f;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001797A File Offset: 0x00015B7A
		public virtual bool IsInsideBounds(Vector3 point)
		{
			return true;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000227C7 File Offset: 0x000209C7
		protected void AssertSafeToUpdateGraph()
		{
			if (!this.active.IsAnyWorkItemInProgress && !this.active.isScanning)
			{
				throw new Exception("Trying to update graphs when it is not safe to do so. Graph updates must be done inside a work item or when a graph is being scanned. See AstarPath.AddWorkItem");
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000227EE File Offset: 0x000209EE
		protected void DirtyBounds(Bounds bounds)
		{
			this.active.DirtyBounds(bounds);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000227FC File Offset: 0x000209FC
		public virtual void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			this.AssertSafeToUpdateGraph();
			this.GetNodes(delegate(GraphNode node)
			{
				node.position = (Int3)deltaMatrix.MultiplyPoint((Vector3)node.position);
			});
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000059E1 File Offset: 0x00003BE1
		public virtual float NearestNodeDistanceSqrLowerBound(Vector3 position, NNConstraint constraint = null)
		{
			return 0f;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00022830 File Offset: 0x00020A30
		public NNInfo GetNearest(Vector3 position, NNConstraint constraint = null)
		{
			float maxDistanceSqr = (constraint == null || constraint.constrainDistance) ? this.active.maxNearestNodeDistanceSqr : float.PositiveInfinity;
			return this.GetNearest(position, constraint, maxDistanceSqr);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00022864 File Offset: 0x00020A64
		public virtual NNInfo GetNearest(Vector3 position, NNConstraint constraint, float maxDistanceSqr)
		{
			GraphNode minNode = null;
			this.GetNodes(delegate(GraphNode node)
			{
				float sqrMagnitude = (position - (Vector3)node.position).sqrMagnitude;
				if (sqrMagnitude < maxDistanceSqr && (constraint == null || constraint.Suitable(node)))
				{
					maxDistanceSqr = sqrMagnitude;
					minNode = node;
				}
			});
			if (minNode == null)
			{
				return NNInfo.Empty;
			}
			return new NNInfo(minNode, (Vector3)minNode.position, maxDistanceSqr);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000228D4 File Offset: 0x00020AD4
		[Obsolete("Use GetNearest instead")]
		public NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
		{
			return this.GetNearest(position, constraint);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000228E0 File Offset: 0x00020AE0
		public virtual NNInfo RandomPointOnSurface(NNConstraint nnConstraint, bool highQuality = true)
		{
			GraphNode bestNode = null;
			float weight = 0f;
			this.GetNodes(delegate(GraphNode node)
			{
				if (nnConstraint == null || nnConstraint.Suitable(node))
				{
					float num = node.SurfaceArea();
					if (num <= 0f)
					{
						num = 0.001f;
					}
					weight += num;
					if (bestNode == null || UnityEngine.Random.value < num / weight)
					{
						bestNode = node;
					}
				}
			});
			if (bestNode == null)
			{
				return NNInfo.Empty;
			}
			return new NNInfo(bestNode, bestNode.RandomPointOnSurface(), 0f);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00022947 File Offset: 0x00020B47
		protected virtual void OnDestroy()
		{
			this.DestroyAllNodes();
			this.DisposeUnmanagedData();
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x000035CE File Offset: 0x000017CE
		protected virtual void DisposeUnmanagedData()
		{
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00022955 File Offset: 0x00020B55
		protected virtual void DestroyAllNodes()
		{
			this.GetNodes(delegate(GraphNode node)
			{
				node.Destroy();
			});
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000035D8 File Offset: 0x000017D8
		public virtual IGraphSnapshot Snapshot(Bounds bounds)
		{
			return null;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0002297C File Offset: 0x00020B7C
		public void Scan()
		{
			this.active.Scan(this);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0002298A File Offset: 0x00020B8A
		protected virtual IGraphUpdatePromise ScanInternal()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00022991 File Offset: 0x00020B91
		protected virtual IGraphUpdatePromise ScanInternal(bool async)
		{
			return this.ScanInternal();
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000035CE File Offset: 0x000017CE
		protected virtual void SerializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x000035CE File Offset: 0x000017CE
		protected virtual void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000035CE File Offset: 0x000017CE
		protected virtual void PostDeserialization(GraphSerializationContext ctx)
		{
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0002299C File Offset: 0x00020B9C
		public virtual void OnDrawGizmos(DrawingData gizmos, bool drawNodes, RedrawScope redrawScope)
		{
			if (!drawNodes)
			{
				return;
			}
			NodeHasher hasher = new NodeHasher(this.active);
			this.GetNodes(delegate(GraphNode node)
			{
				hasher.HashNode(node);
			});
			if (!gizmos.Draw(hasher, redrawScope))
			{
				using (GraphGizmoHelper gizmoHelper = GraphGizmoHelper.GetGizmoHelper(gizmos, this.active, hasher, redrawScope))
				{
					if (gizmoHelper.showSearchTree)
					{
						gizmoHelper.builder.PushLineWidth(2f, true);
					}
					this.GetNodes(new Action<GraphNode>(gizmoHelper.DrawConnections));
					if (gizmoHelper.showSearchTree)
					{
						gizmoHelper.builder.PopLineWidth();
					}
				}
			}
			if (this.active.showUnwalkableNodes)
			{
				this.DrawUnwalkableNodes(gizmos, this.active.unwalkableNodeDebugSize, redrawScope);
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00022A80 File Offset: 0x00020C80
		protected void DrawUnwalkableNodes(DrawingData gizmos, float size, RedrawScope redrawScope)
		{
			DrawingData.Hasher hasher = default(DrawingData.Hasher);
			hasher.Add<NavGraph>(this);
			this.GetNodes(delegate(GraphNode node)
			{
				hasher.Add<bool>(node.Walkable);
				if (!node.Walkable)
				{
					hasher.Add<Int3>(node.position);
				}
			});
			if (!gizmos.Draw(hasher, redrawScope))
			{
				using (CommandBuilder builder = gizmos.GetBuilder(hasher, default(RedrawScope), false))
				{
					using (builder.WithColor(AstarColor.UnwalkableNode))
					{
						this.GetNodes(delegate(GraphNode node)
						{
							if (!node.Walkable)
							{
								builder.SolidBox((Vector3)node.position, new float3(size, size, size));
							}
						});
					}
				}
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x00022B64 File Offset: 0x00020D64
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x00022B6C File Offset: 0x00020D6C
		string IGraphInternals.SerializedEditorSettings
		{
			get
			{
				return this.serializedEditorSettings;
			}
			set
			{
				this.serializedEditorSettings = value;
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00022B75 File Offset: 0x00020D75
		void IGraphInternals.OnDestroy()
		{
			this.OnDestroy();
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00022B7D File Offset: 0x00020D7D
		void IGraphInternals.DisposeUnmanagedData()
		{
			this.DisposeUnmanagedData();
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00022B85 File Offset: 0x00020D85
		void IGraphInternals.DestroyAllNodes()
		{
			this.DestroyAllNodes();
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00022B8D File Offset: 0x00020D8D
		IGraphUpdatePromise IGraphInternals.ScanInternal(bool async)
		{
			return this.ScanInternal(async);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00022B96 File Offset: 0x00020D96
		void IGraphInternals.SerializeExtraInfo(GraphSerializationContext ctx)
		{
			this.SerializeExtraInfo(ctx);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00022B9F File Offset: 0x00020D9F
		void IGraphInternals.DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			this.DeserializeExtraInfo(ctx);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00022BA8 File Offset: 0x00020DA8
		void IGraphInternals.PostDeserialization(GraphSerializationContext ctx)
		{
			this.PostDeserialization(ctx);
		}

		// Token: 0x0400046B RID: 1131
		public AstarPath active;

		// Token: 0x0400046C RID: 1132
		[JsonMember]
		public Pathfinding.Util.Guid guid;

		// Token: 0x0400046D RID: 1133
		[JsonMember]
		public uint initialPenalty;

		// Token: 0x0400046E RID: 1134
		[JsonMember]
		public bool open;

		// Token: 0x0400046F RID: 1135
		public uint graphIndex;

		// Token: 0x04000470 RID: 1136
		[JsonMember]
		public string name;

		// Token: 0x04000471 RID: 1137
		[JsonMember]
		public bool drawGizmos = true;

		// Token: 0x04000472 RID: 1138
		[JsonMember]
		public bool infoScreenOpen;

		// Token: 0x04000473 RID: 1139
		[JsonMember]
		private string serializedEditorSettings;
	}
}
