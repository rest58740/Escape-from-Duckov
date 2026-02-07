using System;
using Pathfinding.Drawing;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200007D RID: 125
	[AddComponentMenu("Pathfinding/Link")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/nodelink.html")]
	public class NodeLink : GraphModifier
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00014F45 File Offset: 0x00013145
		public Transform Start
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00014F4D File Offset: 0x0001314D
		public Transform End
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00014F55 File Offset: 0x00013155
		public override void OnGraphsPostUpdateBeforeAreaRecalculation()
		{
			this.Apply();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00014F60 File Offset: 0x00013160
		public static void DrawArch(Vector3 a, Vector3 b, Vector3 up, Color color)
		{
			Vector3 vector = b - a;
			if (vector == Vector3.zero)
			{
				return;
			}
			Vector3 rhs = Vector3.Cross(up, vector);
			Vector3 b2 = Vector3.Cross(vector, rhs).normalized * vector.magnitude * 0.1f;
			Draw.Bezier(a, a + b2, b + b2, b, color);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00014FDC File Offset: 0x000131DC
		public virtual void Apply()
		{
			if (this.Start == null || this.End == null || AstarPath.active == null)
			{
				return;
			}
			GraphNode node = AstarPath.active.GetNearest(this.Start.position).node;
			GraphNode node2 = AstarPath.active.GetNearest(this.End.position).node;
			if (node == null || node2 == null)
			{
				return;
			}
			if (this.deleteConnection)
			{
				GraphNode.Disconnect(node, node2);
				return;
			}
			uint cost = (uint)Math.Round((double)((float)(node.position - node2.position).costMagnitude * this.costFactor));
			GraphNode.Connect(node, node2, cost, this.oneWay ? OffMeshLinks.Directionality.OneWay : OffMeshLinks.Directionality.TwoWay);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000150A0 File Offset: 0x000132A0
		public override void DrawGizmos()
		{
			if (this.Start == null || this.End == null)
			{
				return;
			}
			NodeLink.DrawArch(this.Start.position, this.End.position, Vector3.up, this.deleteConnection ? Color.red : Color.green);
		}

		// Token: 0x040002B1 RID: 689
		public Transform end;

		// Token: 0x040002B2 RID: 690
		public float costFactor = 1f;

		// Token: 0x040002B3 RID: 691
		public bool oneWay;

		// Token: 0x040002B4 RID: 692
		public bool deleteConnection;
	}
}
