using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000AB RID: 171
	[Category("Movement/Direct")]
	[Description("Moves the agent away from target per frame without pathfinding")]
	public class MoveAway : ActionTask<Transform>
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0000AF50 File Offset: 0x00009150
		protected override void OnUpdate()
		{
			if ((base.agent.position - this.target.value.transform.position).magnitude >= this.stopDistance.value)
			{
				base.EndAction();
				return;
			}
			base.agent.position = Vector3.MoveTowards(base.agent.position, this.target.value.transform.position, -this.speed.value * Time.deltaTime);
			if (!this.waitActionFinish)
			{
				base.EndAction();
			}
		}

		// Token: 0x040001F2 RID: 498
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x040001F3 RID: 499
		public BBParameter<float> speed = 2f;

		// Token: 0x040001F4 RID: 500
		public BBParameter<float> stopDistance = 3f;

		// Token: 0x040001F5 RID: 501
		public bool waitActionFinish;
	}
}
