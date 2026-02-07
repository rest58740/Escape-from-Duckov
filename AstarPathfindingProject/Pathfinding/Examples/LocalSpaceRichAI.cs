using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020002CC RID: 716
	[HelpURL("https://arongranberg.com/astar/documentation/stable/localspacerichai.html")]
	public class LocalSpaceRichAI : RichAI
	{
		// Token: 0x0600110D RID: 4365 RVA: 0x0006ACBC File Offset: 0x00068EBC
		protected override Vector3 ClampPositionToGraph(Vector3 newPosition)
		{
			this.RefreshTransform();
			NNInfo nninfo = (AstarPath.active != null) ? AstarPath.active.GetNearest(this.graph.transformation.InverseTransform(newPosition)) : default(NNInfo);
			float elevation;
			this.movementPlane.ToPlane(newPosition, out elevation);
			return this.movementPlane.ToWorld(this.movementPlane.ToPlane((nninfo.node != null) ? this.graph.transformation.Transform(nninfo.position) : newPosition), elevation);
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0006AD4A File Offset: 0x00068F4A
		private void RefreshTransform()
		{
			this.graph.Refresh();
			this.richPath.transform = this.graph.transformation;
			this.movementPlane = this.graph.transformation.ToSimpleMovementPlane();
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0006AD83 File Offset: 0x00068F83
		protected override void Start()
		{
			this.RefreshTransform();
			base.Start();
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0006AD94 File Offset: 0x00068F94
		protected override void CalculatePathRequestEndpoints(out Vector3 start, out Vector3 end)
		{
			this.RefreshTransform();
			base.CalculatePathRequestEndpoints(out start, out end);
			start = this.graph.transformation.InverseTransform(start);
			end = this.graph.transformation.InverseTransform(end);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0006ADE7 File Offset: 0x00068FE7
		protected override void OnUpdate(float dt)
		{
			this.RefreshTransform();
			base.OnUpdate(dt);
		}

		// Token: 0x04000CDB RID: 3291
		public LocalSpaceGraph graph;
	}
}
