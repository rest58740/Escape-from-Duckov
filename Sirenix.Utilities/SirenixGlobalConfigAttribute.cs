using System;

namespace Sirenix.Utilities
{
	// Token: 0x02000035 RID: 53
	public class SirenixGlobalConfigAttribute : GlobalConfigAttribute
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000D0EC File Offset: 0x0000B2EC
		public SirenixGlobalConfigAttribute() : base(SirenixAssetPaths.OdinResourcesConfigsPath)
		{
		}
	}
}
