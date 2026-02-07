using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000077 RID: 119
	public static class ShapesMath
	{
		// Token: 0x06000CD3 RID: 3283 RVA: 0x0001A8FA File Offset: 0x00018AFA
		[MethodImpl(256)]
		public static float Frac(float x)
		{
			return x - Mathf.Floor(x);
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0001A904 File Offset: 0x00018B04
		[MethodImpl(256)]
		public static float Eerp(float a, float b, float t)
		{
			return Mathf.Pow(a, 1f - t) * Mathf.Pow(b, t);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0001A91B File Offset: 0x00018B1B
		[MethodImpl(256)]
		public static float SmoothCos01(float x)
		{
			return Mathf.Cos(x * 3.1415927f) * -0.5f + 0.5f;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0001A935 File Offset: 0x00018B35
		[MethodImpl(256)]
		public static Vector2 AngToDir(float angRad)
		{
			return new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0001A948 File Offset: 0x00018B48
		[MethodImpl(256)]
		public static float DirToAng(Vector2 dir)
		{
			return Mathf.Atan2(dir.y, dir.x);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0001A95B File Offset: 0x00018B5B
		[MethodImpl(256)]
		public static Vector2 Rotate90CW(Vector2 v)
		{
			return new Vector2(v.y, -v.x);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0001A96F File Offset: 0x00018B6F
		[MethodImpl(256)]
		public static Vector2 Rotate90CCW(Vector2 v)
		{
			return new Vector2(-v.y, v.x);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0001A984 File Offset: 0x00018B84
		[MethodImpl(256)]
		public static Vector4 AtLeast0(Vector4 v)
		{
			return new Vector4(Mathf.Max(0f, v.x), Mathf.Max(0f, v.y), Mathf.Max(0f, v.z), Mathf.Max(0f, v.w));
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0001A9D6 File Offset: 0x00018BD6
		[MethodImpl(256)]
		public static float MaxComp(Vector4 v)
		{
			return Mathf.Max(Mathf.Max(Mathf.Max(v.y, v.x), v.z), v.w);
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0001A9FF File Offset: 0x00018BFF
		[MethodImpl(256)]
		public static bool HasNegativeValues(Vector4 v)
		{
			return v.x < 0f || v.y < 0f || v.z < 0f || v.w < 0f;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0001AA37 File Offset: 0x00018C37
		[MethodImpl(256)]
		public static float Determinant(Vector2 a, Vector2 b)
		{
			return a.x * b.y - a.y * b.x;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0001AA54 File Offset: 0x00018C54
		[MethodImpl(256)]
		public static float Luminance(Color c)
		{
			return c.r * 0.2126f + c.g * 0.7152f + c.b * 0.0722f;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0001AA7C File Offset: 0x00018C7C
		public static float GetLineSegmentProjectionT(Vector3 a, Vector3 b, Vector3 p)
		{
			Vector3 vector = b - a;
			return Vector3.Dot(p - a, vector) / Vector3.Dot(vector, vector);
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0001AAA8 File Offset: 0x00018CA8
		[MethodImpl(256)]
		public static PolylinePoint WeightedSum(Vector4 w, PolylinePoint a, PolylinePoint b, PolylinePoint c, PolylinePoint d)
		{
			return w.x * a + w.y * b + w.z * c + w.w * d;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0001AAF8 File Offset: 0x00018CF8
		[MethodImpl(256)]
		public static Vector3 WeightedSum(Vector4 w, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			return w.x * a + w.y * b + w.z * c + w.w * d;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0001AB48 File Offset: 0x00018D48
		[MethodImpl(256)]
		public static Vector2 WeightedSum(Vector4 w, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
		{
			return w.x * a + w.y * b + w.z * c + w.w * d;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0001AB98 File Offset: 0x00018D98
		[MethodImpl(256)]
		public static Color WeightedSum(Vector4 w, Color a, Color b, Color c, Color d)
		{
			return w.x * a + w.y * b + w.z * c + w.w * d;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0001ABE8 File Offset: 0x00018DE8
		public static bool PointInsideTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 point, float aMargin = 0f, float bMargin = 0f, float cMargin = 0f)
		{
			float num = ShapesMath.Determinant(ShapesMath.Dir(a, b), ShapesMath.Dir(a, point));
			float num2 = ShapesMath.Determinant(ShapesMath.Dir(b, c), ShapesMath.Dir(b, point));
			float num3 = ShapesMath.Determinant(ShapesMath.Dir(c, a), ShapesMath.Dir(c, point));
			bool flag = num < cMargin;
			bool flag2 = num2 < aMargin;
			bool flag3 = num3 < bMargin;
			return flag == flag2 && flag2 == flag3;
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0001AC4C File Offset: 0x00018E4C
		[MethodImpl(256)]
		internal static Vector2 Dir(Vector2 a, Vector2 b)
		{
			float num = b.x - a.x;
			float num2 = b.y - a.y;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			return new Vector2(num / num3, num2 / num3);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0001AC8C File Offset: 0x00018E8C
		public static float PolygonSignedArea(List<Vector2> pts)
		{
			int count = pts.Count;
			float num = 0f;
			for (int i = 0; i < count; i++)
			{
				Vector2 vector = pts[i];
				Vector2 vector2 = pts[(i + 1) % count];
				num += (vector2.x - vector.x) * (vector2.y + vector.y);
			}
			return num;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0001ACE8 File Offset: 0x00018EE8
		public static Vector2 Rotate(Vector2 v, float angRad)
		{
			float num = Mathf.Cos(angRad);
			float num2 = Mathf.Sin(angRad);
			return new Vector2(num * v.x - num2 * v.y, num2 * v.x + num * v.y);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0001AD2A File Offset: 0x00018F2A
		private static float DeltaAngleRad(float a, float b)
		{
			return Mathf.Repeat(b - a + 3.1415927f, 6.2831855f) - 3.1415927f;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0001AD48 File Offset: 0x00018F48
		public static float InverseLerpAngleRad(float a, float b, float v)
		{
			float num = ShapesMath.DeltaAngleRad(a, b);
			b = a + num;
			float num2 = a + num * 0.5f;
			v = num2 + ShapesMath.DeltaAngleRad(num2, v);
			return Mathf.InverseLerp(a, b, v);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0001AD7D File Offset: 0x00018F7D
		[MethodImpl(256)]
		private static Vector2 Lerp(Vector2 a, Vector2 b, Vector2 t)
		{
			return new Vector2(Mathf.Lerp(a.x, b.x, t.x), Mathf.Lerp(a.y, b.y, t.y));
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0001ADB2 File Offset: 0x00018FB2
		[MethodImpl(256)]
		public static Vector2 Lerp(Rect r, Vector2 t)
		{
			return new Vector2(Mathf.Lerp(r.xMin, r.xMax, t.x), Mathf.Lerp(r.yMin, r.yMax, t.y));
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0001ADEB File Offset: 0x00018FEB
		[MethodImpl(256)]
		private static Vector2 InverseLerp(Vector2 a, Vector2 b, Vector2 v)
		{
			return (v - a) / (b - a);
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0001AE00 File Offset: 0x00019000
		[MethodImpl(256)]
		public static Vector2 InverseLerp(Rect r, Vector2 pt)
		{
			return new Vector2(Mathf.InverseLerp(r.xMin, r.xMax, pt.x), Mathf.InverseLerp(r.yMin, r.yMax, pt.y));
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0001AE39 File Offset: 0x00019039
		[MethodImpl(256)]
		private static Vector2 Remap(Vector2 iMin, Vector2 iMax, Vector2 oMin, Vector2 oMax, Vector2 value)
		{
			return ShapesMath.Lerp(oMin, oMax, ShapesMath.InverseLerp(iMin, iMax, value));
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0001AE4B File Offset: 0x0001904B
		[MethodImpl(256)]
		public static Vector2 Remap(Rect iRect, Rect oRect, Vector2 iPos)
		{
			return ShapesMath.Remap(iRect.min, iRect.max, oRect.min, oRect.max, iPos);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0001AE6F File Offset: 0x0001906F
		public static Vector3 Abs(Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0001AE98 File Offset: 0x00019098
		public static float RandomGaussian(float min = 0f, float max = 1f)
		{
			float num;
			float num3;
			do
			{
				num = 2f * Random.value - 1f;
				float num2 = 2f * Random.value - 1f;
				num3 = num * num + num2 * num2;
			}
			while (num3 >= 1f);
			float num4 = num * Mathf.Sqrt(-2f * Mathf.Log(num3) / num3);
			float num5 = (min + max) / 2f;
			float num6 = (max - num5) / 3f;
			return Mathf.Clamp(num4 * num6 + num5, min, max);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0001AF10 File Offset: 0x00019110
		public static Vector3 GetRandomPerpendicularVector(Vector3 a)
		{
			Vector3 onUnitSphere;
			do
			{
				onUnitSphere = Random.onUnitSphere;
			}
			while (Mathf.Abs(Vector3.Dot(a, onUnitSphere)) > 0.98f);
			return onUnitSphere;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0001AF37 File Offset: 0x00019137
		public static IEnumerable<PolylinePoint> GetArcPoints(PolylinePoint a, PolylinePoint b, Vector3 normA, Vector3 normB, Vector3 center, float radius, int count)
		{
			ShapesMath.<>c__DisplayClass35_0 CS$<>8__locals1;
			CS$<>8__locals1.a = a;
			CS$<>8__locals1.b = b;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.radius = radius;
			count = Mathf.Max(2, count);
			yield return ShapesMath.<GetArcPoints>g__DirToPt|35_0(normA, 0f, ref CS$<>8__locals1);
			int num;
			for (int i = 1; i < count - 1; i = num + 1)
			{
				float t = (float)i / ((float)count - 1f);
				yield return ShapesMath.<GetArcPoints>g__DirToPt|35_0(Vector3.Slerp(normA, normB, t), t, ref CS$<>8__locals1);
				num = i;
			}
			yield return ShapesMath.<GetArcPoints>g__DirToPt|35_0(normB, 1f, ref CS$<>8__locals1);
			yield break;
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0001AF74 File Offset: 0x00019174
		public static IEnumerable<Vector3> GetArcPoints(Vector3 normA, Vector3 normB, Vector3 center, float radius, int count)
		{
			ShapesMath.<>c__DisplayClass36_0 CS$<>8__locals1;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.radius = radius;
			count = Mathf.Max(2, count);
			yield return ShapesMath.<GetArcPoints>g__DirToPt|36_0(normA, ref CS$<>8__locals1);
			int num;
			for (int i = 1; i < count - 1; i = num + 1)
			{
				float t = (float)i / ((float)count - 1f);
				yield return ShapesMath.<GetArcPoints>g__DirToPt|36_0(Vector3.Slerp(normA, normB, t), ref CS$<>8__locals1);
				num = i;
			}
			yield return ShapesMath.<GetArcPoints>g__DirToPt|36_0(normB, ref CS$<>8__locals1);
			yield break;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0001AFA1 File Offset: 0x000191A1
		public static IEnumerable<Vector2> GetArcPoints(Vector2 normA, Vector2 normB, Vector2 center, float radius, int count)
		{
			ShapesMath.<>c__DisplayClass37_0 CS$<>8__locals1;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.radius = radius;
			count = Mathf.Max(2, count);
			yield return ShapesMath.<GetArcPoints>g__DirToPt|37_0(normA, ref CS$<>8__locals1);
			int num;
			for (int i = 1; i < count - 1; i = num + 1)
			{
				float t = (float)i / ((float)count - 1f);
				yield return ShapesMath.<GetArcPoints>g__DirToPt|37_0(Vector3.Slerp(normA, normB, t), ref CS$<>8__locals1);
				num = i;
			}
			yield return ShapesMath.<GetArcPoints>g__DirToPt|37_0(normB, ref CS$<>8__locals1);
			yield break;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0001AFCE File Offset: 0x000191CE
		public static IEnumerable<PolylinePoint> CubicBezierPointsSkipFirst(PolylinePoint a, PolylinePoint b, PolylinePoint c, PolylinePoint d, int count)
		{
			int num;
			for (int i = 1; i < count - 1; i = num + 1)
			{
				float t = (float)i / ((float)count - 1f);
				yield return ShapesMath.CubicBezier(a, b, c, d, t);
				num = i;
			}
			yield return d;
			yield break;
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0001AFFB File Offset: 0x000191FB
		public static IEnumerable<PolylinePoint> CubicBezierPointsSkipFirstMatchStyle(PolylinePoint style, Vector3 a, Vector3 b, Vector3 c, Vector3 d, int count)
		{
			int num;
			for (int i = 1; i < count - 1; i = num + 1)
			{
				float t = (float)i / ((float)count - 1f);
				PolylinePoint polylinePoint = style;
				polylinePoint.point = ShapesMath.CubicBezier(a, b, c, d, t);
				yield return polylinePoint;
				num = i;
			}
			PolylinePoint polylinePoint2 = style;
			polylinePoint2.point = d;
			yield return polylinePoint2;
			yield break;
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0001B030 File Offset: 0x00019230
		public static IEnumerable<Vector3> CubicBezierPointsSkipFirst(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int count)
		{
			int num;
			for (int i = 1; i < count - 1; i = num + 1)
			{
				float t = (float)i / ((float)count - 1f);
				yield return ShapesMath.CubicBezier(a, b, c, d, t);
				num = i;
			}
			yield return d;
			yield break;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0001B05D File Offset: 0x0001925D
		public static IEnumerable<Vector2> CubicBezierPointsSkipFirst(Vector2 a, Vector2 b, Vector2 c, Vector2 d, int count)
		{
			int num;
			for (int i = 1; i < count - 1; i = num + 1)
			{
				float t = (float)i / ((float)count - 1f);
				yield return ShapesMath.CubicBezier(a, b, c, d, t);
				num = i;
			}
			yield return d;
			yield break;
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0001B08C File Offset: 0x0001928C
		public static Vector4 GetCubicBezierWeights(float t)
		{
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return new Vector4(num2 * num, 3f * num2 * t, 3f * num * num3, num3 * t);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0001B0C6 File Offset: 0x000192C6
		public static PolylinePoint CubicBezier(PolylinePoint a, PolylinePoint b, PolylinePoint c, PolylinePoint d, float t)
		{
			if (t <= 0f)
			{
				return a;
			}
			if (t >= 1f)
			{
				return d;
			}
			return ShapesMath.WeightedSum(ShapesMath.GetCubicBezierWeights(t), a, b, c, d);
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0001B0EE File Offset: 0x000192EE
		public static Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
		{
			if (t <= 0f)
			{
				return a;
			}
			if (t >= 1f)
			{
				return d;
			}
			return ShapesMath.WeightedSum(ShapesMath.GetCubicBezierWeights(t), a, b, c, d);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0001B116 File Offset: 0x00019316
		public static Vector2 CubicBezier(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
		{
			if (t <= 0f)
			{
				return a;
			}
			if (t >= 1f)
			{
				return d;
			}
			return ShapesMath.WeightedSum(ShapesMath.GetCubicBezierWeights(t), a, b, c, d);
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0001B140 File Offset: 0x00019340
		private static Vector3 CubicBezierDirectionIsh(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
		{
			float num = 1f - t;
			float num2 = t * t;
			float num3 = 3f * num2;
			float num4 = -num * num;
			float num5 = num3 - 4f * t + 1f;
			float num6 = 2f * t - num3;
			float num7 = num2;
			return new Vector3(a.x * num4 + b.x * num5 + c.x * num6 + d.x * num7, a.y * num4 + b.y * num5 + c.y * num6 + d.y * num7, a.z * num4 + b.z * num5 + c.z * num6 + d.z * num7);
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0001B204 File Offset: 0x00019404
		public static float GetApproximateAngularCurveSumDegrees(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int vertCount)
		{
			float num = 0f;
			Vector3 from = b - a;
			for (int i = 1; i < vertCount - 1; i++)
			{
				float t = (float)i / ((float)vertCount - 1f);
				Vector3 vector = ShapesMath.CubicBezierDirectionIsh(a, b, c, d, t);
				num += Vector3.Angle(from, vector);
				from = vector;
			}
			Vector3 to = d - c;
			return num + Vector3.Angle(from, to);
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0001B26C File Offset: 0x0001946C
		public static Matrix4x4 AffineMtxMul(Matrix4x4 lhs, Matrix4x4 rhs)
		{
			Matrix4x4 result;
			result.m00 = (float)((double)lhs.m00 * (double)rhs.m00 + (double)lhs.m01 * (double)rhs.m10 + (double)lhs.m02 * (double)rhs.m20);
			result.m01 = (float)((double)lhs.m00 * (double)rhs.m01 + (double)lhs.m01 * (double)rhs.m11 + (double)lhs.m02 * (double)rhs.m21);
			result.m02 = (float)((double)lhs.m00 * (double)rhs.m02 + (double)lhs.m01 * (double)rhs.m12 + (double)lhs.m02 * (double)rhs.m22);
			result.m03 = (float)((double)lhs.m00 * (double)rhs.m03 + (double)lhs.m01 * (double)rhs.m13 + (double)lhs.m02 * (double)rhs.m23 + (double)lhs.m03);
			result.m10 = (float)((double)lhs.m10 * (double)rhs.m00 + (double)lhs.m11 * (double)rhs.m10 + (double)lhs.m12 * (double)rhs.m20);
			result.m11 = (float)((double)lhs.m10 * (double)rhs.m01 + (double)lhs.m11 * (double)rhs.m11 + (double)lhs.m12 * (double)rhs.m21);
			result.m12 = (float)((double)lhs.m10 * (double)rhs.m02 + (double)lhs.m11 * (double)rhs.m12 + (double)lhs.m12 * (double)rhs.m22);
			result.m13 = (float)((double)lhs.m10 * (double)rhs.m03 + (double)lhs.m11 * (double)rhs.m13 + (double)lhs.m12 * (double)rhs.m23 + (double)lhs.m13);
			result.m20 = (float)((double)lhs.m20 * (double)rhs.m00 + (double)lhs.m21 * (double)rhs.m10 + (double)lhs.m22 * (double)rhs.m20);
			result.m21 = (float)((double)lhs.m20 * (double)rhs.m01 + (double)lhs.m21 * (double)rhs.m11 + (double)lhs.m22 * (double)rhs.m21);
			result.m22 = (float)((double)lhs.m20 * (double)rhs.m02 + (double)lhs.m21 * (double)rhs.m12 + (double)lhs.m22 * (double)rhs.m22);
			result.m23 = (float)((double)lhs.m20 * (double)rhs.m03 + (double)lhs.m21 * (double)rhs.m13 + (double)lhs.m22 * (double)rhs.m23 + (double)lhs.m23);
			result.m30 = 0f;
			result.m31 = 0f;
			result.m32 = 0f;
			result.m33 = 1f;
			return result;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0001B556 File Offset: 0x00019756
		public static float Cosinc(float x)
		{
			return (float)ShapesMath.Cosinc((double)x);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0001B560 File Offset: 0x00019760
		public static double Cosinc(double x)
		{
			if (Math.Abs(x) < 0.01)
			{
				return x / 2.0 - x * x * x / 24.0;
			}
			return (1.0 - Math.Cos(x)) / x;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0001B5AC File Offset: 0x000197AC
		public static float Sinc(float x)
		{
			return (float)ShapesMath.Sinc((double)x);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0001B5B8 File Offset: 0x000197B8
		public static double Sinc(double x)
		{
			x = Math.Abs(x);
			if (x < 0.01)
			{
				double num = x * x;
				double num2 = num * num;
				return 1.0 + -0.16666666666666666 * num + 0.008333333333333333 * num2;
			}
			return Math.Sin(x) / x;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0001B60C File Offset: 0x0001980C
		[CompilerGenerated]
		internal static PolylinePoint <GetArcPoints>g__DirToPt|35_0(Vector3 dir, float t, ref ShapesMath.<>c__DisplayClass35_0 A_2)
		{
			PolylinePoint result = (t <= 0f) ? A_2.a : ((t >= 1f) ? A_2.b : PolylinePoint.Lerp(A_2.a, A_2.b, t));
			result.point = A_2.center + dir * A_2.radius;
			return result;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0001B66B File Offset: 0x0001986B
		[CompilerGenerated]
		internal static Vector3 <GetArcPoints>g__DirToPt|36_0(Vector3 dir, ref ShapesMath.<>c__DisplayClass36_0 A_1)
		{
			return A_1.center + dir * A_1.radius;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0001B684 File Offset: 0x00019884
		[CompilerGenerated]
		internal static Vector2 <GetArcPoints>g__DirToPt|37_0(Vector2 dir, ref ShapesMath.<>c__DisplayClass37_0 A_1)
		{
			return A_1.center + dir * A_1.radius;
		}

		// Token: 0x040002EE RID: 750
		private const MethodImplOptions INLINE = 256;

		// Token: 0x040002EF RID: 751
		public const float TAU = 6.2831855f;

		// Token: 0x040002F0 RID: 752
		public const double DEG_TO_RAD = 0.017453292519943295;

		// Token: 0x040002F1 RID: 753
		private const double SINC_W = 0.01;

		// Token: 0x040002F2 RID: 754
		private const double SINC_P_C2 = -0.16666666666666666;

		// Token: 0x040002F3 RID: 755
		private const double SINC_P_C4 = 0.008333333333333333;
	}
}
