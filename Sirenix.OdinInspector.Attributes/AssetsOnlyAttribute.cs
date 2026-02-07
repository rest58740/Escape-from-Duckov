using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class AssetsOnlyAttribute : Attribute
	{
	}
}
