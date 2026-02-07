using System;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x020000F6 RID: 246
	public class PlatformAndroid : Platform
	{
		// Token: 0x0600064C RID: 1612 RVA: 0x00006FC5 File Offset: 0x000051C5
		static PlatformAndroid()
		{
			Settings.AddPlatformTemplate<PlatformAndroid>("2fea114e74ecf3c4f920e1d5cc1c4c40");
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00006FD1 File Offset: 0x000051D1
		internal override string DisplayName
		{
			get
			{
				return "Android";
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00006FD8 File Offset: 0x000051D8
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
			settings.DeclareRuntimePlatform(RuntimePlatform.Android, this);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00006FE3 File Offset: 0x000051E3
		internal override string GetBankFolder()
		{
			return PlatformAndroid.StaticGetBankFolder();
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00006FEA File Offset: 0x000051EA
		internal static string StaticGetBankFolder()
		{
			if (!Settings.Instance.AndroidUseOBB && !Settings.Instance.AndroidPatchBuild)
			{
				return "file:///android_asset";
			}
			return Application.streamingAssetsPath;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0000700F File Offset: 0x0000520F
		internal override string GetPluginPath(string pluginName)
		{
			return PlatformAndroid.StaticGetPluginPath(pluginName);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00007017 File Offset: 0x00005217
		internal static string StaticGetPluginPath(string pluginName)
		{
			return string.Format("lib{0}.so", pluginName);
		}
	}
}
