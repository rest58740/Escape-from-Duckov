using System;
using TMPro;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000070 RID: 112
	public class ShapesAssets : ScriptableObject
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00019B3C File Offset: 0x00017D3C
		public static ShapesAssets Instance
		{
			get
			{
				if (ShapesAssets.inst == null)
				{
					ShapesAssets.inst = Resources.Load<ShapesAssets>("Shapes Assets");
				}
				return ShapesAssets.inst;
			}
		}

		// Token: 0x04000275 RID: 629
		[Header("Config")]
		public TMP_FontAsset defaultFont;

		// Token: 0x04000276 RID: 630
		[Header("Meshes")]
		public Mesh[] meshQuad = new Mesh[5];

		// Token: 0x04000277 RID: 631
		public Mesh[] meshTriangle = new Mesh[5];

		// Token: 0x04000278 RID: 632
		public Mesh[] meshCube = new Mesh[5];

		// Token: 0x04000279 RID: 633
		public Mesh[] meshSphere = new Mesh[5];

		// Token: 0x0400027A RID: 634
		public Mesh[] meshTorus = new Mesh[5];

		// Token: 0x0400027B RID: 635
		public Mesh[] meshCapsule = new Mesh[5];

		// Token: 0x0400027C RID: 636
		public Mesh[] meshCylinder = new Mesh[5];

		// Token: 0x0400027D RID: 637
		public Mesh[] meshCone = new Mesh[5];

		// Token: 0x0400027E RID: 638
		public Mesh[] meshConeUncapped = new Mesh[5];

		// Token: 0x0400027F RID: 639
		[Header("Misc")]
		public TextAsset packageJson;

		// Token: 0x04000280 RID: 640
		private static ShapesAssets inst;
	}
}
