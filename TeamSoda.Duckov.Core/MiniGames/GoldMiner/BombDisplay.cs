using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AA RID: 682
	public class BombDisplay : MonoBehaviour
	{
		// Token: 0x0600168F RID: 5775 RVA: 0x0005404C File Offset: 0x0005224C
		private void Awake()
		{
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMiner goldMiner = this.master;
			goldMiner.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyLevelPlayTick));
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x000540A8 File Offset: 0x000522A8
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
			this.amountText.text = string.Format("{0}", this.master.run.bomb);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x000540FC File Offset: 0x000522FC
		private void OnInteract(NavEntry entry)
		{
			this.master.UseBomb();
		}

		// Token: 0x040010AE RID: 4270
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010AF RID: 4271
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x040010B0 RID: 4272
		[SerializeField]
		private NavEntry navEntry;
	}
}
