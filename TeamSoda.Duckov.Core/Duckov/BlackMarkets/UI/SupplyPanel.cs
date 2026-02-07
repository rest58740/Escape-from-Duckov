using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x02000322 RID: 802
	public class SupplyPanel : MonoBehaviour
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x0006070B File Offset: 0x0005E90B
		// (set) Token: 0x06001A7A RID: 6778 RVA: 0x00060713 File Offset: 0x0005E913
		public BlackMarket Target { get; private set; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x0006071C File Offset: 0x0005E91C
		private PrefabPool<SupplyPanel_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<SupplyPanel_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, new Action<SupplyPanel_Entry>(this.OnCreateEntry));
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00060760 File Offset: 0x0005E960
		private void OnCreateEntry(SupplyPanel_Entry entry)
		{
			entry.onDealButtonClicked += this.OnEntryClicked;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x00060774 File Offset: 0x0005E974
		private void OnEntryClicked(SupplyPanel_Entry entry)
		{
			Debug.Log("Supply entry clicked");
			this.Target.Buy(entry.Target);
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00060792 File Offset: 0x0005E992
		internal void Setup(BlackMarket target)
		{
			if (target == null)
			{
				Debug.LogError("加载 BlackMarket 的 Supply Panel 失败。Black Market 对象不存在。");
				return;
			}
			this.Target = target;
			this.Refresh();
			if (base.isActiveAndEnabled)
			{
				this.RegisterEvents();
			}
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000607C3 File Offset: 0x0005E9C3
		private void OnEnable()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x000607D1 File Offset: 0x0005E9D1
		private void OnDisable()
		{
			this.UnregsiterEvents();
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x000607DC File Offset: 0x0005E9DC
		private void Refresh()
		{
			if (this.Target == null)
			{
				return;
			}
			this.EntryPool.ReleaseAll();
			foreach (BlackMarket.DemandSupplyEntry target in this.Target.Supplies)
			{
				this.EntryPool.Get(null).Setup(target);
			}
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00060854 File Offset: 0x0005EA54
		private void UnregsiterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onAfterGenerateEntries -= this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0006087C File Offset: 0x0005EA7C
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregsiterEvents();
			this.Target.onAfterGenerateEntries += this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000608AA File Offset: 0x0005EAAA
		private void OnAfterTargetGenerateEntries()
		{
			this.Refresh();
		}

		// Token: 0x0400132A RID: 4906
		[SerializeField]
		private SupplyPanel_Entry entryTemplate;

		// Token: 0x0400132B RID: 4907
		private PrefabPool<SupplyPanel_Entry> _entryPool;
	}
}
