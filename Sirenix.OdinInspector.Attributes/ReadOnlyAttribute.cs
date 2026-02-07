using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000057 RID: 87
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class ReadOnlyAttribute : Attribute
	{
	}
}
