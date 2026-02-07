using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000012 RID: 18
	[NullableContext(1)]
	[Nullable(0)]
	public static class Clipper
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000031E5 File Offset: 0x000013E5
		public static Rect64 InvalidRect64
		{
			get
			{
				return Clipper.invalidRect64;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000031EC File Offset: 0x000013EC
		public static RectD InvalidRectD
		{
			get
			{
				return Clipper.invalidRectD;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000031F3 File Offset: 0x000013F3
		public static List<List<Point64>> Intersect(List<List<Point64>> subject, List<List<Point64>> clip, FillRule fillRule)
		{
			return Clipper.BooleanOp(ClipType.Intersection, subject, clip, fillRule);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000031FE File Offset: 0x000013FE
		public static PathsD Intersect(PathsD subject, PathsD clip, FillRule fillRule, int precision = 2)
		{
			return Clipper.BooleanOp(ClipType.Intersection, subject, clip, fillRule, precision);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000320A File Offset: 0x0000140A
		public static List<List<Point64>> Union(List<List<Point64>> subject, FillRule fillRule)
		{
			return Clipper.BooleanOp(ClipType.Union, subject, null, fillRule);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003215 File Offset: 0x00001415
		public static List<List<Point64>> Union(List<List<Point64>> subject, List<List<Point64>> clip, FillRule fillRule)
		{
			return Clipper.BooleanOp(ClipType.Union, subject, clip, fillRule);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003220 File Offset: 0x00001420
		public static PathsD Union(PathsD subject, FillRule fillRule)
		{
			return Clipper.BooleanOp(ClipType.Union, subject, null, fillRule, 2);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000322C File Offset: 0x0000142C
		public static PathsD Union(PathsD subject, PathsD clip, FillRule fillRule, int precision = 2)
		{
			return Clipper.BooleanOp(ClipType.Union, subject, clip, fillRule, precision);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003238 File Offset: 0x00001438
		public static List<List<Point64>> Difference(List<List<Point64>> subject, List<List<Point64>> clip, FillRule fillRule)
		{
			return Clipper.BooleanOp(ClipType.Difference, subject, clip, fillRule);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003243 File Offset: 0x00001443
		public static PathsD Difference(PathsD subject, PathsD clip, FillRule fillRule, int precision = 2)
		{
			return Clipper.BooleanOp(ClipType.Difference, subject, clip, fillRule, precision);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000324F File Offset: 0x0000144F
		public static List<List<Point64>> Xor(List<List<Point64>> subject, List<List<Point64>> clip, FillRule fillRule)
		{
			return Clipper.BooleanOp(ClipType.Xor, subject, clip, fillRule);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000325A File Offset: 0x0000145A
		public static PathsD Xor(PathsD subject, PathsD clip, FillRule fillRule, int precision = 2)
		{
			return Clipper.BooleanOp(ClipType.Xor, subject, clip, fillRule, precision);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003268 File Offset: 0x00001468
		public static List<List<Point64>> BooleanOp(ClipType clipType, [Nullable(new byte[]
		{
			2,
			1
		})] List<List<Point64>> subject, [Nullable(new byte[]
		{
			2,
			1
		})] List<List<Point64>> clip, FillRule fillRule)
		{
			List<List<Point64>> list = new List<List<Point64>>();
			if (subject == null)
			{
				return list;
			}
			Clipper64 clipper = new Clipper64();
			clipper.AddPaths(subject, PathType.Subject, false);
			if (clip != null)
			{
				clipper.AddPaths(clip, PathType.Clip, false);
			}
			clipper.Execute(clipType, fillRule, list);
			return list;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000032A8 File Offset: 0x000014A8
		public static void BooleanOp(ClipType clipType, [Nullable(new byte[]
		{
			2,
			1
		})] List<List<Point64>> subject, [Nullable(new byte[]
		{
			2,
			1
		})] List<List<Point64>> clip, PolyTree64 polytree, FillRule fillRule)
		{
			if (subject == null)
			{
				return;
			}
			Clipper64 clipper = new Clipper64();
			clipper.AddPaths(subject, PathType.Subject, false);
			if (clip != null)
			{
				clipper.AddPaths(clip, PathType.Clip, false);
			}
			clipper.Execute(clipType, fillRule, polytree);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000032E0 File Offset: 0x000014E0
		public static PathsD BooleanOp(ClipType clipType, PathsD subject, [Nullable(2)] PathsD clip, FillRule fillRule, int precision = 2)
		{
			PathsD pathsD = new PathsD();
			ClipperD clipperD = new ClipperD(precision);
			clipperD.AddSubject(subject);
			if (clip != null)
			{
				clipperD.AddClip(clip);
			}
			clipperD.Execute(clipType, fillRule, pathsD);
			return pathsD;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003318 File Offset: 0x00001518
		[NullableContext(2)]
		public static void BooleanOp(ClipType clipType, PathsD subject, PathsD clip, [Nullable(1)] PolyTreeD polytree, FillRule fillRule, int precision = 2)
		{
			if (subject == null)
			{
				return;
			}
			ClipperD clipperD = new ClipperD(precision);
			clipperD.AddPaths(subject, PathType.Subject, false);
			if (clip != null)
			{
				clipperD.AddPaths(clip, PathType.Clip, false);
			}
			clipperD.Execute(clipType, fillRule, polytree);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003354 File Offset: 0x00001554
		public static List<List<Point64>> InflatePaths(List<List<Point64>> paths, double delta, JoinType joinType, EndType endType, double miterLimit = 2.0)
		{
			ClipperOffset clipperOffset = new ClipperOffset(miterLimit, 0.0, false, false);
			clipperOffset.AddPaths(paths, joinType, endType);
			List<List<Point64>> list = new List<List<Point64>>();
			clipperOffset.Execute(delta, list);
			return list;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000338C File Offset: 0x0000158C
		public static PathsD InflatePaths(PathsD paths, double delta, JoinType joinType, EndType endType, double miterLimit = 2.0, int precision = 2)
		{
			InternalClipper.CheckPrecision(precision);
			double num = Math.Pow(10.0, (double)precision);
			List<List<Point64>> list = Clipper.ScalePaths64(paths, num);
			ClipperOffset clipperOffset = new ClipperOffset(miterLimit, 0.0, false, false);
			clipperOffset.AddPaths(list, joinType, endType);
			clipperOffset.Execute(delta * num, list);
			return Clipper.ScalePathsD(list, 1.0 / num);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000033EF File Offset: 0x000015EF
		public static List<List<Point64>> RectClip(Rect64 rect, List<List<Point64>> paths)
		{
			if (rect.IsEmpty() || paths.Count == 0)
			{
				return new List<List<Point64>>();
			}
			return new RectClip64(rect).Execute(paths);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003414 File Offset: 0x00001614
		public static List<List<Point64>> RectClip(Rect64 rect, List<Point64> path)
		{
			if (rect.IsEmpty() || path.Count == 0)
			{
				return new List<List<Point64>>();
			}
			List<List<Point64>> paths = new List<List<Point64>>
			{
				path
			};
			return Clipper.RectClip(rect, paths);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000344C File Offset: 0x0000164C
		public static PathsD RectClip(RectD rect, PathsD paths, int precision = 2)
		{
			InternalClipper.CheckPrecision(precision);
			if (rect.IsEmpty() || paths.Count == 0)
			{
				return new PathsD();
			}
			double num = Math.Pow(10.0, (double)precision);
			Rect64 rect2 = Clipper.ScaleRect(rect, num);
			List<List<Point64>> paths2 = Clipper.ScalePaths64(paths, num);
			paths2 = new RectClip64(rect2).Execute(paths2);
			return Clipper.ScalePathsD(paths2, 1.0 / num);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000034B4 File Offset: 0x000016B4
		public static PathsD RectClip(RectD rect, PathD path, int precision = 2)
		{
			if (rect.IsEmpty() || path.Count == 0)
			{
				return new PathsD();
			}
			PathsD paths = new PathsD
			{
				path
			};
			return Clipper.RectClip(rect, paths, precision);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000034ED File Offset: 0x000016ED
		public static List<List<Point64>> RectClipLines(Rect64 rect, List<List<Point64>> paths)
		{
			if (rect.IsEmpty() || paths.Count == 0)
			{
				return new List<List<Point64>>();
			}
			return new RectClipLines64(rect).Execute(paths);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003514 File Offset: 0x00001714
		public static List<List<Point64>> RectClipLines(Rect64 rect, List<Point64> path)
		{
			if (rect.IsEmpty() || path.Count == 0)
			{
				return new List<List<Point64>>();
			}
			List<List<Point64>> paths = new List<List<Point64>>
			{
				path
			};
			return Clipper.RectClipLines(rect, paths);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000354C File Offset: 0x0000174C
		public static PathsD RectClipLines(RectD rect, PathsD paths, int precision = 2)
		{
			InternalClipper.CheckPrecision(precision);
			if (rect.IsEmpty() || paths.Count == 0)
			{
				return new PathsD();
			}
			double num = Math.Pow(10.0, (double)precision);
			Rect64 rect2 = Clipper.ScaleRect(rect, num);
			List<List<Point64>> paths2 = Clipper.ScalePaths64(paths, num);
			paths2 = new RectClipLines64(rect2).Execute(paths2);
			return Clipper.ScalePathsD(paths2, 1.0 / num);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000035B4 File Offset: 0x000017B4
		public static PathsD RectClipLines(RectD rect, PathD path, int precision = 2)
		{
			if (rect.IsEmpty() || path.Count == 0)
			{
				return new PathsD();
			}
			PathsD paths = new PathsD
			{
				path
			};
			return Clipper.RectClipLines(rect, paths, precision);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000035ED File Offset: 0x000017ED
		public static List<List<Point64>> MinkowskiSum(List<Point64> pattern, List<Point64> path, bool isClosed)
		{
			return Minkowski.Sum(pattern, path, isClosed);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000035F7 File Offset: 0x000017F7
		public static PathsD MinkowskiSum(PathD pattern, PathD path, bool isClosed)
		{
			return Minkowski.Sum(pattern, path, isClosed, 2);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003602 File Offset: 0x00001802
		public static List<List<Point64>> MinkowskiDiff(List<Point64> pattern, List<Point64> path, bool isClosed)
		{
			return Minkowski.Diff(pattern, path, isClosed);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000360C File Offset: 0x0000180C
		public static PathsD MinkowskiDiff(PathD pattern, PathD path, bool isClosed)
		{
			return Minkowski.Diff(pattern, path, isClosed, 2);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003618 File Offset: 0x00001818
		public static double Area(List<Point64> path)
		{
			double num = 0.0;
			int count = path.Count;
			if (count < 3)
			{
				return 0.0;
			}
			Point64 point = path[count - 1];
			foreach (Point64 point2 in path)
			{
				num += (double)(point.Y + point2.Y) * (double)(point.X - point2.X);
				point = point2;
			}
			return num * 0.5;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000036BC File Offset: 0x000018BC
		public static double Area(List<List<Point64>> paths)
		{
			double num = 0.0;
			foreach (List<Point64> path in paths)
			{
				num += Clipper.Area(path);
			}
			return num;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003718 File Offset: 0x00001918
		public static double Area(PathD path)
		{
			double num = 0.0;
			int count = path.Count;
			if (count < 3)
			{
				return 0.0;
			}
			PointD pointD = path[count - 1];
			foreach (PointD pointD2 in path)
			{
				num += (pointD.y + pointD2.y) * (pointD.x - pointD2.x);
				pointD = pointD2;
			}
			return num * 0.5;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000037B8 File Offset: 0x000019B8
		public static double Area(PathsD paths)
		{
			double num = 0.0;
			foreach (PathD path in paths)
			{
				num += Clipper.Area(path);
			}
			return num;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003814 File Offset: 0x00001A14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPositive(List<Point64> poly)
		{
			return Clipper.Area(poly) >= 0.0;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000382A File Offset: 0x00001A2A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPositive(PathD poly)
		{
			return Clipper.Area(poly) >= 0.0;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003840 File Offset: 0x00001A40
		public static string Path64ToString(List<Point64> path)
		{
			string str = "";
			foreach (Point64 point in path)
			{
				str += point.ToString();
			}
			return str + "\n";
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000038AC File Offset: 0x00001AAC
		public static string Paths64ToString(List<List<Point64>> paths)
		{
			string text = "";
			foreach (List<Point64> path in paths)
			{
				text += Clipper.Path64ToString(path);
			}
			return text;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003908 File Offset: 0x00001B08
		public static string PathDToString(PathD path)
		{
			string str = "";
			foreach (PointD pointD in path)
			{
				str += pointD.ToString(2);
			}
			return str + "\n";
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003970 File Offset: 0x00001B70
		public static string PathsDToString(PathsD paths)
		{
			string text = "";
			foreach (PathD path in paths)
			{
				text += Clipper.PathDToString(path);
			}
			return text;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000039CC File Offset: 0x00001BCC
		public static List<Point64> OffsetPath(List<Point64> path, long dx, long dy)
		{
			List<Point64> list = new List<Point64>(path.Count);
			foreach (Point64 point in path)
			{
				list.Add(new Point64(point.X + dx, point.Y + dy));
			}
			return list;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003A3C File Offset: 0x00001C3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point64 ScalePoint64(Point64 pt, double scale)
		{
			return new Point64
			{
				X = (long)Math.Round((double)pt.X * scale, MidpointRounding.AwayFromZero),
				Y = (long)Math.Round((double)pt.Y * scale, MidpointRounding.AwayFromZero)
			};
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003A80 File Offset: 0x00001C80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static PointD ScalePointD(Point64 pt, double scale)
		{
			return new PointD
			{
				x = (double)pt.X * scale,
				y = (double)pt.Y * scale
			};
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003AB8 File Offset: 0x00001CB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rect64 ScaleRect(RectD rec, double scale)
		{
			return new Rect64
			{
				left = (long)(rec.left * scale),
				top = (long)(rec.top * scale),
				right = (long)(rec.right * scale),
				bottom = (long)(rec.bottom * scale)
			};
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003B10 File Offset: 0x00001D10
		public static List<Point64> ScalePath(List<Point64> path, double scale)
		{
			if (InternalClipper.IsAlmostZero(scale - 1.0))
			{
				return path;
			}
			List<Point64> list = new List<Point64>(path.Count);
			foreach (Point64 point in path)
			{
				list.Add(new Point64((double)point.X * scale, (double)point.Y * scale));
			}
			return list;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003B98 File Offset: 0x00001D98
		public static List<List<Point64>> ScalePaths(List<List<Point64>> paths, double scale)
		{
			if (InternalClipper.IsAlmostZero(scale - 1.0))
			{
				return paths;
			}
			List<List<Point64>> list = new List<List<Point64>>(paths.Count);
			foreach (List<Point64> path in paths)
			{
				list.Add(Clipper.ScalePath(path, scale));
			}
			return list;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003C10 File Offset: 0x00001E10
		public static PathD ScalePath(PathD path, double scale)
		{
			if (InternalClipper.IsAlmostZero(scale - 1.0))
			{
				return path;
			}
			PathD pathD = new PathD(path.Count);
			foreach (PointD pt in path)
			{
				pathD.Add(new PointD(pt, scale));
			}
			return pathD;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003C88 File Offset: 0x00001E88
		public static PathsD ScalePaths(PathsD paths, double scale)
		{
			if (InternalClipper.IsAlmostZero(scale - 1.0))
			{
				return paths;
			}
			PathsD pathsD = new PathsD(paths.Count);
			foreach (PathD path in paths)
			{
				pathsD.Add(Clipper.ScalePath(path, scale));
			}
			return pathsD;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003D00 File Offset: 0x00001F00
		public static List<Point64> ScalePath64(PathD path, double scale)
		{
			List<Point64> list = new List<Point64>(path.Count);
			foreach (PointD pt in path)
			{
				list.Add(new Point64(pt, scale));
			}
			return list;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003D64 File Offset: 0x00001F64
		public static List<List<Point64>> ScalePaths64(PathsD paths, double scale)
		{
			List<List<Point64>> list = new List<List<Point64>>(paths.Count);
			foreach (PathD path in paths)
			{
				list.Add(Clipper.ScalePath64(path, scale));
			}
			return list;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003DC8 File Offset: 0x00001FC8
		public static PathD ScalePathD(List<Point64> path, double scale)
		{
			PathD pathD = new PathD(path.Count);
			foreach (Point64 pt in path)
			{
				pathD.Add(new PointD(pt, scale));
			}
			return pathD;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003E2C File Offset: 0x0000202C
		public static PathsD ScalePathsD(List<List<Point64>> paths, double scale)
		{
			PathsD pathsD = new PathsD(paths.Count);
			foreach (List<Point64> path in paths)
			{
				pathsD.Add(Clipper.ScalePathD(path, scale));
			}
			return pathsD;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003E90 File Offset: 0x00002090
		public static List<Point64> Path64(PathD path)
		{
			List<Point64> list = new List<Point64>(path.Count);
			foreach (PointD pt in path)
			{
				list.Add(new Point64(pt));
			}
			return list;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003EF0 File Offset: 0x000020F0
		public static List<List<Point64>> Paths64(PathsD paths)
		{
			List<List<Point64>> list = new List<List<Point64>>(paths.Count);
			foreach (PathD path in paths)
			{
				list.Add(Clipper.Path64(path));
			}
			return list;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003F50 File Offset: 0x00002150
		public static PathsD PathsD(List<List<Point64>> paths)
		{
			PathsD pathsD = new PathsD(paths.Count);
			foreach (List<Point64> path in paths)
			{
				pathsD.Add(Clipper.PathD(path));
			}
			return pathsD;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003FB0 File Offset: 0x000021B0
		public static PathD PathD(List<Point64> path)
		{
			PathD pathD = new PathD(path.Count);
			foreach (Point64 pt in path)
			{
				pathD.Add(new PointD(pt));
			}
			return pathD;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004010 File Offset: 0x00002210
		public static List<Point64> TranslatePath(List<Point64> path, long dx, long dy)
		{
			List<Point64> list = new List<Point64>(path.Count);
			foreach (Point64 point in path)
			{
				list.Add(new Point64(point.X + dx, point.Y + dy));
			}
			return list;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004080 File Offset: 0x00002280
		public static List<List<Point64>> TranslatePaths(List<List<Point64>> paths, long dx, long dy)
		{
			List<List<Point64>> list = new List<List<Point64>>(paths.Count);
			foreach (List<Point64> path in paths)
			{
				list.Add(Clipper.OffsetPath(path, dx, dy));
			}
			return list;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000040E4 File Offset: 0x000022E4
		public static PathD TranslatePath(PathD path, double dx, double dy)
		{
			PathD pathD = new PathD(path.Count);
			foreach (PointD pointD in path)
			{
				pathD.Add(new PointD(pointD.x + dx, pointD.y + dy));
			}
			return pathD;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004154 File Offset: 0x00002354
		public static PathsD TranslatePaths(PathsD paths, double dx, double dy)
		{
			PathsD pathsD = new PathsD(paths.Count);
			foreach (PathD path in paths)
			{
				pathsD.Add(Clipper.TranslatePath(path, dx, dy));
			}
			return pathsD;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000041B8 File Offset: 0x000023B8
		public static List<Point64> ReversePath(List<Point64> path)
		{
			List<Point64> list = new List<Point64>(path);
			list.Reverse();
			return list;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000041C6 File Offset: 0x000023C6
		public static PathD ReversePath(PathD path)
		{
			PathD pathD = new PathD(path);
			pathD.Reverse();
			return pathD;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000041D4 File Offset: 0x000023D4
		public static List<List<Point64>> ReversePaths(List<List<Point64>> paths)
		{
			List<List<Point64>> list = new List<List<Point64>>(paths.Count);
			foreach (List<Point64> path in paths)
			{
				list.Add(Clipper.ReversePath(path));
			}
			return list;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004234 File Offset: 0x00002434
		public static PathsD ReversePaths(PathsD paths)
		{
			PathsD pathsD = new PathsD(paths.Count);
			foreach (PathD path in paths)
			{
				pathsD.Add(Clipper.ReversePath(path));
			}
			return pathsD;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004294 File Offset: 0x00002494
		public static Rect64 GetBounds(List<Point64> path)
		{
			Rect64 rect = Clipper.InvalidRect64;
			foreach (Point64 point in path)
			{
				if (point.X < rect.left)
				{
					rect.left = point.X;
				}
				if (point.X > rect.right)
				{
					rect.right = point.X;
				}
				if (point.Y < rect.top)
				{
					rect.top = point.Y;
				}
				if (point.Y > rect.bottom)
				{
					rect.bottom = point.Y;
				}
			}
			if (rect.left != 9223372036854775807L)
			{
				return rect;
			}
			return default(Rect64);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000436C File Offset: 0x0000256C
		public static Rect64 GetBounds(List<List<Point64>> paths)
		{
			Rect64 rect = Clipper.InvalidRect64;
			foreach (List<Point64> list in paths)
			{
				foreach (Point64 point in list)
				{
					if (point.X < rect.left)
					{
						rect.left = point.X;
					}
					if (point.X > rect.right)
					{
						rect.right = point.X;
					}
					if (point.Y < rect.top)
					{
						rect.top = point.Y;
					}
					if (point.Y > rect.bottom)
					{
						rect.bottom = point.Y;
					}
				}
			}
			if (rect.left != 9223372036854775807L)
			{
				return rect;
			}
			return default(Rect64);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000447C File Offset: 0x0000267C
		public static RectD GetBounds(PathD path)
		{
			RectD rectD = Clipper.InvalidRectD;
			foreach (PointD pointD in path)
			{
				if (pointD.x < rectD.left)
				{
					rectD.left = pointD.x;
				}
				if (pointD.x > rectD.right)
				{
					rectD.right = pointD.x;
				}
				if (pointD.y < rectD.top)
				{
					rectD.top = pointD.y;
				}
				if (pointD.y > rectD.bottom)
				{
					rectD.bottom = pointD.y;
				}
			}
			if (Math.Abs(rectD.left - 1.7976931348623157E+308) >= 1E-12)
			{
				return rectD;
			}
			return default(RectD);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004560 File Offset: 0x00002760
		public static RectD GetBounds(PathsD paths)
		{
			RectD rectD = Clipper.InvalidRectD;
			foreach (PathD pathD in paths)
			{
				foreach (PointD pointD in pathD)
				{
					if (pointD.x < rectD.left)
					{
						rectD.left = pointD.x;
					}
					if (pointD.x > rectD.right)
					{
						rectD.right = pointD.x;
					}
					if (pointD.y < rectD.top)
					{
						rectD.top = pointD.y;
					}
					if (pointD.y > rectD.bottom)
					{
						rectD.bottom = pointD.y;
					}
				}
			}
			if (Math.Abs(rectD.left - 1.7976931348623157E+308) >= 1E-12)
			{
				return rectD;
			}
			return default(RectD);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004680 File Offset: 0x00002880
		public static List<Point64> MakePath(int[] arr)
		{
			int num = arr.Length / 2;
			List<Point64> list = new List<Point64>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(new Point64((long)arr[i * 2], (long)arr[i * 2 + 1]));
			}
			return list;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000046C0 File Offset: 0x000028C0
		public static List<Point64> MakePath(long[] arr)
		{
			int num = arr.Length / 2;
			List<Point64> list = new List<Point64>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(new Point64(arr[i * 2], arr[i * 2 + 1]));
			}
			return list;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004700 File Offset: 0x00002900
		public static PathD MakePath(double[] arr)
		{
			int num = arr.Length / 2;
			PathD pathD = new PathD(num);
			for (int i = 0; i < num; i++)
			{
				pathD.Add(new PointD(arr[i * 2], arr[i * 2 + 1]));
			}
			return pathD;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000473E File Offset: 0x0000293E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Sqr(double val)
		{
			return val * val;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004743 File Offset: 0x00002943
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Sqr(long val)
		{
			return (double)val * (double)val;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000474A File Offset: 0x0000294A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double DistanceSqr(Point64 pt1, Point64 pt2)
		{
			return Clipper.Sqr(pt1.X - pt2.X) + Clipper.Sqr(pt1.Y - pt2.Y);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004771 File Offset: 0x00002971
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point64 MidPoint(Point64 pt1, Point64 pt2)
		{
			return new Point64((pt1.X + pt2.X) / 2L, (pt1.Y + pt2.Y) / 2L);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004798 File Offset: 0x00002998
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static PointD MidPoint(PointD pt1, PointD pt2)
		{
			return new PointD((pt1.x + pt2.x) / 2.0, (pt1.y + pt2.y) / 2.0);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000047CD File Offset: 0x000029CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InflateRect(ref Rect64 rec, int dx, int dy)
		{
			rec.left -= (long)dx;
			rec.right += (long)dx;
			rec.top -= (long)dy;
			rec.bottom += (long)dy;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000047FF File Offset: 0x000029FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InflateRect(ref RectD rec, double dx, double dy)
		{
			rec.left -= dx;
			rec.right += dx;
			rec.top -= dy;
			rec.bottom += dy;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000482D File Offset: 0x00002A2D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool PointsNearEqual(PointD pt1, PointD pt2, double distanceSqrd)
		{
			return Clipper.Sqr(pt1.x - pt2.x) + Clipper.Sqr(pt1.y - pt2.y) < distanceSqrd;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004858 File Offset: 0x00002A58
		public static PathD StripNearDuplicates(PathD path, double minEdgeLenSqrd, bool isClosedPath)
		{
			int count = path.Count;
			PathD pathD = new PathD(count);
			if (count == 0)
			{
				return pathD;
			}
			PointD pointD = path[0];
			pathD.Add(pointD);
			for (int i = 1; i < count; i++)
			{
				if (!Clipper.PointsNearEqual(pointD, path[i], minEdgeLenSqrd))
				{
					pointD = path[i];
					pathD.Add(pointD);
				}
			}
			if (isClosedPath && Clipper.PointsNearEqual(pointD, pathD[0], minEdgeLenSqrd))
			{
				pathD.RemoveAt(pathD.Count - 1);
			}
			return pathD;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000048D4 File Offset: 0x00002AD4
		public static List<Point64> StripDuplicates(List<Point64> path, bool isClosedPath)
		{
			int count = path.Count;
			List<Point64> list = new List<Point64>(count);
			if (count == 0)
			{
				return list;
			}
			Point64 point = path[0];
			list.Add(point);
			for (int i = 1; i < count; i++)
			{
				if (point != path[i])
				{
					point = path[i];
					list.Add(point);
				}
			}
			if (isClosedPath && point == list[0])
			{
				list.RemoveAt(list.Count - 1);
			}
			return list;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004950 File Offset: 0x00002B50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void AddPolyNodeToPaths(PolyPath64 polyPath, List<List<Point64>> paths)
		{
			if (polyPath.Polygon.Count > 0)
			{
				paths.Add(polyPath.Polygon);
			}
			for (int i = 0; i < polyPath.Count; i++)
			{
				Clipper.AddPolyNodeToPaths((PolyPath64)polyPath._childs[i], paths);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000049A0 File Offset: 0x00002BA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<List<Point64>> PolyTreeToPaths64(PolyTree64 polyTree)
		{
			List<List<Point64>> list = new List<List<Point64>>();
			for (int i = 0; i < polyTree.Count; i++)
			{
				Clipper.AddPolyNodeToPaths((PolyPath64)polyTree._childs[i], list);
			}
			return list;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000049DC File Offset: 0x00002BDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddPolyNodeToPathsD(PolyPathD polyPath, PathsD paths)
		{
			if (polyPath.Polygon.Count > 0)
			{
				paths.Add(polyPath.Polygon);
			}
			for (int i = 0; i < polyPath.Count; i++)
			{
				Clipper.AddPolyNodeToPathsD((PolyPathD)polyPath._childs[i], paths);
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004A2C File Offset: 0x00002C2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static PathsD PolyTreeToPathsD(PolyTreeD polyTree)
		{
			PathsD pathsD = new PathsD();
			foreach (object obj in polyTree)
			{
				Clipper.AddPolyNodeToPathsD((PolyPathD)obj, pathsD);
			}
			return pathsD;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004A88 File Offset: 0x00002C88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double PerpendicDistFromLineSqrd(PointD pt, PointD line1, PointD line2)
		{
			double num = pt.x - line1.x;
			double num2 = pt.y - line1.y;
			double num3 = line2.x - line1.x;
			double num4 = line2.y - line1.y;
			if (num3 == 0.0 && num4 == 0.0)
			{
				return 0.0;
			}
			return Clipper.Sqr(num * num4 - num3 * num2) / (num3 * num3 + num4 * num4);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004B04 File Offset: 0x00002D04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double PerpendicDistFromLineSqrd(Point64 pt, Point64 line1, Point64 line2)
		{
			double num = (double)pt.X - (double)line1.X;
			double num2 = (double)pt.Y - (double)line1.Y;
			double num3 = (double)line2.X - (double)line1.X;
			double num4 = (double)line2.Y - (double)line1.Y;
			if (num3 == 0.0 && num4 == 0.0)
			{
				return 0.0;
			}
			return Clipper.Sqr(num * num4 - num3 * num2) / (num3 * num3 + num4 * num4);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004B88 File Offset: 0x00002D88
		internal static void RDP(List<Point64> path, int begin, int end, double epsSqrd, List<bool> flags)
		{
			int num = 0;
			double num2 = 0.0;
			while (end > begin && path[begin] == path[end])
			{
				flags[end--] = false;
			}
			for (int i = begin + 1; i < end; i++)
			{
				double num3 = Clipper.PerpendicDistFromLineSqrd(path[i], path[begin], path[end]);
				if (num3 > num2)
				{
					num2 = num3;
					num = i;
				}
			}
			if (num2 <= epsSqrd)
			{
				return;
			}
			flags[num] = true;
			if (num > begin + 1)
			{
				Clipper.RDP(path, begin, num, epsSqrd, flags);
			}
			if (num < end - 1)
			{
				Clipper.RDP(path, num, end, epsSqrd, flags);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004C2C File Offset: 0x00002E2C
		public static List<Point64> RamerDouglasPeucker(List<Point64> path, double epsilon)
		{
			int count = path.Count;
			if (count < 5)
			{
				return path;
			}
			List<bool> list = new List<bool>(new bool[count]);
			list[0] = true;
			int index = count - 1;
			list[index] = true;
			List<bool> list2 = list;
			Clipper.RDP(path, 0, count - 1, Clipper.Sqr(epsilon), list2);
			List<Point64> list3 = new List<Point64>(count);
			for (int i = 0; i < count; i++)
			{
				if (list2[i])
				{
					list3.Add(path[i]);
				}
			}
			return list3;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004CA8 File Offset: 0x00002EA8
		public static List<List<Point64>> RamerDouglasPeucker(List<List<Point64>> paths, double epsilon)
		{
			List<List<Point64>> list = new List<List<Point64>>(paths.Count);
			foreach (List<Point64> path in paths)
			{
				list.Add(Clipper.RamerDouglasPeucker(path, epsilon));
			}
			return list;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004D0C File Offset: 0x00002F0C
		internal static void RDP(PathD path, int begin, int end, double epsSqrd, List<bool> flags)
		{
			int num = 0;
			double num2 = 0.0;
			while (end > begin && path[begin] == path[end])
			{
				flags[end--] = false;
			}
			for (int i = begin + 1; i < end; i++)
			{
				double num3 = Clipper.PerpendicDistFromLineSqrd(path[i], path[begin], path[end]);
				if (num3 > num2)
				{
					num2 = num3;
					num = i;
				}
			}
			if (num2 <= epsSqrd)
			{
				return;
			}
			flags[num] = true;
			if (num > begin + 1)
			{
				Clipper.RDP(path, begin, num, epsSqrd, flags);
			}
			if (num < end - 1)
			{
				Clipper.RDP(path, num, end, epsSqrd, flags);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public static PathD RamerDouglasPeucker(PathD path, double epsilon)
		{
			int count = path.Count;
			if (count < 5)
			{
				return path;
			}
			List<bool> list = new List<bool>(new bool[count]);
			list[0] = true;
			int index = count - 1;
			list[index] = true;
			List<bool> list2 = list;
			Clipper.RDP(path, 0, count - 1, Clipper.Sqr(epsilon), list2);
			PathD pathD = new PathD(count);
			for (int i = 0; i < count; i++)
			{
				if (list2[i])
				{
					pathD.Add(path[i]);
				}
			}
			return pathD;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004E2C File Offset: 0x0000302C
		public static PathsD RamerDouglasPeucker(PathsD paths, double epsilon)
		{
			PathsD pathsD = new PathsD(paths.Count);
			foreach (PathD path in paths)
			{
				pathsD.Add(Clipper.RamerDouglasPeucker(path, epsilon));
			}
			return pathsD;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004E90 File Offset: 0x00003090
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int GetNext(int current, int high, ref bool[] flags)
		{
			current++;
			while (current <= high && flags[current])
			{
				current++;
			}
			if (current <= high)
			{
				return current;
			}
			current = 0;
			while (flags[current])
			{
				current++;
			}
			return current;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004EBF File Offset: 0x000030BF
		private static int GetPrior(int current, int high, ref bool[] flags)
		{
			if (current == 0)
			{
				current = high;
			}
			else
			{
				current--;
			}
			while (current > 0 && flags[current])
			{
				current--;
			}
			if (!flags[current])
			{
				return current;
			}
			current = high;
			while (flags[current])
			{
				current--;
			}
			return current;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004EF8 File Offset: 0x000030F8
		public static List<Point64> SimplifyPath(List<Point64> path, double epsilon, bool isClosedPath = true)
		{
			int count = path.Count;
			int num = count - 1;
			double num2 = Clipper.Sqr(epsilon);
			if (count < 4)
			{
				return path;
			}
			bool[] array = new bool[count];
			double[] array2 = new double[count];
			int num3 = 0;
			if (isClosedPath)
			{
				array2[0] = Clipper.PerpendicDistFromLineSqrd(path[0], path[num], path[1]);
				array2[num] = Clipper.PerpendicDistFromLineSqrd(path[num], path[0], path[num - 1]);
			}
			else
			{
				array2[0] = double.MaxValue;
				array2[num] = double.MaxValue;
			}
			for (int i = 1; i < num; i++)
			{
				array2[i] = Clipper.PerpendicDistFromLineSqrd(path[i], path[i - 1], path[i + 1]);
			}
			for (;;)
			{
				if (array2[num3] > num2)
				{
					int num4 = num3;
					do
					{
						num3 = Clipper.GetNext(num3, num, ref array);
					}
					while (num3 != num4 && array2[num3] > num2);
					if (num3 == num4)
					{
						break;
					}
				}
				int num5 = Clipper.GetPrior(num3, num, ref array);
				int next = Clipper.GetNext(num3, num, ref array);
				if (next == num5)
				{
					break;
				}
				int index;
				if (array2[next] < array2[num3])
				{
					index = num5;
					num5 = num3;
					num3 = next;
					next = Clipper.GetNext(next, num, ref array);
				}
				else
				{
					index = Clipper.GetPrior(num5, num, ref array);
				}
				array[num3] = true;
				num3 = next;
				next = Clipper.GetNext(next, num, ref array);
				if (isClosedPath || (num3 != num && num3 != 0))
				{
					array2[num3] = Clipper.PerpendicDistFromLineSqrd(path[num3], path[num5], path[next]);
				}
				if (isClosedPath || (num5 != 0 && num5 != num))
				{
					array2[num5] = Clipper.PerpendicDistFromLineSqrd(path[num5], path[index], path[num3]);
				}
			}
			List<Point64> list = new List<Point64>(count);
			for (int j = 0; j < count; j++)
			{
				if (!array[j])
				{
					list.Add(path[j]);
				}
			}
			return list;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000050F0 File Offset: 0x000032F0
		public static List<List<Point64>> SimplifyPaths(List<List<Point64>> paths, double epsilon, bool isClosedPaths = true)
		{
			List<List<Point64>> list = new List<List<Point64>>(paths.Count);
			foreach (List<Point64> path in paths)
			{
				list.Add(Clipper.SimplifyPath(path, epsilon, isClosedPaths));
			}
			return list;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005154 File Offset: 0x00003354
		public static PathD SimplifyPath(PathD path, double epsilon, bool isClosedPath = true)
		{
			int count = path.Count;
			int num = count - 1;
			double num2 = Clipper.Sqr(epsilon);
			if (count < 4)
			{
				return path;
			}
			bool[] array = new bool[count];
			double[] array2 = new double[count];
			int num3 = 0;
			if (isClosedPath)
			{
				array2[0] = Clipper.PerpendicDistFromLineSqrd(path[0], path[num], path[1]);
				array2[num] = Clipper.PerpendicDistFromLineSqrd(path[num], path[0], path[num - 1]);
			}
			else
			{
				array2[0] = double.MaxValue;
				array2[num] = double.MaxValue;
			}
			for (int i = 1; i < num; i++)
			{
				array2[i] = Clipper.PerpendicDistFromLineSqrd(path[i], path[i - 1], path[i + 1]);
			}
			for (;;)
			{
				if (array2[num3] > num2)
				{
					int num4 = num3;
					do
					{
						num3 = Clipper.GetNext(num3, num, ref array);
					}
					while (num3 != num4 && array2[num3] > num2);
					if (num3 == num4)
					{
						break;
					}
				}
				int num5 = Clipper.GetPrior(num3, num, ref array);
				int next = Clipper.GetNext(num3, num, ref array);
				if (next == num5)
				{
					break;
				}
				int index;
				if (array2[next] < array2[num3])
				{
					index = num5;
					num5 = num3;
					num3 = next;
					next = Clipper.GetNext(next, num, ref array);
				}
				else
				{
					index = Clipper.GetPrior(num5, num, ref array);
				}
				array[num3] = true;
				num3 = next;
				next = Clipper.GetNext(next, num, ref array);
				if (isClosedPath || (num3 != num && num3 != 0))
				{
					array2[num3] = Clipper.PerpendicDistFromLineSqrd(path[num3], path[num5], path[next]);
				}
				if (isClosedPath || (num5 != 0 && num5 != num))
				{
					array2[num5] = Clipper.PerpendicDistFromLineSqrd(path[num5], path[index], path[num3]);
				}
			}
			PathD pathD = new PathD(count);
			for (int j = 0; j < count; j++)
			{
				if (!array[j])
				{
					pathD.Add(path[j]);
				}
			}
			return pathD;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000534C File Offset: 0x0000354C
		public static PathsD SimplifyPaths(PathsD paths, double epsilon, bool isClosedPath = true)
		{
			PathsD pathsD = new PathsD(paths.Count);
			foreach (PathD path in paths)
			{
				pathsD.Add(Clipper.SimplifyPath(path, epsilon, isClosedPath));
			}
			return pathsD;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000053B0 File Offset: 0x000035B0
		public static List<Point64> TrimCollinear(List<Point64> path, bool isOpen = false)
		{
			int num = path.Count;
			int i = 0;
			if (!isOpen)
			{
				while (i < num - 1)
				{
					if (!InternalClipper.IsCollinear(path[num - 1], path[i], path[i + 1]))
					{
						break;
					}
					i++;
				}
				while (i < num - 1 && InternalClipper.IsCollinear(path[num - 2], path[num - 1], path[i]))
				{
					num--;
				}
			}
			if (num - i >= 3)
			{
				List<Point64> list = new List<Point64>(num - i);
				Point64 point = path[i];
				list.Add(point);
				for (i++; i < num - 1; i++)
				{
					if (!InternalClipper.IsCollinear(point, path[i], path[i + 1]))
					{
						point = path[i];
						list.Add(point);
					}
				}
				if (isOpen)
				{
					list.Add(path[num - 1]);
				}
				else if (!InternalClipper.IsCollinear(point, path[num - 1], list[0]))
				{
					list.Add(path[num - 1]);
				}
				else
				{
					while (list.Count > 2 && InternalClipper.IsCollinear(list[list.Count - 1], list[list.Count - 2], list[0]))
					{
						list.RemoveAt(list.Count - 1);
					}
					if (list.Count < 3)
					{
						list.Clear();
					}
				}
				return list;
			}
			if (!isOpen || num < 2 || path[0] == path[1])
			{
				return new List<Point64>();
			}
			return path;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005528 File Offset: 0x00003728
		public static PathD TrimCollinear(PathD path, int precision, bool isOpen = false)
		{
			InternalClipper.CheckPrecision(precision);
			double num = Math.Pow(10.0, (double)precision);
			return Clipper.ScalePathD(Clipper.TrimCollinear(Clipper.ScalePath64(path, num), isOpen), 1.0 / num);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005569 File Offset: 0x00003769
		public static PointInPolygonResult PointInPolygon(Point64 pt, List<Point64> polygon)
		{
			return InternalClipper.PointInPolygon(pt, polygon);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005574 File Offset: 0x00003774
		public static PointInPolygonResult PointInPolygon(PointD pt, PathD polygon, int precision = 2)
		{
			InternalClipper.CheckPrecision(precision);
			double scale = Math.Pow(10.0, (double)precision);
			Point64 pt2 = new Point64(pt, scale);
			List<Point64> polygon2 = Clipper.ScalePath64(polygon, scale);
			return InternalClipper.PointInPolygon(pt2, polygon2);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000055B0 File Offset: 0x000037B0
		public static List<Point64> Ellipse(Point64 center, double radiusX, double radiusY = 0.0, int steps = 0)
		{
			if (radiusX <= 0.0)
			{
				return new List<Point64>();
			}
			if (radiusY <= 0.0)
			{
				radiusY = radiusX;
			}
			if (steps <= 2)
			{
				steps = (int)Math.Ceiling(3.141592653589793 * Math.Sqrt((radiusX + radiusY) / 2.0));
			}
			double num = Math.Sin(6.283185307179586 / (double)steps);
			double num2 = Math.Cos(6.283185307179586 / (double)steps);
			double num3 = num2;
			double num4 = num;
			List<Point64> list = new List<Point64>(steps)
			{
				new Point64((double)center.X + radiusX, (double)center.Y)
			};
			for (int i = 1; i < steps; i++)
			{
				list.Add(new Point64((double)center.X + radiusX * num3, (double)center.Y + radiusY * num4));
				double num5 = num3 * num2 - num4 * num;
				num4 = num4 * num2 + num3 * num;
				num3 = num5;
			}
			return list;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005698 File Offset: 0x00003898
		public static PathD Ellipse(PointD center, double radiusX, double radiusY = 0.0, int steps = 0)
		{
			if (radiusX <= 0.0)
			{
				return new PathD();
			}
			if (radiusY <= 0.0)
			{
				radiusY = radiusX;
			}
			if (steps <= 2)
			{
				steps = (int)Math.Ceiling(3.141592653589793 * Math.Sqrt((radiusX + radiusY) / 2.0));
			}
			double num = Math.Sin(6.283185307179586 / (double)steps);
			double num2 = Math.Cos(6.283185307179586 / (double)steps);
			double num3 = num2;
			double num4 = num;
			PathD pathD = new PathD(steps)
			{
				new PointD(center.x + radiusX, center.y)
			};
			for (int i = 1; i < steps; i++)
			{
				pathD.Add(new PointD(center.x + radiusX * num3, center.y + radiusY * num4));
				double num5 = num3 * num2 - num4 * num;
				num4 = num4 * num2 + num3 * num;
				num3 = num5;
			}
			return pathD;
		}

		// Token: 0x0400002A RID: 42
		private static Rect64 invalidRect64 = new Rect64(false);

		// Token: 0x0400002B RID: 43
		private static RectD invalidRectD = new RectD(false);
	}
}
