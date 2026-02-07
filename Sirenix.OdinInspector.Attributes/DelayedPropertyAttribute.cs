using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200000D RID: 13
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class DelayedPropertyAttribute : Attribute
	{
	}
}
