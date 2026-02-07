using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Attributes
{
	// Token: 0x02000026 RID: 38
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class RigAssetSelectorAttribute : PropertyAttribute
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00003CDF File Offset: 0x00001EDF
		public RigAssetSelectorAttribute(string rigName = "")
		{
			this.assetName = rigName;
		}

		// Token: 0x0400005B RID: 91
		public string assetName;
	}
}
