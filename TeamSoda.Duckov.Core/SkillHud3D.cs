using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class SkillHud3D : MonoBehaviour
{
	// Token: 0x06000685 RID: 1669 RVA: 0x0001D9C2 File Offset: 0x0001BBC2
	private void Awake()
	{
		this.HideAll();
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0001D9CA File Offset: 0x0001BBCA
	private void HideAll()
	{
		this.skillRangeHUD.gameObject.SetActive(false);
		this.projectileLine.gameObject.SetActive(false);
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0001D9F0 File Offset: 0x0001BBF0
	private void LateUpdate()
	{
		if (!this.character)
		{
			this.character = LevelManager.Instance.MainCharacter;
			return;
		}
		this.currentSkill = null;
		this.currentSkill = this.character.skillAction.CurrentRunningSkill;
		if (this.aiming != (this.currentSkill != null))
		{
			this.aiming = !this.aiming;
			if (this.currentSkill != null)
			{
				this.currentSkill = this.character.skillAction.CurrentRunningSkill;
				this.skillRangeHUD.gameObject.SetActive(true);
				float range = 1f;
				if (this.currentSkill.SkillContext.effectRange > 1f)
				{
					range = this.currentSkill.SkillContext.effectRange;
				}
				this.skillRangeHUD.SetRange(range);
				if (this.currentSkill.SkillContext.isGrenade)
				{
					this.projectileLine.gameObject.SetActive(true);
				}
			}
			else
			{
				this.HideAll();
			}
		}
		Vector3 currentSkillAimPoint = this.character.GetCurrentSkillAimPoint();
		Vector3 one = Vector3.one;
		if (this.projectileLine.gameObject.activeSelf)
		{
			bool flag = this.projectileLine.UpdateLine(this.character.CurrentUsingAimSocket.position, currentSkillAimPoint, this.currentSkill.SkillContext.grenageVerticleSpeed, ref one);
		}
		this.skillRangeHUD.transform.position = currentSkillAimPoint;
		this.skillRangeHUD.SetProgress(this.character.skillAction.GetProgress().progress);
	}

	// Token: 0x0400065A RID: 1626
	private CharacterMainControl character;

	// Token: 0x0400065B RID: 1627
	private bool aiming;

	// Token: 0x0400065C RID: 1628
	public SkillRangeHUD skillRangeHUD;

	// Token: 0x0400065D RID: 1629
	public SkillProjectileLineHUD projectileLine;

	// Token: 0x0400065E RID: 1630
	private SkillBase currentSkill;
}
