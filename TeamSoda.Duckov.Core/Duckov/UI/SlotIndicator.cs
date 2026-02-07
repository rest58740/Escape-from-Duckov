using System;
using Duckov.Utilities;
using ItemStatsSystem.Items;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003BB RID: 955
	public class SlotIndicator : MonoBehaviour, IPoolable
	{
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002226 RID: 8742 RVA: 0x00077B8B File Offset: 0x00075D8B
		// (set) Token: 0x06002227 RID: 8743 RVA: 0x00077B93 File Offset: 0x00075D93
		public Slot Target { get; private set; }

		// Token: 0x06002228 RID: 8744 RVA: 0x00077B9C File Offset: 0x00075D9C
		public void Setup(Slot target)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x00077BB7 File Offset: 0x00075DB7
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregisterEvents();
			this.Target.onSlotContentChanged += this.OnSlotContentChanged;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x00077BDF File Offset: 0x00075DDF
		private void UnregisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onSlotContentChanged -= this.OnSlotContentChanged;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x00077C01 File Offset: 0x00075E01
		private void OnSlotContentChanged(Slot slot)
		{
			if (slot != this.Target)
			{
				Debug.LogError("Slot内容改变事件触发了，但它来自别的Slot。这说明Slot Indicator注册的事件发生了泄露，请检查代码。");
				return;
			}
			this.Refresh();
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x00077C1D File Offset: 0x00075E1D
		private void Refresh()
		{
			if (this.contentIndicator == null)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			this.contentIndicator.SetActive(this.Target.Content);
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x00077C52 File Offset: 0x00075E52
		public void NotifyPooled()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x00077C60 File Offset: 0x00075E60
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.Target = null;
			this.contentIndicator.SetActive(false);
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x00077C7B File Offset: 0x00075E7B
		private void OnEnable()
		{
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x00077C89 File Offset: 0x00075E89
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x04001736 RID: 5942
		[SerializeField]
		private GameObject contentIndicator;
	}
}
