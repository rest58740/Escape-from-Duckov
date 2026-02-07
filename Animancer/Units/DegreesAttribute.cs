using System;
using System.Diagnostics;

namespace Animancer.Units
{
	// Token: 0x0200006F RID: 111
	[Conditional("UNITY_EDITOR")]
	public sealed class DegreesAttribute : UnitsAttribute
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x0000EE43 File Offset: 0x0000D043
		public DegreesAttribute() : base(" º")
		{
		}
	}
}
