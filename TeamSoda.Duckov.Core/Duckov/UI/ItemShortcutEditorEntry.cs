using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x020003C7 RID: 967
	public class ItemShortcutEditorEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IItemDragSource, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x00079E90 File Offset: 0x00078090
		private Item TargetItem
		{
			get
			{
				return ItemShortcut.Get(this.index);
			}
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x00079EA0 File Offset: 0x000780A0
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayClicked;
			this.itemDisplay.onReceiveDrop += this.OnDrop;
			ItemShortcut.OnSetItem += this.OnSetItem;
			this.hoveringIndicator.SetActive(false);
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x00079EF9 File Offset: 0x000780F9
		private void OnSetItem(int index)
		{
			if (index == this.index)
			{
				this.Refresh();
			}
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x00079F0A File Offset: 0x0007810A
		private void OnItemDisplayClicked(ItemDisplay display, PointerEventData data)
		{
			this.OnPointerClick(data);
			data.Use();
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x00079F19 File Offset: 0x00078119
		public void OnPointerClick(PointerEventData eventData)
		{
			if (ItemUIUtilities.SelectedItem != null && ItemShortcut.Set(this.index, ItemUIUtilities.SelectedItem))
			{
				this.Refresh();
			}
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x00079F40 File Offset: 0x00078140
		internal void Refresh()
		{
			this.UnregisterEvents();
			if (this.displayingItem != this.TargetItem)
			{
				this.itemDisplay.Punch();
			}
			this.displayingItem = this.TargetItem;
			this.itemDisplay.Setup(this.displayingItem);
			this.itemDisplay.ShowOperationButtons = false;
			this.RegisterEvents();
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x00079FA0 File Offset: 0x000781A0
		private void RegisterEvents()
		{
			if (this.displayingItem != null)
			{
				this.displayingItem.onParentChanged += this.OnTargetParentChanged;
				this.displayingItem.onSetStackCount += this.OnTargetStackCountChanged;
			}
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x00079FDE File Offset: 0x000781DE
		private void UnregisterEvents()
		{
			if (this.displayingItem != null)
			{
				this.displayingItem.onParentChanged -= this.OnTargetParentChanged;
				this.displayingItem.onSetStackCount -= this.OnTargetStackCountChanged;
			}
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x0007A01C File Offset: 0x0007821C
		private void OnTargetStackCountChanged(Item item)
		{
			this.SetDirty();
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x0007A024 File Offset: 0x00078224
		private void OnTargetParentChanged(Item item)
		{
			this.SetDirty();
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x0007A02C File Offset: 0x0007822C
		private void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x0007A035 File Offset: 0x00078235
		private void Update()
		{
			if (this.dirty)
			{
				this.Refresh();
			}
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x0007A045 File Offset: 0x00078245
		private void OnDestroy()
		{
			this.UnregisterEvents();
			ItemShortcut.OnSetItem -= this.OnSetItem;
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x0007A060 File Offset: 0x00078260
		internal void Setup(int i)
		{
			this.index = i;
			this.Refresh();
			InputActionReference inputActionReference = InputActionReference.Create(GameplayDataSettings.InputActions[string.Format("Character/ItemShortcut{0}", i + 3)]);
			this.indicator.Setup(inputActionReference, -1);
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x0007A0AC File Offset: 0x000782AC
		public void OnDrop(PointerEventData eventData)
		{
			eventData.Use();
			IItemDragSource component = eventData.pointerDrag.gameObject.GetComponent<IItemDragSource>();
			if (component == null)
			{
				return;
			}
			if (!component.IsEditable())
			{
				return;
			}
			Item item = component.GetItem();
			if (item == null)
			{
				return;
			}
			Item targetItem = this.TargetItem;
			if (!item.IsInPlayerCharacter())
			{
				ItemUtilities.SendToPlayer(item, false, false);
			}
			if (ItemShortcut.Set(this.index, item))
			{
				this.Refresh();
				AudioManager.Post("UI/click");
			}
			ItemShortcutEditorEntry itemShortcutEditorEntry = component as ItemShortcutEditorEntry;
			if (itemShortcutEditorEntry == null)
			{
				return;
			}
			if (itemShortcutEditorEntry == this)
			{
				return;
			}
			if (targetItem == null)
			{
				return;
			}
			ItemShortcut.Set(itemShortcutEditorEntry.index, targetItem);
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x0007A150 File Offset: 0x00078350
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hoveringIndicator.SetActive(true);
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x0007A15E File Offset: 0x0007835E
		public void OnPointerExit(PointerEventData eventData)
		{
			this.hoveringIndicator.SetActive(false);
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x0007A16C File Offset: 0x0007836C
		public bool IsEditable()
		{
			return this.TargetItem != null;
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x0007A17A File Offset: 0x0007837A
		public Item GetItem()
		{
			return this.TargetItem;
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x0007A182 File Offset: 0x00078382
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x0400178E RID: 6030
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x0400178F RID: 6031
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x04001790 RID: 6032
		[SerializeField]
		private int index;

		// Token: 0x04001791 RID: 6033
		[SerializeField]
		private InputIndicator indicator;

		// Token: 0x04001792 RID: 6034
		private Item displayingItem;

		// Token: 0x04001793 RID: 6035
		private bool dirty;
	}
}
