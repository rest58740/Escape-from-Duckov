using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Saves;
using Sirenix.Utilities;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x02000278 RID: 632
	public class ModManager : MonoBehaviour
	{
		// Token: 0x14000086 RID: 134
		// (add) Token: 0x060013E0 RID: 5088 RVA: 0x0004A778 File Offset: 0x00048978
		// (remove) Token: 0x060013E1 RID: 5089 RVA: 0x0004A7AC File Offset: 0x000489AC
		public static event Action OnReorder;

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0004A7DF File Offset: 0x000489DF
		// (set) Token: 0x060013E3 RID: 5091 RVA: 0x0004A7EC File Offset: 0x000489EC
		public static bool AllowActivatingMod
		{
			get
			{
				return SavesSystem.LoadGlobal<bool>("AllowLoadingMod", false);
			}
			set
			{
				SavesSystem.SaveGlobal<bool>("AllowLoadingMod", value);
				if (ModManager.Instance != null && value)
				{
					ModManager.Instance.ScanAndActivateMods();
				}
			}
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0004A814 File Offset: 0x00048A14
		private List<string> GetActiveModList()
		{
			return (from e in this.activeMods
			where e.Value != null
			select e.Value.info.name).ToList<string>();
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0004A874 File Offset: 0x00048A74
		private void SaveActiveModList()
		{
			List<string> activeModList = this.GetActiveModList();
			SavesSystem.Save<List<string>>("ActiveModList", activeModList);
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0004A894 File Offset: 0x00048A94
		public static List<string> LoadLastActiveModList()
		{
			List<string> list = SavesSystem.Load<List<string>>("ActiveModList");
			if (list == null)
			{
				return new List<string>();
			}
			return list;
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0004A8B6 File Offset: 0x00048AB6
		public static List<string> GetCurrentActiveModList()
		{
			if (ModManager.Instance == null)
			{
				return new List<string>();
			}
			return ModManager.Instance.GetActiveModList();
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0004A8D5 File Offset: 0x00048AD5
		private void Awake()
		{
			if (this.modParent == null)
			{
				this.modParent = base.transform;
			}
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0004A902 File Offset: 0x00048B02
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0004A915 File Offset: 0x00048B15
		private void OnCollectSaveData()
		{
			this.SaveActiveModList();
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0004A91D File Offset: 0x00048B1D
		private void Start()
		{
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0004A920 File Offset: 0x00048B20
		public void ScanAndActivateMods()
		{
			if (!ModManager.AllowActivatingMod)
			{
				return;
			}
			ModManager.Rescan();
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				if (!this.activeMods.ContainsKey(modInfo.name))
				{
					bool flag = this.ShouldActivateMod(modInfo);
					Debug.Log(string.Format("ModActive_{0}: {1}", modInfo.name, flag));
					if (flag && this.ActivateMod(modInfo) == null)
					{
						this.SetShouldActivateMod(modInfo, false);
					}
				}
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0004A9C8 File Offset: 0x00048BC8
		private static void SortModInfosByPriority()
		{
			ModManager.modInfos.Sort(delegate(ModInfo a, ModInfo b)
			{
				int modPriority = ModManager.GetModPriority(a.name);
				int modPriority2 = ModManager.GetModPriority(b.name);
				if (modPriority == modPriority2)
				{
					return 0;
				}
				if (modPriority >= 2147483647)
				{
					return 1;
				}
				if (modPriority2 >= 2147483647)
				{
					return -1;
				}
				return modPriority - modPriority2;
			});
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Sorted mods:");
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				stringBuilder.AppendLine(modInfo.name);
			}
			Debug.Log(stringBuilder);
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x0004AA64 File Offset: 0x00048C64
		private static ES3Settings settings
		{
			get
			{
				if (ModManager._settings == null)
				{
					ModManager._settings = new ES3Settings(null, null)
					{
						location = 0,
						path = "Saves/Mods.ES3"
					};
				}
				return ModManager._settings;
			}
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0004AA90 File Offset: 0x00048C90
		private static void Save<T>(string key, T value)
		{
			try
			{
				ES3.Save<T>(key, value, ModManager.settings);
				ES3.CreateBackup(ModManager.settings);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed saving mod info.");
				Debug.LogException(ex);
			}
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0004AAD8 File Offset: 0x00048CD8
		private static T Load<T>(string key, T defaultValue = default(T))
		{
			T result;
			try
			{
				result = ES3.Load<T>(key, defaultValue, ModManager.settings);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed loading mod info.");
				ES3.RestoreBackup(ModManager.settings);
				Debug.LogException(ex);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0004AB24 File Offset: 0x00048D24
		public static void SetModPriority(string name, int priority)
		{
			ModManager.Save<int>("priority_" + name, priority);
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0004AB37 File Offset: 0x00048D37
		public static int GetModPriority(string name)
		{
			return ModManager.Load<int>("priority_" + name, int.MaxValue);
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0004AB4E File Offset: 0x00048D4E
		private void SetShouldActivateMod(ModInfo info, bool value)
		{
			SavesSystem.SaveGlobal<bool>("ModActive_" + info.name, value);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0004AB66 File Offset: 0x00048D66
		private bool ShouldActivateMod(ModInfo info)
		{
			return SavesSystem.LoadGlobal<bool>("ModActive_" + info.name, false);
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x0004AB7E File Offset: 0x00048D7E
		public static string DefaultModFolderPath
		{
			get
			{
				return Path.Combine(Application.dataPath, "Mods");
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x0004AB8F File Offset: 0x00048D8F
		public static ModManager Instance
		{
			get
			{
				return GameManager.ModManager;
			}
		}

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x060013F7 RID: 5111 RVA: 0x0004AB98 File Offset: 0x00048D98
		// (remove) Token: 0x060013F8 RID: 5112 RVA: 0x0004ABCC File Offset: 0x00048DCC
		public static event Action<List<ModInfo>> OnScan;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x060013F9 RID: 5113 RVA: 0x0004AC00 File Offset: 0x00048E00
		// (remove) Token: 0x060013FA RID: 5114 RVA: 0x0004AC34 File Offset: 0x00048E34
		public static event Action<ModInfo, ModBehaviour> OnModActivated;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x060013FB RID: 5115 RVA: 0x0004AC68 File Offset: 0x00048E68
		// (remove) Token: 0x060013FC RID: 5116 RVA: 0x0004AC9C File Offset: 0x00048E9C
		public static event Action<ModInfo, ModBehaviour> OnModWillBeDeactivated;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x060013FD RID: 5117 RVA: 0x0004ACD0 File Offset: 0x00048ED0
		// (remove) Token: 0x060013FE RID: 5118 RVA: 0x0004AD04 File Offset: 0x00048F04
		public static event Action OnModStatusChanged;

		// Token: 0x060013FF RID: 5119 RVA: 0x0004AD38 File Offset: 0x00048F38
		public static void Rescan()
		{
			ModManager.modInfos.Clear();
			if (Directory.Exists(ModManager.DefaultModFolderPath))
			{
				string[] directories = Directory.GetDirectories(ModManager.DefaultModFolderPath);
				for (int i = 0; i < directories.Length; i++)
				{
					ModInfo item;
					if (ModManager.TryProcessModFolder(directories[i], out item, false, 0UL))
					{
						ModManager.modInfos.Add(item);
					}
				}
			}
			Action<List<ModInfo>> onScan = ModManager.OnScan;
			if (onScan != null)
			{
				onScan(ModManager.modInfos);
			}
			ModManager.SortModInfosByPriority();
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0004ADA8 File Offset: 0x00048FA8
		private static void RegeneratePriorities()
		{
			for (int i = 0; i < ModManager.modInfos.Count; i++)
			{
				string name = ModManager.modInfos[i].name;
				if (!string.IsNullOrWhiteSpace(name))
				{
					ModManager.SetModPriority(name, i);
				}
			}
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0004ADEC File Offset: 0x00048FEC
		public static bool Reorder(int fromIndex, int toIndex)
		{
			if (fromIndex == toIndex)
			{
				return false;
			}
			if (fromIndex < 0 || fromIndex >= ModManager.modInfos.Count)
			{
				return false;
			}
			if (toIndex < 0 || toIndex >= ModManager.modInfos.Count)
			{
				return false;
			}
			ModInfo item = ModManager.modInfos[fromIndex];
			ModManager.modInfos.RemoveAt(fromIndex);
			ModManager.modInfos.Insert(toIndex, item);
			ModManager.RegeneratePriorities();
			Action onReorder = ModManager.OnReorder;
			if (onReorder != null)
			{
				onReorder();
			}
			return true;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0004AE60 File Offset: 0x00049060
		public static bool TryProcessModFolder(string path, out ModInfo info, bool isSteamItem = false, ulong publishedFileId = 0UL)
		{
			info = default(ModInfo);
			info.path = path;
			string path2 = Path.Combine(path, "info.ini");
			if (!File.Exists(path2))
			{
				return false;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			using (StreamReader streamReader = File.OpenText(path2))
			{
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine().Trim();
					if (!string.IsNullOrWhiteSpace(text) && !text.StartsWith('['))
					{
						int num = text.IndexOf('=');
						if (num >= 1 && num + 1 < text.Length)
						{
							string key = text.Substring(0, num).Trim();
							string value = text.Substring(num + 1).Trim();
							dictionary[key] = value;
						}
					}
				}
			}
			string text2;
			if (!dictionary.TryGetValue("name", out text2))
			{
				Debug.LogError("Failed to get name value in mod info.ini file. Aborting.\n" + path);
				return false;
			}
			string displayName;
			if (!dictionary.TryGetValue("displayName", out displayName))
			{
				displayName = text2;
				Debug.LogError("Failed to get displayName value in mod info.ini file.\n" + path);
			}
			string text3;
			if (!dictionary.TryGetValue("description", out text3))
			{
				text3 = "?";
				Debug.LogError("Failed to get description value in mod info.ini file.\n" + path);
			}
			ulong num2 = 0UL;
			string s;
			if (dictionary.TryGetValue("publishedFileId", out s) && !ulong.TryParse(s, out num2))
			{
				Debug.LogError("Invalid publishedFileId");
			}
			string version;
			if (!dictionary.TryGetValue("version", out version))
			{
				version = "?";
			}
			string tags;
			if (!dictionary.TryGetValue("tags", out tags))
			{
				tags = "";
			}
			if (!isSteamItem)
			{
				publishedFileId = num2;
			}
			else if (publishedFileId != num2)
			{
				Debug.LogError("PublishFileId not match.\npath:" + path);
			}
			info.name = text2;
			info.displayName = displayName;
			try
			{
				info.description = Regex.Unescape(text3);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				info.description = text3;
			}
			info.publishedFileId = publishedFileId;
			info.isSteamItem = isSteamItem;
			info.version = version;
			info.tags = tags;
			string dllPath = info.dllPath;
			info.dllFound = File.Exists(dllPath);
			if (!info.dllFound)
			{
				Debug.LogError("Dll for mod " + text2 + " not found.\nExpecting: " + dllPath);
			}
			string path3 = Path.Combine(path, "preview.png");
			if (File.Exists(path3))
			{
				using (FileStream fileStream = File.OpenRead(path3))
				{
					Texture2D texture2D = new Texture2D(256, 256);
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array);
					if (ImageConversion.LoadImage(texture2D, array))
					{
						info.preview = texture2D;
					}
				}
			}
			return true;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0004B114 File Offset: 0x00049314
		public static bool IsModActive(ModInfo info, out ModBehaviour instance)
		{
			instance = null;
			return !(ModManager.Instance == null) && ModManager.Instance.activeMods.TryGetValue(info.name, out instance) && instance != null;
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0004B14C File Offset: 0x0004934C
		public ModBehaviour GetActiveModBehaviour(ModInfo info)
		{
			ModBehaviour result;
			if (this.activeMods.TryGetValue(info.name, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0004B174 File Offset: 0x00049374
		public void DeactivateMod(ModInfo info)
		{
			ModBehaviour activeModBehaviour = this.GetActiveModBehaviour(info);
			if (activeModBehaviour == null)
			{
				return;
			}
			try
			{
				activeModBehaviour.NotifyBeforeDeactivate();
				Action<ModInfo, ModBehaviour> onModWillBeDeactivated = ModManager.OnModWillBeDeactivated;
				if (onModWillBeDeactivated != null)
				{
					onModWillBeDeactivated(info, activeModBehaviour);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			this.activeMods.Remove(info.name);
			try
			{
				Object.Destroy(activeModBehaviour.gameObject);
				Action onModStatusChanged = ModManager.OnModStatusChanged;
				if (onModStatusChanged != null)
				{
					onModStatusChanged();
				}
			}
			catch (Exception ex2)
			{
				Debug.LogException(ex2);
			}
			this.SetShouldActivateMod(info, false);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0004B210 File Offset: 0x00049410
		public ModBehaviour ActivateMod(ModInfo info)
		{
			if (!ModManager.AllowActivatingMod)
			{
				Debug.LogError("Activating mod not allowed! \nUser must first interact with the agreement UI in order to allow activating mods.");
				return null;
			}
			string dllPath = info.dllPath;
			string name = info.name;
			ModBehaviour modBehaviour;
			if (ModManager.IsModActive(info, out modBehaviour))
			{
				Debug.LogError("Mod " + info.name + " instance already exists! Abort. Path: " + info.path, modBehaviour);
				return null;
			}
			Debug.Log("Loading mod dll at path: " + dllPath);
			Type type;
			try
			{
				type = Assembly.LoadFrom(dllPath).GetType(name + ".ModBehaviour");
				if (type == null || !TypeExtensions.InheritsFrom<ModBehaviour>(type))
				{
					Debug.LogError("Cannot load mod.\nA type named " + name + ".ModBehaviour is expected, and it should inherit from Duckov.Modding.ModBehaviour.");
					return null;
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				string arg = "Mod loading failed: " + name + "\n" + ex.Message;
				Action<string, string> onModLoadingFailed = ModManager.OnModLoadingFailed;
				if (onModLoadingFailed != null)
				{
					onModLoadingFailed(info.dllPath, arg);
				}
				return null;
			}
			GameObject gameObject = new GameObject(name);
			ModBehaviour modBehaviour2;
			try
			{
				modBehaviour2 = (gameObject.AddComponent(type) as ModBehaviour);
			}
			catch (Exception ex2)
			{
				Debug.LogException(ex2);
				Debug.LogError("Failed to create component for mod " + name);
				return null;
			}
			if (modBehaviour2 == null)
			{
				Object.Destroy(gameObject);
				Debug.LogError("Failed to create component for mod " + name);
				return null;
			}
			gameObject.transform.SetParent(base.transform);
			Debug.Log("Mod Loaded: " + info.name);
			try
			{
				modBehaviour2.Setup(this, info);
			}
			catch (Exception ex3)
			{
				Debug.LogException(ex3);
				Debug.Log("[MOD Manager] Setup failed. Mod: " + name);
				return null;
			}
			this.activeMods[info.name] = modBehaviour2;
			try
			{
				Action<ModInfo, ModBehaviour> onModActivated = ModManager.OnModActivated;
				if (onModActivated != null)
				{
					onModActivated(info, modBehaviour2);
				}
				Action onModStatusChanged = ModManager.OnModStatusChanged;
				if (onModStatusChanged != null)
				{
					onModStatusChanged();
				}
			}
			catch (Exception ex4)
			{
				Debug.LogException(ex4);
			}
			this.SetShouldActivateMod(info, true);
			return modBehaviour2;
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0004B42C File Offset: 0x0004962C
		internal static void WriteModInfoINI(ModInfo modInfo)
		{
			string path = Path.Combine(modInfo.path, "info.ini");
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			using (FileStream fileStream = File.Create(path))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.WriteLine("name = " + modInfo.name);
				streamWriter.WriteLine("displayName = " + modInfo.displayName);
				string text = modInfo.description;
				try
				{
					text = Regex.Escape(text);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					text = "Cannot escape description";
				}
				streamWriter.WriteLine("description = " + text);
				streamWriter.WriteLine("version = " + modInfo.version);
				streamWriter.WriteLine("tags = " + modInfo.tags);
				streamWriter.WriteLine("");
				streamWriter.WriteLine(string.Format("publishedFileId = {0}", modInfo.publishedFileId));
				streamWriter.Close();
			}
		}

		// Token: 0x04000ED4 RID: 3796
		[SerializeField]
		private Transform modParent;

		// Token: 0x04000ED6 RID: 3798
		public static Action<string, string> OnModLoadingFailed;

		// Token: 0x04000ED7 RID: 3799
		private const string LoadedModListSaveKey = "ActiveModList";

		// Token: 0x04000ED8 RID: 3800
		private static ES3Settings _settings;

		// Token: 0x04000ED9 RID: 3801
		public static List<ModInfo> modInfos = new List<ModInfo>();

		// Token: 0x04000EDA RID: 3802
		private Dictionary<string, ModBehaviour> activeMods = new Dictionary<string, ModBehaviour>();
	}
}
