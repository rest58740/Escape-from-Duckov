using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Duckov.Achievements;
using Saves;
using UnityEngine;

namespace Duckov.Endowment
{
	// Token: 0x02000306 RID: 774
	public class EndowmentManager : MonoBehaviour
	{
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x0005D339 File Offset: 0x0005B539
		// (set) Token: 0x06001966 RID: 6502 RVA: 0x0005D340 File Offset: 0x0005B540
		private static EndowmentManager _instance { get; set; }

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x0005D348 File Offset: 0x0005B548
		public static EndowmentManager Instance
		{
			get
			{
				if (EndowmentManager._instance == null)
				{
					GameManager instance = GameManager.Instance;
				}
				return EndowmentManager._instance;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x0005D362 File Offset: 0x0005B562
		// (set) Token: 0x06001969 RID: 6505 RVA: 0x0005D36E File Offset: 0x0005B56E
		public static EndowmentIndex SelectedIndex
		{
			get
			{
				return SavesSystem.Load<EndowmentIndex>("Endowment_SelectedIndex");
			}
			private set
			{
				SavesSystem.Save<EndowmentIndex>("Endowment_SelectedIndex", value);
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x0005D37B File Offset: 0x0005B57B
		public ReadOnlyCollection<EndowmentEntry> Entries
		{
			get
			{
				if (this._entries_ReadOnly == null)
				{
					this._entries_ReadOnly = new ReadOnlyCollection<EndowmentEntry>(this.entries);
				}
				return this._entries_ReadOnly;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x0600196B RID: 6507 RVA: 0x0005D39C File Offset: 0x0005B59C
		public static EndowmentEntry Current
		{
			get
			{
				if (EndowmentManager._instance == null)
				{
					return null;
				}
				return EndowmentManager._instance.entries.Find((EndowmentEntry e) => e != null && e.Index == EndowmentManager.SelectedIndex);
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x0600196C RID: 6508 RVA: 0x0005D3DB File Offset: 0x0005B5DB
		public static EndowmentIndex CurrentIndex
		{
			get
			{
				if (EndowmentManager.Current == null)
				{
					return EndowmentIndex.None;
				}
				return EndowmentManager.Current.Index;
			}
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0005D3F8 File Offset: 0x0005B5F8
		private EndowmentEntry GetEntry(EndowmentIndex index)
		{
			return this.entries.Find((EndowmentEntry e) => e != null && e.Index == index);
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0005D429 File Offset: 0x0005B629
		private static string GetUnlockKey(EndowmentIndex index)
		{
			return string.Format("Endowment_Unlock_R_{0}", index);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0005D43B File Offset: 0x0005B63B
		public static bool GetEndowmentUnlocked(EndowmentIndex index)
		{
			if (EndowmentManager.Instance != null)
			{
				if (EndowmentManager.Instance.GetEntry(index).UnlockedByDefault)
				{
					return true;
				}
			}
			else
			{
				Debug.LogError("Endowment Manager 不存在。");
			}
			return SavesSystem.LoadGlobal<bool>(EndowmentManager.GetUnlockKey(index), false);
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0005D474 File Offset: 0x0005B674
		private static void SetEndowmentUnlocked(EndowmentIndex index, bool value = true)
		{
			SavesSystem.SaveGlobal<bool>(EndowmentManager.GetUnlockKey(index), value);
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0005D484 File Offset: 0x0005B684
		public static bool UnlockEndowment(EndowmentIndex index)
		{
			try
			{
				Action<EndowmentIndex> onEndowmentUnlock = EndowmentManager.OnEndowmentUnlock;
				if (onEndowmentUnlock != null)
				{
					onEndowmentUnlock(index);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			if (EndowmentManager.GetEndowmentUnlocked(index))
			{
				Debug.Log("尝试解锁天赋，但天赋已经解锁");
				return false;
			}
			EndowmentManager.SetEndowmentUnlocked(index, true);
			return true;
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0005D4D8 File Offset: 0x0005B6D8
		private void Awake()
		{
			if (EndowmentManager._instance != null)
			{
				Debug.LogError("检测到多个Endowment Manager");
				return;
			}
			EndowmentManager._instance = this;
			if (LevelManager.LevelInited)
			{
				this.ApplyCurrentEndowment();
			}
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0005D516 File Offset: 0x0005B716
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0005D529 File Offset: 0x0005B729
		private void OnLevelInitialized()
		{
			this.ApplyCurrentEndowment();
			this.MakeSureEndowmentAchievementsUnlocked();
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0005D538 File Offset: 0x0005B738
		private void MakeSureEndowmentAchievementsUnlocked()
		{
			for (int i = 0; i < 5; i++)
			{
				EndowmentIndex index = (EndowmentIndex)i;
				EndowmentEntry entry = EndowmentManager.Instance.GetEntry(index);
				if (!(entry == null) && !entry.UnlockedByDefault && EndowmentManager.GetEndowmentUnlocked(index))
				{
					AchievementManager.UnlockEndowmentAchievement(index);
				}
			}
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0005D580 File Offset: 0x0005B780
		private void ApplyCurrentEndowment()
		{
			if (!LevelManager.LevelInited)
			{
				return;
			}
			foreach (EndowmentEntry endowmentEntry in this.entries)
			{
				if (!(endowmentEntry == null))
				{
					endowmentEntry.Deactivate();
				}
			}
			EndowmentEntry endowmentEntry2 = EndowmentManager.Current;
			if (endowmentEntry2 == null)
			{
				return;
			}
			endowmentEntry2.Activate();
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x0005D5FC File Offset: 0x0005B7FC
		internal void SelectIndex(EndowmentIndex index)
		{
			EndowmentManager.SelectedIndex = index;
			this.ApplyCurrentEndowment();
			Action<EndowmentIndex> onEndowmentChanged = EndowmentManager.OnEndowmentChanged;
			if (onEndowmentChanged == null)
			{
				return;
			}
			onEndowmentChanged(index);
		}

		// Token: 0x04001284 RID: 4740
		private const string SaveKey = "Endowment_SelectedIndex";

		// Token: 0x04001285 RID: 4741
		public static Action<EndowmentIndex> OnEndowmentChanged;

		// Token: 0x04001286 RID: 4742
		public static Action<EndowmentIndex> OnEndowmentUnlock;

		// Token: 0x04001287 RID: 4743
		[SerializeField]
		private List<EndowmentEntry> entries = new List<EndowmentEntry>();

		// Token: 0x04001288 RID: 4744
		private ReadOnlyCollection<EndowmentEntry> _entries_ReadOnly;
	}
}
