using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000016 RID: 22
	[AttributeUsage(32767)]
	[DontApplyToListElements]
	[Conditional("UNITY_EDITOR")]
	public class DisableInPlayModeAttribute : Attribute
	{
	}
}
