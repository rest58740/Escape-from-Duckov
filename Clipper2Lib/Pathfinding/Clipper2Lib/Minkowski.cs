using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[Nullable(0)]
	public class Minkowski
	{
		// Token: 0x06000181 RID: 385 RVA: 0x0000AD50 File Offset: 0x00008F50
		private static List<List<Point64>> MinkowskiInternal(List<Point64> pattern, List<Point64> path, bool isSum, bool isClosed)
		{
			int num = (!isClosed) ? 1 : 0;
			int count = pattern.Count;
			int count2 = path.Count;
			List<List<Point64>> list = new List<List<Point64>>(count2);
			foreach (Point64 lhs in path)
			{
				List<Point64> list2 = new List<Point64>(count);
				if (isSum)
				{
					using (List<Point64>.Enumerator enumerator2 = pattern.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Point64 rhs = enumerator2.Current;
							list2.Add(lhs + rhs);
						}
						goto IL_B3;
					}
					goto IL_77;
				}
				goto IL_77;
				IL_B3:
				list.Add(list2);
				continue;
				IL_77:
				foreach (Point64 rhs2 in pattern)
				{
					list2.Add(lhs - rhs2);
				}
				goto IL_B3;
			}
			List<List<Point64>> list3 = new List<List<Point64>>((count2 - num) * count);
			int index = isClosed ? (count2 - 1) : 0;
			int index2 = count - 1;
			for (int i = num; i < count2; i++)
			{
				for (int j = 0; j < count; j++)
				{
					List<Point64> list4 = new List<Point64>(4)
					{
						list[index][index2],
						list[i][index2],
						list[i][j],
						list[index][j]
					};
					if (!Clipper.IsPositive(list4))
					{
						list3.Add(Clipper.ReversePath(list4));
					}
					else
					{
						list3.Add(list4);
					}
					index2 = j;
				}
				index = i;
			}
			return list3;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000AF2C File Offset: 0x0000912C
		public static List<List<Point64>> Sum(List<Point64> pattern, List<Point64> path, bool isClosed)
		{
			return Clipper.Union(Minkowski.MinkowskiInternal(pattern, path, true, isClosed), FillRule.NonZero);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000AF40 File Offset: 0x00009140
		public static PathsD Sum(PathD pattern, PathD path, bool isClosed, int decimalPlaces = 2)
		{
			double num = Math.Pow(10.0, (double)decimalPlaces);
			return Clipper.ScalePathsD(Clipper.Union(Minkowski.MinkowskiInternal(Clipper.ScalePath64(pattern, num), Clipper.ScalePath64(path, num), true, isClosed), FillRule.NonZero), 1.0 / num);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000AF89 File Offset: 0x00009189
		public static List<List<Point64>> Diff(List<Point64> pattern, List<Point64> path, bool isClosed)
		{
			return Clipper.Union(Minkowski.MinkowskiInternal(pattern, path, false, isClosed), FillRule.NonZero);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000AF9C File Offset: 0x0000919C
		public static PathsD Diff(PathD pattern, PathD path, bool isClosed, int decimalPlaces = 2)
		{
			double num = Math.Pow(10.0, (double)decimalPlaces);
			return Clipper.ScalePathsD(Clipper.Union(Minkowski.MinkowskiInternal(Clipper.ScalePath64(pattern, num), Clipper.ScalePath64(path, num), false, isClosed), FillRule.NonZero), 1.0 / num);
		}
	}
}
