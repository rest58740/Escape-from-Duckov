using System;
using System.IO;
using Saves;
using UnityEngine;

namespace Duckov.Options
{
	// Token: 0x0200026A RID: 618
	public class OptionsManager : MonoBehaviour
	{
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x00049977 File Offset: 0x00047B77
		private static string Folder
		{
			get
			{
				return SavesSystem.SavesFolder;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x0004997E File Offset: 0x00047B7E
		public static string FilePath
		{
			get
			{
				return Path.Combine(OptionsManager.Folder, "Options.ES3");
			}
		}

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x0600136F RID: 4975 RVA: 0x00049990 File Offset: 0x00047B90
		// (remove) Token: 0x06001370 RID: 4976 RVA: 0x000499C4 File Offset: 0x00047BC4
		public static event Action<string> OnOptionsChanged;

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x000499F7 File Offset: 0x00047BF7
		private static ES3Settings SaveSettings
		{
			get
			{
				if (OptionsManager._saveSettings == null)
				{
					OptionsManager._saveSettings = new ES3Settings(true);
					OptionsManager._saveSettings.path = OptionsManager.FilePath;
					OptionsManager._saveSettings.location = 0;
				}
				return OptionsManager._saveSettings;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x00049A2A File Offset: 0x00047C2A
		// (set) Token: 0x06001373 RID: 4979 RVA: 0x00049A3B File Offset: 0x00047C3B
		public static float MouseSensitivity
		{
			get
			{
				return OptionsManager.Load<float>("MouseSensitivity", 10f);
			}
			set
			{
				OptionsManager.Save<float>("MouseSensitivity", value);
			}
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00049A48 File Offset: 0x00047C48
		public static void Save<T>(string key, T obj)
		{
			if (string.IsNullOrEmpty(key))
			{
				return;
			}
			try
			{
				ES3.Save<T>(key, obj, OptionsManager.SaveSettings);
				Action<string> onOptionsChanged = OptionsManager.OnOptionsChanged;
				if (onOptionsChanged != null)
				{
					onOptionsChanged(key);
				}
				ES3.CreateBackup(OptionsManager.SaveSettings);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				Debug.LogError("Error: Failed saving options: " + key);
			}
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00049AB0 File Offset: 0x00047CB0
		public static T Load<T>(string key, T defaultValue = default(T))
		{
			T result;
			if (string.IsNullOrEmpty(key))
			{
				result = default(T);
				return result;
			}
			try
			{
				if (ES3.KeyExists(key, OptionsManager.SaveSettings))
				{
					result = ES3.Load<T>(key, OptionsManager.SaveSettings);
				}
				else
				{
					ES3.Save<T>(key, defaultValue, OptionsManager.SaveSettings);
					result = defaultValue;
				}
			}
			catch
			{
				if (ES3.RestoreBackup(OptionsManager.SaveSettings))
				{
					try
					{
						if (ES3.KeyExists(key, OptionsManager.SaveSettings))
						{
							return ES3.Load<T>(key, OptionsManager.SaveSettings);
						}
						ES3.Save<T>(key, defaultValue, OptionsManager.SaveSettings);
						return defaultValue;
					}
					catch
					{
						Debug.LogError("[OPTIONS MANAGER] Failed restoring backup");
					}
				}
				ES3.DeleteFile(OptionsManager.SaveSettings);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x04000EAA RID: 3754
		public const string FileName = "Options.ES3";

		// Token: 0x04000EAC RID: 3756
		private static ES3Settings _saveSettings;
	}
}
