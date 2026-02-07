using System;
using System.Diagnostics;

namespace Animancer.Units
{
	// Token: 0x0200006E RID: 110
	[Conditional("UNITY_EDITOR")]
	public sealed class AnimationTimeAttribute : UnitsAttribute
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x0000EE3B File Offset: 0x0000D03B
		public AnimationTimeAttribute(AnimationTimeAttribute.Units units)
		{
		}

		// Token: 0x040000FB RID: 251
		public const string Tooltip = "x = Normalized, s = Seconds, f = Frame";

		// Token: 0x020000B6 RID: 182
		public enum Units
		{
			// Token: 0x04000191 RID: 401
			Normalized,
			// Token: 0x04000192 RID: 402
			Seconds,
			// Token: 0x04000193 RID: 403
			Frames
		}
	}
}
