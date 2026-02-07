using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000017 RID: 23
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use [DisableIn(PrefabKind.PrefabAsset)] instead.", false)]
	public class DisableInPrefabAssetsAttribute : Attribute
	{
	}
}
