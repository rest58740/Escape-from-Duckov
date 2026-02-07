using System;
using System.Diagnostics;

namespace Animancer
{
	// Token: 0x02000020 RID: 32
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class ThresholdLabelAttribute : Attribute
	{
		// Token: 0x06000321 RID: 801 RVA: 0x00008F44 File Offset: 0x00007144
		public ThresholdLabelAttribute(string label)
		{
		}
	}
}
