using System;
using UnityEngine;

namespace LeTai.TrueShadow.PluginInterfaces
{
	// Token: 0x02000020 RID: 32
	public interface ITrueShadowCasterMaterialProvider
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600011F RID: 287
		// (remove) Token: 0x06000120 RID: 288
		event Action materialReplaced;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000121 RID: 289
		// (remove) Token: 0x06000122 RID: 290
		event Action materialModified;

		// Token: 0x06000123 RID: 291
		Material GetTrueShadowCasterMaterial();
	}
}
