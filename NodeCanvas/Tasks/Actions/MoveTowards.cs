using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000AC RID: 172
	[Category("Movement/Direct")]
	[Description("Moves the agent towards to target per frame without pathfinding")]
	public class MoveTowards : ActionTask<Transform>
	{
		// Token: 0x060002D0 RID: 720 RVA: 0x0000B018 File Offset: 0x00009218
		protected override void OnUpdate()
		{
			if ((base.agent.position - this.target.value.transform.position).magnitude <= this.stopDistance.value)
			{
				base.EndAction();
				return;
			}
			base.agent.position = Vector3.MoveTowards(base.agent.position, this.target.value.transform.position, this.speed.value * Time.deltaTime);
			if (!this.waitActionFinish)
			{
				base.EndAction();
			}
		}

		// Token: 0x040001F6 RID: 502
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x040001F7 RID: 503
		public BBParameter<float> speed = 2f;

		// Token: 0x040001F8 RID: 504
		public BBParameter<float> stopDistance = 0.1f;

		// Token: 0x040001F9 RID: 505
		public bool waitActionFinish;
	}
}
