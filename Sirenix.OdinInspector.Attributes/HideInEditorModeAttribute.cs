using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200002E RID: 46
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	public class HideInEditorModeAttribute : Attribute
	{
	}
}
