using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Buffs;
using Duckov.Scenes;
using Duckov.Utilities;
using Duckov.Weathers;
using FX;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000065 RID: 101
public class Health : MonoBehaviour
{
	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060003B5 RID: 949 RVA: 0x0001054E File Offset: 0x0000E74E
	// (set) Token: 0x060003B4 RID: 948 RVA: 0x00010545 File Offset: 0x0000E745
	public bool showHealthBar
	{
		get
		{
			return this._showHealthBar;
		}
		set
		{
			this._showHealthBar = value;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060003B6 RID: 950 RVA: 0x00010556 File Offset: 0x0000E756
	public bool Hidden
	{
		get
		{
			return this.TryGetCharacter() && this.characterCached.Hidden;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060003B7 RID: 951 RVA: 0x00010574 File Offset: 0x0000E774
	public float MaxHealth
	{
		get
		{
			float num;
			if (this.item)
			{
				num = this.item.GetStatValue(this.maxHealthHash);
			}
			else
			{
				num = (float)this.defaultMaxHealth;
			}
			if (!Mathf.Approximately(this.lastMaxHealth, num))
			{
				this.lastMaxHealth = num;
				UnityEvent<Health> onMaxHealthChange = this.OnMaxHealthChange;
				if (onMaxHealthChange != null)
				{
					onMaxHealthChange.Invoke(this);
				}
			}
			return num;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060003B8 RID: 952 RVA: 0x000105D8 File Offset: 0x0000E7D8
	public bool IsMainCharacterHealth
	{
		get
		{
			return !(LevelManager.Instance == null) && !(LevelManager.Instance.MainCharacter == null) && !(LevelManager.Instance.MainCharacter != this.TryGetCharacter());
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060003B9 RID: 953 RVA: 0x00010617 File Offset: 0x0000E817
	// (set) Token: 0x060003BA RID: 954 RVA: 0x0001061F File Offset: 0x0000E81F
	public bool CanDieIfNotRaidMap
	{
		get
		{
			return this.canDieIfNotRaidMap;
		}
		set
		{
			this.canDieIfNotRaidMap = value;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060003BB RID: 955 RVA: 0x00010628 File Offset: 0x0000E828
	// (set) Token: 0x060003BC RID: 956 RVA: 0x00010630 File Offset: 0x0000E830
	public float CurrentHealth
	{
		get
		{
			return this._currentHealth;
		}
		set
		{
			float currentHealth = this._currentHealth;
			this._currentHealth = value;
			if (this._currentHealth != currentHealth)
			{
				UnityEvent<Health> onHealthChange = this.OnHealthChange;
				if (onHealthChange == null)
				{
					return;
				}
				onHealthChange.Invoke(this);
			}
		}
	}

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x060003BD RID: 957 RVA: 0x00010668 File Offset: 0x0000E868
	// (remove) Token: 0x060003BE RID: 958 RVA: 0x0001069C File Offset: 0x0000E89C
	public static event Action<Health, DamageInfo> OnHurt;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x060003BF RID: 959 RVA: 0x000106D0 File Offset: 0x0000E8D0
	// (remove) Token: 0x060003C0 RID: 960 RVA: 0x00010704 File Offset: 0x0000E904
	public static event Action<Health, DamageInfo> OnDead;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x060003C1 RID: 961 RVA: 0x00010738 File Offset: 0x0000E938
	// (remove) Token: 0x060003C2 RID: 962 RVA: 0x0001076C File Offset: 0x0000E96C
	public static event Action<Health> OnRequestHealthBar;

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060003C3 RID: 963 RVA: 0x0001079F File Offset: 0x0000E99F
	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060003C4 RID: 964 RVA: 0x000107A7 File Offset: 0x0000E9A7
	public bool Invincible
	{
		get
		{
			return this.invincible;
		}
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x000107B0 File Offset: 0x0000E9B0
	public CharacterMainControl TryGetCharacter()
	{
		if (this.characterCached != null)
		{
			return this.characterCached;
		}
		if (!this.hasCharacter)
		{
			return null;
		}
		if (!this.item)
		{
			this.hasCharacter = false;
			return null;
		}
		this.characterCached = this.item.GetCharacterMainControl();
		if (!this.characterCached)
		{
			this.hasCharacter = true;
		}
		return this.characterCached;
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060003C6 RID: 966 RVA: 0x0001081D File Offset: 0x0000EA1D
	public float BodyArmor
	{
		get
		{
			if (this.item)
			{
				return this.item.GetStatValue(this.bodyArmorHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060003C7 RID: 967 RVA: 0x00010843 File Offset: 0x0000EA43
	public float HeadArmor
	{
		get
		{
			if (this.item)
			{
				return this.item.GetStatValue(this.headArmorHash);
			}
			return 0f;
		}
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0001086C File Offset: 0x0000EA6C
	public float ElementFactor(ElementTypes type)
	{
		float num = 1f;
		if (!this.item)
		{
			return num;
		}
		Weather currentWeather = TimeOfDayController.Instance.CurrentWeather;
		bool isBaseLevel = LevelManager.Instance.IsBaseLevel;
		switch (type)
		{
		case ElementTypes.physics:
			num = this.item.GetStat(this.Hash_ElementFactor_Physics).Value;
			break;
		case ElementTypes.fire:
			num = this.item.GetStat(this.Hash_ElementFactor_Fire).Value;
			if (!isBaseLevel && currentWeather == Weather.Rainy)
			{
				num -= 0.15f;
			}
			break;
		case ElementTypes.poison:
			num = this.item.GetStat(this.Hash_ElementFactor_Poison).Value;
			break;
		case ElementTypes.electricity:
			num = this.item.GetStat(this.Hash_ElementFactor_Electricity).Value;
			if (!isBaseLevel && currentWeather == Weather.Rainy)
			{
				num += 0.2f;
			}
			break;
		case ElementTypes.space:
			num = this.item.GetStat(this.Hash_ElementFactor_Space).Value;
			break;
		case ElementTypes.ghost:
			num = this.item.GetStat(this.Hash_ElementFactor_Ghost).Value;
			break;
		case ElementTypes.ice:
			num = this.item.GetStat(this.Hash_ElementFactor_Ice).Value;
			break;
		}
		return num;
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x000109A3 File Offset: 0x0000EBA3
	private void Start()
	{
		if (this.autoInit)
		{
			this.Init();
		}
	}

	// Token: 0x060003CA RID: 970 RVA: 0x000109B3 File Offset: 0x0000EBB3
	public void SetItemAndCharacter(Item _item, CharacterMainControl _character)
	{
		this.item = _item;
		if (_character)
		{
			this.hasCharacter = true;
			this.characterCached = _character;
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x000109D2 File Offset: 0x0000EBD2
	public void Init()
	{
		if (this.CurrentHealth <= 0f)
		{
			this.CurrentHealth = this.MaxHealth;
		}
	}

	// Token: 0x060003CC RID: 972 RVA: 0x000109ED File Offset: 0x0000EBED
	public void AddBuff(Buff buffPfb, CharacterMainControl fromWho, int overrideFromWeaponID = 0)
	{
		CharacterMainControl characterMainControl = this.TryGetCharacter();
		if (characterMainControl == null)
		{
			return;
		}
		characterMainControl.AddBuff(buffPfb, fromWho, overrideFromWeaponID);
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00010A02 File Offset: 0x0000EC02
	private void Update()
	{
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00010A04 File Offset: 0x0000EC04
	public bool Hurt(DamageInfo damageInfo)
	{
		if (MultiSceneCore.Instance != null && MultiSceneCore.Instance.IsLoading)
		{
			return false;
		}
		if (this.invincible)
		{
			return false;
		}
		if (this.isDead)
		{
			return false;
		}
		if (damageInfo.buff != null && Random.Range(0f, 1f) < damageInfo.buffChance)
		{
			this.AddBuff(damageInfo.buff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
		}
		bool flag = LevelManager.Rule.AdvancedDebuffMode;
		if (LevelManager.Instance.IsBaseLevel)
		{
			flag = false;
		}
		float num = 0.2f;
		float num2 = 0.12f;
		CharacterMainControl characterMainControl = this.TryGetCharacter();
		if (!this.IsMainCharacterHealth)
		{
			num = 0.1f;
			num2 = 0.1f;
		}
		if (flag && Random.Range(0f, 1f) < damageInfo.bleedChance * num)
		{
			this.AddBuff(GameplayDataSettings.Buffs.BoneCrackBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
		}
		else if (flag && Random.Range(0f, 1f) < damageInfo.bleedChance * num2)
		{
			this.AddBuff(GameplayDataSettings.Buffs.WoundBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
		}
		else if (Random.Range(0f, 1f) < damageInfo.bleedChance)
		{
			if (flag)
			{
				this.AddBuff(GameplayDataSettings.Buffs.UnlimitBleedBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
			}
			else
			{
				this.AddBuff(GameplayDataSettings.Buffs.BleedSBuff, damageInfo.fromCharacter, damageInfo.fromWeaponItemID);
			}
		}
		bool flag2 = Random.Range(0f, 1f) < damageInfo.critRate;
		damageInfo.crit = (flag2 ? 1 : 0);
		if (!damageInfo.ignoreDifficulty && this.team == Teams.player)
		{
			damageInfo.damageValue *= LevelManager.Rule.DamageFactor_ToPlayer;
		}
		float num3 = damageInfo.damageValue * (flag2 ? damageInfo.critDamageFactor : 1f);
		if (damageInfo.damageType != DamageTypes.realDamage && !damageInfo.ignoreArmor)
		{
			float num4 = flag2 ? this.HeadArmor : this.BodyArmor;
			if (characterMainControl && LevelManager.Instance.IsRaidMap)
			{
				Item item = flag2 ? characterMainControl.GetHelmatItem() : characterMainControl.GetArmorItem();
				if (item)
				{
					item.Durability = Mathf.Max(0f, item.Durability - damageInfo.armorBreak);
				}
			}
			float num5 = 1f;
			if (num4 > 0f)
			{
				num5 = 2f / (Mathf.Clamp(num4 - damageInfo.armorPiercing, 0f, 999f) + 2f);
			}
			if (characterMainControl && !characterMainControl.IsMainCharacter && damageInfo.fromCharacter && !damageInfo.fromCharacter.IsMainCharacter)
			{
				CharacterRandomPreset characterPreset = damageInfo.fromCharacter.characterPreset;
				CharacterRandomPreset characterPreset2 = characterMainControl.characterPreset;
				if (characterPreset && characterPreset2)
				{
					num5 *= characterPreset.aiCombatFactor / characterPreset2.aiCombatFactor;
				}
			}
			num3 *= num5;
		}
		if (damageInfo.elementFactors.Count <= 0)
		{
			damageInfo.elementFactors.Add(new ElementFactor(ElementTypes.physics, 1f));
		}
		float num6 = 0f;
		foreach (ElementFactor elementFactor in damageInfo.elementFactors)
		{
			float factor = elementFactor.factor;
			float num7 = this.ElementFactor(elementFactor.elementType);
			float num8 = num3 * factor * num7;
			if (num8 < 1f && num8 > 0f && num7 > 0f && factor > 0f)
			{
				num8 = 1f;
			}
			if (num8 > 0f && !this.Hidden && PopText.instance)
			{
				GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook elementDamagePopTextLook = GameplayDataSettings.UIStyle.GetElementDamagePopTextLook(elementFactor.elementType);
				float size = flag2 ? elementDamagePopTextLook.critSize : elementDamagePopTextLook.normalSize;
				Color color = elementDamagePopTextLook.color;
				PopText.Pop(num8.ToString("F1"), damageInfo.damagePoint + Vector3.up * 2f, color, size, flag2 ? GameplayDataSettings.UIStyle.CritPopSprite : null);
			}
			num6 += num8;
		}
		damageInfo.finalDamage = num6;
		if (this.CurrentHealth < damageInfo.finalDamage)
		{
			damageInfo.finalDamage = this.CurrentHealth + 1f;
		}
		this.CurrentHealth -= damageInfo.finalDamage;
		UnityEvent<DamageInfo> onHurtEvent = this.OnHurtEvent;
		if (onHurtEvent != null)
		{
			onHurtEvent.Invoke(damageInfo);
		}
		Action<Health, DamageInfo> onHurt = Health.OnHurt;
		if (onHurt != null)
		{
			onHurt(this, damageInfo);
		}
		if (this.isDead)
		{
			return true;
		}
		if (this.CurrentHealth <= 0f)
		{
			bool flag3 = true;
			if (!LevelManager.Instance.IsRaidMap && !this.CanDieIfNotRaidMap)
			{
				flag3 = false;
			}
			if (!flag3)
			{
				this.SetHealth(1f);
			}
		}
		if (this.CurrentHealth <= 0f)
		{
			this.CurrentHealth = 0f;
			this.isDead = true;
			if (LevelManager.Instance.MainCharacter != this.TryGetCharacter())
			{
				this.DestroyOnDelay().Forget();
			}
			if (this.item != null && this.team != Teams.player && damageInfo.fromCharacter && damageInfo.fromCharacter.IsMainCharacter)
			{
				EXPManager.AddExp(this.item.GetInt("Exp", 0));
			}
			UnityEvent<DamageInfo> onDeadEvent = this.OnDeadEvent;
			if (onDeadEvent != null)
			{
				onDeadEvent.Invoke(damageInfo);
			}
			Action<Health, DamageInfo> onDead = Health.OnDead;
			if (onDead != null)
			{
				onDead(this, damageInfo);
			}
			base.gameObject.SetActive(false);
			if (damageInfo.fromCharacter && damageInfo.fromCharacter.IsMainCharacter)
			{
				Debug.Log("Killed by maincharacter");
			}
		}
		return true;
	}

	// Token: 0x060003CF RID: 975 RVA: 0x00010FF4 File Offset: 0x0000F1F4
	public void RequestHealthBar()
	{
		if (this.showHealthBar && LevelManager.LevelInited)
		{
			Action<Health> onRequestHealthBar = Health.OnRequestHealthBar;
			if (onRequestHealthBar == null)
			{
				return;
			}
			onRequestHealthBar(this);
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x00011015 File Offset: 0x0000F215
	private void OnDestroy()
	{
		this.hasBeenDestroied = true;
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00011020 File Offset: 0x0000F220
	public UniTask DestroyOnDelay()
	{
		Health.<DestroyOnDelay>d__74 <DestroyOnDelay>d__;
		<DestroyOnDelay>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DestroyOnDelay>d__.<>4__this = this;
		<DestroyOnDelay>d__.<>1__state = -1;
		<DestroyOnDelay>d__.<>t__builder.Start<Health.<DestroyOnDelay>d__74>(ref <DestroyOnDelay>d__);
		return <DestroyOnDelay>d__.<>t__builder.Task;
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00011063 File Offset: 0x0000F263
	public void AddHealth(float healthValue)
	{
		this.CurrentHealth = Mathf.Min(this.MaxHealth, this.CurrentHealth + healthValue);
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0001107E File Offset: 0x0000F27E
	public void SetHealth(float healthValue)
	{
		this.CurrentHealth = Mathf.Min(this.MaxHealth, healthValue);
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x00011092 File Offset: 0x0000F292
	public void SetInvincible(bool value)
	{
		this.invincible = value;
	}

	// Token: 0x040002CD RID: 717
	public Teams team;

	// Token: 0x040002CE RID: 718
	public bool hasSoul = true;

	// Token: 0x040002CF RID: 719
	private Item item;

	// Token: 0x040002D0 RID: 720
	private int maxHealthHash = "MaxHealth".GetHashCode();

	// Token: 0x040002D1 RID: 721
	private float lastMaxHealth;

	// Token: 0x040002D2 RID: 722
	private bool _showHealthBar;

	// Token: 0x040002D3 RID: 723
	[SerializeField]
	private int defaultMaxHealth;

	// Token: 0x040002D4 RID: 724
	private float _currentHealth;

	// Token: 0x040002D5 RID: 725
	private bool canDieIfNotRaidMap;

	// Token: 0x040002D6 RID: 726
	public UnityEvent<Health> OnHealthChange;

	// Token: 0x040002D7 RID: 727
	public UnityEvent<Health> OnMaxHealthChange;

	// Token: 0x040002DD RID: 733
	public float healthBarHeight = 2f;

	// Token: 0x040002DE RID: 734
	private bool isDead;

	// Token: 0x040002DF RID: 735
	public bool autoInit = true;

	// Token: 0x040002E0 RID: 736
	[SerializeField]
	private bool DestroyOnDead = true;

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	private float DeadDestroyDelay = 0.5f;

	// Token: 0x040002E2 RID: 738
	private bool inited;

	// Token: 0x040002E3 RID: 739
	private bool invincible;

	// Token: 0x040002E4 RID: 740
	private bool hasCharacter = true;

	// Token: 0x040002E5 RID: 741
	private CharacterMainControl characterCached;

	// Token: 0x040002E6 RID: 742
	private int bodyArmorHash = "BodyArmor".GetHashCode();

	// Token: 0x040002E7 RID: 743
	private int headArmorHash = "HeadArmor".GetHashCode();

	// Token: 0x040002E8 RID: 744
	private int Hash_ElementFactor_Physics = "ElementFactor_Physics".GetHashCode();

	// Token: 0x040002E9 RID: 745
	private int Hash_ElementFactor_Fire = "ElementFactor_Fire".GetHashCode();

	// Token: 0x040002EA RID: 746
	private int Hash_ElementFactor_Poison = "ElementFactor_Poison".GetHashCode();

	// Token: 0x040002EB RID: 747
	private int Hash_ElementFactor_Electricity = "ElementFactor_Electricity".GetHashCode();

	// Token: 0x040002EC RID: 748
	private int Hash_ElementFactor_Space = "ElementFactor_Space".GetHashCode();

	// Token: 0x040002ED RID: 749
	private int Hash_ElementFactor_Ghost = "ElementFactor_Ghost".GetHashCode();

	// Token: 0x040002EE RID: 750
	private int Hash_ElementFactor_Ice = "ElementFactor_Ice".GetHashCode();

	// Token: 0x040002EF RID: 751
	private bool hasBeenDestroied;
}
