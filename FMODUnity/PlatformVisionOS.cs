using System;
using FMOD;

namespace FMODUnity
{
	// Token: 0x020000FC RID: 252
	public class PlatformVisionOS : Platform
	{
		// Token: 0x06000670 RID: 1648 RVA: 0x00007209 File Offset: 0x00005409
		static PlatformVisionOS()
		{
			Settings.AddPlatformTemplate<PlatformVisionOS>("de700ef3f37a49b58a57ae3addf01ad9");
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00007215 File Offset: 0x00005415
		internal override string DisplayName
		{
			get
			{
				return "visionOS";
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0000721C File Offset: 0x0000541C
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0000721E File Offset: 0x0000541E
		internal override void LoadPlugins(System coreSystem, Action<RESULT, string> reportResult)
		{
			PlatformIOS.StaticLoadPlugins(this, coreSystem, reportResult);
		}
	}
}
