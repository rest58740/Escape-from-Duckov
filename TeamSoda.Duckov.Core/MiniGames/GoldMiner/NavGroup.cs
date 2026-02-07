using System;
using System.Collections.Generic;
using Duckov.MiniGames.GoldMiner.UI;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B4 RID: 692
	public class NavGroup : MiniGameBehaviour
	{
		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x00054A38 File Offset: 0x00052C38
		// (set) Token: 0x060016C5 RID: 5829 RVA: 0x00054A3F File Offset: 0x00052C3F
		public static NavGroup ActiveNavGroup { get; private set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060016C6 RID: 5830 RVA: 0x00054A47 File Offset: 0x00052C47
		public bool active
		{
			get
			{
				return NavGroup.ActiveNavGroup == this;
			}
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00054A54 File Offset: 0x00052C54
		public void SetAsActiveNavGroup()
		{
			NavGroup activeNavGroup = NavGroup.ActiveNavGroup;
			NavGroup.ActiveNavGroup = this;
			this.RefreshAll();
			if (activeNavGroup != null)
			{
				activeNavGroup.RefreshAll();
			}
			Action onNavGroupChanged = NavGroup.OnNavGroupChanged;
			if (onNavGroupChanged == null)
			{
				return;
			}
			onNavGroupChanged();
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x00054A91 File Offset: 0x00052C91
		// (set) Token: 0x060016C9 RID: 5833 RVA: 0x00054A9C File Offset: 0x00052C9C
		public int NavIndex
		{
			get
			{
				return this._navIndex;
			}
			set
			{
				int navIndex = this._navIndex;
				this._navIndex = value;
				this.CleanupIndex();
				int navIndex2 = this._navIndex;
				this.RefreshEntry(navIndex);
				this.RefreshEntry(navIndex2);
			}
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00054AD2 File Offset: 0x00052CD2
		protected override void OnEnable()
		{
			base.OnEnable();
			this.RefreshAll();
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00054AE0 File Offset: 0x00052CE0
		private void CleanupIndex()
		{
			if (this._navIndex < 0)
			{
				this._navIndex = this.entries.Count - 1;
			}
			if (this._navIndex >= this.entries.Count)
			{
				this._navIndex = 0;
			}
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00054B18 File Offset: 0x00052D18
		private void RefreshAll()
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.RefreshEntry(i);
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00054B42 File Offset: 0x00052D42
		private void RefreshEntry(int index)
		{
			if (index < 0 || index >= this.entries.Count)
			{
				return;
			}
			this.entries[index].NotifySelectionState(this.active && this.NavIndex == index);
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00054B7C File Offset: 0x00052D7C
		public NavEntry GetSelectedEntry()
		{
			if (this.NavIndex < 0 || this.NavIndex >= this.entries.Count)
			{
				return null;
			}
			return this.entries[this.NavIndex];
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00054BB0 File Offset: 0x00052DB0
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponentInParent<GoldMiner>();
			}
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00054BFE File Offset: 0x00052DFE
		private void OnLevelBegin(GoldMiner miner)
		{
			this.RefreshAll();
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00054C06 File Offset: 0x00052E06
		internal void Remove(NavEntry navEntry)
		{
			this.entries.Remove(navEntry);
			this.CleanupIndex();
			this.RefreshAll();
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00054C21 File Offset: 0x00052E21
		internal void Add(NavEntry navEntry)
		{
			this.entries.Add(navEntry);
			this.CleanupIndex();
			this.RefreshAll();
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00054C3C File Offset: 0x00052E3C
		internal void TrySelect(NavEntry navEntry)
		{
			if (!this.entries.Contains(navEntry))
			{
				return;
			}
			int navIndex = this.entries.IndexOf(navEntry);
			this.SetAsActiveNavGroup();
			this.NavIndex = navIndex;
		}

		// Token: 0x040010E9 RID: 4329
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010EA RID: 4330
		[SerializeField]
		public List<NavEntry> entries;

		// Token: 0x040010EC RID: 4332
		public static Action OnNavGroupChanged;

		// Token: 0x040010ED RID: 4333
		private int _navIndex;
	}
}
