using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000015 RID: 21
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Conditional("UNITY_EDITOR")]
	[Obsolete("Use [DisableIn(PrefabKind.NonPrefabInstance)] instead.", false)]
	public class DisableInNonPrefabsAttribute : Attribute
	{
	}
}
