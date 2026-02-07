using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000009 RID: 9
	[Conditional("UNITY_EDITOR")]
	public class ChildGameObjectsOnlyAttribute : Attribute
	{
		// Token: 0x04000030 RID: 48
		public bool IncludeSelf = true;

		// Token: 0x04000031 RID: 49
		public bool IncludeInactive;
	}
}
