using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Quests.UI
{
	// Token: 0x02000362 RID: 866
	public class QuestView : View, ISingleSelectionMenu<QuestEntry>, IQuestSortable
	{
		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x0006C520 File Offset: 0x0006A720
		public static QuestView Instance
		{
			get
			{
				return View.GetViewInstance<QuestView>();
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001E09 RID: 7689 RVA: 0x0006C527 File Offset: 0x0006A727
		public QuestView.ShowContent ShowingContentType
		{
			get
			{
				return this.showingContentType;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x0006C52F File Offset: 0x0006A72F
		// (set) Token: 0x06001E0B RID: 7691 RVA: 0x0006C537 File Offset: 0x0006A737
		public Quest.SortingMode SortingMode
		{
			get
			{
				return this._sortingMode;
			}
			set
			{
				this._sortingMode = value;
				this.RefreshEntryList();
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x0006C546 File Offset: 0x0006A746
		// (set) Token: 0x06001E0D RID: 7693 RVA: 0x0006C54E File Offset: 0x0006A74E
		public bool SortRevert
		{
			get
			{
				return this._sortRevert;
			}
			set
			{
				this._sortRevert = value;
				this.RefreshEntryList();
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001E0E RID: 7694 RVA: 0x0006C560 File Offset: 0x0006A760
		public IList<Quest> ShowingContent
		{
			get
			{
				if (this.target == null)
				{
					return null;
				}
				QuestView.ShowContent showContent = this.showingContentType;
				if (showContent == QuestView.ShowContent.Active)
				{
					return this.target.ActiveQuests;
				}
				if (showContent != QuestView.ShowContent.History)
				{
					return null;
				}
				return this.target.HistoryQuests;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001E0F RID: 7695 RVA: 0x0006C5A8 File Offset: 0x0006A7A8
		private PrefabPool<QuestEntry> QuestEntryPool
		{
			get
			{
				if (this._questEntryPool == null)
				{
					this._questEntryPool = new PrefabPool<QuestEntry>(this.questEntry, this.questEntryParent, delegate(QuestEntry e)
					{
						this.activeEntries.Add(e);
						e.SetMenu(this);
					}, delegate(QuestEntry e)
					{
						this.activeEntries.Remove(e);
					}, null, true, 10, 10000, null);
				}
				return this._questEntryPool;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001E10 RID: 7696 RVA: 0x0006C5FC File Offset: 0x0006A7FC
		private QuestEntry SelectedQuestEntry
		{
			get
			{
				return this.selectedQuestEntry;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001E11 RID: 7697 RVA: 0x0006C604 File Offset: 0x0006A804
		public Quest SelectedQuest
		{
			get
			{
				QuestEntry questEntry = this.selectedQuestEntry;
				if (questEntry == null)
				{
					return null;
				}
				return questEntry.Target;
			}
		}

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06001E12 RID: 7698 RVA: 0x0006C618 File Offset: 0x0006A818
		// (remove) Token: 0x06001E13 RID: 7699 RVA: 0x0006C650 File Offset: 0x0006A850
		internal event Action<QuestView, QuestView.ShowContent> onShowingContentChanged;

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06001E14 RID: 7700 RVA: 0x0006C688 File Offset: 0x0006A888
		// (remove) Token: 0x06001E15 RID: 7701 RVA: 0x0006C6C0 File Offset: 0x0006A8C0
		internal event Action<QuestView, QuestEntry> onSelectedEntryChanged;

		// Token: 0x06001E16 RID: 7702 RVA: 0x0006C6F5 File Offset: 0x0006A8F5
		public void Setup()
		{
			this.Setup(QuestManager.Instance);
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x0006C704 File Offset: 0x0006A904
		private void Setup(QuestManager target)
		{
			this.target = target;
			Quest oldSelection = this.SelectedQuest;
			this.RefreshEntryList();
			QuestEntry questEntry = this.activeEntries.Find((QuestEntry e) => e.Target == oldSelection);
			if (questEntry != null)
			{
				this.SetSelection(questEntry);
			}
			else
			{
				this.SetSelection(null);
			}
			this.RefreshDetails();
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0006C769 File Offset: 0x0006A969
		public static void Show()
		{
			QuestView instance = QuestView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Open(null);
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x0006C77B File Offset: 0x0006A97B
		protected override void OnOpen()
		{
			base.OnOpen();
			this.Setup();
			this.fadeGroup.Show();
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x0006C794 File Offset: 0x0006A994
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0006C7A7 File Offset: 0x0006A9A7
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x0006C7AF File Offset: 0x0006A9AF
		private void OnEnable()
		{
			this.RegisterStaticEvents();
			this.Setup(QuestManager.Instance);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0006C7C2 File Offset: 0x0006A9C2
		private void OnDisable()
		{
			if (this.details != null)
			{
				this.details.Setup(null);
			}
			this.UnregisterStaticEvents();
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0006C7E4 File Offset: 0x0006A9E4
		private void RegisterStaticEvents()
		{
			QuestManager.onQuestListsChanged += this.Setup;
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x0006C7F7 File Offset: 0x0006A9F7
		private void UnregisterStaticEvents()
		{
			QuestManager.onQuestListsChanged -= this.Setup;
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0006C80C File Offset: 0x0006AA0C
		private void RefreshEntryList()
		{
			this.QuestEntryPool.ReleaseAll();
			bool flag = this.target != null && this.ShowingContent != null && this.ShowingContent.Count > 0;
			this.entryListPlaceHolder.SetActive(!flag);
			if (!flag)
			{
				return;
			}
			List<Quest> list = this.ShowingContent.ToList<Quest>();
			if (this.SortingMode != Quest.SortingMode.Default)
			{
				list.Sort((Quest a, Quest b) => Quest.Compare(a, b, this.SortingMode, this.SortRevert));
			}
			foreach (Quest quest in list)
			{
				QuestEntry questEntry = this.QuestEntryPool.Get(this.questEntryParent);
				questEntry.Setup(quest);
				questEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x0006C8E4 File Offset: 0x0006AAE4
		private void RefreshDetails()
		{
			this.details.Setup(this.SelectedQuest);
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0006C8F8 File Offset: 0x0006AAF8
		public void SetShowingContent(QuestView.ShowContent flags)
		{
			this.showingContentType = flags;
			this.RefreshEntryList();
			List<QuestEntry> list = this.activeEntries;
			if (list != null && list.Count > 0)
			{
				this.SetSelection(this.activeEntries[0]);
			}
			else
			{
				this.SetSelection(null);
			}
			this.RefreshDetails();
			foreach (QuestEntry questEntry in this.activeEntries)
			{
				questEntry.NotifyRefresh();
			}
			Action<QuestView, QuestView.ShowContent> action = this.onShowingContentChanged;
			if (action == null)
			{
				return;
			}
			action(this, flags);
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0006C9A4 File Offset: 0x0006ABA4
		public void ShowActiveQuests()
		{
			this.SetShowingContent(QuestView.ShowContent.Active);
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x0006C9AD File Offset: 0x0006ABAD
		public void ShowHistoryQuests()
		{
			this.SetShowingContent(QuestView.ShowContent.History);
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x0006C9B6 File Offset: 0x0006ABB6
		public QuestEntry GetSelection()
		{
			return this.selectedQuestEntry;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x0006C9C0 File Offset: 0x0006ABC0
		public bool SetSelection(QuestEntry selection)
		{
			this.selectedQuestEntry = selection;
			Action<QuestView, QuestEntry> action = this.onSelectedEntryChanged;
			if (action != null)
			{
				action(this, this.selectedQuestEntry);
			}
			foreach (QuestEntry questEntry in this.activeEntries)
			{
				questEntry.NotifyRefresh();
			}
			this.RefreshDetails();
			return true;
		}

		// Token: 0x040014E2 RID: 5346
		[SerializeField]
		private QuestEntry questEntry;

		// Token: 0x040014E3 RID: 5347
		[SerializeField]
		private Transform questEntryParent;

		// Token: 0x040014E4 RID: 5348
		[SerializeField]
		private GameObject entryListPlaceHolder;

		// Token: 0x040014E5 RID: 5349
		[SerializeField]
		private QuestViewDetails details;

		// Token: 0x040014E6 RID: 5350
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040014E7 RID: 5351
		private QuestManager target;

		// Token: 0x040014E8 RID: 5352
		[SerializeField]
		private QuestView.ShowContent showingContentType;

		// Token: 0x040014E9 RID: 5353
		private Quest.SortingMode _sortingMode;

		// Token: 0x040014EA RID: 5354
		private bool _sortRevert;

		// Token: 0x040014EB RID: 5355
		private PrefabPool<QuestEntry> _questEntryPool;

		// Token: 0x040014EC RID: 5356
		private List<QuestEntry> activeEntries = new List<QuestEntry>();

		// Token: 0x040014ED RID: 5357
		private QuestEntry selectedQuestEntry;

		// Token: 0x02000629 RID: 1577
		[Flags]
		public enum ShowContent
		{
			// Token: 0x04002240 RID: 8768
			Active = 1,
			// Token: 0x04002241 RID: 8769
			History = 2
		}
	}
}
