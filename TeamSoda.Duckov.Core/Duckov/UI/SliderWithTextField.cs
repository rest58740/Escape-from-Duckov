using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A7 RID: 935
	public class SliderWithTextField : MonoBehaviour
	{
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x000728D0 File Offset: 0x00070AD0
		// (set) Token: 0x060020A1 RID: 8353 RVA: 0x000728D8 File Offset: 0x00070AD8
		[LocalizationKey("Default")]
		public string LabelKey
		{
			get
			{
				return this._labelKey;
			}
			set
			{
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x000728DA File Offset: 0x00070ADA
		// (set) Token: 0x060020A3 RID: 8355 RVA: 0x000728E2 File Offset: 0x00070AE2
		public float Value
		{
			get
			{
				return this.GetValue();
			}
			set
			{
				this.SetValue(value);
			}
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000728EB File Offset: 0x00070AEB
		public void SetValueWithoutNotify(float value)
		{
			this.value = value;
			this.RefreshValues();
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x000728FA File Offset: 0x00070AFA
		public void SetValue(float value)
		{
			this.SetValueWithoutNotify(value);
			Action<float> action = this.onValueChanged;
			if (action == null)
			{
				return;
			}
			action(value);
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x00072914 File Offset: 0x00070B14
		public float GetValue()
		{
			return this.value;
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x0007291C File Offset: 0x00070B1C
		private void Awake()
		{
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.valueField.onEndEdit.AddListener(new UnityAction<string>(this.OnFieldEndEdit));
			this.RefreshLable();
			LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x00072978 File Offset: 0x00070B78
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x0007298B File Offset: 0x00070B8B
		private void OnLanguageChanged(SystemLanguage language)
		{
			this.RefreshLable();
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00072993 File Offset: 0x00070B93
		private void RefreshLable()
		{
			if (this.label)
			{
				this.label.text = LocalizationManager.ToPlainText(this.LabelKey);
			}
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000729B8 File Offset: 0x00070BB8
		private void OnFieldEndEdit(string arg0)
		{
			float num;
			if (float.TryParse(arg0, out num))
			{
				if (this.isPercentage)
				{
					num /= 100f;
				}
				num = Mathf.Clamp(num, this.slider.minValue, this.slider.maxValue);
				this.Value = num;
			}
			this.RefreshValues();
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00072A09 File Offset: 0x00070C09
		private void OnEnable()
		{
			this.RefreshValues();
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00072A11 File Offset: 0x00070C11
		private void OnSliderValueChanged(float value)
		{
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00072A20 File Offset: 0x00070C20
		private void RefreshValues()
		{
			this.valueField.SetTextWithoutNotify(this.Value.ToString(this.valueFormat));
			this.slider.SetValueWithoutNotify(this.Value);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x00072A5D File Offset: 0x00070C5D
		private void OnValidate()
		{
			this.RefreshLable();
		}

		// Token: 0x04001645 RID: 5701
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04001646 RID: 5702
		[SerializeField]
		private Slider slider;

		// Token: 0x04001647 RID: 5703
		[SerializeField]
		private TMP_InputField valueField;

		// Token: 0x04001648 RID: 5704
		[SerializeField]
		private string valueFormat = "0";

		// Token: 0x04001649 RID: 5705
		[SerializeField]
		private bool isPercentage;

		// Token: 0x0400164A RID: 5706
		[SerializeField]
		private string _labelKey = "?";

		// Token: 0x0400164B RID: 5707
		[SerializeField]
		private float value;

		// Token: 0x0400164C RID: 5708
		public Action<float> onValueChanged;
	}
}
