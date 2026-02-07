using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000034 RID: 52
	[Obsolete("Use [HideIn(PrefabKind.PrefabAsset | PrefabKind.PrefabInstance)] instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	public class HideInPrefabsAttribute : Attribute
	{
	}
}
