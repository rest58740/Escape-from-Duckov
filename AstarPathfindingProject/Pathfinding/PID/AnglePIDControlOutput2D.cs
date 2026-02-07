using System;
using Unity.Mathematics;

namespace Pathfinding.PID
{
	// Token: 0x02000253 RID: 595
	public struct AnglePIDControlOutput2D
	{
		// Token: 0x06000DF7 RID: 3575 RVA: 0x00058A78 File Offset: 0x00056C78
		public AnglePIDControlOutput2D(float currentRotation, float targetRotation, float rotationDelta, float moveDistance)
		{
			float y;
			float x;
			math.sincos(currentRotation + rotationDelta * 0.5f, out y, out x);
			this.rotationDelta = rotationDelta;
			this.positionDelta = new float2(x, y) * moveDistance;
			this.targetRotation = targetRotation;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00058ABC File Offset: 0x00056CBC
		public static AnglePIDControlOutput2D WithMovementAtEnd(float currentRotation, float targetRotation, float rotationDelta, float moveDistance)
		{
			float y;
			float x;
			math.sincos(currentRotation + rotationDelta, out y, out x);
			return new AnglePIDControlOutput2D
			{
				rotationDelta = rotationDelta,
				targetRotation = targetRotation,
				positionDelta = new float2(x, y) * moveDistance
			};
		}

		// Token: 0x04000ACD RID: 2765
		public float rotationDelta;

		// Token: 0x04000ACE RID: 2766
		public float targetRotation;

		// Token: 0x04000ACF RID: 2767
		public float2 positionDelta;
	}
}
