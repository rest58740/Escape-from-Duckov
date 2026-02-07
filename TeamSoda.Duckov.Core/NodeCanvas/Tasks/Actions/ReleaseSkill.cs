using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200042E RID: 1070
	public class ReleaseSkill : ActionTask<AICharacterController>
	{
		// Token: 0x060026B5 RID: 9909 RVA: 0x00085ED9 File Offset: 0x000840D9
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x00085EDC File Offset: 0x000840DC
		protected override void OnExecute()
		{
			base.agent.CharacterMainControl.SetSkill(SkillTypes.characterSkill, base.agent.skillInstance, base.agent.skillInstance.gameObject);
			if (!base.agent.CharacterMainControl.StartSkillAim(SkillTypes.characterSkill))
			{
				base.EndAction(false);
				return;
			}
			this.readyTime = base.agent.skillInstance.SkillContext.skillReadyTime;
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x00085F4C File Offset: 0x0008414C
		protected override void OnUpdate()
		{
			if (base.agent.searchedEnemy)
			{
				base.agent.CharacterMainControl.SetAimPoint(base.agent.searchedEnemy.transform.position);
			}
			if (base.elapsedTime <= this.readyTime + 0.1f)
			{
				return;
			}
			if (Random.Range(0f, 1f) < base.agent.skillSuccessChance)
			{
				base.agent.CharacterMainControl.ReleaseSkill(SkillTypes.characterSkill);
				base.EndAction(true);
				return;
			}
			base.agent.CharacterMainControl.CancleSkill();
			base.EndAction(false);
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x00085FF3 File Offset: 0x000841F3
		protected override void OnStop()
		{
			base.agent.CharacterMainControl.CancleSkill();
			base.agent.CharacterMainControl.SwitchToFirstAvailableWeapon();
		}

		// Token: 0x04001A58 RID: 6744
		private float readyTime;

		// Token: 0x04001A59 RID: 6745
		private float tryReleaseSkillTimeMarker = -1f;
	}
}
