using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000425 RID: 1061
	public class Attack : ActionTask<AICharacterController>
	{
		// Token: 0x0600267B RID: 9851 RVA: 0x000852EE File Offset: 0x000834EE
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600267C RID: 9852 RVA: 0x000852F1 File Offset: 0x000834F1
		protected override string info
		{
			get
			{
				return string.Format("Attack", Array.Empty<object>());
			}
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x00085302 File Offset: 0x00083502
		protected override void OnExecute()
		{
			base.agent.CharacterMainControl.Attack();
			base.EndAction(true);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x0008531C File Offset: 0x0008351C
		protected override void OnStop()
		{
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0008531E File Offset: 0x0008351E
		protected override void OnPause()
		{
		}
	}
}
