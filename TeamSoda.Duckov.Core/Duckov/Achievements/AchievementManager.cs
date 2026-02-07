using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Economy;
using Duckov.Endowment;
using Duckov.Quests;
using Duckov.Rules.UI;
using Duckov.Scenes;
using Saves;
using UnityEngine;

namespace Duckov.Achievements
{
	// Token: 0x02000339 RID: 825
	public class AchievementManager : MonoBehaviour
	{
		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001B96 RID: 7062 RVA: 0x0006440F File Offset: 0x0006260F
		public static AchievementManager Instance
		{
			get
			{
				return GameManager.AchievementManager;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001B97 RID: 7063 RVA: 0x00064416 File Offset: 0x00062616
		public static bool CanUnlockAchievement
		{
			get
			{
				return !DifficultySelection.CustomDifficultyMarker;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001B98 RID: 7064 RVA: 0x00064422 File Offset: 0x00062622
		public List<string> UnlockedAchievements
		{
			get
			{
				return this._unlockedAchievements;
			}
		}

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06001B99 RID: 7065 RVA: 0x0006442C File Offset: 0x0006262C
		// (remove) Token: 0x06001B9A RID: 7066 RVA: 0x00064460 File Offset: 0x00062660
		public static event Action<AchievementManager> OnAchievementDataLoaded;

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06001B9B RID: 7067 RVA: 0x00064494 File Offset: 0x00062694
		// (remove) Token: 0x06001B9C RID: 7068 RVA: 0x000644C8 File Offset: 0x000626C8
		public static event Action<string> OnAchievementUnlocked;

		// Token: 0x06001B9D RID: 7069 RVA: 0x000644FB File Offset: 0x000626FB
		private void Awake()
		{
			this.Load();
			this.RegisterEvents();
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x00064509 File Offset: 0x00062709
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x00064511 File Offset: 0x00062711
		private void Start()
		{
			this.MakeSureMoneyAchievementsUnlocked();
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0006451C File Offset: 0x0006271C
		private void RegisterEvents()
		{
			Quest.onQuestCompleted += this.OnQuestCompleted;
			SavesCounter.OnKillCountChanged = (Action<string, int>)Delegate.Combine(SavesCounter.OnKillCountChanged, new Action<string, int>(this.OnKillCountChanged));
			MultiSceneCore.OnSetSceneVisited += this.OnSetSceneVisited;
			LevelManager.OnEvacuated += this.OnEvacuated;
			EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
			EndowmentManager.OnEndowmentUnlock = (Action<EndowmentIndex>)Delegate.Combine(EndowmentManager.OnEndowmentUnlock, new Action<EndowmentIndex>(this.OnEndowmentUnlocked));
			EconomyManager.OnEconomyManagerLoaded += this.OnEconomyManagerLoaded;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000645C0 File Offset: 0x000627C0
		private void UnregisterEvents()
		{
			Quest.onQuestCompleted -= this.OnQuestCompleted;
			SavesCounter.OnKillCountChanged = (Action<string, int>)Delegate.Remove(SavesCounter.OnKillCountChanged, new Action<string, int>(this.OnKillCountChanged));
			MultiSceneCore.OnSetSceneVisited -= this.OnSetSceneVisited;
			LevelManager.OnEvacuated -= this.OnEvacuated;
			EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
			EndowmentManager.OnEndowmentUnlock = (Action<EndowmentIndex>)Delegate.Remove(EndowmentManager.OnEndowmentUnlock, new Action<EndowmentIndex>(this.OnEndowmentUnlocked));
			EconomyManager.OnEconomyManagerLoaded -= this.OnEconomyManagerLoaded;
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x00064662 File Offset: 0x00062862
		private void OnEconomyManagerLoaded()
		{
			this.MakeSureMoneyAchievementsUnlocked();
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0006466A File Offset: 0x0006286A
		private void OnEndowmentUnlocked(EndowmentIndex index)
		{
			this.Unlock(string.Format("Endowmment_{0}", index));
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x00064682 File Offset: 0x00062882
		public static void UnlockEndowmentAchievement(EndowmentIndex index)
		{
			if (AchievementManager.Instance == null)
			{
				return;
			}
			AchievementManager.Instance.Unlock(string.Format("Endowmment_{0}", index));
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x000646AC File Offset: 0x000628AC
		private void OnMoneyChanged(long oldValue, long newValue)
		{
			if (oldValue < 10000L && newValue >= 10000L)
			{
				this.Unlock("Money_10K");
			}
			if (oldValue < 100000L && newValue >= 100000L)
			{
				this.Unlock("Money_100K");
			}
			if (oldValue < 1000000L && newValue >= 1000000L)
			{
				this.Unlock("Money_1M");
			}
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x00064710 File Offset: 0x00062910
		private void MakeSureMoneyAchievementsUnlocked()
		{
			long money = EconomyManager.Money;
			if (money >= 10000L)
			{
				this.Unlock("Money_10K");
			}
			if (money >= 100000L)
			{
				this.Unlock("Money_100K");
			}
			if (money >= 1000000L)
			{
				this.Unlock("Money_1M");
			}
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x00064760 File Offset: 0x00062960
		private void OnEvacuated(EvacuationInfo info)
		{
			string mainSceneID = MultiSceneCore.MainSceneID;
			if (!this.evacuateSceneIDs.Contains(mainSceneID))
			{
				return;
			}
			this.Unlock("Evacuate_" + mainSceneID);
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x00064793 File Offset: 0x00062993
		private void OnSetSceneVisited(string id)
		{
			if (!this.achievementSceneIDs.Contains(id))
			{
				return;
			}
			this.Unlock("Arrive_" + id);
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x000647B8 File Offset: 0x000629B8
		private void OnKillCountChanged(string key, int value)
		{
			this.Unlock("FirstBlood");
			if (AchievementDatabase.Instance == null)
			{
				return;
			}
			Debug.Log("COUNTING " + key);
			foreach (AchievementManager.KillCountAchievement killCountAchievement in this.KillCountAchivements)
			{
				if (killCountAchievement.key == key && value >= killCountAchievement.value)
				{
					this.Unlock(string.Format("Kill_{0}_{1}", key, killCountAchievement.value));
				}
			}
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x00064840 File Offset: 0x00062A40
		private void OnQuestCompleted(Quest quest)
		{
			if (AchievementDatabase.Instance == null)
			{
				return;
			}
			string id = string.Format("Quest_{0}", quest.ID);
			AchievementDatabase.Achievement achievement;
			if (!AchievementDatabase.TryGetAchievementData(id, out achievement))
			{
				return;
			}
			this.Unlock(id);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x00064883 File Offset: 0x00062A83
		private void Save()
		{
			SavesSystem.SaveGlobal<List<string>>("Achievements", this.UnlockedAchievements);
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x00064898 File Offset: 0x00062A98
		private void Load()
		{
			this.UnlockedAchievements.Clear();
			List<string> list = SavesSystem.LoadGlobal<List<string>>("Achievements", null);
			if (list != null)
			{
				this.UnlockedAchievements.AddRange(list);
			}
			Action<AchievementManager> onAchievementDataLoaded = AchievementManager.OnAchievementDataLoaded;
			if (onAchievementDataLoaded == null)
			{
				return;
			}
			onAchievementDataLoaded(this);
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x000648DC File Offset: 0x00062ADC
		public void Unlock(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				Debug.LogError("Trying to unlock a empty acheivement.", this);
				return;
			}
			id = id.Trim();
			AchievementDatabase.Achievement achievement;
			if (!AchievementDatabase.TryGetAchievementData(id, out achievement))
			{
				Debug.LogError("Invalid acheivement id: " + id);
			}
			if (this.UnlockedAchievements.Contains(id))
			{
				return;
			}
			if (!AchievementManager.CanUnlockAchievement)
			{
				return;
			}
			this.UnlockedAchievements.Add(id);
			this.Save();
			Action<string> onAchievementUnlocked = AchievementManager.OnAchievementUnlocked;
			if (onAchievementUnlocked == null)
			{
				return;
			}
			onAchievementUnlocked(id);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x00064958 File Offset: 0x00062B58
		public static bool IsIDValid(string id)
		{
			return !(AchievementDatabase.Instance == null) && AchievementDatabase.Instance.IsIDValid(id);
		}

		// Token: 0x040013BF RID: 5055
		private List<string> _unlockedAchievements = new List<string>();

		// Token: 0x040013C2 RID: 5058
		private readonly string[] evacuateSceneIDs = new string[]
		{
			"Level_GroundZero_Main"
		};

		// Token: 0x040013C3 RID: 5059
		private readonly string[] achievementSceneIDs = new string[]
		{
			"Base",
			"Level_GroundZero_Main",
			"Level_HiddenWarehouse_Main",
			"Level_Farm_Main",
			"Level_JLab_Main",
			"Level_StormZone_Main"
		};

		// Token: 0x040013C4 RID: 5060
		private readonly AchievementManager.KillCountAchievement[] KillCountAchivements = new AchievementManager.KillCountAchievement[]
		{
			new AchievementManager.KillCountAchievement("Cname_ShortEagle", 10),
			new AchievementManager.KillCountAchievement("Cname_ShortEagle", 1),
			new AchievementManager.KillCountAchievement("Cname_Speedy", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss1", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss2", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss3", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss4", 1),
			new AchievementManager.KillCountAchievement("Cname_StormBoss5", 1),
			new AchievementManager.KillCountAchievement("Cname_Boss_Sniper", 1),
			new AchievementManager.KillCountAchievement("Cname_Vida", 1),
			new AchievementManager.KillCountAchievement("Cname_Roadblock", 1),
			new AchievementManager.KillCountAchievement("Cname_SchoolBully", 1),
			new AchievementManager.KillCountAchievement("Cname_Boss_Fly", 1),
			new AchievementManager.KillCountAchievement("Cname_Boss_Arcade", 1),
			new AchievementManager.KillCountAchievement("Cname_UltraMan", 1),
			new AchievementManager.KillCountAchievement("Cname_LabTestObjective", 1)
		};

		// Token: 0x020005E6 RID: 1510
		private struct KillCountAchievement
		{
			// Token: 0x06002A3C RID: 10812 RVA: 0x0009CFDC File Offset: 0x0009B1DC
			public KillCountAchievement(string key, int value)
			{
				this.key = key;
				this.value = value;
			}

			// Token: 0x04002174 RID: 8564
			public string key;

			// Token: 0x04002175 RID: 8565
			public int value;
		}
	}
}
