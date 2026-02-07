using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x020000FA RID: 250
	public class PlatformMac : Platform
	{
		// Token: 0x06000665 RID: 1637 RVA: 0x00007128 File Offset: 0x00005328
		static PlatformMac()
		{
			Settings.AddPlatformTemplate<PlatformMac>("52eb9df5db46521439908db3a29a1bbb");
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0000717C File Offset: 0x0000537C
		internal override string DisplayName
		{
			get
			{
				return "macOS";
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00007183 File Offset: 0x00005383
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
			settings.DeclareRuntimePlatform(RuntimePlatform.OSXPlayer, this);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00007190 File Offset: 0x00005390
		internal override string GetPluginPath(string pluginName)
		{
			string text = string.Format("{0}/{1}.bundle", this.GetPluginBasePath(), pluginName);
			if (Directory.Exists(text))
			{
				return text;
			}
			return string.Format("{0}/{1}.dylib", this.GetPluginBasePath(), pluginName);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x000071CA File Offset: 0x000053CA
		internal override List<CodecChannelCount> DefaultCodecChannels
		{
			get
			{
				return PlatformMac.staticCodecChannels;
			}
		}

		// Token: 0x04000550 RID: 1360
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
