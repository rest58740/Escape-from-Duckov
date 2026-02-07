using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B7 RID: 183
	[Category("Physics")]
	[Description("Get hit info for ALL objects in the linecast, in Lists")]
	public class GetLinecastInfo2DAll : ActionTask<Transform>
	{
		// Token: 0x060002FE RID: 766 RVA: 0x0000BFE4 File Offset: 0x0000A1E4
		protected override void OnExecute()
		{
			this.hits = Physics2D.LinecastAll(base.agent.position, this.target.value.transform.position, this.mask);
			if (this.hits.Length != 0)
			{
				this.saveHitGameObjectsAs.value = (from h in this.hits
				select h.collider.gameObject).ToList<GameObject>();
				this.saveDistancesAs.value = (from h in this.hits
				select h.fraction).ToList<float>();
				this.savePointsAs.value = (from h in this.hits
				select new Vector3(h.point.x, h.point.y, this.target.value.transform.position.z)).ToList<Vector3>();
				this.saveNormalsAs.value = (from h in this.hits
				select new Vector3(h.normal.x, h.normal.y, 0f)).ToList<Vector3>();
				base.EndAction(true);
				return;
			}
			base.EndAction(false);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000C124 File Offset: 0x0000A324
		public override void OnDrawGizmosSelected()
		{
			if (base.agent && this.target.value)
			{
				Gizmos.DrawLine(base.agent.position, this.target.value.transform.position);
			}
		}

		// Token: 0x0400022C RID: 556
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x0400022D RID: 557
		public LayerMask mask = -1;

		// Token: 0x0400022E RID: 558
		[BlackboardOnly]
		public BBParameter<List<GameObject>> saveHitGameObjectsAs;

		// Token: 0x0400022F RID: 559
		[BlackboardOnly]
		public BBParameter<List<float>> saveDistancesAs;

		// Token: 0x04000230 RID: 560
		[BlackboardOnly]
		public BBParameter<List<Vector3>> savePointsAs;

		// Token: 0x04000231 RID: 561
		[BlackboardOnly]
		public BBParameter<List<Vector3>> saveNormalsAs;

		// Token: 0x04000232 RID: 562
		private RaycastHit2D[] hits;
	}
}
