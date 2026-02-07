using System;
using NodeCanvas.Framework;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000428 RID: 1064
	public class FindMainCharacter : ActionTask<AICharacterController>
	{
		// Token: 0x0600268F RID: 9871 RVA: 0x0008569C File Offset: 0x0008389C
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x0008569F File Offset: 0x0008389F
		protected override void OnExecute()
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			this.mainCharacter.value = LevelManager.Instance.MainCharacter;
			if (this.mainCharacter.value != null)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000856DE File Offset: 0x000838DE
		protected override void OnUpdate()
		{
			if (LevelManager.Instance == null)
			{
				return;
			}
			this.mainCharacter.value = LevelManager.Instance.MainCharacter;
			if (this.mainCharacter.value != null)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x0008571D File Offset: 0x0008391D
		protected override void OnStop()
		{
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0008571F File Offset: 0x0008391F
		protected override void OnPause()
		{
		}

		// Token: 0x04001A42 RID: 6722
		public BBParameter<CharacterMainControl> mainCharacter;
	}
}
