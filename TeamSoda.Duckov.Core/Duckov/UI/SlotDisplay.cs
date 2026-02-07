using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B9 RID: 953
	public class SlotDisplay : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler, IItemDragSource, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x140000EA RID: 234
		// (add) Token: 0x060021F5 RID: 8693 RVA: 0x00076F68 File Offset: 0x00075168
		// (remove) Token: 0x060021F6 RID: 8694 RVA: 0x00076FA0 File Offset: 0x000751A0
		public event Action<SlotDisplay> onSlotDisplayClicked;

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x060021F7 RID: 8695 RVA: 0x00076FD8 File Offset: 0x000751D8
		// (remove) Token: 0x060021F8 RID: 8696 RVA: 0x00077010 File Offset: 0x00075210
		public event Action<SlotDisplay> onSlotDisplayDoubleClicked;

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x00077045 File Offset: 0x00075245
		// (set) Token: 0x060021FA RID: 8698 RVA: 0x0007704D File Offset: 0x0007524D
		public bool Editable
		{
			get
			{
				return this.editable;
			}
			set
			{
				this.editable = value;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060021FB RID: 8699 RVA: 0x00077056 File Offset: 0x00075256
		// (set) Token: 0x060021FC RID: 8700 RVA: 0x0007705E File Offset: 0x0007525E
		public bool ContentSelectable
		{
			get
			{
				return this.contentSelectable;
			}
			set
			{
				this.contentSelectable = value;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060021FD RID: 8701 RVA: 0x00077067 File Offset: 0x00075267
		// (set) Token: 0x060021FE RID: 8702 RVA: 0x0007706F File Offset: 0x0007526F
		public bool ShowOperationMenu
		{
			get
			{
				return this.showOperationMenu;
			}
			set
			{
				this.showOperationMenu = value;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x00077078 File Offset: 0x00075278
		// (set) Token: 0x06002200 RID: 8704 RVA: 0x00077080 File Offset: 0x00075280
		public Slot Target { get; private set; }

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x06002201 RID: 8705 RVA: 0x0007708C File Offset: 0x0007528C
		// (remove) Token: 0x06002202 RID: 8706 RVA: 0x000770C0 File Offset: 0x000752C0
		public static event Action<SlotDisplayOperationContext> onOperation;

		// Token: 0x06002203 RID: 8707 RVA: 0x000770F4 File Offset: 0x000752F4
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.Target != null)
			{
				this.Target.onSlotContentChanged += this.OnTargetContentChanged;
			}
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
			this.itemDisplay.onPointerClick += this.OnItemDisplayClicked;
			this.itemDisplay.onDoubleClicked += this.OnItemDisplayDoubleClicked;
			IItemDragSource.OnStartDragItem += this.OnStartDragItem;
			IItemDragSource.OnEndDragItem += this.OnEndDragItem;
			UIInputManager.OnFastPick += this.OnFastPick;
			UIInputManager.OnDropItem += this.OnFastDrop;
			UIInputManager.OnUseItem += this.OnFastUse;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000771C4 File Offset: 0x000753C4
		private void UnregisterEvents()
		{
			if (this.Target != null)
			{
				this.Target.onSlotContentChanged -= this.OnTargetContentChanged;
			}
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
			this.itemDisplay.onPointerClick -= this.OnItemDisplayClicked;
			this.itemDisplay.onDoubleClicked -= this.OnItemDisplayDoubleClicked;
			IItemDragSource.OnStartDragItem -= this.OnStartDragItem;
			IItemDragSource.OnEndDragItem -= this.OnEndDragItem;
			UIInputManager.OnFastPick -= this.OnFastPick;
			UIInputManager.OnDropItem -= this.OnFastDrop;
			UIInputManager.OnUseItem -= this.OnFastUse;
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00077284 File Offset: 0x00075484
		private void OnFastDrop(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Content == null)
			{
				return;
			}
			if (!this.Target.Content.CanDrop)
			{
				return;
			}
			if (this.Editable)
			{
				this.Target.Content.Drop(CharacterMainControl.Main, true);
			}
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000772F4 File Offset: 0x000754F4
		private void OnFastUse(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Content == null)
			{
				return;
			}
			if (!this.Target.Content.IsUsable(CharacterMainControl.Main))
			{
				return;
			}
			CharacterMainControl.Main.UseItem(this.Target.Content);
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x0007735D File Offset: 0x0007555D
		private void OnFastPick(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.hovering)
			{
				return;
			}
			this.OnItemDisplayDoubleClicked(this.itemDisplay, new PointerEventData(EventSystem.current));
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x00077387 File Offset: 0x00075587
		private void OnEndDragItem(Item item)
		{
			this.pluggableIndicator.Hide();
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x00077394 File Offset: 0x00075594
		private void OnStartDragItem(Item item)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (!this.Editable)
			{
				return;
			}
			if (item != this.Target.Content && this.Target.CanPlug(item))
			{
				this.pluggableIndicator.Show();
				return;
			}
			this.pluggableIndicator.Hide();
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x000773EB File Offset: 0x000755EB
		private void OnItemDisplayDoubleClicked(ItemDisplay arg1, PointerEventData arg2)
		{
			Action<SlotDisplay> action = this.onSlotDisplayDoubleClicked;
			if (action != null)
			{
				action(this);
			}
			if (!this.ContentSelectable)
			{
				arg2.Use();
			}
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x00077410 File Offset: 0x00075610
		private void OnItemDisplayClicked(ItemDisplay display, PointerEventData data)
		{
			Action<SlotDisplay> action = this.onSlotDisplayClicked;
			if (action != null)
			{
				action(this);
			}
			if (data.button == null)
			{
				if (Keyboard.current != null && Keyboard.current.altKey.isPressed)
				{
					if (this.Editable && this.Target.Content != null)
					{
						Item content = this.Target.Content;
						content.Detach();
						if (!ItemUtilities.SendToPlayerCharacterInventory(content, false))
						{
							if (PlayerStorage.IsAccessableAndNotFull())
							{
								ItemUtilities.SendToPlayerStorage(content, false);
							}
							else
							{
								ItemUtilities.SendToPlayer(content, false, false);
							}
						}
						data.Use();
						return;
					}
				}
				else if (!this.ContentSelectable)
				{
					data.Use();
					return;
				}
			}
			else if (data.button == 1 && this.Editable)
			{
				Slot target = this.Target;
				if (((target != null) ? target.Content : null) != null)
				{
					ItemOperationMenu.Show(this.itemDisplay);
				}
			}
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000774EC File Offset: 0x000756EC
		private void OnTargetContentChanged(Slot slot)
		{
			this.Refresh();
			this.Punch();
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000774FA File Offset: 0x000756FA
		private void OnItemSelectionChanged()
		{
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000774FC File Offset: 0x000756FC
		public void Setup(Slot target)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.label.text = target.DisplayName;
			this.Refresh();
			this.RegisterEvents();
			this.pluggableIndicator.Hide();
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x00077534 File Offset: 0x00075734
		private void Refresh()
		{
			if (this.Target.Content == null)
			{
				this.slotIcon.gameObject.SetActive(true);
				if (this.Target.SlotIcon != null)
				{
					this.slotIcon.sprite = this.Target.SlotIcon;
				}
				else
				{
					this.slotIcon.sprite = this.defaultSlotIcon;
				}
			}
			else
			{
				this.slotIcon.gameObject.SetActive(false);
			}
			this.itemDisplay.ShowOperationButtons = this.showOperationMenu;
			this.itemDisplay.Setup(this.Target.Content);
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x000775DB File Offset: 0x000757DB
		public static PrefabPool<SlotDisplay> Pool
		{
			get
			{
				return GameplayUIManager.Instance.SlotDisplayPool;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x000775E7 File Offset: 0x000757E7
		// (set) Token: 0x06002212 RID: 8722 RVA: 0x000775F4 File Offset: 0x000757F4
		public bool Movable
		{
			get
			{
				return this.itemDisplay.Movable;
			}
			set
			{
				this.itemDisplay.Movable = value;
			}
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x00077602 File Offset: 0x00075802
		public static SlotDisplay Get()
		{
			return SlotDisplay.Pool.Get(null);
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x0007760F File Offset: 0x0007580F
		public static void Release(SlotDisplay item)
		{
			SlotDisplay.Pool.Release(item);
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x0007761C File Offset: 0x0007581C
		public void NotifyPooled()
		{
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x0007761E File Offset: 0x0007581E
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.Target = null;
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x0007762D File Offset: 0x0007582D
		private void Awake()
		{
			this.itemDisplay.onReceiveDrop += this.OnDrop;
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x00077647 File Offset: 0x00075847
		private void OnEnable()
		{
			this.RegisterEvents();
			this.iconInitialColor = this.slotIcon.color;
			GameObject gameObject = this.hoveringIndicator;
			if (gameObject == null)
			{
				return;
			}
			gameObject.SetActive(false);
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x00077671 File Offset: 0x00075871
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x0007767C File Offset: 0x0007587C
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<SlotDisplay> action = this.onSlotDisplayClicked;
			if (action != null)
			{
				action(this);
			}
			if (!this.Editable)
			{
				this.Punch();
				eventData.Use();
				return;
			}
			if (eventData.button == null)
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				if (selectedItem == null)
				{
					this.Punch();
					return;
				}
				if (this.Target.Content != null)
				{
					Debug.Log("槽位 " + this.Target.DisplayName + " 中已经有物品。操作已取消。");
					this.DenialPunch();
					return;
				}
				if (!this.Target.CanPlug(selectedItem))
				{
					Debug.Log(string.Concat(new string[]
					{
						"物品 ",
						selectedItem.DisplayName,
						" 未通过槽位 ",
						this.Target.DisplayName,
						" 安装检测。操作已取消。"
					}));
					this.DenialPunch();
					return;
				}
				eventData.Use();
				selectedItem.Detach();
				Item item;
				this.Target.Plug(selectedItem, ref item);
				ItemUIUtilities.NotifyPutItem(selectedItem, false);
				if (item != null)
				{
					ItemUIUtilities.RaiseOrphan(item);
				}
				this.Punch();
			}
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00077798 File Offset: 0x00075998
		public void Punch()
		{
			if (this.slotIcon != null)
			{
				ShortcutExtensions.DOKill(this.slotIcon.transform, false);
				this.slotIcon.color = this.iconInitialColor;
				this.slotIcon.transform.localScale = Vector3.one;
				ShortcutExtensions.DOPunchScale(this.slotIcon.transform, Vector3.one * this.slotIconPunchScale, this.punchDuration, 10, 1f);
			}
			if (this.itemDisplay != null)
			{
				this.itemDisplay.Punch();
			}
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x00077834 File Offset: 0x00075A34
		public void DenialPunch()
		{
			if (this.slotIcon == null)
			{
				return;
			}
			ShortcutExtensions.DOKill(this.slotIcon.transform, false);
			this.slotIcon.color = this.iconInitialColor;
			TweenSettingsExtensions.From<TweenerCore<Color, Color, ColorOptions>>(DOTweenModuleUI.DOColor(this.slotIcon, this.slotIconDenialColor, this.denialPunchDuration));
			Action<SlotDisplayOperationContext> action = SlotDisplay.onOperation;
			if (action == null)
			{
				return;
			}
			action(new SlotDisplayOperationContext(this, SlotDisplayOperationContext.Operation.Deny, false));
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000778A7 File Offset: 0x00075AA7
		public bool IsEditable()
		{
			return this.Editable;
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000778AF File Offset: 0x00075AAF
		public Item GetItem()
		{
			Slot target = this.Target;
			if (target == null)
			{
				return null;
			}
			return target.Content;
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000778C4 File Offset: 0x00075AC4
		public void OnDrop(PointerEventData eventData)
		{
			if (!this.Editable)
			{
				return;
			}
			if (eventData.used)
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
			if (this.SetAmmo(item))
			{
				return;
			}
			if (!this.Target.CanPlug(item))
			{
				Debug.Log(string.Concat(new string[]
				{
					"物品 ",
					item.DisplayName,
					" 未通过槽位 ",
					this.Target.DisplayName,
					" 安装检测。操作已取消。"
				}));
				this.DenialPunch();
				return;
			}
			Inventory inInventory = item.InInventory;
			Slot pluggedIntoSlot = item.PluggedIntoSlot;
			if (pluggedIntoSlot == this.Target)
			{
				return;
			}
			ItemUIUtilities.NotifyPutItem(item, false);
			Item item2;
			bool succeed = this.Target.Plug(item, ref item2);
			if (item2 != null && (!(inInventory != null) || !inInventory.AddAndMerge(item2, 0)))
			{
				Item item3;
				if (pluggedIntoSlot != null && pluggedIntoSlot.CanPlug(item2) && pluggedIntoSlot.Plug(item2, ref item3))
				{
					if (item3)
					{
						Debug.LogError("Source slot spit out an unplugged item! " + item3.DisplayName);
					}
				}
				else if (!ItemUtilities.SendToPlayerCharacter(item2, false))
				{
					LootView lootView = View.ActiveView as LootView;
					if (lootView == null || !(lootView.TargetInventory != null) || !lootView.TargetInventory.AddAndMerge(item2, 0))
					{
						if (PlayerStorage.IsAccessableAndNotFull())
						{
							ItemUtilities.SendToPlayerStorage(item2, false);
						}
						else
						{
							item2.Drop(CharacterMainControl.Main, true);
						}
					}
				}
			}
			Action<SlotDisplayOperationContext> action = SlotDisplay.onOperation;
			if (action == null)
			{
				return;
			}
			action(new SlotDisplayOperationContext(this, SlotDisplayOperationContext.Operation.Equip, succeed));
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x00077A79 File Offset: 0x00075C79
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00077A7B File Offset: 0x00075C7B
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

		// Token: 0x06002222 RID: 8738 RVA: 0x00077A9A File Offset: 0x00075C9A
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

		// Token: 0x06002223 RID: 8739 RVA: 0x00077AB4 File Offset: 0x00075CB4
		private bool SetAmmo(Item incomming)
		{
			Slot target = this.Target;
			ItemSetting_Gun itemSetting_Gun;
			if (target == null)
			{
				itemSetting_Gun = null;
			}
			else
			{
				Item content = target.Content;
				itemSetting_Gun = ((content != null) ? content.GetComponent<ItemSetting_Gun>() : null);
			}
			ItemSetting_Gun itemSetting_Gun2 = itemSetting_Gun;
			if (itemSetting_Gun2 == null)
			{
				return false;
			}
			if (!itemSetting_Gun2.IsValidBullet(incomming))
			{
				return false;
			}
			if (View.ActiveView is InventoryView || View.ActiveView is LootView)
			{
				View.ActiveView.Close();
			}
			return itemSetting_Gun2.LoadSpecificBullet(incomming);
		}

		// Token: 0x0400171F RID: 5919
		[SerializeField]
		private Sprite defaultSlotIcon;

		// Token: 0x04001720 RID: 5920
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04001721 RID: 5921
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04001722 RID: 5922
		[SerializeField]
		private Image slotIcon;

		// Token: 0x04001723 RID: 5923
		[SerializeField]
		private FadeGroup pluggableIndicator;

		// Token: 0x04001724 RID: 5924
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x04001725 RID: 5925
		[SerializeField]
		private bool editable = true;

		// Token: 0x04001726 RID: 5926
		[SerializeField]
		private bool showOperationMenu = true;

		// Token: 0x04001727 RID: 5927
		[SerializeField]
		private bool contentSelectable = true;

		// Token: 0x0400172A RID: 5930
		[SerializeField]
		[Range(0f, 1f)]
		private float punchDuration = 0.1f;

		// Token: 0x0400172B RID: 5931
		[SerializeField]
		[Range(-1f, 1f)]
		private float slotIconPunchScale = -0.1f;

		// Token: 0x0400172C RID: 5932
		[SerializeField]
		[Range(0f, 1f)]
		private float denialPunchDuration = 0.2f;

		// Token: 0x0400172D RID: 5933
		[SerializeField]
		private Color slotIconDenialColor = Color.red;

		// Token: 0x0400172E RID: 5934
		private Color iconInitialColor;

		// Token: 0x04001731 RID: 5937
		private bool hovering;
	}
}
