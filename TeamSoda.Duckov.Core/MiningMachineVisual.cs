using System;
using System.Collections.Generic;
using Duckov.Bitcoins;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class MiningMachineVisual : MonoBehaviour
{
	// Token: 0x06000BFC RID: 3068 RVA: 0x00033294 File Offset: 0x00031494
	private void Update()
	{
		if (!this.inited && BitcoinMiner.Instance && BitcoinMiner.Instance.Item != null)
		{
			this.inited = true;
			this.minnerItem = BitcoinMiner.Instance.Item;
			this.minnerItem.onSlotContentChanged += this.OnSlotContentChanged;
			this.slots = this.minnerItem.Slots;
			this.OnSlotContentChanged(this.minnerItem, null);
			return;
		}
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x00033314 File Offset: 0x00031514
	private void OnDestroy()
	{
		if (this.minnerItem)
		{
			this.minnerItem.onSlotContentChanged -= this.OnSlotContentChanged;
		}
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x0003333C File Offset: 0x0003153C
	private void OnSlotContentChanged(Item minnerItem, Slot changedSlot)
	{
		for (int i = 0; i < this.slots.Count; i++)
		{
			if (!(this.cardsDisplay[i] == null))
			{
				Item content = this.slots[i].Content;
				MiningMachineCardDisplay.CardTypes cardType = MiningMachineCardDisplay.CardTypes.normal;
				if (content != null)
				{
					ItemSetting_GPU component = content.GetComponent<ItemSetting_GPU>();
					if (component)
					{
						cardType = component.cardType;
					}
				}
				this.cardsDisplay[i].SetVisualActive(content != null, cardType);
			}
		}
	}

	// Token: 0x04000A49 RID: 2633
	public List<MiningMachineCardDisplay> cardsDisplay;

	// Token: 0x04000A4A RID: 2634
	private bool inited;

	// Token: 0x04000A4B RID: 2635
	private SlotCollection slots;

	// Token: 0x04000A4C RID: 2636
	private Item minnerItem;
}
