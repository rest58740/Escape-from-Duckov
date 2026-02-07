using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000082 RID: 130
	[AddComponentMenu("Pathfinding/Link3")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/nodelink3.html")]
	public class NodeLink3 : GraphModifier
	{
		// Token: 0x06000418 RID: 1048 RVA: 0x000158D8 File Offset: 0x00013AD8
		public static NodeLink3 GetNodeLink(GraphNode node)
		{
			NodeLink3 result;
			NodeLink3.reference.TryGetValue(node, out result);
			return result;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00014F45 File Offset: 0x00013145
		public Transform StartTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x000158F4 File Offset: 0x00013AF4
		public Transform EndTransform
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x000158FC File Offset: 0x00013AFC
		public GraphNode StartNode
		{
			get
			{
				return this.startNode;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x00015904 File Offset: 0x00013B04
		public GraphNode EndNode
		{
			get
			{
				return this.endNode;
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001590C File Offset: 0x00013B0C
		public override void OnPostScan()
		{
			if (AstarPath.active.isScanning)
			{
				this.InternalOnPostScan();
				return;
			}
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(bool _)
			{
				this.InternalOnPostScan();
				return true;
			}));
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001593C File Offset: 0x00013B3C
		public void InternalOnPostScan()
		{
			if (AstarPath.active.data.pointGraph == null)
			{
				AstarPath.active.data.AddGraph(typeof(PointGraph));
			}
			this.startNode = AstarPath.active.data.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3)this.StartTransform.position);
			this.startNode.link = this;
			this.endNode = AstarPath.active.data.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3)this.EndTransform.position);
			this.endNode.link = this;
			this.connectedNode1 = null;
			this.connectedNode2 = null;
			if (this.startNode == null || this.endNode == null)
			{
				this.startNode = null;
				this.endNode = null;
				return;
			}
			this.postScanCalled = true;
			NodeLink3.reference[this.startNode] = this;
			NodeLink3.reference[this.endNode] = this;
			this.Apply(true);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00015A54 File Offset: 0x00013C54
		public override void OnGraphsPostUpdateBeforeAreaRecalculation()
		{
			if (!AstarPath.active.isScanning)
			{
				if (this.connectedNode1 != null && this.connectedNode1.Destroyed)
				{
					this.connectedNode1 = null;
				}
				if (this.connectedNode2 != null && this.connectedNode2.Destroyed)
				{
					this.connectedNode2 = null;
				}
				if (!this.postScanCalled)
				{
					this.OnPostScan();
					return;
				}
				this.Apply(false);
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00015ABB File Offset: 0x00013CBB
		protected override void OnEnable()
		{
			base.OnEnable();
			if (Application.isPlaying && AstarPath.active != null && AstarPath.active.data != null && AstarPath.active.data.pointGraph != null)
			{
				this.OnGraphsPostUpdate();
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00015AFC File Offset: 0x00013CFC
		protected override void OnDisable()
		{
			base.OnDisable();
			this.postScanCalled = false;
			if (this.startNode != null)
			{
				NodeLink3.reference.Remove(this.startNode);
			}
			if (this.endNode != null)
			{
				NodeLink3.reference.Remove(this.endNode);
			}
			if (this.startNode != null && this.endNode != null)
			{
				this.startNode.RemovePartialConnection(this.endNode);
				this.endNode.RemovePartialConnection(this.startNode);
				if (this.connectedNode1 != null && this.connectedNode2 != null)
				{
					this.startNode.RemovePartialConnection(this.connectedNode1);
					this.connectedNode1.RemovePartialConnection(this.startNode);
					this.endNode.RemovePartialConnection(this.connectedNode2);
					this.connectedNode2.RemovePartialConnection(this.endNode);
				}
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00015BCE File Offset: 0x00013DCE
		private void RemoveConnections(GraphNode node)
		{
			node.ClearConnections(true);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00015BD7 File Offset: 0x00013DD7
		[ContextMenu("Recalculate neighbours")]
		private void ContextApplyForce()
		{
			if (Application.isPlaying)
			{
				this.Apply(true);
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00015BE8 File Offset: 0x00013DE8
		public void Apply(bool forceNewCheck)
		{
			NNConstraint none = NNConstraint.None;
			none.distanceMetric = DistanceMetric.ClosestAsSeenFromAboveSoft();
			int graphIndex = (int)this.startNode.GraphIndex;
			none.graphMask = ~(1 << graphIndex);
			bool flag = true;
			NNInfo nearest = AstarPath.active.GetNearest(this.StartTransform.position, none);
			flag &= (nearest.node == this.connectedNode1 && nearest.node != null);
			this.connectedNode1 = (nearest.node as MeshNode);
			this.clamped1 = nearest.position;
			if (this.connectedNode1 != null)
			{
				Debug.DrawRay((Vector3)this.connectedNode1.position, Vector3.up * 5f, Color.red);
			}
			NNInfo nearest2 = AstarPath.active.GetNearest(this.EndTransform.position, none);
			flag &= (nearest2.node == this.connectedNode2 && nearest2.node != null);
			this.connectedNode2 = (nearest2.node as MeshNode);
			this.clamped2 = nearest2.position;
			if (this.connectedNode2 != null)
			{
				Debug.DrawRay((Vector3)this.connectedNode2.position, Vector3.up * 5f, Color.cyan);
			}
			if (this.connectedNode2 == null || this.connectedNode1 == null)
			{
				return;
			}
			this.startNode.position = (Int3)this.StartTransform.position;
			this.endNode.position = (Int3)this.EndTransform.position;
			if (flag && !forceNewCheck)
			{
				return;
			}
			this.RemoveConnections(this.startNode);
			this.RemoveConnections(this.endNode);
			uint cost = (uint)Mathf.RoundToInt((float)((Int3)(this.StartTransform.position - this.EndTransform.position)).costMagnitude * this.costFactor);
			GraphNode.Connect(this.startNode, this.endNode, cost, OffMeshLinks.Directionality.TwoWay);
			Int3 rhs = this.connectedNode2.position - this.connectedNode1.position;
			for (int i = 0; i < this.connectedNode1.GetVertexCount(); i++)
			{
				Int3 vertex = this.connectedNode1.GetVertex(i);
				Int3 vertex2 = this.connectedNode1.GetVertex((i + 1) % this.connectedNode1.GetVertexCount());
				if (Int3.DotLong((vertex2 - vertex).Normal2D(), rhs) <= 0L)
				{
					for (int j = 0; j < this.connectedNode2.GetVertexCount(); j++)
					{
						Int3 vertex3 = this.connectedNode2.GetVertex(j);
						Int3 vertex4 = this.connectedNode2.GetVertex((j + 1) % this.connectedNode2.GetVertexCount());
						if (Int3.DotLong((vertex4 - vertex3).Normal2D(), rhs) >= 0L && (double)Int3.Angle(vertex4 - vertex3, vertex2 - vertex) > 2.967059810956319)
						{
							float num = 0f;
							float num2 = 1f;
							num2 = Math.Min(num2, VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex3));
							num = Math.Max(num, VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex4));
							if (num2 >= num)
							{
								Vector3 vector = (Vector3)(vertex2 - vertex) * num + (Vector3)vertex;
								Vector3 vector2 = (Vector3)(vertex2 - vertex) * num2 + (Vector3)vertex;
								this.startNode.portalA = vector;
								this.startNode.portalB = vector2;
								this.endNode.portalA = vector2;
								this.endNode.portalB = vector;
								GraphNode.Connect(this.connectedNode1, this.startNode, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor), OffMeshLinks.Directionality.TwoWay);
								GraphNode.Connect(this.endNode, this.connectedNode2, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor), OffMeshLinks.Directionality.TwoWay);
								return;
							}
							Debug.LogError(string.Concat(new string[]
							{
								"Something went wrong! ",
								num.ToString(),
								" ",
								num2.ToString(),
								" ",
								vertex,
								" ",
								vertex2,
								" ",
								vertex3,
								" ",
								vertex4,
								"\nTODO, how can this happen?"
							}));
						}
					}
				}
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000160D0 File Offset: 0x000142D0
		public override void DrawGizmos()
		{
			bool flag = GizmoContext.InActiveSelection(this);
			Color color = flag ? NodeLink3.GizmosColorSelected : NodeLink3.GizmosColor;
			if (this.StartTransform != null)
			{
				Draw.xz.Circle(this.StartTransform.position, 0.4f, color);
			}
			if (this.EndTransform != null)
			{
				Draw.xz.Circle(this.EndTransform.position, 0.4f, color);
			}
			if (this.StartTransform != null && this.EndTransform != null)
			{
				NodeLink.DrawArch(this.StartTransform.position, this.EndTransform.position, Vector3.up, color);
				if (flag)
				{
					Vector3 normalized = Vector3.Cross(Vector3.up, this.EndTransform.position - this.StartTransform.position).normalized;
					NodeLink.DrawArch(this.StartTransform.position + normalized * 0.1f, this.EndTransform.position + normalized * 0.1f, Vector3.up, color);
					NodeLink.DrawArch(this.StartTransform.position - normalized * 0.1f, this.EndTransform.position - normalized * 0.1f, Vector3.up, color);
				}
			}
		}

		// Token: 0x040002C1 RID: 705
		protected static Dictionary<GraphNode, NodeLink3> reference = new Dictionary<GraphNode, NodeLink3>();

		// Token: 0x040002C2 RID: 706
		public Transform end;

		// Token: 0x040002C3 RID: 707
		public float costFactor = 1f;

		// Token: 0x040002C4 RID: 708
		private NodeLink3Node startNode;

		// Token: 0x040002C5 RID: 709
		private NodeLink3Node endNode;

		// Token: 0x040002C6 RID: 710
		private MeshNode connectedNode1;

		// Token: 0x040002C7 RID: 711
		private MeshNode connectedNode2;

		// Token: 0x040002C8 RID: 712
		private Vector3 clamped1;

		// Token: 0x040002C9 RID: 713
		private Vector3 clamped2;

		// Token: 0x040002CA RID: 714
		private bool postScanCalled;

		// Token: 0x040002CB RID: 715
		private static readonly Color GizmosColor = new Color(0.80784315f, 0.53333336f, 0.1882353f, 0.5f);

		// Token: 0x040002CC RID: 716
		private static readonly Color GizmosColorSelected = new Color(0.92156863f, 0.48235294f, 0.1254902f, 1f);
	}
}
