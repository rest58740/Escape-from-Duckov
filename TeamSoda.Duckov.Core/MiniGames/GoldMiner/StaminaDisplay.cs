using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B9 RID: 697
	public class StaminaDisplay : MonoBehaviour
	{
		// Token: 0x060016F6 RID: 5878 RVA: 0x00055380 File Offset: 0x00053580
		private void FixedUpdate()
		{
			this.Refresh();
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00055388 File Offset: 0x00053588
		private void Refresh()
		{
			if (this.master == null)
			{
				return;
			}
			GoldMinerRunData run = this.master.run;
			if (run == null)
			{
				return;
			}
			float stamina = run.stamina;
			float value = run.maxStamina.Value;
			float value2 = run.extraStamina.Value;
			if (stamina > 0f)
			{
				float num = stamina / value;
				this.fill.fillAmount = num;
				this.fill.color = this.normalColor.Evaluate(num);
				this.text.text = string.Format("{0:0.0}", stamina);
				return;
			}
			float num2 = value2 + stamina;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			float fillAmount = num2 / value2;
			this.fill.fillAmount = fillAmount;
			this.fill.color = this.extraColor;
			this.text.text = string.Format("{0:0.00}", num2);
		}

		// Token: 0x04001106 RID: 4358
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001107 RID: 4359
		[SerializeField]
		private Image fill;

		// Token: 0x04001108 RID: 4360
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001109 RID: 4361
		[SerializeField]
		private Gradient normalColor;

		// Token: 0x0400110A RID: 4362
		[SerializeField]
		private Color extraColor = Color.red;
	}
}
