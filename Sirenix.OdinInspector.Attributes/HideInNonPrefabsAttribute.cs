using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000030 RID: 48
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use [HideIn(PrefabKind.NonPrefabInstance)] instead.", false)]
	public class HideInNonPrefabsAttribute : Attribute
	{
	}
}
