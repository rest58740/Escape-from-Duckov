using System;
using System.Collections.Generic;
using System.Linq;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029D RID: 669
	[Serializable]
	public class GoldMinerRunData
	{
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x00051835 File Offset: 0x0004FA35
		// (set) Token: 0x060015E2 RID: 5602 RVA: 0x0005183D File Offset: 0x0004FA3D
		public int seed { get; private set; }

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x00051846 File Offset: 0x0004FA46
		// (set) Token: 0x060015E4 RID: 5604 RVA: 0x0005184E File Offset: 0x0004FA4E
		public Random shopRandom { get; set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060015E5 RID: 5605 RVA: 0x00051857 File Offset: 0x0004FA57
		// (set) Token: 0x060015E6 RID: 5606 RVA: 0x0005185F File Offset: 0x0004FA5F
		public Random levelRandom { get; private set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060015E7 RID: 5607 RVA: 0x00051868 File Offset: 0x0004FA68
		public float GameSpeedFactor
		{
			get
			{
				return this.gameSpeedFactor.Value;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x00051875 File Offset: 0x0004FA75
		// (set) Token: 0x060015E9 RID: 5609 RVA: 0x0005187D File Offset: 0x0004FA7D
		public float stamina { get; set; }

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x00051886 File Offset: 0x0004FA86
		// (set) Token: 0x060015EB RID: 5611 RVA: 0x0005188E File Offset: 0x0004FA8E
		public bool gameOver { get; set; }

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x00051897 File Offset: 0x0004FA97
		// (set) Token: 0x060015ED RID: 5613 RVA: 0x0005189F File Offset: 0x0004FA9F
		public int level { get; set; }

		// Token: 0x060015EE RID: 5614 RVA: 0x000518A8 File Offset: 0x0004FAA8
		public GoldMinerArtifact AttachArtifactFromPrefab(GoldMinerArtifact prefab)
		{
			if (prefab == null)
			{
				return null;
			}
			GoldMinerArtifact goldMinerArtifact = Object.Instantiate<GoldMinerArtifact>(prefab, this.master.transform);
			this.AttachArtifact(goldMinerArtifact);
			return goldMinerArtifact;
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x000518DC File Offset: 0x0004FADC
		private void AttachArtifact(GoldMinerArtifact artifact)
		{
			if (this.artifactCount.ContainsKey(artifact.ID))
			{
				Dictionary<string, int> dictionary = this.artifactCount;
				string id = artifact.ID;
				dictionary[id]++;
			}
			else
			{
				this.artifactCount[artifact.ID] = 1;
			}
			this.artifacts.Add(artifact);
			artifact.Attach(this.master);
			this.master.NotifyArtifactChange();
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00051954 File Offset: 0x0004FB54
		public bool DetachArtifact(GoldMinerArtifact artifact)
		{
			bool result = this.artifacts.Remove(artifact);
			artifact.Detatch(this.master);
			if (this.artifactCount.ContainsKey(artifact.ID))
			{
				Dictionary<string, int> dictionary = this.artifactCount;
				string id = artifact.ID;
				dictionary[id]--;
			}
			else
			{
				Debug.LogError("Artifact counter error.", this.master);
			}
			this.master.NotifyArtifactChange();
			return result;
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x000519C8 File Offset: 0x0004FBC8
		public int GetArtifactCount(string id)
		{
			int result;
			if (this.artifactCount.TryGetValue(id, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x000519E8 File Offset: 0x0004FBE8
		public GoldMinerRunData(GoldMiner master, int seed = 0)
		{
			this.master = master;
			if (seed == 0)
			{
				seed = Random.Range(int.MinValue, int.MaxValue);
			}
			this.seed = seed;
			this.levelRandom = new Random(seed);
			this.strengthPotionModifier = new Modifier(0, 100f, this);
			this.eagleEyeModifier = new Modifier(200, -0.5f, this);
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060015F3 RID: 5619 RVA: 0x00051BF7 File Offset: 0x0004FDF7
		// (set) Token: 0x060015F4 RID: 5620 RVA: 0x00051BFF File Offset: 0x0004FDFF
		public bool StrengthPotionActivated { get; private set; }

		// Token: 0x060015F5 RID: 5621 RVA: 0x00051C08 File Offset: 0x0004FE08
		public void ActivateStrengthPotion()
		{
			if (this.StrengthPotionActivated)
			{
				return;
			}
			this.strength.AddModifier(this.strengthPotionModifier);
			this.StrengthPotionActivated = true;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x00051C2B File Offset: 0x0004FE2B
		public void DeactivateStrengthPotion()
		{
			this.strength.RemoveModifier(this.strengthPotionModifier);
			this.StrengthPotionActivated = false;
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00051C46 File Offset: 0x0004FE46
		// (set) Token: 0x060015F8 RID: 5624 RVA: 0x00051C4E File Offset: 0x0004FE4E
		public bool EagleEyeActivated { get; private set; }

		// Token: 0x060015F9 RID: 5625 RVA: 0x00051C57 File Offset: 0x0004FE57
		public void ActivateEagleEye()
		{
			if (this.EagleEyeActivated)
			{
				return;
			}
			this.gameSpeedFactor.AddModifier(this.eagleEyeModifier);
			this.EagleEyeActivated = true;
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00051C7A File Offset: 0x0004FE7A
		public void DeactivateEagleEye()
		{
			this.gameSpeedFactor.RemoveModifier(this.eagleEyeModifier);
			this.EagleEyeActivated = false;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00051C98 File Offset: 0x0004FE98
		internal void Cleanup()
		{
			foreach (GoldMinerArtifact goldMinerArtifact in this.artifacts)
			{
				if (!(goldMinerArtifact == null))
				{
					if (Application.isPlaying)
					{
						Object.Destroy(goldMinerArtifact.gameObject);
					}
					else
					{
						Object.Destroy(goldMinerArtifact.gameObject);
					}
				}
			}
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00051D0C File Offset: 0x0004FF0C
		public bool IsGold(GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			using (List<Func<GoldMinerEntity, bool>>.Enumerator enumerator = this.isGoldPredicators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current(entity))
					{
						return true;
					}
				}
			}
			return entity.tags.Contains(GoldMinerEntity.Tag.Gold);
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00051D7C File Offset: 0x0004FF7C
		public bool IsRock(GoldMinerEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			using (List<Func<GoldMinerEntity, bool>>.Enumerator enumerator = this.isGoldPredicators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current(entity))
					{
						return true;
					}
				}
			}
			return entity.tags.Contains(GoldMinerEntity.Tag.Rock);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00051DEC File Offset: 0x0004FFEC
		internal bool IsPig(GoldMinerEntity entity)
		{
			return entity.tags.Contains(GoldMinerEntity.Tag.Pig);
		}

		// Token: 0x0400100F RID: 4111
		public readonly GoldMiner master;

		// Token: 0x04001013 RID: 4115
		public int money;

		// Token: 0x04001014 RID: 4116
		public int bomb;

		// Token: 0x04001015 RID: 4117
		public int strengthPotion;

		// Token: 0x04001016 RID: 4118
		public int eagleEyePotion;

		// Token: 0x04001017 RID: 4119
		public int shopTicket;

		// Token: 0x04001018 RID: 4120
		public const int shopDefaultItemAmount = 3;

		// Token: 0x04001019 RID: 4121
		public const int shopMaxItemAmount = 3;

		// Token: 0x0400101A RID: 4122
		public int shopCapacity = 3;

		// Token: 0x0400101B RID: 4123
		public float levelScoreFactor;

		// Token: 0x0400101C RID: 4124
		public Stat maxStamina = new Stat("maxStamina", 15f, false);

		// Token: 0x0400101D RID: 4125
		public Stat extraStamina = new Stat("extraStamina", 2f, false);

		// Token: 0x0400101E RID: 4126
		public Stat staminaDrain = new Stat("staminaDrain", 1f, false);

		// Token: 0x0400101F RID: 4127
		public Stat gameSpeedFactor = new Stat("gameSpeedFactor", 1f, false);

		// Token: 0x04001020 RID: 4128
		public Stat emptySpeed = new Stat("emptySpeed", 300f, false);

		// Token: 0x04001021 RID: 4129
		public Stat strength = new Stat("strength", 0f, false);

		// Token: 0x04001022 RID: 4130
		public Stat scoreFactorBase = new Stat("scoreFactor", 1f, false);

		// Token: 0x04001023 RID: 4131
		public Stat rockValueFactor = new Stat("rockValueFactor", 1f, false);

		// Token: 0x04001024 RID: 4132
		public Stat goldValueFactor = new Stat("goldValueFactor", 1f, false);

		// Token: 0x04001025 RID: 4133
		public Stat charm = new Stat("charm", 1f, false);

		// Token: 0x04001026 RID: 4134
		public Stat shopRefreshPrice = new Stat("shopRefreshPrice", 100f, false);

		// Token: 0x04001027 RID: 4135
		public Stat shopRefreshPriceIncrement = new Stat("shopRefreshPriceIncrement", 50f, false);

		// Token: 0x04001028 RID: 4136
		public Stat shopRefreshChances = new Stat("shopRefreshChances", 2f, false);

		// Token: 0x04001029 RID: 4137
		public Stat shopPriceCut = new Stat("shopPriceCut", 0.7f, false);

		// Token: 0x0400102A RID: 4138
		public Stat defuse = new Stat("defuse", 0f, false);

		// Token: 0x0400102B RID: 4139
		public float extraRocks;

		// Token: 0x0400102C RID: 4140
		public float extraGold;

		// Token: 0x0400102D RID: 4141
		public float extraDiamond;

		// Token: 0x0400102E RID: 4142
		public List<GoldMinerArtifact> artifacts = new List<GoldMinerArtifact>();

		// Token: 0x04001032 RID: 4146
		private Dictionary<string, int> artifactCount = new Dictionary<string, int>();

		// Token: 0x04001033 RID: 4147
		private Modifier strengthPotionModifier;

		// Token: 0x04001035 RID: 4149
		private Modifier eagleEyeModifier;

		// Token: 0x04001036 RID: 4150
		internal int targetScore = 100;

		// Token: 0x04001038 RID: 4152
		public List<Func<GoldMinerEntity, bool>> isGoldPredicators = new List<Func<GoldMinerEntity, bool>>();

		// Token: 0x04001039 RID: 4153
		public List<Func<GoldMinerEntity, bool>> isRockPredicators = new List<Func<GoldMinerEntity, bool>>();

		// Token: 0x0400103A RID: 4154
		public List<Func<float>> additionalFactorFuncs = new List<Func<float>>();

		// Token: 0x0400103B RID: 4155
		public List<Func<int, int>> settleValueProcessor = new List<Func<int, int>>();

		// Token: 0x0400103C RID: 4156
		public List<Func<bool>> forceLevelSuccessFuncs = new List<Func<bool>>();

		// Token: 0x0400103D RID: 4157
		internal int minMoneySum;
	}
}
