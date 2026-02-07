using System;
using Duckov.Bitcoins;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001A9 RID: 425
public class BitcoinMinerView : View
{
	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00036178 File Offset: 0x00034378
	public static BitcoinMinerView Instance
	{
		get
		{
			return View.GetViewInstance<BitcoinMinerView>();
		}
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0003617F File Offset: 0x0003437F
	// (set) Token: 0x06000CBB RID: 3259 RVA: 0x00036186 File Offset: 0x00034386
	[LocalizationKey("Default")]
	private string ActiveCommentKey
	{
		get
		{
			return "UI_BitcoinMiner_Active";
		}
		set
		{
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00036188 File Offset: 0x00034388
	// (set) Token: 0x06000CBD RID: 3261 RVA: 0x0003618F File Offset: 0x0003438F
	[LocalizationKey("Default")]
	private string StoppedCommentKey
	{
		get
		{
			return "UI_BitcoinMiner_Stopped";
		}
		set
		{
		}
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x00036194 File Offset: 0x00034394
	protected override void Awake()
	{
		base.Awake();
		this.minerInventoryDisplay.onDisplayDoubleClicked += this.OnMinerInventoryEntryDoubleClicked;
		this.inventoryDisplay.onDisplayDoubleClicked += this.OnPlayerItemsDoubleClicked;
		this.storageDisplay.onDisplayDoubleClicked += this.OnPlayerItemsDoubleClicked;
		this.minerSlotsDisplay.onElementDoubleClicked += this.OnMinerSlotEntryDoubleClicked;
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x00036204 File Offset: 0x00034404
	private void OnMinerSlotEntryDoubleClicked(ItemSlotCollectionDisplay display1, SlotDisplay slotDisplay)
	{
		Slot target = slotDisplay.Target;
		if (target == null)
		{
			return;
		}
		Item content = target.Content;
		if (content == null)
		{
			return;
		}
		ItemUtilities.SendToPlayer(content, false, PlayerStorage.Instance != null);
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x00036240 File Offset: 0x00034440
	private void OnPlayerItemsDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
	{
		Item content = entry.Content;
		if (content == null)
		{
			return;
		}
		Item item = BitcoinMiner.Instance.Item;
		if (item == null)
		{
			return;
		}
		item.TryPlug(content, true, content.InInventory, 0);
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x00036284 File Offset: 0x00034484
	private void OnMinerInventoryEntryDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
	{
		Item content = entry.Content;
		if (content == null)
		{
			return;
		}
		if (data.button == null)
		{
			ItemUtilities.SendToPlayer(content, false, true);
		}
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x000362B2 File Offset: 0x000344B2
	public static void Show()
	{
		if (BitcoinMinerView.Instance == null)
		{
			return;
		}
		if (BitcoinMiner.Instance == null)
		{
			return;
		}
		BitcoinMinerView.Instance.Open(null);
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x000362DC File Offset: 0x000344DC
	protected override void OnOpen()
	{
		base.OnOpen();
		CharacterMainControl main = CharacterMainControl.Main;
		if (!(main == null))
		{
			Item characterItem = main.CharacterItem;
			if (!(characterItem == null))
			{
				BitcoinMiner instance = BitcoinMiner.Instance;
				if (!instance.Loading)
				{
					Item item = instance.Item;
					if (!(item == null))
					{
						this.inventoryDisplay.Setup(characterItem.Inventory, null, null, false, null);
						if (PlayerStorage.Inventory != null)
						{
							this.storageDisplay.gameObject.SetActive(true);
							this.storageDisplay.Setup(PlayerStorage.Inventory, null, null, false, null);
						}
						else
						{
							this.storageDisplay.gameObject.SetActive(false);
						}
						this.minerSlotsDisplay.Setup(item, false);
						this.minerInventoryDisplay.Setup(item.Inventory, null, null, false, null);
						this.fadeGroup.Show();
						return;
					}
				}
			}
		}
		Debug.Log("Failed");
		base.Close();
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x000363D0 File Offset: 0x000345D0
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x000363E3 File Offset: 0x000345E3
	private void FixedUpdate()
	{
		this.RefreshStatus();
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x000363EC File Offset: 0x000345EC
	private void RefreshStatus()
	{
		if (BitcoinMiner.Instance.WorkPerSecond > 0.0)
		{
			TimeSpan remainingTime = BitcoinMiner.Instance.RemainingTime;
			TimeSpan timePerCoin = BitcoinMiner.Instance.TimePerCoin;
			this.remainingTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float)remainingTime.TotalHours), remainingTime.Minutes, remainingTime.Seconds);
			this.timeEachCoinText.text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float)timePerCoin.TotalHours), timePerCoin.Minutes, timePerCoin.Seconds);
			this.performanceText.text = string.Format("{0:0.#}", BitcoinMiner.Instance.Performance);
			this.commentText.text = LocalizationManager.ToPlainText(this.ActiveCommentKey);
		}
		else
		{
			this.remainingTimeText.text = "--:--:--";
			this.timeEachCoinText.text = "--:--:--";
			this.commentText.text = LocalizationManager.ToPlainText(this.StoppedCommentKey);
			this.performanceText.text = string.Format("{0:0.#}", BitcoinMiner.Instance.Performance);
		}
		this.fill.fillAmount = BitcoinMiner.Instance.NormalizedProgress;
	}

	// Token: 0x04000B12 RID: 2834
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000B13 RID: 2835
	[SerializeField]
	private InventoryDisplay inventoryDisplay;

	// Token: 0x04000B14 RID: 2836
	[SerializeField]
	private InventoryDisplay storageDisplay;

	// Token: 0x04000B15 RID: 2837
	[SerializeField]
	private ItemSlotCollectionDisplay minerSlotsDisplay;

	// Token: 0x04000B16 RID: 2838
	[SerializeField]
	private InventoryDisplay minerInventoryDisplay;

	// Token: 0x04000B17 RID: 2839
	[SerializeField]
	private TextMeshProUGUI commentText;

	// Token: 0x04000B18 RID: 2840
	[SerializeField]
	private TextMeshProUGUI remainingTimeText;

	// Token: 0x04000B19 RID: 2841
	[SerializeField]
	private TextMeshProUGUI timeEachCoinText;

	// Token: 0x04000B1A RID: 2842
	[SerializeField]
	private TextMeshProUGUI performanceText;

	// Token: 0x04000B1B RID: 2843
	[SerializeField]
	private Image fill;
}
