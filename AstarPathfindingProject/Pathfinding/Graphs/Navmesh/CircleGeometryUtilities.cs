using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x0200019B RID: 411
	[BurstCompile]
	internal struct CircleGeometryUtilities
	{
		// Token: 0x06000B40 RID: 2880 RVA: 0x0003F2C4 File Offset: 0x0003D4C4
		public static int CircleSteps(Matrix4x4 matrix, float radius, float maxError)
		{
			float num = math.sqrt(math.max(math.max(math.lengthsq(matrix.GetColumn(0)), math.lengthsq(matrix.GetColumn(1))), math.lengthsq(matrix.GetColumn(2))));
			float num2 = radius * num;
			float x = 1f - maxError / num2;
			return math.max(3, (int)math.ceil(3.1415927f / math.acos(x)));
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003F34C File Offset: 0x0003D54C
		[GenerateTestsForBurstCompatibility]
		public static float CircleRadiusAdjustmentFactor(int steps)
		{
			int num = steps - 3;
			if (num >= CircleGeometryUtilities.circleRadiusAdjustmentFactors.Length)
			{
				return 1f;
			}
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("Steps must be at least 3");
			}
			return CircleGeometryUtilities.circleRadiusAdjustmentFactors[num];
		}

		// Token: 0x040007A8 RID: 1960
		private static readonly float[] circleRadiusAdjustmentFactors = new float[]
		{
			1.56f,
			1.25f,
			1.15f,
			1.1f,
			1.07f,
			1.05f,
			1.04f,
			1.03f,
			1.03f,
			1.02f,
			1.02f,
			1.02f,
			1.01f,
			1.01f,
			1.01f,
			1.01f,
			1.01f,
			1.01f,
			1.01f,
			1.01f
		};
	}
}
