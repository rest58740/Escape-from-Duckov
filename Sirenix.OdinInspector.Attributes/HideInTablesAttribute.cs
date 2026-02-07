using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000035 RID: 53
	[AttributeUsage(32767, AllowMultiple = false)]
	[Conditional("UNITY_EDITOR")]
	public class HideInTablesAttribute : Attribute
	{
	}
}
