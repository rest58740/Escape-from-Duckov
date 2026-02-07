using System;
using Duckov;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000137 RID: 311
public abstract class SkillBase : MonoBehaviour
{
	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0002C4A5 File Offset: 0x0002A6A5
	public float LastReleaseTime
	{
		get
		{
			return this.lastReleaseTime;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0002C4AD File Offset: 0x0002A6AD
	public SkillContext SkillContext
	{
		get
		{
			return this.skillContext;
		}
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x0002C4B8 File Offset: 0x0002A6B8
	public void ReleaseSkill(SkillReleaseContext releaseContext, CharacterMainControl from)
	{
		this.lastReleaseTime = Time.time;
		this.skillReleaseContext = releaseContext;
		this.fromCharacter = from;
		this.fromCharacter.UseStamina(this.staminaCost);
		if (this.hasReleaseSound && this.fromCharacter != null && this.onReleaseSound != "")
		{
			AudioManager.Post(this.onReleaseSound, from.gameObject);
		}
		this.OnRelease();
		Action onSkillReleasedEvent = this.OnSkillReleasedEvent;
		if (onSkillReleasedEvent == null)
		{
			return;
		}
		onSkillReleasedEvent();
	}

	// Token: 0x06000A41 RID: 2625
	public abstract void OnRelease();

	// Token: 0x04000906 RID: 2310
	public bool hasReleaseSound;

	// Token: 0x04000907 RID: 2311
	public string onReleaseSound;

	// Token: 0x04000908 RID: 2312
	public Sprite icon;

	// Token: 0x04000909 RID: 2313
	public float staminaCost = 10f;

	// Token: 0x0400090A RID: 2314
	public float coolDownTime = 1f;

	// Token: 0x0400090B RID: 2315
	private float lastReleaseTime = -999f;

	// Token: 0x0400090C RID: 2316
	[SerializeField]
	protected SkillContext skillContext;

	// Token: 0x0400090D RID: 2317
	protected SkillReleaseContext skillReleaseContext;

	// Token: 0x0400090E RID: 2318
	protected CharacterMainControl fromCharacter;

	// Token: 0x0400090F RID: 2319
	[HideInInspector]
	public Item fromItem;

	// Token: 0x04000910 RID: 2320
	public Action OnSkillReleasedEvent;
}
