using System;
using SodaCraft.Localizations;

// Token: 0x02000003 RID: 3
public class LocalizationDataModel
{
	// Token: 0x06000002 RID: 2 RVA: 0x0000205F File Offset: 0x0000025F
	public LocalizationDataModel(LanguageSettings settings)
	{
		this.settings = settings;
		settings.Reinitialize();
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002074 File Offset: 0x00000274
	public string DisplayName
	{
		get
		{
			if (this.settings == null)
			{
				return "?";
			}
			return this.settings.GetDisplayName();
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000208F File Offset: 0x0000028F
	public string GetPlainText(string key)
	{
		return this.settings.GetPlainText(key);
	}

	// Token: 0x04000002 RID: 2
	internal LanguageSettings settings;
}
