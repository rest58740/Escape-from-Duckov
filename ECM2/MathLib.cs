using System;
using UnityEngine;

namespace ECM2
{
	// Token: 0x02000005 RID: 5
	public static class MathLib
	{
		// Token: 0x06000137 RID: 311 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public static float Remap(float inA, float inB, float outA, float outB, float value)
		{
			float t = Mathf.InverseLerp(inA, inB, value);
			return Mathf.Lerp(outA, outB, t);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00004CEF File Offset: 0x00002EEF
		public static float Square(float value)
		{
			return value * value;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00004CF4 File Offset: 0x00002EF4
		public static Vector3 GetTangent(Vector3 direction, Vector3 normal, Vector3 up)
		{
			Vector3 otherVector = direction.perpendicularTo(up);
			return normal.perpendicularTo(otherVector);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00004D10 File Offset: 0x00002F10
		public static Vector3 ProjectPointOnPlane(Vector3 point, Vector3 planeOrigin, Vector3 planeNormal)
		{
			Vector3 b = Vector3.Project(point - planeOrigin, planeNormal);
			return point - b;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004D34 File Offset: 0x00002F34
		public static float ClampAngle(float a, float min, float max)
		{
			while (max < min)
			{
				max += 360f;
			}
			while (a > max)
			{
				a -= 360f;
			}
			while (a < min)
			{
				a += 360f;
			}
			if (a <= max)
			{
				return a;
			}
			if (a - (max + min) * 0.5f >= 180f)
			{
				return min;
			}
			return max;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004D89 File Offset: 0x00002F89
		public static float ClampAngle(float angle)
		{
			angle %= 360f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			return angle;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004DA6 File Offset: 0x00002FA6
		public static float NormalizeAngle(float angle)
		{
			angle = MathLib.ClampAngle(angle);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			return angle;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00004DC4 File Offset: 0x00002FC4
		private static float Clamp0360(float eulerAngles)
		{
			float num = eulerAngles - (float)Mathf.CeilToInt(eulerAngles / 360f) * 360f;
			if (num < 0f)
			{
				num += 360f;
			}
			return num;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00004DF8 File Offset: 0x00002FF8
		public static float FixedTurn(float current, float target, float maxDegreesDelta)
		{
			if (maxDegreesDelta == 0f)
			{
				return MathLib.Clamp0360(current);
			}
			if (maxDegreesDelta >= 360f)
			{
				return MathLib.Clamp0360(target);
			}
			float num = MathLib.Clamp0360(current);
			current = num;
			target = MathLib.Clamp0360(target);
			if (current > target)
			{
				if (current - target < 180f)
				{
					num -= Mathf.Min(current - target, Mathf.Abs(maxDegreesDelta));
				}
				else
				{
					num += Mathf.Min(target + 360f - current, Mathf.Abs(maxDegreesDelta));
				}
			}
			else if (target - current < 180f)
			{
				num += Mathf.Min(target - current, Mathf.Abs(maxDegreesDelta));
			}
			else
			{
				num -= Mathf.Min(current + 360f - target, Mathf.Abs(maxDegreesDelta));
			}
			return MathLib.Clamp0360(num);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00004EA9 File Offset: 0x000030A9
		public static float Damp(float a, float b, float lambda, float dt)
		{
			return Mathf.Lerp(a, b, 1f - Mathf.Exp(-lambda * dt));
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00004EC1 File Offset: 0x000030C1
		public static Vector3 Damp(Vector3 a, Vector3 b, float lambda, float dt)
		{
			return Vector3.Lerp(a, b, 1f - Mathf.Exp(-lambda * dt));
		}
	}
}
