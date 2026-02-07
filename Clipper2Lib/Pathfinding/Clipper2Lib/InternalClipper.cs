using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000011 RID: 17
	public static class InternalClipper
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00002ACC File Offset: 0x00000CCC
		public static double CrossProduct(Point64 pt1, Point64 pt2, Point64 pt3)
		{
			return (double)(pt2.X - pt1.X) * (double)(pt3.Y - pt2.Y) - (double)(pt2.Y - pt1.Y) * (double)(pt3.X - pt2.X);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B09 File Offset: 0x00000D09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void CheckPrecision(int precision)
		{
			if (precision < -8 || precision > 8)
			{
				throw new Exception(InternalClipper.precision_range_error);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002B1F File Offset: 0x00000D1F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsAlmostZero(double value)
		{
			return Math.Abs(value) <= 1E-12;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B35 File Offset: 0x00000D35
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int TriSign(long x)
		{
			if (x < 0L)
			{
				return -1;
			}
			if (x > 1L)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002B48 File Offset: 0x00000D48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static InternalClipper.MultiplyUInt64Result MultiplyUInt64(ulong a, ulong b)
		{
			ulong num = (a & (ulong)-1) * (b & (ulong)-1);
			ulong num2 = (a >> 32) * (b & (ulong)-1) + (num >> 32);
			ulong num3 = (a & (ulong)-1) * (b >> 32) + (num2 & (ulong)-1);
			InternalClipper.MultiplyUInt64Result result;
			result.lo64 = ((num3 & (ulong)-1) << 32 | (num & (ulong)-1));
			result.hi64 = (a >> 32) * (b >> 32) + (num2 >> 32) + (num3 >> 32);
			return result;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002BAC File Offset: 0x00000DAC
		internal static bool ProductsAreEqual(long a, long b, long c, long d)
		{
			ulong a2 = (ulong)Math.Abs(a);
			ulong b2 = (ulong)Math.Abs(b);
			ulong a3 = (ulong)Math.Abs(c);
			ulong b3 = (ulong)Math.Abs(d);
			InternalClipper.MultiplyUInt64Result multiplyUInt64Result = InternalClipper.MultiplyUInt64(a2, b2);
			InternalClipper.MultiplyUInt64Result multiplyUInt64Result2 = InternalClipper.MultiplyUInt64(a3, b3);
			int num = InternalClipper.TriSign(a) * InternalClipper.TriSign(b);
			int num2 = InternalClipper.TriSign(c) * InternalClipper.TriSign(d);
			return multiplyUInt64Result.lo64 == multiplyUInt64Result2.lo64 && multiplyUInt64Result.hi64 == multiplyUInt64Result2.hi64 && num == num2;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002C28 File Offset: 0x00000E28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsCollinear(Point64 pt1, Point64 sharedPt, Point64 pt2)
		{
			long a = sharedPt.X - pt1.X;
			long b = pt2.Y - sharedPt.Y;
			long c = sharedPt.Y - pt1.Y;
			long d = pt2.X - sharedPt.X;
			return InternalClipper.ProductsAreEqual(a, b, c, d);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C74 File Offset: 0x00000E74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double DotProduct(Point64 pt1, Point64 pt2, Point64 pt3)
		{
			return (double)(pt2.X - pt1.X) * (double)(pt3.X - pt2.X) + (double)(pt2.Y - pt1.Y) * (double)(pt3.Y - pt2.Y);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002CB1 File Offset: 0x00000EB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double CrossProduct(PointD vec1, PointD vec2)
		{
			return vec1.y * vec2.x - vec2.y * vec1.x;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002CCE File Offset: 0x00000ECE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double DotProduct(PointD vec1, PointD vec2)
		{
			return vec1.x * vec2.x + vec1.y * vec2.y;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002CEB File Offset: 0x00000EEB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static long CheckCastInt64(double val)
		{
			if (val >= 2.305843009213694E+18 || val <= -2.305843009213694E+18)
			{
				return long.MaxValue;
			}
			return (long)Math.Round(val, MidpointRounding.AwayFromZero);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002D18 File Offset: 0x00000F18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GetSegmentIntersectPt(Point64 ln1a, Point64 ln1b, Point64 ln2a, Point64 ln2b, out Point64 ip)
		{
			double num = (double)(ln1b.Y - ln1a.Y);
			double num2 = (double)(ln1b.X - ln1a.X);
			double num3 = (double)(ln2b.Y - ln2a.Y);
			double num4 = (double)(ln2b.X - ln2a.X);
			double num5 = num * num4 - num3 * num2;
			if (num5 == 0.0)
			{
				ip = default(Point64);
				return false;
			}
			double num6 = ((double)(ln1a.X - ln2a.X) * num3 - (double)(ln1a.Y - ln2a.Y) * num4) / num5;
			if (num6 <= 0.0)
			{
				ip = ln1a;
			}
			else if (num6 >= 1.0)
			{
				ip = ln1b;
			}
			else
			{
				ip.X = (long)((double)ln1a.X + num6 * num2);
				ip.Y = (long)((double)ln1a.Y + num6 * num);
			}
			return true;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002E00 File Offset: 0x00001000
		internal static bool SegsIntersect(Point64 seg1a, Point64 seg1b, Point64 seg2a, Point64 seg2b, bool inclusive = false)
		{
			if (!inclusive)
			{
				return InternalClipper.CrossProduct(seg1a, seg2a, seg2b) * InternalClipper.CrossProduct(seg1b, seg2a, seg2b) < 0.0 && InternalClipper.CrossProduct(seg2a, seg1a, seg1b) * InternalClipper.CrossProduct(seg2b, seg1a, seg1b) < 0.0;
			}
			double num = InternalClipper.CrossProduct(seg1a, seg2a, seg2b);
			double num2 = InternalClipper.CrossProduct(seg1b, seg2a, seg2b);
			if (num * num2 > 0.0)
			{
				return false;
			}
			double num3 = InternalClipper.CrossProduct(seg2a, seg1a, seg1b);
			double num4 = InternalClipper.CrossProduct(seg2b, seg1a, seg1b);
			return num3 * num4 <= 0.0 && (num != 0.0 || num2 != 0.0 || num3 != 0.0 || num4 != 0.0);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002EC8 File Offset: 0x000010C8
		public static Point64 GetClosestPtOnSegment(Point64 offPt, Point64 seg1, Point64 seg2)
		{
			if (seg1.X == seg2.X && seg1.Y == seg2.Y)
			{
				return seg1;
			}
			double num = (double)(seg2.X - seg1.X);
			double num2 = (double)(seg2.Y - seg1.Y);
			double num3 = ((double)(offPt.X - seg1.X) * num + (double)(offPt.Y - seg1.Y) * num2) / (num * num + num2 * num2);
			if (num3 < 0.0)
			{
				num3 = 0.0;
			}
			else if (num3 > 1.0)
			{
				num3 = 1.0;
			}
			return new Point64((double)seg1.X + Math.Round(num3 * num, MidpointRounding.ToEven), (double)seg1.Y + Math.Round(num3 * num2, MidpointRounding.ToEven));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002F90 File Offset: 0x00001190
		[NullableContext(1)]
		public static PointInPolygonResult PointInPolygon(Point64 pt, List<Point64> polygon)
		{
			int count = polygon.Count;
			int num = 0;
			if (count < 3)
			{
				return PointInPolygonResult.IsOutside;
			}
			while (num < count && polygon[num].Y == pt.Y)
			{
				num++;
			}
			if (num == count)
			{
				return PointInPolygonResult.IsOutside;
			}
			bool flag = polygon[num].Y < pt.Y;
			bool flag2 = flag;
			int num2 = 0;
			int num3 = num + 1;
			int num4 = count;
			for (;;)
			{
				if (num3 == num4)
				{
					if (num4 == 0 || num == 0)
					{
						goto IL_1CF;
					}
					num4 = num;
					num3 = 0;
				}
				if (flag)
				{
					while (num3 < num4 && polygon[num3].Y < pt.Y)
					{
						num3++;
					}
					if (num3 == num4)
					{
						continue;
					}
				}
				else
				{
					while (num3 < num4 && polygon[num3].Y > pt.Y)
					{
						num3++;
					}
					if (num3 == num4)
					{
						continue;
					}
				}
				Point64 point = polygon[num3];
				Point64 point2;
				if (num3 > 0)
				{
					point2 = polygon[num3 - 1];
				}
				else
				{
					point2 = polygon[count - 1];
				}
				if (point.Y == pt.Y)
				{
					if (point.X == pt.X || (point.Y == point2.Y && pt.X < point2.X != pt.X < point.X))
					{
						break;
					}
					num3++;
					if (num3 == num)
					{
						goto Block_15;
					}
				}
				else
				{
					if (pt.X >= point.X || pt.X >= point2.X)
					{
						if (pt.X > point2.X && pt.X > point.X)
						{
							num2 = 1 - num2;
						}
						else
						{
							double num5 = InternalClipper.CrossProduct(point2, point, pt);
							if (num5 == 0.0)
							{
								return PointInPolygonResult.IsOn;
							}
							if (num5 < 0.0 == flag)
							{
								num2 = 1 - num2;
							}
						}
					}
					flag = !flag;
					num3++;
				}
			}
			return PointInPolygonResult.IsOn;
			Block_15:
			IL_1CF:
			if (flag != flag2)
			{
				if (num3 == count)
				{
					num3 = 0;
				}
				double num5;
				if (num3 == 0)
				{
					num5 = InternalClipper.CrossProduct(polygon[count - 1], polygon[0], pt);
				}
				else
				{
					num5 = InternalClipper.CrossProduct(polygon[num3 - 1], polygon[num3], pt);
				}
				if (num5 == 0.0)
				{
					return PointInPolygonResult.IsOn;
				}
				if (num5 < 0.0 == flag)
				{
					num2 = 1 - num2;
				}
			}
			if (num2 == 0)
			{
				return PointInPolygonResult.IsOutside;
			}
			return PointInPolygonResult.IsInside;
		}

		// Token: 0x04000021 RID: 33
		internal const long MaxInt64 = 9223372036854775807L;

		// Token: 0x04000022 RID: 34
		internal const long MaxCoord = 2305843009213693951L;

		// Token: 0x04000023 RID: 35
		internal const double max_coord = 2.305843009213694E+18;

		// Token: 0x04000024 RID: 36
		internal const double min_coord = -2.305843009213694E+18;

		// Token: 0x04000025 RID: 37
		internal const long Invalid64 = 9223372036854775807L;

		// Token: 0x04000026 RID: 38
		internal const double defaultArcTolerance = 0.25;

		// Token: 0x04000027 RID: 39
		internal const double floatingPointTolerance = 1E-12;

		// Token: 0x04000028 RID: 40
		internal const double defaultMinimumEdgeLength = 0.1;

		// Token: 0x04000029 RID: 41
		[Nullable(1)]
		private static readonly string precision_range_error = "Error: Precision is out of range.";

		// Token: 0x02000035 RID: 53
		public struct MultiplyUInt64Result
		{
			// Token: 0x040000BE RID: 190
			public ulong lo64;

			// Token: 0x040000BF RID: 191
			public ulong hi64;
		}
	}
}
