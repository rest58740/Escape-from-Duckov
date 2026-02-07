using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AB RID: 683
	public class EagleEyePotionDisplay : MonoBehaviour
	{
		// Token: 0x06001693 RID: 5779 RVA: 0x00054114 File Offset: 0x00052314
		private void Awake()
		{
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMiner goldMiner = this.master;
			goldMiner.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyLevelPlayTick));
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x00054170 File Offset: 0x00052370
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
			this.amountText.text = string.Format("{0}", this.master.run.eagleEyePotion);
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x000541C4 File Offset: 0x000523C4
		private void OnInteract(NavEntry entry)
		{
			this.master.UseEagleEyePotion();
		}

		// Token: 0x040010B1 RID: 4273
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010B2 RID: 4274
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x040010B3 RID: 4275
		[SerializeField]
		private NavEntry navEntry;
	}
}
