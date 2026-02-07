using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

namespace FMODUnity
{
	// Token: 0x0200010D RID: 269
	public abstract class Platform : ScriptableObject
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x00007DE6 File Offset: 0x00005FE6
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x00007DEE File Offset: 0x00005FEE
		internal string Identifier
		{
			get
			{
				return this.identifier;
			}
			set
			{
				this.identifier = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060006BF RID: 1727
		internal abstract string DisplayName { get; }

		// Token: 0x060006C0 RID: 1728
		internal abstract void DeclareRuntimePlatforms(Settings settings);

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00007DF7 File Offset: 0x00005FF7
		internal virtual float Priority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00007DFE File Offset: 0x00005FFE
		internal virtual bool MatchesCurrentEnvironment
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00007E01 File Offset: 0x00006001
		internal virtual bool IsIntrinsic
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00007E04 File Offset: 0x00006004
		internal virtual void PreSystemCreate(Action<RESULT, string> reportResult)
		{
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00007E06 File Offset: 0x00006006
		internal virtual void PreInitialize(FMOD.Studio.System studioSystem)
		{
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00007E08 File Offset: 0x00006008
		internal virtual string GetBankFolder()
		{
			return Application.streamingAssetsPath;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00007E0F File Offset: 0x0000600F
		protected virtual string GetPluginBasePath()
		{
			return string.Format("{0}/Plugins", Application.dataPath);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00007E20 File Offset: 0x00006020
		internal virtual string GetPluginPath(string pluginName)
		{
			throw new NotImplementedException(string.Format("Plugins are not implemented on platform {0}", this.Identifier));
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00007E37 File Offset: 0x00006037
		internal virtual void LoadPlugins(FMOD.System coreSystem, Action<RESULT, string> reportResult)
		{
			this.LoadDynamicPlugins(coreSystem, reportResult);
			this.LoadStaticPlugins(coreSystem, reportResult);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00007E4C File Offset: 0x0000604C
		internal virtual void LoadDynamicPlugins(FMOD.System coreSystem, Action<RESULT, string> reportResult)
		{
			List<string> plugins = this.Plugins;
			if (plugins == null)
			{
				return;
			}
			foreach (string text in plugins)
			{
				if (!string.IsNullOrEmpty(text))
				{
					string pluginPath = this.GetPluginPath(text);
					uint num;
					RESULT result = coreSystem.loadPlugin(pluginPath, out num, 0U);
					if (result == RESULT.ERR_FILE_BAD || result == RESULT.ERR_FILE_NOTFOUND)
					{
						string pluginPath2 = this.GetPluginPath(text + "64");
						result = coreSystem.loadPlugin(pluginPath2, out num, 0U);
					}
					reportResult(result, string.Format("Loading plugin '{0}' from '{1}'", text, pluginPath));
				}
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00007EFC File Offset: 0x000060FC
		internal virtual void LoadStaticPlugins(FMOD.System coreSystem, Action<RESULT, string> reportResult)
		{
			if (this.StaticPlugins.Count > 0)
			{
				RuntimeUtils.DebugLogWarningFormat("FMOD: {0} static plugins specified, but static plugins are only supported on the IL2CPP scripting backend", new object[]
				{
					this.StaticPlugins.Count
				});
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00007F2F File Offset: 0x0000612F
		internal void AffirmProperties()
		{
			if (!this.active)
			{
				this.Properties = new Platform.PropertyStorage();
				this.InitializeProperties();
				this.active = true;
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00007F51 File Offset: 0x00006151
		internal void ClearProperties()
		{
			if (this.active)
			{
				this.Properties = new Platform.PropertyStorage();
				this.active = false;
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00007F6D File Offset: 0x0000616D
		internal virtual void InitializeProperties()
		{
			if (!this.IsIntrinsic)
			{
				this.ParentIdentifier = "default";
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00007F82 File Offset: 0x00006182
		internal virtual void EnsurePropertiesAreValid()
		{
			if (!this.IsIntrinsic && string.IsNullOrEmpty(this.ParentIdentifier))
			{
				this.ParentIdentifier = "default";
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00007FA4 File Offset: 0x000061A4
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x00007FAC File Offset: 0x000061AC
		internal string ParentIdentifier
		{
			get
			{
				return this.parentIdentifier;
			}
			set
			{
				this.parentIdentifier = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00007FB5 File Offset: 0x000061B5
		internal bool IsLiveUpdateEnabled
		{
			get
			{
				return this.LiveUpdate == TriStateBool.Enabled;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00007FC0 File Offset: 0x000061C0
		internal bool IsOverlayEnabled
		{
			get
			{
				return this.Overlay == TriStateBool.Enabled;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00007FCB File Offset: 0x000061CB
		internal bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00007FD4 File Offset: 0x000061D4
		internal bool HasAnyOverriddenProperties
		{
			get
			{
				return this.active && (this.Properties.LiveUpdate.HasValue || this.Properties.LiveUpdatePort.HasValue || this.Properties.Overlay.HasValue || this.Properties.OverlayPosition.HasValue || this.Properties.OverlayFontSize.HasValue || this.Properties.Logging.HasValue || this.Properties.SampleRate.HasValue || this.Properties.BuildDirectory.HasValue || this.Properties.SpeakerMode.HasValue || this.Properties.VirtualChannelCount.HasValue || this.Properties.RealChannelCount.HasValue || this.Properties.DSPBufferLength.HasValue || this.Properties.DSPBufferCount.HasValue || this.Properties.Plugins.HasValue || this.Properties.StaticPlugins.HasValue);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x00008114 File Offset: 0x00006314
		public TriStateBool LiveUpdate
		{
			get
			{
				return Platform.PropertyAccessors.LiveUpdate.Get(this);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00008130 File Offset: 0x00006330
		public int LiveUpdatePort
		{
			get
			{
				return Platform.PropertyAccessors.LiveUpdatePort.Get(this);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0000814C File Offset: 0x0000634C
		public TriStateBool Overlay
		{
			get
			{
				return Platform.PropertyAccessors.Overlay.Get(this);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00008168 File Offset: 0x00006368
		public ScreenPosition OverlayRect
		{
			get
			{
				return Platform.PropertyAccessors.OverlayPosition.Get(this);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x00008184 File Offset: 0x00006384
		public int OverlayFontSize
		{
			get
			{
				return Platform.PropertyAccessors.OverlayFontSize.Get(this);
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x000081A0 File Offset: 0x000063A0
		public void SetOverlayFontSize(int size)
		{
			Platform.PropertyAccessors.OverlayFontSize.Set(this, size);
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x000081BC File Offset: 0x000063BC
		public TriStateBool Logging
		{
			get
			{
				return Platform.PropertyAccessors.Logging.Get(this);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x000081D8 File Offset: 0x000063D8
		public int SampleRate
		{
			get
			{
				return Platform.PropertyAccessors.SampleRate.Get(this);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x000081F4 File Offset: 0x000063F4
		public string BuildDirectory
		{
			get
			{
				return Platform.PropertyAccessors.BuildDirectory.Get(this);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00008210 File Offset: 0x00006410
		public SPEAKERMODE SpeakerMode
		{
			get
			{
				return Platform.PropertyAccessors.SpeakerMode.Get(this);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0000822C File Offset: 0x0000642C
		public int VirtualChannelCount
		{
			get
			{
				return Platform.PropertyAccessors.VirtualChannelCount.Get(this);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00008248 File Offset: 0x00006448
		public int RealChannelCount
		{
			get
			{
				return Platform.PropertyAccessors.RealChannelCount.Get(this);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00008264 File Offset: 0x00006464
		public int DSPBufferLength
		{
			get
			{
				return Platform.PropertyAccessors.DSPBufferLength.Get(this);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00008280 File Offset: 0x00006480
		public int DSPBufferCount
		{
			get
			{
				return Platform.PropertyAccessors.DSPBufferCount.Get(this);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0000829C File Offset: 0x0000649C
		public List<string> Plugins
		{
			get
			{
				return Platform.PropertyAccessors.Plugins.Get(this);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x000082B8 File Offset: 0x000064B8
		public List<string> StaticPlugins
		{
			get
			{
				return Platform.PropertyAccessors.StaticPlugins.Get(this);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x000082D4 File Offset: 0x000064D4
		public PlatformCallbackHandler CallbackHandler
		{
			get
			{
				return Platform.PropertyAccessors.CallbackHandler.Get(this);
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000082EF File Offset: 0x000064EF
		internal bool InheritsFrom(Platform platform)
		{
			return platform == this || (this.Parent != null && this.Parent.InheritsFrom(platform));
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00008318 File Offset: 0x00006518
		internal OUTPUTTYPE GetOutputType()
		{
			if (Enum.IsDefined(typeof(OUTPUTTYPE), this.OutputTypeName))
			{
				return (OUTPUTTYPE)Enum.Parse(typeof(OUTPUTTYPE), this.OutputTypeName);
			}
			return OUTPUTTYPE.AUTODETECT;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0000834D File Offset: 0x0000654D
		internal virtual List<ThreadAffinityGroup> DefaultThreadAffinities
		{
			get
			{
				return Platform.StaticThreadAffinities;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00008354 File Offset: 0x00006554
		public IEnumerable<ThreadAffinityGroup> ThreadAffinities
		{
			get
			{
				if (this.threadAffinities.HasValue)
				{
					return this.threadAffinities.Value;
				}
				return this.DefaultThreadAffinities;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00008375 File Offset: 0x00006575
		internal Platform.PropertyThreadAffinityList ThreadAffinitiesProperty
		{
			get
			{
				return this.threadAffinities;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0000837D File Offset: 0x0000657D
		internal virtual List<CodecChannelCount> DefaultCodecChannels
		{
			get
			{
				return Platform.staticCodecChannels;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00008384 File Offset: 0x00006584
		internal List<CodecChannelCount> CodecChannels
		{
			get
			{
				if (this.codecChannels.HasValue)
				{
					return this.codecChannels.Value;
				}
				return this.DefaultCodecChannels;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x000083A5 File Offset: 0x000065A5
		internal Platform.PropertyCodecChannels CodecChannelsProperty
		{
			get
			{
				return this.codecChannels;
			}
		}

		// Token: 0x0400058C RID: 1420
		internal const float DefaultPriority = 0f;

		// Token: 0x0400058D RID: 1421
		internal const string RegisterStaticPluginsClassName = "StaticPluginManager";

		// Token: 0x0400058E RID: 1422
		internal const string RegisterStaticPluginsFunctionName = "Register";

		// Token: 0x0400058F RID: 1423
		[SerializeField]
		private string identifier;

		// Token: 0x04000590 RID: 1424
		[SerializeField]
		private string parentIdentifier;

		// Token: 0x04000591 RID: 1425
		[SerializeField]
		private bool active;

		// Token: 0x04000592 RID: 1426
		[SerializeField]
		protected Platform.PropertyStorage Properties = new Platform.PropertyStorage();

		// Token: 0x04000593 RID: 1427
		[SerializeField]
		[FormerlySerializedAs("outputType")]
		internal string OutputTypeName;

		// Token: 0x04000594 RID: 1428
		private static List<ThreadAffinityGroup> StaticThreadAffinities = new List<ThreadAffinityGroup>();

		// Token: 0x04000595 RID: 1429
		[SerializeField]
		private Platform.PropertyThreadAffinityList threadAffinities = new Platform.PropertyThreadAffinityList();

		// Token: 0x04000596 RID: 1430
		[NonSerialized]
		public Platform Parent;

		// Token: 0x04000597 RID: 1431
		private static List<CodecChannelCount> staticCodecChannels = new List<CodecChannelCount>
		{
			new CodecChannelCount
			{
				format = CodecType.FADPCM,
				channels = 32
			},
			new CodecChannelCount
			{
				format = CodecType.Vorbis,
				channels = 0
			}
		};

		// Token: 0x04000598 RID: 1432
		[SerializeField]
		private Platform.PropertyCodecChannels codecChannels = new Platform.PropertyCodecChannels();

		// Token: 0x02000134 RID: 308
		public class Property<T>
		{
			// Token: 0x0400068C RID: 1676
			public T Value;

			// Token: 0x0400068D RID: 1677
			public bool HasValue;
		}

		// Token: 0x02000135 RID: 309
		[Serializable]
		public class PropertyBool : Platform.Property<TriStateBool>
		{
		}

		// Token: 0x02000136 RID: 310
		[Serializable]
		public class PropertyScreenPosition : Platform.Property<ScreenPosition>
		{
		}

		// Token: 0x02000137 RID: 311
		[Serializable]
		public class PropertyInt : Platform.Property<int>
		{
		}

		// Token: 0x02000138 RID: 312
		[Serializable]
		public class PropertySpeakerMode : Platform.Property<SPEAKERMODE>
		{
		}

		// Token: 0x02000139 RID: 313
		[Serializable]
		public class PropertyString : Platform.Property<string>
		{
		}

		// Token: 0x0200013A RID: 314
		[Serializable]
		public class PropertyStringList : Platform.Property<List<string>>
		{
		}

		// Token: 0x0200013B RID: 315
		[Serializable]
		public class PropertyCallbackHandler : Platform.Property<PlatformCallbackHandler>
		{
		}

		// Token: 0x0200013C RID: 316
		internal interface PropertyOverrideControl
		{
			// Token: 0x060007F3 RID: 2035
			bool HasValue(Platform platform);

			// Token: 0x060007F4 RID: 2036
			void Clear(Platform platform);
		}

		// Token: 0x0200013D RID: 317
		internal struct PropertyAccessor<T> : Platform.PropertyOverrideControl
		{
			// Token: 0x060007F5 RID: 2037 RVA: 0x0000C6A0 File Offset: 0x0000A8A0
			public PropertyAccessor(Func<Platform.PropertyStorage, Platform.Property<T>> getter, T defaultValue)
			{
				this.Getter = getter;
				this.DefaultValue = defaultValue;
			}

			// Token: 0x060007F6 RID: 2038 RVA: 0x0000C6B0 File Offset: 0x0000A8B0
			public bool HasValue(Platform platform)
			{
				return platform.Active && this.Getter(platform.Properties).HasValue;
			}

			// Token: 0x060007F7 RID: 2039 RVA: 0x0000C6D4 File Offset: 0x0000A8D4
			public T Get(Platform platform)
			{
				Platform platform2 = platform;
				while (platform2 != null)
				{
					if (platform2.Active)
					{
						Platform.Property<T> property = this.Getter(platform2.Properties);
						if (property.HasValue)
						{
							return property.Value;
						}
					}
					platform2 = platform2.Parent;
				}
				return this.DefaultValue;
			}

			// Token: 0x060007F8 RID: 2040 RVA: 0x0000C724 File Offset: 0x0000A924
			public void Set(Platform platform, T value)
			{
				Platform.Property<T> property = this.Getter(platform.Properties);
				property.Value = value;
				property.HasValue = true;
			}

			// Token: 0x060007F9 RID: 2041 RVA: 0x0000C744 File Offset: 0x0000A944
			public void Clear(Platform platform)
			{
				this.Getter(platform.Properties).HasValue = false;
			}

			// Token: 0x0400068E RID: 1678
			private readonly Func<Platform.PropertyStorage, Platform.Property<T>> Getter;

			// Token: 0x0400068F RID: 1679
			private readonly T DefaultValue;
		}

		// Token: 0x0200013E RID: 318
		[Serializable]
		public class PropertyStorage
		{
			// Token: 0x04000690 RID: 1680
			public Platform.PropertyBool LiveUpdate = new Platform.PropertyBool();

			// Token: 0x04000691 RID: 1681
			public Platform.PropertyInt LiveUpdatePort = new Platform.PropertyInt();

			// Token: 0x04000692 RID: 1682
			public Platform.PropertyBool Overlay = new Platform.PropertyBool();

			// Token: 0x04000693 RID: 1683
			public Platform.PropertyScreenPosition OverlayPosition = new Platform.PropertyScreenPosition();

			// Token: 0x04000694 RID: 1684
			public Platform.PropertyInt OverlayFontSize = new Platform.PropertyInt();

			// Token: 0x04000695 RID: 1685
			public Platform.PropertyBool Logging = new Platform.PropertyBool();

			// Token: 0x04000696 RID: 1686
			public Platform.PropertyInt SampleRate = new Platform.PropertyInt();

			// Token: 0x04000697 RID: 1687
			public Platform.PropertyString BuildDirectory = new Platform.PropertyString();

			// Token: 0x04000698 RID: 1688
			public Platform.PropertySpeakerMode SpeakerMode = new Platform.PropertySpeakerMode();

			// Token: 0x04000699 RID: 1689
			public Platform.PropertyInt VirtualChannelCount = new Platform.PropertyInt();

			// Token: 0x0400069A RID: 1690
			public Platform.PropertyInt RealChannelCount = new Platform.PropertyInt();

			// Token: 0x0400069B RID: 1691
			public Platform.PropertyInt DSPBufferLength = new Platform.PropertyInt();

			// Token: 0x0400069C RID: 1692
			public Platform.PropertyInt DSPBufferCount = new Platform.PropertyInt();

			// Token: 0x0400069D RID: 1693
			public Platform.PropertyStringList Plugins = new Platform.PropertyStringList();

			// Token: 0x0400069E RID: 1694
			public Platform.PropertyStringList StaticPlugins = new Platform.PropertyStringList();

			// Token: 0x0400069F RID: 1695
			public Platform.PropertyCallbackHandler CallbackHandler = new Platform.PropertyCallbackHandler();
		}

		// Token: 0x0200013F RID: 319
		internal static class PropertyAccessors
		{
			// Token: 0x040006A0 RID: 1696
			public static readonly Platform.PropertyAccessor<TriStateBool> LiveUpdate = new Platform.PropertyAccessor<TriStateBool>((Platform.PropertyStorage properties) => properties.LiveUpdate, TriStateBool.Disabled);

			// Token: 0x040006A1 RID: 1697
			public static readonly Platform.PropertyAccessor<int> LiveUpdatePort = new Platform.PropertyAccessor<int>((Platform.PropertyStorage properties) => properties.LiveUpdatePort, 9264);

			// Token: 0x040006A2 RID: 1698
			public static readonly Platform.PropertyAccessor<TriStateBool> Overlay = new Platform.PropertyAccessor<TriStateBool>((Platform.PropertyStorage properties) => properties.Overlay, TriStateBool.Disabled);

			// Token: 0x040006A3 RID: 1699
			public static readonly Platform.PropertyAccessor<ScreenPosition> OverlayPosition = new Platform.PropertyAccessor<ScreenPosition>((Platform.PropertyStorage properties) => properties.OverlayPosition, ScreenPosition.TopLeft);

			// Token: 0x040006A4 RID: 1700
			public static readonly Platform.PropertyAccessor<int> OverlayFontSize = new Platform.PropertyAccessor<int>((Platform.PropertyStorage properties) => properties.OverlayFontSize, 14);

			// Token: 0x040006A5 RID: 1701
			public static readonly Platform.PropertyAccessor<TriStateBool> Logging = new Platform.PropertyAccessor<TriStateBool>((Platform.PropertyStorage properties) => properties.Logging, TriStateBool.Disabled);

			// Token: 0x040006A6 RID: 1702
			public static readonly Platform.PropertyAccessor<int> SampleRate = new Platform.PropertyAccessor<int>((Platform.PropertyStorage properties) => properties.SampleRate, 0);

			// Token: 0x040006A7 RID: 1703
			public static readonly Platform.PropertyAccessor<string> BuildDirectory = new Platform.PropertyAccessor<string>((Platform.PropertyStorage properties) => properties.BuildDirectory, "Desktop");

			// Token: 0x040006A8 RID: 1704
			public static readonly Platform.PropertyAccessor<SPEAKERMODE> SpeakerMode = new Platform.PropertyAccessor<SPEAKERMODE>((Platform.PropertyStorage properties) => properties.SpeakerMode, SPEAKERMODE.STEREO);

			// Token: 0x040006A9 RID: 1705
			public static readonly Platform.PropertyAccessor<int> VirtualChannelCount = new Platform.PropertyAccessor<int>((Platform.PropertyStorage properties) => properties.VirtualChannelCount, 128);

			// Token: 0x040006AA RID: 1706
			public static readonly Platform.PropertyAccessor<int> RealChannelCount = new Platform.PropertyAccessor<int>((Platform.PropertyStorage properties) => properties.RealChannelCount, 32);

			// Token: 0x040006AB RID: 1707
			public static readonly Platform.PropertyAccessor<int> DSPBufferLength = new Platform.PropertyAccessor<int>((Platform.PropertyStorage properties) => properties.DSPBufferLength, 0);

			// Token: 0x040006AC RID: 1708
			public static readonly Platform.PropertyAccessor<int> DSPBufferCount = new Platform.PropertyAccessor<int>((Platform.PropertyStorage properties) => properties.DSPBufferCount, 0);

			// Token: 0x040006AD RID: 1709
			public static readonly Platform.PropertyAccessor<List<string>> Plugins = new Platform.PropertyAccessor<List<string>>((Platform.PropertyStorage properties) => properties.Plugins, null);

			// Token: 0x040006AE RID: 1710
			public static readonly Platform.PropertyAccessor<List<string>> StaticPlugins = new Platform.PropertyAccessor<List<string>>((Platform.PropertyStorage properties) => properties.StaticPlugins, null);

			// Token: 0x040006AF RID: 1711
			public static readonly Platform.PropertyAccessor<PlatformCallbackHandler> CallbackHandler = new Platform.PropertyAccessor<PlatformCallbackHandler>((Platform.PropertyStorage properties) => properties.CallbackHandler, null);
		}

		// Token: 0x02000140 RID: 320
		[Serializable]
		public class PropertyThreadAffinityList : Platform.Property<List<ThreadAffinityGroup>>
		{
		}

		// Token: 0x02000141 RID: 321
		[Serializable]
		internal class PropertyCodecChannels : Platform.Property<List<CodecChannelCount>>
		{
		}
	}
}
