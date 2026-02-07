using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x020000F9 RID: 249
	public class PlatformLinux : Platform
	{
		// Token: 0x0600065F RID: 1631 RVA: 0x000070A0 File Offset: 0x000052A0
		static PlatformLinux()
		{
			Settings.AddPlatformTemplate<PlatformLinux>("b7716510a1f36934c87976f3a81dbf3d");
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x000070F4 File Offset: 0x000052F4
		internal override string DisplayName
		{
			get
			{
				return "Linux";
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000070FB File Offset: 0x000052FB
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
			settings.DeclareRuntimePlatform(RuntimePlatform.LinuxPlayer, this);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00007106 File Offset: 0x00005306
		internal override string GetPluginPath(string pluginName)
		{
			return string.Format("{0}/lib{1}.so", this.GetPluginBasePath(), pluginName);
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00007119 File Offset: 0x00005319
		internal override List<CodecChannelCount> DefaultCodecChannels
		{
			get
			{
				return PlatformLinux.staticCodecChannels;
			}
		}

		// Token: 0x0400054F RID: 1359
		private static List<CodecChannelCount> staticCodecChannels = new List<CodecChannelCount>
		{
			new CodecChannelCount
			{
				format = CodecType.FADPCM,
				channels = 0
			},
			new CodecChannelCount
			{
				format = CodecType.Vorbis,
				channels = 32
			}
		};
	}
}
