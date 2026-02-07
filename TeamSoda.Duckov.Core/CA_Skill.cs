using System;
using Duckov;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class CA_Skill : CharacterActionBase, IProgress
{
	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600023D RID: 573 RVA: 0x0000A690 File Offset: 0x00008890
	public SkillBase CurrentRunningSkill
	{
		get
		{
			if (!base.Running || this.currentRunningSkillKeeper == null)
			{
				return null;
			}
			return this.currentRunningSkillKeeper.Skill;
		}
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000A6AF File Offset: 0x000088AF
	public CharacterSkillKeeper GetSkillKeeper(SkillTypes skillType)
	{
		if (skillType == SkillTypes.itemSkill)
		{
			return this.holdItemSkillKeeper;
		}
		if (skillType != SkillTypes.characterSkill)
		{
			return null;
		}
		return this.characterSkillKeeper;
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000A6C9 File Offset: 0x000088C9
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Skills;
	}

	// Token: 0x06000240 RID: 576 RVA: 0x0000A6CC File Offset: 0x000088CC
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000A6CF File Offset: 0x000088CF
	public override bool CanEditInventory()
	{
		return false;
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000A6D2 File Offset: 0x000088D2
	public override bool CanMove()
	{
		return !(this.CurrentRunningSkill != null) || this.CurrentRunningSkill.SkillContext.movableWhileAim;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000A6F4 File Offset: 0x000088F4
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000A6F7 File Offset: 0x000088F7
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000A6FA File Offset: 0x000088FA
	public override bool IsReady()
	{
		return !base.Running;
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000A707 File Offset: 0x00008907
	public bool IsSkillHasEnoughStaminaAndCD(SkillBase skill)
	{
		return this.characterController.CurrentStamina >= skill.staminaCost && Time.time - skill.LastReleaseTime >= skill.coolDownTime;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000A738 File Offset: 0x00008938
	protected override bool OnStart()
	{
		CharacterSkillKeeper skillKeeper = this.GetSkillKeeper(this.skillTypeToRelease);
		if (skillKeeper != null && skillKeeper.CheckSkillAndBinding())
		{
			if (skillKeeper.Skill != null)
			{
				if (!this.IsSkillHasEnoughStaminaAndCD(skillKeeper.Skill))
				{
					return false;
				}
				SkillContext skillContext = skillKeeper.Skill.SkillContext;
			}
			this.currentRunningSkillKeeper = skillKeeper;
			Debug.Log(string.Format("skillType is {0}", this.skillTypeToRelease));
			return true;
		}
		return false;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000A7AB File Offset: 0x000089AB
	public void SetNextSkillType(SkillTypes skillType)
	{
		if (base.Running)
		{
			return;
		}
		this.skillTypeToRelease = skillType;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000A7C0 File Offset: 0x000089C0
	public bool SetSkillOfType(SkillTypes skillType, SkillBase _skill, GameObject _bindingObject)
	{
		CharacterSkillKeeper skillKeeper = this.GetSkillKeeper(skillType);
		if (skillKeeper == null)
		{
			return false;
		}
		if (base.Running && skillKeeper == this.currentRunningSkillKeeper)
		{
			base.StopAction();
		}
		skillKeeper.SetSkill(_skill, _bindingObject);
		return true;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000A7FC File Offset: 0x000089FC
	public bool ReleaseSkill(SkillTypes skillType)
	{
		if (!base.Running)
		{
			return false;
		}
		if (this.CurrentRunningSkill == null)
		{
			base.StopAction();
			return false;
		}
		if (skillType != this.skillTypeToRelease)
		{
			base.StopAction();
			return false;
		}
		if (!this.IsSkillHasEnoughStaminaAndCD(this.CurrentRunningSkill))
		{
			return false;
		}
		if (this.actionTimer < this.CurrentRunningSkill.SkillContext.skillReadyTime)
		{
			base.StopAction();
			return false;
		}
		Vector3 currentSkillAimPoint = this.characterController.GetCurrentSkillAimPoint();
		SkillReleaseContext releaseContext = default(SkillReleaseContext);
		releaseContext.releasePoint = currentSkillAimPoint;
		this.CurrentRunningSkill.ReleaseSkill(releaseContext, this.characterController);
		this.currentRunningSkillKeeper = null;
		base.StopAction();
		return true;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000A8AA File Offset: 0x00008AAA
	protected override void OnStop()
	{
		this.currentRunningSkillKeeper = null;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0000A8B3 File Offset: 0x00008AB3
	protected override void OnUpdateAction(float deltaTime)
	{
		if (this.currentRunningSkillKeeper == null || !this.currentRunningSkillKeeper.CheckSkillAndBinding())
		{
			base.StopAction();
		}
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000A8D4 File Offset: 0x00008AD4
	public Progress GetProgress()
	{
		Progress result = default(Progress);
		SkillBase currentRunningSkill = this.CurrentRunningSkill;
		if (currentRunningSkill != null)
		{
			result.total = currentRunningSkill.SkillContext.skillReadyTime;
			result.current = this.actionTimer;
			result.inProgress = (result.progress < 1f);
		}
		else
		{
			result.inProgress = false;
		}
		return result;
	}

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	public CharacterSkillKeeper holdItemSkillKeeper;

	// Token: 0x040001D5 RID: 469
	[SerializeField]
	public CharacterSkillKeeper characterSkillKeeper;

	// Token: 0x040001D6 RID: 470
	private SkillTypes skillTypeToRelease;

	// Token: 0x040001D7 RID: 471
	private CharacterSkillKeeper currentRunningSkillKeeper;
}
