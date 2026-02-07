using System;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x020003AC RID: 940
	public class InventoryEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler, IDropHandler, IItemDragSource, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060020FC RID: 8444 RVA: 0x00073A8E File Offset: 0x00071C8E
		// (set) Token: 0x060020FD RID: 8445 RVA: 0x00073A96 File Offset: 0x00071C96
		public InventoryDisplay Master { get; private set; }

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060020FE RID: 8446 RVA: 0x00073A9F File Offset: 0x00071C9F
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x00073AA7 File Offset: 0x00071CA7
		// (set) Token: 0x06002100 RID: 8448 RVA: 0x00073AAF File Offset: 0x00071CAF
		public bool Disabled
		{
			get
			{
				return this.disabled;
			}
			set
			{
				this.disabled = value;
				this.Refresh();
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06002101 RID: 8449 RVA: 0x00073AC0 File Offset: 0x00071CC0
		public Item Content
		{
			get
			{
				InventoryDisplay master = this.Master;
				Inventory inventory = (master != null) ? master.Target : null;
				if (inventory == null)
				{
					return null;
				}
				if (this.index >= inventory.Capacity)
				{
					return null;
				}
				InventoryDisplay master2 = this.Master;
				if (master2 == null)
				{
					return null;
				}
				Inventory target = master2.Target;
				if (target == null)
				{
					return null;
				}
				return target.GetItemAt(this.index);
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x00073B20 File Offset: 0x00071D20
		public bool ShouldHighlight
		{
			get
			{
				return !(this.Master == null) && !(this.Content == null) && (this.Master.EvaluateShouldHighlight(this.Content) || (this.Editable && ItemUIUtilities.IsGunSelected && !this.cacheContentIsGun && this.IsCaliberMatchItemSelected()));
			}
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00073B81 File Offset: 0x00071D81
		private bool IsCaliberMatchItemSelected()
		{
			return !(this.Content == null) && ItemUIUtilities.SelectedItemCaliber == this.cachedMeta.caliber;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06002104 RID: 8452 RVA: 0x00073BA8 File Offset: 0x00071DA8
		public bool CanOperate
		{
			get
			{
				return !(this.Master == null) && this.Master.Func_CanOperate(this.Content);
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002105 RID: 8453 RVA: 0x00073BD0 File Offset: 0x00071DD0
		public bool Editable
		{
			get
			{
				return !(this.Master == null) && this.Master.Editable && this.CanOperate;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002106 RID: 8454 RVA: 0x00073BF7 File Offset: 0x00071DF7
		public bool Movable
		{
			get
			{
				return !(this.Master == null) && this.Master.Movable;
			}
		}

		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x06002107 RID: 8455 RVA: 0x00073C14 File Offset: 0x00071E14
		// (remove) Token: 0x06002108 RID: 8456 RVA: 0x00073C4C File Offset: 0x00071E4C
		public event Action<InventoryEntry> onRefresh;

		// Token: 0x06002109 RID: 8457 RVA: 0x00073C84 File Offset: 0x00071E84
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayPointerClicked;
			this.itemDisplay.onDoubleClicked += this.OnDisplayDoubleClicked;
			this.itemDisplay.onReceiveDrop += this.OnDrop;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			UIInputManager.OnFastPick += this.OnFastPick;
			UIInputManager.OnDropItem += this.OnDropItemButton;
			UIInputManager.OnUseItem += this.OnUseItemButton;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00073D1C File Offset: 0x00071F1C
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
			UIInputManager.OnLockInventoryIndex += this.OnInputLockInventoryIndex;
			UIInputManager.OnShortcutInput += this.OnShortcutInput;
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00073D54 File Offset: 0x00071F54
		private void OnDisable()
		{
			this.hovering = false;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
			UIInputManager.OnLockInventoryIndex -= this.OnInputLockInventoryIndex;
			UIInputManager.OnShortcutInput -= this.OnShortcutInput;
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x00073DAD File Offset: 0x00071FAD
		private void OnShortcutInput(UIInputEventData data, int shortcutIndex)
		{
			if (!this.hovering)
			{
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			ItemShortcut.Set(shortcutIndex, this.Item);
			ItemUIUtilities.NotifyPutItem(this.Item, false);
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x00073DE0 File Offset: 0x00071FE0
		private void OnInputLockInventoryIndex(UIInputEventData data)
		{
			if (!this.hovering)
			{
				return;
			}
			this.ToggleLock();
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x00073DF1 File Offset: 0x00071FF1
		private void OnSelectionChanged()
		{
			this.highlightIndicator.SetActive(this.ShouldHighlight);
			if (ItemUIUtilities.SelectedItemDisplay == this.itemDisplay)
			{
				this.Refresh();
			}
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x00073E1C File Offset: 0x0007201C
		private void OnDestroy()
		{
			UIInputManager.OnFastPick -= this.OnFastPick;
			UIInputManager.OnDropItem -= this.OnDropItemButton;
			UIInputManager.OnUseItem -= this.OnUseItemButton;
			if (this.itemDisplay != null)
			{
				this.itemDisplay.onPointerClick -= this.OnItemDisplayPointerClicked;
				this.itemDisplay.onDoubleClicked -= this.OnDisplayDoubleClicked;
				this.itemDisplay.onReceiveDrop -= this.OnDrop;
			}
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x00073EB0 File Offset: 0x000720B0
		private void OnFastPick(UIInputEventData data)
		{
			if (data.Used)
			{
				return;
			}
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			this.Master.NotifyItemDoubleClicked(this, new PointerEventData(EventSystem.current));
			data.Use();
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x00073EEC File Offset: 0x000720EC
		private void OnDropItemButton(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			if (!this.Item.CanDrop)
			{
				return;
			}
			if (this.CanOperate)
			{
				this.Item.Drop(CharacterMainControl.Main, true);
			}
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x00073F44 File Offset: 0x00072144
		private void OnUseItemButton(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			if (!this.Item.IsUsable(CharacterMainControl.Main))
			{
				return;
			}
			if (this.CanOperate)
			{
				CharacterMainControl.Main.UseItem(this.Item);
			}
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x00073FA0 File Offset: 0x000721A0
		private void OnItemDisplayPointerClicked(ItemDisplay display, PointerEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.disabled || !this.CanOperate)
			{
				data.Use();
				return;
			}
			if (!this.Editable)
			{
				return;
			}
			if (data.button == null)
			{
				if (this.Content == null)
				{
					return;
				}
				if (Keyboard.current != null && Keyboard.current.altKey.isPressed)
				{
					data.Use();
					if (ItemUIUtilities.SelectedItem != null)
					{
						ItemUIUtilities.SelectedItem.TryPlug(this.Content, false, null, 0);
					}
					CharacterMainControl.Main.CharacterItem.TryPlug(this.Content, false, null, 0);
					return;
				}
				if (ItemUIUtilities.SelectedItem == null)
				{
					return;
				}
				if (this.Content.Stackable && ItemUIUtilities.SelectedItem != this.Content && ItemUIUtilities.SelectedItem.TypeID == this.Content.TypeID)
				{
					ItemUIUtilities.SelectedItem.CombineInto(this.Content);
					return;
				}
			}
			else if (data.button == 1 && this.Editable && this.Content != null)
			{
				ItemOperationMenu.Show(this.itemDisplay);
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000740C8 File Offset: 0x000722C8
		private void OnDisplayDoubleClicked(ItemDisplay display, PointerEventData data)
		{
			this.Master.NotifyItemDoubleClicked(this, data);
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000740D7 File Offset: 0x000722D7
		public void Setup(InventoryDisplay master, int index, bool disabled = false)
		{
			this.Master = master;
			this.index = index;
			this.disabled = disabled;
			this.Refresh();
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000740F4 File Offset: 0x000722F4
		internal void Refresh()
		{
			Item content = this.Content;
			if (content != null)
			{
				this.cachedMeta = ItemAssetsCollection.GetMetaData(content.TypeID);
				this.cacheContentIsGun = content.Tags.Contains("Gun");
			}
			else
			{
				this.cacheContentIsGun = false;
				this.cachedMeta = default(ItemMetaData);
			}
			this.itemDisplay.Setup(content);
			this.itemDisplay.CanDrop = this.CanOperate;
			this.itemDisplay.Movable = this.Movable;
			this.itemDisplay.Editable = (this.Editable && this.CanOperate);
			this.itemDisplay.CanLockSort = true;
			if (!this.Master.Target.NeedInspection && content != null)
			{
				content.Inspected = true;
			}
			this.itemDisplay.ShowOperationButtons = this.Master.ShowOperationButtons;
			this.shortcutIndicator.gameObject.SetActive(this.Master.IsShortcut(this.index));
			this.disabledIndicator.SetActive(this.disabled || !this.CanOperate);
			this.highlightIndicator.SetActive(this.ShouldHighlight);
			bool active = this.Master.Target.IsIndexLocked(this.Index);
			this.lockIndicator.SetActive(active);
			Action<InventoryEntry> action = this.onRefresh;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002117 RID: 8471 RVA: 0x00074260 File Offset: 0x00072460
		public static PrefabPool<InventoryEntry> Pool
		{
			get
			{
				return GameplayUIManager.Instance.InventoryEntryPool;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002118 RID: 8472 RVA: 0x0007426C File Offset: 0x0007246C
		public Item Item
		{
			get
			{
				if (this.itemDisplay != null && this.itemDisplay.isActiveAndEnabled)
				{
					return this.itemDisplay.Target;
				}
				return null;
			}
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x00074296 File Offset: 0x00072496
		public static InventoryEntry Get()
		{
			return InventoryEntry.Pool.Get(null);
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000742A3 File Offset: 0x000724A3
		public static void Release(InventoryEntry item)
		{
			InventoryEntry.Pool.Release(item);
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000742B0 File Offset: 0x000724B0
		public void NotifyPooled()
		{
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000742B2 File Offset: 0x000724B2
		public void NotifyReleased()
		{
			this.Master = null;
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x000742BC File Offset: 0x000724BC
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Punch();
			if (eventData.button == null)
			{
				this.lastClickTime = eventData.clickTime;
				if (this.Editable)
				{
					Item selectedItem = ItemUIUtilities.SelectedItem;
					if (!(selectedItem == null))
					{
						if (this.Content != null)
						{
							Debug.Log(string.Format("{0}(Inventory) 的 {1} 已经有物品。操作已取消。", this.Master.Target.name, this.index));
						}
						else
						{
							eventData.Use();
							selectedItem.Detach();
							this.Master.Target.AddAt(selectedItem, this.index);
							ItemUIUtilities.NotifyPutItem(selectedItem, false);
						}
					}
				}
				this.lastClickTime = eventData.clickTime;
			}
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x0007436E File Offset: 0x0007256E
		internal void Punch()
		{
			this.itemDisplay.Punch();
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x0007437B File Offset: 0x0007257B
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x00074380 File Offset: 0x00072580
		public void OnDrop(PointerEventData eventData)
		{
			if (eventData.used)
			{
				return;
			}
			if (!this.Editable)
			{
				return;
			}
			if (eventData.button != null)
			{
				return;
			}
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
			if (item.Sticky && !this.Master.Target.AcceptSticky)
			{
				return;
			}
			if (Keyboard.current != null && Keyboard.current.ctrlKey.isPressed)
			{
				if (this.Content != null)
				{
					NotificationText.Push(LocalizationManager.ToPlainText("UI_Inventory_TargetOccupiedCannotSplit"));
					return;
				}
				Debug.Log("SPLIT");
				SplitDialogue.SetupAndShow(item, this.Master.Target, this.index);
				return;
			}
			else
			{
				ItemUIUtilities.NotifyPutItem(item, false);
				if (this.Content == null)
				{
					item.Detach();
					this.Master.Target.AddAt(item, this.index);
					return;
				}
				if (this.Content.TypeID == item.TypeID && this.Content.Stackable)
				{
					this.Content.Combine(item);
					return;
				}
				Inventory inInventory = item.InInventory;
				Inventory target = this.Master.Target;
				if (inInventory != null)
				{
					int num = inInventory.GetIndex(item);
					int num2 = this.index;
					Item content = this.Content;
					if (content != item)
					{
						item.Detach();
						content.Detach();
						inInventory.AddAt(content, num);
						target.AddAt(item, num2);
					}
				}
				return;
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x0007450F File Offset: 0x0007270F
		public bool IsEditable()
		{
			return !(this.Content == null) && !this.Content.NeedInspection && this.Editable;
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00074536 File Offset: 0x00072736
		public Item GetItem()
		{
			return this.Content;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x0007453E File Offset: 0x0007273E
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hovering = true;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(this.Editable);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x0007455D File Offset: 0x0007275D
		public void OnPointerExit(PointerEventData eventData)
		{
			this.hovering = false;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(false);
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00074577 File Offset: 0x00072777
		public void ToggleLock()
		{
			this.Master.Target.ToggleLockIndex(this.Index);
		}

		// Token: 0x04001687 RID: 5767
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04001688 RID: 5768
		[SerializeField]
		private GameObject shortcutIndicator;

		// Token: 0x04001689 RID: 5769
		[SerializeField]
		private GameObject disabledIndicator;

		// Token: 0x0400168A RID: 5770
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x0400168B RID: 5771
		[SerializeField]
		private GameObject highlightIndicator;

		// Token: 0x0400168C RID: 5772
		[SerializeField]
		private GameObject lockIndicator;

		// Token: 0x0400168E RID: 5774
		[SerializeField]
		private int index;

		// Token: 0x0400168F RID: 5775
		[SerializeField]
		private bool disabled;

		// Token: 0x04001691 RID: 5777
		private bool cacheContentIsGun;

		// Token: 0x04001692 RID: 5778
		private ItemMetaData cachedMeta;

		// Token: 0x04001693 RID: 5779
		public const float doubleClickTimeThreshold = 0.3f;

		// Token: 0x04001694 RID: 5780
		private float lastClickTime;

		// Token: 0x04001695 RID: 5781
		private bool hovering;
	}
}
