using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Options
{
	// Token: 0x02000035 RID: 53
	public struct QuaternionOptions : IPlugOptions
	{
		// Token: 0x06000251 RID: 593 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
		public void Reset()
		{
			this.rotateMode = RotateMode.Fast;
			this.axisConstraint = AxisConstraint.None;
			this.up = Vector3.zero;
			this.dynamicLookAt = false;
			this.dynamicLookAtWorldPosition = Vector3.zero;
		}

		// Token: 0x040000FE RID: 254
		public RotateMode rotateMode;

		// Token: 0x040000FF RID: 255
		public AxisConstraint axisConstraint;

		// Token: 0x04000100 RID: 256
		public Vector3 up;

		// Token: 0x04000101 RID: 257
		public bool dynamicLookAt;

		// Token: 0x04000102 RID: 258
		public Vector3 dynamicLookAtWorldPosition;
	}
}
