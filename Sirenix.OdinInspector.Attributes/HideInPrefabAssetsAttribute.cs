using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000032 RID: 50
	[DontApplyToListElements]
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	[Obsolete("Use [HideIn(PrefabKind.PrefabAsset)] instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class HideInPrefabAssetsAttribute : Attribute
	{
	}
}
