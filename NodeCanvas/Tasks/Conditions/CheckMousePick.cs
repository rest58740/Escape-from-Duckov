using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200002C RID: 44
	[Category("Input (Legacy System)")]
	public class CheckMousePick : ConditionTask
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000047C8 File Offset: 0x000029C8
		protected override string info
		{
			get
			{
				string text = this.buttonKey.ToString() + " Click";
				if (!string.IsNullOrEmpty(this.savePosAs.name))
				{
					text += string.Format("\n<i>(SavePos As {0})</i>", this.savePosAs);
				}
				if (!string.IsNullOrEmpty(this.saveGoAs.name))
				{
					text += string.Format("\n<i>(SaveGo As {0})</i>", this.saveGoAs);
				}
				return text;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004844 File Offset: 0x00002A44
		protected override bool OnCheck()
		{
			if (Input.GetMouseButtonDown((int)this.buttonKey) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.hit, float.PositiveInfinity, 1 << this.layer))
			{
				this.saveGoAs.value = this.hit.collider.gameObject;
				this.saveDistanceAs.value = this.hit.distance;
				this.savePosAs.value = this.hit.point;
				return true;
			}
			return false;
		}

		// Token: 0x0400007F RID: 127
		public ButtonKeys buttonKey;

		// Token: 0x04000080 RID: 128
		[LayerField]
		public int layer;

		// Token: 0x04000081 RID: 129
		[BlackboardOnly]
		public BBParameter<GameObject> saveGoAs;

		// Token: 0x04000082 RID: 130
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;

		// Token: 0x04000083 RID: 131
		[BlackboardOnly]
		public BBParameter<Vector3> savePosAs;

		// Token: 0x04000084 RID: 132
		private RaycastHit hit;
	}
}
