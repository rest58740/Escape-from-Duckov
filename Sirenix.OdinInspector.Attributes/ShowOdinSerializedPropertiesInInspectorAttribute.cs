using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000066 RID: 102
	[AttributeUsage(4, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class ShowOdinSerializedPropertiesInInspectorAttribute : Attribute
	{
	}
}
