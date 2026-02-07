using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000421 RID: 1057
	public class CheckReloading : ConditionTask<AICharacterController>
	{
		// Token: 0x0600266D RID: 9837 RVA: 0x0008516E File Offset: 0x0008336E
		protected override bool OnCheck()
		{
			return !(base.agent == null) && !(base.agent.CharacterMainControl == null) && base.agent.CharacterMainControl.reloadAction.Running;
		}
	}
}
