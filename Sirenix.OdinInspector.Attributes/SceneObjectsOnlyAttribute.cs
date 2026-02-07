using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200005E RID: 94
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class SceneObjectsOnlyAttribute : Attribute
	{
	}
}
