using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000436 RID: 1078
	public class StopMoving : ActionTask<AICharacterController>
	{
		// Token: 0x060026E9 RID: 9961 RVA: 0x0008681F File Offset: 0x00084A1F
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x00086822 File Offset: 0x00084A22
		protected override void OnExecute()
		{
			base.agent.StopMove();
			base.EndAction(true);
		}
	}
}
