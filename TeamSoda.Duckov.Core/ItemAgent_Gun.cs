using System;
using System.Runtime.CompilerServices;
using Duckov;
using Duckov.Utilities;
using FMOD.Studio;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using Unity.VisualScripting;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class ItemAgent_Gun : DuckovItemAgent
{
	// Token: 0x1700015C RID: 348
	// (get) Token: 0x0600076F RID: 1903 RVA: 0x000216E0 File Offset: 0x0001F8E0
	public Item BulletItem
	{
		get
		{
			if (this._bulletItem == null || this._bulletItem.ParentItem != base.Item)
			{
				foreach (Item item in base.Item.Inventory)
				{
					if (item != null)
					{
						this._bulletItem = item;
						break;
					}
				}
			}
			return this._bulletItem;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000770 RID: 1904 RVA: 0x0002176C File Offset: 0x0001F96C
	public float ShootSpeed
	{
		get
		{
			return (base.Item.GetStatValue(ItemAgent_Gun.ShootSpeedHash) + this.shootSpeedGainOverShoot) * this.CharacterShootSpeedMultiplier;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000771 RID: 1905 RVA: 0x0002178C File Offset: 0x0001F98C
	public float ShootSpeedGainEachShoot
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.ShootSpeedGainEachShootHash);
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000772 RID: 1906 RVA: 0x0002179E File Offset: 0x0001F99E
	public float ShootSpeedGainByShootMax
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.ShootSpeedGainByShootMaxHash);
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000773 RID: 1907 RVA: 0x000217B0 File Offset: 0x0001F9B0
	public float ReloadTime
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.ReloadTimeHash) / (1f + this.CharacterReloadSpeedGain);
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000774 RID: 1908 RVA: 0x000217CF File Offset: 0x0001F9CF
	public int Capacity
	{
		get
		{
			return Mathf.RoundToInt(base.Item.GetStatValue(ItemAgent_Gun.CapacityHash));
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000775 RID: 1909 RVA: 0x000217E6 File Offset: 0x0001F9E6
	public float TraceAbility
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.TraceAbilityHash);
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000776 RID: 1910 RVA: 0x000217F8 File Offset: 0x0001F9F8
	public float durabilityPercent
	{
		get
		{
			return this.Durability / this.MaxDurability;
		}
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000777 RID: 1911 RVA: 0x00021807 File Offset: 0x0001FA07
	public float Durability
	{
		get
		{
			return base.Item.Variables.GetFloat(ItemAgent_Gun.DurabilityHash, 0f);
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000778 RID: 1912 RVA: 0x00021823 File Offset: 0x0001FA23
	public float MaxDurability
	{
		get
		{
			if (this.maxDurability <= 0f)
			{
				this.maxDurability = base.Item.Constants.GetFloat("MaxDurability", 50f);
			}
			return this.maxDurability;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000779 RID: 1913 RVA: 0x00021858 File Offset: 0x0001FA58
	public float Damage
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.DamageHash);
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x0600077A RID: 1914 RVA: 0x0002186A File Offset: 0x0001FA6A
	public int BurstCount
	{
		get
		{
			return Mathf.Max(1, Mathf.RoundToInt(base.Item.GetStatValue(ItemAgent_Gun.BurstCountHash)));
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x0600077B RID: 1915 RVA: 0x00021887 File Offset: 0x0001FA87
	public float BulletSpeed
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.BulletSpeedHash);
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x0600077C RID: 1916 RVA: 0x00021899 File Offset: 0x0001FA99
	public float BulletDistance
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.BulletDistanceHash) * (base.Holder ? base.Holder.GunDistanceMultiplier : 1f);
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x0600077D RID: 1917 RVA: 0x000218CB File Offset: 0x0001FACB
	public int Penetrate
	{
		get
		{
			return Mathf.RoundToInt(base.Item.GetStatValue(ItemAgent_Gun.PenetrateHash));
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x0600077E RID: 1918 RVA: 0x000218E2 File Offset: 0x0001FAE2
	public float ExplosionDamageMultiplier
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.explosionDamageMultiplierHash);
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x0600077F RID: 1919 RVA: 0x000218F4 File Offset: 0x0001FAF4
	public float CritRate
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.CritRateHash);
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000780 RID: 1920 RVA: 0x00021906 File Offset: 0x0001FB06
	public float CritDamageFactor
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.CritDamageFactorHash);
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000781 RID: 1921 RVA: 0x00021918 File Offset: 0x0001FB18
	public float SoundRange
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.SoundRangeHash);
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000782 RID: 1922 RVA: 0x0002192C File Offset: 0x0001FB2C
	public bool Silenced
	{
		get
		{
			Stat stat = base.Item.GetStat(ItemAgent_Gun.SoundRangeHash);
			return stat.Value < stat.BaseValue * 0.95f;
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06000783 RID: 1923 RVA: 0x0002195E File Offset: 0x0001FB5E
	public float ArmorPiercing
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.ArmorPiercingHash);
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06000784 RID: 1924 RVA: 0x00021970 File Offset: 0x0001FB70
	public float ArmorBreak
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.ArmorBreakHash);
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06000785 RID: 1925 RVA: 0x00021982 File Offset: 0x0001FB82
	public int ShotCount
	{
		get
		{
			return Mathf.RoundToInt(base.Item.GetStatValue(ItemAgent_Gun.ShotCountHash));
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000786 RID: 1926 RVA: 0x00021999 File Offset: 0x0001FB99
	public float ShotAngle
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.ShotAngleHash) * (this.IsInAds ? 0.5f : 1f);
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000787 RID: 1927 RVA: 0x000219C0 File Offset: 0x0001FBC0
	public float ADSAimDistanceFactor
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.ADSAimDistanceFactorHash);
		}
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000788 RID: 1928 RVA: 0x000219D2 File Offset: 0x0001FBD2
	public float AdsSpeed
	{
		get
		{
			return 1f / Mathf.Max(0.01f, base.Item.GetStatValue(ItemAgent_Gun.AdsTimeHash));
		}
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000789 RID: 1929 RVA: 0x000219F4 File Offset: 0x0001FBF4
	public float DefaultScatter
	{
		get
		{
			float num = base.Item.GetStatValue(ItemAgent_Gun.DefaultScatterHash) * this.scatterFactorHips;
			float num2 = base.Item.GetStatValue(ItemAgent_Gun.DefaultScatterHashADS) * this.scatterFactorAds;
			return Mathf.Lerp(num, num2, this.adsValue);
		}
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x0600078A RID: 1930 RVA: 0x00021A3C File Offset: 0x0001FC3C
	public float MaxScatter
	{
		get
		{
			float num = base.Item.GetStatValue(ItemAgent_Gun.MaxScatterHash) * this.scatterFactorHips;
			float num2 = base.Item.GetStatValue(ItemAgent_Gun.MaxScatterHashADS) * this.scatterFactorAds;
			return Mathf.Lerp(num, num2, this.adsValue);
		}
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x0600078B RID: 1931 RVA: 0x00021A84 File Offset: 0x0001FC84
	public float ScatterGrow
	{
		get
		{
			float num = base.Item.GetStatValue(ItemAgent_Gun.ScatterGrowHash) * this.scatterFactorHips;
			float num2 = base.Item.GetStatValue(ItemAgent_Gun.ScatterGrowHashADS) * this.scatterFactorAds;
			return Mathf.Lerp(num, num2, this.adsValue);
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x0600078C RID: 1932 RVA: 0x00021ACC File Offset: 0x0001FCCC
	public float ScatterRecover
	{
		get
		{
			float statValue = base.Item.GetStatValue(ItemAgent_Gun.ScatterRecoverHash);
			float statValue2 = base.Item.GetStatValue(ItemAgent_Gun.ScatterRecoverHashADS);
			return Mathf.Lerp(statValue, statValue2, this.adsValue) * this.ScatterGrow * this.ShootSpeed;
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x0600078D RID: 1933 RVA: 0x00021B14 File Offset: 0x0001FD14
	public float RecoilVMin
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.RecoilVMinHash);
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x0600078E RID: 1934 RVA: 0x00021B26 File Offset: 0x0001FD26
	public float RecoilVMax
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.RecoilVMaxHash);
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x00021B38 File Offset: 0x0001FD38
	public float RecoilHMin
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.RecoilHMinHash);
		}
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000790 RID: 1936 RVA: 0x00021B4A File Offset: 0x0001FD4A
	public float RecoilHMax
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.RecoilHMaxHash);
		}
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x00021B5C File Offset: 0x0001FD5C
	public float RecoilScaleV
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.RecoilScaleVHash);
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000792 RID: 1938 RVA: 0x00021B6E File Offset: 0x0001FD6E
	public float RecoilScaleH
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.RecoilScaleHHash);
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06000793 RID: 1939 RVA: 0x00021B80 File Offset: 0x0001FD80
	public float RecoilRecover
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.RecoilRecoverHash);
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06000794 RID: 1940 RVA: 0x00021B92 File Offset: 0x0001FD92
	public float RecoilTime
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.RecoilTimeHash);
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000795 RID: 1941 RVA: 0x00021BA4 File Offset: 0x0001FDA4
	public float RecoilRecoverTime
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.RecoilRecoverTimeHash);
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x00021BB6 File Offset: 0x0001FDB6
	public float MoveSpeedMultiplier
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.MoveSpeedMultiplierHash);
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000797 RID: 1943 RVA: 0x00021BC8 File Offset: 0x0001FDC8
	public float AdsWalkSpeedMultiplier
	{
		get
		{
			return Mathf.Min(1f, base.Item.GetStatValue(ItemAgent_Gun.AdsWalkSpeedMultiplierHash));
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06000798 RID: 1944 RVA: 0x00021BE4 File Offset: 0x0001FDE4
	public float burstCoolTime
	{
		get
		{
			return 1f / this.ShootSpeed * ((float)(3 * this.BurstCount) / ((float)this.BurstCount + 2f));
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06000799 RID: 1945 RVA: 0x00021C0A File Offset: 0x0001FE0A
	public float burstShotTimeSpace
	{
		get
		{
			return 1f / this.ShootSpeed * ((float)this.BurstCount / ((float)this.BurstCount + 2f));
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x0600079A RID: 1946 RVA: 0x00021C2E File Offset: 0x0001FE2E
	public float BuffChance
	{
		get
		{
			return base.Item.GetStatValue(ItemAgent_Gun.BuffChanceHash);
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x0600079B RID: 1947 RVA: 0x00021C40 File Offset: 0x0001FE40
	public float bulletCritRateGain
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.bulletCritRateGainHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x0600079C RID: 1948 RVA: 0x00021C6F File Offset: 0x0001FE6F
	public float BulletCritDamageFactorGain
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.bulletCritDamageFactorGainHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x0600079D RID: 1949 RVA: 0x00021C9E File Offset: 0x0001FE9E
	public float BulletArmorPiercingGain
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.bulletArmorPiercingGainHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x0600079E RID: 1950 RVA: 0x00021CCD File Offset: 0x0001FECD
	public float BulletDamageMultiplier
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.BulletDamageMultiplierHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x00021CFC File Offset: 0x0001FEFC
	public float BulletExplosionRange
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.bulletExplosionRangeHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00021D2B File Offset: 0x0001FF2B
	public float BulletBuffChanceMultiplier
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.BulletBuffChanceMultiplierHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00021D5A File Offset: 0x0001FF5A
	public float BulletBleedChance
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.BulletBleedChanceHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00021D89 File Offset: 0x0001FF89
	public float BulletExplosionDamage
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.bulletExplosionDamageHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00021DB8 File Offset: 0x0001FFB8
	public float BulletArmorBreakGain
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.armorBreakGainHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00021DE7 File Offset: 0x0001FFE7
	public float bulletDurabilityCost
	{
		get
		{
			if (this.BulletItem)
			{
				return this.BulletItem.Constants.GetFloat(ItemAgent_Gun.bulletDurabilityCostHash, 0f);
			}
			return 0f;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00021E16 File Offset: 0x00020016
	public float CharacterDamageMultiplier
	{
		get
		{
			if (!base.Holder)
			{
				return 1f;
			}
			return base.Holder.GunDamageMultiplier;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00021E36 File Offset: 0x00020036
	public float CharacterShootSpeedMultiplier
	{
		get
		{
			if (!base.Holder)
			{
				return 1f;
			}
			return base.Holder.GunShootSpeedMultiplier;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00021E56 File Offset: 0x00020056
	public float CharacterReloadSpeedGain
	{
		get
		{
			if (!base.Holder)
			{
				return 0f;
			}
			return base.Holder.ReloadSpeedGain;
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00021E76 File Offset: 0x00020076
	public float CharacterGunCritRateGain
	{
		get
		{
			if (!base.Holder)
			{
				return 0f;
			}
			return base.Holder.GunCritRateGain;
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00021E96 File Offset: 0x00020096
	public float CharacterGunCritDamageGain
	{
		get
		{
			if (!base.Holder)
			{
				return 0f;
			}
			return base.Holder.GunCritDamageGain;
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x060007AA RID: 1962 RVA: 0x00021EB6 File Offset: 0x000200B6
	public float CharacterRecoilControl
	{
		get
		{
			if (!base.Holder)
			{
				return 1f;
			}
			return base.Holder.RecoilControl;
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060007AB RID: 1963 RVA: 0x00021ED6 File Offset: 0x000200D6
	public float CharacterScatterMultiplier
	{
		get
		{
			if (!base.Holder)
			{
				return 1f;
			}
			return Mathf.Max(0.1f, base.Holder.GunScatterMultiplier);
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x060007AC RID: 1964 RVA: 0x00021F00 File Offset: 0x00020100
	public int BulletCount
	{
		get
		{
			if (!this.GunItemSetting)
			{
				return 0;
			}
			return this.GunItemSetting.BulletCount;
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x060007AD RID: 1965 RVA: 0x00021F1C File Offset: 0x0002011C
	public float AdsValue
	{
		get
		{
			return this.adsValue;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x060007AE RID: 1966 RVA: 0x00021F24 File Offset: 0x00020124
	public CharacterMainControl TraceTarget
	{
		get
		{
			return this.traceTarget;
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x060007AF RID: 1967 RVA: 0x00021F2C File Offset: 0x0002012C
	public Transform muzzle
	{
		get
		{
			if (this.muzzleIndex != 0 && this.muzzle2 != null)
			{
				return this.muzzle2;
			}
			return this.muzzle1;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00021F51 File Offset: 0x00020151
	private Transform muzzle1
	{
		get
		{
			if (this._mz1 == null)
			{
				this._mz1 = base.GetSocket("Muzzle", true);
			}
			if (this._mz1 == null)
			{
				return base.transform;
			}
			return this._mz1;
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00021F90 File Offset: 0x00020190
	private Transform muzzle2
	{
		get
		{
			if (this._mz2 == null && this.hasMz2)
			{
				this._mz2 = base.GetSocket("Muzzle2", false);
				if (this._mz2 == null)
				{
					this.hasMz2 = false;
				}
			}
			return this._mz2;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00021FE0 File Offset: 0x000201E0
	private GameObject muzzleFxPfb
	{
		get
		{
			return this.GunItemSetting.muzzleFxPfb;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00021FED File Offset: 0x000201ED
	public ItemSetting_Gun GunItemSetting
	{
		get
		{
			if (!this._gunItemSetting && base.Item)
			{
				this._gunItemSetting = base.Item.GetComponent<ItemSetting_Gun>();
			}
			return this._gunItemSetting;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00022020 File Offset: 0x00020220
	public bool IsInAds
	{
		get
		{
			return base.Holder && base.Holder.IsInAdsInput;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0002203C File Offset: 0x0002023C
	public float CurrentScatter
	{
		get
		{
			return this.scatterBeforeControl * this.CharacterScatterMultiplier;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0002204B File Offset: 0x0002024B
	public float MinScatter
	{
		get
		{
			return this.DefaultScatter;
		}
	}

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x060007B7 RID: 1975 RVA: 0x00022054 File Offset: 0x00020254
	// (remove) Token: 0x060007B8 RID: 1976 RVA: 0x00022088 File Offset: 0x00020288
	public static event Action<ItemAgent_Gun> OnMainCharacterShootEvent;

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x060007B9 RID: 1977 RVA: 0x000220BC File Offset: 0x000202BC
	// (remove) Token: 0x060007BA RID: 1978 RVA: 0x000220F4 File Offset: 0x000202F4
	public event Action OnShootEvent;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x060007BB RID: 1979 RVA: 0x0002212C File Offset: 0x0002032C
	// (remove) Token: 0x060007BC RID: 1980 RVA: 0x00022164 File Offset: 0x00020364
	public event Action OnLoadedEvent;

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x060007BD RID: 1981 RVA: 0x00022199 File Offset: 0x00020399
	public float StateTimer
	{
		get
		{
			return this.stateTimer;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x060007BE RID: 1982 RVA: 0x000221A1 File Offset: 0x000203A1
	public ItemAgent_Gun.GunStates GunState
	{
		get
		{
			return this.gunState;
		}
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x000221AC File Offset: 0x000203AC
	private void Update()
	{
		this.UpdateGun();
		if (base.Holder && base.Holder.IsMainCharacter)
		{
			if (this.adsValue < 0.5f)
			{
				this.traceTarget = null;
			}
			else
			{
				this.SearchTraceTarget();
			}
		}
		this.UpdateScatterFactor();
		this.triggerInput = false;
		this.triggerThisFrame = false;
		this.releaseThisFrame = false;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00022210 File Offset: 0x00020410
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.StopReloadSound();
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0002221E File Offset: 0x0002041E
	private void UpdateScatterFactor()
	{
		this.scatterFactorHips = base.Item.GetStatValue(ItemAgent_Gun.ScatterFactorHash);
		this.scatterFactorAds = base.Item.GetStatValue(ItemAgent_Gun.ScatterFactorHashADS);
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0002224C File Offset: 0x0002044C
	private void UpdateGun()
	{
		float maxScatter = this.MaxScatter;
		if (this.scatterBeforeControl > maxScatter)
		{
			this.scatterBeforeControl = maxScatter;
		}
		this.scatterBeforeControl = Mathf.MoveTowards(this.scatterBeforeControl, this.DefaultScatter, this.ScatterRecover * Time.deltaTime * ((this.scatterBeforeControl < this.DefaultScatter) ? 6f : 1f));
		if (!this.triggerInput)
		{
			this.shootSpeedGainOverShoot = 0f;
		}
		this.UpdateStates();
		this.UpdateAds();
		this.UpdateVisualRecoil();
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000222D4 File Offset: 0x000204D4
	protected override void OnInitialize()
	{
		base.OnInitialize();
		if (this.GunItemSetting)
		{
			if (base.Holder && base.Holder.CharacterItem && base.Holder.CharacterItem.Inventory)
			{
				this.GunItemSetting.AutoSetTypeInInventory(base.Holder.CharacterItem.Inventory);
			}
			else
			{
				this.GunItemSetting.AutoSetTypeInInventory(null);
			}
			this.scatterBeforeControl = this.DefaultScatter;
			if (this.loadedVisualObject != null)
			{
				this.loadedVisualObject.SetActive(this.GunItemSetting.BulletCount > 0);
			}
		}
		this.damageReceiverLayermask = GameplayDataSettings.Layers.damageReceiverLayerMask;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0002239C File Offset: 0x0002059C
	public void UpdateStates()
	{
		if (this.GunItemSetting.LoadingBullets)
		{
			return;
		}
		if (this.triggerThisFrame && this.ShootSpeed >= 5f)
		{
			this.triggerBuffer = true;
			this.triggerThisFrame = false;
		}
		switch (this.gunState)
		{
		case ItemAgent_Gun.GunStates.shootCooling:
			this.stateTimer += Time.deltaTime;
			if (this.stateTimer >= this.burstCoolTime)
			{
				this.TransToReady();
				return;
			}
			break;
		case ItemAgent_Gun.GunStates.ready:
		{
			bool flag = false;
			ItemSetting_Gun.TriggerModes currentTriggerMode = this.GunItemSetting.currentTriggerMode;
			if (this.BulletCount <= 0)
			{
				this.TransToEmpty();
			}
			else if (currentTriggerMode == ItemSetting_Gun.TriggerModes.auto)
			{
				if (this.triggerInput)
				{
					flag = true;
				}
			}
			else if ((currentTriggerMode == ItemSetting_Gun.TriggerModes.semi || currentTriggerMode == ItemSetting_Gun.TriggerModes.bolt) && (this.triggerBuffer || this.triggerThisFrame))
			{
				this.triggerThisFrame = false;
				this.triggerBuffer = false;
				flag = true;
			}
			if (flag)
			{
				this.TransToFire(this.triggerThisFrame);
				return;
			}
			if (this.needAutoReload)
			{
				this.needAutoReload = false;
				this.CharacterReload(null);
				return;
			}
			break;
		}
		case ItemAgent_Gun.GunStates.fire:
			this.triggerBuffer = false;
			if (this.BulletCount <= 0)
			{
				this.muzzleIndex = ((this.muzzleIndex == 0) ? 1 : 0);
				this.TransToEmpty();
				return;
			}
			if (this.burstCounter >= this.BurstCount)
			{
				this.muzzleIndex = ((this.muzzleIndex == 0) ? 1 : 0);
				this.TransToBurstCooling();
				return;
			}
			this.TransToBurstEachShotCooling();
			return;
		case ItemAgent_Gun.GunStates.burstEachShotCooling:
			this.stateTimer += Time.deltaTime;
			if (this.stateTimer >= this.burstShotTimeSpace)
			{
				this.TransToFire(false);
				return;
			}
			break;
		case ItemAgent_Gun.GunStates.empty:
			if (this.needAutoReload)
			{
				this.needAutoReload = false;
				this.CharacterReload(null);
				return;
			}
			if ((this.triggerThisFrame || this.triggerBuffer) && base.Holder != null)
			{
				this.triggerThisFrame = false;
				this.triggerBuffer = false;
				base.Holder.TryToReload(null);
				return;
			}
			break;
		case ItemAgent_Gun.GunStates.reloading:
			this.triggerBuffer = false;
			this.stateTimer += Time.deltaTime;
			if (this.stateTimer < this.ReloadTime)
			{
				this.loadBulletsStarted = false;
				return;
			}
			if (!this.loadBulletsStarted)
			{
				this.loadBulletsStarted = true;
				this.StartLoadBullets();
				return;
			}
			if (!this.GunItemSetting.LoadingBullets)
			{
				if (this.GunItemSetting.LoadBulletsSuccess)
				{
					this.PostReloadSuccessSound();
				}
				this.needAutoReload = (this.GunItemSetting.reloadMode == ItemSetting_Gun.ReloadModes.singleBullet && !this.GunItemSetting.IsFull());
				this.loadBulletsStarted = false;
				if (this.GunItemSetting.BulletCount > 0 && this.loadedVisualObject != null)
				{
					this.loadedVisualObject.SetActive(true);
				}
				Action onLoadedEvent = this.OnLoadedEvent;
				if (onLoadedEvent != null)
				{
					onLoadedEvent();
				}
				this.TransToReady();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00022658 File Offset: 0x00020858
	private void UpdateAds()
	{
		float num = 0f;
		if (base.Holder && base.Holder.IsInAdsInput)
		{
			num = 1f;
		}
		float num2 = this.AdsSpeed;
		if (num == 0f)
		{
			num2 = Mathf.Max(num2, 4f);
		}
		this.adsValue = Mathf.MoveTowards(this.adsValue, num, Time.deltaTime * num2);
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x000226BF File Offset: 0x000208BF
	private void TransToBurstCooling()
	{
		this.gunState = ItemAgent_Gun.GunStates.shootCooling;
		this.burstCounter = 0;
		this.stateTimer = 0f;
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000226DA File Offset: 0x000208DA
	private void TransToReady()
	{
		this.gunState = ItemAgent_Gun.GunStates.ready;
		this.burstCounter = 0;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x000226EC File Offset: 0x000208EC
	private void SearchTraceTarget()
	{
		if (base.Holder == null)
		{
			return;
		}
		this.traceTarget = null;
		if (this.TraceAbility < 0.1f)
		{
			return;
		}
		Vector3 vector = Vector3.right;
		if (base.Holder.IsMainCharacter)
		{
			vector = LevelManager.Instance.InputManager.InputAimPoint;
		}
		else
		{
			vector = base.Holder.GetCurrentAimPoint();
		}
		int num = Physics.OverlapSphereNonAlloc(vector, 8f, this.traceColliders, this.damageReceiverLayermask);
		float num2 = 999f;
		DamageReceiver damageReceiver = null;
		for (int i = 0; i < num; i++)
		{
			float num3 = Vector3.Distance(vector, this.traceColliders[i].transform.position);
			if (num3 <= num2)
			{
				DamageReceiver component = this.traceColliders[i].GetComponent<DamageReceiver>();
				if (base.Holder && component && Team.IsEnemy(base.Holder.Team, component.Team) && component.health)
				{
					if (base.Holder.IsMainCharacter)
					{
						CharacterMainControl characterMainControl = component.health.TryGetCharacter();
						if (characterMainControl == null || characterMainControl.Hidden)
						{
							goto IL_126;
						}
					}
					num2 = num3;
					damageReceiver = component;
				}
			}
			IL_126:;
		}
		if (damageReceiver != null)
		{
			this.traceTarget = damageReceiver.health.TryGetCharacter();
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00022848 File Offset: 0x00020A48
	private void TransToFire(bool isFirstShot)
	{
		if (this.BulletCount <= 0)
		{
			return;
		}
		if (base.Item.Durability <= 0f)
		{
			return;
		}
		this.gunState = ItemAgent_Gun.GunStates.fire;
		Vector3 vector = this.muzzle.forward;
		if (base.Holder && base.Holder.CharacterMoveability > 0.5f)
		{
			Vector3 currentAimPoint = base.Holder.GetCurrentAimPoint();
			currentAimPoint.y = 0f;
			Vector3 position = base.Holder.transform.position;
			position.y = 0f;
			Vector3 position2 = this.muzzle.position;
			position2.y = 0f;
			if (Vector3.Distance(position, currentAimPoint) > Vector3.Distance(position, position2) + 0.1f)
			{
				vector = base.Holder.GetCurrentAimPoint() - this.muzzle.position;
				vector.Normalize();
			}
		}
		if (base.Holder && !base.Holder.IsMainCharacter)
		{
			this.SearchTraceTarget();
		}
		for (int i = 0; i < this.ShotCount; i++)
		{
			Vector3 vector2 = vector;
			float num = this.ShotAngle;
			bool flag = num > 359f;
			if (flag)
			{
				num -= num / (float)this.ShotCount;
			}
			float num2 = -num * 0.5f;
			float num3 = num / ((float)this.ShotCount - 1f);
			if ((float)this.ShotCount % 2f < 0.01f && flag)
			{
				num2 -= num3 * 0.5f;
			}
			if (this.ShotCount > 1)
			{
				vector2 = Quaternion.Euler(0f, num2 + (float)i * num3, 0f) * vector;
			}
			Vector3 localPosition = this.muzzle.localPosition;
			localPosition.y = 0f;
			float magnitude = localPosition.magnitude;
			this.shootSpeedGainOverShoot = Mathf.MoveTowards(this.shootSpeedGainOverShoot, this.ShootSpeedGainByShootMax, this.ShootSpeedGainEachShoot);
			this.ShootOneBullet(this.muzzle.position, vector2, this.muzzle.position - magnitude * vector2);
			if (base.Holder != null)
			{
				AIMainBrain.MakeSound(new AISound
				{
					fromCharacter = base.Holder,
					fromObject = base.gameObject,
					pos = this.muzzle.position,
					soundType = SoundTypes.combatSound,
					fromTeam = base.Holder.Team,
					radius = this.SoundRange
				});
			}
		}
		this.PostShootSound();
		this.scatterBeforeControl = Mathf.Clamp(this.scatterBeforeControl + this.ScatterGrow, this.DefaultScatter, this.MaxScatter);
		this.AimRecoil(vector);
		if (base.Holder == LevelManager.Instance.MainCharacter)
		{
			LevelManager.Instance.InputManager.AddRecoil(this);
		}
		this.StartVisualRecoil();
		this.GunItemSetting.UseABullet();
		base.Holder.TriggerShootEvent(this);
		Action onShootEvent = this.OnShootEvent;
		if (onShootEvent != null)
		{
			onShootEvent();
		}
		if (this.BulletCount <= 0 && this.GunItemSetting.autoReload)
		{
			this.needAutoReload = true;
		}
		if (this.GunItemSetting.BulletCount <= 0 && this.loadedVisualObject != null)
		{
			this.loadedVisualObject.SetActive(false);
		}
		if (base.Holder && base.Holder.IsMainCharacter && LevelManager.Instance.IsRaidMap)
		{
			base.Item.Durability = Mathf.Max(0f, base.Item.Durability - this.bulletDurabilityCost);
		}
		if (this.muzzleFxPfb)
		{
			Object.Instantiate<GameObject>(this.muzzleFxPfb, this.muzzle.position, this.muzzle.rotation).transform.SetParent(this.muzzle);
		}
		if (this.shellParticle)
		{
			this.shellParticle.Emit(1);
		}
		this.burstCounter++;
		if (base.Holder && base.Holder.IsMainCharacter)
		{
			CameraShaker.Shake(-this.muzzle.forward * 0.07f, CameraShaker.CameraShakeTypes.recoil);
			Action<ItemAgent_Gun> onMainCharacterShootEvent = ItemAgent_Gun.OnMainCharacterShootEvent;
			if (onMainCharacterShootEvent == null)
			{
				return;
			}
			onMainCharacterShootEvent(this);
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00022CA2 File Offset: 0x00020EA2
	private void TransToBurstEachShotCooling()
	{
		this.gunState = ItemAgent_Gun.GunStates.burstEachShotCooling;
		this.stateTimer = 0f;
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x00022CB6 File Offset: 0x00020EB6
	private void TransToEmpty()
	{
		this.gunState = ItemAgent_Gun.GunStates.empty;
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00022CC0 File Offset: 0x00020EC0
	private void ShootOneBullet(Vector3 _muzzlePoint, Vector3 _shootDirection, Vector3 firstFrameCheckStartPoint)
	{
		bool flag = false;
		if (this.GunItemSetting.LoadingBullets)
		{
			return;
		}
		if (!this.BulletItem)
		{
			return;
		}
		if (base.Holder && base.Holder.IsMainCharacter)
		{
			flag = true;
			HardwareSyncingManager.SetEvent("Shoot_Pistol");
		}
		ItemSetting_Bullet component = this.BulletItem.GetComponent<ItemSetting_Bullet>();
		float num = 0f;
		if (flag)
		{
			num = Mathf.Max(1f, this.CurrentScatter) * Mathf.Lerp(1.5f, 0f, Mathf.InverseLerp(0f, 0.5f, this.durabilityPercent));
		}
		float num2 = Random.Range(-0.5f, 0.5f) * (this.CurrentScatter + num);
		_shootDirection = Quaternion.Euler(0f, num2, 0f) * _shootDirection;
		_shootDirection.Normalize();
		Projectile projectile = this._gunItemSetting.bulletPfb;
		if (projectile == null)
		{
			projectile = GameplayDataSettings.Prefabs.DefaultBullet;
		}
		this.projInst = LevelManager.Instance.BulletPool.GetABullet(projectile);
		this.projInst.transform.position = _muzzlePoint;
		this.projInst.transform.rotation = Quaternion.LookRotation(_shootDirection, Vector3.up);
		ProjectileContext projectileContext = default(ProjectileContext);
		projectileContext.firstFrameCheck = true;
		projectileContext.firstFrameCheckStartPoint = firstFrameCheckStartPoint;
		projectileContext.direction = _shootDirection.normalized;
		projectileContext.speed = this.BulletSpeed;
		projectileContext.traceTarget = this.traceTarget;
		projectileContext.traceAbility = this.TraceAbility;
		if (base.Holder)
		{
			projectileContext.team = base.Holder.Team;
			projectileContext.speed *= base.Holder.GunBulletSpeedMultiplier;
		}
		projectileContext.distance = this.BulletDistance + 0.4f;
		projectileContext.halfDamageDistance = projectileContext.distance * 0.5f;
		if (!flag)
		{
			projectileContext.distance *= 1.05f;
		}
		projectileContext.penetrate = this.Penetrate;
		float characterDamageMultiplier = this.CharacterDamageMultiplier;
		float num3 = 1f;
		projectileContext.damage = this.Damage * this.BulletDamageMultiplier * num3 * characterDamageMultiplier / (float)this.ShotCount;
		if (this.Damage > 1f && projectileContext.damage < 1f)
		{
			projectileContext.damage = 1f;
		}
		projectileContext.critDamageFactor = (this.CritDamageFactor + this.BulletCritDamageFactorGain) * (1f + this.CharacterGunCritDamageGain);
		projectileContext.critRate = this.CritRate * (1f + this.CharacterGunCritRateGain + this.bulletCritRateGain);
		if (flag)
		{
			projectileContext.critRate = (LevelManager.Instance.InputManager.AimingEnemyHead ? 1f : 0f);
		}
		projectileContext.armorPiercing = this.ArmorPiercing + this.BulletArmorPiercingGain;
		projectileContext.armorBreak = this.ArmorBreak + this.BulletArmorBreakGain;
		projectileContext.fromCharacter = base.Holder;
		projectileContext.explosionRange = this.BulletExplosionRange;
		projectileContext.explosionDamage = this.BulletExplosionDamage * this.ExplosionDamageMultiplier;
		switch (this._gunItemSetting.element)
		{
		case ElementTypes.physics:
			projectileContext.element_Physics = 1f;
			break;
		case ElementTypes.fire:
			projectileContext.element_Fire = 1f;
			break;
		case ElementTypes.poison:
			projectileContext.element_Poison = 1f;
			break;
		case ElementTypes.electricity:
			projectileContext.element_Electricity = 1f;
			break;
		case ElementTypes.space:
			projectileContext.element_Space = 1f;
			break;
		case ElementTypes.ghost:
			projectileContext.element_Ghost = 1f;
			break;
		case ElementTypes.ice:
			projectileContext.element_Ice = 1f;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		projectileContext.fromWeaponItemID = base.Item.TypeID;
		projectileContext.buff = this._gunItemSetting.buff;
		if (component)
		{
			projectileContext.buffChance = this.BulletBuffChanceMultiplier * this.BuffChance;
		}
		projectileContext.bleedChance = this.BulletBleedChance;
		if (base.Holder)
		{
			if (flag)
			{
				if (base.Holder.HasNearByHalfObsticle())
				{
					projectileContext.ignoreHalfObsticle = true;
				}
			}
			else
			{
				this.projInst.damagedObjects.AddRange(base.Holder.GetNearByHalfObsticles());
			}
		}
		if (projectileContext.critRate > 0.99f)
		{
			projectileContext.ignoreHalfObsticle = true;
		}
		this.projInst.Init(projectileContext);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00023128 File Offset: 0x00021328
	private void AimRecoil(Vector3 shootDir)
	{
		if (!base.Holder || !(base.Holder == CharacterMainControl.Main))
		{
			return;
		}
		Vector3 vector = base.Holder.CurrentAimDirection;
		vector.y = 0f;
		vector = vector.normalized * 0.2f;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0002317F File Offset: 0x0002137F
	public bool CharacterReload(Item prefererdBullet = null)
	{
		return base.Holder && base.Holder.TryToReload(prefererdBullet);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0002319C File Offset: 0x0002139C
	public bool BeginReload()
	{
		if (this.gunState != ItemAgent_Gun.GunStates.ready && this.gunState != ItemAgent_Gun.GunStates.empty && this.gunState != ItemAgent_Gun.GunStates.shootCooling)
		{
			return false;
		}
		this.burstCounter = 0;
		if (this.GunItemSetting.PreferdBulletsToLoad != null)
		{
			this.GunItemSetting.SetTargetBulletType(this.GunItemSetting.PreferdBulletsToLoad);
		}
		if (this.GunItemSetting.TargetBulletID == -1)
		{
			this.GunItemSetting.AutoSetTypeInInventory(base.Holder.CharacterItem.Inventory);
		}
		if (this.GunItemSetting.TargetBulletID == -1)
		{
			return false;
		}
		int num = -1;
		Item currentLoadedBullet = this.GunItemSetting.GetCurrentLoadedBullet();
		if (currentLoadedBullet != null)
		{
			num = currentLoadedBullet.TypeID;
		}
		if (this.BulletCount >= this.Capacity && num == this.GunItemSetting.TargetBulletID)
		{
			return false;
		}
		if (this.GunItemSetting.PreferdBulletsToLoad == null && this.GunItemSetting.GetBulletCountofTypeInInventory(this.GunItemSetting.TargetBulletID, base.Holder.CharacterItem.Inventory) <= 0)
		{
			if (base.Holder && this.GunItemSetting.BulletCount <= 0)
			{
				base.Holder.PopText(LocalizationManager.ToPlainText("Poptext_OutOfAmmo"), -1f);
			}
			return false;
		}
		this.gunState = ItemAgent_Gun.GunStates.reloading;
		this.stateTimer = 0f;
		this.PostStartReloadSound();
		return true;
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x000232F8 File Offset: 0x000214F8
	private void PostStartReloadSound()
	{
		if (this._reloadSoundLoopEvent != null)
		{
			this._reloadSoundLoopEvent.Value.stop(1);
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		string soundkey = this.GunItemSetting.reloadKey.ToLower() + "_start";
		string eventName = StringExtensions.Format("SFX/Combat/Gun/Reload/{soundkey}", new
		{
			soundkey
		});
		this._reloadSoundLoopEvent = AudioManager.Post(eventName, base.gameObject);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00023374 File Offset: 0x00021574
	private void PostReloadSuccessSound()
	{
		if (this._reloadSoundLoopEvent != null)
		{
			this._reloadSoundLoopEvent.Value.stop(1);
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		string soundkey = this.GunItemSetting.reloadKey.ToLower() + "_end";
		AudioManager.Post(StringExtensions.Format("SFX/Combat/Gun/Reload/{soundkey}", new
		{
			soundkey
		}), base.gameObject);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x000233E8 File Offset: 0x000215E8
	private void PostShootSound()
	{
		string text = this.GunItemSetting.shootKey.ToLower();
		if (this.Silenced)
		{
			text += "_mute";
		}
		string eventName = StringExtensions.Format("SFX/Combat/Gun/Shoot/{soundkey}", new
		{
			soundkey = text
		});
		this._shootSoundEvent = AudioManager.Post(eventName, base.gameObject);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0002343D File Offset: 0x0002163D
	private void StopAllSound()
	{
		AudioManager.StopAll(base.gameObject, 1);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0002344C File Offset: 0x0002164C
	private void StopReloadSound()
	{
		if (this._reloadSoundLoopEvent != null)
		{
			this._reloadSoundLoopEvent.Value.stop(1);
		}
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002347B File Offset: 0x0002167B
	public void CancleReload()
	{
		this.StopReloadSound();
		if (this.gunState == ItemAgent_Gun.GunStates.reloading)
		{
			this.TransToBurstCooling();
			return;
		}
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00023493 File Offset: 0x00021693
	public bool IsFull()
	{
		return this.BulletCount >= this.Capacity;
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x000234A8 File Offset: 0x000216A8
	public int GetBulletCountInInventory()
	{
		if (!this.GunItemSetting || !base.Holder || !base.Holder.CharacterItem)
		{
			return 0;
		}
		return this.GunItemSetting.GetBulletCountofTypeInInventory(this.GunItemSetting.TargetBulletID, base.Holder.CharacterItem.Inventory);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0002350C File Offset: 0x0002170C
	private void StartLoadBullets()
	{
		this.GunItemSetting.LoadBulletsFromInventory(base.Holder.CharacterItem.Inventory).Forget();
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0002353C File Offset: 0x0002173C
	private void StartVisualRecoil()
	{
		this._recoilBack = true;
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00023548 File Offset: 0x00021748
	private void UpdateVisualRecoil()
	{
		bool flag = false;
		if (this._recoilBack)
		{
			flag = true;
			this._recoilMoveValue = Mathf.MoveTowards(this._recoilMoveValue, 1f, this._recoilBackSpeed * Time.deltaTime);
			if (Mathf.Approximately(this._recoilMoveValue, 1f))
			{
				this._recoilBack = false;
			}
		}
		else if (this._recoilMoveValue > 0f)
		{
			flag = true;
			this._recoilMoveValue = Mathf.MoveTowards(this._recoilMoveValue, 0f, this._recoilRecoverSpeed * Time.deltaTime);
		}
		if (flag)
		{
			base.transform.localPosition = Vector3.back * this._recoilMoveValue * this._recoilDistance;
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x000235F8 File Offset: 0x000217F8
	public void SetTrigger(bool trigger, bool _triggerThisFrame, bool _releaseThisFrame)
	{
		this.triggerInput = trigger;
		this.triggerThisFrame = _triggerThisFrame;
		this.releaseThisFrame = _releaseThisFrame;
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x0002360F File Offset: 0x0002180F
	public bool IsReloading()
	{
		return this.gunState == ItemAgent_Gun.GunStates.reloading;
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x0002361C File Offset: 0x0002181C
	public Progress GetReloadProgress()
	{
		Progress result = default(Progress);
		if (this.IsReloading())
		{
			result.inProgress = true;
			result.total = this.ReloadTime;
			result.current = this.stateTimer;
		}
		else
		{
			result.inProgress = false;
		}
		return result;
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00023668 File Offset: 0x00021868
	public ADSAimMarker GetAimMarkerPfb()
	{
		Slot slot = base.Item.Slots.GetSlot("Scope");
		if (slot != null && slot.Content != null)
		{
			ItemSetting_Accessory component = slot.Content.GetComponent<ItemSetting_Accessory>();
			if (component.overrideAdsAimMarker)
			{
				return component.overrideAdsAimMarker;
			}
		}
		slot = base.Item.Slots.GetSlot("Special");
		if (slot != null && slot.Content != null)
		{
			ItemSetting_Accessory component2 = slot.Content.GetComponent<ItemSetting_Accessory>();
			if (component2.overrideAdsAimMarker)
			{
				return component2.overrideAdsAimMarker;
			}
		}
		return this._gunItemSetting.adsAimMarker;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x0002370E File Offset: 0x0002190E
	public void SetMuzzleSocketLocalPos(Vector3 pos)
	{
		if (!this.muzzle1)
		{
			return;
		}
		this.muzzle1.transform.localPosition = pos;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x0002372F File Offset: 0x0002192F
	public void SetTecSocketLocalPosition(Vector3 pos)
	{
		base.GetSocket("Tec", true).transform.localPosition = pos;
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00023748 File Offset: 0x00021948
	public static ItemAgent_Gun BuildAgent(GameObject modelPfb)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(modelPfb);
		Transform transform = gameObject.transform.Find("Sockets");
		if (!transform)
		{
			transform = new GameObject().transform;
			transform.SetParent(gameObject.transform);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
		Transform transform2 = transform.Find("Muzzle");
		if (!transform2)
		{
			transform2 = ItemAgent_Gun.<BuildAgent>g__CreateSocket|280_0(transform, "Muzzle");
		}
		Transform transform3 = transform.Find("Muzzle2");
		Transform transform4 = transform.Find("Tec");
		if (!transform4)
		{
			transform4 = ItemAgent_Gun.<BuildAgent>g__CreateSocket|280_0(transform, "Tec");
		}
		ItemAgent_Gun itemAgent_Gun = gameObject.AddComponent<ItemAgent_Gun>();
		itemAgent_Gun.AddSocket(transform2);
		if (transform3)
		{
			itemAgent_Gun.AddSocket(transform3);
		}
		itemAgent_Gun.AddSocket(transform4);
		itemAgent_Gun.handheldSocket = HandheldSocketTypes.normalHandheld;
		itemAgent_Gun.handAnimationType = HandheldAnimationType.gun;
		Transform transform5 = gameObject.transform.Find("ShellEmitter");
		if (transform5)
		{
			ParticleSystem component = transform5.GetComponent<ParticleSystem>();
			if (component)
			{
				itemAgent_Gun.shellParticle = component;
			}
		}
		CharacterSubVisuals characterSubVisuals = itemAgent_Gun.GetComponent<CharacterSubVisuals>();
		if (!characterSubVisuals)
		{
			characterSubVisuals = ComponentHolderProtocol.AddComponent<CharacterSubVisuals>(itemAgent_Gun);
		}
		characterSubVisuals.SetRenderers();
		return itemAgent_Gun;
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00023C2F File Offset: 0x00021E2F
	[CompilerGenerated]
	internal static Transform <BuildAgent>g__CreateSocket|280_0(Transform _socketParent, string name)
	{
		Transform transform = new GameObject().transform;
		transform.name = name;
		transform.SetParent(_socketParent);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		return transform;
	}

	// Token: 0x04000722 RID: 1826
	private Item _bulletItem;

	// Token: 0x04000723 RID: 1827
	private static int ShootSpeedHash = "ShootSpeed".GetHashCode();

	// Token: 0x04000724 RID: 1828
	private static int ShootSpeedGainEachShootHash = "ShootSpeedGainEachShoot".GetHashCode();

	// Token: 0x04000725 RID: 1829
	private static int ShootSpeedGainByShootMaxHash = "ShootSpeedGainByShootMax".GetHashCode();

	// Token: 0x04000726 RID: 1830
	private float shootSpeedGainOverShoot;

	// Token: 0x04000727 RID: 1831
	private static int ReloadTimeHash = "ReloadTime".GetHashCode();

	// Token: 0x04000728 RID: 1832
	private static int CapacityHash = "Capacity".GetHashCode();

	// Token: 0x04000729 RID: 1833
	private static int TraceAbilityHash = "TraceAbility".GetHashCode();

	// Token: 0x0400072A RID: 1834
	private static int DurabilityHash = "Durability".GetHashCode();

	// Token: 0x0400072B RID: 1835
	private float maxDurability;

	// Token: 0x0400072C RID: 1836
	private static int DamageHash = "Damage".GetHashCode();

	// Token: 0x0400072D RID: 1837
	private static int BurstCountHash = "BurstCount".GetHashCode();

	// Token: 0x0400072E RID: 1838
	private static int BulletSpeedHash = "BulletSpeed".GetHashCode();

	// Token: 0x0400072F RID: 1839
	private static int BulletDistanceHash = "BulletDistance".GetHashCode();

	// Token: 0x04000730 RID: 1840
	private static int PenetrateHash = "Penetrate".GetHashCode();

	// Token: 0x04000731 RID: 1841
	private static int explosionDamageMultiplierHash = "ExplosionDamageMultiplier".GetHashCode();

	// Token: 0x04000732 RID: 1842
	private static int CritRateHash = "CritRate".GetHashCode();

	// Token: 0x04000733 RID: 1843
	private static int CritDamageFactorHash = "CritDamageFactor".GetHashCode();

	// Token: 0x04000734 RID: 1844
	private static int SoundRangeHash = "SoundRange".GetHashCode();

	// Token: 0x04000735 RID: 1845
	private static int ArmorPiercingHash = "ArmorPiercing".GetHashCode();

	// Token: 0x04000736 RID: 1846
	private static int ArmorBreakHash = "ArmorBreak".GetHashCode();

	// Token: 0x04000737 RID: 1847
	private static int ShotCountHash = "ShotCount".GetHashCode();

	// Token: 0x04000738 RID: 1848
	private static int ShotAngleHash = "ShotAngle".GetHashCode();

	// Token: 0x04000739 RID: 1849
	private static int ADSAimDistanceFactorHash = "ADSAimDistanceFactor".GetHashCode();

	// Token: 0x0400073A RID: 1850
	private static int AdsTimeHash = "ADSTime".GetHashCode();

	// Token: 0x0400073B RID: 1851
	private float scatterFactorHips = 1f;

	// Token: 0x0400073C RID: 1852
	private float scatterFactorAds = 1f;

	// Token: 0x0400073D RID: 1853
	private static int ScatterFactorHash = "ScatterFactor".GetHashCode();

	// Token: 0x0400073E RID: 1854
	private static int ScatterFactorHashADS = "ScatterFactorADS".GetHashCode();

	// Token: 0x0400073F RID: 1855
	private static int DefaultScatterHash = "DefaultScatter".GetHashCode();

	// Token: 0x04000740 RID: 1856
	private static int DefaultScatterHashADS = "DefaultScatterADS".GetHashCode();

	// Token: 0x04000741 RID: 1857
	private static int MaxScatterHash = "MaxScatter".GetHashCode();

	// Token: 0x04000742 RID: 1858
	private static int MaxScatterHashADS = "MaxScatterADS".GetHashCode();

	// Token: 0x04000743 RID: 1859
	private static int ScatterGrowHash = "ScatterGrow".GetHashCode();

	// Token: 0x04000744 RID: 1860
	private static int ScatterGrowHashADS = "ScatterGrowADS".GetHashCode();

	// Token: 0x04000745 RID: 1861
	private static int ScatterRecoverHash = "ScatterRecover".GetHashCode();

	// Token: 0x04000746 RID: 1862
	private static int ScatterRecoverHashADS = "ScatterRecoverADS".GetHashCode();

	// Token: 0x04000747 RID: 1863
	private static int RecoilVMinHash = "RecoilVMin".GetHashCode();

	// Token: 0x04000748 RID: 1864
	private static int RecoilVMaxHash = "RecoilVMax".GetHashCode();

	// Token: 0x04000749 RID: 1865
	private static int RecoilHMinHash = "RecoilHMin".GetHashCode();

	// Token: 0x0400074A RID: 1866
	private static int RecoilHMaxHash = "RecoilHMax".GetHashCode();

	// Token: 0x0400074B RID: 1867
	private static int RecoilScaleVHash = "RecoilScaleV".GetHashCode();

	// Token: 0x0400074C RID: 1868
	private static int RecoilScaleHHash = "RecoilScaleH".GetHashCode();

	// Token: 0x0400074D RID: 1869
	private static int RecoilRecoverHash = "RecoilRecover".GetHashCode();

	// Token: 0x0400074E RID: 1870
	private static int RecoilTimeHash = "RecoilTime".GetHashCode();

	// Token: 0x0400074F RID: 1871
	private static int RecoilRecoverTimeHash = "RecoilRecoverTime".GetHashCode();

	// Token: 0x04000750 RID: 1872
	private static int MoveSpeedMultiplierHash = "MoveSpeedMultiplier".GetHashCode();

	// Token: 0x04000751 RID: 1873
	private static int AdsWalkSpeedMultiplierHash = "AdsWalkSpeedMultiplier".GetHashCode();

	// Token: 0x04000752 RID: 1874
	private static int BuffChanceHash = "BuffChance".GetHashCode();

	// Token: 0x04000753 RID: 1875
	private static int bulletCritRateGainHash = "CritRateGain".GetHashCode();

	// Token: 0x04000754 RID: 1876
	private static int bulletCritDamageFactorGainHash = "CritDamageFactorGain".GetHashCode();

	// Token: 0x04000755 RID: 1877
	private static int bulletArmorPiercingGainHash = "ArmorPiercingGain".GetHashCode();

	// Token: 0x04000756 RID: 1878
	private static int BulletDamageMultiplierHash = "damageMultiplier".GetHashCode();

	// Token: 0x04000757 RID: 1879
	private static int bulletExplosionRangeHash = "ExplosionRange".GetHashCode();

	// Token: 0x04000758 RID: 1880
	private static int BulletBuffChanceMultiplierHash = "buffChanceMultiplier".GetHashCode();

	// Token: 0x04000759 RID: 1881
	private static int BulletBleedChanceHash = "bleedChance".GetHashCode();

	// Token: 0x0400075A RID: 1882
	private static int bulletExplosionDamageHash = "ExplosionDamage".GetHashCode();

	// Token: 0x0400075B RID: 1883
	private static int armorBreakGainHash = "ArmorBreakGain".GetHashCode();

	// Token: 0x0400075C RID: 1884
	private static int bulletDurabilityCostHash = "DurabilityCost".GetHashCode();

	// Token: 0x0400075D RID: 1885
	private int muzzleIndex;

	// Token: 0x0400075E RID: 1886
	public GameObject loadedVisualObject;

	// Token: 0x0400075F RID: 1887
	private float adsValue;

	// Token: 0x04000760 RID: 1888
	private CharacterMainControl traceTarget;

	// Token: 0x04000761 RID: 1889
	private Transform _mz1;

	// Token: 0x04000762 RID: 1890
	private Transform _mz2;

	// Token: 0x04000763 RID: 1891
	private bool hasMz2 = true;

	// Token: 0x04000764 RID: 1892
	[SerializeField]
	private ParticleSystem shellParticle;

	// Token: 0x04000765 RID: 1893
	private ItemSetting_Gun _gunItemSetting;

	// Token: 0x04000766 RID: 1894
	private bool triggerInput;

	// Token: 0x04000767 RID: 1895
	private bool triggerThisFrame;

	// Token: 0x04000768 RID: 1896
	private bool releaseThisFrame;

	// Token: 0x04000769 RID: 1897
	private bool triggerBuffer;

	// Token: 0x0400076A RID: 1898
	private float scatterBeforeControl;

	// Token: 0x0400076E RID: 1902
	private EventInstance? _shootSoundEvent;

	// Token: 0x0400076F RID: 1903
	private EventInstance? _reloadSoundLoopEvent;

	// Token: 0x04000770 RID: 1904
	private Collider[] traceColliders = new Collider[10];

	// Token: 0x04000771 RID: 1905
	private LayerMask damageReceiverLayermask;

	// Token: 0x04000772 RID: 1906
	private float stateTimer;

	// Token: 0x04000773 RID: 1907
	private int burstCounter;

	// Token: 0x04000774 RID: 1908
	private Projectile projInst;

	// Token: 0x04000775 RID: 1909
	private ItemAgent_Gun.GunStates gunState = ItemAgent_Gun.GunStates.ready;

	// Token: 0x04000776 RID: 1910
	private bool needAutoReload;

	// Token: 0x04000777 RID: 1911
	private bool loadBulletsStarted;

	// Token: 0x04000778 RID: 1912
	private float _recoilMoveValue;

	// Token: 0x04000779 RID: 1913
	private float _recoilDistance = 0.2f;

	// Token: 0x0400077A RID: 1914
	private float _recoilBackSpeed = 20f;

	// Token: 0x0400077B RID: 1915
	private float _recoilRecoverSpeed = 8f;

	// Token: 0x0400077C RID: 1916
	private bool _recoilBack;

	// Token: 0x02000486 RID: 1158
	public enum GunStates
	{
		// Token: 0x04001C15 RID: 7189
		shootCooling,
		// Token: 0x04001C16 RID: 7190
		ready,
		// Token: 0x04001C17 RID: 7191
		fire,
		// Token: 0x04001C18 RID: 7192
		burstEachShotCooling,
		// Token: 0x04001C19 RID: 7193
		empty,
		// Token: 0x04001C1A RID: 7194
		reloading
	}
}
