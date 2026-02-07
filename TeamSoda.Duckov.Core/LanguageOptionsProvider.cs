using System;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class LanguageOptionsProvider : OptionsProviderBase
{
	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000E03 RID: 3587 RVA: 0x0003A377 File Offset: 0x00038577
	public override string Key
	{
		get
		{
			return "Language";
		}
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x0003A37E File Offset: 0x0003857E
	public override string GetCurrentOption()
	{
		return LocalizationManager.CurrentLanguageDisplayName;
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x0003A388 File Offset: 0x00038588
	public override string[] GetOptions()
	{
		LocalizationDatabase instance = LocalizationDatabase.Instance;
		if (instance == null)
		{
			return new string[]
			{
				"?"
			};
		}
		string[] languageDisplayNameList = instance.GetLanguageDisplayNameList();
		this.cache = languageDisplayNameList;
		return languageDisplayNameList;
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x0003A3C2 File Offset: 0x000385C2
	public override void Set(int index)
	{
		if (this.cache == null)
		{
			this.GetOptions();
		}
		if (index < 0 || index >= this.cache.Length)
		{
			Debug.LogError("语言越界");
			return;
		}
		LocalizationManager.SetLanguage(index);
	}

	// Token: 0x04000BFF RID: 3071
	private string[] cache;
}
