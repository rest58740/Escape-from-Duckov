using System;
using UnityEngine;

namespace GameMaths
{
	// Token: 0x02000012 RID: 18
	public class GameMaths
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003314 File Offset: 0x00001514
		public static Vector3 CalculateSniperTargetPoint(Vector3 sniperPoint, Vector3 targetCurrentPos, float bulletSpeed, Vector3 targetVelocity)
		{
			Vector3 normalized = targetVelocity.normalized;
			float num = Vector3.Angle((sniperPoint - targetCurrentPos).normalized, normalized);
			float magnitude = targetVelocity.magnitude;
			float num2 = Mathf.Asin(Mathf.Sin(num * 0.017453292f) * magnitude / bulletSpeed);
			float num3 = 180f - num2 * 57.29578f - num;
			float d = Mathf.Abs(Vector3.Distance(sniperPoint, targetCurrentPos) * Mathf.Sin(num * 0.017453292f) / Mathf.Sin(num3 * 0.017453292f) / bulletSpeed);
			return targetCurrentPos + normalized * d * magnitude;
		}
	}
}
