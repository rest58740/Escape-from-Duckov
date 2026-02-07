using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Saves
{
	// Token: 0x0200022F RID: 559
	public class SavesSystem
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x000426A0 File Offset: 0x000408A0
		// (set) Token: 0x060010FB RID: 4347 RVA: 0x000426FF File Offset: 0x000408FF
		public static int CurrentSlot
		{
			get
			{
				if (SavesSystem._currentSlot == null)
				{
					SavesSystem._currentSlot = new int?(PlayerPrefs.GetInt("CurrentSlot", 1));
					int? currentSlot = SavesSystem._currentSlot;
					int num = 1;
					if (currentSlot.GetValueOrDefault() < num & currentSlot != null)
					{
						SavesSystem._currentSlot = new int?(1);
					}
				}
				return SavesSystem._currentSlot.Value;
			}
			private set
			{
				SavesSystem._currentSlot = new int?(value);
				PlayerPrefs.SetInt("CurrentSlot", value);
				SavesSystem.CacheFile();
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x0004271C File Offset: 0x0004091C
		public static string CurrentFilePath
		{
			get
			{
				return SavesSystem.GetFilePath(SavesSystem.CurrentSlot);
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x00042728 File Offset: 0x00040928
		public static bool IsSaving
		{
			get
			{
				return SavesSystem.saving;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x0004272F File Offset: 0x0004092F
		public static string SavesFolder
		{
			get
			{
				return "Saves";
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00042736 File Offset: 0x00040936
		public static string GetFullPathToSavesFolder()
		{
			return Path.Combine(Application.persistentDataPath, SavesSystem.SavesFolder);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00042747 File Offset: 0x00040947
		public static string GetFilePath(int slot)
		{
			return Path.Combine(SavesSystem.SavesFolder, SavesSystem.GetSaveFileName(slot));
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00042759 File Offset: 0x00040959
		public static string GetSaveFileName(int slot)
		{
			return string.Format("Save_{0}.sav", slot);
		}

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06001102 RID: 4354 RVA: 0x0004276C File Offset: 0x0004096C
		// (remove) Token: 0x06001103 RID: 4355 RVA: 0x000427A0 File Offset: 0x000409A0
		public static event Action OnSetFile;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06001104 RID: 4356 RVA: 0x000427D4 File Offset: 0x000409D4
		// (remove) Token: 0x06001105 RID: 4357 RVA: 0x00042808 File Offset: 0x00040A08
		public static event Action OnSaveDeleted;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06001106 RID: 4358 RVA: 0x0004283C File Offset: 0x00040A3C
		// (remove) Token: 0x06001107 RID: 4359 RVA: 0x00042870 File Offset: 0x00040A70
		public static event Action OnCollectSaveData;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06001108 RID: 4360 RVA: 0x000428A4 File Offset: 0x00040AA4
		// (remove) Token: 0x06001109 RID: 4361 RVA: 0x000428D8 File Offset: 0x00040AD8
		public static event Action OnRestoreFailureDetected;

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x0004290B File Offset: 0x00040B0B
		// (set) Token: 0x0600110B RID: 4363 RVA: 0x00042912 File Offset: 0x00040B12
		public static bool RestoreFailureMarker { get; private set; }

		// Token: 0x0600110C RID: 4364 RVA: 0x0004291A File Offset: 0x00040B1A
		public static bool IsOldSave(int index)
		{
			return !SavesSystem.KeyExisits("CreatedWithVersion", index);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0004292A File Offset: 0x00040B2A
		public static void SetFile(int index)
		{
			SavesSystem.cached = false;
			SavesSystem.CurrentSlot = index;
			Action onSetFile = SavesSystem.OnSetFile;
			if (onSetFile == null)
			{
				return;
			}
			onSetFile();
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00042947 File Offset: 0x00040B47
		public static SavesSystem.BackupInfo[] GetBackupList()
		{
			return SavesSystem.GetBackupList(SavesSystem.CurrentSlot);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00042953 File Offset: 0x00040B53
		public static SavesSystem.BackupInfo[] GetBackupList(int slot)
		{
			return SavesSystem.GetBackupList(SavesSystem.GetFilePath(slot), slot);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00042964 File Offset: 0x00040B64
		public static SavesSystem.BackupInfo[] GetBackupList(string mainPath, int slot = -1)
		{
			SavesSystem.BackupInfo[] array = new SavesSystem.BackupInfo[10];
			for (int i = 0; i < 10; i++)
			{
				try
				{
					string backupPathByIndex = SavesSystem.GetBackupPathByIndex(mainPath, i);
					ES3Settings es3Settings = new ES3Settings(backupPathByIndex, null);
					es3Settings.location = 0;
					bool flag = ES3.FileExists(backupPathByIndex, es3Settings);
					long num = 0L;
					if (flag && ES3.KeyExists("SaveTime", backupPathByIndex, es3Settings))
					{
						num = ES3.Load<long>("SaveTime", backupPathByIndex, es3Settings);
					}
					DateTime.FromBinary(num);
					SavesSystem.BackupInfo backupInfo = new SavesSystem.BackupInfo
					{
						slot = slot,
						index = i,
						path = backupPathByIndex,
						exists = flag,
						time_raw = num
					};
					array[i] = backupInfo;
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					array[i] = default(SavesSystem.BackupInfo);
				}
			}
			return array;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00042A40 File Offset: 0x00040C40
		private static int GetEmptyOrOldestBackupIndex()
		{
			SavesSystem.BackupInfo[] backupList = SavesSystem.GetBackupList();
			int result = -1;
			DateTime t = DateTime.MaxValue;
			foreach (SavesSystem.BackupInfo backupInfo in backupList)
			{
				if (!backupInfo.exists)
				{
					return backupInfo.index;
				}
				if (backupInfo.Time < t)
				{
					result = backupInfo.index;
					t = backupInfo.Time;
				}
			}
			return result;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00042AA4 File Offset: 0x00040CA4
		private static int GetOldestBackupIndex()
		{
			SavesSystem.BackupInfo[] backupList = SavesSystem.GetBackupList();
			int result = -1;
			DateTime t = DateTime.MaxValue;
			foreach (SavesSystem.BackupInfo backupInfo in backupList)
			{
				if (backupInfo.exists && backupInfo.Time < t)
				{
					result = backupInfo.index;
					t = backupInfo.Time;
				}
			}
			return result;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00042B00 File Offset: 0x00040D00
		private static int GetNewestBackupIndex()
		{
			SavesSystem.BackupInfo[] backupList = SavesSystem.GetBackupList();
			int result = -1;
			DateTime t = DateTime.MinValue;
			foreach (SavesSystem.BackupInfo backupInfo in backupList)
			{
				if (backupInfo.exists && backupInfo.Time > t)
				{
					result = backupInfo.index;
					t = backupInfo.Time;
				}
			}
			return result;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00042B5B File Offset: 0x00040D5B
		private static string GetBackupPathByIndex(int index)
		{
			return SavesSystem.GetBackupPathByIndex(SavesSystem.CurrentSlot, index);
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00042B68 File Offset: 0x00040D68
		private static string GetBackupPathByIndex(int slot, int index)
		{
			return SavesSystem.GetBackupPathByIndex(SavesSystem.GetFilePath(slot), index);
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00042B76 File Offset: 0x00040D76
		private static string GetBackupPathByIndex(string path, int index)
		{
			return string.Format("{0}.bac.{1:00}", path, index + 1);
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00042B8C File Offset: 0x00040D8C
		private static void CreateIndexedBackup(int index = -1)
		{
			SavesSystem.LastIndexedBackupTime = DateTime.UtcNow;
			try
			{
				if (index < 0)
				{
					index = SavesSystem.GetEmptyOrOldestBackupIndex();
				}
				string backupPathByIndex = SavesSystem.GetBackupPathByIndex(index);
				ES3.DeleteFile(backupPathByIndex, SavesSystem.settings);
				ES3.CopyFile(SavesSystem.CurrentFilePath, backupPathByIndex);
				ES3.StoreCachedFile(backupPathByIndex);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				Debug.Log("[Saves] Failed creating indexed backup");
			}
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00042BF4 File Offset: 0x00040DF4
		private static void CreateBackup()
		{
			try
			{
				SavesSystem.CreateBackup(SavesSystem.CurrentFilePath);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				Debug.Log("[Saves] Failed creating backup");
			}
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00042C30 File Offset: 0x00040E30
		private static void CreateBackup(string path)
		{
			try
			{
				string text = path + ".bac";
				ES3.DeleteFile(text, SavesSystem.settings);
				ES3.CreateBackup(path);
				ES3.StoreCachedFile(text);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				Debug.Log("[Saves] Failed creating backup for path " + path);
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00042C88 File Offset: 0x00040E88
		public static void UpgradeSaveFileAssemblyInfo(string path)
		{
			if (!File.Exists(path))
			{
				Debug.Log("没有找到存档文件：" + path);
				return;
			}
			string text;
			using (StreamReader streamReader = File.OpenText(path))
			{
				text = streamReader.ReadToEnd();
				if (text.Contains("TeamSoda.Duckov.Core"))
				{
					streamReader.Close();
					return;
				}
				text = text.Replace("Assembly-CSharp", "TeamSoda.Duckov.Core");
				streamReader.Close();
			}
			File.Delete(path);
			using (FileStream fileStream = File.OpenWrite(path))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.Write(text);
				streamWriter.Close();
				fileStream.Close();
			}
			Debug.Log("存档格式已更新：" + path);
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00042D54 File Offset: 0x00040F54
		public static void RestoreIndexedBackup(int slot, int index)
		{
			string backupPathByIndex = SavesSystem.GetBackupPathByIndex(slot, index);
			SavesSystem.UpgradeSaveFileAssemblyInfo(Path.Combine(Application.persistentDataPath, backupPathByIndex));
			string filePath = SavesSystem.GetFilePath(slot);
			string text = filePath + ".bac";
			try
			{
				ES3.CacheFile(backupPathByIndex);
				ES3.DeleteFile(text, SavesSystem.settings);
				ES3.CopyFile(backupPathByIndex, text);
				ES3.DeleteFile(filePath, SavesSystem.settings);
				ES3.RestoreBackup(filePath, SavesSystem.settings);
				ES3.StoreCachedFile(filePath);
				ES3.CacheFile(filePath);
				Action onSetFile = SavesSystem.OnSetFile;
				if (onSetFile != null)
				{
					onSetFile();
				}
			}
			catch
			{
				SavesSystem.RestoreFailureMarker = true;
				Debug.LogError("文件损坏，且无法修复。");
				ES3.DeleteFile(filePath);
				File.Delete(filePath);
				ES3.Save<bool>("Created", true, filePath);
				ES3.StoreCachedFile(filePath);
				ES3.CacheFile(filePath);
				Action onRestoreFailureDetected = SavesSystem.OnRestoreFailureDetected;
				if (onRestoreFailureDetected != null)
				{
					onRestoreFailureDetected();
				}
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00042E30 File Offset: 0x00041030
		private static bool RestoreBackup(string path)
		{
			bool flag = false;
			try
			{
				string text = path + ".bac";
				SavesSystem.UpgradeSaveFileAssemblyInfo(Path.Combine(Application.persistentDataPath, text));
				ES3.CacheFile(text);
				ES3.DeleteFile(path, SavesSystem.settings);
				ES3.RestoreBackup(path, SavesSystem.settings);
				ES3.StoreCachedFile(path);
				ES3.CacheFile(path);
				ES3.CacheFile(path);
				flag = true;
			}
			catch
			{
				Debug.Log("默认备份损坏。");
			}
			if (!flag)
			{
				SavesSystem.RestoreFailureMarker = true;
				Debug.LogError("恢复默认备份失败");
				ES3.DeleteFile(path);
				ES3.Save<bool>("Created", true, path);
				ES3.StoreCachedFile(path);
				ES3.CacheFile(path);
				Action onRestoreFailureDetected = SavesSystem.OnRestoreFailureDetected;
				if (onRestoreFailureDetected != null)
				{
					onRestoreFailureDetected();
				}
			}
			return flag;
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x00042EF0 File Offset: 0x000410F0
		// (set) Token: 0x0600111E RID: 4382 RVA: 0x00042F17 File Offset: 0x00041117
		private static DateTime LastSavedTime
		{
			get
			{
				if (SavesSystem._lastSavedTime > DateTime.UtcNow)
				{
					SavesSystem._lastSavedTime = DateTime.UtcNow;
					GameManager.TimeTravelDetected();
				}
				return SavesSystem._lastSavedTime;
			}
			set
			{
				SavesSystem._lastSavedTime = value;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x00042F1F File Offset: 0x0004111F
		private static TimeSpan TimeSinceLastSave
		{
			get
			{
				return DateTime.UtcNow - SavesSystem.LastSavedTime;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001120 RID: 4384 RVA: 0x00042F30 File Offset: 0x00041130
		// (set) Token: 0x06001121 RID: 4385 RVA: 0x00042F57 File Offset: 0x00041157
		private static DateTime LastIndexedBackupTime
		{
			get
			{
				if (SavesSystem._lastIndexedBackupTime > DateTime.UtcNow)
				{
					SavesSystem._lastIndexedBackupTime = DateTime.UtcNow;
					GameManager.TimeTravelDetected();
				}
				return SavesSystem._lastIndexedBackupTime;
			}
			set
			{
				SavesSystem._lastIndexedBackupTime = value;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x00042F5F File Offset: 0x0004115F
		private static TimeSpan TimeSinceLastIndexedBackup
		{
			get
			{
				return DateTime.UtcNow - SavesSystem.LastIndexedBackupTime;
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00042F70 File Offset: 0x00041170
		public DateTime GetSaveTimeUTC(int slot = -1)
		{
			if (slot < 0)
			{
				slot = SavesSystem.CurrentSlot;
			}
			if (!SavesSystem.KeyExisits("SaveTime", slot))
			{
				return default(DateTime);
			}
			return DateTime.FromBinary(SavesSystem.Load<long>("SaveTime", slot));
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00042FB0 File Offset: 0x000411B0
		public DateTime GetSaveTimeLocal(int slot = -1)
		{
			if (slot < 0)
			{
				slot = SavesSystem.CurrentSlot;
			}
			DateTime saveTimeUTC = this.GetSaveTimeUTC(slot);
			if (saveTimeUTC == default(DateTime))
			{
				return default(DateTime);
			}
			return saveTimeUTC.ToLocalTime();
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00042FF4 File Offset: 0x000411F4
		public static void SaveFile(bool writeSaveTime = true)
		{
			TimeSpan timeSinceLastIndexedBackup = SavesSystem.TimeSinceLastIndexedBackup;
			SavesSystem.LastSavedTime = DateTime.UtcNow;
			if (writeSaveTime)
			{
				SavesSystem.Save<long>("SaveTime", DateTime.UtcNow.ToBinary());
			}
			SavesSystem.saving = true;
			SavesSystem.CreateBackup();
			if (timeSinceLastIndexedBackup > TimeSpan.FromMinutes(5.0))
			{
				SavesSystem.CreateIndexedBackup(-1);
			}
			SavesSystem.SetAsOldGame();
			ES3.StoreCachedFile(SavesSystem.CurrentFilePath);
			SavesSystem.saving = false;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00043065 File Offset: 0x00041265
		private static void CacheFile()
		{
			SavesSystem.CacheFile(SavesSystem.CurrentSlot);
			SavesSystem.cached = true;
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00043078 File Offset: 0x00041278
		private static void CacheFile(int slot)
		{
			if (slot == SavesSystem.CurrentSlot && SavesSystem.cached)
			{
				return;
			}
			string filePath = SavesSystem.GetFilePath(slot);
			if (!SavesSystem.CacheFile(filePath))
			{
				Debug.Log("尝试恢复 indexed backups");
				List<SavesSystem.BackupInfo> list = (from e in SavesSystem.GetBackupList(filePath, slot)
				where e.exists
				select e).ToList<SavesSystem.BackupInfo>();
				list.Sort(delegate(SavesSystem.BackupInfo a, SavesSystem.BackupInfo b)
				{
					if (!(a.Time > b.Time))
					{
						return 1;
					}
					return -1;
				});
				if (list.Count > 0)
				{
					for (int i = 0; i < list.Count; i++)
					{
						SavesSystem.BackupInfo backupInfo = list[i];
						try
						{
							Debug.Log(string.Format("Restoreing {0}.bac.{1} \t", slot, backupInfo.index) + backupInfo.Time.ToString("MM/dd HH:mm:ss"));
							SavesSystem.RestoreIndexedBackup(slot, backupInfo.index);
							break;
						}
						catch
						{
							Debug.LogError(string.Format("slot:{0} backup_index:{1} 恢复失败。", slot, backupInfo.index));
						}
					}
				}
			}
			if (!ES3.FileExists(filePath))
			{
				ES3.Save<bool>("Created", true, filePath);
				ES3.StoreCachedFile(filePath);
				ES3.CacheFile(filePath);
			}
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x000431D4 File Offset: 0x000413D4
		private static bool CacheFile(string path)
		{
			bool result;
			try
			{
				ES3.CacheFile(path);
				result = true;
			}
			catch
			{
				result = SavesSystem.RestoreBackup(path);
			}
			return result;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00043208 File Offset: 0x00041408
		public static void Save<T>(string prefix, string key, T value)
		{
			SavesSystem.Save<T>(prefix + key, value);
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00043217 File Offset: 0x00041417
		public static void Save<T>(string realKey, T value)
		{
			if (!SavesSystem.cached)
			{
				SavesSystem.CacheFile();
			}
			if (string.IsNullOrWhiteSpace(SavesSystem.CurrentFilePath))
			{
				Debug.Log("Save failed " + realKey);
				return;
			}
			ES3.Save<T>(realKey, value, SavesSystem.CurrentFilePath);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0004324E File Offset: 0x0004144E
		public static T Load<T>(string prefix, string key)
		{
			return SavesSystem.Load<T>(prefix + key);
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0004325C File Offset: 0x0004145C
		public static T Load<T>(string realKey)
		{
			if (!SavesSystem.cached)
			{
				SavesSystem.CacheFile();
			}
			string.IsNullOrWhiteSpace(realKey);
			if (ES3.KeyExists(realKey, SavesSystem.CurrentFilePath))
			{
				return ES3.Load<T>(realKey, SavesSystem.CurrentFilePath);
			}
			return default(T);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0004329E File Offset: 0x0004149E
		public static bool KeyExisits(string prefix, string key)
		{
			return ES3.KeyExists(prefix + key);
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x000432AC File Offset: 0x000414AC
		public static bool KeyExisits(string realKey)
		{
			if (!SavesSystem.cached)
			{
				SavesSystem.CacheFile();
			}
			return ES3.KeyExists(realKey, SavesSystem.CurrentFilePath);
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x000432C8 File Offset: 0x000414C8
		public static bool KeyExisits(string realKey, int slotIndex)
		{
			if (slotIndex == SavesSystem.CurrentSlot)
			{
				return SavesSystem.KeyExisits(realKey);
			}
			string filePath = SavesSystem.GetFilePath(slotIndex);
			SavesSystem.CacheFile(slotIndex);
			return ES3.KeyExists(realKey, filePath);
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x000432F8 File Offset: 0x000414F8
		public static T Load<T>(string realKey, int slotIndex)
		{
			if (slotIndex == SavesSystem.CurrentSlot)
			{
				return SavesSystem.Load<T>(realKey);
			}
			string filePath = SavesSystem.GetFilePath(slotIndex);
			SavesSystem.CacheFile(slotIndex);
			if (ES3.KeyExists(realKey, filePath))
			{
				return ES3.Load<T>(realKey, filePath);
			}
			return default(T);
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x0004333B File Offset: 0x0004153B
		public static string GlobalSaveDataFilePath
		{
			get
			{
				return Path.Combine(SavesSystem.SavesFolder, SavesSystem.GlobalSaveDataFileName);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001132 RID: 4402 RVA: 0x0004334C File Offset: 0x0004154C
		public static string GlobalSaveDataFileName
		{
			get
			{
				return "Global.json";
			}
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00043353 File Offset: 0x00041553
		public static void SaveGlobal<T>(string key, T value)
		{
			if (!SavesSystem.globalCached)
			{
				SavesSystem.CacheFile(SavesSystem.GlobalSaveDataFilePath);
				SavesSystem.globalCached = true;
			}
			ES3.Save<T>(key, value, SavesSystem.GlobalSaveDataFilePath);
			SavesSystem.CreateBackup(SavesSystem.GlobalSaveDataFilePath);
			ES3.StoreCachedFile(SavesSystem.GlobalSaveDataFilePath);
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0004338D File Offset: 0x0004158D
		public static T LoadGlobal<T>(string key, T defaultValue = default(T))
		{
			if (!SavesSystem.globalCached)
			{
				SavesSystem.CacheFile(SavesSystem.GlobalSaveDataFilePath);
				SavesSystem.globalCached = true;
			}
			if (ES3.KeyExists(key, SavesSystem.GlobalSaveDataFilePath))
			{
				return ES3.Load<T>(key, SavesSystem.GlobalSaveDataFilePath);
			}
			return defaultValue;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000433C1 File Offset: 0x000415C1
		public static void CollectSaveData()
		{
			Action onCollectSaveData = SavesSystem.OnCollectSaveData;
			if (onCollectSaveData == null)
			{
				return;
			}
			onCollectSaveData();
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x000433D2 File Offset: 0x000415D2
		public static bool IsOldGame()
		{
			return SavesSystem.Load<bool>("IsOldGame");
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x000433DE File Offset: 0x000415DE
		public static bool IsOldGame(int index)
		{
			return SavesSystem.Load<bool>("IsOldGame", index);
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x000433EB File Offset: 0x000415EB
		private static void SetAsOldGame()
		{
			SavesSystem.Save<bool>("IsOldGame", true);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000433F8 File Offset: 0x000415F8
		public static void DeleteCurrentSave()
		{
			ES3.CacheFile(SavesSystem.CurrentFilePath);
			ES3.DeleteFile(SavesSystem.CurrentFilePath);
			ES3.Save<bool>("Created", false, SavesSystem.CurrentFilePath);
			ES3.StoreCachedFile(SavesSystem.CurrentFilePath);
			Debug.Log(string.Format("已删除存档{0}", SavesSystem.CurrentSlot));
			Action onSaveDeleted = SavesSystem.OnSaveDeleted;
			if (onSaveDeleted == null)
			{
				return;
			}
			onSaveDeleted();
		}

		// Token: 0x04000D93 RID: 3475
		private static int? _currentSlot = null;

		// Token: 0x04000D94 RID: 3476
		private static bool saving;

		// Token: 0x04000D95 RID: 3477
		private static ES3Settings settings = ES3Settings.defaultSettings;

		// Token: 0x04000D96 RID: 3478
		private static bool cached;

		// Token: 0x04000D9C RID: 3484
		private const int BackupListCount = 10;

		// Token: 0x04000D9D RID: 3485
		private static DateTime _lastSavedTime = DateTime.MinValue;

		// Token: 0x04000D9E RID: 3486
		private static DateTime _lastIndexedBackupTime = DateTime.MinValue;

		// Token: 0x04000D9F RID: 3487
		private static bool globalCached;

		// Token: 0x04000DA0 RID: 3488
		private static ES3Settings GlobalFileSetting = new ES3Settings(null, null)
		{
			location = 0
		};

		// Token: 0x0200052B RID: 1323
		public struct BackupInfo
		{
			// Token: 0x17000780 RID: 1920
			// (get) Token: 0x060028A9 RID: 10409 RVA: 0x000955DE File Offset: 0x000937DE
			public bool TimeValid
			{
				get
				{
					return this.time_raw > 0L;
				}
			}

			// Token: 0x17000781 RID: 1921
			// (get) Token: 0x060028AA RID: 10410 RVA: 0x000955EA File Offset: 0x000937EA
			public DateTime Time
			{
				get
				{
					return DateTime.FromBinary(this.time_raw);
				}
			}

			// Token: 0x04001EB9 RID: 7865
			public int slot;

			// Token: 0x04001EBA RID: 7866
			public int index;

			// Token: 0x04001EBB RID: 7867
			public string path;

			// Token: 0x04001EBC RID: 7868
			public bool exists;

			// Token: 0x04001EBD RID: 7869
			public long time_raw;
		}
	}
}
