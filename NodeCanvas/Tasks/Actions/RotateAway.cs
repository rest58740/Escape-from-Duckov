using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000AD RID: 173
	[Category("Movement/Direct")]
	[Description("Rotate the agent away from target per frame")]
	public class RotateAway : ActionTask<Transform>
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x0000B0E0 File Offset: 0x000092E0
		protected override void OnUpdate()
		{
			if (Vector3.Angle(this.target.value.transform.position - base.agent.position, -base.agent.forward) <= this.angleDifference.value)
			{
				base.EndAction();
				return;
			}
			Vector3 vector = this.target.value.transform.position - base.agent.position;
			base.agent.rotation = Quaternion.LookRotation(Vector3.RotateTowards(base.agent.forward, vector, -this.speed.value * Time.deltaTime, 0f), this.upVector.value);
			if (!this.waitActionFinish)
			{
				base.EndAction();
			}
		}

		// Token: 0x040001FA RID: 506
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x040001FB RID: 507
		public BBParameter<float> speed = 2f;

		// Token: 0x040001FC RID: 508
		[SliderField(1, 180)]
		public BBParameter<float> angleDifference = 5f;

		// Token: 0x040001FD RID: 509
		public BBParameter<Vector3> upVector = Vector3.up;

		// Token: 0x040001FE RID: 510
		public bool waitActionFinish;
	}
}
