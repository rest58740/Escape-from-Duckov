using System;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class FishingRod : MonoBehaviour
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x0600076B RID: 1899 RVA: 0x00021634 File Offset: 0x0001F834
	private ItemAgent selfAgent
	{
		get
		{
			if (this._selfAgent == null)
			{
				this._selfAgent = base.GetComponent<ItemAgent>();
			}
			return this._selfAgent;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x0600076C RID: 1900 RVA: 0x00021656 File Offset: 0x0001F856
	public Item Bait
	{
		get
		{
			if (this.baitSlot == null)
			{
				this.baitSlot = this.selfAgent.Item.Slots.GetSlot("Bait");
			}
			if (this.baitSlot != null)
			{
				return this.baitSlot.Content;
			}
			return null;
		}
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00021698 File Offset: 0x0001F898
	public bool UseBait()
	{
		Item bait = this.Bait;
		if (bait == null)
		{
			return false;
		}
		if (bait.Stackable)
		{
			bait.StackCount--;
		}
		else
		{
			ItemTreeExtensions.DestroyTree(bait);
		}
		return true;
	}

	// Token: 0x0400071F RID: 1823
	[SerializeField]
	private ItemAgent _selfAgent;

	// Token: 0x04000720 RID: 1824
	private Slot baitSlot;

	// Token: 0x04000721 RID: 1825
	public Transform lineStart;
}
