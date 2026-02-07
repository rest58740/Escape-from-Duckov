using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Options.UI
{
	// Token: 0x0200026D RID: 621
	public class OptionsUIEntry_Slider : MonoBehaviour
	{
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x00049E6A File Offset: 0x0004806A
		// (set) Token: 0x0600138B RID: 5003 RVA: 0x00049E7C File Offset: 0x0004807C
		[LocalizationKey("Options")]
		private string labelKey
		{
			get
			{
				return "Options_" + this.key;
			}
			set
			{
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x00049E7E File Offset: 0x0004807E
		// (set) Token: 0x0600138D RID: 5005 RVA: 0x00049E91 File Offset: 0x00048091
		public float Value
		{
			get
			{
				return OptionsManager.Load<float>(this.key, this.defaultValue);
			}
			set
			{
				OptionsManager.Save<float>(this.key, value);
			}
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00049EA0 File Offset: 0x000480A0
		private void Awake()
		{
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.valueField.onEndEdit.AddListener(new UnityAction<string>(this.OnFieldEndEdit));
			this.RefreshLable();
			LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00049EFC File Offset: 0x000480FC
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00049F0F File Offset: 0x0004810F
		private void OnLanguageChanged(SystemLanguage language)
		{
			this.RefreshLable();
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00049F17 File Offset: 0x00048117
		private void RefreshLable()
		{
			if (this.label)
			{
				this.label.text = LocalizationManager.ToPlainText(this.labelKey);
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00049F3C File Offset: 0x0004813C
		private void OnFieldEndEdit(string arg0)
		{
			float num;
			if (float.TryParse(arg0, out num))
			{
				num = Mathf.Clamp(num, this.slider.minValue, this.slider.maxValue);
				this.Value = num;
			}
			this.RefreshValues();
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00049F7D File Offset: 0x0004817D
		private void OnEnable()
		{
			this.RefreshValues();
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00049F85 File Offset: 0x00048185
		private void OnSliderValueChanged(float value)
		{
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00049F94 File Offset: 0x00048194
		private void RefreshValues()
		{
			this.valueField.SetTextWithoutNotify(this.Value.ToString(this.valueFormat));
			this.slider.SetValueWithoutNotify(this.Value);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00049FD1 File Offset: 0x000481D1
		private void OnValidate()
		{
			this.RefreshLable();
		}

		// Token: 0x04000EB2 RID: 3762
		[SerializeField]
		private string key;

		// Token: 0x04000EB3 RID: 3763
		[Space]
		[SerializeField]
		private float defaultValue;

		// Token: 0x04000EB4 RID: 3764
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000EB5 RID: 3765
		[SerializeField]
		private Slider slider;

		// Token: 0x04000EB6 RID: 3766
		[SerializeField]
		private TMP_InputField valueField;

		// Token: 0x04000EB7 RID: 3767
		[SerializeField]
		private string valueFormat = "0";
	}
}
