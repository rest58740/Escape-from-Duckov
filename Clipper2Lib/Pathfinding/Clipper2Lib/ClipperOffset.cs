using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200002E RID: 46
	[NullableContext(1)]
	[Nullable(0)]
	public class ClipperOffset
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000AFED File Offset: 0x000091ED
		// (set) Token: 0x06000188 RID: 392 RVA: 0x0000AFF5 File Offset: 0x000091F5
		public double ArcTolerance { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000AFFE File Offset: 0x000091FE
		// (set) Token: 0x0600018A RID: 394 RVA: 0x0000B006 File Offset: 0x00009206
		public bool MergeGroups { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000B00F File Offset: 0x0000920F
		// (set) Token: 0x0600018C RID: 396 RVA: 0x0000B017 File Offset: 0x00009217
		public double MiterLimit { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000B020 File Offset: 0x00009220
		// (set) Token: 0x0600018E RID: 398 RVA: 0x0000B028 File Offset: 0x00009228
		public bool PreserveCollinear { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000B031 File Offset: 0x00009231
		// (set) Token: 0x06000190 RID: 400 RVA: 0x0000B039 File Offset: 0x00009239
		public bool ReverseSolution { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000B042 File Offset: 0x00009242
		// (set) Token: 0x06000192 RID: 402 RVA: 0x0000B04A File Offset: 0x0000924A
		[Nullable(2)]
		public ClipperOffset.DeltaCallback64 DeltaCallback { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x06000193 RID: 403 RVA: 0x0000B054 File Offset: 0x00009254
		public ClipperOffset(double miterLimit = 2.0, double arcTolerance = 0.0, bool preserveCollinear = false, bool reverseSolution = false)
		{
			this.MiterLimit = miterLimit;
			this.ArcTolerance = arcTolerance;
			this.MergeGroups = true;
			this.PreserveCollinear = preserveCollinear;
			this.ReverseSolution = reverseSolution;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000B0B7 File Offset: 0x000092B7
		public void Clear()
		{
			this._groupList.Clear();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000B0C4 File Offset: 0x000092C4
		public void AddPath(List<Point64> path, JoinType joinType, EndType endType)
		{
			if (path.Count == 0)
			{
				return;
			}
			List<List<Point64>> paths = new List<List<Point64>>(1)
			{
				path
			};
			this.AddPaths(paths, joinType, endType);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000B0F1 File Offset: 0x000092F1
		public void AddPaths(List<List<Point64>> paths, JoinType joinType, EndType endType)
		{
			if (paths.Count == 0)
			{
				return;
			}
			this._groupList.Add(new ClipperOffset.Group(paths, joinType, endType));
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000B110 File Offset: 0x00009310
		private int CalcSolutionCapacity()
		{
			int num = 0;
			foreach (ClipperOffset.Group group in this._groupList)
			{
				num += ((group.endType == EndType.Joined) ? (group.inPaths.Count * 2) : group.inPaths.Count);
			}
			return num;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000B188 File Offset: 0x00009388
		internal bool CheckPathsReversed()
		{
			bool result = false;
			foreach (ClipperOffset.Group group in this._groupList)
			{
				if (group.endType == EndType.Polygon)
				{
					result = group.pathsReversed;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B1E8 File Offset: 0x000093E8
		private void ExecuteInternal(double delta)
		{
			if (this._groupList.Count == 0)
			{
				return;
			}
			this._solution.EnsureCapacity(this.CalcSolutionCapacity());
			if (Math.Abs(delta) < 0.5)
			{
				foreach (ClipperOffset.Group group in this._groupList)
				{
					foreach (List<Point64> item in group.inPaths)
					{
						this._solution.Add(item);
					}
				}
				return;
			}
			this._delta = delta;
			this._mitLimSqr = ((this.MiterLimit <= 1.0) ? 2.0 : (2.0 / Clipper.Sqr(this.MiterLimit)));
			foreach (ClipperOffset.Group group2 in this._groupList)
			{
				this.DoGroupOffset(group2);
			}
			if (this._groupList.Count == 0)
			{
				return;
			}
			bool flag = this.CheckPathsReversed();
			FillRule fillRule = flag ? FillRule.Negative : FillRule.Positive;
			Clipper64 clipper = new Clipper64();
			clipper.PreserveCollinear = this.PreserveCollinear;
			clipper.ReverseSolution = (this.ReverseSolution != flag);
			clipper.AddSubject(this._solution);
			if (this._solutionTree != null)
			{
				clipper.Execute(ClipType.Union, fillRule, this._solutionTree);
				return;
			}
			clipper.Execute(ClipType.Union, fillRule, this._solution);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000B3A4 File Offset: 0x000095A4
		public void Execute(double delta, List<List<Point64>> solution)
		{
			solution.Clear();
			this._solution = solution;
			this.ExecuteInternal(delta);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000B3BA File Offset: 0x000095BA
		public void Execute(double delta, PolyTree64 solutionTree)
		{
			solutionTree.Clear();
			this._solutionTree = solutionTree;
			this._solution.Clear();
			this.ExecuteInternal(delta);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B3DC File Offset: 0x000095DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static PointD GetUnitNormal(Point64 pt1, Point64 pt2)
		{
			double num = (double)(pt2.X - pt1.X);
			double num2 = (double)(pt2.Y - pt1.Y);
			if (num == 0.0 && num2 == 0.0)
			{
				return default(PointD);
			}
			double num3 = 1.0 / Math.Sqrt(num * num + num2 * num2);
			num *= num3;
			num2 *= num3;
			return new PointD(num2, -num);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000B450 File Offset: 0x00009650
		public void Execute(ClipperOffset.DeltaCallback64 deltaCallback, List<List<Point64>> solution)
		{
			this.DeltaCallback = deltaCallback;
			this.Execute(1.0, solution);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000B46C File Offset: 0x0000966C
		internal static int GetLowestPathIdx(List<List<Point64>> paths)
		{
			int result = -1;
			Point64 point = new Point64(long.MaxValue, long.MinValue);
			for (int i = 0; i < paths.Count; i++)
			{
				foreach (Point64 point2 in paths[i])
				{
					if (point2.Y >= point.Y && (point2.Y != point.Y || point2.X < point.X))
					{
						result = i;
						point.X = point2.X;
						point.Y = point2.Y;
					}
				}
			}
			return result;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000B538 File Offset: 0x00009738
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static PointD TranslatePoint(PointD pt, double dx, double dy)
		{
			return new PointD(pt.x + dx, pt.y + dy);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000B54F File Offset: 0x0000974F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static PointD ReflectPoint(PointD pt, PointD pivot)
		{
			return new PointD(pivot.x + (pivot.x - pt.x), pivot.y + (pivot.y - pt.y));
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000B57E File Offset: 0x0000977E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool AlmostZero(double value, double epsilon = 0.001)
		{
			return Math.Abs(value) < epsilon;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000B589 File Offset: 0x00009789
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static double Hypotenuse(double x, double y)
		{
			return Math.Sqrt(Math.Pow(x, 2.0) + Math.Pow(y, 2.0));
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000B5B0 File Offset: 0x000097B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static PointD NormalizeVector(PointD vec)
		{
			double num = ClipperOffset.Hypotenuse(vec.x, vec.y);
			if (ClipperOffset.AlmostZero(num, 0.001))
			{
				return new PointD(0L, 0L);
			}
			double num2 = 1.0 / num;
			return new PointD(vec.x * num2, vec.y * num2);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000B60B File Offset: 0x0000980B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static PointD GetAvgUnitVector(PointD vec1, PointD vec2)
		{
			return ClipperOffset.NormalizeVector(new PointD(vec1.x + vec2.x, vec1.y + vec2.y));
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000B634 File Offset: 0x00009834
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static PointD IntersectPoint(PointD pt1a, PointD pt1b, PointD pt2a, PointD pt2b)
		{
			if (InternalClipper.IsAlmostZero(pt1a.x - pt1b.x))
			{
				if (InternalClipper.IsAlmostZero(pt2a.x - pt2b.x))
				{
					return new PointD(0L, 0L);
				}
				double num = (pt2b.y - pt2a.y) / (pt2b.x - pt2a.x);
				double num2 = pt2a.y - num * pt2a.x;
				return new PointD(pt1a.x, num * pt1a.x + num2);
			}
			else
			{
				if (InternalClipper.IsAlmostZero(pt2a.x - pt2b.x))
				{
					double num3 = (pt1b.y - pt1a.y) / (pt1b.x - pt1a.x);
					double num4 = pt1a.y - num3 * pt1a.x;
					return new PointD(pt2a.x, num3 * pt2a.x + num4);
				}
				double num5 = (pt1b.y - pt1a.y) / (pt1b.x - pt1a.x);
				double num6 = pt1a.y - num5 * pt1a.x;
				double num7 = (pt2b.y - pt2a.y) / (pt2b.x - pt2a.x);
				double num8 = pt2a.y - num7 * pt2a.x;
				if (InternalClipper.IsAlmostZero(num5 - num7))
				{
					return new PointD(0L, 0L);
				}
				double num9 = (num8 - num6) / (num5 - num7);
				return new PointD(num9, num5 * num9 + num6);
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000B79B File Offset: 0x0000999B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Point64 GetPerpendic(Point64 pt, PointD norm)
		{
			return new Point64((double)pt.X + norm.x * this._groupDelta, (double)pt.Y + norm.y * this._groupDelta);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000B7CC File Offset: 0x000099CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private PointD GetPerpendicD(Point64 pt, PointD norm)
		{
			return new PointD((double)pt.X + norm.x * this._groupDelta, (double)pt.Y + norm.y * this._groupDelta);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000B800 File Offset: 0x00009A00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoBevel(List<Point64> path, int j, int k)
		{
			Point64 item;
			Point64 item2;
			if (j == k)
			{
				double num = Math.Abs(this._groupDelta);
				item = new Point64((double)path[j].X - num * this._normals[j].x, (double)path[j].Y - num * this._normals[j].y);
				item2 = new Point64((double)path[j].X + num * this._normals[j].x, (double)path[j].Y + num * this._normals[j].y);
			}
			else
			{
				item = new Point64((double)path[j].X + this._groupDelta * this._normals[k].x, (double)path[j].Y + this._groupDelta * this._normals[k].y);
				item2 = new Point64((double)path[j].X + this._groupDelta * this._normals[j].x, (double)path[j].Y + this._groupDelta * this._normals[j].y);
			}
			this.pathOut.Add(item);
			this.pathOut.Add(item2);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000B978 File Offset: 0x00009B78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoSquare(List<Point64> path, int j, int k)
		{
			PointD avgUnitVector;
			if (j == k)
			{
				avgUnitVector = new PointD(this._normals[j].y, -this._normals[j].x);
			}
			else
			{
				avgUnitVector = ClipperOffset.GetAvgUnitVector(new PointD(-this._normals[k].y, this._normals[k].x), new PointD(this._normals[j].y, -this._normals[j].x));
			}
			double num = Math.Abs(this._groupDelta);
			PointD pointD = new PointD(path[j]);
			pointD = ClipperOffset.TranslatePoint(pointD, num * avgUnitVector.x, num * avgUnitVector.y);
			PointD pt1a = ClipperOffset.TranslatePoint(pointD, this._groupDelta * avgUnitVector.y, this._groupDelta * -avgUnitVector.x);
			PointD pt1b = ClipperOffset.TranslatePoint(pointD, this._groupDelta * -avgUnitVector.y, this._groupDelta * avgUnitVector.x);
			PointD perpendicD = this.GetPerpendicD(path[k], this._normals[k]);
			if (j == k)
			{
				PointD pt2b = new PointD(perpendicD.x + avgUnitVector.x * this._groupDelta, perpendicD.y + avgUnitVector.y * this._groupDelta);
				PointD pt = ClipperOffset.IntersectPoint(pt1a, pt1b, perpendicD, pt2b);
				this.pathOut.Add(new Point64(ClipperOffset.ReflectPoint(pt, pointD)));
				this.pathOut.Add(new Point64(pt));
				return;
			}
			PointD perpendicD2 = this.GetPerpendicD(path[j], this._normals[k]);
			PointD pt2 = ClipperOffset.IntersectPoint(pt1a, pt1b, perpendicD, perpendicD2);
			this.pathOut.Add(new Point64(pt2));
			this.pathOut.Add(new Point64(ClipperOffset.ReflectPoint(pt2, pointD)));
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000BB60 File Offset: 0x00009D60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoMiter(List<Point64> path, int j, int k, double cosA)
		{
			double num = this._groupDelta / (cosA + 1.0);
			this.pathOut.Add(new Point64((double)path[j].X + (this._normals[k].x + this._normals[j].x) * num, (double)path[j].Y + (this._normals[k].y + this._normals[j].y) * num));
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000BBF8 File Offset: 0x00009DF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoRound(List<Point64> path, int j, int k, double angle)
		{
			if (this.DeltaCallback != null)
			{
				double num = Math.Abs(this._groupDelta);
				double num2 = (this.ArcTolerance > 0.01) ? this.ArcTolerance : (Math.Log10(2.0 + num) * 0.25);
				double num3 = 3.141592653589793 / Math.Acos(1.0 - num2 / num);
				this._stepSin = Math.Sin(6.283185307179586 / num3);
				this._stepCos = Math.Cos(6.283185307179586 / num3);
				if (this._groupDelta < 0.0)
				{
					this._stepSin = -this._stepSin;
				}
				this._stepsPerRad = num3 / 6.283185307179586;
			}
			Point64 point = path[j];
			PointD pointD = new PointD(this._normals[k].x * this._groupDelta, this._normals[k].y * this._groupDelta);
			if (j == k)
			{
				pointD.Negate();
			}
			this.pathOut.Add(new Point64((double)point.X + pointD.x, (double)point.Y + pointD.y));
			int num4 = (int)Math.Ceiling(this._stepsPerRad * Math.Abs(angle));
			for (int i = 1; i < num4; i++)
			{
				pointD = new PointD(pointD.x * this._stepCos - this._stepSin * pointD.y, pointD.x * this._stepSin + pointD.y * this._stepCos);
				this.pathOut.Add(new Point64((double)point.X + pointD.x, (double)point.Y + pointD.y));
			}
			this.pathOut.Add(this.GetPerpendic(point, this._normals[j]));
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000BDF0 File Offset: 0x00009FF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void BuildNormals(List<Point64> path)
		{
			int count = path.Count;
			this._normals.Clear();
			if (count == 0)
			{
				return;
			}
			this._normals.EnsureCapacity(count);
			for (int i = 0; i < count - 1; i++)
			{
				this._normals.Add(ClipperOffset.GetUnitNormal(path[i], path[i + 1]));
			}
			this._normals.Add(ClipperOffset.GetUnitNormal(path[count - 1], path[0]));
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000BE70 File Offset: 0x0000A070
		private void OffsetPoint(ClipperOffset.Group group, List<Point64> path, int j, ref int k)
		{
			if (path[j] == path[k])
			{
				k = j;
				return;
			}
			double num = InternalClipper.CrossProduct(this._normals[j], this._normals[k]);
			double num2 = InternalClipper.DotProduct(this._normals[j], this._normals[k]);
			if (num > 1.0)
			{
				num = 1.0;
			}
			else if (num < -1.0)
			{
				num = -1.0;
			}
			if (this.DeltaCallback != null)
			{
				this._groupDelta = this.DeltaCallback(path, this._normals, j, k);
				if (group.pathsReversed)
				{
					this._groupDelta = -this._groupDelta;
				}
			}
			if (Math.Abs(this._groupDelta) < ClipperOffset.Tolerance)
			{
				this.pathOut.Add(path[j]);
				return;
			}
			if (num2 > -0.999 && num * this._groupDelta < 0.0)
			{
				this.pathOut.Add(this.GetPerpendic(path[j], this._normals[k]));
				if (num2 < 0.999)
				{
					this.pathOut.Add(path[j]);
				}
				this.pathOut.Add(this.GetPerpendic(path[j], this._normals[j]));
			}
			else if (num2 > 0.999 && this._joinType != JoinType.Round)
			{
				this.DoMiter(path, j, k, num2);
			}
			else if (this._joinType == JoinType.Miter)
			{
				if (num2 > this._mitLimSqr - 1.0)
				{
					this.DoMiter(path, j, k, num2);
				}
				else
				{
					this.DoSquare(path, j, k);
				}
			}
			else if (this._joinType == JoinType.Round)
			{
				this.DoRound(path, j, k, Math.Atan2(num, num2));
			}
			else if (this._joinType == JoinType.Bevel)
			{
				this.DoBevel(path, j, k);
			}
			else
			{
				this.DoSquare(path, j, k);
			}
			k = j;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000C090 File Offset: 0x0000A290
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void OffsetPolygon(ClipperOffset.Group group, List<Point64> path)
		{
			this.pathOut = new List<Point64>();
			int count = path.Count;
			int num = count - 1;
			for (int i = 0; i < count; i++)
			{
				this.OffsetPoint(group, path, i, ref num);
			}
			this._solution.Add(this.pathOut);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000C0DB File Offset: 0x0000A2DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void OffsetOpenJoined(ClipperOffset.Group group, List<Point64> path)
		{
			this.OffsetPolygon(group, path);
			path = Clipper.ReversePath(path);
			this.BuildNormals(path);
			this.OffsetPolygon(group, path);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000C0FC File Offset: 0x0000A2FC
		private void OffsetOpenPath(ClipperOffset.Group group, List<Point64> path)
		{
			this.pathOut = new List<Point64>();
			int num = path.Count - 1;
			if (this.DeltaCallback != null)
			{
				this._groupDelta = this.DeltaCallback(path, this._normals, 0, 0);
			}
			if (Math.Abs(this._groupDelta) < ClipperOffset.Tolerance)
			{
				this.pathOut.Add(path[0]);
			}
			else
			{
				EndType endType = this._endType;
				if (endType != EndType.Butt)
				{
					if (endType != EndType.Round)
					{
						this.DoSquare(path, 0, 0);
					}
					else
					{
						this.DoRound(path, 0, 0, 3.141592653589793);
					}
				}
				else
				{
					this.DoBevel(path, 0, 0);
				}
			}
			int i = 1;
			int num2 = 0;
			while (i < num)
			{
				this.OffsetPoint(group, path, i, ref num2);
				i++;
			}
			for (int j = num; j > 0; j--)
			{
				this._normals[j] = new PointD(-this._normals[j - 1].x, -this._normals[j - 1].y);
			}
			this._normals[0] = this._normals[num];
			if (this.DeltaCallback != null)
			{
				this._groupDelta = this.DeltaCallback(path, this._normals, num, num);
			}
			if (Math.Abs(this._groupDelta) < ClipperOffset.Tolerance)
			{
				this.pathOut.Add(path[num]);
			}
			else
			{
				EndType endType = this._endType;
				if (endType != EndType.Butt)
				{
					if (endType != EndType.Round)
					{
						this.DoSquare(path, num, num);
					}
					else
					{
						this.DoRound(path, num, num, 3.141592653589793);
					}
				}
				else
				{
					this.DoBevel(path, num, num);
				}
			}
			int k = num - 1;
			int num3 = num;
			while (k > 0)
			{
				this.OffsetPoint(group, path, k, ref num3);
				k--;
			}
			this._solution.Add(this.pathOut);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000C2D0 File Offset: 0x0000A4D0
		private void DoGroupOffset(ClipperOffset.Group group)
		{
			if (group.endType == EndType.Polygon)
			{
				if (group.lowestPathIdx < 0)
				{
					this._delta = Math.Abs(this._delta);
				}
				this._groupDelta = (group.pathsReversed ? (-this._delta) : this._delta);
			}
			else
			{
				this._groupDelta = Math.Abs(this._delta);
			}
			double num = Math.Abs(this._groupDelta);
			this._joinType = group.joinType;
			this._endType = group.endType;
			if (group.joinType == JoinType.Round || group.endType == EndType.Round)
			{
				double num2 = (this.ArcTolerance > 0.01) ? this.ArcTolerance : (Math.Log10(2.0 + num) * 0.25);
				double num3 = 3.141592653589793 / Math.Acos(1.0 - num2 / num);
				this._stepSin = Math.Sin(6.283185307179586 / num3);
				this._stepCos = Math.Cos(6.283185307179586 / num3);
				if (this._groupDelta < 0.0)
				{
					this._stepSin = -this._stepSin;
				}
				this._stepsPerRad = num3 / 6.283185307179586;
			}
			foreach (List<Point64> list in group.inPaths)
			{
				this.pathOut = new List<Point64>();
				int count = list.Count;
				if (count == 1)
				{
					Point64 point = list[0];
					if (this.DeltaCallback != null)
					{
						this._groupDelta = this.DeltaCallback(list, this._normals, 0, 0);
						if (group.pathsReversed)
						{
							this._groupDelta = -this._groupDelta;
						}
						num = Math.Abs(this._groupDelta);
					}
					if (group.endType == EndType.Round)
					{
						double num4 = num;
						int steps = (int)Math.Ceiling(this._stepsPerRad * 2.0 * 3.141592653589793);
						this.pathOut = Clipper.Ellipse(point, num4, num4, steps);
					}
					else
					{
						int num5 = (int)Math.Ceiling(this._groupDelta);
						Rect64 rect = new Rect64(point.X - (long)num5, point.Y - (long)num5, point.X + (long)num5, point.Y + (long)num5);
						this.pathOut = rect.AsPath();
					}
					this._solution.Add(this.pathOut);
				}
				else
				{
					if (count == 2 && group.endType == EndType.Joined)
					{
						this._endType = ((group.joinType == JoinType.Round) ? EndType.Round : EndType.Square);
					}
					this.BuildNormals(list);
					if (this._endType == EndType.Polygon)
					{
						this.OffsetPolygon(group, list);
					}
					else if (this._endType == EndType.Joined)
					{
						this.OffsetOpenJoined(group, list);
					}
					else
					{
						this.OffsetOpenPath(group, list);
					}
				}
			}
		}

		// Token: 0x04000099 RID: 153
		private static readonly double Tolerance = 1E-12;

		// Token: 0x0400009A RID: 154
		private readonly List<ClipperOffset.Group> _groupList = new List<ClipperOffset.Group>();

		// Token: 0x0400009B RID: 155
		private List<Point64> pathOut = new List<Point64>();

		// Token: 0x0400009C RID: 156
		private readonly PathD _normals = new PathD();

		// Token: 0x0400009D RID: 157
		private List<List<Point64>> _solution = new List<List<Point64>>();

		// Token: 0x0400009E RID: 158
		[Nullable(2)]
		private PolyTree64 _solutionTree;

		// Token: 0x0400009F RID: 159
		private double _groupDelta;

		// Token: 0x040000A0 RID: 160
		private double _delta;

		// Token: 0x040000A1 RID: 161
		private double _mitLimSqr;

		// Token: 0x040000A2 RID: 162
		private double _stepsPerRad;

		// Token: 0x040000A3 RID: 163
		private double _stepSin;

		// Token: 0x040000A4 RID: 164
		private double _stepCos;

		// Token: 0x040000A5 RID: 165
		private JoinType _joinType;

		// Token: 0x040000A6 RID: 166
		private EndType _endType;

		// Token: 0x02000038 RID: 56
		[Nullable(0)]
		private class Group
		{
			// Token: 0x060001E3 RID: 483 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
			public Group(List<List<Point64>> paths, JoinType joinType, EndType endType = EndType.Polygon)
			{
				this.joinType = joinType;
				this.endType = endType;
				bool isClosedPath = endType == EndType.Polygon || endType == EndType.Joined;
				this.inPaths = new List<List<Point64>>(paths.Count);
				foreach (List<Point64> path in paths)
				{
					this.inPaths.Add(Clipper.StripDuplicates(path, isClosedPath));
				}
				if (endType == EndType.Polygon)
				{
					this.lowestPathIdx = ClipperOffset.GetLowestPathIdx(this.inPaths);
					this.pathsReversed = (this.lowestPathIdx >= 0 && Clipper.Area(this.inPaths[this.lowestPathIdx]) < 0.0);
					return;
				}
				this.lowestPathIdx = -1;
				this.pathsReversed = false;
			}

			// Token: 0x040000C2 RID: 194
			internal List<List<Point64>> inPaths;

			// Token: 0x040000C3 RID: 195
			internal JoinType joinType;

			// Token: 0x040000C4 RID: 196
			internal EndType endType;

			// Token: 0x040000C5 RID: 197
			internal bool pathsReversed;

			// Token: 0x040000C6 RID: 198
			internal int lowestPathIdx;
		}

		// Token: 0x02000039 RID: 57
		// (Invoke) Token: 0x060001E5 RID: 485
		[NullableContext(0)]
		public delegate double DeltaCallback64(List<Point64> path, PathD path_norms, int currPt, int prevPt);
	}
}
