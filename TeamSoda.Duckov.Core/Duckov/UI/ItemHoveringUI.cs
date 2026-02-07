using System;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x02000394 RID: 916
	public class ItemHoveringUI : MonoBehaviour
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x00070450 File Offset: 0x0006E650
		// (set) Token: 0x06001FD6 RID: 8150 RVA: 0x00070457 File Offset: 0x0006E657
		public static ItemHoveringUI Instance { get; private set; }

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x0007045F File Offset: 0x0006E65F
		public RectTransform LayoutParent
		{
			get
			{
				return this.layoutParent;
			}
		}

		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06001FD8 RID: 8152 RVA: 0x00070468 File Offset: 0x0006E668
		// (remove) Token: 0x06001FD9 RID: 8153 RVA: 0x0007049C File Offset: 0x0006E69C
		public static event Action<ItemHoveringUI, ItemMetaData> onSetupMeta;

		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06001FDA RID: 8154 RVA: 0x000704D0 File Offset: 0x0006E6D0
		// (remove) Token: 0x06001FDB RID: 8155 RVA: 0x00070504 File Offset: 0x0006E704
		public static event Action<ItemHoveringUI, Item> onSetupItem;

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x00070537 File Offset: 0x0006E737
		// (set) Token: 0x06001FDD RID: 8157 RVA: 0x0007053E File Offset: 0x0006E73E
		public static int DisplayingItemID { get; private set; }

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x00070546 File Offset: 0x0006E746
		public static bool Shown
		{
			get
			{
				return !(ItemHoveringUI.Instance == null) && ItemHoveringUI.Instance.fadeGroup.IsShown;
			}
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x00070568 File Offset: 0x0006E768
		private void Awake()
		{
			ItemHoveringUI.Instance = this;
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			ItemDisplay.OnPointerEnterItemDisplay += this.OnPointerEnterItemDisplay;
			ItemDisplay.OnPointerExitItemDisplay += this.OnPointerExitItemDisplay;
			ItemAmountDisplay.OnMouseEnter += this.OnMouseEnterItemAmountDisplay;
			ItemAmountDisplay.OnMouseExit += this.OnMouseExitItemAmountDisplay;
			ItemMetaDisplay.OnMouseEnter += this.OnMouseEnterMetaDisplay;
			ItemMetaDisplay.OnMouseExit += this.OnMouseExitMetaDisplay;
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000705FC File Offset: 0x0006E7FC
		private void OnDestroy()
		{
			ItemDisplay.OnPointerEnterItemDisplay -= this.OnPointerEnterItemDisplay;
			ItemDisplay.OnPointerExitItemDisplay -= this.OnPointerExitItemDisplay;
			ItemAmountDisplay.OnMouseEnter -= this.OnMouseEnterItemAmountDisplay;
			ItemAmountDisplay.OnMouseExit -= this.OnMouseExitItemAmountDisplay;
			ItemMetaDisplay.OnMouseEnter -= this.OnMouseEnterMetaDisplay;
			ItemMetaDisplay.OnMouseExit -= this.OnMouseExitMetaDisplay;
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x0007066F File Offset: 0x0006E86F
		private void OnMouseExitMetaDisplay(ItemMetaDisplay display)
		{
			if (this.target == display)
			{
				this.Hide();
			}
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x00070685 File Offset: 0x0006E885
		private void OnMouseEnterMetaDisplay(ItemMetaDisplay display)
		{
			this.SetupAndShowMeta<ItemMetaDisplay>(display);
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x0007068E File Offset: 0x0006E88E
		private void OnMouseExitItemAmountDisplay(ItemAmountDisplay display)
		{
			if (this.target == display)
			{
				this.Hide();
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000706A4 File Offset: 0x0006E8A4
		private void OnMouseEnterItemAmountDisplay(ItemAmountDisplay display)
		{
			this.SetupAndShowMeta<ItemAmountDisplay>(display);
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000706AD File Offset: 0x0006E8AD
		private void OnPointerExitItemDisplay(ItemDisplay display)
		{
			if (this.target == display)
			{
				this.Hide();
			}
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000706C3 File Offset: 0x0006E8C3
		private void OnPointerEnterItemDisplay(ItemDisplay display)
		{
			this.SetupAndShow(display);
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000706CC File Offset: 0x0006E8CC
		private void SetupAndShow(ItemDisplay display)
		{
			if (display == null)
			{
				return;
			}
			Item item = display.Target;
			if (item == null)
			{
				return;
			}
			if (item.NeedInspection)
			{
				return;
			}
			this.registeredIndicator.SetActive(false);
			this.target = display;
			this.itemName.text = (item.DisplayName ?? "");
			this.itemDescription.text = (item.Description ?? "");
			this.weightDisplay.gameObject.SetActive(true);
			this.weightDisplay.text = string.Format("{0:0.#} kg", item.TotalWeight);
			this.itemID.text = string.Format("#{0}", item.TypeID);
			ItemHoveringUI.DisplayingItemID = item.TypeID;
			this.itemProperties.gameObject.SetActive(true);
			this.itemProperties.Setup(item);
			this.interactionIndicatorsContainer.SetActive(true);
			this.interactionIndicator_Menu.SetActive(display.ShowOperationButtons);
			this.interactionIndicator_Move.SetActive(display.Movable);
			this.interactionIndicator_Drop.SetActive(display.CanDrop);
			this.interactionIndicator_Use.SetActive(display.CanUse);
			this.interactionIndicator_Split.SetActive(display.CanSplit);
			this.interactionIndicator_LockSort.SetActive(display.CanLockSort);
			this.interactionIndicator_Shortcut.SetActive(display.CanSetShortcut);
			this.usageUtilitiesDisplay.Setup(item);
			this.SetupWishlistInfos(item.TypeID);
			this.SetupBulletDisplay();
			try
			{
				Action<ItemHoveringUI, Item> action = ItemHoveringUI.onSetupItem;
				if (action != null)
				{
					action(this, item);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			this.RefreshPosition();
			this.SetupRegisteredInfo(item);
			this.fadeGroup.Show();
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000708A4 File Offset: 0x0006EAA4
		private void SetupRegisteredInfo(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (item.IsRegistered())
			{
				this.registeredIndicator.SetActive(true);
			}
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000708C4 File Offset: 0x0006EAC4
		private void SetupAndShowMeta<T>(T dataProvider) where T : MonoBehaviour, IItemMetaDataProvider
		{
			if (dataProvider == null)
			{
				return;
			}
			this.registeredIndicator.SetActive(false);
			this.target = dataProvider;
			ItemMetaData metaData = dataProvider.GetMetaData();
			this.itemName.text = metaData.DisplayName;
			this.itemID.text = string.Format("{0}", metaData.id);
			ItemHoveringUI.DisplayingItemID = metaData.id;
			this.itemDescription.text = metaData.Description;
			this.interactionIndicatorsContainer.SetActive(true);
			this.weightDisplay.gameObject.SetActive(false);
			this.bulletTypeDisplay.gameObject.SetActive(false);
			this.itemProperties.gameObject.SetActive(false);
			this.interactionIndicator_Menu.gameObject.SetActive(false);
			this.interactionIndicator_Move.gameObject.SetActive(false);
			this.interactionIndicator_Drop.gameObject.SetActive(false);
			this.interactionIndicator_Use.gameObject.SetActive(false);
			this.usageUtilitiesDisplay.gameObject.SetActive(false);
			this.interactionIndicator_Split.SetActive(false);
			this.interactionIndicator_Shortcut.SetActive(false);
			this.SetupWishlistInfos(metaData.id);
			try
			{
				Action<ItemHoveringUI, ItemMetaData> action = ItemHoveringUI.onSetupMeta;
				if (action != null)
				{
					action(this, metaData);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			this.RefreshPosition();
			this.fadeGroup.Show();
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x00070A48 File Offset: 0x0006EC48
		private void SetupBulletDisplay()
		{
			ItemDisplay itemDisplay = this.target as ItemDisplay;
			if (itemDisplay == null)
			{
				return;
			}
			ItemSetting_Gun component = itemDisplay.Target.GetComponent<ItemSetting_Gun>();
			if (component == null)
			{
				this.bulletTypeDisplay.gameObject.SetActive(false);
				return;
			}
			this.bulletTypeDisplay.gameObject.SetActive(true);
			this.bulletTypeDisplay.Setup(component.TargetBulletID);
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x00070AB4 File Offset: 0x0006ECB4
		private unsafe void RefreshPosition()
		{
			Vector2 vector = *Mouse.current.position.value;
			Vector2 vector2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, vector, null, ref vector2);
			float xMax = this.contents.rect.xMax;
			float yMin = this.contents.rect.yMin;
			float num = this.rectTransform.rect.xMax - xMax;
			float num2 = this.rectTransform.rect.yMin - yMin;
			vector2.x = Mathf.Min(vector2.x, num);
			vector2.y = Mathf.Max(vector2.y, num2);
			this.contents.anchoredPosition = vector2;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x00070B74 File Offset: 0x0006ED74
		private void Hide()
		{
			this.fadeGroup.Hide();
			ItemHoveringUI.DisplayingItemID = -1;
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x00070B88 File Offset: 0x0006ED88
		private void Update()
		{
			if (this.fadeGroup.IsShown)
			{
				if (this.target == null || !this.target.isActiveAndEnabled)
				{
					this.Hide();
				}
				ItemDisplay itemDisplay = this.target as ItemDisplay;
				if (itemDisplay != null && itemDisplay.Target == null)
				{
					this.Hide();
				}
			}
			this.RefreshPosition();
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x00070BEC File Offset: 0x0006EDEC
		private void SetupWishlistInfos(int itemTypeID)
		{
			ItemWishlist.WishlistInfo wishlistInfo = ItemWishlist.GetWishlistInfo(itemTypeID);
			bool isManuallyWishlisted = wishlistInfo.isManuallyWishlisted;
			bool isBuildingRequired = wishlistInfo.isBuildingRequired;
			bool isQuestRequired = wishlistInfo.isQuestRequired;
			bool active = isManuallyWishlisted || isBuildingRequired || isQuestRequired;
			this.wishlistIndicator.SetActive(isManuallyWishlisted);
			this.buildingIndicator.SetActive(isBuildingRequired);
			this.questIndicator.SetActive(isQuestRequired);
			this.wishlistInfoParent.SetActive(active);
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x00070C49 File Offset: 0x0006EE49
		internal static void NotifyRefreshWishlistInfo()
		{
			if (ItemHoveringUI.Instance == null)
			{
				return;
			}
			ItemHoveringUI.Instance.SetupWishlistInfos(ItemHoveringUI.DisplayingItemID);
		}

		// Token: 0x040015C7 RID: 5575
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x040015C8 RID: 5576
		[SerializeField]
		private RectTransform layoutParent;

		// Token: 0x040015C9 RID: 5577
		[SerializeField]
		private RectTransform contents;

		// Token: 0x040015CA RID: 5578
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040015CB RID: 5579
		[SerializeField]
		private TextMeshProUGUI itemName;

		// Token: 0x040015CC RID: 5580
		[SerializeField]
		private TextMeshProUGUI weightDisplay;

		// Token: 0x040015CD RID: 5581
		[SerializeField]
		private TextMeshProUGUI itemDescription;

		// Token: 0x040015CE RID: 5582
		[SerializeField]
		private TextMeshProUGUI itemID;

		// Token: 0x040015CF RID: 5583
		[SerializeField]
		private ItemPropertiesDisplay itemProperties;

		// Token: 0x040015D0 RID: 5584
		[SerializeField]
		private BulletTypeDisplay bulletTypeDisplay;

		// Token: 0x040015D1 RID: 5585
		[SerializeField]
		private UsageUtilitiesDisplay usageUtilitiesDisplay;

		// Token: 0x040015D2 RID: 5586
		[SerializeField]
		private GameObject interactionIndicatorsContainer;

		// Token: 0x040015D3 RID: 5587
		[SerializeField]
		private GameObject interactionIndicator_Move;

		// Token: 0x040015D4 RID: 5588
		[SerializeField]
		private GameObject interactionIndicator_Menu;

		// Token: 0x040015D5 RID: 5589
		[SerializeField]
		private GameObject interactionIndicator_Drop;

		// Token: 0x040015D6 RID: 5590
		[SerializeField]
		private GameObject interactionIndicator_Use;

		// Token: 0x040015D7 RID: 5591
		[SerializeField]
		private GameObject interactionIndicator_Split;

		// Token: 0x040015D8 RID: 5592
		[SerializeField]
		private GameObject interactionIndicator_LockSort;

		// Token: 0x040015D9 RID: 5593
		[SerializeField]
		private GameObject interactionIndicator_Shortcut;

		// Token: 0x040015DA RID: 5594
		[SerializeField]
		private GameObject wishlistInfoParent;

		// Token: 0x040015DB RID: 5595
		[SerializeField]
		private GameObject wishlistIndicator;

		// Token: 0x040015DC RID: 5596
		[SerializeField]
		private GameObject buildingIndicator;

		// Token: 0x040015DD RID: 5597
		[SerializeField]
		private GameObject questIndicator;

		// Token: 0x040015DE RID: 5598
		[SerializeField]
		private GameObject registeredIndicator;

		// Token: 0x040015E2 RID: 5602
		private MonoBehaviour target;
	}
}
