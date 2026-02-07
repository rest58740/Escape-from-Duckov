using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200041E RID: 1054
	public class CheckHoldGun : ConditionTask<AICharacterController>
	{
		// Token: 0x0600265E RID: 9822 RVA: 0x0008505D File Offset: 0x0008325D
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x00085060 File Offset: 0x00083260
		protected override void OnEnable()
		{
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x00085062 File Offset: 0x00083262
		protected override void OnDisable()
		{
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x00085064 File Offset: 0x00083264
		protected override bool OnCheck()
		{
			return base.agent.CharacterMainControl.GetGun() != null;
		}
	}
}
