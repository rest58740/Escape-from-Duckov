using System;
using Duckov.BlackMarkets.UI;
using Duckov.Crops;
using Duckov.Crops.UI;
using Duckov.Endowment.UI;
using Duckov.MasterKeys.UI;
using Duckov.MiniGames;
using Duckov.MiniMaps.UI;
using Duckov.Quests.UI;
using Duckov.UI;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class ViewsProxy : MonoBehaviour
{
	// Token: 0x060009A8 RID: 2472 RVA: 0x0002B1B5 File Offset: 0x000293B5
	public void ShowInventoryView()
	{
		if (LevelManager.Instance.IsBaseLevel && PlayerStorage.Instance)
		{
			PlayerStorage.Instance.InteractableLootBox.InteractWithMainCharacter();
			return;
		}
		InventoryView.Show();
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x0002B1E4 File Offset: 0x000293E4
	public void ShowQuestView()
	{
		QuestView.Show();
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0002B1EB File Offset: 0x000293EB
	public void ShowMapView()
	{
		MiniMapView.Show();
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0002B1F2 File Offset: 0x000293F2
	public void ShowKeyView()
	{
		MasterKeysView.Show();
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0002B1F9 File Offset: 0x000293F9
	public void ShowPlayerStats()
	{
		PlayerStatsView.Instance.Open(null);
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x0002B206 File Offset: 0x00029406
	public void ShowEndowmentView()
	{
		EndowmentSelectionPanel.Show();
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0002B20D File Offset: 0x0002940D
	public void ShowMapSelectionView()
	{
		MapSelectionView.Instance.Open(null);
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0002B21A File Offset: 0x0002941A
	public void ShowRepairView()
	{
		ItemRepairView.Instance.Open(null);
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0002B227 File Offset: 0x00029427
	public void ShowFormulasIndexView()
	{
		FormulasIndexView.Show();
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0002B22E File Offset: 0x0002942E
	public void ShowBitcoinView()
	{
		BitcoinMinerView.Show();
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0002B235 File Offset: 0x00029435
	public void ShowStorageDock()
	{
		StorageDock.Show();
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0002B23C File Offset: 0x0002943C
	public void ShowBlackMarket_Demands()
	{
		BlackMarketView.Show(BlackMarketView.Mode.Demand);
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0002B244 File Offset: 0x00029444
	public void ShowBlackMarket_Supplies()
	{
		BlackMarketView.Show(BlackMarketView.Mode.Supply);
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x0002B24C File Offset: 0x0002944C
	public void ShowSleepView()
	{
		SleepView.Show();
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x0002B253 File Offset: 0x00029453
	public void ShowATMView()
	{
		ATMView.Show();
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x0002B25A File Offset: 0x0002945A
	public void ShowDecomposeView()
	{
		ItemDecomposeView.Show();
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0002B261 File Offset: 0x00029461
	public void ShowGardenView(Garden garnden)
	{
		GardenView.Show(garnden);
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x0002B269 File Offset: 0x00029469
	public void ShowGamingConsoleView(GamingConsole console)
	{
		GamingConsoleView.Show(console);
	}
}
