using System;
using System.Diagnostics;

namespace Animancer.Units
{
	// Token: 0x02000075 RID: 117
	[Conditional("UNITY_EDITOR")]
	public sealed class SecondsAttribute : UnitsAttribute
	{
		// Token: 0x06000597 RID: 1431 RVA: 0x0000EE91 File Offset: 0x0000D091
		public SecondsAttribute() : base(" s")
		{
		}
	}
}
