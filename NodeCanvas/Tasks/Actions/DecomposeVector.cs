using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000066 RID: 102
	[Category("✫ Blackboard")]
	[Description("Create up to 3 floats from a Vector and save them to blackboard")]
	public class DecomposeVector : ActionTask
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00008A6E File Offset: 0x00006C6E
		protected override string info
		{
			get
			{
				string text = "Decompose Vector ";
				BBParameter<Vector3> bbparameter = this.targetVector;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00008A8C File Offset: 0x00006C8C
		protected override void OnExecute()
		{
			this.x.value = this.targetVector.value.x;
			this.y.value = this.targetVector.value.y;
			this.z.value = this.targetVector.value.z;
			base.EndAction();
		}

		// Token: 0x0400013D RID: 317
		public BBParameter<Vector3> targetVector;

		// Token: 0x0400013E RID: 318
		[BlackboardOnly]
		public BBParameter<float> x;

		// Token: 0x0400013F RID: 319
		[BlackboardOnly]
		public BBParameter<float> y;

		// Token: 0x04000140 RID: 320
		[BlackboardOnly]
		public BBParameter<float> z;
	}
}
