using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A9 RID: 937
	public class BarDisplay : MonoBehaviour
	{
		// Token: 0x060020BC RID: 8380 RVA: 0x00072DC4 File Offset: 0x00070FC4
		private void Awake()
		{
			this.fill.fillAmount = 0f;
			this.ApplyLook();
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00072DDC File Offset: 0x00070FDC
		public void Setup(string labelText, Color color, float current, float max, string format = "0.#", float min = 0f)
		{
			this.SetupLook(labelText, color);
			this.SetValue(current, max, format, min);
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00072DF3 File Offset: 0x00070FF3
		public void Setup(string labelText, Color color, int current, int max, int min = 0)
		{
			this.SetupLook(labelText, color);
			this.SetValue(current, max, min);
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x00072E08 File Offset: 0x00071008
		public void SetupLook(string labelText, Color color)
		{
			this.labelText = labelText;
			this.color = color;
			this.ApplyLook();
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x00072E1E File Offset: 0x0007101E
		private void ApplyLook()
		{
			this.text_Label.text = LocalizationManager.ToPlainText(this.labelText);
			this.fill.color = this.color;
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x00072E48 File Offset: 0x00071048
		public void SetValue(float current, float max, string format = "0.#", float min = 0f)
		{
			this.text_Current.text = current.ToString(format);
			this.text_Max.text = max.ToString(format);
			float num = max - min;
			float num2 = 1f;
			if (num > 0f)
			{
				num2 = (current - min) / num;
			}
			ShortcutExtensions.DOKill(this.fill, false);
			TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTweenModuleUI.DOFillAmount(this.fill, num2, this.animateDuration), 9);
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x00072EBC File Offset: 0x000710BC
		public void SetValue(int current, int max, int min = 0)
		{
			this.text_Current.text = current.ToString();
			this.text_Max.text = max.ToString();
			int num = max - min;
			float num2 = 1f;
			if (num > 0)
			{
				num2 = (float)(current - min) / (float)num;
			}
			ShortcutExtensions.DOKill(this.fill, false);
			TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTweenModuleUI.DOFillAmount(this.fill, num2, this.animateDuration), 9);
		}

		// Token: 0x0400165D RID: 5725
		[SerializeField]
		private string labelText;

		// Token: 0x0400165E RID: 5726
		[SerializeField]
		private Color color = Color.red;

		// Token: 0x0400165F RID: 5727
		[SerializeField]
		private float animateDuration = 0.25f;

		// Token: 0x04001660 RID: 5728
		[SerializeField]
		private TextMeshProUGUI text_Label;

		// Token: 0x04001661 RID: 5729
		[SerializeField]
		private TextMeshProUGUI text_Current;

		// Token: 0x04001662 RID: 5730
		[SerializeField]
		private TextMeshProUGUI text_Max;

		// Token: 0x04001663 RID: 5731
		[SerializeField]
		private Image fill;
	}
}
