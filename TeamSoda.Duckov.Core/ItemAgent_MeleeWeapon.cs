using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class ItemAgent_MeleeWeapon : DuckovItemAgent
{
	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x060007ED RID: 2029 RVA: 0x000240C5 File Offset: 0x000222C5
	public float Damage
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.DamageHash);
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x060007EE RID: 2030 RVA: 0x000240D7 File Offset: 0x000222D7
	public float CritRate
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.CritRateHash);
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x060007EF RID: 2031 RVA: 0x000240E9 File Offset: 0x000222E9
	public float CritDamageFactor
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.CritDamageFactorHash);
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x060007F0 RID: 2032 RVA: 0x000240FB File Offset: 0x000222FB
	public float ArmorPiercing
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.ArmorPiercingHash);
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0002410D File Offset: 0x0002230D
	public float AttackSpeed
	{
		get
		{
			return Mathf.Max(0.1f, base.Item.GetStatValue(ItemAgent_MeleeWeapon.AttackSpeedHash));
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00024129 File Offset: 0x00022329
	public float AttackRange
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.AttackRangeHash);
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0002413B File Offset: 0x0002233B
	public float DealDamageTime
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.DealDamageTimeHash);
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0002414D File Offset: 0x0002234D
	public float StaminaCost
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.StaminaCostHash);
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0002415F File Offset: 0x0002235F
	public float BleedChance
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.BleedChanceHash);
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x060007F6 RID: 2038 RVA: 0x00024171 File Offset: 0x00022371
	public float MoveSpeedMultiplier
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_MeleeWeapon.MoveSpeedMultiplierHash);
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00024183 File Offset: 0x00022383
	public float CharacterDamageMultiplier
	{
		get
		{
			if (!base.Holder)
			{
				return 1f;
			}
			return base.Holder.MeleeDamageMultiplier;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x060007F8 RID: 2040 RVA: 0x000241A3 File Offset: 0x000223A3
	public float CharacterCritRateGain
	{
		get
		{
			if (!base.Holder)
			{
				return 0f;
			}
			return base.Holder.MeleeCritRateGain;
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x060007F9 RID: 2041 RVA: 0x000241C3 File Offset: 0x000223C3
	public float CharacterCritDamageGain
	{
		get
		{
			if (!base.Holder)
			{
				return 0f;
			}
			return base.Holder.MeleeCritDamageGain;
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x060007FA RID: 2042 RVA: 0x000241E3 File Offset: 0x000223E3
	public string SoundKey
	{
		get
		{
			if (string.IsNullOrWhiteSpace(this.soundKey))
			{
				return "Default";
			}
			return this.soundKey;
		}
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00024200 File Offset: 0x00022400
	private int UpdateColliders()
	{
		if (this.colliders == null)
		{
			this.colliders = new Collider[6];
		}
		return Physics.OverlapSphereNonAlloc(base.Holder.transform.position, this.AttackRange + 0.05f, this.colliders, GameplayDataSettings.Layers.damageReceiverLayerMask);
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00024257 File Offset: 0x00022457
	public void CheckAndDealDamage()
	{
		this.CheckCollidersInRange(true);
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00024261 File Offset: 0x00022461
	public bool AttackableTargetInRange()
	{
		return this.CheckCollidersInRange(false) > 0;
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00024270 File Offset: 0x00022470
	private int CheckCollidersInRange(bool dealDamage)
	{
		if (this.colliders == null)
		{
			this.colliders = new Collider[6];
		}
		int num = this.UpdateColliders();
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.colliders[i];
			DamageReceiver component = collider.GetComponent<DamageReceiver>();
			if (!(component == null) && Team.IsEnemy(component.Team, base.Holder.Team))
			{
				Health health = component.health;
				if (health)
				{
					CharacterMainControl characterMainControl = health.TryGetCharacter();
					if (characterMainControl == base.Holder || (characterMainControl && characterMainControl.Dashing))
					{
						goto IL_30A;
					}
				}
				Vector3 vector = collider.transform.position - base.Holder.transform.position;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				vector.Normalize();
				if (Vector3.Angle(vector, base.Holder.CurrentAimDirection) < 90f || magnitude < 0.6f)
				{
					num2++;
					if (dealDamage)
					{
						DamageInfo damageInfo = new DamageInfo(base.Holder);
						damageInfo.damageValue = this.Damage * this.CharacterDamageMultiplier;
						damageInfo.armorPiercing = this.ArmorPiercing;
						damageInfo.critDamageFactor = this.CritDamageFactor * (1f + this.CharacterCritDamageGain);
						damageInfo.critRate = this.CritRate * (1f + this.CharacterCritRateGain);
						damageInfo.crit = -1;
						damageInfo.damageNormal = -base.Holder.modelRoot.right;
						damageInfo.damagePoint = collider.transform.position - vector * 0.2f;
						damageInfo.damagePoint.y = base.transform.position.y;
						damageInfo.fromWeaponItemID = base.Item.TypeID;
						damageInfo.bleedChance = this.BleedChance;
						if (this.setting)
						{
							damageInfo.isExplosion = this.setting.dealExplosionDamage;
							damageInfo.elementFactors.Add(new ElementFactor(this.setting.element, 1f));
							damageInfo.buff = this.setting.buff;
							damageInfo.buffChance = this.setting.buffChance;
						}
						component.Hurt(damageInfo);
						component.AddBuff(GameplayDataSettings.Buffs.Pain, base.Holder);
						if (this.hitFx)
						{
							Object.Instantiate<GameObject>(this.hitFx, damageInfo.damagePoint, Quaternion.LookRotation(damageInfo.damageNormal, Vector3.up));
						}
						if (base.Holder && base.Holder == CharacterMainControl.Main)
						{
							Vector3 vector2 = base.Holder.modelRoot.right;
							vector2 += Random.insideUnitSphere * 0.3f;
							vector2.Normalize();
							CameraShaker.Shake(vector2 * 0.05f, CameraShaker.CameraShakeTypes.meleeAttackHit);
						}
					}
				}
			}
			IL_30A:;
		}
		return num2;
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00024593 File Offset: 0x00022793
	private void Update()
	{
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00024595 File Offset: 0x00022795
	protected override void OnInitialize()
	{
		base.OnInitialize();
		this.setting = base.Item.GetComponent<ItemSetting_MeleeWeapon>();
	}

	// Token: 0x0400078A RID: 1930
	public GameObject hitFx;

	// Token: 0x0400078B RID: 1931
	public GameObject slashFx;

	// Token: 0x0400078C RID: 1932
	public float slashFxDelayTime = 0.05f;

	// Token: 0x0400078D RID: 1933
	[SerializeField]
	private string soundKey = "Default";

	// Token: 0x0400078E RID: 1934
	private Collider[] colliders;

	// Token: 0x0400078F RID: 1935
	private ItemSetting_MeleeWeapon setting;

	// Token: 0x04000790 RID: 1936
	private static int DamageHash = "Damage".GetHashCode();

	// Token: 0x04000791 RID: 1937
	private static int CritRateHash = "CritRate".GetHashCode();

	// Token: 0x04000792 RID: 1938
	private static int CritDamageFactorHash = "CritDamageFactor".GetHashCode();

	// Token: 0x04000793 RID: 1939
	private static int ArmorPiercingHash = "ArmorPiercing".GetHashCode();

	// Token: 0x04000794 RID: 1940
	private static int AttackSpeedHash = "AttackSpeed".GetHashCode();

	// Token: 0x04000795 RID: 1941
	private static int AttackRangeHash = "AttackRange".GetHashCode();

	// Token: 0x04000796 RID: 1942
	private static int DealDamageTimeHash = "DealDamageTime".GetHashCode();

	// Token: 0x04000797 RID: 1943
	private static int StaminaCostHash = "StaminaCost".GetHashCode();

	// Token: 0x04000798 RID: 1944
	private static int BleedChanceHash = "BleedChance".GetHashCode();

	// Token: 0x04000799 RID: 1945
	private static int MoveSpeedMultiplierHash = "MoveSpeedMultiplier".GetHashCode();
}
