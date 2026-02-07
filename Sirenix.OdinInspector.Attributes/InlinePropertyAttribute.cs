using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200003F RID: 63
	[AttributeUsage(32767, Inherited = false)]
	[Conditional("UNITY_EDITOR")]
	public class InlinePropertyAttribute : Attribute
	{
		// Token: 0x0400008D RID: 141
		public int LabelWidth;
	}
}
