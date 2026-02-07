using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Duckov.Quests.Relations;
using Duckov.Scenes;
using Duckov.Utilities;
using Eflatun.SceneReference;
using ItemStatsSystem;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Duckov.Quests
{
	// Token: 0x0200034A RID: 842
	public class Quest : MonoBehaviour, ISaveDataProvider, INeedInspection
	{
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x0006879C File Offset: 0x0006699C
		public SceneInfoEntry RequireSceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.requireSceneID);
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001CAA RID: 7338 RVA: 0x000687AC File Offset: 0x000669AC
		public SceneReference RequireScene
		{
			get
			{
				SceneInfoEntry requireSceneInfo = this.RequireSceneInfo;
				if (requireSceneInfo == null)
				{
					return null;
				}
				return requireSceneInfo.SceneReference;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001CAB RID: 7339 RVA: 0x000687CB File Offset: 0x000669CB
		public List<Task> Tasks
		{
			get
			{
				return this.tasks;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001CAC RID: 7340 RVA: 0x000687D3 File Offset: 0x000669D3
		public ReadOnlyCollection<Reward> Rewards
		{
			get
			{
				if (this._readonly_rewards == null)
				{
					this._readonly_rewards = new ReadOnlyCollection<Reward>(this.rewards);
				}
				return this._readonly_rewards;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x000687F4 File Offset: 0x000669F4
		public ReadOnlyCollection<Condition> Prerequisits
		{
			get
			{
				if (this.prerequisits_ReadOnly == null)
				{
					this.prerequisits_ReadOnly = new ReadOnlyCollection<Condition>(this.prerequisit);
				}
				return this.prerequisits_ReadOnly;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x00068818 File Offset: 0x00066A18
		public bool SceneRequirementSatisfied
		{
			get
			{
				SceneReference requireScene = this.RequireScene;
				return requireScene == null || requireScene.UnsafeReason == 1 || requireScene.UnsafeReason != null || requireScene.LoadedScene.isLoaded;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001CAF RID: 7343 RVA: 0x00068854 File Offset: 0x00066A54
		public int RequireLevel
		{
			get
			{
				return this.requireLevel;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x0006885C File Offset: 0x00066A5C
		public bool LockInDemo
		{
			get
			{
				return this.lockInDemo;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x00068864 File Offset: 0x00066A64
		// (set) Token: 0x06001CB2 RID: 7346 RVA: 0x0006886C File Offset: 0x00066A6C
		public bool Complete
		{
			get
			{
				return this.complete;
			}
			internal set
			{
				this.complete = value;
				Action<Quest> action = this.onStatusChanged;
				if (action != null)
				{
					action(this);
				}
				Action<Quest> action2 = Quest.onQuestStatusChanged;
				if (action2 != null)
				{
					action2(this);
				}
				if (this.complete)
				{
					Action<Quest> action3 = this.onCompleted;
					if (action3 != null)
					{
						action3(this);
					}
					UnityEvent onCompletedUnityEvent = this.OnCompletedUnityEvent;
					if (onCompletedUnityEvent != null)
					{
						onCompletedUnityEvent.Invoke();
					}
					Action<Quest> action4 = Quest.onQuestCompleted;
					if (action4 == null)
					{
						return;
					}
					action4(this);
				}
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x000688E0 File Offset: 0x00066AE0
		// (set) Token: 0x06001CB4 RID: 7348 RVA: 0x00068930 File Offset: 0x00066B30
		public bool NeedInspection
		{
			get
			{
				return (!this.Active && !QuestManager.EverInspected(this.ID)) || (this.Active && ((this.Active && this.AreTasksFinished()) || this.AnyTaskNeedInspection() || this.needInspection));
			}
			set
			{
				this.needInspection = value;
				Action<Quest> action = this.onNeedInspectionChanged;
				if (action != null)
				{
					action(this);
				}
				Action<Quest> action2 = Quest.onQuestNeedInspectionChanged;
				if (action2 == null)
				{
					return;
				}
				action2(this);
			}
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0006895C File Offset: 0x00066B5C
		private bool AnyTaskNeedInspection()
		{
			if (this.tasks != null)
			{
				foreach (Task task in this.tasks)
				{
					if (!(task == null) && task.NeedInspection)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06001CB6 RID: 7350 RVA: 0x000689C8 File Offset: 0x00066BC8
		// (remove) Token: 0x06001CB7 RID: 7351 RVA: 0x00068A00 File Offset: 0x00066C00
		public event Action<Quest> onNeedInspectionChanged;

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06001CB8 RID: 7352 RVA: 0x00068A38 File Offset: 0x00066C38
		// (remove) Token: 0x06001CB9 RID: 7353 RVA: 0x00068A70 File Offset: 0x00066C70
		internal event Action<Quest> onStatusChanged;

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x06001CBA RID: 7354 RVA: 0x00068AA8 File Offset: 0x00066CA8
		// (remove) Token: 0x06001CBB RID: 7355 RVA: 0x00068AE0 File Offset: 0x00066CE0
		internal event Action<Quest> onActivated;

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x06001CBC RID: 7356 RVA: 0x00068B18 File Offset: 0x00066D18
		// (remove) Token: 0x06001CBD RID: 7357 RVA: 0x00068B50 File Offset: 0x00066D50
		internal event Action<Quest> onCompleted;

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06001CBE RID: 7358 RVA: 0x00068B88 File Offset: 0x00066D88
		// (remove) Token: 0x06001CBF RID: 7359 RVA: 0x00068BBC File Offset: 0x00066DBC
		public static event Action<Quest> onQuestStatusChanged;

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x06001CC0 RID: 7360 RVA: 0x00068BF0 File Offset: 0x00066DF0
		// (remove) Token: 0x06001CC1 RID: 7361 RVA: 0x00068C24 File Offset: 0x00066E24
		public static event Action<Quest> onQuestNeedInspectionChanged;

		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x06001CC2 RID: 7362 RVA: 0x00068C58 File Offset: 0x00066E58
		// (remove) Token: 0x06001CC3 RID: 7363 RVA: 0x00068C8C File Offset: 0x00066E8C
		public static event Action<Quest> onQuestActivated;

		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06001CC4 RID: 7364 RVA: 0x00068CC0 File Offset: 0x00066EC0
		// (remove) Token: 0x06001CC5 RID: 7365 RVA: 0x00068CF4 File Offset: 0x00066EF4
		public static event Action<Quest> onQuestCompleted;

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x00068D27 File Offset: 0x00066F27
		// (set) Token: 0x06001CC7 RID: 7367 RVA: 0x00068D2F File Offset: 0x00066F2F
		public int ID
		{
			get
			{
				return this.id;
			}
			internal set
			{
				this.id = value;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x00068D38 File Offset: 0x00066F38
		public bool Active
		{
			get
			{
				return QuestManager.IsQuestActive(this);
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x00068D40 File Offset: 0x00066F40
		public int RequiredItemID
		{
			get
			{
				return this.requiredItemID;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x00068D48 File Offset: 0x00066F48
		public int RequiredItemCount
		{
			get
			{
				return this.requiredItemCount;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x00068D50 File Offset: 0x00066F50
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayName);
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x00068D5D File Offset: 0x00066F5D
		public string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.description);
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x00068D6A File Offset: 0x00066F6A
		// (set) Token: 0x06001CCE RID: 7374 RVA: 0x00068D72 File Offset: 0x00066F72
		public string DisplayNameRaw
		{
			get
			{
				return this.displayName;
			}
			set
			{
				this.displayName = value;
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x00068D7B File Offset: 0x00066F7B
		// (set) Token: 0x06001CD0 RID: 7376 RVA: 0x00068D83 File Offset: 0x00066F83
		public string DescriptionRaw
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x00068D8C File Offset: 0x00066F8C
		// (set) Token: 0x06001CD2 RID: 7378 RVA: 0x00068D94 File Offset: 0x00066F94
		public QuestGiverID QuestGiverID
		{
			get
			{
				return this.questGiverID;
			}
			internal set
			{
				this.questGiverID = value;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x00068D9D File Offset: 0x00066F9D
		public object FinishedTaskCount
		{
			get
			{
				return this.tasks.Count((Task e) => e.IsFinished());
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x00068DD0 File Offset: 0x00066FD0
		public bool MeetsPrerequisit()
		{
			if (this.RequireLevel > EXPManager.Level)
			{
				return false;
			}
			if (this.LockInDemo && GameMetaData.Instance.IsDemo)
			{
				return false;
			}
			QuestRelationGraph questRelation = GameplayDataSettings.QuestRelation;
			if (questRelation.GetNode(this.id) == null)
			{
				return false;
			}
			if (!QuestManager.AreQuestFinished(questRelation.GetRequiredIDs(this.id)))
			{
				return false;
			}
			using (List<Condition>.Enumerator enumerator = this.prerequisit.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Evaluate())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x00068E7C File Offset: 0x0006707C
		public bool AreTasksFinished()
		{
			foreach (Task task in this.tasks)
			{
				if (task == null)
				{
					Debug.LogError(string.Format("存在空的Task，QuestID：{0}", this.id));
				}
				else if (!task.IsFinished())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x00068EFC File Offset: 0x000670FC
		public void Initialize()
		{
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x00068EFE File Offset: 0x000670FE
		public void OnValidate()
		{
			this.displayName = string.Format("Quest_{0}", this.id);
			this.description = string.Format("Quest_{0}_Desc", this.id);
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x00068F38 File Offset: 0x00067138
		public object GenerateSaveData()
		{
			Quest.SaveData saveData = default(Quest.SaveData);
			saveData.id = this.id;
			saveData.complete = this.complete;
			saveData.needInspection = this.needInspection;
			saveData.taskStatus = new List<ValueTuple<int, object>>();
			saveData.rewardStatus = new List<ValueTuple<int, object>>();
			foreach (Task task in this.tasks)
			{
				int item = task.ID;
				object item2 = task.GenerateSaveData();
				if (!(task == null))
				{
					saveData.taskStatus.Add(new ValueTuple<int, object>(item, item2));
				}
			}
			foreach (Reward reward in this.rewards)
			{
				if (reward == null)
				{
					Debug.LogError(string.Format("Null Reward detected in quest {0}", this.id));
				}
				else
				{
					int item3 = reward.ID;
					object item4 = reward.GenerateSaveData();
					saveData.rewardStatus.Add(new ValueTuple<int, object>(item3, item4));
				}
			}
			return saveData;
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x00069084 File Offset: 0x00067284
		public void SetupSaveData(object obj)
		{
			Quest.SaveData saveData = (Quest.SaveData)obj;
			if (saveData.id != this.id)
			{
				Debug.LogError("任务ID不匹配，加载失败");
				return;
			}
			this.complete = saveData.complete;
			this.needInspection = saveData.needInspection;
			using (List<ValueTuple<int, object>>.Enumerator enumerator = saveData.taskStatus.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ValueTuple<int, object> cur = enumerator.Current;
					Task task = this.tasks.Find((Task e) => e.ID == cur.Item1);
					if (task == null)
					{
						Debug.LogWarning(string.Format("未找到Task {0}", cur.Item1));
					}
					else
					{
						task.SetupSaveData(cur.Item2);
					}
				}
			}
			using (List<ValueTuple<int, object>>.Enumerator enumerator = saveData.rewardStatus.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ValueTuple<int, object> cur = enumerator.Current;
					Reward reward = this.rewards.Find((Reward e) => e.ID == cur.Item1);
					if (reward == null)
					{
						Debug.LogWarning(string.Format("未找到Reward {0}", cur.Item1));
					}
					else
					{
						reward.SetupSaveData(cur.Item2);
						reward.NotifyReload(this);
					}
				}
			}
			this.InitTasks();
			if (this.complete)
			{
				foreach (Reward reward2 in this.rewards)
				{
					if (!(reward2 == null) && !reward2.Claimed && reward2.AutoClaim)
					{
						reward2.Claim();
					}
				}
			}
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x00069280 File Offset: 0x00067480
		internal void NotifyTaskFinished(Task task)
		{
			if (task.Master != this)
			{
				Debug.LogError("Task.Master 与 Quest不匹配");
				return;
			}
			Action<Quest> action = Quest.onQuestStatusChanged;
			if (action != null)
			{
				action(this);
			}
			Action<Quest> action2 = this.onStatusChanged;
			if (action2 != null)
			{
				action2(this);
			}
			QuestManager.NotifyTaskFinished(this, task);
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000692D0 File Offset: 0x000674D0
		internal void NotifyRewardClaimed(Reward reward)
		{
			if (reward.Master != this)
			{
				Debug.LogError("Reward.Master 与Quest 不匹配");
			}
			if (this.AreRewardsClaimed())
			{
				this.needInspection = false;
			}
			Action<Quest> action = Quest.onQuestStatusChanged;
			if (action != null)
			{
				action(this);
			}
			Action<Quest> action2 = this.onStatusChanged;
			if (action2 != null)
			{
				action2(this);
			}
			Action<Quest> action3 = Quest.onQuestNeedInspectionChanged;
			if (action3 == null)
			{
				return;
			}
			action3(this);
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x00069338 File Offset: 0x00067538
		internal bool AreRewardsClaimed()
		{
			using (List<Reward>.Enumerator enumerator = this.rewards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Claimed)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x00069394 File Offset: 0x00067594
		internal void NotifyActivated()
		{
			this.InitTasks();
			Action<Quest> action = this.onStatusChanged;
			if (action != null)
			{
				action(this);
			}
			Action<Quest> action2 = this.onActivated;
			if (action2 != null)
			{
				action2(this);
			}
			Action<Quest> action3 = Quest.onQuestActivated;
			if (action3 == null)
			{
				return;
			}
			action3(this);
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000693D0 File Offset: 0x000675D0
		private void InitTasks()
		{
			foreach (Task task in this.tasks)
			{
				task.Init();
			}
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x00069420 File Offset: 0x00067620
		public bool TryComplete()
		{
			if (this.Complete)
			{
				return false;
			}
			if (!this.AreTasksFinished())
			{
				return false;
			}
			this.Complete = true;
			return true;
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x0006943E File Offset: 0x0006763E
		internal Quest.QuestInfo GetInfo()
		{
			return new Quest.QuestInfo(this);
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x00069448 File Offset: 0x00067648
		public static int Compare(Quest x, Quest y, Quest.SortingMode sortingMode, bool invert = false)
		{
			int num = 0;
			switch (sortingMode)
			{
			case Quest.SortingMode.ID:
				num = x.ID - y.ID;
				break;
			case Quest.SortingMode.Giver:
				num = x.QuestGiverID - y.QuestGiverID;
				break;
			case Quest.SortingMode.Location:
			{
				SceneInfoEntry requireSceneInfo = x.RequireSceneInfo;
				string x2 = (requireSceneInfo == null) ? "" : requireSceneInfo.ID;
				SceneInfoEntry requireSceneInfo2 = y.RequireSceneInfo;
				string y2 = (requireSceneInfo2 == null) ? "" : requireSceneInfo2.ID;
				num = StringComparer.CurrentCulture.Compare(x2, y2);
				break;
			}
			}
			if (invert)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x04001440 RID: 5184
		[SerializeField]
		private int id;

		// Token: 0x04001441 RID: 5185
		[LocalizationKey("Quests")]
		[SerializeField]
		private string displayName;

		// Token: 0x04001442 RID: 5186
		[LocalizationKey("Quests")]
		[SerializeField]
		private string description;

		// Token: 0x04001443 RID: 5187
		[SerializeField]
		private int requireLevel;

		// Token: 0x04001444 RID: 5188
		[SerializeField]
		private bool lockInDemo;

		// Token: 0x04001445 RID: 5189
		[FormerlySerializedAs("requiredItem")]
		[SerializeField]
		[ItemTypeID]
		private int requiredItemID;

		// Token: 0x04001446 RID: 5190
		[SerializeField]
		private int requiredItemCount = 1;

		// Token: 0x04001447 RID: 5191
		[SceneID]
		[SerializeField]
		private string requireSceneID;

		// Token: 0x04001448 RID: 5192
		[SerializeField]
		private QuestGiverID questGiverID;

		// Token: 0x04001449 RID: 5193
		[SerializeField]
		internal List<Condition> prerequisit = new List<Condition>();

		// Token: 0x0400144A RID: 5194
		[SerializeField]
		internal List<Task> tasks = new List<Task>();

		// Token: 0x0400144B RID: 5195
		[SerializeField]
		internal List<Reward> rewards = new List<Reward>();

		// Token: 0x0400144C RID: 5196
		private ReadOnlyCollection<Reward> _readonly_rewards;

		// Token: 0x0400144D RID: 5197
		[SerializeField]
		[HideInInspector]
		private int nextTaskID;

		// Token: 0x0400144E RID: 5198
		[SerializeField]
		[HideInInspector]
		private int nextRewardID;

		// Token: 0x0400144F RID: 5199
		private ReadOnlyCollection<Condition> prerequisits_ReadOnly;

		// Token: 0x04001450 RID: 5200
		[SerializeField]
		private bool complete;

		// Token: 0x04001451 RID: 5201
		[SerializeField]
		private bool needInspection;

		// Token: 0x04001456 RID: 5206
		public UnityEvent OnCompletedUnityEvent;

		// Token: 0x02000609 RID: 1545
		[Serializable]
		public struct SaveData
		{
			// Token: 0x040021E8 RID: 8680
			public int id;

			// Token: 0x040021E9 RID: 8681
			public bool complete;

			// Token: 0x040021EA RID: 8682
			public bool needInspection;

			// Token: 0x040021EB RID: 8683
			public QuestGiverID questGiverID;

			// Token: 0x040021EC RID: 8684
			[TupleElementNames(new string[]
			{
				"id",
				"data"
			})]
			public List<ValueTuple<int, object>> taskStatus;

			// Token: 0x040021ED RID: 8685
			[TupleElementNames(new string[]
			{
				"id",
				"data"
			})]
			public List<ValueTuple<int, object>> rewardStatus;
		}

		// Token: 0x0200060A RID: 1546
		public struct QuestInfo
		{
			// Token: 0x06002A8B RID: 10891 RVA: 0x0009E877 File Offset: 0x0009CA77
			public QuestInfo(Quest quest)
			{
				this.questId = quest.id;
			}

			// Token: 0x040021EE RID: 8686
			public int questId;
		}

		// Token: 0x0200060B RID: 1547
		public enum SortingMode
		{
			// Token: 0x040021F0 RID: 8688
			Default,
			// Token: 0x040021F1 RID: 8689
			ID,
			// Token: 0x040021F2 RID: 8690
			Giver,
			// Token: 0x040021F3 RID: 8691
			Location
		}
	}
}
