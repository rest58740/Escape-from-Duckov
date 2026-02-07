using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x02000320 RID: 800
	public class DemandPanel : MonoBehaviour
	{
		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x00060295 File Offset: 0x0005E495
		// (set) Token: 0x06001A5C RID: 6748 RVA: 0x0006029D File Offset: 0x0005E49D
		public BlackMarket Target { get; private set; }

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x000602A8 File Offset: 0x0005E4A8
		private PrefabPool<DemandPanel_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<DemandPanel_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, new Action<DemandPanel_Entry>(this.OnCreateEntry));
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x000602EC File Offset: 0x0005E4EC
		private void OnCreateEntry(DemandPanel_Entry entry)
		{
			entry.onDealButtonClicked += this.OnEntryClicked;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x00060300 File Offset: 0x0005E500
		private void OnEntryClicked(DemandPanel_Entry entry)
		{
			this.Target.Sell(entry.Target);
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x00060314 File Offset: 0x0005E514
		internal void Setup(BlackMarket target)
		{
			if (target == null)
			{
				Debug.LogError("加载 BlackMarket 的 DemandPanel 失败。Black Market 对象不存在。");
				return;
			}
			this.Target = target;
			this.Refresh();
			if (base.isActiveAndEnabled)
			{
				this.RegisterEvents();
			}
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00060345 File Offset: 0x0005E545
		private void OnEnable()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x00060353 File Offset: 0x0005E553
		private void OnDisable()
		{
			this.UnregsiterEvents();
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x0006035C File Offset: 0x0005E55C
		private void Refresh()
		{
			if (this.Target == null)
			{
				return;
			}
			this.EntryPool.ReleaseAll();
			foreach (BlackMarket.DemandSupplyEntry target in this.Target.Demands)
			{
				this.EntryPool.Get(null).Setup(target);
			}
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x000603D4 File Offset: 0x0005E5D4
		private void UnregsiterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onAfterGenerateEntries -= this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x000603FC File Offset: 0x0005E5FC
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregsiterEvents();
			this.Target.onAfterGenerateEntries += this.OnAfterTargetGenerateEntries;
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0006042A File Offset: 0x0005E62A
		private void OnAfterTargetGenerateEntries()
		{
			this.Refresh();
		}

		// Token: 0x0400131B RID: 4891
		[SerializeField]
		private DemandPanel_Entry entryTemplate;

		// Token: 0x0400131C RID: 4892
		private PrefabPool<DemandPanel_Entry> _entryPool;
	}
}
