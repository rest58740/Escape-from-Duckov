using System;
using DG.Tweening;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C6 RID: 966
	public class ItemShortcutButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x000798F8 File Offset: 0x00077AF8
		// (set) Token: 0x060022AE RID: 8878 RVA: 0x00079900 File Offset: 0x00077B00
		public int Index { get; private set; }

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x00079909 File Offset: 0x00077B09
		// (set) Token: 0x060022B0 RID: 8880 RVA: 0x00079911 File Offset: 0x00077B11
		public ItemShortcutPanel Master { get; private set; }

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x0007991A File Offset: 0x00077B1A
		// (set) Token: 0x060022B2 RID: 8882 RVA: 0x00079922 File Offset: 0x00077B22
		public Inventory Inventory { get; private set; }

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060022B3 RID: 8883 RVA: 0x0007992B File Offset: 0x00077B2B
		// (set) Token: 0x060022B4 RID: 8884 RVA: 0x00079933 File Offset: 0x00077B33
		public CharacterMainControl Character { get; private set; }

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060022B5 RID: 8885 RVA: 0x0007993C File Offset: 0x00077B3C
		// (set) Token: 0x060022B6 RID: 8886 RVA: 0x00079944 File Offset: 0x00077B44
		public Item TargetItem { get; private set; }

		// Token: 0x060022B7 RID: 8887 RVA: 0x0007994D File Offset: 0x00077B4D
		private Item GetTargetItem()
		{
			return ItemShortcut.Get(this.Index);
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060022B8 RID: 8888 RVA: 0x0007995C File Offset: 0x00077B5C
		private bool Interactable
		{
			get
			{
				Item targetItem = this.TargetItem;
				return ((targetItem != null) ? targetItem.UsageUtilities : null) || (this.TargetItem && this.TargetItem.HasHandHeldAgent) || (this.TargetItem && this.TargetItem.GetBool("IsSkill", false));
			}
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x000799C4 File Offset: 0x00077BC4
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.Interactable)
			{
				this.denialIndicator.color = this.denialColor;
				DOTweenModuleUI.DOColor(this.denialIndicator, Color.clear, 0.1f);
				return;
			}
			if (this.Character && this.TargetItem && this.TargetItem.UsageUtilities && this.TargetItem.UsageUtilities.IsUsable(this.TargetItem, this.Character))
			{
				this.Character.UseItem(this.TargetItem);
				return;
			}
			if (this.Character && this.TargetItem && this.TargetItem.GetBool("IsSkill", false))
			{
				this.Character.ChangeHoldItem(this.TargetItem);
				return;
			}
			if (this.Character && this.TargetItem && this.TargetItem.HasHandHeldAgent)
			{
				this.Character.ChangeHoldItem(this.TargetItem);
				return;
			}
			this.AnimateDenial();
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x00079ADD File Offset: 0x00077CDD
		public void AnimateDenial()
		{
			ShortcutExtensions.DOKill(this.denialIndicator, false);
			this.denialIndicator.color = this.denialColor;
			DOTweenModuleUI.DOColor(this.denialIndicator, Color.clear, 0.1f);
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x00079B13 File Offset: 0x00077D13
		private void Awake()
		{
			ItemShortcutButton.OnRequireAnimateDenial += this.OnStaticAnimateDenial;
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x00079B26 File Offset: 0x00077D26
		private void OnDestroy()
		{
			ItemShortcutButton.OnRequireAnimateDenial -= this.OnStaticAnimateDenial;
			this.isBeingDestroyed = true;
			this.UnregisterEvents();
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x00079B46 File Offset: 0x00077D46
		private void OnStaticAnimateDenial(int index)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (index == this.Index)
			{
				this.AnimateDenial();
			}
		}

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x060022BE RID: 8894 RVA: 0x00079B60 File Offset: 0x00077D60
		// (remove) Token: 0x060022BF RID: 8895 RVA: 0x00079B94 File Offset: 0x00077D94
		private static event Action<int> OnRequireAnimateDenial;

		// Token: 0x060022C0 RID: 8896 RVA: 0x00079BC7 File Offset: 0x00077DC7
		public static void AnimateDenial(int index)
		{
			Action<int> onRequireAnimateDenial = ItemShortcutButton.OnRequireAnimateDenial;
			if (onRequireAnimateDenial == null)
			{
				return;
			}
			onRequireAnimateDenial(index);
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x00079BDC File Offset: 0x00077DDC
		internal void Initialize(ItemShortcutPanel itemShortcutPanel, int index)
		{
			this.UnregisterEvents();
			this.Master = itemShortcutPanel;
			this.Inventory = this.Master.Target;
			this.Index = index;
			this.Character = this.Master.Character;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x00079C2C File Offset: 0x00077E2C
		private void Refresh()
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			this.UnregisterEvents();
			this.TargetItem = this.GetTargetItem();
			if (this.TargetItem == null)
			{
				this.SetupEmpty();
			}
			else
			{
				this.SetupItem(this.TargetItem);
			}
			this.RegisterEvents();
			this.requireRefresh = false;
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00079C84 File Offset: 0x00077E84
		private void SetupItem(Item targetItem)
		{
			if (this.notInteractableIndicator)
			{
				this.notInteractableIndicator.gameObject.SetActive(false);
			}
			this.itemDisplay.Setup(targetItem);
			this.itemDisplay.gameObject.SetActive(true);
			this.notInteractableIndicator.gameObject.SetActive(!this.Interactable);
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x00079CE5 File Offset: 0x00077EE5
		private void SetupEmpty()
		{
			this.itemDisplay.gameObject.SetActive(false);
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x00079CF8 File Offset: 0x00077EF8
		private void RegisterEvents()
		{
			ItemShortcut.OnSetItem += this.OnItemShortcutSetItem;
			if (this.Inventory != null)
			{
				this.Inventory.onContentChanged += this.OnContentChanged;
			}
			if (this.TargetItem != null)
			{
				this.TargetItem.onSetStackCount += this.OnItemStackCountChanged;
			}
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x00079D60 File Offset: 0x00077F60
		private void UnregisterEvents()
		{
			ItemShortcut.OnSetItem -= this.OnItemShortcutSetItem;
			if (this.Inventory != null)
			{
				this.Inventory.onContentChanged -= this.OnContentChanged;
			}
			if (this.TargetItem != null)
			{
				this.TargetItem.onSetStackCount -= this.OnItemStackCountChanged;
			}
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x00079DC8 File Offset: 0x00077FC8
		private void OnItemShortcutSetItem(int obj)
		{
			this.Refresh();
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x00079DD0 File Offset: 0x00077FD0
		private void OnItemStackCountChanged(Item item)
		{
			if (item != this.TargetItem)
			{
				return;
			}
			this.requireRefresh = true;
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x00079DE8 File Offset: 0x00077FE8
		private void OnContentChanged(Inventory inventory, int index)
		{
			this.requireRefresh = true;
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x00079DF4 File Offset: 0x00077FF4
		private void Update()
		{
			if (this.requireRefresh)
			{
				this.Refresh();
			}
			bool flag = this.TargetItem != null && this.Character.CurrentHoldItemAgent != null && this.TargetItem == this.Character.CurrentHoldItemAgent.Item;
			if (flag && !this.lastFrameUsing)
			{
				this.OnStartedUsing();
			}
			else if (!flag && this.lastFrameUsing)
			{
				this.OnStoppedUsing();
			}
			this.usingIndicator.gameObject.SetActive(flag);
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x00079E84 File Offset: 0x00078084
		private void OnStartedUsing()
		{
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x00079E86 File Offset: 0x00078086
		private void OnStoppedUsing()
		{
		}

		// Token: 0x04001780 RID: 6016
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04001781 RID: 6017
		[SerializeField]
		private GameObject usingIndicator;

		// Token: 0x04001782 RID: 6018
		[SerializeField]
		private GameObject notInteractableIndicator;

		// Token: 0x04001783 RID: 6019
		[SerializeField]
		private Image denialIndicator;

		// Token: 0x04001784 RID: 6020
		[SerializeField]
		private Color denialColor;

		// Token: 0x0400178B RID: 6027
		private bool isBeingDestroyed;

		// Token: 0x0400178C RID: 6028
		private bool requireRefresh;

		// Token: 0x0400178D RID: 6029
		private bool lastFrameUsing;
	}
}
