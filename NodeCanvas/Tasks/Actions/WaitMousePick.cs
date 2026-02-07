using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A7 RID: 167
	[Category("Input (Legacy System)")]
	public class WaitMousePick : ActionTask
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000AA5B File Offset: 0x00008C5B
		protected override string info
		{
			get
			{
				return string.Format("Wait Object '{0}' Click. Save As {1}", this.buttonKey, this.saveObjectAs);
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000AA78 File Offset: 0x00008C78
		protected override void OnUpdate()
		{
			this.buttonID = (int)this.buttonKey;
			if (Input.GetMouseButtonDown(this.buttonID) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.hit, float.PositiveInfinity, this.mask))
			{
				this.savePositionAs.value = this.hit.point;
				this.saveObjectAs.value = this.hit.collider.gameObject;
				this.saveDistanceAs.value = this.hit.distance;
				base.EndAction(true);
			}
		}

		// Token: 0x040001D4 RID: 468
		public WaitMousePick.ButtonKeys buttonKey;

		// Token: 0x040001D5 RID: 469
		public LayerMask mask = -1;

		// Token: 0x040001D6 RID: 470
		[BlackboardOnly]
		public BBParameter<GameObject> saveObjectAs;

		// Token: 0x040001D7 RID: 471
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;

		// Token: 0x040001D8 RID: 472
		[BlackboardOnly]
		public BBParameter<Vector3> savePositionAs;

		// Token: 0x040001D9 RID: 473
		private int buttonID;

		// Token: 0x040001DA RID: 474
		private RaycastHit hit;

		// Token: 0x0200013B RID: 315
		public enum ButtonKeys
		{
			// Token: 0x04000387 RID: 903
			Left,
			// Token: 0x04000388 RID: 904
			Right,
			// Token: 0x04000389 RID: 905
			Middle
		}
	}
}
