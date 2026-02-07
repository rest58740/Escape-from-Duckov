using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x020000FD RID: 253
	public class PlatformWindows : Platform
	{
		// Token: 0x06000675 RID: 1653 RVA: 0x00007230 File Offset: 0x00005430
		static PlatformWindows()
		{
			Settings.AddPlatformTemplate<PlatformWindows>("2c5177b11d81d824dbb064f9ac8527da");
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x00007284 File Offset: 0x00005484
		internal override string DisplayName
		{
			get
			{
				return "Windows";
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0000728B File Offset: 0x0000548B
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
			settings.DeclareRuntimePlatform(RuntimePlatform.WindowsPlayer, this);
			settings.DeclareRuntimePlatform(RuntimePlatform.MetroPlayerX86, this);
			settings.DeclareRuntimePlatform(RuntimePlatform.MetroPlayerX64, this);
			settings.DeclareRuntimePlatform(RuntimePlatform.MetroPlayerARM, this);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000072B0 File Offset: 0x000054B0
		internal override string GetPluginPath(string pluginName)
		{
			return string.Format("{0}/X86_64/{1}.dll", this.GetPluginBasePath(), pluginName);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x000072C3 File Offset: 0x000054C3
		internal override List<CodecChannelCount> DefaultCodecChannels
		{
			get
			{
				return PlatformWindows.staticCodecChannels;
			}
		}

		// Token: 0x04000551 RID: 1361
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
