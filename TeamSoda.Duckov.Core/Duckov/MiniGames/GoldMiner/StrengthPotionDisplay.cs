using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002BA RID: 698
	public class StrengthPotionDisplay : MonoBehaviour
	{
		// Token: 0x060016F9 RID: 5881 RVA: 0x0005548C File Offset: 0x0005368C
		private void Awake()
		{
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMiner goldMiner = this.master;
			goldMiner.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyLevelPlayTick));
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x000554E8 File Offset: 0x000536E8
		private void OnEarlyLevelPlayTick(GoldMiner miner)
		{
			if (this.master == null)
			{
				return;
			}
			if (this.master.run == null)
			{
				return;
			}
			this.amountText.text = string.Format("{0}", this.master.run.strengthPotion);
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x0005553C File Offset: 0x0005373C
		private void OnInteract(NavEntry entry)
		{
			this.master.UseStrengthPotion();
		}

		// Token: 0x0400110B RID: 4363
		[SerializeField]
		private GoldMiner master;

		// Token: 0x0400110C RID: 4364
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x0400110D RID: 4365
		[SerializeField]
		private NavEntry navEntry;
	}
}
