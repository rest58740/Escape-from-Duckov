using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B8 RID: 696
	public class ScoreDisplay : MonoBehaviour
	{
		// Token: 0x060016F1 RID: 5873 RVA: 0x000551F8 File Offset: 0x000533F8
		private void Awake()
		{
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = this.master;
			goldMiner2.onAfterResolveEntity = (Action<GoldMiner, GoldMinerEntity>)Delegate.Combine(goldMiner2.onAfterResolveEntity, new Action<GoldMiner, GoldMinerEntity>(this.OnAfterResolveEntity));
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00055253 File Offset: 0x00053453
		private void OnAfterResolveEntity(GoldMiner miner, GoldMinerEntity entity)
		{
			this.Refresh();
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x0005525B File Offset: 0x0005345B
		private void OnLevelBegin(GoldMiner miner)
		{
			this.Refresh();
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00055264 File Offset: 0x00053464
		private void Refresh()
		{
			GoldMinerRunData run = this.master.run;
			if (run == null)
			{
				return;
			}
			int num = 0;
			float num2 = run.scoreFactorBase.Value + run.levelScoreFactor;
			int targetScore = run.targetScore;
			foreach (GoldMinerEntity goldMinerEntity in this.master.resolvedEntities)
			{
				int num3 = Mathf.CeilToInt((float)goldMinerEntity.Value * run.charm.Value);
				if (num3 != 0)
				{
					num += num3;
				}
			}
			this.moneyText.text = string.Format("${0}", num);
			this.factorText.text = string.Format("{0}", num2);
			this.scoreText.text = string.Format("{0}", Mathf.CeilToInt((float)num * num2));
			this.targetScoreText.text = string.Format("{0}", targetScore);
		}

		// Token: 0x04001101 RID: 4353
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001102 RID: 4354
		[SerializeField]
		private TextMeshProUGUI moneyText;

		// Token: 0x04001103 RID: 4355
		[SerializeField]
		private TextMeshProUGUI factorText;

		// Token: 0x04001104 RID: 4356
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x04001105 RID: 4357
		[SerializeField]
		private TextMeshProUGUI targetScoreText;
	}
}
