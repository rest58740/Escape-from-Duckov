using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000AE RID: 174
	[Category("Movement/Direct")]
	[Description("Rotate the agent towards the target per frame")]
	public class RotateTowards : ActionTask<Transform>
	{
		// Token: 0x060002D4 RID: 724 RVA: 0x0000B1EC File Offset: 0x000093EC
		protected override void OnUpdate()
		{
			if (Vector3.Angle(this.target.value.transform.position - base.agent.position, base.agent.forward) <= this.angleDifference.value)
			{
				base.EndAction();
				return;
			}
			Vector3 vector = this.target.value.transform.position - base.agent.position;
			base.agent.rotation = Quaternion.LookRotation(Vector3.RotateTowards(base.agent.forward, vector, this.speed.value * Time.deltaTime, 0f), this.upVector.value);
			if (!this.waitActionFinish)
			{
				base.EndAction();
			}
		}

		// Token: 0x040001FF RID: 511
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x04000200 RID: 512
		public BBParameter<float> speed = 2f;

		// Token: 0x04000201 RID: 513
		[SliderField(1, 180)]
		public BBParameter<float> angleDifference = 5f;

		// Token: 0x04000202 RID: 514
		public BBParameter<Vector3> upVector = Vector3.up;

		// Token: 0x04000203 RID: 515
		public bool waitActionFinish;
	}
}
