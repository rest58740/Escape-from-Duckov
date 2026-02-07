using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200002F RID: 47
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	public class HideInInlineEditorsAttribute : Attribute
	{
	}
}
