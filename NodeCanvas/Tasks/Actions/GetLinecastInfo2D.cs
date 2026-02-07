using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B6 RID: 182
	[Category("Physics")]
	public class GetLinecastInfo2D : ActionTask<Transform>
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0000BEA4 File Offset: 0x0000A0A4
		protected override void OnExecute()
		{
			this.hit = Physics2D.Linecast(base.agent.position, this.target.value.transform.position, this.mask);
			if (this.hit.collider != null)
			{
				this.saveHitGameObjectAs.value = this.hit.collider.gameObject;
				this.saveDistanceAs.value = this.hit.fraction;
				this.savePointAs.value = this.hit.point;
				this.saveNormalAs.value = this.hit.normal;
				base.EndAction(true);
				return;
			}
			base.EndAction(false);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000BF7C File Offset: 0x0000A17C
		public override void OnDrawGizmosSelected()
		{
			if (base.agent && this.target.value)
			{
				Gizmos.DrawLine(base.agent.position, this.target.value.transform.position);
			}
		}

		// Token: 0x04000225 RID: 549
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x04000226 RID: 550
		public LayerMask mask = -1;

		// Token: 0x04000227 RID: 551
		[BlackboardOnly]
		public BBParameter<GameObject> saveHitGameObjectAs;

		// Token: 0x04000228 RID: 552
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;

		// Token: 0x04000229 RID: 553
		[BlackboardOnly]
		public BBParameter<Vector3> savePointAs;

		// Token: 0x0400022A RID: 554
		[BlackboardOnly]
		public BBParameter<Vector3> saveNormalAs;

		// Token: 0x0400022B RID: 555
		private RaycastHit2D hit;
	}
}
