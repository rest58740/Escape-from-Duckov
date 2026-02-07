using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000069 RID: 105
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class SuppressInvalidAttributeErrorAttribute : Attribute
	{
	}
}
