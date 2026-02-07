using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000066 RID: 102
public class HealthSimpleBase : MonoBehaviour
{
	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060003D6 RID: 982 RVA: 0x00011181 File Offset: 0x0000F381
	public float HealthValue
	{
		get
		{
			return this.healthValue;
		}
	}

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x060003D7 RID: 983 RVA: 0x0001118C File Offset: 0x0000F38C
	// (remove) Token: 0x060003D8 RID: 984 RVA: 0x000111C4 File Offset: 0x0000F3C4
	public event Action<DamageInfo> OnHurtEvent;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x060003D9 RID: 985 RVA: 0x000111FC File Offset: 0x0000F3FC
	// (remove) Token: 0x060003DA RID: 986 RVA: 0x00011230 File Offset: 0x0000F430
	public static event Action<HealthSimpleBase, DamageInfo> OnSimpleHealthHit;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060003DB RID: 987 RVA: 0x00011264 File Offset: 0x0000F464
	// (remove) Token: 0x060003DC RID: 988 RVA: 0x0001129C File Offset: 0x0000F49C
	public event Action<DamageInfo> OnDeadEvent;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x060003DD RID: 989 RVA: 0x000112D4 File Offset: 0x0000F4D4
	// (remove) Token: 0x060003DE RID: 990 RVA: 0x00011308 File Offset: 0x0000F508
	public static event Action<HealthSimpleBase, DamageInfo> OnSimpleHealthDead;

	// Token: 0x060003DF RID: 991 RVA: 0x0001133B File Offset: 0x0000F53B
	private void Awake()
	{
		this.healthValue = this.maxHealthValue;
		this.dmgReceiver.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x00011368 File Offset: 0x0000F568
	private void OnHurt(DamageInfo dmgInfo)
	{
		if (this.onlyReceiveExplosion && !dmgInfo.isExplosion)
		{
			return;
		}
		float num = 1f;
		bool flag = Random.Range(0f, 1f) <= dmgInfo.critRate;
		dmgInfo.crit = (flag ? 1 : 0);
		if (!dmgInfo.fromCharacter || !dmgInfo.fromCharacter.IsMainCharacter)
		{
			num = this.damageMultiplierIfNotMainCharacter;
		}
		this.healthValue -= (flag ? dmgInfo.critDamageFactor : 1f) * dmgInfo.damageValue * num;
		Action<DamageInfo> onHurtEvent = this.OnHurtEvent;
		if (onHurtEvent != null)
		{
			onHurtEvent(dmgInfo);
		}
		Action<HealthSimpleBase, DamageInfo> onSimpleHealthHit = HealthSimpleBase.OnSimpleHealthHit;
		if (onSimpleHealthHit != null)
		{
			onSimpleHealthHit(this, dmgInfo);
		}
		if (this.healthValue <= 0f)
		{
			this.Dead(dmgInfo);
		}
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x00011434 File Offset: 0x0000F634
	private void Dead(DamageInfo dmgInfo)
	{
		this.dmgReceiver.OnDead(dmgInfo);
		Action<DamageInfo> onDeadEvent = this.OnDeadEvent;
		if (onDeadEvent != null)
		{
			onDeadEvent(dmgInfo);
		}
		Action<HealthSimpleBase, DamageInfo> onSimpleHealthDead = HealthSimpleBase.OnSimpleHealthDead;
		if (onSimpleHealthDead == null)
		{
			return;
		}
		onSimpleHealthDead(this, dmgInfo);
	}

	// Token: 0x040002F0 RID: 752
	public Teams team;

	// Token: 0x040002F1 RID: 753
	public bool onlyReceiveExplosion;

	// Token: 0x040002F2 RID: 754
	public float maxHealthValue = 250f;

	// Token: 0x040002F3 RID: 755
	private float healthValue;

	// Token: 0x040002F4 RID: 756
	public DamageReceiver dmgReceiver;

	// Token: 0x040002F8 RID: 760
	public float damageMultiplierIfNotMainCharacter = 1f;
}
