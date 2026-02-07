using System;
using System.Collections.Generic;
using UnityEngine;

namespace SodaCraft.Localizations
{
	// Token: 0x02000009 RID: 9
	public static class LocalizationManager
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000026F5 File Offset: 0x000008F5
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000026FC File Offset: 0x000008FC
		public static bool Initialized { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002704 File Offset: 0x00000904
		// (set) Token: 0x06000023 RID: 35 RVA: 0x0000270B File Offset: 0x0000090B
		public static SystemLanguage CurrentLanguage { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002713 File Offset: 0x00000913
		public static string CurrentLanguageDisplayName
		{
			get
			{
				return LocalizationManager.GetDisplayName(LocalizationManager.CurrentLanguage);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000271F File Offset: 0x0000091F
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002726 File Offset: 0x00000926
		public static LocalizationDataModel DataModel { get; private set; }

		// Token: 0x06000027 RID: 39 RVA: 0x0000272E File Offset: 0x0000092E
		public static void SetOverrideText(string key, string value)
		{
			LocalizationManager.overrideTexts[key] = value;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000273C File Offset: 0x0000093C
		public static bool TryGetOverrideText(string key, out string value)
		{
			return LocalizationManager.overrideTexts.TryGetValue(key, ref value);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000274A File Offset: 0x0000094A
		public static bool RemoveOverrideText(string key)
		{
			return LocalizationManager.overrideTexts.Remove(key);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002758 File Offset: 0x00000958
		public static void SetLanguage(SystemLanguage language)
		{
			LanguageSettings languageSettings = LocalizationDatabase.Instance.GetEntry(language);
			if (languageSettings == null)
			{
				languageSettings = LocalizationDatabase.Instance.GetDefaultEntry();
			}
			LocalizationManager.CurrentLanguage = languageSettings.Language;
			LocalizationManager.DataModel = new LocalizationDataModel(languageSettings);
			LocalizationManager.Initialized = true;
			PlayerPrefs.SetInt("language", (int)language);
			Action<SystemLanguage> onSetLanguage = LocalizationManager.OnSetLanguage;
			if (onSetLanguage == null)
			{
				return;
			}
			onSetLanguage.Invoke(language);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000027B8 File Offset: 0x000009B8
		public static LocalizationDataModel FallbackDataModel
		{
			get
			{
				if (LocalizationManager._fallbackDataModel == null)
				{
					LanguageSettings languageSettings = LocalizationDatabase.Instance.GetEntry(SystemLanguage.English);
					if (languageSettings == null)
					{
						languageSettings = LocalizationDatabase.Instance.GetDefaultEntry();
					}
					LocalizationManager._fallbackDataModel = new LocalizationDataModel(languageSettings);
				}
				return LocalizationManager._fallbackDataModel;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000027F8 File Offset: 0x000009F8
		public static string GetDisplayName(SystemLanguage language)
		{
			LocalizationDatabase instance = LocalizationDatabase.Instance;
			if (instance == null)
			{
				return language.ToString() + "?";
			}
			LanguageSettings entry = instance.GetEntry(language);
			if (entry == null)
			{
				return language.ToString() + "?";
			}
			return entry.GetDisplayName();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002855 File Offset: 0x00000A55
		private static void Initialize()
		{
			LocalizationManager.SetLanguage((SystemLanguage)PlayerPrefs.GetInt("language", (int)Application.systemLanguage));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000286C File Offset: 0x00000A6C
		public static string GetPlainText(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return "";
			}
			key = key.Trim();
			string result;
			if (LocalizationManager.TryGetOverrideText(key, out result))
			{
				return result;
			}
			if (LocalizationManager.DataModel == null)
			{
				LocalizationManager.Initialize();
			}
			string text = LocalizationManager.DataModel.GetPlainText(key);
			if (text == null)
			{
				text = LocalizationManager.FallbackDataModel.GetPlainText(key);
			}
			if (text == null)
			{
				text = "*" + key + "*";
			}
			return text;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000028D7 File Offset: 0x00000AD7
		public static string ToPlainText(this string key)
		{
			return LocalizationManager.GetPlainText(key);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000028E0 File Offset: 0x00000AE0
		public static void SetLanguage(string name)
		{
			if (LocalizationDatabase.Instance == null)
			{
				Debug.LogError("未配置本地化数据库");
				return;
			}
			object obj;
			if (!Enum.TryParse(typeof(SystemLanguage), name, ref obj))
			{
				Debug.LogError("语言解析失败 " + name);
				return;
			}
			if (obj is SystemLanguage)
			{
				SystemLanguage language = (SystemLanguage)obj;
				LocalizationManager.SetLanguage(language);
				return;
			}
			Debug.LogError("语言解析失败 " + name);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002954 File Offset: 0x00000B54
		public static void SetLanguage(int index)
		{
			LocalizationDatabase instance = LocalizationDatabase.Instance;
			if (instance == null)
			{
				Debug.LogError("未配置本地化数据库");
				return;
			}
			LocalizationManager.SetLanguage(instance.GetLanguageByIndex(index));
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002987 File Offset: 0x00000B87
		internal static void SetLanguage()
		{
			LocalizationManager.SetLanguage(LocalizationManager.CurrentLanguage);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000033 RID: 51 RVA: 0x00002994 File Offset: 0x00000B94
		// (remove) Token: 0x06000034 RID: 52 RVA: 0x000029C8 File Offset: 0x00000BC8
		public static event Action<SystemLanguage> OnSetLanguage;

		// Token: 0x0400000C RID: 12
		public static Dictionary<string, string> overrideTexts = new Dictionary<string, string>();

		// Token: 0x0400000D RID: 13
		private static LocalizationDataModel _fallbackDataModel;
	}
}
