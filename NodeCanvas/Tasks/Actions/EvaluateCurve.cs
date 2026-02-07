using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000069 RID: 105
	[Category("✫ Blackboard")]
	public class EvaluateCurve : ActionTask
	{
		// Token: 0x06000206 RID: 518 RVA: 0x00008BE4 File Offset: 0x00006DE4
		protected override void OnUpdate()
		{
			this.saveAs.value = this.curve.value.Evaluate(Mathf.Lerp(this.from.value, this.to.value, base.elapsedTime / this.time.value));
			if (base.elapsedTime > this.time.value)
			{
				base.EndAction();
			}
		}

		// Token: 0x04000147 RID: 327
		[RequiredField]
		public BBParameter<AnimationCurve> curve;

		// Token: 0x04000148 RID: 328
		public BBParameter<float> from;

		// Token: 0x04000149 RID: 329
		public BBParameter<float> to = 1f;

		// Token: 0x0400014A RID: 330
		public BBParameter<float> time = 1f;

		// Token: 0x0400014B RID: 331
		[BlackboardOnly]
		public BBParameter<float> saveAs;
	}
}
