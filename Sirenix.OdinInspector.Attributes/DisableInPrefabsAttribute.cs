using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000019 RID: 25
	[Obsolete("Use [DisableIn(PrefabKind.PrefabAsset | PrefabKind.PrefabInstance)] instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	public class DisableInPrefabsAttribute : Attribute
	{
	}
}
