using System;
using System.Diagnostics;

namespace Animancer.Units
{
	// Token: 0x02000072 RID: 114
	[Conditional("UNITY_EDITOR")]
	public sealed class MetersPerSecondAttribute : UnitsAttribute
	{
		// Token: 0x06000594 RID: 1428 RVA: 0x0000EE6A File Offset: 0x0000D06A
		public MetersPerSecondAttribute() : base(" m/s")
		{
		}
	}
}
