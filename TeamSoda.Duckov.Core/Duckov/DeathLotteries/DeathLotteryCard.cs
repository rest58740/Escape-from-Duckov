using System;
using Duckov.Economy;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000317 RID: 791
	public class DeathLotteryCard : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060019F2 RID: 6642 RVA: 0x0005EF24 File Offset: 0x0005D124
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0005EF2C File Offset: 0x0005D12C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.master == null)
			{
				return;
			}
			if (this.master.Target == null)
			{
				return;
			}
			DeathLottery.OptionalCosts cost = this.master.Target.GetCost();
			this.master.NotifyEntryClicked(this, cost.costA);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0005EF80 File Offset: 0x0005D180
		public void Setup(DeathLotteryVIew master, int index)
		{
			if (master == null)
			{
				return;
			}
			if (master.Target == null)
			{
				return;
			}
			this.master = master;
			this.targetItem = master.Target.ItemInstances[index];
			this.index = index;
			this.itemDisplay.Setup(this.targetItem);
			this.cardDisplay.SetFacing(master.Target.CurrentStatus.selectedItems.Contains(index), true);
			this.Refresh();
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0005F004 File Offset: 0x0005D204
		public void NotifyFacing(bool uncovered)
		{
			this.cardDisplay.SetFacing(uncovered, false);
			this.Refresh();
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x0005F01C File Offset: 0x0005D21C
		private bool Selected
		{
			get
			{
				return !(this.master == null) && !(this.master.Target == null) && this.master.Target.CurrentStatus.selectedItems != null && this.master.Target.CurrentStatus.selectedItems.Contains(this.Index);
			}
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0005F087 File Offset: 0x0005D287
		private void Refresh()
		{
			this.selectedIndicator.SetActive(this.Selected);
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0005F09A File Offset: 0x0005D29A
		private void Awake()
		{
			this.costFade.Hide();
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0005F0A8 File Offset: 0x0005D2A8
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.master.Target.CurrentStatus.SelectedCount >= this.master.Target.MaxChances)
			{
				return;
			}
			Cost costA = this.master.Target.GetCost().costA;
			this.costDisplay.Setup(costA, 1);
			this.freeIndicator.SetActive(costA.IsFree);
			this.costFade.Show();
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0005F120 File Offset: 0x0005D320
		public void OnPointerExit(PointerEventData eventData)
		{
			this.costFade.Hide();
		}

		// Token: 0x040012DC RID: 4828
		[SerializeField]
		private CardDisplay cardDisplay;

		// Token: 0x040012DD RID: 4829
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x040012DE RID: 4830
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x040012DF RID: 4831
		[SerializeField]
		private GameObject freeIndicator;

		// Token: 0x040012E0 RID: 4832
		[SerializeField]
		private FadeGroup costFade;

		// Token: 0x040012E1 RID: 4833
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x040012E2 RID: 4834
		private DeathLotteryVIew master;

		// Token: 0x040012E3 RID: 4835
		private int index;

		// Token: 0x040012E4 RID: 4836
		private Item targetItem;
	}
}
