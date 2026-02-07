using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000022 RID: 34
	[NullableContext(1)]
	[Nullable(0)]
	public class ClipperBase
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00006010 File Offset: 0x00004210
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00006018 File Offset: 0x00004218
		public bool PreserveCollinear { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00006021 File Offset: 0x00004221
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00006029 File Offset: 0x00004229
		public bool ReverseSolution { get; set; }

		// Token: 0x060000D6 RID: 214 RVA: 0x00006034 File Offset: 0x00004234
		public ClipperBase()
		{
			this._minimaList = new List<LocalMinima>();
			this._intersectList = new List<IntersectNode>();
			this._vertexList = new List<Vertex>();
			this._outrecList = new List<OutRec>();
			this._scanlineList = new List<long>();
			this._horzSegList = new List<HorzSegment>();
			this._horzJoinList = new List<HorzJoin>();
			this._vertexPool = new VertexPool(16);
			this._activeArena = new GCArena<Active>();
			this.PreserveCollinear = true;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000060B3 File Offset: 0x000042B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsOdd(int val)
		{
			return (val & 1) != 0;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000060BB File Offset: 0x000042BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsHotEdge(Active ae)
		{
			return ae.outrec != null;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000060C6 File Offset: 0x000042C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsOpen(Active ae)
		{
			return ae.localMin.isOpen;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000060D3 File Offset: 0x000042D3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsOpenEnd(Active ae)
		{
			return ae.localMin.isOpen && ClipperBase.IsOpenEnd(ae.vertexTop);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000060EF File Offset: 0x000042EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsOpenEnd(Vertex v)
		{
			return (v.flags & (VertexFlags.OpenStart | VertexFlags.OpenEnd)) > VertexFlags.None;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000060FC File Offset: 0x000042FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private static Active GetPrevHotEdge(Active ae)
		{
			Active prevInAEL = ae.prevInAEL;
			while (prevInAEL != null && (ClipperBase.IsOpen(prevInAEL) || !ClipperBase.IsHotEdge(prevInAEL)))
			{
				prevInAEL = prevInAEL.prevInAEL;
			}
			return prevInAEL;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000612D File Offset: 0x0000432D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsFront(Active ae)
		{
			return ae == ae.outrec.frontEdge;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006140 File Offset: 0x00004340
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static double GetDx(Point64 pt1, Point64 pt2)
		{
			double num = (double)(pt2.Y - pt1.Y);
			if (num != 0.0)
			{
				return (double)(pt2.X - pt1.X) / num;
			}
			if (pt2.X > pt1.X)
			{
				return double.NegativeInfinity;
			}
			return double.PositiveInfinity;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000619C File Offset: 0x0000439C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long TopX(Active ae, long currentY)
		{
			if (currentY == ae.top.Y || ae.top.X == ae.bot.X)
			{
				return ae.top.X;
			}
			if (currentY == ae.bot.Y)
			{
				return ae.bot.X;
			}
			return ae.bot.X + (long)Math.Round(ae.dx * (double)(currentY - ae.bot.Y), MidpointRounding.ToEven);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000621D File Offset: 0x0000441D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsHorizontal(Active ae)
		{
			return ae.top.Y == ae.bot.Y;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006237 File Offset: 0x00004437
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsHeadingRightHorz(Active ae)
		{
			return double.IsNegativeInfinity(ae.dx);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006244 File Offset: 0x00004444
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsHeadingLeftHorz(Active ae)
		{
			return double.IsPositiveInfinity(ae.dx);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006254 File Offset: 0x00004454
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SwapActives(ref Active ae1, ref Active ae2)
		{
			Active active = ae1;
			ae1 = ae2;
			ae2 = active;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000626B File Offset: 0x0000446B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static PathType GetPolyType(Active ae)
		{
			return ae.localMin.polytype;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006278 File Offset: 0x00004478
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsSamePolyType(Active ae1, Active ae2)
		{
			return ae1.localMin.polytype == ae2.localMin.polytype;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006292 File Offset: 0x00004492
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SetDx(Active ae)
		{
			ae.dx = ClipperBase.GetDx(ae.bot, ae.top);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000062AB File Offset: 0x000044AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vertex NextVertex(Active ae)
		{
			if (ae.windDx > 0)
			{
				return ae.vertexTop.next;
			}
			return ae.vertexTop.prev;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000062CD File Offset: 0x000044CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vertex PrevPrevVertex(Active ae)
		{
			if (ae.windDx > 0)
			{
				return ae.vertexTop.prev.prev;
			}
			return ae.vertexTop.next.next;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000062F9 File Offset: 0x000044F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsMaxima(Vertex vertex)
		{
			return (vertex.flags & VertexFlags.LocalMax) > VertexFlags.None;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006306 File Offset: 0x00004506
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsMaxima(Active ae)
		{
			return ClipperBase.IsMaxima(ae.vertexTop);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006314 File Offset: 0x00004514
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private static Active GetMaximaPair(Active ae)
		{
			for (Active nextInAEL = ae.nextInAEL; nextInAEL != null; nextInAEL = nextInAEL.nextInAEL)
			{
				if (nextInAEL.vertexTop == ae.vertexTop)
				{
					return nextInAEL;
				}
			}
			return null;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006348 File Offset: 0x00004548
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private static Vertex GetCurrYMaximaVertex_Open(Active ae)
		{
			Vertex vertex = ae.vertexTop;
			if (ae.windDx > 0)
			{
				while (vertex.next.pt.Y == vertex.pt.Y)
				{
					if ((vertex.flags & (VertexFlags.OpenEnd | VertexFlags.LocalMax)) != VertexFlags.None)
					{
						break;
					}
					vertex = vertex.next;
				}
			}
			else
			{
				while (vertex.prev.pt.Y == vertex.pt.Y && (vertex.flags & (VertexFlags.OpenEnd | VertexFlags.LocalMax)) == VertexFlags.None)
				{
					vertex = vertex.prev;
				}
			}
			if (!ClipperBase.IsMaxima(vertex))
			{
				vertex = null;
			}
			return vertex;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000063D0 File Offset: 0x000045D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private static Vertex GetCurrYMaximaVertex(Active ae)
		{
			Vertex vertex = ae.vertexTop;
			if (ae.windDx > 0)
			{
				while (vertex.next.pt.Y == vertex.pt.Y)
				{
					vertex = vertex.next;
				}
			}
			else
			{
				while (vertex.prev.pt.Y == vertex.pt.Y)
				{
					vertex = vertex.prev;
				}
			}
			if (!ClipperBase.IsMaxima(vertex))
			{
				vertex = null;
			}
			return vertex;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006444 File Offset: 0x00004644
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SetSides(OutRec outrec, Active startEdge, Active endEdge)
		{
			outrec.frontEdge = startEdge;
			outrec.backEdge = endEdge;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006454 File Offset: 0x00004654
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SwapOutrecs(Active ae1, Active ae2)
		{
			OutRec outrec = ae1.outrec;
			OutRec outrec2 = ae2.outrec;
			if (outrec == outrec2)
			{
				Active frontEdge = outrec.frontEdge;
				outrec.frontEdge = outrec.backEdge;
				outrec.backEdge = frontEdge;
				return;
			}
			if (outrec != null)
			{
				if (ae1 == outrec.frontEdge)
				{
					outrec.frontEdge = ae2;
				}
				else
				{
					outrec.backEdge = ae2;
				}
			}
			if (outrec2 != null)
			{
				if (ae2 == outrec2.frontEdge)
				{
					outrec2.frontEdge = ae1;
				}
				else
				{
					outrec2.backEdge = ae1;
				}
			}
			ae1.outrec = outrec2;
			ae2.outrec = outrec;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000064D4 File Offset: 0x000046D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SetOwner(OutRec outrec, OutRec newOwner)
		{
			while (newOwner.owner != null && newOwner.owner.pts == null)
			{
				newOwner.owner = newOwner.owner.owner;
			}
			OutRec outRec = newOwner;
			while (outRec != null && outRec != outrec)
			{
				outRec = outRec.owner;
			}
			if (outRec != null)
			{
				newOwner.owner = outrec.owner;
			}
			outrec.owner = newOwner;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006534 File Offset: 0x00004734
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static double Area(OutPt op)
		{
			double num = 0.0;
			OutPt outPt = op;
			do
			{
				num += (double)(outPt.prev.pt.Y + outPt.pt.Y) * (double)(outPt.prev.pt.X - outPt.pt.X);
				outPt = outPt.next;
			}
			while (outPt != op);
			return num * 0.5;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000065A4 File Offset: 0x000047A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static double AreaTriangle(Point64 pt1, Point64 pt2, Point64 pt3)
		{
			return (double)(pt3.Y + pt1.Y) * (double)(pt3.X - pt1.X) + (double)(pt1.Y + pt2.Y) * (double)(pt1.X - pt2.X) + (double)(pt2.Y + pt3.Y) * (double)(pt2.X - pt3.X);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000660A File Offset: 0x0000480A
		[NullableContext(2)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static OutRec GetRealOutRec(OutRec outRec)
		{
			while (outRec != null && outRec.pts == null)
			{
				outRec = outRec.owner;
			}
			return outRec;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006622 File Offset: 0x00004822
		[NullableContext(2)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsValidOwner(OutRec outRec, OutRec testOwner)
		{
			while (testOwner != null && testOwner != outRec)
			{
				testOwner = testOwner.owner;
			}
			return testOwner == null;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000663C File Offset: 0x0000483C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void UncoupleOutRec(Active ae)
		{
			OutRec outrec = ae.outrec;
			if (outrec == null)
			{
				return;
			}
			outrec.frontEdge.outrec = null;
			outrec.backEdge.outrec = null;
			outrec.frontEdge = null;
			outrec.backEdge = null;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000667A File Offset: 0x0000487A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool OutrecIsAscending(Active hotEdge)
		{
			return hotEdge == hotEdge.outrec.frontEdge;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000668C File Offset: 0x0000488C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SwapFrontBackSides(OutRec outrec)
		{
			Active frontEdge = outrec.frontEdge;
			outrec.frontEdge = outrec.backEdge;
			outrec.backEdge = frontEdge;
			outrec.pts = outrec.pts.next;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000066C4 File Offset: 0x000048C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool EdgesAdjacentInAEL(IntersectNode inode)
		{
			return inode.edge1.nextInAEL == inode.edge2 || inode.edge1.prevInAEL == inode.edge2;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000066F0 File Offset: 0x000048F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void ClearSolutionOnly()
		{
			while (this._actives != null)
			{
				this.DeleteFromAEL(this._actives);
			}
			this._scanlineList.Clear();
			this.DisposeIntersectNodes();
			this._outrecList.Clear();
			this._horzSegList.Clear();
			this._horzJoinList.Clear();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006748 File Offset: 0x00004948
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			this.ClearSolutionOnly();
			foreach (Vertex v in this._vertexList)
			{
				this._vertexPool.Pool(v);
			}
			this._minimaList.Clear();
			this._vertexList.Clear();
			this._currentLocMin = 0;
			this._isSortedMinimaList = false;
			this._hasOpenPaths = false;
			this._activeArena.Reclaim();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000067E0 File Offset: 0x000049E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void Reset()
		{
			if (!this._isSortedMinimaList)
			{
				this._minimaList.Sort(default(LocMinSorter));
				this._isSortedMinimaList = true;
			}
			this._scanlineList.EnsureCapacity(this._minimaList.Count);
			for (int i = this._minimaList.Count - 1; i >= 0; i--)
			{
				this._scanlineList.Add(this._minimaList[i].vertex.pt.Y);
			}
			this._currentBotY = 0L;
			this._currentLocMin = 0;
			this._actives = null;
			this._sel = null;
			this._succeeded = true;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006890 File Offset: 0x00004A90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void InsertScanline(long y)
		{
			int num = this._scanlineList.BinarySearch(y);
			if (num >= 0)
			{
				return;
			}
			num = ~num;
			this._scanlineList.Insert(num, y);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000068C0 File Offset: 0x00004AC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool PopScanline(out long y)
		{
			int num = this._scanlineList.Count - 1;
			if (num < 0)
			{
				y = 0L;
				return false;
			}
			y = this._scanlineList[num];
			this._scanlineList.RemoveAt(num--);
			while (num >= 0 && y == this._scanlineList[num])
			{
				this._scanlineList.RemoveAt(num--);
			}
			return true;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000692A File Offset: 0x00004B2A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool HasLocMinAtY(long y)
		{
			return this._currentLocMin < this._minimaList.Count && this._minimaList[this._currentLocMin].vertex.pt.Y == y;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006964 File Offset: 0x00004B64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private LocalMinima PopLocalMinima()
		{
			List<LocalMinima> minimaList = this._minimaList;
			int currentLocMin = this._currentLocMin;
			this._currentLocMin = currentLocMin + 1;
			return minimaList[currentLocMin];
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000698D File Offset: 0x00004B8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddSubject(List<Point64> path)
		{
			this.AddPath(path, PathType.Subject, false);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006998 File Offset: 0x00004B98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddOpenSubject(List<Point64> path)
		{
			this.AddPath(path, PathType.Subject, true);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000069A3 File Offset: 0x00004BA3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddClip(List<Point64> path)
		{
			this.AddPath(path, PathType.Clip, false);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000069B0 File Offset: 0x00004BB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void AddPath(List<Point64> path, PathType polytype, bool isOpen = false)
		{
			List<List<Point64>> paths = new List<List<Point64>>(1)
			{
				path
			};
			this.AddPaths(paths, polytype, isOpen);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000069D4 File Offset: 0x00004BD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void AddPaths(List<List<Point64>> paths, PathType polytype, bool isOpen = false)
		{
			if (isOpen)
			{
				this._hasOpenPaths = true;
			}
			this._isSortedMinimaList = false;
			ClipperEngine.AddPathsToVertexList(paths, polytype, isOpen, this._minimaList, this._vertexList, this._vertexPool);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006A01 File Offset: 0x00004C01
		[NullableContext(0)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void AddPath(Point64* path, int length, PathType polytype, bool isOpen = false)
		{
			SpanCompat<Point64> path2 = new SpanCompat<Point64>((void*)path, length);
			if (isOpen)
			{
				this._hasOpenPaths = true;
			}
			this._isSortedMinimaList = false;
			ClipperEngine.AddPathToVertexList(path2, polytype, isOpen, this._minimaList, this._vertexList, this._vertexPool);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006A38 File Offset: 0x00004C38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void AddReuseableData(ReuseableDataContainer64 reuseableData)
		{
			if (reuseableData._minimaList.Count == 0)
			{
				return;
			}
			this._isSortedMinimaList = false;
			foreach (LocalMinima localMinima in reuseableData._minimaList)
			{
				this._minimaList.Add(new LocalMinima(localMinima.vertex, localMinima.polytype, localMinima.isOpen));
				if (localMinima.isOpen)
				{
					this._hasOpenPaths = true;
				}
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006ACC File Offset: 0x00004CCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsContributingClosed(Active ae)
		{
			switch (this._fillrule)
			{
			case FillRule.NonZero:
				if (Math.Abs(ae.windCount) != 1)
				{
					return false;
				}
				break;
			case FillRule.Positive:
				if (ae.windCount != 1)
				{
					return false;
				}
				break;
			case FillRule.Negative:
				if (ae.windCount != -1)
				{
					return false;
				}
				break;
			}
			switch (this._cliptype)
			{
			case ClipType.Intersection:
			{
				FillRule fillrule = this._fillrule;
				bool flag;
				if (fillrule != FillRule.Positive)
				{
					if (fillrule != FillRule.Negative)
					{
						flag = (ae.windCount2 != 0);
					}
					else
					{
						flag = (ae.windCount2 < 0);
					}
				}
				else
				{
					flag = (ae.windCount2 > 0);
				}
				return flag;
			}
			case ClipType.Union:
			{
				FillRule fillrule = this._fillrule;
				bool flag;
				if (fillrule != FillRule.Positive)
				{
					if (fillrule != FillRule.Negative)
					{
						flag = (ae.windCount2 == 0);
					}
					else
					{
						flag = (ae.windCount2 >= 0);
					}
				}
				else
				{
					flag = (ae.windCount2 <= 0);
				}
				return flag;
			}
			case ClipType.Difference:
			{
				FillRule fillrule = this._fillrule;
				bool flag;
				if (fillrule != FillRule.Positive)
				{
					if (fillrule != FillRule.Negative)
					{
						flag = (ae.windCount2 == 0);
					}
					else
					{
						flag = (ae.windCount2 >= 0);
					}
				}
				else
				{
					flag = (ae.windCount2 <= 0);
				}
				bool flag2 = flag;
				if (ClipperBase.GetPolyType(ae) != PathType.Subject)
				{
					return !flag2;
				}
				return flag2;
			}
			case ClipType.Xor:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006C00 File Offset: 0x00004E00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsContributingOpen(Active ae)
		{
			FillRule fillrule = this._fillrule;
			bool flag;
			bool flag2;
			if (fillrule != FillRule.Positive)
			{
				if (fillrule != FillRule.Negative)
				{
					flag = (ae.windCount != 0);
					flag2 = (ae.windCount2 != 0);
				}
				else
				{
					flag = (ae.windCount < 0);
					flag2 = (ae.windCount2 < 0);
				}
			}
			else
			{
				flag = (ae.windCount > 0);
				flag2 = (ae.windCount2 > 0);
			}
			ClipType cliptype = this._cliptype;
			bool result;
			if (cliptype != ClipType.Intersection)
			{
				if (cliptype != ClipType.Union)
				{
					result = !flag2;
				}
				else
				{
					result = (!flag && !flag2);
				}
			}
			else
			{
				result = flag2;
			}
			return result;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006C8C File Offset: 0x00004E8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetWindCountForClosedPathEdge(Active ae)
		{
			Active active = ae.prevInAEL;
			PathType polyType = ClipperBase.GetPolyType(ae);
			while (active != null && (ClipperBase.GetPolyType(active) != polyType || ClipperBase.IsOpen(active)))
			{
				active = active.prevInAEL;
			}
			if (active == null)
			{
				ae.windCount = ae.windDx;
				active = this._actives;
			}
			else if (this._fillrule == FillRule.EvenOdd)
			{
				ae.windCount = ae.windDx;
				ae.windCount2 = active.windCount2;
				active = active.nextInAEL;
			}
			else
			{
				if (active.windCount * active.windDx < 0)
				{
					if (Math.Abs(active.windCount) > 1)
					{
						if (active.windDx * ae.windDx < 0)
						{
							ae.windCount = active.windCount;
						}
						else
						{
							ae.windCount = active.windCount + ae.windDx;
						}
					}
					else
					{
						ae.windCount = (ClipperBase.IsOpen(ae) ? 1 : ae.windDx);
					}
				}
				else if (active.windDx * ae.windDx < 0)
				{
					ae.windCount = active.windCount;
				}
				else
				{
					ae.windCount = active.windCount + ae.windDx;
				}
				ae.windCount2 = active.windCount2;
				active = active.nextInAEL;
			}
			if (this._fillrule == FillRule.EvenOdd)
			{
				while (active != ae)
				{
					if (ClipperBase.GetPolyType(active) != polyType && !ClipperBase.IsOpen(active))
					{
						ae.windCount2 = ((ae.windCount2 == 0) ? 1 : 0);
					}
					active = active.nextInAEL;
				}
				return;
			}
			while (active != ae)
			{
				if (ClipperBase.GetPolyType(active) != polyType && !ClipperBase.IsOpen(active))
				{
					ae.windCount2 += active.windDx;
				}
				active = active.nextInAEL;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006E20 File Offset: 0x00005020
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetWindCountForOpenPathEdge(Active ae)
		{
			Active active = this._actives;
			if (this._fillrule == FillRule.EvenOdd)
			{
				int num = 0;
				int num2 = 0;
				while (active != ae)
				{
					if (ClipperBase.GetPolyType(active) == PathType.Clip)
					{
						num2++;
					}
					else if (!ClipperBase.IsOpen(active))
					{
						num++;
					}
					active = active.nextInAEL;
				}
				ae.windCount = ((ClipperBase.IsOdd(num) > false) ? 1 : 0);
				ae.windCount2 = ((ClipperBase.IsOdd(num2) > false) ? 1 : 0);
				return;
			}
			while (active != ae)
			{
				if (ClipperBase.GetPolyType(active) == PathType.Clip)
				{
					ae.windCount2 += active.windDx;
				}
				else if (!ClipperBase.IsOpen(active))
				{
					ae.windCount += active.windDx;
				}
				active = active.nextInAEL;
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006ED0 File Offset: 0x000050D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsValidAelOrder(Active resident, Active newcomer)
		{
			if (newcomer.curX != resident.curX)
			{
				return newcomer.curX > resident.curX;
			}
			double num = InternalClipper.CrossProduct(resident.top, newcomer.bot, newcomer.top);
			if (num != 0.0)
			{
				return num < 0.0;
			}
			if (!ClipperBase.IsMaxima(resident) && resident.top.Y > newcomer.top.Y)
			{
				return InternalClipper.CrossProduct(newcomer.bot, resident.top, ClipperBase.NextVertex(resident).pt) <= 0.0;
			}
			if (!ClipperBase.IsMaxima(newcomer) && newcomer.top.Y > resident.top.Y)
			{
				return InternalClipper.CrossProduct(newcomer.bot, newcomer.top, ClipperBase.NextVertex(newcomer).pt) >= 0.0;
			}
			long y = newcomer.bot.Y;
			bool isLeftBound = newcomer.isLeftBound;
			if (resident.bot.Y != y || resident.localMin.vertex.pt.Y != y)
			{
				return newcomer.isLeftBound;
			}
			if (resident.isLeftBound != isLeftBound)
			{
				return isLeftBound;
			}
			return InternalClipper.IsCollinear(ClipperBase.PrevPrevVertex(resident).pt, resident.bot, resident.top) || InternalClipper.CrossProduct(ClipperBase.PrevPrevVertex(resident).pt, newcomer.bot, ClipperBase.PrevPrevVertex(newcomer).pt) > 0.0 == isLeftBound;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000705C File Offset: 0x0000525C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void InsertLeftEdge(Active ae)
		{
			if (this._actives == null)
			{
				ae.prevInAEL = null;
				ae.nextInAEL = null;
				this._actives = ae;
				return;
			}
			if (!ClipperBase.IsValidAelOrder(this._actives, ae))
			{
				ae.prevInAEL = null;
				ae.nextInAEL = this._actives;
				this._actives.prevInAEL = ae;
				this._actives = ae;
				return;
			}
			Active active = this._actives;
			while (active.nextInAEL != null && ClipperBase.IsValidAelOrder(active.nextInAEL, ae))
			{
				active = active.nextInAEL;
			}
			if (active.joinWith == JoinWith.Right)
			{
				active = active.nextInAEL;
			}
			ae.nextInAEL = active.nextInAEL;
			if (active.nextInAEL != null)
			{
				active.nextInAEL.prevInAEL = ae;
			}
			ae.prevInAEL = active;
			active.nextInAEL = ae;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007120 File Offset: 0x00005320
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void InsertRightEdge(Active ae, Active ae2)
		{
			ae2.nextInAEL = ae.nextInAEL;
			if (ae.nextInAEL != null)
			{
				ae.nextInAEL.prevInAEL = ae2;
			}
			ae2.prevInAEL = ae;
			ae.nextInAEL = ae2;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007150 File Offset: 0x00005350
		private void InsertLocalMinimaIntoAEL(long botY)
		{
			while (this.HasLocMinAtY(botY))
			{
				LocalMinima localMinima = this.PopLocalMinima();
				Active active;
				if ((localMinima.vertex.flags & VertexFlags.OpenStart) != VertexFlags.None)
				{
					active = null;
				}
				else
				{
					active = this._activeArena.Get();
					active.bot = localMinima.vertex.pt;
					active.curX = localMinima.vertex.pt.X;
					active.windDx = -1;
					active.vertexTop = localMinima.vertex.prev;
					active.top = localMinima.vertex.prev.pt;
					active.outrec = null;
					active.localMin = localMinima;
					ClipperBase.SetDx(active);
				}
				Active active2;
				if ((localMinima.vertex.flags & VertexFlags.OpenEnd) != VertexFlags.None)
				{
					active2 = null;
				}
				else
				{
					active2 = this._activeArena.Get();
					active2.bot = localMinima.vertex.pt;
					active2.curX = localMinima.vertex.pt.X;
					active2.windDx = 1;
					active2.vertexTop = localMinima.vertex.next;
					active2.top = localMinima.vertex.next.pt;
					active2.outrec = null;
					active2.localMin = localMinima;
					ClipperBase.SetDx(active2);
				}
				if (active != null && active2 != null)
				{
					if (ClipperBase.IsHorizontal(active))
					{
						if (ClipperBase.IsHeadingRightHorz(active))
						{
							ClipperBase.SwapActives(ref active, ref active2);
						}
					}
					else if (ClipperBase.IsHorizontal(active2))
					{
						if (ClipperBase.IsHeadingLeftHorz(active2))
						{
							ClipperBase.SwapActives(ref active, ref active2);
						}
					}
					else if (active.dx < active2.dx)
					{
						ClipperBase.SwapActives(ref active, ref active2);
					}
				}
				else if (active == null)
				{
					active = active2;
					active2 = null;
				}
				active.isLeftBound = true;
				this.InsertLeftEdge(active);
				bool flag;
				if (ClipperBase.IsOpen(active))
				{
					this.SetWindCountForOpenPathEdge(active);
					flag = this.IsContributingOpen(active);
				}
				else
				{
					this.SetWindCountForClosedPathEdge(active);
					flag = this.IsContributingClosed(active);
				}
				if (active2 != null)
				{
					active2.windCount = active.windCount;
					active2.windCount2 = active.windCount2;
					ClipperBase.InsertRightEdge(active, active2);
					if (flag)
					{
						this.AddLocalMinPoly(active, active2, active.bot, true);
						if (!ClipperBase.IsHorizontal(active))
						{
							this.CheckJoinLeft(active, active.bot, false);
						}
					}
					while (active2.nextInAEL != null && ClipperBase.IsValidAelOrder(active2.nextInAEL, active2))
					{
						this.IntersectEdges(active2, active2.nextInAEL, active2.bot);
						this.SwapPositionsInAEL(active2, active2.nextInAEL);
					}
					if (ClipperBase.IsHorizontal(active2))
					{
						this.PushHorz(active2);
					}
					else
					{
						this.CheckJoinRight(active2, active2.bot, false);
						this.InsertScanline(active2.top.Y);
					}
				}
				else if (flag)
				{
					this.StartOpenPath(active, active.bot);
				}
				if (ClipperBase.IsHorizontal(active))
				{
					this.PushHorz(active);
				}
				else
				{
					this.InsertScanline(active.top.Y);
				}
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007402 File Offset: 0x00005602
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void PushHorz(Active ae)
		{
			ae.nextInSEL = this._sel;
			this._sel = ae;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00007417 File Offset: 0x00005617
		[NullableContext(2)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool PopHorz(out Active ae)
		{
			ae = this._sel;
			if (this._sel == null)
			{
				return false;
			}
			this._sel = this._sel.nextInSEL;
			return true;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00007440 File Offset: 0x00005640
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private OutPt AddLocalMinPoly(Active ae1, Active ae2, Point64 pt, bool isNew = false)
		{
			OutRec outRec = this.NewOutRec();
			ae1.outrec = outRec;
			ae2.outrec = outRec;
			if (ClipperBase.IsOpen(ae1))
			{
				outRec.owner = null;
				outRec.isOpen = true;
				if (ae1.windDx > 0)
				{
					ClipperBase.SetSides(outRec, ae1, ae2);
				}
				else
				{
					ClipperBase.SetSides(outRec, ae2, ae1);
				}
			}
			else
			{
				outRec.isOpen = false;
				Active prevHotEdge = ClipperBase.GetPrevHotEdge(ae1);
				if (prevHotEdge != null)
				{
					if (this._using_polytree)
					{
						ClipperBase.SetOwner(outRec, prevHotEdge.outrec);
					}
					outRec.owner = prevHotEdge.outrec;
					if (ClipperBase.OutrecIsAscending(prevHotEdge) == isNew)
					{
						ClipperBase.SetSides(outRec, ae2, ae1);
					}
					else
					{
						ClipperBase.SetSides(outRec, ae1, ae2);
					}
				}
				else
				{
					outRec.owner = null;
					if (isNew)
					{
						ClipperBase.SetSides(outRec, ae1, ae2);
					}
					else
					{
						ClipperBase.SetSides(outRec, ae2, ae1);
					}
				}
			}
			OutPt outPt = new OutPt(pt, outRec);
			outRec.pts = outPt;
			return outPt;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007514 File Offset: 0x00005714
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private OutPt AddLocalMaxPoly(Active ae1, Active ae2, Point64 pt)
		{
			if (ClipperBase.IsJoined(ae1))
			{
				this.Split(ae1, pt);
			}
			if (ClipperBase.IsJoined(ae2))
			{
				this.Split(ae2, pt);
			}
			if (ClipperBase.IsFront(ae1) == ClipperBase.IsFront(ae2))
			{
				if (ClipperBase.IsOpenEnd(ae1))
				{
					ClipperBase.SwapFrontBackSides(ae1.outrec);
				}
				else
				{
					if (!ClipperBase.IsOpenEnd(ae2))
					{
						this._succeeded = false;
						return null;
					}
					ClipperBase.SwapFrontBackSides(ae2.outrec);
				}
			}
			OutPt outPt = ClipperBase.AddOutPt(ae1, pt);
			if (ae1.outrec == ae2.outrec)
			{
				OutRec outrec = ae1.outrec;
				outrec.pts = outPt;
				if (this._using_polytree)
				{
					Active prevHotEdge = ClipperBase.GetPrevHotEdge(ae1);
					if (prevHotEdge == null)
					{
						outrec.owner = null;
					}
					else
					{
						ClipperBase.SetOwner(outrec, prevHotEdge.outrec);
					}
				}
				ClipperBase.UncoupleOutRec(ae1);
			}
			else if (ClipperBase.IsOpen(ae1))
			{
				if (ae1.windDx < 0)
				{
					ClipperBase.JoinOutrecPaths(ae1, ae2);
				}
				else
				{
					ClipperBase.JoinOutrecPaths(ae2, ae1);
				}
			}
			else if (ae1.outrec.idx < ae2.outrec.idx)
			{
				ClipperBase.JoinOutrecPaths(ae1, ae2);
			}
			else
			{
				ClipperBase.JoinOutrecPaths(ae2, ae1);
			}
			return outPt;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007624 File Offset: 0x00005824
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void JoinOutrecPaths(Active ae1, Active ae2)
		{
			OutPt pts = ae1.outrec.pts;
			OutPt pts2 = ae2.outrec.pts;
			OutPt next = pts.next;
			OutPt next2 = pts2.next;
			if (ClipperBase.IsFront(ae1))
			{
				next2.prev = pts;
				pts.next = next2;
				pts2.next = next;
				next.prev = pts2;
				ae1.outrec.pts = pts2;
				ae1.outrec.frontEdge = ae2.outrec.frontEdge;
				if (ae1.outrec.frontEdge != null)
				{
					ae1.outrec.frontEdge.outrec = ae1.outrec;
				}
			}
			else
			{
				next.prev = pts2;
				pts2.next = next;
				pts.next = next2;
				next2.prev = pts;
				ae1.outrec.backEdge = ae2.outrec.backEdge;
				if (ae1.outrec.backEdge != null)
				{
					ae1.outrec.backEdge.outrec = ae1.outrec;
				}
			}
			ae2.outrec.frontEdge = null;
			ae2.outrec.backEdge = null;
			ae2.outrec.pts = null;
			ClipperBase.SetOwner(ae2.outrec, ae1.outrec);
			if (ClipperBase.IsOpenEnd(ae1))
			{
				ae2.outrec.pts = ae1.outrec.pts;
				ae1.outrec.pts = null;
			}
			ae1.outrec = null;
			ae2.outrec = null;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007784 File Offset: 0x00005984
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static OutPt AddOutPt(Active ae, Point64 pt)
		{
			OutRec outrec = ae.outrec;
			bool flag = ClipperBase.IsFront(ae);
			OutPt pts = outrec.pts;
			OutPt next = pts.next;
			if (flag && pt == pts.pt)
			{
				return pts;
			}
			if (!flag && pt == next.pt)
			{
				return next;
			}
			OutPt outPt = new OutPt(pt, outrec);
			next.prev = outPt;
			outPt.prev = pts;
			outPt.next = next;
			pts.next = outPt;
			if (flag)
			{
				outrec.pts = outPt;
			}
			return outPt;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000780C File Offset: 0x00005A0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private OutRec NewOutRec()
		{
			OutRec outRec = new OutRec
			{
				idx = this._outrecList.Count
			};
			this._outrecList.Add(outRec);
			return outRec;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007840 File Offset: 0x00005A40
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private OutPt StartOpenPath(Active ae, Point64 pt)
		{
			OutRec outRec = this.NewOutRec();
			outRec.isOpen = true;
			if (ae.windDx > 0)
			{
				outRec.frontEdge = ae;
				outRec.backEdge = null;
			}
			else
			{
				outRec.frontEdge = null;
				outRec.backEdge = ae;
			}
			ae.outrec = outRec;
			OutPt outPt = new OutPt(pt, outRec);
			outRec.pts = outPt;
			return outPt;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000789C File Offset: 0x00005A9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void UpdateEdgeIntoAEL(Active ae)
		{
			ae.bot = ae.top;
			ae.vertexTop = ClipperBase.NextVertex(ae);
			ae.top = ae.vertexTop.pt;
			ae.curX = ae.bot.X;
			ClipperBase.SetDx(ae);
			if (ClipperBase.IsJoined(ae))
			{
				this.Split(ae, ae.bot);
			}
			if (ClipperBase.IsHorizontal(ae))
			{
				if (!ClipperBase.IsOpen(ae))
				{
					ClipperBase.TrimHorz(ae, this.PreserveCollinear);
				}
				return;
			}
			this.InsertScanline(ae.top.Y);
			this.CheckJoinLeft(ae, ae.bot, false);
			this.CheckJoinRight(ae, ae.bot, true);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007948 File Offset: 0x00005B48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private static Active FindEdgeWithMatchingLocMin(Active e)
		{
			Active active = e.nextInAEL;
			while (active != null)
			{
				if (active.localMin == e.localMin)
				{
					return active;
				}
				if (!ClipperBase.IsHorizontal(active) && e.bot != active.bot)
				{
					active = null;
				}
				else
				{
					active = active.nextInAEL;
				}
			}
			for (active = e.prevInAEL; active != null; active = active.prevInAEL)
			{
				if (active.localMin == e.localMin)
				{
					return active;
				}
				if (!ClipperBase.IsHorizontal(active) && e.bot != active.bot)
				{
					return null;
				}
			}
			return active;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000079E4 File Offset: 0x00005BE4
		private void IntersectEdges(Active ae1, Active ae2, Point64 pt)
		{
			if (this._hasOpenPaths && (ClipperBase.IsOpen(ae1) || ClipperBase.IsOpen(ae2)))
			{
				if (ClipperBase.IsOpen(ae1) && ClipperBase.IsOpen(ae2))
				{
					return;
				}
				if (ClipperBase.IsOpen(ae2))
				{
					ClipperBase.SwapActives(ref ae1, ref ae2);
				}
				if (ClipperBase.IsJoined(ae2))
				{
					this.Split(ae2, pt);
				}
				if (this._cliptype == ClipType.Union)
				{
					if (!ClipperBase.IsHotEdge(ae2))
					{
						return;
					}
				}
				else if (ae2.localMin.polytype == PathType.Subject)
				{
					return;
				}
				FillRule fillrule = this._fillrule;
				if (fillrule != FillRule.Positive)
				{
					if (fillrule != FillRule.Negative)
					{
						if (Math.Abs(ae2.windCount) != 1)
						{
							return;
						}
					}
					else if (ae2.windCount != -1)
					{
						return;
					}
				}
				else if (ae2.windCount != 1)
				{
					return;
				}
				if (ClipperBase.IsHotEdge(ae1))
				{
					ClipperBase.AddOutPt(ae1, pt);
					if (ClipperBase.IsFront(ae1))
					{
						ae1.outrec.frontEdge = null;
					}
					else
					{
						ae1.outrec.backEdge = null;
					}
					ae1.outrec = null;
					return;
				}
				if (!(pt == ae1.localMin.vertex.pt) || ClipperBase.IsOpenEnd(ae1.localMin.vertex))
				{
					this.StartOpenPath(ae1, pt);
					return;
				}
				Active active = ClipperBase.FindEdgeWithMatchingLocMin(ae1);
				if (active == null || !ClipperBase.IsHotEdge(active))
				{
					this.StartOpenPath(ae1, pt);
					return;
				}
				ae1.outrec = active.outrec;
				if (ae1.windDx > 0)
				{
					ClipperBase.SetSides(active.outrec, ae1, active);
					return;
				}
				ClipperBase.SetSides(active.outrec, active, ae1);
				return;
			}
			else
			{
				if (ClipperBase.IsJoined(ae1))
				{
					this.Split(ae1, pt);
				}
				if (ClipperBase.IsJoined(ae2))
				{
					this.Split(ae2, pt);
				}
				int num;
				if (ae1.localMin.polytype == ae2.localMin.polytype)
				{
					if (this._fillrule == FillRule.EvenOdd)
					{
						num = ae1.windCount;
						ae1.windCount = ae2.windCount;
						ae2.windCount = num;
					}
					else
					{
						if (ae1.windCount + ae2.windDx == 0)
						{
							ae1.windCount = -ae1.windCount;
						}
						else
						{
							ae1.windCount += ae2.windDx;
						}
						if (ae2.windCount - ae1.windDx == 0)
						{
							ae2.windCount = -ae2.windCount;
						}
						else
						{
							ae2.windCount -= ae1.windDx;
						}
					}
				}
				else
				{
					if (this._fillrule != FillRule.EvenOdd)
					{
						ae1.windCount2 += ae2.windDx;
					}
					else
					{
						ae1.windCount2 = ((ae1.windCount2 == 0) ? 1 : 0);
					}
					if (this._fillrule != FillRule.EvenOdd)
					{
						ae2.windCount2 -= ae1.windDx;
					}
					else
					{
						ae2.windCount2 = ((ae2.windCount2 == 0) ? 1 : 0);
					}
				}
				FillRule fillrule = this._fillrule;
				int num2;
				if (fillrule != FillRule.Positive)
				{
					if (fillrule != FillRule.Negative)
					{
						num = Math.Abs(ae1.windCount);
						num2 = Math.Abs(ae2.windCount);
					}
					else
					{
						num = -ae1.windCount;
						num2 = -ae2.windCount;
					}
				}
				else
				{
					num = ae1.windCount;
					num2 = ae2.windCount;
				}
				bool flag = num == 0 || num == 1;
				bool flag2 = num2 == 0 || num2 == 1;
				if ((!ClipperBase.IsHotEdge(ae1) && !flag) || (!ClipperBase.IsHotEdge(ae2) && !flag2))
				{
					return;
				}
				if (ClipperBase.IsHotEdge(ae1) && ClipperBase.IsHotEdge(ae2))
				{
					if ((num != 0 && num != 1) || (num2 != 0 && num2 != 1) || (ae1.localMin.polytype != ae2.localMin.polytype && this._cliptype != ClipType.Xor))
					{
						this.AddLocalMaxPoly(ae1, ae2, pt);
						return;
					}
					if (ClipperBase.IsFront(ae1) || ae1.outrec == ae2.outrec)
					{
						this.AddLocalMaxPoly(ae1, ae2, pt);
						this.AddLocalMinPoly(ae1, ae2, pt, false);
						return;
					}
					ClipperBase.AddOutPt(ae1, pt);
					ClipperBase.AddOutPt(ae2, pt);
					ClipperBase.SwapOutrecs(ae1, ae2);
					return;
				}
				else
				{
					if (ClipperBase.IsHotEdge(ae1))
					{
						ClipperBase.AddOutPt(ae1, pt);
						ClipperBase.SwapOutrecs(ae1, ae2);
						return;
					}
					if (ClipperBase.IsHotEdge(ae2))
					{
						ClipperBase.AddOutPt(ae2, pt);
						ClipperBase.SwapOutrecs(ae1, ae2);
						return;
					}
					fillrule = this._fillrule;
					long num3;
					long num4;
					if (fillrule != FillRule.Positive)
					{
						if (fillrule != FillRule.Negative)
						{
							num3 = (long)Math.Abs(ae1.windCount2);
							num4 = (long)Math.Abs(ae2.windCount2);
						}
						else
						{
							num3 = (long)(-(long)ae1.windCount2);
							num4 = (long)(-(long)ae2.windCount2);
						}
					}
					else
					{
						num3 = (long)ae1.windCount2;
						num4 = (long)ae2.windCount2;
					}
					if (!ClipperBase.IsSamePolyType(ae1, ae2))
					{
						this.AddLocalMinPoly(ae1, ae2, pt, false);
						return;
					}
					if (num == 1 && num2 == 1)
					{
						switch (this._cliptype)
						{
						case ClipType.Union:
							if (num3 > 0L && num4 > 0L)
							{
								return;
							}
							this.AddLocalMinPoly(ae1, ae2, pt, false);
							return;
						case ClipType.Difference:
							if ((ClipperBase.GetPolyType(ae1) == PathType.Clip && num3 > 0L && num4 > 0L) || (ClipperBase.GetPolyType(ae1) == PathType.Subject && num3 <= 0L && num4 <= 0L))
							{
								this.AddLocalMinPoly(ae1, ae2, pt, false);
								return;
							}
							break;
						case ClipType.Xor:
							this.AddLocalMinPoly(ae1, ae2, pt, false);
							return;
						default:
							if (num3 <= 0L || num4 <= 0L)
							{
								return;
							}
							this.AddLocalMinPoly(ae1, ae2, pt, false);
							break;
						}
					}
					return;
				}
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007EC4 File Offset: 0x000060C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DeleteFromAEL(Active ae)
		{
			Active prevInAEL = ae.prevInAEL;
			Active nextInAEL = ae.nextInAEL;
			if (prevInAEL == null && nextInAEL == null && ae != this._actives)
			{
				return;
			}
			if (prevInAEL != null)
			{
				prevInAEL.nextInAEL = nextInAEL;
			}
			else
			{
				this._actives = nextInAEL;
			}
			if (nextInAEL != null)
			{
				nextInAEL.prevInAEL = prevInAEL;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00007F0C File Offset: 0x0000610C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AdjustCurrXAndCopyToSEL(long topY)
		{
			Active active = this._actives;
			this._sel = active;
			while (active != null)
			{
				active.prevInSEL = active.prevInAEL;
				active.nextInSEL = active.nextInAEL;
				active.jump = active.nextInSEL;
				if (active.joinWith == JoinWith.Left)
				{
					active.curX = active.prevInAEL.curX;
				}
				else
				{
					active.curX = ClipperBase.TopX(active, topY);
				}
				active = active.nextInAEL;
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007F80 File Offset: 0x00006180
		protected void ExecuteInternal(ClipType ct, FillRule fillRule)
		{
			if (ct == ClipType.None)
			{
				return;
			}
			this._fillrule = fillRule;
			this._cliptype = ct;
			this.Reset();
			long num;
			if (!this.PopScanline(out num))
			{
				return;
			}
			while (this._succeeded)
			{
				this.InsertLocalMinimaIntoAEL(num);
				Active horz;
				while (this.PopHorz(out horz))
				{
					this.DoHorizontal(horz);
				}
				if (this._horzSegList.Count > 0)
				{
					this.ConvertHorzSegsToJoins();
					this._horzSegList.Clear();
				}
				this._currentBotY = num;
				if (!this.PopScanline(out num))
				{
					break;
				}
				this.DoIntersections(num);
				this.DoTopOfScanbeam(num);
				while (this.PopHorz(out horz))
				{
					this.DoHorizontal(horz);
				}
			}
			if (this._succeeded)
			{
				this.ProcessHorzJoins();
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00008031 File Offset: 0x00006231
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoIntersections(long topY)
		{
			if (this.BuildIntersectList(topY))
			{
				this.ProcessIntersectList();
				this.DisposeIntersectNodes();
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008048 File Offset: 0x00006248
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DisposeIntersectNodes()
		{
			this._intersectList.Clear();
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00008058 File Offset: 0x00006258
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddNewIntersectNode(Active ae1, Active ae2, long topY)
		{
			Point64 closestPtOnSegment;
			if (!InternalClipper.GetSegmentIntersectPt(ae1.bot, ae1.top, ae2.bot, ae2.top, out closestPtOnSegment))
			{
				closestPtOnSegment = new Point64(ae1.curX, topY);
			}
			if (closestPtOnSegment.Y > this._currentBotY || closestPtOnSegment.Y < topY)
			{
				double num = Math.Abs(ae1.dx);
				double num2 = Math.Abs(ae2.dx);
				if (num > 100.0 && num2 > 100.0)
				{
					if (num > num2)
					{
						closestPtOnSegment = InternalClipper.GetClosestPtOnSegment(closestPtOnSegment, ae1.bot, ae1.top);
					}
					else
					{
						closestPtOnSegment = InternalClipper.GetClosestPtOnSegment(closestPtOnSegment, ae2.bot, ae2.top);
					}
				}
				else if (num > 100.0)
				{
					closestPtOnSegment = InternalClipper.GetClosestPtOnSegment(closestPtOnSegment, ae1.bot, ae1.top);
				}
				else if (num2 > 100.0)
				{
					closestPtOnSegment = InternalClipper.GetClosestPtOnSegment(closestPtOnSegment, ae2.bot, ae2.top);
				}
				else
				{
					if (closestPtOnSegment.Y < topY)
					{
						closestPtOnSegment.Y = topY;
					}
					else
					{
						closestPtOnSegment.Y = this._currentBotY;
					}
					if (num < num2)
					{
						closestPtOnSegment.X = ClipperBase.TopX(ae1, closestPtOnSegment.Y);
					}
					else
					{
						closestPtOnSegment.X = ClipperBase.TopX(ae2, closestPtOnSegment.Y);
					}
				}
			}
			IntersectNode item = new IntersectNode(closestPtOnSegment, ae1, ae2);
			this._intersectList.Add(item);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000081B8 File Offset: 0x000063B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private static Active ExtractFromSEL(Active ae)
		{
			Active nextInSEL = ae.nextInSEL;
			if (nextInSEL != null)
			{
				nextInSEL.prevInSEL = ae.prevInSEL;
			}
			ae.prevInSEL.nextInSEL = nextInSEL;
			return nextInSEL;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000081E8 File Offset: 0x000063E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Insert1Before2InSEL(Active ae1, Active ae2)
		{
			ae1.prevInSEL = ae2.prevInSEL;
			if (ae1.prevInSEL != null)
			{
				ae1.prevInSEL.nextInSEL = ae1;
			}
			ae1.nextInSEL = ae2;
			ae2.prevInSEL = ae1;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008218 File Offset: 0x00006418
		private bool BuildIntersectList(long topY)
		{
			if (this._actives == null || this._actives.nextInAEL == null)
			{
				return false;
			}
			this.AdjustCurrXAndCopyToSEL(topY);
			Active active = this._sel;
			while (active.jump != null)
			{
				Active active2 = null;
				while (active != null && active.jump != null)
				{
					Active active3 = active;
					Active active4 = active.jump;
					Active active5 = active4;
					Active jump = active4.jump;
					active.jump = jump;
					while (active != active5 && active4 != jump)
					{
						if (active4.curX < active.curX)
						{
							Active active6 = active4.prevInSEL;
							for (;;)
							{
								this.AddNewIntersectNode(active6, active4, topY);
								if (active6 == active)
								{
									break;
								}
								active6 = active6.prevInSEL;
							}
							active6 = active4;
							active4 = ClipperBase.ExtractFromSEL(active6);
							active5 = active4;
							ClipperBase.Insert1Before2InSEL(active6, active);
							if (active == active3)
							{
								active3 = active6;
								active3.jump = jump;
								if (active2 == null)
								{
									this._sel = active3;
								}
								else
								{
									active2.jump = active3;
								}
							}
						}
						else
						{
							active = active.nextInSEL;
						}
					}
					active2 = active3;
					active = jump;
				}
				active = this._sel;
			}
			return this._intersectList.Count > 0;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008328 File Offset: 0x00006528
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ProcessIntersectList()
		{
			this._intersectList.Sort(default(ClipperBase.IntersectListSort));
			for (int i = 0; i < this._intersectList.Count; i++)
			{
				if (!ClipperBase.EdgesAdjacentInAEL(this._intersectList[i]))
				{
					int num = i + 1;
					while (!ClipperBase.EdgesAdjacentInAEL(this._intersectList[num]))
					{
						num++;
					}
					IntersectNode value = this._intersectList[i];
					this._intersectList[i] = this._intersectList[num];
					this._intersectList[num] = value;
				}
				IntersectNode intersectNode = this._intersectList[i];
				this.IntersectEdges(intersectNode.edge1, intersectNode.edge2, intersectNode.pt);
				this.SwapPositionsInAEL(intersectNode.edge1, intersectNode.edge2);
				intersectNode.edge1.curX = intersectNode.pt.X;
				intersectNode.edge2.curX = intersectNode.pt.X;
				this.CheckJoinLeft(intersectNode.edge2, intersectNode.pt, true);
				this.CheckJoinRight(intersectNode.edge1, intersectNode.pt, true);
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00008458 File Offset: 0x00006658
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SwapPositionsInAEL(Active ae1, Active ae2)
		{
			Active nextInAEL = ae2.nextInAEL;
			if (nextInAEL != null)
			{
				nextInAEL.prevInAEL = ae1;
			}
			Active prevInAEL = ae1.prevInAEL;
			if (prevInAEL != null)
			{
				prevInAEL.nextInAEL = ae2;
			}
			ae2.prevInAEL = prevInAEL;
			ae2.nextInAEL = ae1;
			ae1.prevInAEL = ae2;
			ae1.nextInAEL = nextInAEL;
			if (ae2.prevInAEL == null)
			{
				this._actives = ae2;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000084B4 File Offset: 0x000066B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool ResetHorzDirection(Active horz, [Nullable(2)] Vertex vertexMax, out long leftX, out long rightX)
		{
			if (horz.bot.X == horz.top.X)
			{
				leftX = horz.curX;
				rightX = horz.curX;
				Active nextInAEL = horz.nextInAEL;
				while (nextInAEL != null && nextInAEL.vertexTop != vertexMax)
				{
					nextInAEL = nextInAEL.nextInAEL;
				}
				return nextInAEL != null;
			}
			if (horz.curX < horz.top.X)
			{
				leftX = horz.curX;
				rightX = horz.top.X;
				return true;
			}
			leftX = horz.top.X;
			rightX = horz.curX;
			return false;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000854C File Offset: 0x0000674C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void TrimHorz(Active horzEdge, bool preserveCollinear)
		{
			bool flag = false;
			Point64 pt = ClipperBase.NextVertex(horzEdge).pt;
			while (pt.Y == horzEdge.top.Y && (!preserveCollinear || pt.X < horzEdge.top.X == horzEdge.bot.X < horzEdge.top.X))
			{
				horzEdge.vertexTop = ClipperBase.NextVertex(horzEdge);
				horzEdge.top = pt;
				flag = true;
				if (ClipperBase.IsMaxima(horzEdge))
				{
					break;
				}
				pt = ClipperBase.NextVertex(horzEdge).pt;
			}
			if (flag)
			{
				ClipperBase.SetDx(horzEdge);
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000085DE File Offset: 0x000067DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddToHorzSegList(OutPt op)
		{
			if (op.outrec.isOpen)
			{
				return;
			}
			this._horzSegList.Add(new HorzSegment(op));
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00008600 File Offset: 0x00006800
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private OutPt GetLastOp(Active hotEdge)
		{
			OutRec outrec = hotEdge.outrec;
			if (hotEdge != outrec.frontEdge)
			{
				return outrec.pts.next;
			}
			return outrec.pts;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00008630 File Offset: 0x00006830
		private void DoHorizontal(Active horz)
		{
			bool flag = ClipperBase.IsOpen(horz);
			long y = horz.bot.Y;
			Vertex vertex = flag ? ClipperBase.GetCurrYMaximaVertex_Open(horz) : ClipperBase.GetCurrYMaximaVertex(horz);
			long num;
			long num2;
			bool flag2 = ClipperBase.ResetHorzDirection(horz, vertex, out num, out num2);
			if (ClipperBase.IsHotEdge(horz))
			{
				OutPt op = ClipperBase.AddOutPt(horz, new Point64(horz.curX, y));
				this.AddToHorzSegList(op);
			}
			Active active;
			for (;;)
			{
				active = (flag2 ? horz.nextInAEL : horz.prevInAEL);
				while (active != null)
				{
					if (active.vertexTop == vertex)
					{
						goto Block_4;
					}
					Point64 pt;
					if (vertex != horz.vertexTop || ClipperBase.IsOpenEnd(horz))
					{
						if ((flag2 && active.curX > num2) || (!flag2 && active.curX < num))
						{
							break;
						}
						if (active.curX == horz.top.X && !ClipperBase.IsHorizontal(active))
						{
							pt = ClipperBase.NextVertex(horz).pt;
							if (ClipperBase.IsOpen(active) && !ClipperBase.IsSamePolyType(active, horz) && !ClipperBase.IsHotEdge(active))
							{
								if (flag2 && ClipperBase.TopX(active, pt.Y) > pt.X)
								{
									break;
								}
								if (!flag2 && ClipperBase.TopX(active, pt.Y) < pt.X)
								{
									break;
								}
							}
							else if ((flag2 && ClipperBase.TopX(active, pt.Y) >= pt.X) || (!flag2 && ClipperBase.TopX(active, pt.Y) <= pt.X))
							{
								break;
							}
						}
					}
					pt = new Point64(active.curX, y);
					if (flag2)
					{
						this.IntersectEdges(horz, active, pt);
						this.SwapPositionsInAEL(horz, active);
						this.CheckJoinLeft(active, pt, false);
						horz.curX = active.curX;
						active = horz.nextInAEL;
					}
					else
					{
						this.IntersectEdges(active, horz, pt);
						this.SwapPositionsInAEL(active, horz);
						this.CheckJoinRight(active, pt, false);
						horz.curX = active.curX;
						active = horz.prevInAEL;
					}
					if (ClipperBase.IsHotEdge(horz))
					{
						this.AddToHorzSegList(this.GetLastOp(horz));
					}
				}
				if (flag && ClipperBase.IsOpenEnd(horz))
				{
					goto Block_26;
				}
				if (ClipperBase.NextVertex(horz).pt.Y != horz.top.Y)
				{
					goto IL_31C;
				}
				if (ClipperBase.IsHotEdge(horz))
				{
					ClipperBase.AddOutPt(horz, horz.top);
				}
				this.UpdateEdgeIntoAEL(horz);
				flag2 = ClipperBase.ResetHorzDirection(horz, vertex, out num, out num2);
			}
			Block_4:
			if (ClipperBase.IsHotEdge(horz) && ClipperBase.IsJoined(active))
			{
				this.Split(active, active.top);
			}
			if (ClipperBase.IsHotEdge(horz))
			{
				while (horz.vertexTop != vertex)
				{
					ClipperBase.AddOutPt(horz, horz.top);
					this.UpdateEdgeIntoAEL(horz);
				}
				if (flag2)
				{
					this.AddLocalMaxPoly(horz, active, horz.top);
				}
				else
				{
					this.AddLocalMaxPoly(active, horz, horz.top);
				}
			}
			this.DeleteFromAEL(active);
			this.DeleteFromAEL(horz);
			return;
			Block_26:
			if (ClipperBase.IsHotEdge(horz))
			{
				ClipperBase.AddOutPt(horz, horz.top);
				if (ClipperBase.IsFront(horz))
				{
					horz.outrec.frontEdge = null;
				}
				else
				{
					horz.outrec.backEdge = null;
				}
				horz.outrec = null;
			}
			this.DeleteFromAEL(horz);
			return;
			IL_31C:
			if (ClipperBase.IsHotEdge(horz))
			{
				OutPt op2 = ClipperBase.AddOutPt(horz, horz.top);
				this.AddToHorzSegList(op2);
			}
			this.UpdateEdgeIntoAEL(horz);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008980 File Offset: 0x00006B80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoTopOfScanbeam(long y)
		{
			this._sel = null;
			for (Active active = this._actives; active != null; active = active.nextInAEL)
			{
				if (active.top.Y == y)
				{
					active.curX = active.top.X;
					if (ClipperBase.IsMaxima(active))
					{
						active = this.DoMaxima(active);
						continue;
					}
					if (ClipperBase.IsHotEdge(active))
					{
						ClipperBase.AddOutPt(active, active.top);
					}
					this.UpdateEdgeIntoAEL(active);
					if (ClipperBase.IsHorizontal(active))
					{
						this.PushHorz(active);
					}
				}
				else
				{
					active.curX = ClipperBase.TopX(active, y);
				}
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008A14 File Offset: 0x00006C14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private Active DoMaxima(Active ae)
		{
			Active prevInAEL = ae.prevInAEL;
			Active nextInAEL = ae.nextInAEL;
			if (ClipperBase.IsOpenEnd(ae))
			{
				if (ClipperBase.IsHotEdge(ae))
				{
					ClipperBase.AddOutPt(ae, ae.top);
				}
				if (!ClipperBase.IsHorizontal(ae))
				{
					if (ClipperBase.IsHotEdge(ae))
					{
						if (ClipperBase.IsFront(ae))
						{
							ae.outrec.frontEdge = null;
						}
						else
						{
							ae.outrec.backEdge = null;
						}
						ae.outrec = null;
					}
					this.DeleteFromAEL(ae);
				}
				return nextInAEL;
			}
			Active maximaPair = ClipperBase.GetMaximaPair(ae);
			if (maximaPair == null)
			{
				return nextInAEL;
			}
			if (ClipperBase.IsJoined(ae))
			{
				this.Split(ae, ae.top);
			}
			if (ClipperBase.IsJoined(maximaPair))
			{
				this.Split(maximaPair, maximaPair.top);
			}
			while (nextInAEL != maximaPair)
			{
				this.IntersectEdges(ae, nextInAEL, ae.top);
				this.SwapPositionsInAEL(ae, nextInAEL);
				nextInAEL = ae.nextInAEL;
			}
			if (ClipperBase.IsOpen(ae))
			{
				if (ClipperBase.IsHotEdge(ae))
				{
					this.AddLocalMaxPoly(ae, maximaPair, ae.top);
				}
				this.DeleteFromAEL(maximaPair);
				this.DeleteFromAEL(ae);
				if (prevInAEL == null)
				{
					return this._actives;
				}
				return prevInAEL.nextInAEL;
			}
			else
			{
				if (ClipperBase.IsHotEdge(ae))
				{
					this.AddLocalMaxPoly(ae, maximaPair, ae.top);
				}
				this.DeleteFromAEL(ae);
				this.DeleteFromAEL(maximaPair);
				if (prevInAEL == null)
				{
					return this._actives;
				}
				return prevInAEL.nextInAEL;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008B5A File Offset: 0x00006D5A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsJoined(Active e)
		{
			return e.joinWith > JoinWith.None;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008B68 File Offset: 0x00006D68
		private void Split(Active e, Point64 currPt)
		{
			if (e.joinWith == JoinWith.Right)
			{
				e.joinWith = JoinWith.None;
				e.nextInAEL.joinWith = JoinWith.None;
				this.AddLocalMinPoly(e, e.nextInAEL, currPt, true);
				return;
			}
			e.joinWith = JoinWith.None;
			e.prevInAEL.joinWith = JoinWith.None;
			this.AddLocalMinPoly(e.prevInAEL, e, currPt, true);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00008BC8 File Offset: 0x00006DC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckJoinLeft(Active e, Point64 pt, bool checkCurrX = false)
		{
			Active prevInAEL = e.prevInAEL;
			if (prevInAEL == null || !ClipperBase.IsHotEdge(e) || !ClipperBase.IsHotEdge(prevInAEL) || ClipperBase.IsHorizontal(e) || ClipperBase.IsHorizontal(prevInAEL) || ClipperBase.IsOpen(e) || ClipperBase.IsOpen(prevInAEL))
			{
				return;
			}
			if ((pt.Y < e.top.Y + 2L || pt.Y < prevInAEL.top.Y + 2L) && (e.bot.Y > pt.Y || prevInAEL.bot.Y > pt.Y))
			{
				return;
			}
			if (checkCurrX)
			{
				if (Clipper.PerpendicDistFromLineSqrd(pt, prevInAEL.bot, prevInAEL.top) > 0.25)
				{
					return;
				}
			}
			else if (e.curX != prevInAEL.curX)
			{
				return;
			}
			if (!InternalClipper.IsCollinear(e.top, pt, prevInAEL.top))
			{
				return;
			}
			if (e.outrec.idx == prevInAEL.outrec.idx)
			{
				this.AddLocalMaxPoly(prevInAEL, e, pt);
			}
			else if (e.outrec.idx < prevInAEL.outrec.idx)
			{
				ClipperBase.JoinOutrecPaths(e, prevInAEL);
			}
			else
			{
				ClipperBase.JoinOutrecPaths(prevInAEL, e);
			}
			prevInAEL.joinWith = JoinWith.Right;
			e.joinWith = JoinWith.Left;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00008D04 File Offset: 0x00006F04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckJoinRight(Active e, Point64 pt, bool checkCurrX = false)
		{
			Active nextInAEL = e.nextInAEL;
			if (nextInAEL == null || !ClipperBase.IsHotEdge(e) || !ClipperBase.IsHotEdge(nextInAEL) || ClipperBase.IsHorizontal(e) || ClipperBase.IsHorizontal(nextInAEL) || ClipperBase.IsOpen(e) || ClipperBase.IsOpen(nextInAEL))
			{
				return;
			}
			if ((pt.Y < e.top.Y + 2L || pt.Y < nextInAEL.top.Y + 2L) && (e.bot.Y > pt.Y || nextInAEL.bot.Y > pt.Y))
			{
				return;
			}
			if (checkCurrX)
			{
				if (Clipper.PerpendicDistFromLineSqrd(pt, nextInAEL.bot, nextInAEL.top) > 0.25)
				{
					return;
				}
			}
			else if (e.curX != nextInAEL.curX)
			{
				return;
			}
			if (!InternalClipper.IsCollinear(e.top, pt, nextInAEL.top))
			{
				return;
			}
			if (e.outrec.idx == nextInAEL.outrec.idx)
			{
				this.AddLocalMaxPoly(e, nextInAEL, pt);
			}
			else if (e.outrec.idx < nextInAEL.outrec.idx)
			{
				ClipperBase.JoinOutrecPaths(e, nextInAEL);
			}
			else
			{
				ClipperBase.JoinOutrecPaths(nextInAEL, e);
			}
			e.joinWith = JoinWith.Right;
			nextInAEL.joinWith = JoinWith.Left;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00008E40 File Offset: 0x00007040
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void FixOutRecPts(OutRec outrec)
		{
			OutPt outPt = outrec.pts;
			do
			{
				outPt.outrec = outrec;
				outPt = outPt.next;
			}
			while (outPt != outrec.pts);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00008E6C File Offset: 0x0000706C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool SetHorzSegHeadingForward(HorzSegment hs, OutPt opP, OutPt opN)
		{
			if (opP.pt.X == opN.pt.X)
			{
				return false;
			}
			if (opP.pt.X < opN.pt.X)
			{
				hs.leftOp = opP;
				hs.rightOp = opN;
				hs.leftToRight = true;
			}
			else
			{
				hs.leftOp = opN;
				hs.rightOp = opP;
				hs.leftToRight = false;
			}
			return true;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00008ED8 File Offset: 0x000070D8
		private static bool UpdateHorzSegment(HorzSegment hs)
		{
			OutPt leftOp = hs.leftOp;
			OutRec realOutRec = ClipperBase.GetRealOutRec(leftOp.outrec);
			bool flag = realOutRec.frontEdge != null;
			long y = leftOp.pt.Y;
			OutPt outPt = leftOp;
			OutPt outPt2 = leftOp;
			if (flag)
			{
				OutPt pts = realOutRec.pts;
				OutPt next = pts.next;
				while (outPt != next)
				{
					if (outPt.prev.pt.Y != y)
					{
						break;
					}
					outPt = outPt.prev;
				}
				while (outPt2 != pts)
				{
					if (outPt2.next.pt.Y != y)
					{
						break;
					}
					outPt2 = outPt2.next;
				}
			}
			else
			{
				while (outPt.prev != outPt2)
				{
					if (outPt.prev.pt.Y != y)
					{
						break;
					}
					outPt = outPt.prev;
				}
				while (outPt2.next != outPt && outPt2.next.pt.Y == y)
				{
					outPt2 = outPt2.next;
				}
			}
			bool flag2 = ClipperBase.SetHorzSegHeadingForward(hs, outPt, outPt2) && hs.leftOp.horz == null;
			if (flag2)
			{
				hs.leftOp.horz = hs;
				return flag2;
			}
			hs.rightOp = null;
			return flag2;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00008FF0 File Offset: 0x000071F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static OutPt DuplicateOp(OutPt op, bool insert_after)
		{
			OutPt outPt = new OutPt(op.pt, op.outrec);
			if (insert_after)
			{
				outPt.next = op.next;
				outPt.next.prev = outPt;
				outPt.prev = op;
				op.next = outPt;
			}
			else
			{
				outPt.prev = op.prev;
				outPt.prev.next = outPt;
				outPt.next = op;
				op.prev = outPt;
			}
			return outPt;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00009064 File Offset: 0x00007264
		[NullableContext(2)]
		private int HorzSegSort(HorzSegment hs1, HorzSegment hs2)
		{
			if (hs1 == null || hs2 == null)
			{
				return 0;
			}
			if (hs1.rightOp == null)
			{
				return (hs2.rightOp != null) ? 1 : 0;
			}
			if (hs2.rightOp == null)
			{
				return -1;
			}
			return hs1.leftOp.pt.X.CompareTo(hs2.leftOp.pt.X);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000090BC File Offset: 0x000072BC
		private void ConvertHorzSegsToJoins()
		{
			int num = 0;
			using (List<HorzSegment>.Enumerator enumerator = this._horzSegList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ClipperBase.UpdateHorzSegment(enumerator.Current))
					{
						num++;
					}
				}
			}
			if (num < 2)
			{
				return;
			}
			this._horzSegList.Sort(new Comparison<HorzSegment>(this.HorzSegSort));
			for (int i = 0; i < num - 1; i++)
			{
				HorzSegment horzSegment = this._horzSegList[i];
				for (int j = i + 1; j < num; j++)
				{
					HorzSegment horzSegment2 = this._horzSegList[j];
					if (horzSegment2.leftOp.pt.X < horzSegment.rightOp.pt.X && horzSegment2.leftToRight != horzSegment.leftToRight && horzSegment2.rightOp.pt.X > horzSegment.leftOp.pt.X)
					{
						long y = horzSegment.leftOp.pt.Y;
						if (horzSegment.leftToRight)
						{
							while (horzSegment.leftOp.next.pt.Y == y)
							{
								if (horzSegment.leftOp.next.pt.X > horzSegment2.leftOp.pt.X)
								{
									break;
								}
								horzSegment.leftOp = horzSegment.leftOp.next;
							}
							while (horzSegment2.leftOp.prev.pt.Y == y && horzSegment2.leftOp.prev.pt.X <= horzSegment.leftOp.pt.X)
							{
								horzSegment2.leftOp = horzSegment2.leftOp.prev;
							}
							HorzJoin item = new HorzJoin(ClipperBase.DuplicateOp(horzSegment.leftOp, true), ClipperBase.DuplicateOp(horzSegment2.leftOp, false));
							this._horzJoinList.Add(item);
						}
						else
						{
							while (horzSegment.leftOp.prev.pt.Y == y)
							{
								if (horzSegment.leftOp.prev.pt.X > horzSegment2.leftOp.pt.X)
								{
									break;
								}
								horzSegment.leftOp = horzSegment.leftOp.prev;
							}
							while (horzSegment2.leftOp.next.pt.Y == y && horzSegment2.leftOp.next.pt.X <= horzSegment.leftOp.pt.X)
							{
								horzSegment2.leftOp = horzSegment2.leftOp.next;
							}
							HorzJoin item2 = new HorzJoin(ClipperBase.DuplicateOp(horzSegment2.leftOp, true), ClipperBase.DuplicateOp(horzSegment.leftOp, false));
							this._horzJoinList.Add(item2);
						}
					}
				}
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000093A8 File Offset: 0x000075A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static List<Point64> GetCleanPath(OutPt op)
		{
			List<Point64> list = new List<Point64>();
			OutPt outPt = op;
			while (outPt.next != op && ((outPt.pt.X == outPt.next.pt.X && outPt.pt.X == outPt.prev.pt.X) || (outPt.pt.Y == outPt.next.pt.Y && outPt.pt.Y == outPt.prev.pt.Y)))
			{
				outPt = outPt.next;
			}
			list.Add(outPt.pt);
			OutPt outPt2 = outPt;
			for (outPt = outPt.next; outPt != op; outPt = outPt.next)
			{
				if ((outPt.pt.X != outPt.next.pt.X || outPt.pt.X != outPt2.pt.X) && (outPt.pt.Y != outPt.next.pt.Y || outPt.pt.Y != outPt2.pt.Y))
				{
					list.Add(outPt.pt);
					outPt2 = outPt;
				}
			}
			return list;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000094E4 File Offset: 0x000076E4
		private static PointInPolygonResult PointInOpPolygon(Point64 pt, OutPt op)
		{
			if (op == op.next || op.prev == op.next)
			{
				return PointInPolygonResult.IsOutside;
			}
			OutPt outPt = op;
			while (op.pt.Y == pt.Y)
			{
				op = op.next;
				if (op == outPt)
				{
					break;
				}
			}
			if (op.pt.Y == pt.Y)
			{
				return PointInPolygonResult.IsOutside;
			}
			bool flag = op.pt.Y < pt.Y;
			bool flag2 = flag;
			int num = 0;
			outPt = op.next;
			while (outPt != op)
			{
				if (flag)
				{
					while (outPt != op)
					{
						if (outPt.pt.Y >= pt.Y)
						{
							break;
						}
						outPt = outPt.next;
					}
				}
				else
				{
					while (outPt != op && outPt.pt.Y > pt.Y)
					{
						outPt = outPt.next;
					}
				}
				if (outPt == op)
				{
					break;
				}
				if (outPt.pt.Y == pt.Y)
				{
					if (outPt.pt.X == pt.X || (outPt.pt.Y == outPt.prev.pt.Y && pt.X < outPt.prev.pt.X != pt.X < outPt.pt.X))
					{
						return PointInPolygonResult.IsOn;
					}
					outPt = outPt.next;
					if (outPt == op)
					{
						break;
					}
				}
				else
				{
					if (outPt.pt.X <= pt.X || outPt.prev.pt.X <= pt.X)
					{
						if (outPt.prev.pt.X < pt.X && outPt.pt.X < pt.X)
						{
							num = 1 - num;
						}
						else
						{
							double num2 = InternalClipper.CrossProduct(outPt.prev.pt, outPt.pt, pt);
							if (num2 == 0.0)
							{
								return PointInPolygonResult.IsOn;
							}
							if (num2 < 0.0 == flag)
							{
								num = 1 - num;
							}
						}
					}
					flag = !flag;
					outPt = outPt.next;
				}
			}
			if (flag != flag2)
			{
				double num3 = InternalClipper.CrossProduct(outPt.prev.pt, outPt.pt, pt);
				if (num3 == 0.0)
				{
					return PointInPolygonResult.IsOn;
				}
				if (num3 < 0.0 == flag)
				{
					num = 1 - num;
				}
			}
			if (num == 0)
			{
				return PointInPolygonResult.IsOutside;
			}
			return PointInPolygonResult.IsInside;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00009724 File Offset: 0x00007924
		private static bool Path1InsidePath2(OutPt op1, OutPt op2)
		{
			int num = 0;
			OutPt outPt = op1;
			do
			{
				PointInPolygonResult pointInPolygonResult = ClipperBase.PointInOpPolygon(outPt.pt, op2);
				if (pointInPolygonResult == PointInPolygonResult.IsOutside)
				{
					num++;
				}
				else if (pointInPolygonResult == PointInPolygonResult.IsInside)
				{
					num--;
				}
				outPt = outPt.next;
			}
			while (outPt != op1 && Math.Abs(num) < 2);
			if (Math.Abs(num) > 1)
			{
				return num < 0;
			}
			Point64 pt = ClipperBase.GetBounds(ClipperBase.GetCleanPath(op1)).MidPoint();
			List<Point64> cleanPath = ClipperBase.GetCleanPath(op2);
			return InternalClipper.PointInPolygon(pt, cleanPath) != PointInPolygonResult.IsOutside;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000097A0 File Offset: 0x000079A0
		private void MoveSplits(OutRec fromOr, OutRec toOr)
		{
			if (fromOr.splits == null)
			{
				return;
			}
			if (toOr.splits == null)
			{
				toOr.splits = new List<int>();
			}
			foreach (int item in fromOr.splits)
			{
				toOr.splits.Add(item);
			}
			fromOr.splits = null;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009820 File Offset: 0x00007A20
		private void ProcessHorzJoins()
		{
			foreach (HorzJoin horzJoin in this._horzJoinList)
			{
				OutRec realOutRec = ClipperBase.GetRealOutRec(horzJoin.op1.outrec);
				OutRec outRec = ClipperBase.GetRealOutRec(horzJoin.op2.outrec);
				OutPt next = horzJoin.op1.next;
				OutPt prev = horzJoin.op2.prev;
				horzJoin.op1.next = horzJoin.op2;
				horzJoin.op2.prev = horzJoin.op1;
				next.prev = prev;
				prev.next = next;
				if (realOutRec == outRec)
				{
					outRec = this.NewOutRec();
					outRec.pts = next;
					ClipperBase.FixOutRecPts(outRec);
					if (realOutRec.pts.outrec == outRec)
					{
						realOutRec.pts = horzJoin.op1;
						realOutRec.pts.outrec = realOutRec;
					}
					if (this._using_polytree)
					{
						if (ClipperBase.Path1InsidePath2(realOutRec.pts, outRec.pts))
						{
							OutPt pts = realOutRec.pts;
							realOutRec.pts = outRec.pts;
							outRec.pts = pts;
							ClipperBase.FixOutRecPts(realOutRec);
							ClipperBase.FixOutRecPts(outRec);
							outRec.owner = realOutRec;
						}
						else if (ClipperBase.Path1InsidePath2(outRec.pts, realOutRec.pts))
						{
							outRec.owner = realOutRec;
						}
						else
						{
							outRec.owner = realOutRec.owner;
						}
						OutRec outRec2 = realOutRec;
						if (outRec2.splits == null)
						{
							outRec2.splits = new List<int>();
						}
						realOutRec.splits.Add(outRec.idx);
					}
					else
					{
						outRec.owner = realOutRec;
					}
				}
				else
				{
					outRec.pts = null;
					if (this._using_polytree)
					{
						ClipperBase.SetOwner(outRec, realOutRec);
						this.MoveSplits(outRec, realOutRec);
					}
					else
					{
						outRec.owner = realOutRec;
					}
				}
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009A04 File Offset: 0x00007C04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool PtsReallyClose(Point64 pt1, Point64 pt2)
		{
			return Math.Abs(pt1.X - pt2.X) < 2L && Math.Abs(pt1.Y - pt2.Y) < 2L;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00009A34 File Offset: 0x00007C34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsVerySmallTriangle(OutPt op)
		{
			return op.next.next == op.prev && (ClipperBase.PtsReallyClose(op.prev.pt, op.next.pt) || ClipperBase.PtsReallyClose(op.pt, op.next.pt) || ClipperBase.PtsReallyClose(op.pt, op.prev.pt));
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00009AA3 File Offset: 0x00007CA3
		[NullableContext(2)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsValidClosedPath(OutPt op)
		{
			return op != null && op.next != op && (op.next != op.prev || !ClipperBase.IsVerySmallTriangle(op));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00009ACC File Offset: 0x00007CCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private static OutPt DisposeOutPt(OutPt op)
		{
			OutPt result = (op.next == op) ? null : op.next;
			op.prev.next = op.next;
			op.next.prev = op.prev;
			return result;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00009B04 File Offset: 0x00007D04
		[NullableContext(2)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CleanCollinear(OutRec outrec)
		{
			outrec = ClipperBase.GetRealOutRec(outrec);
			if (outrec == null || outrec.isOpen)
			{
				return;
			}
			if (!ClipperBase.IsValidClosedPath(outrec.pts))
			{
				outrec.pts = null;
				return;
			}
			OutPt outPt = outrec.pts;
			OutPt outPt2 = outPt;
			for (;;)
			{
				if (InternalClipper.IsCollinear(outPt2.prev.pt, outPt2.pt, outPt2.next.pt) && (outPt2.pt == outPt2.prev.pt || outPt2.pt == outPt2.next.pt || !this.PreserveCollinear || InternalClipper.DotProduct(outPt2.prev.pt, outPt2.pt, outPt2.next.pt) < 0.0))
				{
					if (outPt2 == outrec.pts)
					{
						outrec.pts = outPt2.prev;
					}
					outPt2 = ClipperBase.DisposeOutPt(outPt2);
					if (!ClipperBase.IsValidClosedPath(outPt2))
					{
						break;
					}
					outPt = outPt2;
				}
				else
				{
					outPt2 = outPt2.next;
					if (outPt2 == outPt)
					{
						goto Block_9;
					}
				}
			}
			outrec.pts = null;
			return;
			Block_9:
			this.FixSelfIntersects(outrec);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00009C18 File Offset: 0x00007E18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoSplitOp(OutRec outrec, OutPt splitOp)
		{
			OutPt prev = splitOp.prev;
			OutPt next = splitOp.next.next;
			outrec.pts = prev;
			Point64 point;
			InternalClipper.GetSegmentIntersectPt(prev.pt, splitOp.pt, splitOp.next.pt, next.pt, out point);
			double num = ClipperBase.Area(prev);
			double num2 = Math.Abs(num);
			if (num2 < 2.0)
			{
				outrec.pts = null;
				return;
			}
			double num3 = ClipperBase.AreaTriangle(point, splitOp.pt, splitOp.next.pt);
			double num4 = Math.Abs(num3);
			if (point == prev.pt || point == next.pt)
			{
				next.prev = prev;
				prev.next = next;
			}
			else
			{
				OutPt outPt = new OutPt(point, outrec)
				{
					prev = prev,
					next = next
				};
				next.prev = outPt;
				prev.next = outPt;
			}
			if (num4 > 1.0 && (num4 > num2 || num3 > 0.0 == num > 0.0))
			{
				OutRec outRec = this.NewOutRec();
				outRec.owner = outrec.owner;
				splitOp.outrec = outRec;
				splitOp.next.outrec = outRec;
				OutPt outPt2 = new OutPt(point, outRec)
				{
					prev = splitOp.next,
					next = splitOp
				};
				outRec.pts = outPt2;
				splitOp.prev = outPt2;
				splitOp.next.next = outPt2;
				if (this._using_polytree)
				{
					if (ClipperBase.Path1InsidePath2(prev, outPt2))
					{
						OutRec outRec2 = outRec;
						if (outRec2.splits == null)
						{
							outRec2.splits = new List<int>();
						}
						outRec.splits.Add(outrec.idx);
						return;
					}
					if (outrec.splits == null)
					{
						outrec.splits = new List<int>();
					}
					outrec.splits.Add(outRec.idx);
				}
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00009E00 File Offset: 0x00008000
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void FixSelfIntersects(OutRec outrec)
		{
			OutPt outPt = outrec.pts;
			while (outPt.prev != outPt.next.next)
			{
				if (InternalClipper.SegsIntersect(outPt.prev.pt, outPt.pt, outPt.next.pt, outPt.next.next.pt, false))
				{
					this.DoSplitOp(outrec, outPt);
					if (outrec.pts == null)
					{
						return;
					}
					outPt = outrec.pts;
				}
				else
				{
					outPt = outPt.next;
					if (outPt == outrec.pts)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00009E88 File Offset: 0x00008088
		internal static bool BuildPath([Nullable(2)] OutPt op, bool reverse, bool isOpen, List<Point64> path)
		{
			if (op == null || op.next == op || (!isOpen && op.next == op.prev))
			{
				return false;
			}
			path.Clear();
			Point64 pt;
			OutPt outPt;
			if (reverse)
			{
				pt = op.pt;
				outPt = op.prev;
			}
			else
			{
				op = op.next;
				pt = op.pt;
				outPt = op.next;
			}
			path.Add(pt);
			while (outPt != op)
			{
				if (outPt.pt != pt)
				{
					pt = outPt.pt;
					path.Add(pt);
				}
				if (reverse)
				{
					outPt = outPt.prev;
				}
				else
				{
					outPt = outPt.next;
				}
			}
			return path.Count != 3 || isOpen || !ClipperBase.IsVerySmallTriangle(outPt);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00009F38 File Offset: 0x00008138
		protected bool BuildPaths(List<List<Point64>> solutionClosed, List<List<Point64>> solutionOpen)
		{
			solutionClosed.Clear();
			solutionOpen.Clear();
			solutionClosed.EnsureCapacity(this._outrecList.Count);
			solutionOpen.EnsureCapacity(this._outrecList.Count);
			int i = 0;
			while (i < this._outrecList.Count)
			{
				OutRec outRec = this._outrecList[i++];
				if (outRec.pts != null)
				{
					List<Point64> list = new List<Point64>();
					if (outRec.isOpen)
					{
						if (ClipperBase.BuildPath(outRec.pts, this.ReverseSolution, true, list))
						{
							solutionOpen.Add(list);
						}
					}
					else
					{
						this.CleanCollinear(outRec);
						if (ClipperBase.BuildPath(outRec.pts, this.ReverseSolution, false, list))
						{
							solutionClosed.Add(list);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00009FF0 File Offset: 0x000081F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Rect64 GetBounds(List<Point64> path)
		{
			if (path.Count == 0)
			{
				return default(Rect64);
			}
			Rect64 invalidRect = Clipper.InvalidRect64;
			foreach (Point64 point in path)
			{
				if (point.X < invalidRect.left)
				{
					invalidRect.left = point.X;
				}
				if (point.X > invalidRect.right)
				{
					invalidRect.right = point.X;
				}
				if (point.Y < invalidRect.top)
				{
					invalidRect.top = point.Y;
				}
				if (point.Y > invalidRect.bottom)
				{
					invalidRect.bottom = point.Y;
				}
			}
			return invalidRect;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000A0BC File Offset: 0x000082BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CheckBounds(OutRec outrec)
		{
			if (outrec.pts == null)
			{
				return false;
			}
			if (!outrec.bounds.IsEmpty())
			{
				return true;
			}
			this.CleanCollinear(outrec);
			if (outrec.pts == null || !ClipperBase.BuildPath(outrec.pts, this.ReverseSolution, false, outrec.path))
			{
				return false;
			}
			outrec.bounds = ClipperBase.GetBounds(outrec.path);
			return true;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000A120 File Offset: 0x00008320
		private bool CheckSplitOwner(OutRec outrec, [Nullable(2)] List<int> splits)
		{
			foreach (int index in splits)
			{
				OutRec realOutRec = ClipperBase.GetRealOutRec(this._outrecList[index]);
				if (realOutRec != null && realOutRec != outrec && realOutRec.recursiveSplit != outrec)
				{
					realOutRec.recursiveSplit = outrec;
					if (realOutRec.splits != null && this.CheckSplitOwner(outrec, realOutRec.splits))
					{
						return true;
					}
					if (ClipperBase.IsValidOwner(outrec, realOutRec) && this.CheckBounds(realOutRec) && realOutRec.bounds.Contains(outrec.bounds) && ClipperBase.Path1InsidePath2(outrec.pts, realOutRec.pts))
					{
						outrec.owner = realOutRec;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000A1F8 File Offset: 0x000083F8
		private void RecursiveCheckOwners(OutRec outrec, PolyPathBase polypath)
		{
			if (outrec.polypath != null || outrec.bounds.IsEmpty())
			{
				return;
			}
			while (outrec.owner != null && (outrec.owner.splits == null || !this.CheckSplitOwner(outrec, outrec.owner.splits)) && (outrec.owner.pts == null || !this.CheckBounds(outrec.owner) || !ClipperBase.Path1InsidePath2(outrec.pts, outrec.owner.pts)))
			{
				outrec.owner = outrec.owner.owner;
			}
			if (outrec.owner != null)
			{
				if (outrec.owner.polypath == null)
				{
					this.RecursiveCheckOwners(outrec.owner, polypath);
				}
				outrec.polypath = outrec.owner.polypath.AddChild(outrec.path);
				return;
			}
			outrec.polypath = polypath.AddChild(outrec.path);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000A2DC File Offset: 0x000084DC
		protected void BuildTree(PolyPathBase polytree, List<List<Point64>> solutionOpen)
		{
			polytree.Clear();
			solutionOpen.Clear();
			if (this._hasOpenPaths)
			{
				solutionOpen.EnsureCapacity(this._outrecList.Count);
			}
			int i = 0;
			while (i < this._outrecList.Count)
			{
				OutRec outRec = this._outrecList[i++];
				if (outRec.pts != null)
				{
					if (outRec.isOpen)
					{
						List<Point64> list = new List<Point64>();
						if (ClipperBase.BuildPath(outRec.pts, this.ReverseSolution, true, list))
						{
							solutionOpen.Add(list);
						}
					}
					else if (this.CheckBounds(outRec))
					{
						this.RecursiveCheckOwners(outRec, polytree);
					}
				}
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000A378 File Offset: 0x00008578
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect64 GetBounds()
		{
			Rect64 invalidRect = Clipper.InvalidRect64;
			foreach (Vertex vertex in this._vertexList)
			{
				Vertex vertex2 = vertex;
				do
				{
					if (vertex2.pt.X < invalidRect.left)
					{
						invalidRect.left = vertex2.pt.X;
					}
					if (vertex2.pt.X > invalidRect.right)
					{
						invalidRect.right = vertex2.pt.X;
					}
					if (vertex2.pt.Y < invalidRect.top)
					{
						invalidRect.top = vertex2.pt.Y;
					}
					if (vertex2.pt.Y > invalidRect.bottom)
					{
						invalidRect.bottom = vertex2.pt.Y;
					}
					vertex2 = vertex2.next;
				}
				while (vertex2 != vertex);
			}
			if (!invalidRect.IsEmpty())
			{
				return invalidRect;
			}
			return new Rect64(0L, 0L, 0L, 0L);
		}

		// Token: 0x04000071 RID: 113
		private ClipType _cliptype;

		// Token: 0x04000072 RID: 114
		private FillRule _fillrule;

		// Token: 0x04000073 RID: 115
		[Nullable(2)]
		private Active _actives;

		// Token: 0x04000074 RID: 116
		[Nullable(2)]
		private Active _sel;

		// Token: 0x04000075 RID: 117
		private readonly List<LocalMinima> _minimaList;

		// Token: 0x04000076 RID: 118
		private readonly List<IntersectNode> _intersectList;

		// Token: 0x04000077 RID: 119
		private readonly List<Vertex> _vertexList;

		// Token: 0x04000078 RID: 120
		private readonly List<OutRec> _outrecList;

		// Token: 0x04000079 RID: 121
		private readonly List<long> _scanlineList;

		// Token: 0x0400007A RID: 122
		private readonly List<HorzSegment> _horzSegList;

		// Token: 0x0400007B RID: 123
		private readonly List<HorzJoin> _horzJoinList;

		// Token: 0x0400007C RID: 124
		private readonly VertexPool _vertexPool;

		// Token: 0x0400007D RID: 125
		private readonly GCArena<Active> _activeArena;

		// Token: 0x0400007E RID: 126
		private int _currentLocMin;

		// Token: 0x0400007F RID: 127
		private long _currentBotY;

		// Token: 0x04000080 RID: 128
		private bool _isSortedMinimaList;

		// Token: 0x04000081 RID: 129
		private bool _hasOpenPaths;

		// Token: 0x04000082 RID: 130
		internal bool _using_polytree;

		// Token: 0x04000083 RID: 131
		internal bool _succeeded;

		// Token: 0x02000036 RID: 54
		[NullableContext(0)]
		private struct IntersectListSort : IComparer<IntersectNode>
		{
			// Token: 0x060001DE RID: 478 RVA: 0x0000E3D0 File Offset: 0x0000C5D0
			public readonly int Compare(IntersectNode a, IntersectNode b)
			{
				if (a.pt.Y == b.pt.Y)
				{
					if (a.pt.X == b.pt.X)
					{
						return 0;
					}
					if (a.pt.X >= b.pt.X)
					{
						return 1;
					}
					return -1;
				}
				else
				{
					if (a.pt.Y <= b.pt.Y)
					{
						return 1;
					}
					return -1;
				}
			}
		}
	}
}
