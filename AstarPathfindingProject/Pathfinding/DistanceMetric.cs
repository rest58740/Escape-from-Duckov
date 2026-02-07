using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000027 RID: 39
	public struct DistanceMetric
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00009BDA File Offset: 0x00007DDA
		public bool isProjectedDistance
		{
			get
			{
				return this.projectionAxis != Vector3.zero;
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009BEC File Offset: 0x00007DEC
		public static DistanceMetric ClosestAsSeenFromAboveSoft()
		{
			return new DistanceMetric
			{
				projectionAxis = Vector3.positiveInfinity,
				distanceScaleAlongProjectionDirection = 0.2f
			};
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009C1C File Offset: 0x00007E1C
		public static DistanceMetric ClosestAsSeenFromAboveSoft(Vector3 up)
		{
			return new DistanceMetric
			{
				projectionAxis = up,
				distanceScaleAlongProjectionDirection = 0.2f
			};
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009C48 File Offset: 0x00007E48
		public static DistanceMetric ClosestAsSeenFromAbove()
		{
			return new DistanceMetric
			{
				projectionAxis = Vector3.positiveInfinity,
				distanceScaleAlongProjectionDirection = 0f
			};
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009C78 File Offset: 0x00007E78
		public static DistanceMetric ClosestAsSeenFromAbove(Vector3 up)
		{
			return new DistanceMetric
			{
				projectionAxis = up,
				distanceScaleAlongProjectionDirection = 0f
			};
		}

		// Token: 0x04000134 RID: 308
		public Vector3 projectionAxis;

		// Token: 0x04000135 RID: 309
		public float distanceScaleAlongProjectionDirection;

		// Token: 0x04000136 RID: 310
		public static readonly DistanceMetric Euclidean = new DistanceMetric
		{
			projectionAxis = Vector3.zero,
			distanceScaleAlongProjectionDirection = 0f
		};
	}
}
