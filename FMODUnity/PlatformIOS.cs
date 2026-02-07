using System;
using FMOD;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x020000F8 RID: 248
	public class PlatformIOS : Platform
	{
		// Token: 0x06000659 RID: 1625 RVA: 0x00007065 File Offset: 0x00005265
		static PlatformIOS()
		{
			Settings.AddPlatformTemplate<PlatformIOS>("0f8eb3f400726694eb47beb1a9f94ce8");
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00007071 File Offset: 0x00005271
		internal override string DisplayName
		{
			get
			{
				return "iOS";
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00007078 File Offset: 0x00005278
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
			settings.DeclareRuntimePlatform(RuntimePlatform.IPhonePlayer, this);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00007082 File Offset: 0x00005282
		internal override void LoadPlugins(System coreSystem, Action<RESULT, string> reportResult)
		{
			PlatformIOS.StaticLoadPlugins(this, coreSystem, reportResult);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0000708C File Offset: 0x0000528C
		public static void StaticLoadPlugins(Platform platform, System coreSystem, Action<RESULT, string> reportResult)
		{
			platform.LoadStaticPlugins(coreSystem, reportResult);
		}
	}
}
