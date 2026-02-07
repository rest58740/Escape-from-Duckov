using System;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x02000005 RID: 5
	public class DisableGroupAttribute : PropertyAttribute
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000020EF File Offset: 0x000002EF
		public DisableGroupAttribute(string valueName, bool flipCondition = false)
		{
			this.valueName = valueName;
			this.flipCondition = flipCondition;
		}

		// Token: 0x04000008 RID: 8
		public bool flipCondition;

		// Token: 0x04000009 RID: 9
		public string valueName;
	}
}
