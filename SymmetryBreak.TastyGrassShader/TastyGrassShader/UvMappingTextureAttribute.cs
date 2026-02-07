using System;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x0200000D RID: 13
	public class UvMappingTextureAttribute : PropertyAttribute
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002165 File Offset: 0x00000365
		public UvMappingTextureAttribute(string stylePropertyName, string proceduralShapeBlend, string upperArcAttribute, string lowerArcAttribute, string tipRounding)
		{
			this.StylePropertyName = stylePropertyName;
			this.ProceduralShapeBlend = proceduralShapeBlend;
			this.UpperArcAttribute = upperArcAttribute;
			this.LowerArcAttribute = lowerArcAttribute;
			this.TipRounding = tipRounding;
		}

		// Token: 0x04000013 RID: 19
		public string LowerArcAttribute;

		// Token: 0x04000014 RID: 20
		public string ProceduralShapeBlend;

		// Token: 0x04000015 RID: 21
		public string StylePropertyName;

		// Token: 0x04000016 RID: 22
		public string TipRounding;

		// Token: 0x04000017 RID: 23
		public string UpperArcAttribute;
	}
}
