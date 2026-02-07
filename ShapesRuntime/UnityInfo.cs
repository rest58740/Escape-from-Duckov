using System;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x0200007D RID: 125
	internal static class UnityInfo
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x0001CB0D File Offset: 0x0001AD0D
		public static bool UsingSRP
		{
			get
			{
				return GraphicsSettings.defaultRenderPipeline != null;
			}
		}

		// Token: 0x0400030C RID: 780
		public const int INSTANCES_MAX = 1023;
	}
}
