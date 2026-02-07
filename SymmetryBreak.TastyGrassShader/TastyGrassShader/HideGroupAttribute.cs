using System;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader
{
	// Token: 0x02000007 RID: 7
	public class HideGroupAttribute : PropertyAttribute
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000210D File Offset: 0x0000030D
		public HideGroupAttribute(string valueName, int showIfValue)
		{
			this.ValueName = valueName;
			this.ShowIfValue = showIfValue;
		}

		// Token: 0x0400000A RID: 10
		public readonly int ShowIfValue;

		// Token: 0x0400000B RID: 11
		public readonly string ValueName;
	}
}
