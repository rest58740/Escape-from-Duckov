using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B5 RID: 181
	[Category("Physics")]
	public class GetLinecastInfo : ActionTask<Transform>
	{
		// Token: 0x060002F8 RID: 760 RVA: 0x0000BD80 File Offset: 0x00009F80
		protected override void OnExecute()
		{
			if (Physics.Linecast(base.agent.position, this.target.value.transform.position, out this.hit, this.layerMask.value))
			{
				this.saveHitGameObjectAs.value = this.hit.collider.gameObject;
				this.saveDistanceAs.value = this.hit.distance;
				this.savePointAs.value = this.hit.point;
				this.saveNormalAs.value = this.hit.normal;
				base.EndAction(true);
				return;
			}
			base.EndAction(false);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000BE38 File Offset: 0x0000A038
		public override void OnDrawGizmosSelected()
		{
			if (base.agent && this.target.value)
			{
				Gizmos.DrawLine(base.agent.position, this.target.value.transform.position);
			}
		}

		// Token: 0x0400021E RID: 542
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x0400021F RID: 543
		public BBParameter<LayerMask> layerMask = -1;

		// Token: 0x04000220 RID: 544
		[BlackboardOnly]
		public BBParameter<GameObject> saveHitGameObjectAs;

		// Token: 0x04000221 RID: 545
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;

		// Token: 0x04000222 RID: 546
		[BlackboardOnly]
		public BBParameter<Vector3> savePointAs;

		// Token: 0x04000223 RID: 547
		[BlackboardOnly]
		public BBParameter<Vector3> saveNormalAs;

		// Token: 0x04000224 RID: 548
		private RaycastHit hit;
	}
}
