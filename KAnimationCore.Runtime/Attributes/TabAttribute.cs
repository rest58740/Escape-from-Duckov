using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Attributes
{
	// Token: 0x0200002A RID: 42
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class TabAttribute : PropertyAttribute
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00003D12 File Offset: 0x00001F12
		public TabAttribute(string tabName)
		{
			this.tabName = tabName;
		}

		// Token: 0x0400005C RID: 92
		public string tabName;
	}
}
