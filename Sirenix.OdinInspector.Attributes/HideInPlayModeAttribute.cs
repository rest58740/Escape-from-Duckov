using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000031 RID: 49
	[AttributeUsage(32767)]
	[DontApplyToListElements]
	[Conditional("UNITY_EDITOR")]
	public class HideInPlayModeAttribute : Attribute
	{
	}
}
