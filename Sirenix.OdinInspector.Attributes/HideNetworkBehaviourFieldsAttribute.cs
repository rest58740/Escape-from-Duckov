using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000038 RID: 56
	[AttributeUsage(4, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class HideNetworkBehaviourFieldsAttribute : Attribute
	{
	}
}
