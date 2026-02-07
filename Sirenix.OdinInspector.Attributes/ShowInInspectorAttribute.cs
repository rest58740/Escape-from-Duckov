using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000065 RID: 101
	[MeansImplicitUse]
	[AttributeUsage(32767, AllowMultiple = false, Inherited = false)]
	[Conditional("UNITY_EDITOR")]
	public class ShowInInspectorAttribute : Attribute
	{
	}
}
