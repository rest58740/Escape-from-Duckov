using System;
using FMOD;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x020000FB RID: 251
	public class PlatformAppleTV : Platform
	{
		// Token: 0x0600066B RID: 1643 RVA: 0x000071D9 File Offset: 0x000053D9
		static PlatformAppleTV()
		{
			Settings.AddPlatformTemplate<PlatformAppleTV>("e7a046c753c3c3d4aacc91f6597f310d");
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x000071E5 File Offset: 0x000053E5
		internal override string DisplayName
		{
			get
			{
				return "Apple TV";
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x000071EC File Offset: 0x000053EC
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
			settings.DeclareRuntimePlatform(RuntimePlatform.tvOS, this);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x000071F7 File Offset: 0x000053F7
		internal override void LoadPlugins(System coreSystem, Action<RESULT, string> reportResult)
		{
			PlatformIOS.StaticLoadPlugins(this, coreSystem, reportResult);
		}
	}
}
