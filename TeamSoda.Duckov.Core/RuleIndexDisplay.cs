using System;
using Duckov.Rules;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class RuleIndexDisplay : MonoBehaviour
{
	// Token: 0x06000B12 RID: 2834 RVA: 0x000307B2 File Offset: 0x0002E9B2
	private void Awake()
	{
		LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x000307C5 File Offset: 0x0002E9C5
	private void OnDestroy()
	{
		LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x000307D8 File Offset: 0x0002E9D8
	private void OnLanguageChanged(SystemLanguage language)
	{
		this.Refresh();
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x000307E0 File Offset: 0x0002E9E0
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x000307E8 File Offset: 0x0002E9E8
	private void Refresh()
	{
		this.text.text = LocalizationManager.ToPlainText(string.Format("Rule_{0}", GameRulesManager.SelectedRuleIndex));
	}

	// Token: 0x040009B9 RID: 2489
	[SerializeField]
	private TextMeshProUGUI text;
}
