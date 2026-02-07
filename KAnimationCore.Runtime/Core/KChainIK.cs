using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x02000017 RID: 23
	public class KChainIK
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public static bool SolveFABRIK(ref ChainIKData ikData)
		{
			Vector3 vector = ikData.target - ikData.positions[0];
			if (vector.sqrMagnitude > KMath.Square(ikData.maxReach))
			{
				Vector3 normalized = vector.normalized;
				for (int i = 1; i < ikData.positions.Length; i++)
				{
					ikData.positions[i] = ikData.positions[i - 1] + normalized * ikData.lengths[i - 1];
				}
				return true;
			}
			int num = ikData.positions.Length - 1;
			float num2 = KMath.Square(ikData.tolerance);
			if (KMath.SqrDistance(ikData.positions[num], ikData.target) > num2)
			{
				Vector3 vector2 = ikData.positions[0];
				int num3 = 0;
				do
				{
					ikData.positions[num] = ikData.target;
					for (int j = num - 1; j > -1; j--)
					{
						ikData.positions[j] = ikData.positions[j + 1] + (ikData.positions[j] - ikData.positions[j + 1]).normalized * ikData.lengths[j];
					}
					ikData.positions[0] = vector2;
					for (int k = 1; k < ikData.positions.Length; k++)
					{
						ikData.positions[k] = ikData.positions[k - 1] + (ikData.positions[k] - ikData.positions[k - 1]).normalized * ikData.lengths[k - 1];
					}
				}
				while (KMath.SqrDistance(ikData.positions[num], ikData.target) > num2 && ++num3 < ikData.maxIterations);
				return true;
			}
			return false;
		}
	}
}
