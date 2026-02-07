using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000065 RID: 101
	[Category("✫ Blackboard")]
	[Description("Create a new Vector out of 3 floats and save it to the blackboard")]
	public class ComposeVector : ActionTask
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00008A0F File Offset: 0x00006C0F
		protected override string info
		{
			get
			{
				string text = "New Vector as ";
				BBParameter<Vector3> bbparameter = this.saveAs;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00008A2D File Offset: 0x00006C2D
		protected override void OnExecute()
		{
			this.saveAs.value = new Vector3(this.x.value, this.y.value, this.z.value);
			base.EndAction();
		}

		// Token: 0x04000139 RID: 313
		public BBParameter<float> x;

		// Token: 0x0400013A RID: 314
		public BBParameter<float> y;

		// Token: 0x0400013B RID: 315
		public BBParameter<float> z;

		// Token: 0x0400013C RID: 316
		[BlackboardOnly]
		public BBParameter<Vector3> saveAs;
	}
}
