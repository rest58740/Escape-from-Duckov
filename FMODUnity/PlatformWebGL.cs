using System;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x020000F7 RID: 247
	public class PlatformWebGL : Platform
	{
		// Token: 0x06000654 RID: 1620 RVA: 0x0000702C File Offset: 0x0000522C
		static PlatformWebGL()
		{
			Settings.AddPlatformTemplate<PlatformWebGL>("46fbfdf3fc43db0458918377fd40293e");
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00007038 File Offset: 0x00005238
		internal override string DisplayName
		{
			get
			{
				return "WebGL";
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0000703F File Offset: 0x0000523F
		internal override void DeclareRuntimePlatforms(Settings settings)
		{
			settings.DeclareRuntimePlatform(RuntimePlatform.WebGLPlayer, this);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0000704A File Offset: 0x0000524A
		internal override string GetPluginPath(string pluginName)
		{
			return string.Format("{0}/{1}.a", this.GetPluginBasePath(), pluginName);
		}
	}
}
