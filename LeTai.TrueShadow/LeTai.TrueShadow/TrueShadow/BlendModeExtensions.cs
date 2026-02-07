using System;
using UnityEngine;

namespace LeTai.TrueShadow
{
	// Token: 0x02000014 RID: 20
	public static class BlendModeExtensions
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public static Material GetMaterial(this BlendMode blendMode)
		{
			switch (blendMode)
			{
			case BlendMode.Normal:
				if (!BlendModeExtensions.matNormal)
				{
					BlendModeExtensions.matNormal = new Material(Shader.Find("UI/TrueShadow-Normal"));
				}
				return BlendModeExtensions.matNormal;
			case BlendMode.Additive:
				if (!BlendModeExtensions.materialAdditive)
				{
					BlendModeExtensions.materialAdditive = new Material(Shader.Find("UI/TrueShadow-Additive"));
				}
				return BlendModeExtensions.materialAdditive;
			case BlendMode.Screen:
				if (!BlendModeExtensions.matScreen)
				{
					BlendModeExtensions.matScreen = new Material(Shader.Find("UI/TrueShadow-Screen"));
				}
				return BlendModeExtensions.matScreen;
			case BlendMode.Multiply:
				if (!BlendModeExtensions.matMultiply)
				{
					BlendModeExtensions.matMultiply = new Material(Shader.Find("UI/TrueShadow-Multiply"));
				}
				return BlendModeExtensions.matMultiply;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0400006E RID: 110
		private static Material matNormal;

		// Token: 0x0400006F RID: 111
		private static Material materialAdditive;

		// Token: 0x04000070 RID: 112
		private static Material matScreen;

		// Token: 0x04000071 RID: 113
		private static Material matMultiply;
	}
}
