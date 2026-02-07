using System;

namespace Sirenix.Utilities
{
	// Token: 0x02000034 RID: 52
	public class SirenixEditorConfigAttribute : GlobalConfigAttribute
	{
		// Token: 0x06000238 RID: 568 RVA: 0x0000D0DF File Offset: 0x0000B2DF
		public SirenixEditorConfigAttribute() : base(SirenixAssetPaths.OdinEditorConfigsPath)
		{
		}
	}
}
