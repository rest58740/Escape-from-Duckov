using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;

namespace Duckov.Quests.UI
{
	// Token: 0x02000364 RID: 868
	public class QuestViewDetails : MonoBehaviour
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001E2F RID: 7727 RVA: 0x0006CA84 File Offset: 0x0006AC84
		public Quest Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001E30 RID: 7728 RVA: 0x0006CA8C File Offset: 0x0006AC8C
		// (set) Token: 0x06001E31 RID: 7729 RVA: 0x0006CA94 File Offset: 0x0006AC94
		public bool Interactable
		{
			get
			{
				return this.interactable;
			}
			internal set
			{
				this.interactable = value;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x0006CAA0 File Offset: 0x0006ACA0
		private PrefabPool<TaskEntry> TaskEntryPool
		{
			get
			{
				if (this._taskEntryPool == null)
				{
					this._taskEntryPool = new PrefabPool<TaskEntry>(this.taskEntryPrefab, this.tasksParent, null, null, null, true, 10, 10000, null);
				}
				return this._taskEntryPool;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001E33 RID: 7731 RVA: 0x0006CAE0 File Offset: 0x0006ACE0
		private PrefabPool<RewardEntry> RewardEntryPool
		{
			get
			{
				if (this._rewardEntryPool == null)
				{
					this._rewardEntryPool = new PrefabPool<RewardEntry>(this.rewardEntry, this.rewardsParent, null, null, null, true, 10, 10000, null);
				}
				return this._rewardEntryPool;
			}
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x0006CB1E File Offset: 0x0006AD1E
		private void Awake()
		{
			this.rewardEntry.gameObject.SetActive(false);
			this.taskEntryPrefab.gameObject.SetActive(false);
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x0006CB42 File Offset: 0x0006AD42
		internal void Refresh()
		{
			this.RefreshAsync().Forget();
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x0006CB50 File Offset: 0x0006AD50
		private int GetNewToken()
		{
			int num;
			for (num = this.activeTaskToken; num == this.activeTaskToken; num = Random.Range(1, int.MaxValue))
			{
			}
			this.activeTaskToken = num;
			return this.activeTaskToken;
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x0006CB88 File Offset: 0x0006AD88
		private UniTask RefreshAsync()
		{
			QuestViewDetails.<RefreshAsync>d__28 <RefreshAsync>d__;
			<RefreshAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RefreshAsync>d__.<>4__this = this;
			<RefreshAsync>d__.<>1__state = -1;
			<RefreshAsync>d__.<>t__builder.Start<QuestViewDetails.<RefreshAsync>d__28>(ref <RefreshAsync>d__);
			return <RefreshAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x0006CBCC File Offset: 0x0006ADCC
		private void SetupTasks()
		{
			this.TaskEntryPool.ReleaseAll();
			if (this.target == null)
			{
				return;
			}
			foreach (Task task in this.target.tasks)
			{
				TaskEntry taskEntry = this.TaskEntryPool.Get(this.tasksParent);
				taskEntry.Interactable = this.Interactable;
				taskEntry.Setup(task);
				taskEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0006CC68 File Offset: 0x0006AE68
		private void SetupRewards()
		{
			this.RewardEntryPool.ReleaseAll();
			if (this.target == null)
			{
				return;
			}
			foreach (Reward reward in this.target.rewards)
			{
				if (reward == null)
				{
					Debug.LogError(string.Format("任务 {0} - {1} 中包含值为 null 的奖励。", this.target.ID, this.target.DisplayName));
				}
				else
				{
					RewardEntry rewardEntry = this.RewardEntryPool.Get(this.rewardsParent);
					rewardEntry.Interactable = this.Interactable;
					rewardEntry.Setup(reward);
					rewardEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0006CD38 File Offset: 0x0006AF38
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged += this.OnTargetStatusChanged;
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0006CD60 File Offset: 0x0006AF60
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged -= this.OnTargetStatusChanged;
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x0006CD88 File Offset: 0x0006AF88
		private void OnTargetStatusChanged(Quest quest)
		{
			this.Refresh();
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x0006CD90 File Offset: 0x0006AF90
		internal void Setup(Quest quest)
		{
			this.target = quest;
			this.Refresh();
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x0006CD9F File Offset: 0x0006AF9F
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x040014F0 RID: 5360
		private Quest target;

		// Token: 0x040014F1 RID: 5361
		[SerializeField]
		private TaskEntry taskEntryPrefab;

		// Token: 0x040014F2 RID: 5362
		[SerializeField]
		private RewardEntry rewardEntry;

		// Token: 0x040014F3 RID: 5363
		[SerializeField]
		private GameObject placeHolder;

		// Token: 0x040014F4 RID: 5364
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x040014F5 RID: 5365
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x040014F6 RID: 5366
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x040014F7 RID: 5367
		[SerializeField]
		private TextMeshProUGUI questGiverDisplayName;

		// Token: 0x040014F8 RID: 5368
		[SerializeField]
		private Transform tasksParent;

		// Token: 0x040014F9 RID: 5369
		[SerializeField]
		private Transform rewardsParent;

		// Token: 0x040014FA RID: 5370
		[SerializeField]
		private QuestRequiredItem requiredItem;

		// Token: 0x040014FB RID: 5371
		[SerializeField]
		private bool interactable;

		// Token: 0x040014FC RID: 5372
		private PrefabPool<TaskEntry> _taskEntryPool;

		// Token: 0x040014FD RID: 5373
		private PrefabPool<RewardEntry> _rewardEntryPool;

		// Token: 0x040014FE RID: 5374
		private Quest showingQuest;

		// Token: 0x040014FF RID: 5375
		private int activeTaskToken;
	}
}
