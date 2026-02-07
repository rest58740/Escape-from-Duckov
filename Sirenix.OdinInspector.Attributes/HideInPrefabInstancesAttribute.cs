using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000033 RID: 51
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use [HideIn(PrefabKind.PrefabInstance)] instead.", false)]
	public class HideInPrefabInstancesAttribute : Attribute
	{
	}
}
