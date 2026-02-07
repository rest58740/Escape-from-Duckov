using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200007B RID: 123
	[Category("✫ Blackboard")]
	public class SampleCurve : ActionTask
	{
		// Token: 0x06000237 RID: 567 RVA: 0x00009352 File Offset: 0x00007552
		protected override void OnExecute()
		{
			this.saveAs.value = this.curve.value.Evaluate(this.sampleAt.value);
			base.EndAction();
		}

		// Token: 0x0400016F RID: 367
		[RequiredField]
		public BBParameter<AnimationCurve> curve;

		// Token: 0x04000170 RID: 368
		[SliderField(0, 1)]
		public BBParameter<float> sampleAt;

		// Token: 0x04000171 RID: 369
		[BlackboardOnly]
		public BBParameter<float> saveAs;
	}
}
