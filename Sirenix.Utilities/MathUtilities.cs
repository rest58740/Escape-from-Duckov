using System;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x02000028 RID: 40
	public static class MathUtilities
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x0000BB48 File Offset: 0x00009D48
		public static float PointDistanceToLine(Vector3 point, Vector3 a, Vector3 b)
		{
			return Mathf.Abs((b.x - a.x) * (a.y - point.y) - (a.x - point.x) * (b.y - a.y)) / Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2f) + Mathf.Pow(b.y - a.y, 2f));
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000BBC6 File Offset: 0x00009DC6
		public static float Hermite(float start, float end, float t)
		{
			return Mathf.Lerp(start, end, t * t * (3f - 2f * t));
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000BBE0 File Offset: 0x00009DE0
		public static float StackHermite(float start, float end, float t, int count)
		{
			for (int i = 0; i < count; i++)
			{
				t = MathUtilities.Hermite(start, end, t);
			}
			return t;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000BC04 File Offset: 0x00009E04
		public static float Fract(float value)
		{
			return value - (float)Math.Truncate((double)value);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000BC10 File Offset: 0x00009E10
		public static Vector2 Fract(Vector2 value)
		{
			return new Vector3(MathUtilities.Fract(value.x), MathUtilities.Fract(value.y));
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000BC32 File Offset: 0x00009E32
		public static Vector3 Fract(Vector3 value)
		{
			return new Vector3(MathUtilities.Fract(value.x), MathUtilities.Fract(value.y), MathUtilities.Fract(value.z));
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000BC5A File Offset: 0x00009E5A
		public static float BounceEaseInFastOut(float t)
		{
			return Mathf.Cos(t * t * 3.1415927f * 2f) * -0.5f + 0.5f;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000BC7C File Offset: 0x00009E7C
		public static float Hermite01(float t)
		{
			return Mathf.Lerp(0f, 1f, t * t * (3f - 2f * t));
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000BCA0 File Offset: 0x00009EA0
		public static float StackHermite01(float t, int count)
		{
			for (int i = 0; i < count; i++)
			{
				t = MathUtilities.Hermite01(t);
			}
			return t;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000BCC2 File Offset: 0x00009EC2
		public static Vector3 LerpUnclamped(Vector3 from, Vector3 to, float amount)
		{
			return from + (to - from) * amount;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000BCD7 File Offset: 0x00009ED7
		public static Vector2 LerpUnclamped(Vector2 from, Vector2 to, float amount)
		{
			return from + (to - from) * amount;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000BCEC File Offset: 0x00009EEC
		public static float Bounce(float value)
		{
			return Mathf.Abs(Mathf.Sin(value % 1f * 3.1415927f));
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000BD08 File Offset: 0x00009F08
		public static float EaseInElastic(float value, float amplitude = 0.25f, float length = 0.6f)
		{
			value = Mathf.Clamp01(value);
			float num = Mathf.Clamp01(value * 7.5f);
			float num2 = 1f - num * num * (3f - 2f * num);
			float num3 = Mathf.Pow(1f - Mathf.Sin(Mathf.Min(value * (1f - length), 0.5f) * 3.1415927f), 2f);
			float num4 = Mathf.Cos(3.1415927f + value * 23f) * amplitude + num2 * -(1f - amplitude);
			return 1f + num4 * num3;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000BD9A File Offset: 0x00009F9A
		public static Vector3 Pow(this Vector3 v, float p)
		{
			v.x = Mathf.Pow(v.x, p);
			v.y = Mathf.Pow(v.y, p);
			v.z = Mathf.Pow(v.z, p);
			return v;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000BDD6 File Offset: 0x00009FD6
		public static Vector3 Abs(this Vector3 v)
		{
			v.x = Mathf.Abs(v.x);
			v.y = Mathf.Abs(v.y);
			v.z = Mathf.Abs(v.z);
			return v;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000BE0F File Offset: 0x0000A00F
		public static Vector3 Sign(this Vector3 v)
		{
			return new Vector3(Mathf.Sign(v.x), Mathf.Sign(v.y), Mathf.Sign(v.z));
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000BE37 File Offset: 0x0000A037
		public static float EaseOutElastic(float value, float amplitude = 0.25f, float length = 0.6f)
		{
			return 1f - MathUtilities.EaseInElastic(1f - value, amplitude, length);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000BE4D File Offset: 0x0000A04D
		public static float EaseInOut(float t)
		{
			t = 1f - Mathf.Abs(Mathf.Clamp01(t) * 2f - 1f);
			t = t * t * (3f - 2f * t);
			return t;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000BE84 File Offset: 0x0000A084
		public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
		{
			return new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000BEDB File Offset: 0x0000A0DB
		public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
		{
			return new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000BF10 File Offset: 0x0000A110
		public static int ComputeByteArrayHash(byte[] data)
		{
			int num = -2128831035;
			for (int i = 0; i < data.Length; i++)
			{
				num = (num ^ (int)data[i]) * 16777619;
			}
			num += num << 13;
			num ^= num >> 7;
			num += num << 3;
			num ^= num >> 17;
			return num + (num << 5);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000BF60 File Offset: 0x0000A160
		public static Vector3 InterpolatePoints(Vector3[] path, float t)
		{
			t = Mathf.Clamp01(t * (1f - 1f / (float)path.Length));
			int b = path.Length - 1;
			int num = Mathf.FloorToInt(t * (float)path.Length);
			float num2 = t * (float)path.Length - (float)num;
			Vector3 a = path[Mathf.Max(0, --num)];
			Vector3 a2 = path[Mathf.Min(num + 1, b)];
			Vector3 vector = path[Mathf.Min(num + 2, b)];
			Vector3 b2 = path[Mathf.Min(num + 3, b)];
			return 0.5f * ((-a + 3f * a2 - 3f * vector + b2) * (num2 * num2 * num2) + (2f * a - 5f * a2 + 4f * vector - b2) * (num2 * num2) + (-a + vector) * num2 + 2f * a2);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000C094 File Offset: 0x0000A294
		public static bool LineIntersectsLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
		{
			intersection = Vector2.zero;
			Vector2 vector = new Vector2((b1.x < b2.x) ? b1.x : b2.x, (b1.y > b2.y) ? b1.y : b2.y);
			Vector2 vector2 = new Vector2((b1.x > b2.x) ? b1.x : b2.x, (b1.y < b2.y) ? b1.y : b2.y);
			if ((a1.x < vector.x && a2.x < vector.x) || (a1.y > vector.y && a2.y > vector.y) || (a1.x > vector2.x && a2.x > vector2.x) || (a1.y < vector2.y && a2.y < vector2.y))
			{
				return false;
			}
			Vector2 vector3 = a2 - a1;
			Vector2 vector4 = b2 - b1;
			float num = vector3.x * vector4.y - vector3.y * vector4.x;
			if (num == 0f)
			{
				return false;
			}
			Vector2 vector5 = b1 - a1;
			float num2 = (vector5.x * vector4.y - vector5.y * vector4.x) / num;
			if (num2 < 0f || num2 > 1f)
			{
				return false;
			}
			float num3 = (vector5.x * vector3.y - vector5.y * vector3.x) / num;
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			intersection = a1 + num2 * vector3;
			return true;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000C260 File Offset: 0x0000A460
		public static Vector2 InfiniteLineIntersect(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
		{
			float num = pe1.y - ps1.y;
			float num2 = ps1.x - pe1.x;
			float num3 = num * ps1.x + num2 * ps1.y;
			float num4 = pe2.y - ps2.y;
			float num5 = ps2.x - pe2.x;
			float num6 = num4 * ps2.x + num5 * ps2.y;
			float num7 = num * num5 - num4 * num2;
			if (num7 == 0f)
			{
				throw new Exception("Lines are parallel");
			}
			return new Vector2((num5 * num3 - num2 * num6) / num7, (num * num6 - num4 * num3) / num7);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000C306 File Offset: 0x0000A506
		public static float LineDistToPlane(Vector3 planeOrigin, Vector3 planeNormal, Vector3 lineOrigin, Vector3 lineDirectionNormalized)
		{
			return Vector3.Dot(lineDirectionNormalized, planeNormal) * Vector3.Distance(planeOrigin, lineOrigin);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000C318 File Offset: 0x0000A518
		public static float RayDistToPlane(Ray ray, Plane plane)
		{
			float num = Vector3.Dot(plane.normal, ray.direction);
			if (Mathf.Abs(num) < 1E-06f)
			{
				return 0f;
			}
			float num2 = Vector3.Dot(plane.normal, ray.origin);
			return (-plane.distance - num2) / num;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000C36C File Offset: 0x0000A56C
		public static Vector2 RotatePoint(Vector2 point, float degrees)
		{
			float f = degrees * 0.017453292f;
			float num = Mathf.Cos(f);
			float num2 = Mathf.Sin(f);
			return new Vector2(num * point.x - num2 * point.y, num2 * point.x + num * point.y);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000C3B8 File Offset: 0x0000A5B8
		public static Vector2 RotatePoint(Vector2 point, Vector2 around, float degrees)
		{
			float f = degrees * 0.017453292f;
			float num = Mathf.Cos(f);
			float num2 = Mathf.Sin(f);
			return new Vector2(num * (point.x - around.x) - num2 * (point.y - around.y) + around.x, num2 * (point.x - around.x) + num * (point.y - around.y) + around.y);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000C42C File Offset: 0x0000A62C
		public static float SmoothStep(float a, float b, float t)
		{
			t = Mathf.Clamp01((t - a) / (b - a));
			return t * t * (3f - 2f * t);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000C44D File Offset: 0x0000A64D
		public static float LinearStep(float a, float b, float t)
		{
			return Mathf.Clamp01((t - a) / (b - a));
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000C45C File Offset: 0x0000A65C
		public static double Wrap(double value, double min, double max)
		{
			double num = max - min;
			num = ((num < 0.0) ? (-num) : num);
			if (value < min)
			{
				return value + num * Math.Ceiling(Math.Abs(value / num));
			}
			if (value >= max)
			{
				return value - num * Math.Floor(Math.Abs(value / num));
			}
			return value;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		public static float Wrap(float value, float min, float max)
		{
			float num = max - min;
			num = (((double)num < 0.0) ? (-num) : num);
			if (value < min)
			{
				return value + num * (float)Math.Ceiling((double)Math.Abs(value / num));
			}
			if (value >= max)
			{
				return value - num * (float)Math.Floor((double)Math.Abs(value / num));
			}
			return value;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000C504 File Offset: 0x0000A704
		public static int Wrap(int value, int min, int max)
		{
			int num = max - min;
			num = ((num < 0) ? (-num) : num);
			if (value < min)
			{
				return value + num * (Math.Abs(value / num) + 1);
			}
			if (value >= max)
			{
				return value - num * Math.Abs(value / num);
			}
			return value;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000C544 File Offset: 0x0000A744
		public static double RoundBasedOnMinimumDifference(double valueToRound, double minDifference)
		{
			double result;
			if (minDifference == 0.0)
			{
				result = MathUtilities.DiscardLeastSignificantDecimal(valueToRound);
			}
			else
			{
				result = (double)((float)Math.Round(valueToRound, MathUtilities.GetNumberOfDecimalsForMinimumDifference(minDifference), 1));
			}
			return result;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000C578 File Offset: 0x0000A778
		public static double DiscardLeastSignificantDecimal(double v)
		{
			int num = Math.Max(0, (int)(5.0 - Math.Log10(Math.Abs(v))));
			double result;
			try
			{
				result = Math.Round(v, num);
			}
			catch (ArgumentOutOfRangeException)
			{
				result = 0.0;
			}
			return result;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000C5CC File Offset: 0x0000A7CC
		public static float ClampWrapAngle(float angle, float min, float max)
		{
			float num = 360f;
			float num2 = min;
			float num3 = max;
			float num4 = angle;
			if (num2 < 0f)
			{
				num2 = num2 % num + num;
			}
			if (num3 < 0f)
			{
				num3 = num3 % num + num;
			}
			if (num4 < 0f)
			{
				num4 = num4 % num + num;
			}
			float num5 = (float)((int)(Math.Abs(min - max) / num)) * num;
			num3 += num5;
			num4 += num5;
			if (min > max)
			{
				num3 += num;
			}
			if (num4 < num2)
			{
				num4 = num2;
			}
			if (num4 > num3)
			{
				num4 = num3;
			}
			return num4;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000C63E File Offset: 0x0000A83E
		private static int GetNumberOfDecimalsForMinimumDifference(double minDifference)
		{
			return Mathf.Clamp(-Mathf.FloorToInt(Mathf.Log10(Mathf.Abs((float)minDifference))), 0, 15);
		}

		// Token: 0x04000059 RID: 89
		private const float ZERO_TOLERANCE = 1E-06f;
	}
}
