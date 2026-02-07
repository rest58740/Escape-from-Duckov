using System;
using System.Collections.Generic;
using Duckov.Buffs;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x0200006C RID: 108
[Serializable]
public struct DamageInfo
{
	// Token: 0x06000435 RID: 1077 RVA: 0x00012A5E File Offset: 0x00010C5E
	public void AddElementFactor(ElementTypes _type, float _factor)
	{
		if (_factor <= 0f)
		{
			return;
		}
		this.elementFactors.Add(new ElementFactor(_type, _factor));
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00012A7C File Offset: 0x00010C7C
	public string GenerateDescription()
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		if (this.fromCharacter != null)
		{
			if (this.fromCharacter.IsMainCharacter)
			{
				text = LocalizationManager.ToPlainText("DeathReason_Self");
			}
			else if (this.fromCharacter.characterPreset != null)
			{
				text = this.fromCharacter.characterPreset.DisplayName;
			}
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.fromWeaponItemID);
		if (metaData.id > 0)
		{
			text2 = metaData.DisplayName;
		}
		if (this.isExplosion)
		{
			text2 = LocalizationManager.ToPlainText("DeathReason_Explosion");
		}
		if (this.crit > 0)
		{
			text3 = LocalizationManager.ToPlainText("DeathReason_Critical");
		}
		bool flag = string.IsNullOrEmpty(text);
		bool flag2 = string.IsNullOrEmpty(text2);
		if (flag && flag2)
		{
			return "?";
		}
		if (flag)
		{
			return text2;
		}
		if (flag2)
		{
			return text;
		}
		return string.Concat(new string[]
		{
			text,
			" (",
			text2,
			") ",
			text3
		});
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00012B7C File Offset: 0x00010D7C
	public DamageInfo(CharacterMainControl fromCharacter = null)
	{
		this.damageValue = 0f;
		this.critDamageFactor = 1f;
		this.ignoreArmor = false;
		this.critRate = 0f;
		this.armorBreak = 0f;
		this.armorPiercing = 0f;
		this.fromCharacter = fromCharacter;
		this.toDamageReceiver = null;
		this.damagePoint = Vector3.zero;
		this.damageNormal = Vector3.up;
		this.elementFactors = new List<ElementFactor>();
		this.crit = -1;
		this.damageType = DamageTypes.normal;
		this.buffChance = 0f;
		this.buff = null;
		this.finalDamage = 0f;
		this.isFromBuffOrEffect = false;
		this.fromWeaponItemID = 0;
		this.isExplosion = false;
		this.bleedChance = 0f;
		this.ignoreDifficulty = false;
	}

	// Token: 0x04000324 RID: 804
	public DamageTypes damageType;

	// Token: 0x04000325 RID: 805
	public bool isFromBuffOrEffect;

	// Token: 0x04000326 RID: 806
	public float damageValue;

	// Token: 0x04000327 RID: 807
	public bool ignoreArmor;

	// Token: 0x04000328 RID: 808
	public bool ignoreDifficulty;

	// Token: 0x04000329 RID: 809
	public float critDamageFactor;

	// Token: 0x0400032A RID: 810
	public float critRate;

	// Token: 0x0400032B RID: 811
	public float armorPiercing;

	// Token: 0x0400032C RID: 812
	[SerializeField]
	public List<ElementFactor> elementFactors;

	// Token: 0x0400032D RID: 813
	public bool isExplosion;

	// Token: 0x0400032E RID: 814
	public float armorBreak;

	// Token: 0x0400032F RID: 815
	public float finalDamage;

	// Token: 0x04000330 RID: 816
	public CharacterMainControl fromCharacter;

	// Token: 0x04000331 RID: 817
	public DamageReceiver toDamageReceiver;

	// Token: 0x04000332 RID: 818
	[HideInInspector]
	public Vector3 damagePoint;

	// Token: 0x04000333 RID: 819
	[HideInInspector]
	public Vector3 damageNormal;

	// Token: 0x04000334 RID: 820
	public int crit;

	// Token: 0x04000335 RID: 821
	[ItemTypeID]
	public int fromWeaponItemID;

	// Token: 0x04000336 RID: 822
	public float buffChance;

	// Token: 0x04000337 RID: 823
	public Buff buff;

	// Token: 0x04000338 RID: 824
	public float bleedChance;
}
