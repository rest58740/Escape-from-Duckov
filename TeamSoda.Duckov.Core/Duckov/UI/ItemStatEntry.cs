using System;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B1 RID: 945
	public class ItemStatEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x0600215B RID: 8539 RVA: 0x00075071 File Offset: 0x00073271
		public void NotifyPooled()
		{
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x00075073 File Offset: 0x00073273
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x00075082 File Offset: 0x00073282
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x0007508A File Offset: 0x0007328A
		internal void Setup(Stat target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000750A8 File Offset: 0x000732A8
		private void Refresh()
		{
			StatInfoDatabase.Entry entry = StatInfoDatabase.Get(this.target.Key);
			this.displayName.text = this.target.DisplayName;
			this.value.text = this.target.Value.ToString(entry.DisplayFormat);
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x00075101 File Offset: 0x00073301
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty += this.OnTargetSetDirty;
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x00075123 File Offset: 0x00073323
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetDirty -= this.OnTargetSetDirty;
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x00075145 File Offset: 0x00073345
		private void OnTargetSetDirty(Stat stat)
		{
			if (stat != this.target)
			{
				Debug.LogError("ItemStatEntry.target与事件触发者不匹配。");
				return;
			}
			this.Refresh();
		}

		// Token: 0x040016C4 RID: 5828
		private Stat target;

		// Token: 0x040016C5 RID: 5829
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x040016C6 RID: 5830
		[SerializeField]
		private TextMeshProUGUI value;
	}
}
