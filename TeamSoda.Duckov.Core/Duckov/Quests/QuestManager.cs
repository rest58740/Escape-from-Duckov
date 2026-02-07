using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Quests.Relations;
using Duckov.Quests.Tasks;
using Duckov.UI;
using Duckov.Utilities;
using Saves;
using Sirenix.Utilities;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200034E RID: 846
	public class QuestManager : MonoBehaviour, ISaveDataProvider, INeedInspection
	{
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x00069B0A File Offset: 0x00067D0A
		public string TaskFinishNotificationFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.taskFinishNotificationFormatKey);
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x00069B17 File Offset: 0x00067D17
		public static QuestManager Instance
		{
			get
			{
				return QuestManager.instance;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x00069B1E File Offset: 0x00067D1E
		public static bool AnyQuestNeedsInspection
		{
			get
			{
				return !(QuestManager.Instance == null) && QuestManager.Instance.NeedInspection;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x00069B39 File Offset: 0x00067D39
		public bool NeedInspection
		{
			get
			{
				if (this.activeQuests == null)
				{
					return false;
				}
				return this.activeQuests.Any((Quest e) => e != null && e.NeedInspection);
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x00069B6F File Offset: 0x00067D6F
		public static IEnumerable<int> GetAllRequiredItems()
		{
			if (QuestManager.Instance == null)
			{
				yield break;
			}
			List<Quest> list = QuestManager.Instance.ActiveQuests;
			foreach (Quest quest in list)
			{
				if (quest.tasks != null)
				{
					foreach (Task task in quest.tasks)
					{
						SubmitItems submitItems = task as SubmitItems;
						if (submitItems != null && !submitItems.IsFinished())
						{
							yield return submitItems.ItemTypeID;
						}
					}
					List<Task>.Enumerator enumerator2 = default(List<Task>.Enumerator);
				}
			}
			List<Quest>.Enumerator enumerator = default(List<Quest>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x00069B78 File Offset: 0x00067D78
		public static bool AnyActiveQuestNeedsInspection(QuestGiverID giverID)
		{
			return !(QuestManager.Instance == null) && QuestManager.Instance.activeQuests != null && QuestManager.Instance.activeQuests.Any((Quest e) => e != null && e.QuestGiverID == giverID && e.NeedInspection);
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x00069BCA File Offset: 0x00067DCA
		private ICollection<Quest> QuestPrefabCollection
		{
			get
			{
				return GameplayDataSettings.QuestCollection;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001D0F RID: 7439 RVA: 0x00069BD1 File Offset: 0x00067DD1
		private QuestRelationGraph QuestRelation
		{
			get
			{
				return GameplayDataSettings.QuestRelation;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001D10 RID: 7440 RVA: 0x00069BD8 File Offset: 0x00067DD8
		public List<Quest> ActiveQuests
		{
			get
			{
				this.activeQuests.Sort(delegate(Quest a, Quest b)
				{
					int num = a.AreTasksFinished() ? 1 : 0;
					return (b.AreTasksFinished() ? 1 : 0) - num;
				});
				return this.activeQuests;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x00069C0A File Offset: 0x00067E0A
		public List<Quest> HistoryQuests
		{
			get
			{
				return this.historyQuests;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001D12 RID: 7442 RVA: 0x00069C12 File Offset: 0x00067E12
		public List<int> EverInspectedQuest
		{
			get
			{
				return this.everInspectedQuest;
			}
		}

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06001D13 RID: 7443 RVA: 0x00069C1C File Offset: 0x00067E1C
		// (remove) Token: 0x06001D14 RID: 7444 RVA: 0x00069C50 File Offset: 0x00067E50
		public static event Action<QuestManager> onQuestListsChanged;

		// Token: 0x06001D15 RID: 7445 RVA: 0x00069C84 File Offset: 0x00067E84
		public object GenerateSaveData()
		{
			QuestManager.SaveData saveData = default(QuestManager.SaveData);
			saveData.activeQuestsData = new List<object>();
			saveData.historyQuestsData = new List<object>();
			saveData.everInspectedQuest = new List<int>();
			foreach (Quest quest in this.ActiveQuests)
			{
				saveData.activeQuestsData.Add(quest.GenerateSaveData());
			}
			foreach (Quest quest2 in this.HistoryQuests)
			{
				saveData.historyQuestsData.Add(quest2.GenerateSaveData());
			}
			saveData.everInspectedQuest.AddRange(this.EverInspectedQuest);
			return saveData;
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x00069D70 File Offset: 0x00067F70
		public void SetupSaveData(object dataObj)
		{
			if (dataObj is QuestManager.SaveData)
			{
				QuestManager.SaveData saveData = (QuestManager.SaveData)dataObj;
				if (saveData.activeQuestsData != null)
				{
					foreach (object obj in saveData.activeQuestsData)
					{
						int id = ((Quest.SaveData)obj).id;
						Quest questPrefab = this.GetQuestPrefab(id);
						if (questPrefab == null)
						{
							Debug.LogError(string.Format("未找到Quest {0}", id));
						}
						else
						{
							Quest quest = Object.Instantiate<Quest>(questPrefab, base.transform);
							quest.SetupSaveData(obj);
							this.activeQuests.Add(quest);
						}
					}
				}
				if (saveData.historyQuestsData != null)
				{
					foreach (object obj2 in saveData.historyQuestsData)
					{
						int id2 = ((Quest.SaveData)obj2).id;
						Quest questPrefab2 = this.GetQuestPrefab(id2);
						if (questPrefab2 == null)
						{
							Debug.LogError(string.Format("未找到Quest {0}", id2));
						}
						else
						{
							Quest quest2 = Object.Instantiate<Quest>(questPrefab2, base.transform);
							quest2.SetupSaveData(obj2);
							this.historyQuests.Add(quest2);
						}
					}
				}
				if (saveData.everInspectedQuest != null)
				{
					this.EverInspectedQuest.Clear();
					this.EverInspectedQuest.AddRange(saveData.everInspectedQuest);
				}
				return;
			}
			Debug.LogError("错误的数据类型");
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x00069F08 File Offset: 0x00068108
		private void Save()
		{
			SavesSystem.Save<object>("Quest", "Data", this.GenerateSaveData());
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x00069F20 File Offset: 0x00068120
		private void Load()
		{
			try
			{
				QuestManager.SaveData saveData = SavesSystem.Load<QuestManager.SaveData>("Quest", "Data");
				this.SetupSaveData(saveData);
			}
			catch
			{
				Debug.LogError("在加载Quest存档时出现了错误");
			}
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x00069F68 File Offset: 0x00068168
		public IEnumerable<Quest> GetAllQuestsByQuestGiverID(QuestGiverID questGiverID)
		{
			return from e in this.QuestPrefabCollection
			where e != null && e.QuestGiverID == questGiverID
			select e;
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x00069F9C File Offset: 0x0006819C
		private Quest GetQuestPrefab(int id)
		{
			return this.QuestPrefabCollection.FirstOrDefault((Quest q) => q != null && q.ID == id);
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x00069FCD File Offset: 0x000681CD
		private void Awake()
		{
			if (QuestManager.instance == null)
			{
				QuestManager.instance = this;
			}
			if (QuestManager.instance != this)
			{
				Debug.LogError("侦测到多个QuestManager！");
				return;
			}
			this.RegisterEvents();
			this.Load();
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x0006A006 File Offset: 0x00068206
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x0006A010 File Offset: 0x00068210
		private void RegisterEvents()
		{
			Quest.onQuestStatusChanged += this.OnQuestStatusChanged;
			Quest.onQuestCompleted += this.OnQuestCompleted;
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x0006A064 File Offset: 0x00068264
		private void UnregisterEvents()
		{
			Quest.onQuestStatusChanged -= this.OnQuestStatusChanged;
			Quest.onQuestCompleted -= this.OnQuestCompleted;
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x0006A0B8 File Offset: 0x000682B8
		private void OnQuestCompleted(Quest quest)
		{
			if (!this.activeQuests.Remove(quest))
			{
				Debug.LogWarning(quest.DisplayName + " 并不存在于活跃任务表中。已终止操作。");
				return;
			}
			this.historyQuests.Add(quest);
			Action<QuestManager> action = QuestManager.onQuestListsChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x0006A105 File Offset: 0x00068305
		private void OnQuestStatusChanged(Quest quest)
		{
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x0006A108 File Offset: 0x00068308
		public void ActivateQuest(int id, QuestGiverID? overrideQuestGiverID = null)
		{
			Quest quest = Object.Instantiate<Quest>(this.GetQuestPrefab(id), base.transform);
			if (overrideQuestGiverID != null)
			{
				quest.QuestGiverID = overrideQuestGiverID.Value;
			}
			this.activeQuests.Add(quest);
			quest.NotifyActivated();
			Action<QuestManager> action = QuestManager.onQuestListsChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x0006A160 File Offset: 0x00068360
		internal static bool IsQuestAvaliable(int id)
		{
			return !(QuestManager.Instance == null) && !QuestManager.IsQuestFinished(id) && !QuestManager.instance.activeQuests.Any((Quest e) => e.ID == id) && QuestManager.Instance.GetQuestPrefab(id).MeetsPrerequisit();
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x0006A1CC File Offset: 0x000683CC
		internal static bool IsQuestFinished(int id)
		{
			return !(QuestManager.instance == null) && QuestManager.instance.historyQuests.Any((Quest e) => e.ID == id);
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x0006A210 File Offset: 0x00068410
		internal static bool AreQuestFinished(IEnumerable<int> requiredQuestIDs)
		{
			if (QuestManager.instance == null)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			LinqExtensions.AddRange<int>(hashSet, requiredQuestIDs);
			foreach (Quest quest in QuestManager.instance.historyQuests)
			{
				hashSet.Remove(quest.ID);
			}
			return hashSet.Count <= 0;
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x0006A298 File Offset: 0x00068498
		internal static List<Quest> GetActiveQuestsFromGiver(QuestGiverID giverID)
		{
			List<Quest> result = new List<Quest>();
			if (QuestManager.instance == null)
			{
				return result;
			}
			return (from e in QuestManager.instance.ActiveQuests
			where e.QuestGiverID == giverID
			select e).ToList<Quest>();
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x0006A2EC File Offset: 0x000684EC
		internal static List<Quest> GetHistoryQuestsFromGiver(QuestGiverID giverID)
		{
			List<Quest> result = new List<Quest>();
			if (QuestManager.instance == null)
			{
				return result;
			}
			return (from e in QuestManager.instance.historyQuests
			where e != null && e.QuestGiverID == giverID
			select e).ToList<Quest>();
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x0006A33B File Offset: 0x0006853B
		internal static bool IsQuestActive(Quest quest)
		{
			return !(QuestManager.instance == null) && QuestManager.instance.activeQuests.Contains(quest);
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x0006A35C File Offset: 0x0006855C
		internal static bool IsQuestActive(int questID)
		{
			return !(QuestManager.instance == null) && QuestManager.instance.activeQuests.Any((Quest e) => e.ID == questID);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0006A3A8 File Offset: 0x000685A8
		internal static bool AreQuestsActive(IEnumerable<int> requiredQuestIDs)
		{
			if (QuestManager.instance == null)
			{
				return false;
			}
			using (IEnumerator<int> enumerator = requiredQuestIDs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int id = enumerator.Current;
					if (!QuestManager.instance.activeQuests.Any((Quest e) => e.ID == id))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x0006A428 File Offset: 0x00068628
		private void OnTaskFinished(Quest quest, Task task)
		{
			NotificationText.Push(StringExtensions.Format(this.TaskFinishNotificationFormat, new
			{
				questDisplayName = quest.DisplayName,
				finishedTasks = quest.FinishedTaskCount,
				totalTasks = quest.tasks.Count
			}));
			Action<Quest, Task> onTaskFinishedEvent = QuestManager.OnTaskFinishedEvent;
			if (onTaskFinishedEvent != null)
			{
				onTaskFinishedEvent(quest, task);
			}
			AudioManager.Post("UI/mission_small");
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x0006A47E File Offset: 0x0006867E
		internal static void NotifyTaskFinished(Quest quest, Task task)
		{
			QuestManager questManager = QuestManager.instance;
			if (questManager == null)
			{
				return;
			}
			questManager.OnTaskFinished(quest, task);
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x0006A491 File Offset: 0x00068691
		internal static bool EverInspected(int id)
		{
			return !(QuestManager.Instance == null) && QuestManager.Instance.EverInspectedQuest.Contains(id);
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x0006A4B2 File Offset: 0x000686B2
		internal static void SetEverInspected(int id)
		{
			if (QuestManager.EverInspected(id))
			{
				return;
			}
			if (QuestManager.Instance == null)
			{
				return;
			}
			QuestManager.Instance.EverInspectedQuest.Add(id);
		}

		// Token: 0x0400146B RID: 5227
		[SerializeField]
		private string taskFinishNotificationFormatKey = "UI_Quest_TaskFinishedNotification";

		// Token: 0x0400146C RID: 5228
		private static QuestManager instance;

		// Token: 0x0400146D RID: 5229
		public static Action<Quest, Task> OnTaskFinishedEvent;

		// Token: 0x0400146E RID: 5230
		private List<Quest> activeQuests = new List<Quest>();

		// Token: 0x0400146F RID: 5231
		private List<Quest> historyQuests = new List<Quest>();

		// Token: 0x04001470 RID: 5232
		private List<int> everInspectedQuest = new List<int>();

		// Token: 0x04001472 RID: 5234
		private const string savePrefix = "Quest";

		// Token: 0x04001473 RID: 5235
		private const string saveKey = "Data";

		// Token: 0x02000612 RID: 1554
		[Serializable]
		public struct SaveData
		{
			// Token: 0x04002202 RID: 8706
			public List<object> activeQuestsData;

			// Token: 0x04002203 RID: 8707
			public List<object> historyQuestsData;

			// Token: 0x04002204 RID: 8708
			public List<int> everInspectedQuest;
		}
	}
}
