using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x0200035B RID: 859
	public class QuestCompletePanel : MonoBehaviour
	{
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x0006B560 File Offset: 0x00069760
		private PrefabPool<RewardEntry> RewardEntryPool
		{
			get
			{
				if (this._rewardEntryPool == null)
				{
					this._rewardEntryPool = new PrefabPool<RewardEntry>(this.rewardEntryTemplate, this.rewardEntryTemplate.transform.parent, null, null, null, true, 10, 10000, null);
					this.rewardEntryTemplate.gameObject.SetActive(false);
				}
				return this._rewardEntryPool;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x0006B5B9 File Offset: 0x000697B9
		public Quest Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0006B5C1 File Offset: 0x000697C1
		private void Awake()
		{
			this.skipButton.onClick.AddListener(new UnityAction(this.Skip));
			this.takeAllButton.onClick.AddListener(new UnityAction(this.TakeAll));
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0006B5FC File Offset: 0x000697FC
		private void TakeAll()
		{
			if (this.target == null)
			{
				return;
			}
			foreach (Reward reward in this.target.rewards)
			{
				if (!reward.Claimed)
				{
					reward.Claim();
				}
			}
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0006B66C File Offset: 0x0006986C
		public void Skip()
		{
			this.skipClicked = true;
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0006B678 File Offset: 0x00069878
		public UniTask Show(Quest quest)
		{
			QuestCompletePanel.<Show>d__14 <Show>d__;
			<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Show>d__.<>4__this = this;
			<Show>d__.quest = quest;
			<Show>d__.<>1__state = -1;
			<Show>d__.<>t__builder.Start<QuestCompletePanel.<Show>d__14>(ref <Show>d__);
			return <Show>d__.<>t__builder.Task;
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0006B6C4 File Offset: 0x000698C4
		private UniTask WaitForEndOfInteraction()
		{
			QuestCompletePanel.<WaitForEndOfInteraction>d__16 <WaitForEndOfInteraction>d__;
			<WaitForEndOfInteraction>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForEndOfInteraction>d__.<>4__this = this;
			<WaitForEndOfInteraction>d__.<>1__state = -1;
			<WaitForEndOfInteraction>d__.<>t__builder.Start<QuestCompletePanel.<WaitForEndOfInteraction>d__16>(ref <WaitForEndOfInteraction>d__);
			return <WaitForEndOfInteraction>d__.<>t__builder.Task;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0006B708 File Offset: 0x00069908
		private void SetupContent(Quest quest)
		{
			this.target = quest;
			if (quest == null)
			{
				return;
			}
			this.questNameText.text = quest.DisplayName;
			this.RewardEntryPool.ReleaseAll();
			foreach (Reward reward in quest.rewards)
			{
				RewardEntry rewardEntry = this.RewardEntryPool.Get(this.rewardEntryTemplate.transform.parent);
				rewardEntry.Setup(reward);
				rewardEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x040014A3 RID: 5283
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x040014A4 RID: 5284
		[SerializeField]
		private TextMeshProUGUI questNameText;

		// Token: 0x040014A5 RID: 5285
		[SerializeField]
		private RewardEntry rewardEntryTemplate;

		// Token: 0x040014A6 RID: 5286
		[SerializeField]
		private Button skipButton;

		// Token: 0x040014A7 RID: 5287
		[SerializeField]
		private Button takeAllButton;

		// Token: 0x040014A8 RID: 5288
		private PrefabPool<RewardEntry> _rewardEntryPool;

		// Token: 0x040014A9 RID: 5289
		private Quest target;

		// Token: 0x040014AA RID: 5290
		private bool skipClicked;
	}
}
