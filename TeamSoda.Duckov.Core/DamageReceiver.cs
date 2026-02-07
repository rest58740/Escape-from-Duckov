using System;
using Duckov.Buffs;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200006F RID: 111
public class DamageReceiver : MonoBehaviour
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000439 RID: 1081 RVA: 0x00012C58 File Offset: 0x00010E58
	public Teams Team
	{
		get
		{
			if (!this.useSimpleHealth && this.health)
			{
				return this.health.team;
			}
			if (this.useSimpleHealth && this.simpleHealth)
			{
				return this.simpleHealth.team;
			}
			return Teams.all;
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x0600043A RID: 1082 RVA: 0x00012CA8 File Offset: 0x00010EA8
	public bool IsMainCharacter
	{
		get
		{
			return !this.useSimpleHealth && this.health && this.health.IsMainCharacterHealth;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x0600043B RID: 1083 RVA: 0x00012CCC File Offset: 0x00010ECC
	public bool IsDead
	{
		get
		{
			return this.health && this.health.IsDead;
		}
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00012CE8 File Offset: 0x00010EE8
	private void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("DamageReceiver");
		if (this.health)
		{
			this.health.OnDeadEvent.AddListener(new UnityAction<DamageInfo>(this.OnDead));
		}
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00012D28 File Offset: 0x00010F28
	private void OnDestroy()
	{
		if (this.health)
		{
			this.health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnDead));
		}
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00012D53 File Offset: 0x00010F53
	public bool Hurt(DamageInfo damageInfo)
	{
		damageInfo.toDamageReceiver = this;
		UnityEvent<DamageInfo> onHurtEvent = this.OnHurtEvent;
		if (onHurtEvent != null)
		{
			onHurtEvent.Invoke(damageInfo);
		}
		if (this.health)
		{
			this.health.Hurt(damageInfo);
		}
		return true;
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00012D8C File Offset: 0x00010F8C
	public bool AddBuff(Buff buffPfb, CharacterMainControl fromWho)
	{
		if (this.useSimpleHealth)
		{
			return false;
		}
		if (!this.health)
		{
			return false;
		}
		CharacterMainControl characterMainControl = this.health.TryGetCharacter();
		if (!characterMainControl)
		{
			return false;
		}
		characterMainControl.AddBuff(buffPfb, fromWho, 0);
		return true;
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00012DD2 File Offset: 0x00010FD2
	public void OnDead(DamageInfo dmgInfo)
	{
		base.gameObject.SetActive(false);
		UnityEvent<DamageInfo> onDeadEvent = this.OnDeadEvent;
		if (onDeadEvent == null)
		{
			return;
		}
		onDeadEvent.Invoke(dmgInfo);
	}

	// Token: 0x04000343 RID: 835
	public bool useSimpleHealth;

	// Token: 0x04000344 RID: 836
	public Health health;

	// Token: 0x04000345 RID: 837
	public HealthSimpleBase simpleHealth;

	// Token: 0x04000346 RID: 838
	public bool isHalfObsticle;

	// Token: 0x04000347 RID: 839
	public UnityEvent<DamageInfo> OnHurtEvent;

	// Token: 0x04000348 RID: 840
	public UnityEvent<DamageInfo> OnDeadEvent;
}
