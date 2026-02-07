using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000424 RID: 1060
	public class AimToPlayer : ActionTask<AICharacterController>
	{
		// Token: 0x06002677 RID: 9847 RVA: 0x000852A7 File Offset: 0x000834A7
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000852AA File Offset: 0x000834AA
		protected override void OnExecute()
		{
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000852AC File Offset: 0x000834AC
		protected override void OnUpdate()
		{
			if (!this.target)
			{
				this.target = CharacterMainControl.Main;
			}
			base.agent.CharacterMainControl.SetAimPoint(this.target.transform.position);
		}

		// Token: 0x04001A35 RID: 6709
		private CharacterMainControl target;
	}
}
