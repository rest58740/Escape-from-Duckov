using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A8 RID: 168
	[Category("Input (Legacy System)")]
	public class WaitMousePick2D : ActionTask
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000AB2C File Offset: 0x00008D2C
		protected override string info
		{
			get
			{
				return string.Format("Wait Object '{0}' Click. Save As {1}", this.buttonKey, this.saveObjectAs);
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000AB4C File Offset: 0x00008D4C
		protected override void OnUpdate()
		{
			this.buttonID = (int)this.buttonKey;
			if (Input.GetMouseButtonDown(this.buttonID))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				this.hit = Physics2D.Raycast(ray.origin, ray.direction, float.PositiveInfinity, this.mask);
				if (this.hit.collider != null)
				{
					this.savePositionAs.value = this.hit.point;
					this.saveObjectAs.value = this.hit.collider.gameObject;
					this.saveDistanceAs.value = this.hit.distance;
					base.EndAction(true);
				}
			}
		}

		// Token: 0x040001DB RID: 475
		public WaitMousePick2D.ButtonKeys buttonKey;

		// Token: 0x040001DC RID: 476
		public LayerMask mask = -1;

		// Token: 0x040001DD RID: 477
		[BlackboardOnly]
		public BBParameter<GameObject> saveObjectAs;

		// Token: 0x040001DE RID: 478
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;

		// Token: 0x040001DF RID: 479
		[BlackboardOnly]
		public BBParameter<Vector3> savePositionAs;

		// Token: 0x040001E0 RID: 480
		private int buttonID;

		// Token: 0x040001E1 RID: 481
		private RaycastHit2D hit;

		// Token: 0x0200013C RID: 316
		public enum ButtonKeys
		{
			// Token: 0x0400038B RID: 907
			Left,
			// Token: 0x0400038C RID: 908
			Right,
			// Token: 0x0400038D RID: 909
			Middle
		}
	}
}
