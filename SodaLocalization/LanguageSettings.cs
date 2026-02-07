using System;
using System.IO;
using MiniLocalizor;
using UnityEngine;

namespace SodaCraft.Localizations
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class LanguageSettings
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000261A File Offset: 0x0000081A
		public string path
		{
			get
			{
				return Path.Combine(Application.streamingAssetsPath, "Localization/" + this.language.ToString() + ".csv");
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002646 File Offset: 0x00000846
		public ILocalizationProvider Provider
		{
			get
			{
				if (this._provider == null)
				{
					this.LoadProvider();
				}
				return this._provider;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000265C File Offset: 0x0000085C
		private void LoadProvider()
		{
			this._provider = new CSVFileLocalizor(this.path);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000266F File Offset: 0x0000086F
		public string GetPlainText(string key)
		{
			key = key.Trim();
			return this.Provider.Get(key);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002685 File Offset: 0x00000885
		public string GetDisplayName()
		{
			return this.GetPlainText("language_name");
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002692 File Offset: 0x00000892
		internal void Reinitialize()
		{
			this.LoadProvider();
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000269A File Offset: 0x0000089A
		public SystemLanguage Language
		{
			get
			{
				return this.language;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026B4 File Offset: 0x000008B4
		public static LanguageSettings CreateFromPath(string path)
		{
			if (path.EndsWith(".xlsx"))
			{
				Debug.LogError("Wrong extension");
			}
			SystemLanguage systemLanguage;
			if (!Enum.TryParse<SystemLanguage>(Path.GetFileNameWithoutExtension(path), ref systemLanguage))
			{
				return null;
			}
			return new LanguageSettings
			{
				language = systemLanguage
			};
		}

		// Token: 0x04000007 RID: 7
		[SerializeField]
		private SystemLanguage language = SystemLanguage.Unknown;

		// Token: 0x04000008 RID: 8
		private CSVFileLocalizor _provider;
	}
}
