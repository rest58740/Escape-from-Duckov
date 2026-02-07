using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000AA RID: 170
	[Category("Movement/Direct")]
	[Description("Move & turn the agent based on input values provided ranging from -1 to 1, per second (using delta time)")]
	public class InputMove : ActionTask<Transform>
	{
		// Token: 0x060002CC RID: 716 RVA: 0x0000ADE4 File Offset: 0x00008FE4
		protected override void OnUpdate()
		{
			Quaternion b = base.agent.rotation * Quaternion.Euler(Vector3.up * this.turn.value * 10f);
			base.agent.rotation = Quaternion.Slerp(base.agent.rotation, b, this.rotationSpeed.value * Time.deltaTime);
			Vector3 b2 = base.agent.forward * this.forward.value * this.moveSpeed.value * Time.deltaTime;
			Vector3 a = base.agent.right * this.strafe.value * this.moveSpeed.value * Time.deltaTime;
			Vector3 b3 = base.agent.up * this.up.value * this.moveSpeed.value * Time.deltaTime;
			base.agent.position += a + b2 + b3;
			if (!this.repeat)
			{
				base.EndAction();
			}
		}

		// Token: 0x040001EB RID: 491
		[BlackboardOnly]
		public BBParameter<float> strafe;

		// Token: 0x040001EC RID: 492
		[BlackboardOnly]
		public BBParameter<float> turn;

		// Token: 0x040001ED RID: 493
		[BlackboardOnly]
		public BBParameter<float> forward;

		// Token: 0x040001EE RID: 494
		[BlackboardOnly]
		public BBParameter<float> up;

		// Token: 0x040001EF RID: 495
		public BBParameter<float> moveSpeed = 1f;

		// Token: 0x040001F0 RID: 496
		public BBParameter<float> rotationSpeed = 1f;

		// Token: 0x040001F1 RID: 497
		public bool repeat;
	}
}
