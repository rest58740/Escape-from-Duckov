using System;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B3 RID: 691
	public class LevelSettlementUI : MonoBehaviour
	{
		// Token: 0x060016BB RID: 5819 RVA: 0x00054828 File Offset: 0x00052A28
		internal void Reset()
		{
			if (this.clearIndicator != null)
			{
				this.clearIndicator.SetActive(false);
			}
			if (this.clearIndicator != null)
			{
				this.failIndicator.SetActive(false);
			}
			this.money = 0;
			this.score = 0;
			this.factor = 0f;
			this.RefreshTexts();
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00054888 File Offset: 0x00052A88
		public void SetTargetScore(int targetScore)
		{
			this.targetScore = targetScore;
			this.RefreshTexts();
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x00054897 File Offset: 0x00052A97
		public void StepResolveEntity(GoldMinerEntity entity)
		{
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x00054899 File Offset: 0x00052A99
		public void StepResult(bool clear)
		{
			this.clearIndicator.SetActive(clear);
			this.failIndicator.SetActive(!clear);
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000548B8 File Offset: 0x00052AB8
		public void Step(int money, float factor, int score)
		{
			bool flag = money > this.money;
			bool flag2 = factor > this.factor;
			bool flag3 = score > this.score;
			this.money = money;
			this.factor = factor;
			this.score = score;
			this.RefreshTexts();
			if (flag)
			{
				this.moneyPunch.Punch();
			}
			if (flag2)
			{
				this.factorPunch.Punch();
			}
			if (flag3)
			{
				this.scorePunch.Punch();
			}
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x00054928 File Offset: 0x00052B28
		private void RefreshTexts()
		{
			if (this.levelText == null)
			{
				Debug.LogWarning("Text is missing, abort.");
				return;
			}
			this.levelText.text = string.Format("LEVEL {0}", this.goldMiner.run.level + 1);
			this.targetScoreText.text = string.Format("{0}", this.targetScore);
			this.moneyText.text = string.Format("${0}", this.money);
			this.factorText.text = string.Format("{0}", this.factor);
			this.scoreText.text = string.Format("{0}", this.score);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x000549FA File Offset: 0x00052BFA
		public void Show()
		{
			if (this.goldMiner.isBeingDestroyed)
			{
				return;
			}
			this.fadeGroup.Show();
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00054A15 File Offset: 0x00052C15
		public void Hide()
		{
			if (this.goldMiner.isBeingDestroyed)
			{
				return;
			}
			this.fadeGroup.Hide();
		}

		// Token: 0x040010D9 RID: 4313
		[SerializeField]
		private GoldMiner goldMiner;

		// Token: 0x040010DA RID: 4314
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040010DB RID: 4315
		[SerializeField]
		private PunchReceiver moneyPunch;

		// Token: 0x040010DC RID: 4316
		[SerializeField]
		private PunchReceiver factorPunch;

		// Token: 0x040010DD RID: 4317
		[SerializeField]
		private PunchReceiver scorePunch;

		// Token: 0x040010DE RID: 4318
		[SerializeField]
		private TextMeshProUGUI moneyText;

		// Token: 0x040010DF RID: 4319
		[SerializeField]
		private TextMeshProUGUI factorText;

		// Token: 0x040010E0 RID: 4320
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x040010E1 RID: 4321
		[SerializeField]
		private TextMeshProUGUI levelText;

		// Token: 0x040010E2 RID: 4322
		[SerializeField]
		private TextMeshProUGUI targetScoreText;

		// Token: 0x040010E3 RID: 4323
		[SerializeField]
		private GameObject clearIndicator;

		// Token: 0x040010E4 RID: 4324
		[SerializeField]
		private GameObject failIndicator;

		// Token: 0x040010E5 RID: 4325
		private int targetScore;

		// Token: 0x040010E6 RID: 4326
		private int money;

		// Token: 0x040010E7 RID: 4327
		private int score;

		// Token: 0x040010E8 RID: 4328
		private float factor;
	}
}
