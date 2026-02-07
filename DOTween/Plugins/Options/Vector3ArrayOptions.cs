using System;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000037 RID: 55
	public struct Vector3ArrayOptions : IPlugOptions
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000D91E File Offset: 0x0000BB1E
		public void Reset()
		{
			this.axisConstraint = AxisConstraint.None;
			this.snapping = false;
			this.durations = null;
		}

		// Token: 0x04000104 RID: 260
		public AxisConstraint axisConstraint;

		// Token: 0x04000105 RID: 261
		public bool snapping;

		// Token: 0x04000106 RID: 262
		internal float[] durations;
	}
}
