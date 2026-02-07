using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Options.UI
{
	// Token: 0x0200026E RID: 622
	public class OptionsUIEntry_Toggle : MonoBehaviour
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x00049FEC File Offset: 0x000481EC
		// (set) Token: 0x06001399 RID: 5017 RVA: 0x00049FFE File Offset: 0x000481FE
		[LocalizationKey("Default")]
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

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x0004A000 File Offset: 0x00048200
		// (set) Token: 0x0600139B RID: 5019 RVA: 0x0004A013 File Offset: 0x00048213
		public bool Value
		{
			get
			{
				return OptionsManager.Load<bool>(this.key, this.defaultValue);
			}
			set
			{
				OptionsManager.Save<bool>(this.key, value);
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x0004A021 File Offset: 0x00048221
		private int SliderValue
		{
			get
			{
				if (!this.Value)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0004A030 File Offset: 0x00048230
		private void Awake()
		{
			this.toggle.wholeNumbers = true;
			this.toggle.minValue = 0f;
			this.toggle.maxValue = 1f;
			this.toggle.onValueChanged.AddListener(new UnityAction<float>(this.OnToggleValueChanged));
			this.label.text = LocalizationManager.ToPlainText(this.labelKey);
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0004A09B File Offset: 0x0004829B
		private void OnEnable()
		{
			this.toggle.SetValueWithoutNotify((float)this.SliderValue);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0004A0AF File Offset: 0x000482AF
		private void OnToggleValueChanged(float value)
		{
			this.Value = (value > 0f);
		}

		// Token: 0x04000EB8 RID: 3768
		[SerializeField]
		private string key;

		// Token: 0x04000EB9 RID: 3769
		[SerializeField]
		private bool defaultValue;

		// Token: 0x04000EBA RID: 3770
		[Space]
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000EBB RID: 3771
		[SerializeField]
		private Slider toggle;
	}
}
