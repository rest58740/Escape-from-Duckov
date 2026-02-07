using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x0200001C RID: 28
	public class KMath
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003281 File Offset: 0x00001481
		public static float Square(float value)
		{
			return value * value;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003288 File Offset: 0x00001488
		public static float SqrDistance(Vector3 a, Vector3 b)
		{
			return (b - a).sqrMagnitude;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000032A4 File Offset: 0x000014A4
		public static float NormalizeEulerAngle(float angle)
		{
			while (angle < -180f)
			{
				angle += 360f;
			}
			while (angle >= 180f)
			{
				angle -= 360f;
			}
			return angle;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000032CD File Offset: 0x000014CD
		public static float TriangleAngle(float aLen, float aLen1, float aLen2)
		{
			return Mathf.Acos(Mathf.Clamp((aLen1 * aLen1 + aLen2 * aLen2 - aLen * aLen) / (aLen1 * aLen2) / 2f, -1f, 1f));
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000032F8 File Offset: 0x000014F8
		public static Quaternion FromToRotation(Vector3 from, Vector3 to)
		{
			float num = Vector3.Dot(from.normalized, to.normalized);
			if (num >= 1f)
			{
				return Quaternion.identity;
			}
			if (num <= -1f)
			{
				Vector3 axis = Vector3.Cross(from, Vector3.right);
				if (axis.sqrMagnitude == 0f)
				{
					axis = Vector3.Cross(from, Vector3.up);
				}
				return Quaternion.AngleAxis(180f, axis);
			}
			return Quaternion.AngleAxis(Mathf.Acos(num) * 57.29578f, Vector3.Cross(from, to).normalized);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003384 File Offset: 0x00001584
		public static Quaternion NormalizeSafe(Quaternion q)
		{
			float num = Quaternion.Dot(q, q);
			if (num > 1E-10f)
			{
				float num2 = 1f / Mathf.Sqrt(num);
				return new Quaternion(q.x * num2, q.y * num2, q.z * num2, q.w * num2);
			}
			return Quaternion.identity;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000033DC File Offset: 0x000015DC
		public static float InvLerp(float value, float a, float b)
		{
			float value2 = 0f;
			if (!Mathf.Approximately(a, b))
			{
				value2 = (value - a) / (b - a);
			}
			return Mathf.Clamp01(value2);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003406 File Offset: 0x00001606
		public static float ExpDecayAlpha(float speed, float deltaTime)
		{
			return 1f - Mathf.Exp(-speed * deltaTime);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003418 File Offset: 0x00001618
		public static Vector2 ComputeLookAtInput(Transform root, Transform from, Transform to)
		{
			Vector2 zero = Vector2.zero;
			Quaternion rhs = Quaternion.LookRotation(to.position - from.position);
			Vector3 eulerAngles = (Quaternion.Inverse(root.rotation) * rhs).eulerAngles;
			zero.x = KMath.NormalizeEulerAngle(eulerAngles.x);
			zero.y = KMath.NormalizeEulerAngle(eulerAngles.y);
			return zero;
		}

		// Token: 0x04000041 RID: 65
		public const float FloatMin = 1E-10f;

		// Token: 0x04000042 RID: 66
		public const float SqrEpsilon = 1E-08f;
	}
}
