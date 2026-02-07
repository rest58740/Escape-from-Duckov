using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B5 RID: 693
	public class NavGroupLinearController : MiniGameBehaviour
	{
		// Token: 0x060016D5 RID: 5845 RVA: 0x00054C7C File Offset: 0x00052E7C
		private void Awake()
		{
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			NavGroup.OnNavGroupChanged = (Action)Delegate.Combine(NavGroup.OnNavGroupChanged, new Action(this.OnNavGroupChanged));
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00054CD0 File Offset: 0x00052ED0
		private void OnLevelBegin(GoldMiner miner)
		{
			if (this.setActiveWhenLevelBegin)
			{
				this.navGroup.SetAsActiveNavGroup();
			}
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00054CE5 File Offset: 0x00052EE5
		private void OnNavGroupChanged()
		{
			this.changeLock = true;
		}

		// Token: 0x040010EE RID: 4334
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010EF RID: 4335
		[SerializeField]
		private NavGroup navGroup;

		// Token: 0x040010F0 RID: 4336
		[SerializeField]
		private NavGroup otherNavGroup;

		// Token: 0x040010F1 RID: 4337
		[SerializeField]
		private bool setActiveWhenLevelBegin;

		// Token: 0x040010F2 RID: 4338
		private bool changeLock;
	}
}
