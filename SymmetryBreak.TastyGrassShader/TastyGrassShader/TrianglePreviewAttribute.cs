using System;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x0200000B RID: 11
	public class TrianglePreviewAttribute : PropertyAttribute
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002140 File Offset: 0x00000340
		public TrianglePreviewAttribute(string heightName, string widthName, string thicknessName, string thicknessApexName)
		{
			this.HeightName = heightName;
			this.WidthName = widthName;
			this.ThicknessName = thicknessName;
			this.ThicknessApexName = thicknessApexName;
		}

		// Token: 0x0400000F RID: 15
		public string HeightName;

		// Token: 0x04000010 RID: 16
		public string WidthName;

		// Token: 0x04000011 RID: 17
		public string ThicknessName;

		// Token: 0x04000012 RID: 18
		public string ThicknessApexName;
	}
}
