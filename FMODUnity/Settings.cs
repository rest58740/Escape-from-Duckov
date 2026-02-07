using System;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using UnityEngine;
using UnityEngine.Serialization;

namespace FMODUnity
{
	// Token: 0x02000128 RID: 296
	public class Settings : ScriptableObject
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0000AAD4 File Offset: 0x00008CD4
		public static Settings Instance
		{
			get
			{
				if (Settings.isInitializing)
				{
					return null;
				}
				Settings.Initialize();
				return Settings.instance;
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0000AAEC File Offset: 0x00008CEC
		internal static void Initialize()
		{
			if (Settings.instance == null)
			{
				Settings.isInitializing = true;
				Settings.instance = (Resources.Load("FMODStudioSettings") as Settings);
				if (Settings.instance == null)
				{
					RuntimeUtils.DebugLog("[FMOD] Cannot find integration settings, creating default settings");
					Settings.instance = ScriptableObject.CreateInstance<Settings>();
					Settings.instance.name = "FMOD Studio Integration Settings";
					Settings.instance.CurrentVersion = 131848;
					Settings.instance.LastEventReferenceScanVersion = 131848;
				}
				Settings.isInitializing = false;
			}
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0000AB74 File Offset: 0x00008D74
		internal static bool IsInitialized()
		{
			return !(Settings.instance == null) && !Settings.isInitializing;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x0000AB8D File Offset: 0x00008D8D
		// (set) Token: 0x06000783 RID: 1923 RVA: 0x0000AB94 File Offset: 0x00008D94
		internal static IEditorSettings EditorSettings
		{
			get
			{
				return Settings.editorSettings;
			}
			set
			{
				Settings.editorSettings = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x0000AB9C File Offset: 0x00008D9C
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x0000ABA4 File Offset: 0x00008DA4
		public string SourceProjectPath
		{
			get
			{
				return this.sourceProjectPath;
			}
			set
			{
				this.sourceProjectPath = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x0000ABAD File Offset: 0x00008DAD
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x0000ABB5 File Offset: 0x00008DB5
		public string SourceBankPath
		{
			get
			{
				return this.sourceBankPath;
			}
			set
			{
				this.sourceBankPath = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x0000ABC0 File Offset: 0x00008DC0
		internal string TargetPath
		{
			get
			{
				if (this.ImportType == ImportType.AssetBundle)
				{
					if (string.IsNullOrEmpty(this.TargetAssetPath))
					{
						return Application.dataPath;
					}
					return Application.dataPath + "/" + this.TargetAssetPath;
				}
				else
				{
					if (string.IsNullOrEmpty(this.TargetBankFolder))
					{
						return Application.streamingAssetsPath;
					}
					return Application.streamingAssetsPath + "/" + this.TargetBankFolder;
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0000AC27 File Offset: 0x00008E27
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x0000AC3F File Offset: 0x00008E3F
		public string TargetSubFolder
		{
			get
			{
				if (this.ImportType == ImportType.AssetBundle)
				{
					return this.TargetAssetPath;
				}
				return this.TargetBankFolder;
			}
			set
			{
				if (this.ImportType == ImportType.AssetBundle)
				{
					this.TargetAssetPath = value;
					return;
				}
				this.TargetBankFolder = value;
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0000AC5C File Offset: 0x00008E5C
		internal Platform FindPlatform(string identifier)
		{
			foreach (Platform platform in this.Platforms)
			{
				if (platform.Identifier == identifier)
				{
					return platform;
				}
			}
			return null;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0000ACC0 File Offset: 0x00008EC0
		internal bool PlatformExists(string identifier)
		{
			return this.FindPlatform(identifier) != null;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0000ACCF File Offset: 0x00008ECF
		internal void AddPlatform(Platform platform)
		{
			if (this.PlatformExists(platform.Identifier))
			{
				throw new ArgumentException(string.Format("Duplicate platform identifier: {0}", platform.Identifier));
			}
			this.Platforms.Add(platform);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0000AD04 File Offset: 0x00008F04
		internal void RemovePlatform(string identifier)
		{
			this.Platforms.RemoveAll((Platform p) => p.Identifier == identifier);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0000AD36 File Offset: 0x00008F36
		internal void LinkPlatform(Platform platform)
		{
			this.LinkPlatformToParent(platform);
			platform.DeclareRuntimePlatforms(this);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0000AD48 File Offset: 0x00008F48
		internal void DeclareRuntimePlatform(RuntimePlatform runtimePlatform, Platform platform)
		{
			List<Platform> list;
			if (!this.PlatformForRuntimePlatform.TryGetValue(runtimePlatform, out list))
			{
				list = new List<Platform>();
				this.PlatformForRuntimePlatform.Add(runtimePlatform, list);
			}
			list.Add(platform);
			list.Sort((Platform a, Platform b) => b.Priority.CompareTo(a.Priority));
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0000ADA4 File Offset: 0x00008FA4
		private void LinkPlatformToParent(Platform platform)
		{
			if (!string.IsNullOrEmpty(platform.ParentIdentifier))
			{
				this.SetPlatformParent(platform, this.FindPlatform(platform.ParentIdentifier));
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0000ADC8 File Offset: 0x00008FC8
		internal Platform FindCurrentPlatform()
		{
			List<Platform> list;
			if (this.PlatformForRuntimePlatform.TryGetValue(Application.platform, out list))
			{
				foreach (Platform platform in list)
				{
					if (platform.MatchesCurrentEnvironment)
					{
						return platform;
					}
				}
			}
			return this.DefaultPlatform;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0000AE38 File Offset: 0x00009038
		private Settings()
		{
			this.MasterBanks = new List<string>();
			this.Banks = new List<string>();
			this.BanksToLoad = new List<string>();
			this.RealChannelSettings = new List<Legacy.PlatformIntSetting>();
			this.VirtualChannelSettings = new List<Legacy.PlatformIntSetting>();
			this.LiveUpdateSettings = new List<Legacy.PlatformBoolSetting>();
			this.OverlaySettings = new List<Legacy.PlatformBoolSetting>();
			this.SampleRateSettings = new List<Legacy.PlatformIntSetting>();
			this.SpeakerModeSettings = new List<Legacy.PlatformIntSetting>();
			this.BankDirectorySettings = new List<Legacy.PlatformStringSetting>();
			this.ImportType = ImportType.StreamingAssets;
			this.AutomaticEventLoading = true;
			this.AutomaticSampleLoading = false;
			this.EnableMemoryTracking = false;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0000AF3A File Offset: 0x0000913A
		internal void AddPlatformProperties(Platform platform)
		{
			platform.AffirmProperties();
			this.LinkPlatformToParent(platform);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0000AF49 File Offset: 0x00009149
		public void SetPlatformParent(Platform platform, Platform newParent)
		{
			platform.Parent = newParent;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0000AF54 File Offset: 0x00009154
		internal static void AddPlatformTemplate<T>(string identifier) where T : Platform
		{
			Settings.PlatformTemplates.Add(new Settings.PlatformTemplate
			{
				Identifier = identifier,
				CreateInstance = (() => Settings.CreatePlatformInstance<T>(identifier))
			});
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0000AFA1 File Offset: 0x000091A1
		private static Platform CreatePlatformInstance<T>(string identifier) where T : Platform
		{
			T t = ScriptableObject.CreateInstance<T>();
			t.InitializeProperties();
			t.Identifier = identifier;
			return t;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0000AFBC File Offset: 0x000091BC
		internal void OnEnable()
		{
			if (this.hasLoaded)
			{
				return;
			}
			this.hasLoaded = true;
			this.PopulatePlatformsFromAsset();
			this.DefaultPlatform = this.Platforms.FirstOrDefault((Platform platform) => platform is PlatformDefault);
			this.PlayInEditorPlatform = this.Platforms.FirstOrDefault((Platform platform) => platform is PlatformPlayInEditor);
			this.Platforms.ForEach(new Action<Platform>(this.LinkPlatform));
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0000B058 File Offset: 0x00009258
		private void PopulatePlatformsFromAsset()
		{
			this.Platforms.Clear();
			foreach (Platform platform in Resources.LoadAll<Platform>("FMODStudioSettings"))
			{
				Platform platform2 = this.FindPlatform(platform.Identifier);
				if (platform2 != null)
				{
					Platform platform3;
					if (platform.Active && !platform2.Active)
					{
						this.RemovePlatform(platform2.Identifier);
						platform3 = platform2;
						platform2 = null;
					}
					else
					{
						platform3 = platform;
					}
					RuntimeUtils.DebugLogWarningFormat("FMOD: Cleaning up duplicate platform: ID  = {0}, name = '{1}', type = {2}", new object[]
					{
						platform3.Identifier,
						platform3.DisplayName,
						platform3.GetType().Name
					});
					UnityEngine.Object.DestroyImmediate(platform3, true);
				}
				if (platform2 == null)
				{
					platform.EnsurePropertiesAreValid();
					this.AddPlatform(platform);
				}
			}
		}

		// Token: 0x04000622 RID: 1570
		internal const string SettingsAssetName = "FMODStudioSettings";

		// Token: 0x04000623 RID: 1571
		private static Settings instance = null;

		// Token: 0x04000624 RID: 1572
		private static IEditorSettings editorSettings = null;

		// Token: 0x04000625 RID: 1573
		private static bool isInitializing = false;

		// Token: 0x04000626 RID: 1574
		[SerializeField]
		public bool HasSourceProject = true;

		// Token: 0x04000627 RID: 1575
		[SerializeField]
		public bool HasPlatforms = true;

		// Token: 0x04000628 RID: 1576
		[SerializeField]
		private string sourceProjectPath;

		// Token: 0x04000629 RID: 1577
		[SerializeField]
		private string sourceBankPath;

		// Token: 0x0400062A RID: 1578
		[FormerlySerializedAs("SourceBankPathUnformatted")]
		[SerializeField]
		private string sourceBankPathUnformatted;

		// Token: 0x0400062B RID: 1579
		[SerializeField]
		public int BankRefreshCooldown = 5;

		// Token: 0x0400062C RID: 1580
		[SerializeField]
		public bool ShowBankRefreshWindow = true;

		// Token: 0x0400062D RID: 1581
		internal const int BankRefreshPrompt = -1;

		// Token: 0x0400062E RID: 1582
		internal const int BankRefreshManual = -2;

		// Token: 0x0400062F RID: 1583
		[SerializeField]
		public bool AutomaticEventLoading;

		// Token: 0x04000630 RID: 1584
		[SerializeField]
		public BankLoadType BankLoadType;

		// Token: 0x04000631 RID: 1585
		[SerializeField]
		public bool AutomaticSampleLoading;

		// Token: 0x04000632 RID: 1586
		[SerializeField]
		public string EncryptionKey;

		// Token: 0x04000633 RID: 1587
		[SerializeField]
		public ImportType ImportType;

		// Token: 0x04000634 RID: 1588
		[SerializeField]
		public string TargetAssetPath = "FMODBanks";

		// Token: 0x04000635 RID: 1589
		[SerializeField]
		public string TargetBankFolder = "";

		// Token: 0x04000636 RID: 1590
		[SerializeField]
		public EventLinkage EventLinkage;

		// Token: 0x04000637 RID: 1591
		[SerializeField]
		public DEBUG_FLAGS LoggingLevel = DEBUG_FLAGS.WARNING;

		// Token: 0x04000638 RID: 1592
		[SerializeField]
		internal List<Legacy.PlatformIntSetting> SpeakerModeSettings;

		// Token: 0x04000639 RID: 1593
		[SerializeField]
		internal List<Legacy.PlatformIntSetting> SampleRateSettings;

		// Token: 0x0400063A RID: 1594
		[SerializeField]
		internal List<Legacy.PlatformBoolSetting> LiveUpdateSettings;

		// Token: 0x0400063B RID: 1595
		[SerializeField]
		internal List<Legacy.PlatformBoolSetting> OverlaySettings;

		// Token: 0x0400063C RID: 1596
		[SerializeField]
		internal List<Legacy.PlatformStringSetting> BankDirectorySettings;

		// Token: 0x0400063D RID: 1597
		[SerializeField]
		internal List<Legacy.PlatformIntSetting> VirtualChannelSettings;

		// Token: 0x0400063E RID: 1598
		[SerializeField]
		internal List<Legacy.PlatformIntSetting> RealChannelSettings;

		// Token: 0x0400063F RID: 1599
		[SerializeField]
		internal List<string> Plugins = new List<string>();

		// Token: 0x04000640 RID: 1600
		[SerializeField]
		public List<string> MasterBanks;

		// Token: 0x04000641 RID: 1601
		[SerializeField]
		public List<string> Banks;

		// Token: 0x04000642 RID: 1602
		[SerializeField]
		public List<string> BanksToLoad;

		// Token: 0x04000643 RID: 1603
		[SerializeField]
		public ushort LiveUpdatePort = 9264;

		// Token: 0x04000644 RID: 1604
		[SerializeField]
		public bool EnableMemoryTracking;

		// Token: 0x04000645 RID: 1605
		[SerializeField]
		public bool AndroidUseOBB;

		// Token: 0x04000646 RID: 1606
		[SerializeField]
		public bool AndroidPatchBuild;

		// Token: 0x04000647 RID: 1607
		[SerializeField]
		public MeterChannelOrderingType MeterChannelOrdering;

		// Token: 0x04000648 RID: 1608
		[SerializeField]
		public bool StopEventsOutsideMaxDistance;

		// Token: 0x04000649 RID: 1609
		[SerializeField]
		internal bool BoltUnitOptionsBuildPending;

		// Token: 0x0400064A RID: 1610
		[SerializeField]
		public bool EnableErrorCallback;

		// Token: 0x0400064B RID: 1611
		[SerializeField]
		internal Settings.SharedLibraryUpdateStages SharedLibraryUpdateStage;

		// Token: 0x0400064C RID: 1612
		[SerializeField]
		internal double SharedLibraryTimeSinceStart;

		// Token: 0x0400064D RID: 1613
		[SerializeField]
		internal int CurrentVersion;

		// Token: 0x0400064E RID: 1614
		[SerializeField]
		public bool HideSetupWizard;

		// Token: 0x0400064F RID: 1615
		[SerializeField]
		internal int LastEventReferenceScanVersion;

		// Token: 0x04000650 RID: 1616
		[SerializeField]
		public List<Platform> Platforms = new List<Platform>();

		// Token: 0x04000651 RID: 1617
		internal Dictionary<RuntimePlatform, List<Platform>> PlatformForRuntimePlatform = new Dictionary<RuntimePlatform, List<Platform>>();

		// Token: 0x04000652 RID: 1618
		[NonSerialized]
		public Platform DefaultPlatform;

		// Token: 0x04000653 RID: 1619
		[NonSerialized]
		public Platform PlayInEditorPlatform;

		// Token: 0x04000654 RID: 1620
		internal static List<Settings.PlatformTemplate> PlatformTemplates = new List<Settings.PlatformTemplate>();

		// Token: 0x04000655 RID: 1621
		[NonSerialized]
		private bool hasLoaded;

		// Token: 0x02000149 RID: 329
		internal enum SharedLibraryUpdateStages
		{
			// Token: 0x040006C5 RID: 1733
			Start,
			// Token: 0x040006C6 RID: 1734
			DisableExistingLibraries,
			// Token: 0x040006C7 RID: 1735
			RestartUnity,
			// Token: 0x040006C8 RID: 1736
			CopyNewLibraries
		}

		// Token: 0x0200014A RID: 330
		internal struct PlatformTemplate
		{
			// Token: 0x040006C9 RID: 1737
			public string Identifier;

			// Token: 0x040006CA RID: 1738
			public Func<Platform> CreateInstance;
		}
	}
}
