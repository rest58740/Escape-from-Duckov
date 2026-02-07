using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000014 RID: 20
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	public class DisableInInlineEditorsAttribute : Attribute
	{
	}
}
