using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000039 RID: 57
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class HideReferenceObjectPickerAttribute : Attribute
	{
	}
}
