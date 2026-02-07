using System;
using System.Collections.Generic;
using System.IO;
using FMOD;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x02000112 RID: 274
	public class PlatformPlayInEditor : Platform
	{
		// Token: 0x06000705 RID: 1797 RVA: 0x00008538 File Offset: 0x00006738
		public PlatformPlayInEditor()
		{
			base.Identifier = "playInEditor";
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0000854B File Offset: 0x0000674B
		internal override string DisplayName
		{
			get
			{
				return "Editor";
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00008552 File Offset: 0x00006752
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
			settings.DeclareRuntimePlatform(RuntimePlatform.OSXEditor, this);
			settings.DeclareRuntimePlatform(RuntimePlatform.WindowsEditor, this);
			settings.DeclareRuntimePlatform(RuntimePlatform.LinuxEditor, this);
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0000856D File Offset: 0x0000676D
		internal override bool IsIntrinsic
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00008570 File Offset: 0x00006770
		internal override string GetBankFolder()
		{
			Settings instance = Settings.Instance;
			string text = instance.SourceBankPath;
			if (instance.HasPlatforms)
			{
				text = RuntimeUtils.GetCommonPlatformPath(Path.Combine(text, base.BuildDirectory));
			}
			return text;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000085A3 File Offset: 0x000067A3
		internal override void LoadStaticPlugins(System coreSystem, Action<RESULT, string> reportResult)
		{
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x000085A8 File Offset: 0x000067A8
		internal override void InitializeProperties()
		{
			base.InitializeProperties();
			Platform.PropertyAccessors.LiveUpdate.Set(this, TriStateBool.Enabled);
			Platform.PropertyAccessors.Overlay.Set(this, TriStateBool.Enabled);
			Platform.PropertyAccessors.SampleRate.Set(this, 48000);
			Platform.PropertyAccessors.RealChannelCount.Set(this, 256);
			Platform.PropertyAccessors.VirtualChannelCount.Set(this, 1024);
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00008612 File Offset: 0x00006812
		internal override List<CodecChannelCount> DefaultCodecChannels
		{
			get
			{
				return PlatformPlayInEditor.staticCodecChannels;
			}
		}

		// Token: 0x0400059C RID: 1436
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
				channels = 256
			}
		};
	}
}
