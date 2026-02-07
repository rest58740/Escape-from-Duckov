using System;
using UnityEngine;

// Token: 0x02000134 RID: 308
[Serializable]
public class CharacterSkillKeeper
{
	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0002C426 File Offset: 0x0002A626
	public SkillBase Skill
	{
		get
		{
			return this.skill;
		}
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x0002C42E File Offset: 0x0002A62E
	public void SetSkill(SkillBase _skill, GameObject _bindingObject)
	{
		this.skill = null;
		this.skillBindingObject = null;
		if (_skill != null && _bindingObject != null)
		{
			this.skill = _skill;
			this.skillBindingObject = _bindingObject;
		}
		Action onSkillChanged = this.OnSkillChanged;
		if (onSkillChanged == null)
		{
			return;
		}
		onSkillChanged();
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x0002C46E File Offset: 0x0002A66E
	public bool CheckSkillAndBinding()
	{
		if (this.skill != null && this.skillBindingObject != null)
		{
			return true;
		}
		this.skill = null;
		this.skillBindingObject = null;
		return false;
	}

	// Token: 0x040008FA RID: 2298
	private SkillBase skill;

	// Token: 0x040008FB RID: 2299
	private GameObject skillBindingObject;

	// Token: 0x040008FC RID: 2300
	public Action OnSkillChanged;
}
