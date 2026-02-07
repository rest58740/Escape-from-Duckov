using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200001F RID: 31
	[AttributeUsage(32767)]
	[Conditional("UNITY_EDITOR")]
	public class DrawWithUnityAttribute : Attribute
	{
		// Token: 0x0400004B RID: 75
		public bool PreferImGUI;
	}
}
