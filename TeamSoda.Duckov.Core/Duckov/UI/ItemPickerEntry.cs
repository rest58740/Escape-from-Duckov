using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000396 RID: 918
	public class ItemPickerEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06002002 RID: 8194 RVA: 0x00070EB5 File Offset: 0x0006F0B5
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayClicked;
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x00070ECE File Offset: 0x0006F0CE
		private void OnDestroy()
		{
			this.itemDisplay.onPointerClick -= this.OnItemDisplayClicked;
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x00070EE7 File Offset: 0x0006F0E7
		private void OnItemDisplayClicked(ItemDisplay display, PointerEventData eventData)
		{
			this.master.NotifyEntryClicked(this, this.target);
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x00070EFC File Offset: 0x0006F0FC
		public void Setup(ItemPicker master, Item item)
		{
			this.master = master;
			this.target = item;
			if (this.target != null)
			{
				this.itemDisplay.Setup(this.target);
			}
			else
			{
				Debug.LogError("Item Picker不应当展示空的Item。");
			}
			this.itemDisplay.ShowOperationButtons = false;
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x00070F4E File Offset: 0x0006F14E
		public void NotifyPooled()
		{
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x00070F50 File Offset: 0x0006F150
		public void NotifyReleased()
		{
		}

		// Token: 0x040015EE RID: 5614
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x040015EF RID: 5615
		private ItemPicker master;

		// Token: 0x040015F0 RID: 5616
		private Item target;
	}
}
