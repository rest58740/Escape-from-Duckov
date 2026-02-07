using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000013 RID: 19
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	public class DisableInEditorModeAttribute : Attribute
	{
	}
}
