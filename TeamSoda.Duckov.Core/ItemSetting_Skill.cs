using System;
using ItemStatsSystem;

// Token: 0x020000FA RID: 250
public class ItemSetting_Skill : ItemSettingBase
{
	// Token: 0x06000853 RID: 2131 RVA: 0x00025938 File Offset: 0x00023B38
	public override void OnInit()
	{
		if (this.Skill)
		{
			SkillBase skill = this.Skill;
			skill.OnSkillReleasedEvent = (Action)Delegate.Combine(skill.OnSkillReleasedEvent, new Action(this.OnSkillReleased));
			this.Skill.fromItem = base.Item;
		}
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0002598C File Offset: 0x00023B8C
	private void OnSkillReleased()
	{
		ItemSetting_Skill.OnReleaseAction onReleaseAction = this.onRelease;
		if (onReleaseAction != ItemSetting_Skill.OnReleaseAction.none && onReleaseAction == ItemSetting_Skill.OnReleaseAction.reduceCount && (!LevelManager.Instance || !LevelManager.Instance.IsBaseLevel))
		{
			if (base.Item.Stackable)
			{
				base.Item.StackCount--;
				return;
			}
			base.Item.Detach();
			ItemTreeExtensions.DestroyTree(base.Item);
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x000259F6 File Offset: 0x00023BF6
	private void OnDestroy()
	{
		if (this.Skill)
		{
			SkillBase skill = this.Skill;
			skill.OnSkillReleasedEvent = (Action)Delegate.Remove(skill.OnSkillReleasedEvent, new Action(this.OnSkillReleased));
		}
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00025A2C File Offset: 0x00023C2C
	public override void SetMarkerParam(Item selfItem)
	{
		selfItem.SetBool("IsSkill", true, true);
	}

	// Token: 0x040007C9 RID: 1993
	public ItemSetting_Skill.OnReleaseAction onRelease;

	// Token: 0x040007CA RID: 1994
	public SkillBase Skill;

	// Token: 0x0200048D RID: 1165
	public enum OnReleaseAction
	{
		// Token: 0x04001C39 RID: 7225
		none,
		// Token: 0x04001C3A RID: 7226
		reduceCount
	}
}
