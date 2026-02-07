using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B4 RID: 948
	public class UsageUtilitiesDisplay : MonoBehaviour
	{
		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002173 RID: 8563 RVA: 0x00075347 File Offset: 0x00073547
		// (set) Token: 0x06002174 RID: 8564 RVA: 0x0007534F File Offset: 0x0007354F
		public UsageUtilities Target { get; private set; }

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002175 RID: 8565 RVA: 0x00075358 File Offset: 0x00073558
		private PrefabPool<UsageUtilitiesDisplay_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<UsageUtilitiesDisplay_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x00075394 File Offset: 0x00073594
		public void Setup(Item item)
		{
			if (!(item == null))
			{
				UsageUtilities component = item.GetComponent<UsageUtilities>();
				if (!(component == null))
				{
					this.Target = component;
					base.gameObject.SetActive(true);
					this.Refresh();
					return;
				}
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000753E0 File Offset: 0x000735E0
		private void Refresh()
		{
			this.EntryPool.ReleaseAll();
			foreach (UsageBehavior usageBehavior in this.Target.behaviors)
			{
				if (!(usageBehavior == null) && usageBehavior.DisplaySettings.display)
				{
					this.EntryPool.Get(null).Setup(usageBehavior);
				}
			}
			if (this.EntryPool.ActiveEntries.Count <= 0)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x040016CD RID: 5837
		[SerializeField]
		private UsageUtilitiesDisplay_Entry entryTemplate;

		// Token: 0x040016CE RID: 5838
		private PrefabPool<UsageUtilitiesDisplay_Entry> _entryPool;
	}
}
