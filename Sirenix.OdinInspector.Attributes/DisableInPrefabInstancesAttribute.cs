using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000018 RID: 24
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use [DisableIn(PrefabKind.PrefabInstance)] instead.", false)]
	public class DisableInPrefabInstancesAttribute : Attribute
	{
	}
}
