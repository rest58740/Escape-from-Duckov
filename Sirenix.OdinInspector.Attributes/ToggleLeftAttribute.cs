using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000073 RID: 115
	[AttributeUsage(32767, AllowMultiple = true, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class ToggleLeftAttribute : Attribute
	{
	}
}
