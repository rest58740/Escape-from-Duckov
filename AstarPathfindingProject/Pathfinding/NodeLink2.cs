using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000080 RID: 128
	[AddComponentMenu("Pathfinding/Link2")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/nodelink2.html")]
	public class NodeLink2 : GraphModifier
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00014F45 File Offset: 0x00013145
		public Transform StartTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00015111 File Offset: 0x00013311
		public Transform EndTransform
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001511C File Offset: 0x0001331C
		public static NodeLink2 GetNodeLink(GraphNode node)
		{
			LinkNode linkNode = node as LinkNode;
			if (linkNode == null)
			{
				return null;
			}
			return linkNode.linkSource.component as NodeLink2;
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00015145 File Offset: 0x00013345
		internal bool isActive
		{
			get
			{
				return this.linkSource != null && (this.linkSource.status & OffMeshLinks.OffMeshLinkStatus.Active) > (OffMeshLinks.OffMeshLinkStatus)0;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00015161 File Offset: 0x00013361
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00015169 File Offset: 0x00013369
		public IOffMeshLinkHandler onTraverseOffMeshLink
		{
			get
			{
				return this.onTraverseOffMeshLinkHandler;
			}
			set
			{
				this.onTraverseOffMeshLinkHandler = value;
				if (this.linkSource != null)
				{
					this.linkSource.handler = value;
				}
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00015186 File Offset: 0x00013386
		public override void OnPostScan()
		{
			this.TryAddLink();
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001518E File Offset: 0x0001338E
		protected override void OnEnable()
		{
			base.OnEnable();
			if (Application.isPlaying && !BatchedEvents.Has<NodeLink2>(this))
			{
				BatchedEvents.Add<NodeLink2>(this, BatchedEvents.Event.Update, new Action<NodeLink2[], int>(NodeLink2.OnUpdate), 0);
			}
			this.TryAddLink();
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000151C0 File Offset: 0x000133C0
		private static void OnUpdate(NodeLink2[] components, int count)
		{
			if (Time.frameCount % 16 != 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				NodeLink2 nodeLink = components[i];
				Transform startTransform = nodeLink.StartTransform;
				Transform endTransform = nodeLink.EndTransform;
				bool flag = nodeLink.linkSource != null;
				if ((startTransform != null && endTransform != null) != flag || (flag && (startTransform.hasChanged || endTransform.hasChanged)))
				{
					if (startTransform != null)
					{
						startTransform.hasChanged = false;
					}
					if (endTransform != null)
					{
						endTransform.hasChanged = false;
					}
					nodeLink.RemoveLink();
					nodeLink.TryAddLink();
				}
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00015258 File Offset: 0x00013458
		private void TryAddLink()
		{
			if (this.linkSource != null && (this.linkSource.status == OffMeshLinks.OffMeshLinkStatus.Inactive || (this.linkSource.status & OffMeshLinks.OffMeshLinkStatus.PendingRemoval) != (OffMeshLinks.OffMeshLinkStatus)0))
			{
				this.linkSource = null;
			}
			if (this.linkSource == null && AstarPath.active != null && this.EndTransform != null)
			{
				this.StartTransform.hasChanged = false;
				this.EndTransform.hasChanged = false;
				this.linkSource = new OffMeshLinks.OffMeshLinkSource
				{
					start = new OffMeshLinks.Anchor
					{
						center = this.StartTransform.position,
						rotation = this.StartTransform.rotation,
						width = 0f
					},
					end = new OffMeshLinks.Anchor
					{
						center = this.EndTransform.position,
						rotation = this.EndTransform.rotation,
						width = 0f
					},
					directionality = (this.oneWay ? OffMeshLinks.Directionality.OneWay : OffMeshLinks.Directionality.TwoWay),
					tag = this.pathfindingTag,
					costFactor = this.costFactor,
					graphMask = this.graphMask,
					maxSnappingDistance = 1f,
					component = this,
					handler = this.onTraverseOffMeshLink
				};
				AstarPath.active.offMeshLinks.Add(this.linkSource);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000153C8 File Offset: 0x000135C8
		private void RemoveLink()
		{
			if (AstarPath.active != null && this.linkSource != null)
			{
				AstarPath.active.offMeshLinks.Remove(this.linkSource);
			}
			this.linkSource = null;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000153FB File Offset: 0x000135FB
		protected override void OnDisable()
		{
			base.OnDisable();
			BatchedEvents.Remove<NodeLink2>(this);
			this.RemoveLink();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001540F File Offset: 0x0001360F
		[ContextMenu("Recalculate neighbours")]
		private void ContextApplyForce()
		{
			this.Apply();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00015417 File Offset: 0x00013617
		public virtual void Apply()
		{
			this.RemoveLink();
			this.TryAddLink();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00015428 File Offset: 0x00013628
		public override void DrawGizmos()
		{
			if (this.StartTransform == null || this.EndTransform == null)
			{
				return;
			}
			Vector3 position = this.StartTransform.position;
			Vector3 position2 = this.EndTransform.position;
			if (this.linkSource != null && Time.renderedFrameCount % 16 == 0 && Application.isEditor && (this.linkSource.start.center != position || this.linkSource.end.center != position2 || this.linkSource.directionality != (this.oneWay ? OffMeshLinks.Directionality.OneWay : OffMeshLinks.Directionality.TwoWay) || this.linkSource.costFactor != this.costFactor || this.linkSource.graphMask != this.graphMask || this.linkSource.tag != this.pathfindingTag))
			{
				this.Apply();
			}
			bool flag = GizmoContext.InActiveSelection(this);
			List<NavGraph> list = (this.linkSource != null && AstarPath.active != null) ? AstarPath.active.offMeshLinks.ConnectedGraphs(this.linkSource) : null;
			Vector3 vector = Vector3.up;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					NavGraph navGraph = list[i];
					if (navGraph != null)
					{
						NavmeshBase navmeshBase = navGraph as NavmeshBase;
						if (navmeshBase != null)
						{
							vector = navmeshBase.transform.WorldUpAtGraphPosition(Vector3.zero);
							break;
						}
						GridGraph gridGraph = navGraph as GridGraph;
						if (gridGraph != null)
						{
							vector = gridGraph.transform.WorldUpAtGraphPosition(Vector3.zero);
							break;
						}
					}
				}
				ListPool<NavGraph>.Release(ref list);
			}
			bool flag2 = this.linkSource != null && this.linkSource.status == OffMeshLinks.OffMeshLinkStatus.Active;
			Color color = flag ? NodeLink2.GizmosColorSelected : NodeLink2.GizmosColor;
			if (flag2)
			{
				color = Color.green;
			}
			Draw.Circle(position, vector, 0.4f, (this.linkSource != null && this.linkSource.status.HasFlag(OffMeshLinks.OffMeshLinkStatus.FailedToConnectStart)) ? Color.red : color);
			Draw.Circle(position2, vector, 0.4f, (this.linkSource != null && this.linkSource.status.HasFlag(OffMeshLinks.OffMeshLinkStatus.FailedToConnectEnd)) ? Color.red : color);
			NodeLink.DrawArch(position, position2, vector, color);
			if (flag)
			{
				Vector3 normalized = Vector3.Cross(vector, position2 - position).normalized;
				using (Draw.WithLineWidth(2f, true))
				{
					NodeLink.DrawArch(position + normalized * 0f, position2 + normalized * 0f, vector, color);
				}
			}
		}

		// Token: 0x040002B5 RID: 693
		public Transform end;

		// Token: 0x040002B6 RID: 694
		public float costFactor = 1f;

		// Token: 0x040002B7 RID: 695
		public bool oneWay;

		// Token: 0x040002B8 RID: 696
		public PathfindingTag pathfindingTag = 0U;

		// Token: 0x040002B9 RID: 697
		public GraphMask graphMask = -1;

		// Token: 0x040002BA RID: 698
		protected OffMeshLinks.OffMeshLinkSource linkSource;

		// Token: 0x040002BB RID: 699
		private IOffMeshLinkHandler onTraverseOffMeshLinkHandler;

		// Token: 0x040002BC RID: 700
		private static readonly Color GizmosColor = new Color(0.80784315f, 0.53333336f, 0.1882353f, 0.5f);

		// Token: 0x040002BD RID: 701
		private static readonly Color GizmosColorSelected = new Color(0.92156863f, 0.48235294f, 0.1254902f, 1f);
	}
}
