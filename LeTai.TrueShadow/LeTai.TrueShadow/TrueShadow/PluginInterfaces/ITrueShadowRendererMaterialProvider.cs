using System;
using UnityEngine;

namespace LeTai.TrueShadow.PluginInterfaces
{
	// Token: 0x02000024 RID: 36
	public interface ITrueShadowRendererMaterialProvider
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000127 RID: 295
		// (remove) Token: 0x06000128 RID: 296
		event Action materialReplaced;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000129 RID: 297
		// (remove) Token: 0x0600012A RID: 298
		event Action materialModified;

		// Token: 0x0600012B RID: 299
		Material GetTrueShadowRendererMaterial();
	}
}
