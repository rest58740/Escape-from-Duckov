using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000064 RID: 100
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	public class ShowInInlineEditorsAttribute : Attribute
	{
	}
}
