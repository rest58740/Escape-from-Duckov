using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000023 RID: 35
	[NullableContext(1)]
	[Nullable(0)]
	public class Clipper64 : ClipperBase
	{
		// Token: 0x0600014A RID: 330 RVA: 0x0000A48C File Offset: 0x0000868C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal new void AddPath(List<Point64> path, PathType polytype, bool isOpen = false)
		{
			base.AddPath(path, polytype, isOpen);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000A497 File Offset: 0x00008697
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public new void AddReuseableData(ReuseableDataContainer64 reuseableData)
		{
			base.AddReuseableData(reuseableData);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000A4A0 File Offset: 0x000086A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal new void AddPaths(List<List<Point64>> paths, PathType polytype, bool isOpen = false)
		{
			base.AddPaths(paths, polytype, isOpen);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A4AB File Offset: 0x000086AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddSubject(List<List<Point64>> paths)
		{
			this.AddPaths(paths, PathType.Subject, false);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A4B6 File Offset: 0x000086B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddOpenSubject(List<List<Point64>> paths)
		{
			this.AddPaths(paths, PathType.Subject, true);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A4C1 File Offset: 0x000086C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddClip(List<List<Point64>> paths)
		{
			this.AddPaths(paths, PathType.Clip, false);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A4CC File Offset: 0x000086CC
		public bool Execute(ClipType clipType, FillRule fillRule, List<List<Point64>> solutionClosed, List<List<Point64>> solutionOpen)
		{
			solutionClosed.Clear();
			solutionOpen.Clear();
			try
			{
				base.ExecuteInternal(clipType, fillRule);
				base.BuildPaths(solutionClosed, solutionOpen);
			}
			catch
			{
				this._succeeded = false;
			}
			base.ClearSolutionOnly();
			return this._succeeded;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000A520 File Offset: 0x00008720
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Execute(ClipType clipType, FillRule fillRule, List<List<Point64>> solutionClosed)
		{
			return this.Execute(clipType, fillRule, solutionClosed, new List<List<Point64>>());
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000A530 File Offset: 0x00008730
		public bool Execute(ClipType clipType, FillRule fillRule, PolyTree64 polytree, List<List<Point64>> openPaths)
		{
			polytree.Clear();
			openPaths.Clear();
			this._using_polytree = true;
			try
			{
				base.ExecuteInternal(clipType, fillRule);
				base.BuildTree(polytree, openPaths);
			}
			catch
			{
				this._succeeded = false;
			}
			base.ClearSolutionOnly();
			return this._succeeded;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000A58C File Offset: 0x0000878C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Execute(ClipType clipType, FillRule fillRule, PolyTree64 polytree)
		{
			return this.Execute(clipType, fillRule, polytree, new List<List<Point64>>());
		}
	}
}
