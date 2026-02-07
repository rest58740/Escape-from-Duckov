using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SodaCraft.Localizations
{
	// Token: 0x02000007 RID: 7
	[CreateAssetMenu]
	public class LocalizationDatabase : ScriptableObject
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000247E File Offset: 0x0000067E
		public static LocalizationDatabase Instance
		{
			get
			{
				if (LocalizationDatabase._instance == null)
				{
					LocalizationDatabase._instance = Resources.Load<LocalizationDatabase>("LocalizationDatabase");
					if (LocalizationDatabase._instance == null)
					{
						Debug.LogError("需要配置文件LocalizationDatabase。放在Resources文件夹里。");
					}
				}
				return LocalizationDatabase._instance;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000024B8 File Offset: 0x000006B8
		private List<LanguageSettings> Entries
		{
			get
			{
				if (this._e == null || this._e.Count == 0)
				{
					this._e = new List<LanguageSettings>();
					string text = Application.streamingAssetsPath + "/Localization";
					Debug.Log(text);
					foreach (string path in from e in Directory.GetFiles(text)
					where e.EndsWith(".csv")
					select e)
					{
						LanguageSettings languageSettings = LanguageSettings.CreateFromPath(path);
						if (languageSettings != null)
						{
							this._e.Add(languageSettings);
						}
					}
				}
				return this._e;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002574 File Offset: 0x00000774
		public LanguageSettings GetEntry(SystemLanguage language)
		{
			return this.Entries.Find((LanguageSettings e) => e != null && e.Language == language);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025A5 File Offset: 0x000007A5
		internal LanguageSettings GetDefaultEntry()
		{
			return this.GetEntry(SystemLanguage.English);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025AF File Offset: 0x000007AF
		public string[] GetLanguageDisplayNameList()
		{
			if (this.Entries == null)
			{
				return null;
			}
			return (from e in this.Entries
			select e.GetDisplayName()).ToArray<string>();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025EA File Offset: 0x000007EA
		internal SystemLanguage GetLanguageByIndex(int index)
		{
			if (index < 0 || index >= this.Entries.Count)
			{
				return SystemLanguage.Unknown;
			}
			return this.Entries[index].Language;
		}

		// Token: 0x04000005 RID: 5
		private static LocalizationDatabase _instance;

		// Token: 0x04000006 RID: 6
		private List<LanguageSettings> _e;
	}
}
