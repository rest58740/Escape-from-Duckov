using System;
using System.Collections.Generic;
using System.Linq;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Duckov.Options.UI
{
	// Token: 0x0200026C RID: 620
	public class OptionsUIEntry_Dropdown : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x00049C9C File Offset: 0x00047E9C
		private string optionKey
		{
			get
			{
				if (this.provider == null)
				{
					return "";
				}
				return this.provider.Key;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x00049CBD File Offset: 0x00047EBD
		// (set) Token: 0x06001380 RID: 4992 RVA: 0x00049CCF File Offset: 0x00047ECF
		[LocalizationKey("Options")]
		public string LabelKey
		{
			get
			{
				return "Options_" + this.optionKey;
			}
			set
			{
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00049CD1 File Offset: 0x00047ED1
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.SetupDropdown();
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00049CDC File Offset: 0x00047EDC
		private void SetupDropdown()
		{
			if (!this.provider)
			{
				return;
			}
			List<string> list = this.provider.GetOptions().ToList<string>();
			string currentOption = this.provider.GetCurrentOption();
			int num = list.IndexOf(currentOption);
			if (num < 0)
			{
				list.Insert(0, currentOption);
				num = 0;
			}
			this.dropdown.ClearOptions();
			this.dropdown.AddOptions(list.ToList<string>());
			this.dropdown.SetValueWithoutNotify(num);
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00049D54 File Offset: 0x00047F54
		private void Awake()
		{
			LocalizationManager.OnSetLanguage += this.OnSetLanguage;
			this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnDropdownValueChanged));
			this.label.text = LocalizationManager.ToPlainText(this.LabelKey);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00049DA4 File Offset: 0x00047FA4
		private void Start()
		{
			this.SetupDropdown();
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00049DAC File Offset: 0x00047FAC
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= this.OnSetLanguage;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00049DBF File Offset: 0x00047FBF
		private void OnSetLanguage(SystemLanguage language)
		{
			this.SetupDropdown();
			this.label.text = LocalizationManager.ToPlainText(this.LabelKey);
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00049DE0 File Offset: 0x00047FE0
		private void OnDropdownValueChanged(int index)
		{
			if (!this.provider)
			{
				return;
			}
			int num = this.provider.GetOptions().ToList<string>().IndexOf(this.dropdown.options[index].text);
			if (num >= 0)
			{
				this.provider.Set(num);
			}
			this.SetupDropdown();
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00049E3D File Offset: 0x0004803D
		private void OnValidate()
		{
			if (this.label)
			{
				this.label.text = LocalizationManager.ToPlainText(this.LabelKey);
			}
		}

		// Token: 0x04000EAF RID: 3759
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000EB0 RID: 3760
		[SerializeField]
		private OptionsProviderBase provider;

		// Token: 0x04000EB1 RID: 3761
		[SerializeField]
		private TMP_Dropdown dropdown;
	}
}
