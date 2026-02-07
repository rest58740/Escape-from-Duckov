using System;
using Duckov.MiniGames;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class GamingConsoleView : View
{
	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00039AB6 File Offset: 0x00037CB6
	public static GamingConsoleView Instance
	{
		get
		{
			return View.GetViewInstance<GamingConsoleView>();
		}
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00039AC0 File Offset: 0x00037CC0
	protected override void OnOpen()
	{
		base.OnOpen();
		this.fadeGroup.Show();
		this.Setup(this.target);
		if (CharacterMainControl.Main)
		{
			this.characterInventory.Setup(CharacterMainControl.Main.CharacterItem.Inventory, null, null, false, null);
		}
		if (PetProxy.PetInventory)
		{
			this.petInventory.Setup(PetProxy.PetInventory, null, null, false, null);
		}
		if (PlayerStorage.Inventory)
		{
			this.storageInventory.Setup(PlayerStorage.Inventory, null, null, false, null);
		}
		this.RefreshConsole();
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x00039B5A File Offset: 0x00037D5A
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x00039B70 File Offset: 0x00037D70
	private void SetTarget(GamingConsole target)
	{
		if (this.target != null)
		{
			this.target.onContentChanged -= this.OnTargetContentChanged;
		}
		if (target != null)
		{
			this.target = target;
			return;
		}
		this.target = Object.FindObjectOfType<GamingConsole>();
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x00039BC0 File Offset: 0x00037DC0
	private void Setup(GamingConsole target)
	{
		this.SetTarget(target);
		if (this.target == null)
		{
			return;
		}
		this.target.onContentChanged += this.OnTargetContentChanged;
		this.consoleSlotDisplay.Setup(this.target.ConsoleSlot);
		this.monitorSlotDisplay.Setup(this.target.MonitorSlot);
		this.RefreshConsole();
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x00039C2C File Offset: 0x00037E2C
	private void OnTargetContentChanged(GamingConsole console)
	{
		this.RefreshConsole();
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x00039C34 File Offset: 0x00037E34
	private void RefreshConsole()
	{
		if (this.isBeingDestroyed)
		{
			return;
		}
		Slot consoleSlot = this.target.ConsoleSlot;
		if (consoleSlot == null)
		{
			return;
		}
		Item content = consoleSlot.Content;
		this.consoleSlotCollectionDisplay.gameObject.SetActive(content);
		if (content)
		{
			this.consoleSlotCollectionDisplay.Setup(content, false);
		}
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x00039C8C File Offset: 0x00037E8C
	internal static void Show(GamingConsole console)
	{
		GamingConsoleView.Instance.target = console;
		GamingConsoleView.Instance.Open(null);
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x00039CA4 File Offset: 0x00037EA4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.isBeingDestroyed = true;
	}

	// Token: 0x04000BD0 RID: 3024
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000BD1 RID: 3025
	[SerializeField]
	private InventoryDisplay characterInventory;

	// Token: 0x04000BD2 RID: 3026
	[SerializeField]
	private InventoryDisplay petInventory;

	// Token: 0x04000BD3 RID: 3027
	[SerializeField]
	private InventoryDisplay storageInventory;

	// Token: 0x04000BD4 RID: 3028
	[SerializeField]
	private SlotDisplay monitorSlotDisplay;

	// Token: 0x04000BD5 RID: 3029
	[SerializeField]
	private SlotDisplay consoleSlotDisplay;

	// Token: 0x04000BD6 RID: 3030
	[SerializeField]
	private ItemSlotCollectionDisplay consoleSlotCollectionDisplay;

	// Token: 0x04000BD7 RID: 3031
	private GamingConsole target;

	// Token: 0x04000BD8 RID: 3032
	private bool isBeingDestroyed;
}
