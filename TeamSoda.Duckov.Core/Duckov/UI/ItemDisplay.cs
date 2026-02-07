using System;
using DG.Tweening;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B6 RID: 950
	public class ItemDisplay : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
	{
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x0600217D RID: 8573 RVA: 0x000754CE File Offset: 0x000736CE
		private Sprite FallbackIcon
		{
			get
			{
				return GameplayDataSettings.UIStyle.FallbackItemIcon;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x000754DA File Offset: 0x000736DA
		// (set) Token: 0x0600217F RID: 8575 RVA: 0x000754E2 File Offset: 0x000736E2
		public Item Target { get; private set; }

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x000754EB File Offset: 0x000736EB
		// (set) Token: 0x06002181 RID: 8577 RVA: 0x000754F3 File Offset: 0x000736F3
		internal Action releaseAction { get; set; }

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06002182 RID: 8578 RVA: 0x000754FC File Offset: 0x000736FC
		// (remove) Token: 0x06002183 RID: 8579 RVA: 0x00075534 File Offset: 0x00073734
		internal event Action<ItemDisplay, PointerEventData> onDoubleClicked;

		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x06002184 RID: 8580 RVA: 0x0007556C File Offset: 0x0007376C
		// (remove) Token: 0x06002185 RID: 8581 RVA: 0x000755A4 File Offset: 0x000737A4
		public event Action<PointerEventData> onReceiveDrop;

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x000755D9 File Offset: 0x000737D9
		public bool Selected
		{
			get
			{
				return ItemUIUtilities.SelectedItemDisplay == this;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x000755E8 File Offset: 0x000737E8
		private PrefabPool<SlotIndicator> SlotIndicatorPool
		{
			get
			{
				if (this._slotIndicatorPool == null)
				{
					if (this.slotIndicatorTemplate == null)
					{
						Debug.LogError("SI is null", base.gameObject);
					}
					this._slotIndicatorPool = new PrefabPool<SlotIndicator>(this.slotIndicatorTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._slotIndicatorPool;
			}
		}

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06002188 RID: 8584 RVA: 0x00075640 File Offset: 0x00073840
		// (remove) Token: 0x06002189 RID: 8585 RVA: 0x00075674 File Offset: 0x00073874
		public static event Action<ItemDisplay> OnPointerEnterItemDisplay;

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x0600218A RID: 8586 RVA: 0x000756A8 File Offset: 0x000738A8
		// (remove) Token: 0x0600218B RID: 8587 RVA: 0x000756DC File Offset: 0x000738DC
		public static event Action<ItemDisplay> OnPointerExitItemDisplay;

		// Token: 0x0600218C RID: 8588 RVA: 0x00075710 File Offset: 0x00073910
		public void Setup(Item target)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.Clear();
			this.slotIndicatorTemplate.gameObject.SetActive(false);
			if (target == null)
			{
				this.SetupEmpty();
			}
			else
			{
				this.icon.color = Color.white;
				this.icon.sprite = target.Icon;
				if (this.icon.sprite == null)
				{
					this.icon.sprite = this.FallbackIcon;
				}
				this.icon.gameObject.SetActive(true);
				ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(target.DisplayQuality);
				this.displayQualityShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
				this.displayQualityShadow.Color = shadowOffsetAndColorOfQuality.Item2;
				this.displayQualityShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
				bool stackable = this.Target.Stackable;
				this.countGameObject.SetActive(stackable);
				this.nameText.text = this.Target.DisplayName;
				if (target.Slots != null)
				{
					foreach (Slot target2 in target.Slots)
					{
						this.SlotIndicatorPool.Get(null).Setup(target2);
					}
				}
			}
			this.Refresh();
			if (base.isActiveAndEnabled)
			{
				this.RegisterEvents();
			}
		}

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x0600218D RID: 8589 RVA: 0x0007588C File Offset: 0x00073A8C
		// (remove) Token: 0x0600218E RID: 8590 RVA: 0x000758C4 File Offset: 0x00073AC4
		public event Action<ItemDisplay, PointerEventData> onPointerClick;

		// Token: 0x0600218F RID: 8591 RVA: 0x000758FC File Offset: 0x00073AFC
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			ItemUIUtilities.OnSelectionChanged += this.OnItemUtilitiesSelectionChanged;
			ItemWishlist.OnWishlistChanged += this.OnWishlistChanged;
			if (this.Target == null)
			{
				return;
			}
			this.Target.onDestroy += this.OnTargetDestroy;
			this.Target.onSetStackCount += this.OnTargetSetStackCount;
			this.Target.onInspectionStateChanged += this.OnTargetInspectionStateChanged;
			this.Target.onDurabilityChanged += this.OnTargetDurabilityChanged;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0007599C File Offset: 0x00073B9C
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUtilitiesSelectionChanged;
			ItemWishlist.OnWishlistChanged -= this.OnWishlistChanged;
			if (this.Target == null)
			{
				return;
			}
			this.Target.onDestroy -= this.OnTargetDestroy;
			this.Target.onSetStackCount -= this.OnTargetSetStackCount;
			this.Target.onInspectionStateChanged -= this.OnTargetInspectionStateChanged;
			this.Target.onDurabilityChanged -= this.OnTargetDurabilityChanged;
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x00075A36 File Offset: 0x00073C36
		private void OnWishlistChanged(int type)
		{
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.TypeID == type)
			{
				this.RefreshWishlistInfo();
			}
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x00075A5B File Offset: 0x00073C5B
		private void OnTargetDurabilityChanged(Item item)
		{
			this.Refresh();
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x00075A63 File Offset: 0x00073C63
		private void OnTargetDestroy(Item item)
		{
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x00075A65 File Offset: 0x00073C65
		private void OnTargetSetStackCount(Item item)
		{
			if (item != this.Target)
			{
				Debug.LogError("触发事件的Item不匹配!");
			}
			this.Refresh();
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x00075A85 File Offset: 0x00073C85
		private void OnItemUtilitiesSelectionChanged()
		{
			this.Refresh();
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x00075A8D File Offset: 0x00073C8D
		private void OnTargetInspectionStateChanged(Item item)
		{
			this.Refresh();
			this.Punch();
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x00075A9B File Offset: 0x00073C9B
		private void Clear()
		{
			this.SlotIndicatorPool.ReleaseAll();
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x00075AA8 File Offset: 0x00073CA8
		private void SetupEmpty()
		{
			this.icon.sprite = EmptySprite.Get();
			this.icon.color = Color.clear;
			this.countText.text = string.Empty;
			this.nameText.text = string.Empty;
			this.durabilityFill.fillAmount = 0f;
			this.durabilityLoss.fillAmount = 0f;
			this.durabilityZeroIndicator.gameObject.SetActive(false);
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x00075B28 File Offset: 0x00073D28
		private void Refresh()
		{
			if (this == null)
			{
				Debug.Log("NULL");
				return;
			}
			if (this.isBeingDestroyed)
			{
				return;
			}
			if (this.Target == null)
			{
				this.HideMainContentAndDisableControl();
				this.HideInspectionElements();
				if (ItemUIUtilities.SelectedItemDisplayRaw == this)
				{
					ItemUIUtilities.Select(null);
				}
			}
			else if (this.Target.NeedInspection)
			{
				this.HideMainContentAndDisableControl();
				this.ShowInspectionElements();
			}
			else
			{
				this.HideInspectionElements();
				this.ShowMainContentAndEnableControl();
			}
			this.selectionIndicator.gameObject.SetActive(this.Selected);
			this.RefreshWishlistInfo();
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x00075BC4 File Offset: 0x00073DC4
		private void RefreshWishlistInfo()
		{
			if (this.Target == null || this.Target.NeedInspection)
			{
				this.wishlistedIndicator.SetActive(false);
				this.questRequiredIndicator.SetActive(false);
				this.buildingRequiredIndicator.SetActive(false);
				return;
			}
			ItemWishlist.WishlistInfo wishlistInfo = ItemWishlist.GetWishlistInfo(this.Target.TypeID);
			this.wishlistedIndicator.SetActive(wishlistInfo.isManuallyWishlisted);
			this.questRequiredIndicator.SetActive(wishlistInfo.isQuestRequired);
			this.buildingRequiredIndicator.SetActive(wishlistInfo.isBuildingRequired);
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x00075C58 File Offset: 0x00073E58
		private void HideMainContentAndDisableControl()
		{
			this.mainContentShown = false;
			if (this.mainContentShown && ItemUIUtilities.SelectedItemDisplay == this)
			{
				ItemUIUtilities.Select(null);
			}
			this.interactionEventReceiver.raycastTarget = false;
			this.icon.gameObject.SetActive(false);
			this.countGameObject.SetActive(false);
			this.durabilityGameObject.SetActive(false);
			this.durabilityZeroIndicator.gameObject.SetActive(false);
			this.nameContainer.SetActive(false);
			this.slotIndicatorContainer.SetActive(false);
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x00075CE8 File Offset: 0x00073EE8
		private void ShowMainContentAndEnableControl()
		{
			this.mainContentShown = true;
			this.interactionEventReceiver.raycastTarget = true;
			this.icon.gameObject.SetActive(true);
			this.nameContainer.SetActive(true);
			this.countText.text = (this.Target.Stackable ? this.Target.StackCount.ToString() : string.Empty);
			bool useDurability = this.Target.UseDurability;
			if (useDurability)
			{
				float num = this.Target.Durability / this.Target.MaxDurability;
				this.durabilityFill.fillAmount = num;
				this.durabilityFill.color = this.durabilityFillColorOverT.Evaluate(num);
				this.durabilityZeroIndicator.SetActive(this.Target.Durability <= 0f);
				this.durabilityLoss.fillAmount = this.Target.DurabilityLoss;
			}
			else
			{
				this.durabilityZeroIndicator.gameObject.SetActive(false);
			}
			this.countGameObject.SetActive(this.Target.Stackable);
			this.durabilityGameObject.SetActive(useDurability);
			this.slotIndicatorContainer.SetActive(true);
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x00075E18 File Offset: 0x00074018
		private void ShowInspectionElements()
		{
			this.inspectionElementRoot.gameObject.SetActive(true);
			bool inspecting = this.Target.Inspecting;
			if (this.inspectingElement)
			{
				this.inspectingElement.SetActive(inspecting);
			}
			if (this.notInspectingElement)
			{
				this.notInspectingElement.SetActive(!inspecting);
			}
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x00075E77 File Offset: 0x00074077
		private void HideInspectionElements()
		{
			this.inspectionElementRoot.gameObject.SetActive(false);
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x00075E8A File Offset: 0x0007408A
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x00075E92 File Offset: 0x00074092
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUtilitiesSelectionChanged;
			if (this.Selected)
			{
				ItemUIUtilities.Select(null);
			}
			this.UnregisterEvents();
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x00075EB9 File Offset: 0x000740B9
		private void OnDestroy()
		{
			this.UnregisterEvents();
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUtilitiesSelectionChanged;
			this.isBeingDestroyed = true;
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060021A2 RID: 8610 RVA: 0x00075ED9 File Offset: 0x000740D9
		public static PrefabPool<ItemDisplay> Pool
		{
			get
			{
				return GameplayUIManager.Instance.ItemDisplayPool;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x00075EE5 File Offset: 0x000740E5
		// (set) Token: 0x060021A4 RID: 8612 RVA: 0x00075EED File Offset: 0x000740ED
		public bool ShowOperationButtons
		{
			get
			{
				return this.showOperationButtons;
			}
			internal set
			{
				this.showOperationButtons = value;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x00075EF6 File Offset: 0x000740F6
		// (set) Token: 0x060021A6 RID: 8614 RVA: 0x00075EFE File Offset: 0x000740FE
		public bool Editable { get; set; }

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060021A7 RID: 8615 RVA: 0x00075F07 File Offset: 0x00074107
		// (set) Token: 0x060021A8 RID: 8616 RVA: 0x00075F0F File Offset: 0x0007410F
		public bool Movable { get; set; }

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x00075F18 File Offset: 0x00074118
		// (set) Token: 0x060021AA RID: 8618 RVA: 0x00075F20 File Offset: 0x00074120
		public bool CanDrop { get; set; }

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060021AB RID: 8619 RVA: 0x00075F29 File Offset: 0x00074129
		// (set) Token: 0x060021AC RID: 8620 RVA: 0x00075F31 File Offset: 0x00074131
		public bool IsStockshopSample { get; set; }

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x00075F3A File Offset: 0x0007413A
		public bool CanUse
		{
			get
			{
				return !(this.Target == null) && this.Editable && this.Target.IsUsable(CharacterMainControl.Main);
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x00075F6B File Offset: 0x0007416B
		public bool CanSplit
		{
			get
			{
				return !(this.Target == null) && this.Editable && (this.Movable && this.Target.StackCount > 1);
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x00075FA0 File Offset: 0x000741A0
		// (set) Token: 0x060021B0 RID: 8624 RVA: 0x00075FA8 File Offset: 0x000741A8
		public bool CanLockSort { get; internal set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060021B1 RID: 8625 RVA: 0x00075FB1 File Offset: 0x000741B1
		public bool CanSetShortcut
		{
			get
			{
				return !(this.Target == null) && this.showOperationButtons && ItemShortcut.IsItemValid(this.Target);
			}
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x00075FDD File Offset: 0x000741DD
		public static ItemDisplay Get()
		{
			return ItemDisplay.Pool.Get(null);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x00075FEA File Offset: 0x000741EA
		public static void Release(ItemDisplay item)
		{
			ItemDisplay.Pool.Release(item);
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00075FF7 File Offset: 0x000741F7
		public void NotifyPooled()
		{
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x00075FF9 File Offset: 0x000741F9
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.Target = null;
			this.SetupEmpty();
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x0007600E File Offset: 0x0007420E
		[ContextMenu("Select")]
		private void Select()
		{
			ItemUIUtilities.Select(this);
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x00076016 File Offset: 0x00074216
		public void NotifySelected()
		{
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x00076018 File Offset: 0x00074218
		public void NotifyUnselected()
		{
			KontextMenu.Hide(this);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x00076020 File Offset: 0x00074220
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<ItemDisplay, PointerEventData> action = this.onPointerClick;
			if (action != null)
			{
				action(this, eventData);
			}
			if (!eventData.used && eventData.button == null)
			{
				if (eventData.clickTime - this.lastClickTime <= 0.3f && !this.doubleClickInvoked)
				{
					this.doubleClickInvoked = true;
					Action<ItemDisplay, PointerEventData> action2 = this.onDoubleClicked;
					if (action2 != null)
					{
						action2(this, eventData);
					}
				}
				if (!eventData.used && (!this.Target || !this.Target.NeedInspection))
				{
					if (ItemUIUtilities.SelectedItemDisplay != this)
					{
						this.Select();
						eventData.Use();
					}
					else
					{
						ItemUIUtilities.Select(null);
						eventData.Use();
					}
				}
			}
			if (eventData.clickTime - this.lastClickTime > 0.3f)
			{
				this.doubleClickInvoked = false;
			}
			this.lastClickTime = eventData.clickTime;
			this.Punch();
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x00076100 File Offset: 0x00074300
		public void Punch()
		{
			ShortcutExtensions.DOKill(this.selectionIndicator.transform, false);
			ShortcutExtensions.DOKill(this.icon.transform, false);
			ShortcutExtensions.DOKill(this.backgroundRing.transform, false);
			this.selectionIndicator.transform.localScale = Vector3.one;
			this.icon.transform.localScale = Vector3.one;
			this.backgroundRing.transform.localScale = Vector3.one;
			ShortcutExtensions.DOPunchScale(this.selectionIndicator.transform, Vector3.one * this.selectionRingPunchScale, this.punchDuration, 10, 1f);
			ShortcutExtensions.DOPunchScale(this.icon.transform, Vector3.one * this.iconPunchScale, this.punchDuration, 10, 1f);
			ShortcutExtensions.DOPunchScale(this.backgroundRing.transform, Vector3.one * this.backgroundRingPunchScale, this.punchDuration, 10, 1f);
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0007620C File Offset: 0x0007440C
		public void OnPointerDown(PointerEventData eventData)
		{
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x0007620E File Offset: 0x0007440E
		public void OnPointerUp(PointerEventData eventData)
		{
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x00076210 File Offset: 0x00074410
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.Target == null)
			{
				return;
			}
			Action<ItemDisplay> onPointerExitItemDisplay = ItemDisplay.OnPointerExitItemDisplay;
			if (onPointerExitItemDisplay == null)
			{
				return;
			}
			onPointerExitItemDisplay(this);
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x00076231 File Offset: 0x00074431
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.Target == null)
			{
				return;
			}
			Action<ItemDisplay> onPointerEnterItemDisplay = ItemDisplay.OnPointerEnterItemDisplay;
			if (onPointerEnterItemDisplay == null)
			{
				return;
			}
			onPointerEnterItemDisplay(this);
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x00076252 File Offset: 0x00074452
		public void OnDrop(PointerEventData eventData)
		{
			this.HandleDirectDrop(eventData);
			if (eventData.used)
			{
				return;
			}
			Action<PointerEventData> action = this.onReceiveDrop;
			if (action == null)
			{
				return;
			}
			action(eventData);
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x00076278 File Offset: 0x00074478
		private void HandleDirectDrop(PointerEventData eventData)
		{
			if (this.Target == null)
			{
				return;
			}
			if (eventData.button != null)
			{
				return;
			}
			if (this.IsStockshopSample)
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
			if (!this.Target.TryPlug(item, false, null, 0))
			{
				return;
			}
			ItemUIUtilities.NotifyPutItem(item, false);
			eventData.Use();
		}

		// Token: 0x040016D1 RID: 5841
		[SerializeField]
		private Image icon;

		// Token: 0x040016D2 RID: 5842
		[SerializeField]
		private TrueShadow displayQualityShadow;

		// Token: 0x040016D3 RID: 5843
		[SerializeField]
		private GameObject countGameObject;

		// Token: 0x040016D4 RID: 5844
		[SerializeField]
		private TextMeshProUGUI countText;

		// Token: 0x040016D5 RID: 5845
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x040016D6 RID: 5846
		[SerializeField]
		private Graphic interactionEventReceiver;

		// Token: 0x040016D7 RID: 5847
		[SerializeField]
		private GameObject backgroundRing;

		// Token: 0x040016D8 RID: 5848
		[SerializeField]
		private GameObject inspectionElementRoot;

		// Token: 0x040016D9 RID: 5849
		[SerializeField]
		private GameObject inspectingElement;

		// Token: 0x040016DA RID: 5850
		[SerializeField]
		private GameObject notInspectingElement;

		// Token: 0x040016DB RID: 5851
		[SerializeField]
		private GameObject nameContainer;

		// Token: 0x040016DC RID: 5852
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x040016DD RID: 5853
		[SerializeField]
		private GameObject durabilityGameObject;

		// Token: 0x040016DE RID: 5854
		[SerializeField]
		private Image durabilityFill;

		// Token: 0x040016DF RID: 5855
		[SerializeField]
		private Gradient durabilityFillColorOverT;

		// Token: 0x040016E0 RID: 5856
		[SerializeField]
		private GameObject durabilityZeroIndicator;

		// Token: 0x040016E1 RID: 5857
		[SerializeField]
		private Image durabilityLoss;

		// Token: 0x040016E2 RID: 5858
		[SerializeField]
		private GameObject slotIndicatorContainer;

		// Token: 0x040016E3 RID: 5859
		[SerializeField]
		private SlotIndicator slotIndicatorTemplate;

		// Token: 0x040016E4 RID: 5860
		[SerializeField]
		private GameObject wishlistedIndicator;

		// Token: 0x040016E5 RID: 5861
		[SerializeField]
		private GameObject questRequiredIndicator;

		// Token: 0x040016E6 RID: 5862
		[SerializeField]
		private GameObject buildingRequiredIndicator;

		// Token: 0x040016E7 RID: 5863
		[SerializeField]
		[Range(0f, 1f)]
		private float punchDuration = 0.2f;

		// Token: 0x040016E8 RID: 5864
		[SerializeField]
		[Range(-1f, 1f)]
		private float selectionRingPunchScale = 0.1f;

		// Token: 0x040016E9 RID: 5865
		[SerializeField]
		[Range(-1f, 1f)]
		private float backgroundRingPunchScale = 0.2f;

		// Token: 0x040016EA RID: 5866
		[SerializeField]
		[Range(-1f, 1f)]
		private float iconPunchScale = 0.1f;

		// Token: 0x040016EF RID: 5871
		public const float doubleClickTimeThreshold = 0.3f;

		// Token: 0x040016F0 RID: 5872
		private PrefabPool<SlotIndicator> _slotIndicatorPool;

		// Token: 0x040016F4 RID: 5876
		private bool mainContentShown = true;

		// Token: 0x040016F5 RID: 5877
		private bool isBeingDestroyed;

		// Token: 0x040016F6 RID: 5878
		[SerializeField]
		private bool showOperationButtons = true;

		// Token: 0x040016FC RID: 5884
		private float lastClickTime;

		// Token: 0x040016FD RID: 5885
		private bool doubleClickInvoked;
	}
}
