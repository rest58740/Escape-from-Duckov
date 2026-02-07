using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x0200003D RID: 61
	public struct VectorOptions : IPlugOptions
	{
		// Token: 0x06000259 RID: 601 RVA: 0x0000D982 File Offset: 0x0000BB82
		public void Reset()
		{
			this.axisConstraint = AxisConstraint.None;
			this.snapping = false;
		}

		// Token: 0x0400010F RID: 271
		public AxisConstraint axisConstraint;

		// Token: 0x04000110 RID: 272
		public bool snapping;
	}
}
