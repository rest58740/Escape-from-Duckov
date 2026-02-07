using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200002D RID: 45
	[Category("Input (Legacy System)")]
	public class CheckMousePick2D : ConditionTask
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000048DC File Offset: 0x00002ADC
		protected override string info
		{
			get
			{
				string text = this.buttonKey.ToString() + " Click";
				if (!this.savePosAs.isNone)
				{
					string text2 = text;
					string text3 = "\nSavePos As ";
					BBParameter<Vector3> bbparameter = this.savePosAs;
					text = text2 + text3 + ((bbparameter != null) ? bbparameter.ToString() : null);
				}
				if (!this.saveGoAs.isNone)
				{
					string text4 = text;
					string text5 = "\nSaveGo As ";
					BBParameter<GameObject> bbparameter2 = this.saveGoAs;
					text = text4 + text5 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
				}
				return text;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000495C File Offset: 0x00002B5C
		protected override bool OnCheck()
		{
			this.buttonID = (int)this.buttonKey;
			if (Input.GetMouseButtonDown(this.buttonID))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				this.hit = Physics2D.Raycast(ray.origin, ray.direction, float.PositiveInfinity, this.mask);
				if (this.hit.collider != null)
				{
					this.savePosAs.value = this.hit.point;
					this.saveGoAs.value = this.hit.collider.gameObject;
					this.saveDistanceAs.value = this.hit.distance;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000085 RID: 133
		public ButtonKeys buttonKey;

		// Token: 0x04000086 RID: 134
		public LayerMask mask = -1;

		// Token: 0x04000087 RID: 135
		[BlackboardOnly]
		public BBParameter<GameObject> saveGoAs;

		// Token: 0x04000088 RID: 136
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;

		// Token: 0x04000089 RID: 137
		[BlackboardOnly]
		public BBParameter<Vector3> savePosAs;

		// Token: 0x0400008A RID: 138
		private int buttonID;

		// Token: 0x0400008B RID: 139
		private RaycastHit2D hit;
	}
}
