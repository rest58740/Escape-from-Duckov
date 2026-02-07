using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200001E RID: 30
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class DontValidateAttribute : Attribute
	{
	}
}
