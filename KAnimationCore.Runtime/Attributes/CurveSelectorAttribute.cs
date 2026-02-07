using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Attributes
{
	// Token: 0x02000024 RID: 36
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class CurveSelectorAttribute : PropertyAttribute
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00003CBA File Offset: 0x00001EBA
		public CurveSelectorAttribute(bool useAnimator = true, bool usePlayables = true, bool useInput = true)
		{
			this.useAnimator = useAnimator;
			this.usePlayables = usePlayables;
			this.useInput = useInput;
		}

		// Token: 0x04000058 RID: 88
		public bool useAnimator;

		// Token: 0x04000059 RID: 89
		public bool usePlayables;

		// Token: 0x0400005A RID: 90
		public bool useInput;
	}
}
