using System;
using System.Diagnostics;

namespace Animancer.Units
{
	// Token: 0x02000070 RID: 112
	[Conditional("UNITY_EDITOR")]
	public sealed class DegreesPerSecondAttribute : UnitsAttribute
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x0000EE50 File Offset: 0x0000D050
		public DegreesPerSecondAttribute() : base(" º/s")
		{
		}
	}
}
