using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x0200001E RID: 30
	public class KSpringMath
	{
		// Token: 0x0600004F RID: 79 RVA: 0x000034B0 File Offset: 0x000016B0
		public static float FloatSpringInterp(float current, float target, float speed, float criticalDamping, float stiffness, float scale, ref FloatSpringState state)
		{
			float num = Mathf.Min(Time.deltaTime * speed, 1f);
			if (!Mathf.Approximately(num, 0f))
			{
				float num2 = 2f * Mathf.Sqrt(stiffness) * criticalDamping;
				float num3 = target * scale - current;
				float num4 = num3 - state.Error;
				state.Velocity += num3 * stiffness * num + num4 * num2;
				state.Error = num3;
				return current + state.Velocity * num;
			}
			return current;
		}
	}
}
