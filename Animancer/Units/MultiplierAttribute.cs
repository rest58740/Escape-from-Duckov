using System;
using System.Diagnostics;

namespace Animancer.Units
{
	// Token: 0x02000074 RID: 116
	[Conditional("UNITY_EDITOR")]
	public sealed class MultiplierAttribute : UnitsAttribute
	{
		// Token: 0x06000596 RID: 1430 RVA: 0x0000EE84 File Offset: 0x0000D084
		public MultiplierAttribute() : base(" x")
		{
		}
	}
}
