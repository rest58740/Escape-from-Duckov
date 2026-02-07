using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Economy.UI
{
	// Token: 0x0200033F RID: 831
	public class StockShopView : View, ISingleSelectionMenu<StockShopItemEntry>
	{
		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x000663D3 File Offset: 0x000645D3
		public static StockShopView Instance
		{
			get
			{
				return View.GetViewInstance<StockShopView>();
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x000663DA File Offset: 0x000645DA
		private string TextBuy
		{
			get
			{
				return LocalizationManager.ToPlainText(this.textBuy);
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001C20 RID: 7200 RVA: 0x000663E7 File Offset: 0x000645E7
		private string TextSoldOut
		{
			get
			{
				return LocalizationManager.ToPlainText(this.textSoldOut);
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001C21 RID: 7201 RVA: 0x000663F4 File Offset: 0x000645F4
		private string TextSell
		{
			get
			{
				return LocalizationManager.ToPlainText(this.textSell);
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001C22 RID: 7202 RVA: 0x00066401 File Offset: 0x00064601
		private string TextUnlock
		{
			get
			{
				return LocalizationManager.ToPlainText(this.textUnlock);
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001C23 RID: 7203 RVA: 0x0006640E File Offset: 0x0006460E
		private string TextLocked
		{
			get
			{
				return LocalizationManager.ToPlainText(this.textLocked);
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001C24 RID: 7204 RVA: 0x0006641C File Offset: 0x0006461C
		private PrefabPool<StockShopItemEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<StockShopItemEntry>(this.entryTemplate, this.entryTemplate.transform.parent, null, null, null, true, 10, 10000, null);
					this.entryTemplate.gameObject.SetActive(false);
				}
				return this._entryPool;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x00066475 File Offset: 0x00064675
		private Object Selection
		{
			get
			{
				if (ItemUIUtilities.SelectedItemDisplay != null)
				{
					return ItemUIUtilities.SelectedItemDisplay;
				}
				if (this.selectedItem != null)
				{
					return this.selectedItem;
				}
				return null;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x000664A0 File Offset: 0x000646A0
		public StockShop Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x000664A8 File Offset: 0x000646A8
		protected override void Awake()
		{
			base.Awake();
			this.interactionButton.onClick.AddListener(new UnityAction(this.OnInteractionButtonClicked));
			UIInputManager.OnFastPick += this.OnFastPick;
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x000664DD File Offset: 0x000646DD
		protected override void OnDestroy()
		{
			base.OnDestroy();
			UIInputManager.OnFastPick -= this.OnFastPick;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000664F6 File Offset: 0x000646F6
		private void OnFastPick(UIInputEventData data)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.OnInteractionButtonClicked();
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x00066507 File Offset: 0x00064707
		private void FixedUpdate()
		{
			this.RefreshCountDown();
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x00066510 File Offset: 0x00064710
		private void RefreshCountDown()
		{
			if (this.target == null)
			{
				this.refreshCountDown.text = "-";
			}
			TimeSpan nextRefreshETA = this.target.NextRefreshETA;
			int days = nextRefreshETA.Days;
			int hours = nextRefreshETA.Hours;
			int minutes = nextRefreshETA.Minutes;
			int seconds = nextRefreshETA.Seconds;
			this.refreshCountDown.text = string.Format("{0}{1:00}:{2:00}:{3:00}", new object[]
			{
				(days > 0) ? (days.ToString() + " - ") : "",
				hours,
				minutes,
				seconds
			});
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x000665C0 File Offset: 0x000647C0
		private void OnInteractionButtonClicked()
		{
			if (this.Selection == null)
			{
				return;
			}
			ItemDisplay itemDisplay = this.Selection as ItemDisplay;
			if (itemDisplay != null)
			{
				this.Target.Sell(itemDisplay.Target).Forget();
				AudioManager.Post(this.sfx_Sell);
				ItemUIUtilities.Select(null);
				this.OnSelectionChanged();
				return;
			}
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				int itemTypeID = stockShopItemEntry.Target.ItemTypeID;
				if (stockShopItemEntry.IsUnlocked())
				{
					this.BuyTask(itemTypeID).Forget();
					return;
				}
				if (EconomyManager.IsWaitingForUnlockConfirm(itemTypeID))
				{
					EconomyManager.ConfirmUnlock(itemTypeID);
				}
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x00066658 File Offset: 0x00064858
		private UniTask BuyTask(int itemTypeID)
		{
			StockShopView.<BuyTask>d__58 <BuyTask>d__;
			<BuyTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<BuyTask>d__.<>4__this = this;
			<BuyTask>d__.itemTypeID = itemTypeID;
			<BuyTask>d__.<>1__state = -1;
			<BuyTask>d__.<>t__builder.Start<StockShopView.<BuyTask>d__58>(ref <BuyTask>d__);
			return <BuyTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000666A4 File Offset: 0x000648A4
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemUIUtilitiesSelectionChanged;
			EconomyManager.OnItemUnlockStateChanged += this.OnItemUnlockStateChanged;
			StockShop.OnAfterItemSold += this.OnAfterItemSold;
			UIInputManager.OnNextPage += this.OnNextPage;
			UIInputManager.OnPreviousPage += this.OnPreviousPage;
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x00066708 File Offset: 0x00064908
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemUIUtilitiesSelectionChanged;
			EconomyManager.OnItemUnlockStateChanged -= this.OnItemUnlockStateChanged;
			StockShop.OnAfterItemSold -= this.OnAfterItemSold;
			UIInputManager.OnNextPage -= this.OnNextPage;
			UIInputManager.OnPreviousPage -= this.OnPreviousPage;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x0006676A File Offset: 0x0006496A
		private void OnNextPage(UIInputEventData data)
		{
			this.playerStorageDisplay.NextPage();
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x00066777 File Offset: 0x00064977
		private void OnPreviousPage(UIInputEventData data)
		{
			this.playerStorageDisplay.PreviousPage();
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x00066784 File Offset: 0x00064984
		private void OnAfterItemSold(StockShop shop)
		{
			this.RefreshInteractionButton();
			this.RefreshStockText();
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x00066792 File Offset: 0x00064992
		private void OnItemUnlockStateChanged(int itemTypeID)
		{
			if (this.details.Target == null)
			{
				return;
			}
			if (itemTypeID == this.details.Target.TypeID)
			{
				this.RefreshInteractionButton();
				this.RefreshStockText();
			}
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x000667C7 File Offset: 0x000649C7
		private void OnItemUIUtilitiesSelectionChanged()
		{
			if (this.selectedItem != null && ItemUIUtilities.SelectedItemDisplay != null)
			{
				this.selectedItem = null;
			}
			this.OnSelectionChanged();
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000667F4 File Offset: 0x000649F4
		private void OnSelectionChanged()
		{
			Action action = this.onSelectionChanged;
			if (action != null)
			{
				action();
			}
			if (this.Selection == null)
			{
				this.detailsFadeGroup.Hide();
				return;
			}
			Item item = null;
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				item = stockShopItemEntry.GetItem();
			}
			else
			{
				ItemDisplay itemDisplay = this.Selection as ItemDisplay;
				if (itemDisplay != null)
				{
					item = itemDisplay.Target;
				}
			}
			if (item == null)
			{
				this.detailsFadeGroup.Hide();
				return;
			}
			this.details.Setup(item);
			this.RefreshStockText();
			this.RefreshInteractionButton();
			this.RefreshCountDown();
			this.detailsFadeGroup.Show();
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x0006689C File Offset: 0x00064A9C
		private void RefreshStockText()
		{
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				this.stockText.gameObject.SetActive(true);
				this.stockText.text = StringExtensions.Format(this.stockTextFormat, new
				{
					text = LocalizationManager.ToPlainText(this.stockTextKey),
					current = stockShopItemEntry.Target.CurrentStock,
					max = stockShopItemEntry.Target.MaxStock
				});
				return;
			}
			if (this.Selection is ItemDisplay)
			{
				this.stockText.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x00066924 File Offset: 0x00064B24
		public StockShopItemEntry GetSelection()
		{
			return this.Selection as StockShopItemEntry;
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x00066931 File Offset: 0x00064B31
		public bool SetSelection(StockShopItemEntry selection)
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				ItemUIUtilities.Select(null);
			}
			this.selectedItem = selection;
			this.OnSelectionChanged();
			return true;
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x00066954 File Offset: 0x00064B54
		internal void Setup(StockShop target)
		{
			this.target = target;
			this.detailsFadeGroup.SkipHide();
			this.merchantNameText.text = target.DisplayName;
			LevelManager instance = LevelManager.Instance;
			Inventory inventory;
			if (instance == null)
			{
				inventory = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter == null)
				{
					inventory = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					inventory = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			Inventory inventory2 = inventory;
			this.playerInventoryDisplay.Setup(inventory2, null, (Item e) => e == null || e.CanBeSold, false, null);
			if (PetProxy.PetInventory != null && LevelConfig.SavePet)
			{
				this.petInventoryDisplay.Setup(PetProxy.PetInventory, null, (Item e) => e == null || e.CanBeSold, false, null);
				this.petInventoryDisplay.gameObject.SetActive(true);
			}
			else
			{
				this.petInventoryDisplay.gameObject.SetActive(false);
			}
			Inventory inventory3 = PlayerStorage.Inventory;
			if (inventory3 != null)
			{
				this.playerStorageDisplay.gameObject.SetActive(true);
				this.playerStorageDisplay.Setup(inventory3, null, (Item e) => e == null || e.CanBeSold, false, null);
			}
			else
			{
				this.playerStorageDisplay.gameObject.SetActive(false);
			}
			this.EntryPool.ReleaseAll();
			Transform parent = this.entryTemplate.transform.parent;
			foreach (StockShop.Entry entry in target.entries)
			{
				if (entry.Show)
				{
					StockShopItemEntry stockShopItemEntry = this.EntryPool.Get(parent);
					stockShopItemEntry.Setup(this, entry);
					stockShopItemEntry.transform.SetAsLastSibling();
				}
			}
			TradingUIUtilities.ActiveMerchant = target;
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x00066B34 File Offset: 0x00064D34
		private void RefreshInteractionButton()
		{
			this.cannotSellIndicator.SetActive(false);
			this.cashOnlyIndicator.SetActive(!this.Target.AccountAvaliable);
			ItemDisplay itemDisplay = this.Selection as ItemDisplay;
			if (itemDisplay != null)
			{
				bool canBeSold = itemDisplay.Target.CanBeSold;
				this.interactionButton.interactable = canBeSold;
				this.priceDisplay.gameObject.SetActive(true);
				this.lockDisplay.gameObject.SetActive(false);
				this.interactionText.text = this.TextSell;
				this.interactionButtonImage.color = this.buttonColor_Interactable;
				this.priceText.text = this.<RefreshInteractionButton>g__GetPriceText|71_1(itemDisplay.Target, true);
				this.cannotSellIndicator.SetActive(!itemDisplay.Target.CanBeSold);
				return;
			}
			StockShopItemEntry stockShopItemEntry = this.Selection as StockShopItemEntry;
			if (stockShopItemEntry != null)
			{
				bool flag = stockShopItemEntry.IsUnlocked();
				bool flag2 = EconomyManager.IsWaitingForUnlockConfirm(stockShopItemEntry.Target.ItemTypeID);
				this.interactionButton.interactable = (flag || flag2);
				this.priceDisplay.gameObject.SetActive(flag);
				this.lockDisplay.gameObject.SetActive(!flag);
				this.cannotSellIndicator.SetActive(false);
				if (flag)
				{
					Item item = stockShopItemEntry.GetItem();
					int num = this.<RefreshInteractionButton>g__GetPrice|71_0(item, false);
					bool enough = new Cost((long)num).Enough;
					this.priceText.text = num.ToString("n0");
					if (stockShopItemEntry.Target.CurrentStock > 0)
					{
						this.interactionText.text = this.TextBuy;
						this.interactionButtonImage.color = (enough ? this.buttonColor_Interactable : this.buttonColor_NotInteractable);
						return;
					}
					this.interactionButton.interactable = false;
					this.interactionText.text = this.TextSoldOut;
					this.interactionButtonImage.color = this.buttonColor_NotInteractable;
					return;
				}
				else
				{
					if (flag2)
					{
						this.interactionText.text = this.TextUnlock;
						this.interactionButtonImage.color = this.buttonColor_Interactable;
						return;
					}
					this.interactionText.text = this.TextLocked;
					this.interactionButtonImage.color = this.buttonColor_NotInteractable;
				}
			}
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x00066D69 File Offset: 0x00064F69
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00066D7C File Offset: 0x00064F7C
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x00066D8F File Offset: 0x00064F8F
		internal void SetupAndShow(StockShop stockShop)
		{
			ItemUIUtilities.Select(null);
			this.SetSelection(null);
			this.Setup(stockShop);
			base.Open(null);
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x00066E26 File Offset: 0x00065026
		[CompilerGenerated]
		private int <RefreshInteractionButton>g__GetPrice|71_0(Item item, bool selling)
		{
			return this.Target.ConvertPrice(item, selling);
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x00066E38 File Offset: 0x00065038
		[CompilerGenerated]
		private string <RefreshInteractionButton>g__GetPriceText|71_1(Item item, bool selling)
		{
			return this.<RefreshInteractionButton>g__GetPrice|71_0(item, selling).ToString("n0");
		}

		// Token: 0x040013F3 RID: 5107
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040013F4 RID: 5108
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x040013F5 RID: 5109
		[SerializeField]
		private ItemDetailsDisplay details;

		// Token: 0x040013F6 RID: 5110
		[SerializeField]
		private InventoryDisplay playerInventoryDisplay;

		// Token: 0x040013F7 RID: 5111
		[SerializeField]
		private InventoryDisplay petInventoryDisplay;

		// Token: 0x040013F8 RID: 5112
		[SerializeField]
		private InventoryDisplay playerStorageDisplay;

		// Token: 0x040013F9 RID: 5113
		[SerializeField]
		private StockShopItemEntry entryTemplate;

		// Token: 0x040013FA RID: 5114
		[SerializeField]
		private TextMeshProUGUI stockText;

		// Token: 0x040013FB RID: 5115
		[SerializeField]
		[LocalizationKey("Default")]
		private string stockTextKey = "UI_Stock";

		// Token: 0x040013FC RID: 5116
		[SerializeField]
		private string stockTextFormat = "{text} {current}/{max}";

		// Token: 0x040013FD RID: 5117
		[SerializeField]
		private TextMeshProUGUI merchantNameText;

		// Token: 0x040013FE RID: 5118
		[SerializeField]
		private Button interactionButton;

		// Token: 0x040013FF RID: 5119
		[SerializeField]
		private Image interactionButtonImage;

		// Token: 0x04001400 RID: 5120
		[SerializeField]
		private Color buttonColor_Interactable;

		// Token: 0x04001401 RID: 5121
		[SerializeField]
		private Color buttonColor_NotInteractable;

		// Token: 0x04001402 RID: 5122
		[SerializeField]
		private TextMeshProUGUI interactionText;

		// Token: 0x04001403 RID: 5123
		[SerializeField]
		private GameObject cashOnlyIndicator;

		// Token: 0x04001404 RID: 5124
		[SerializeField]
		private GameObject cannotSellIndicator;

		// Token: 0x04001405 RID: 5125
		[LocalizationKey("Default")]
		[SerializeField]
		private string textBuy = "购买";

		// Token: 0x04001406 RID: 5126
		[LocalizationKey("Default")]
		[SerializeField]
		private string textSoldOut = "已售罄";

		// Token: 0x04001407 RID: 5127
		[LocalizationKey("Default")]
		[SerializeField]
		private string textSell = "出售";

		// Token: 0x04001408 RID: 5128
		[LocalizationKey("Default")]
		[SerializeField]
		private string textUnlock = "解锁";

		// Token: 0x04001409 RID: 5129
		[LocalizationKey("Default")]
		[SerializeField]
		private string textLocked = "已锁定";

		// Token: 0x0400140A RID: 5130
		[SerializeField]
		private GameObject priceDisplay;

		// Token: 0x0400140B RID: 5131
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x0400140C RID: 5132
		[SerializeField]
		private GameObject lockDisplay;

		// Token: 0x0400140D RID: 5133
		[SerializeField]
		private FadeGroup clickBlockerFadeGroup;

		// Token: 0x0400140E RID: 5134
		[SerializeField]
		private TextMeshProUGUI refreshCountDown;

		// Token: 0x0400140F RID: 5135
		private string sfx_Buy = "UI/buy";

		// Token: 0x04001410 RID: 5136
		private string sfx_Sell = "UI/sell";

		// Token: 0x04001411 RID: 5137
		private PrefabPool<StockShopItemEntry> _entryPool;

		// Token: 0x04001412 RID: 5138
		private StockShop target;

		// Token: 0x04001413 RID: 5139
		private StockShopItemEntry selectedItem;

		// Token: 0x04001414 RID: 5140
		public Action onSelectionChanged;
	}
}
