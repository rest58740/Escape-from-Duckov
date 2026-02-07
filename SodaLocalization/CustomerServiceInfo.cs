using System;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class CustomerServiceInfo : MonoBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x0000209D File Offset: 0x0000029D
	private void Awake()
	{
		LocalizationManager.OnSetLanguage += new Action<SystemLanguage>(this.Refresh);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020B0 File Offset: 0x000002B0
	private void OnDestroy()
	{
		LocalizationManager.OnSetLanguage -= new Action<SystemLanguage>(this.Refresh);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000020C3 File Offset: 0x000002C3
	private void OnEnable()
	{
		this.Refresh(LocalizationManager.CurrentLanguage);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000020D0 File Offset: 0x000002D0
	private void Refresh(SystemLanguage language)
	{
		bool flag = language == SystemLanguage.ChineseSimplified;
		this.chineseInfo.SetActive(flag);
		this.nonChineseInfo.SetActive(!flag);
	}

	// Token: 0x04000003 RID: 3
	[SerializeField]
	private GameObject chineseInfo;

	// Token: 0x04000004 RID: 4
	[SerializeField]
	private GameObject nonChineseInfo;
}
