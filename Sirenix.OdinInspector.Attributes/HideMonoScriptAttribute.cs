using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000037 RID: 55
	[AttributeUsage(4, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class HideMonoScriptAttribute : Attribute
	{
	}
}
