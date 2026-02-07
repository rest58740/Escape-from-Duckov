using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200041C RID: 1052
	public class CheckCanReleaseSkill : ConditionTask<AICharacterController>
	{
		// Token: 0x06002657 RID: 9815 RVA: 0x00084EB0 File Offset: 0x000830B0
		protected override bool OnCheck()
		{
			if (base.agent == null)
			{
				return false;
			}
			if (!base.agent.hasSkill)
			{
				return false;
			}
			if (!base.agent.skillInstance)
			{
				return false;
			}
			if (Time.time < base.agent.nextReleaseSkillTimeMarker)
			{
				return false;
			}
			if (!base.agent.CharacterMainControl.skillAction.IsSkillHasEnoughStaminaAndCD(base.agent.skillInstance))
			{
				return false;
			}
			if (base.agent.CharacterMainControl.CurrentAction && base.agent.CharacterMainControl.CurrentAction.Running)
			{
				return false;
			}
			base.agent.nextReleaseSkillTimeMarker = Time.time + Random.Range(base.agent.skillCoolTimeRange.x, base.agent.skillCoolTimeRange.y);
			return Random.Range(0f, 1f) <= base.agent.skillSuccessChance;
		}
	}
}
