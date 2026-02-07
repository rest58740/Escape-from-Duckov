using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fishing.UI
{
	// Token: 0x02000221 RID: 545
	public class BaitSelectPanelEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x00040FD5 File Offset: 0x0003F1D5
		public Item Target
		{
			get
			{
				return this.targetItem;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x00040FDD File Offset: 0x0003F1DD
		private bool Selected
		{
			get
			{
				return !(this.master == null) && this.master.GetSelection() == this;
			}
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00041000 File Offset: 0x0003F200
		internal void Setup(BaitSelectPanel master, Item cur)
		{
			this.UnregisterEvents();
			this.master = master;
			this.targetItem = cur;
			this.itemDisplay.Setup(this.targetItem);
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00041033 File Offset: 0x0003F233
		private void RegisterEvents()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.onSetSelection += this.Refresh;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0004105B File Offset: 0x0003F25B
		private void UnregisterEvents()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.onSetSelection -= this.Refresh;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00041083 File Offset: 0x0003F283
		private void Refresh()
		{
			this.selectedIndicator.SetActive(this.Selected);
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00041096 File Offset: 0x0003F296
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnPointerClick;
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x000410AF File Offset: 0x0003F2AF
		public void OnPointerClick(PointerEventData eventData)
		{
			eventData.Use();
			this.master.NotifySelect(this);
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x000410C3 File Offset: 0x0003F2C3
		private void OnPointerClick(ItemDisplay display, PointerEventData data)
		{
			this.OnPointerClick(data);
		}

		// Token: 0x04000D46 RID: 3398
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04000D47 RID: 3399
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04000D48 RID: 3400
		private BaitSelectPanel master;

		// Token: 0x04000D49 RID: 3401
		private Item targetItem;
	}
}
