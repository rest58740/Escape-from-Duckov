using System;
using Duckov.Utilities;
using Duckov.Weathers;
using UnityEngine;

// Token: 0x0200010B RID: 267
public class LevelConfig : MonoBehaviour
{
	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x0002967A File Offset: 0x0002787A
	public static LevelConfig Instance
	{
		get
		{
			if (!LevelConfig.instance)
			{
				LevelConfig.SetInstance();
			}
			return LevelConfig.instance;
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06000916 RID: 2326 RVA: 0x00029692 File Offset: 0x00027892
	public static bool SaveCharacter
	{
		get
		{
			return LevelConfig.Instance && LevelConfig.Instance.saveCharacter;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06000917 RID: 2327 RVA: 0x000296AC File Offset: 0x000278AC
	public static bool SavePet
	{
		get
		{
			return LevelConfig.Instance && LevelConfig.Instance.savePet;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06000918 RID: 2328 RVA: 0x000296C6 File Offset: 0x000278C6
	public static Seasons Season
	{
		get
		{
			if (!LevelConfig.Instance)
			{
				return Seasons.spring;
			}
			return LevelConfig.Instance.season;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000919 RID: 2329 RVA: 0x000296E0 File Offset: 0x000278E0
	public float LootBoxQualityLowPercent
	{
		get
		{
			return 1f - 1f / this.lootBoxHighQualityChanceMultiplier;
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x0600091A RID: 2330 RVA: 0x000296F4 File Offset: 0x000278F4
	public float LootboxItemCountMultiplier
	{
		get
		{
			return this.lootboxItemCountMultiplier;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x0600091B RID: 2331 RVA: 0x000296FC File Offset: 0x000278FC
	public static bool IsBaseLevel
	{
		get
		{
			return LevelConfig.Instance && LevelConfig.Instance.isBaseLevel;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x0600091C RID: 2332 RVA: 0x00029716 File Offset: 0x00027916
	public static bool IsRaidMap
	{
		get
		{
			return LevelConfig.Instance && LevelConfig.Instance.isRaidMap;
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x00029730 File Offset: 0x00027930
	public static int MinExitCount
	{
		get
		{
			if (!LevelConfig.Instance)
			{
				return 0;
			}
			return LevelConfig.Instance.minExitCount;
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x0600091E RID: 2334 RVA: 0x0002974A File Offset: 0x0002794A
	public static bool SpawnTomb
	{
		get
		{
			return !LevelConfig.Instance || LevelConfig.Instance.spawnTomb;
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x0600091F RID: 2335 RVA: 0x00029764 File Offset: 0x00027964
	public static int MaxExitCount
	{
		get
		{
			if (!LevelConfig.Instance)
			{
				return 0;
			}
			return LevelConfig.Instance.maxExitCount;
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0002977E File Offset: 0x0002797E
	private void Awake()
	{
		Object.Instantiate<LevelManager>(GameplayDataSettings.Prefabs.LevelManagerPrefab).transform.SetParent(base.transform);
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0002979F File Offset: 0x0002799F
	private static void SetInstance()
	{
		if (LevelConfig.instance)
		{
			return;
		}
		LevelConfig.instance = Object.FindFirstObjectByType<LevelConfig>();
		LevelConfig.instance;
	}

	// Token: 0x04000853 RID: 2131
	private static LevelConfig instance;

	// Token: 0x04000854 RID: 2132
	[SerializeField]
	private bool isBaseLevel;

	// Token: 0x04000855 RID: 2133
	[SerializeField]
	private bool isRaidMap = true;

	// Token: 0x04000856 RID: 2134
	[SerializeField]
	private bool spawnTomb = true;

	// Token: 0x04000857 RID: 2135
	[SerializeField]
	private bool saveCharacter = true;

	// Token: 0x04000858 RID: 2136
	[SerializeField]
	private bool savePet = true;

	// Token: 0x04000859 RID: 2137
	[SerializeField]
	private int minExitCount;

	// Token: 0x0400085A RID: 2138
	[SerializeField]
	private int maxExitCount;

	// Token: 0x0400085B RID: 2139
	[SerializeField]
	private Seasons season;

	// Token: 0x0400085C RID: 2140
	public TimeOfDayConfig timeOfDayConfig;

	// Token: 0x0400085D RID: 2141
	[SerializeField]
	[Min(1f)]
	private float lootBoxHighQualityChanceMultiplier = 1f;

	// Token: 0x0400085E RID: 2142
	[SerializeField]
	[Range(0.1f, 10f)]
	private float lootboxItemCountMultiplier = 1f;
}
