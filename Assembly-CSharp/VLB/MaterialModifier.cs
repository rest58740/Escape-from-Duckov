using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000030 RID: 48
	public static class MaterialModifier
	{
		// Token: 0x020000B2 RID: 178
		public interface Interface
		{
			// Token: 0x060004B9 RID: 1209
			void SetMaterialProp(int nameID, float value);

			// Token: 0x060004BA RID: 1210
			void SetMaterialProp(int nameID, Vector4 value);

			// Token: 0x060004BB RID: 1211
			void SetMaterialProp(int nameID, Color value);

			// Token: 0x060004BC RID: 1212
			void SetMaterialProp(int nameID, Matrix4x4 value);

			// Token: 0x060004BD RID: 1213
			void SetMaterialProp(int nameID, Texture value);
		}

		// Token: 0x020000B3 RID: 179
		// (Invoke) Token: 0x060004BF RID: 1215
		public delegate void Callback(MaterialModifier.Interface owner);
	}
}
