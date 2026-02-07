using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000024 RID: 36
	[NullableContext(1)]
	[Nullable(0)]
	public class ClipperD : ClipperBase
	{
		// Token: 0x06000155 RID: 341 RVA: 0x0000A5A4 File Offset: 0x000087A4
		public ClipperD(int roundingDecimalPrecision = 2)
		{
			if (roundingDecimalPrecision < -8 || roundingDecimalPrecision > 8)
			{
				throw new ClipperLibException(this.precision_range_error);
			}
			this._scale = Math.Pow(10.0, (double)roundingDecimalPrecision);
			this._invScale = 1.0 / this._scale;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A603 File Offset: 0x00008803
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddPath(PathD path, PathType polytype, bool isOpen = false)
		{
			base.AddPath(Clipper.ScalePath64(path, this._scale), polytype, isOpen);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000A619 File Offset: 0x00008819
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddPaths(PathsD paths, PathType polytype, bool isOpen = false)
		{
			base.AddPaths(Clipper.ScalePaths64(paths, this._scale), polytype, isOpen);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000A62F File Offset: 0x0000882F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddSubject(PathD path)
		{
			this.AddPath(path, PathType.Subject, false);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000A63A File Offset: 0x0000883A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddOpenSubject(PathD path)
		{
			this.AddPath(path, PathType.Subject, true);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000A645 File Offset: 0x00008845
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddClip(PathD path)
		{
			this.AddPath(path, PathType.Clip, false);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000A650 File Offset: 0x00008850
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddSubject(PathsD paths)
		{
			this.AddPaths(paths, PathType.Subject, false);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000A65B File Offset: 0x0000885B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddOpenSubject(PathsD paths)
		{
			this.AddPaths(paths, PathType.Subject, true);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000A666 File Offset: 0x00008866
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddClip(PathsD paths)
		{
			this.AddPaths(paths, PathType.Clip, false);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000A674 File Offset: 0x00008874
		public bool Execute(ClipType clipType, FillRule fillRule, PathsD solutionClosed, PathsD solutionOpen)
		{
			List<List<Point64>> list = new List<List<Point64>>();
			List<List<Point64>> list2 = new List<List<Point64>>();
			bool flag = true;
			solutionClosed.Clear();
			solutionOpen.Clear();
			try
			{
				base.ExecuteInternal(clipType, fillRule);
				base.BuildPaths(list, list2);
			}
			catch
			{
				flag = false;
			}
			base.ClearSolutionOnly();
			if (!flag)
			{
				return false;
			}
			solutionClosed.EnsureCapacity(list.Count);
			foreach (List<Point64> path in list)
			{
				solutionClosed.Add(Clipper.ScalePathD(path, this._invScale));
			}
			solutionOpen.EnsureCapacity(list2.Count);
			foreach (List<Point64> path2 in list2)
			{
				solutionOpen.Add(Clipper.ScalePathD(path2, this._invScale));
			}
			return true;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000A780 File Offset: 0x00008980
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Execute(ClipType clipType, FillRule fillRule, PathsD solutionClosed)
		{
			return this.Execute(clipType, fillRule, solutionClosed, new PathsD());
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000A790 File Offset: 0x00008990
		public bool Execute(ClipType clipType, FillRule fillRule, PolyTreeD polytree, PathsD openPaths)
		{
			polytree.Clear();
			openPaths.Clear();
			this._using_polytree = true;
			polytree.Scale = this._scale;
			List<List<Point64>> list = new List<List<Point64>>();
			bool flag = true;
			try
			{
				base.ExecuteInternal(clipType, fillRule);
				base.BuildTree(polytree, list);
			}
			catch
			{
				flag = false;
			}
			base.ClearSolutionOnly();
			if (!flag)
			{
				return false;
			}
			if (list.Count > 0)
			{
				openPaths.EnsureCapacity(list.Count);
				foreach (List<Point64> path in list)
				{
					openPaths.Add(Clipper.ScalePathD(path, this._invScale));
				}
			}
			return true;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000A858 File Offset: 0x00008A58
		public bool Execute(ClipType clipType, FillRule fillRule, PolyTreeD polytree)
		{
			return this.Execute(clipType, fillRule, polytree, new PathsD());
		}

		// Token: 0x04000086 RID: 134
		private readonly string precision_range_error = "Error: Precision is out of range.";

		// Token: 0x04000087 RID: 135
		private readonly double _scale;

		// Token: 0x04000088 RID: 136
		private readonly double _invScale;
	}
}
