using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000030 RID: 48
	[NullableContext(1)]
	[Nullable(0)]
	public class RectClip64
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		internal RectClip64(Rect64 rect)
		{
			this.currIdx_ = -1;
			this.rect_ = rect;
			this.mp_ = rect.MidPoint();
			this.rectPath_ = this.rect_.AsPath();
			this.results_ = new List<OutPt2>();
			this.edges_ = new List<OutPt2>[8];
			for (int i = 0; i < 8; i++)
			{
				this.edges_[i] = new List<OutPt2>();
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000C660 File Offset: 0x0000A860
		internal OutPt2 Add(Point64 pt, bool startingNewPath = false)
		{
			int num = this.results_.Count;
			OutPt2 outPt;
			if (num == 0 || startingNewPath)
			{
				outPt = new OutPt2(pt);
				this.results_.Add(outPt);
				outPt.ownerIdx = num;
				outPt.prev = outPt;
				outPt.next = outPt;
			}
			else
			{
				num--;
				OutPt2 outPt2 = this.results_[num];
				if (outPt2.pt == pt)
				{
					return outPt2;
				}
				outPt = new OutPt2(pt)
				{
					ownerIdx = num,
					next = outPt2.next
				};
				outPt2.next.prev = outPt;
				outPt2.next = outPt;
				outPt.prev = outPt2;
				this.results_[num] = outPt;
			}
			return outPt;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000C710 File Offset: 0x0000A910
		private static bool Path1ContainsPath2(List<Point64> path1, List<Point64> path2)
		{
			int num = 0;
			foreach (Point64 pt in path2)
			{
				PointInPolygonResult pointInPolygonResult = InternalClipper.PointInPolygon(pt, path1);
				if (pointInPolygonResult != PointInPolygonResult.IsInside)
				{
					if (pointInPolygonResult == PointInPolygonResult.IsOutside)
					{
						num++;
					}
				}
				else
				{
					num--;
				}
				if (Math.Abs(num) > 1)
				{
					break;
				}
			}
			return num <= 0;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000C788 File Offset: 0x0000A988
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsClockwise(RectClip64.Location prev, RectClip64.Location curr, Point64 prevPt, Point64 currPt, Point64 rectMidPoint)
		{
			if (RectClip64.AreOpposites(prev, curr))
			{
				return InternalClipper.CrossProduct(prevPt, rectMidPoint, currPt) < 0.0;
			}
			return RectClip64.HeadingClockwise(prev, curr);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000C7AF File Offset: 0x0000A9AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool AreOpposites(RectClip64.Location prev, RectClip64.Location curr)
		{
			return Math.Abs(prev - curr) == 2;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000C7BC File Offset: 0x0000A9BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool HeadingClockwise(RectClip64.Location prev, RectClip64.Location curr)
		{
			return (prev + 1) % RectClip64.Location.inside == curr;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static RectClip64.Location GetAdjacentLocation(RectClip64.Location loc, bool isClockwise)
		{
			int num = isClockwise ? 1 : 3;
			return (loc + num) % RectClip64.Location.inside;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000C7E2 File Offset: 0x0000A9E2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private static OutPt2 UnlinkOp(OutPt2 op)
		{
			if (op.next == op)
			{
				return null;
			}
			op.prev.next = op.next;
			op.next.prev = op.prev;
			return op.next;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000C817 File Offset: 0x0000AA17
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(2)]
		private static OutPt2 UnlinkOpBack(OutPt2 op)
		{
			if (op.next == op)
			{
				return null;
			}
			op.prev.next = op.next;
			op.next.prev = op.prev;
			return op.prev;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000C84C File Offset: 0x0000AA4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint GetEdgesForPt(Point64 pt, Rect64 rec)
		{
			uint num = 0U;
			if (pt.X == rec.left)
			{
				num = 1U;
			}
			else if (pt.X == rec.right)
			{
				num = 4U;
			}
			if (pt.Y == rec.top)
			{
				num += 2U;
			}
			else if (pt.Y == rec.bottom)
			{
				num += 8U;
			}
			return num;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsHeadingClockwise(Point64 pt1, Point64 pt2, int edgeIdx)
		{
			bool result;
			switch (edgeIdx)
			{
			case 0:
				result = (pt2.Y < pt1.Y);
				break;
			case 1:
				result = (pt2.X > pt1.X);
				break;
			case 2:
				result = (pt2.Y > pt1.Y);
				break;
			default:
				result = (pt2.X < pt1.X);
				break;
			}
			return result;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000C908 File Offset: 0x0000AB08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool HasHorzOverlap(Point64 left1, Point64 right1, Point64 left2, Point64 right2)
		{
			return left1.X < right2.X && right1.X > left2.X;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000C928 File Offset: 0x0000AB28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool HasVertOverlap(Point64 top1, Point64 bottom1, Point64 top2, Point64 bottom2)
		{
			return top1.Y < bottom2.Y && bottom1.Y > top2.Y;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000C948 File Offset: 0x0000AB48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void AddToEdge([Nullable(new byte[]
		{
			1,
			2
		})] List<OutPt2> edge, OutPt2 op)
		{
			if (op.edge != null)
			{
				return;
			}
			op.edge = edge;
			edge.Add(op);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000C964 File Offset: 0x0000AB64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void UncoupleEdge(OutPt2 op)
		{
			if (op.edge == null)
			{
				return;
			}
			for (int i = 0; i < op.edge.Count; i++)
			{
				if (op.edge[i] == op)
				{
					op.edge[i] = null;
					break;
				}
			}
			op.edge = null;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SetNewOwner(OutPt2 op, int newIdx)
		{
			op.ownerIdx = newIdx;
			for (OutPt2 next = op.next; next != op; next = next.next)
			{
				next.ownerIdx = newIdx;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000C9E7 File Offset: 0x0000ABE7
		private void AddCorner(RectClip64.Location prev, RectClip64.Location curr)
		{
			if (RectClip64.HeadingClockwise(prev, curr))
			{
				this.Add(this.rectPath_[(int)prev], false);
				return;
			}
			this.Add(this.rectPath_[(int)curr], false);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000CA1C File Offset: 0x0000AC1C
		private void AddCorner(ref RectClip64.Location loc, bool isClockwise)
		{
			if (isClockwise)
			{
				this.Add(this.rectPath_[(int)loc], false);
				loc = RectClip64.GetAdjacentLocation(loc, true);
				return;
			}
			loc = RectClip64.GetAdjacentLocation(loc, false);
			this.Add(this.rectPath_[(int)loc], false);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000CA6C File Offset: 0x0000AC6C
		protected static bool GetLocation(Rect64 rec, Point64 pt, out RectClip64.Location loc)
		{
			if (pt.X == rec.left && pt.Y >= rec.top && pt.Y <= rec.bottom)
			{
				loc = RectClip64.Location.left;
				return false;
			}
			if (pt.X == rec.right && pt.Y >= rec.top && pt.Y <= rec.bottom)
			{
				loc = RectClip64.Location.right;
				return false;
			}
			if (pt.Y == rec.top && pt.X >= rec.left && pt.X <= rec.right)
			{
				loc = RectClip64.Location.top;
				return false;
			}
			if (pt.Y == rec.bottom && pt.X >= rec.left && pt.X <= rec.right)
			{
				loc = RectClip64.Location.bottom;
				return false;
			}
			if (pt.X < rec.left)
			{
				loc = RectClip64.Location.left;
			}
			else if (pt.X > rec.right)
			{
				loc = RectClip64.Location.right;
			}
			else if (pt.Y < rec.top)
			{
				loc = RectClip64.Location.top;
			}
			else if (pt.Y > rec.bottom)
			{
				loc = RectClip64.Location.bottom;
			}
			else
			{
				loc = RectClip64.Location.inside;
			}
			return true;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000CB85 File Offset: 0x0000AD85
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsHorizontal(Point64 pt1, Point64 pt2)
		{
			return pt1.Y == pt2.Y;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000CB98 File Offset: 0x0000AD98
		private static bool GetSegmentIntersection(Point64 p1, Point64 p2, Point64 p3, Point64 p4, out Point64 ip)
		{
			double num = InternalClipper.CrossProduct(p1, p3, p4);
			double num2 = InternalClipper.CrossProduct(p2, p3, p4);
			if (num == 0.0)
			{
				ip = p1;
				if (num2 == 0.0)
				{
					return false;
				}
				if (p1 == p3 || p1 == p4)
				{
					return true;
				}
				if (RectClip64.IsHorizontal(p3, p4))
				{
					return p1.X > p3.X == p1.X < p4.X;
				}
				return p1.Y > p3.Y == p1.Y < p4.Y;
			}
			else if (num2 == 0.0)
			{
				ip = p2;
				if (p2 == p3 || p2 == p4)
				{
					return true;
				}
				if (RectClip64.IsHorizontal(p3, p4))
				{
					return p2.X > p3.X == p2.X < p4.X;
				}
				return p2.Y > p3.Y == p2.Y < p4.Y;
			}
			else
			{
				if (num > 0.0 == num2 > 0.0)
				{
					ip = new Point64(0L, 0L);
					return false;
				}
				double num3 = InternalClipper.CrossProduct(p3, p1, p2);
				double num4 = InternalClipper.CrossProduct(p4, p1, p2);
				if (num3 == 0.0)
				{
					ip = p3;
					if (p3 == p1 || p3 == p2)
					{
						return true;
					}
					if (RectClip64.IsHorizontal(p1, p2))
					{
						return p3.X > p1.X == p3.X < p2.X;
					}
					return p3.Y > p1.Y == p3.Y < p2.Y;
				}
				else if (num4 == 0.0)
				{
					ip = p4;
					if (p4 == p1 || p4 == p2)
					{
						return true;
					}
					if (RectClip64.IsHorizontal(p1, p2))
					{
						return p4.X > p1.X == p4.X < p2.X;
					}
					return p4.Y > p1.Y == p4.Y < p2.Y;
				}
				else
				{
					if (num3 > 0.0 == num4 > 0.0)
					{
						ip = new Point64(0L, 0L);
						return false;
					}
					return InternalClipper.GetSegmentIntersectPt(p1, p2, p3, p4, out ip);
				}
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
		protected static bool GetIntersection(List<Point64> rectPath, Point64 p, Point64 p2, ref RectClip64.Location loc, out Point64 ip)
		{
			ip = default(Point64);
			switch (loc)
			{
			case RectClip64.Location.left:
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[0], rectPath[3], out ip))
				{
					return true;
				}
				if (p.Y < rectPath[0].Y && RectClip64.GetSegmentIntersection(p, p2, rectPath[0], rectPath[1], out ip))
				{
					loc = RectClip64.Location.top;
					return true;
				}
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[2], rectPath[3], out ip))
				{
					loc = RectClip64.Location.bottom;
					return true;
				}
				return false;
			case RectClip64.Location.top:
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[0], rectPath[1], out ip))
				{
					return true;
				}
				if (p.X < rectPath[0].X && RectClip64.GetSegmentIntersection(p, p2, rectPath[0], rectPath[3], out ip))
				{
					loc = RectClip64.Location.left;
					return true;
				}
				if (p.X > rectPath[1].X && RectClip64.GetSegmentIntersection(p, p2, rectPath[1], rectPath[2], out ip))
				{
					loc = RectClip64.Location.right;
					return true;
				}
				return false;
			case RectClip64.Location.right:
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[1], rectPath[2], out ip))
				{
					return true;
				}
				if (p.Y < rectPath[0].Y && RectClip64.GetSegmentIntersection(p, p2, rectPath[0], rectPath[1], out ip))
				{
					loc = RectClip64.Location.top;
					return true;
				}
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[2], rectPath[3], out ip))
				{
					loc = RectClip64.Location.bottom;
					return true;
				}
				return false;
			case RectClip64.Location.bottom:
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[2], rectPath[3], out ip))
				{
					return true;
				}
				if (p.X < rectPath[3].X && RectClip64.GetSegmentIntersection(p, p2, rectPath[0], rectPath[3], out ip))
				{
					loc = RectClip64.Location.left;
					return true;
				}
				if (p.X > rectPath[2].X && RectClip64.GetSegmentIntersection(p, p2, rectPath[1], rectPath[2], out ip))
				{
					loc = RectClip64.Location.right;
					return true;
				}
				return false;
			default:
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[0], rectPath[3], out ip))
				{
					loc = RectClip64.Location.left;
					return true;
				}
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[0], rectPath[1], out ip))
				{
					loc = RectClip64.Location.top;
					return true;
				}
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[1], rectPath[2], out ip))
				{
					loc = RectClip64.Location.right;
					return true;
				}
				if (RectClip64.GetSegmentIntersection(p, p2, rectPath[2], rectPath[3], out ip))
				{
					loc = RectClip64.Location.bottom;
					return true;
				}
				return false;
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000D080 File Offset: 0x0000B280
		protected void GetNextLocation(List<Point64> path, ref RectClip64.Location loc, ref int i, int highI)
		{
			switch (loc)
			{
			case RectClip64.Location.left:
				while (i <= highI && path[i].X <= this.rect_.left)
				{
					i++;
				}
				if (i <= highI)
				{
					if (path[i].X >= this.rect_.right)
					{
						loc = RectClip64.Location.right;
						return;
					}
					if (path[i].Y <= this.rect_.top)
					{
						loc = RectClip64.Location.top;
						return;
					}
					if (path[i].Y >= this.rect_.bottom)
					{
						loc = RectClip64.Location.bottom;
						return;
					}
					loc = RectClip64.Location.inside;
					return;
				}
				break;
			case RectClip64.Location.top:
				while (i <= highI && path[i].Y <= this.rect_.top)
				{
					i++;
				}
				if (i <= highI)
				{
					if (path[i].Y >= this.rect_.bottom)
					{
						loc = RectClip64.Location.bottom;
						return;
					}
					if (path[i].X <= this.rect_.left)
					{
						loc = RectClip64.Location.left;
						return;
					}
					if (path[i].X >= this.rect_.right)
					{
						loc = RectClip64.Location.right;
						return;
					}
					loc = RectClip64.Location.inside;
					return;
				}
				break;
			case RectClip64.Location.right:
				while (i <= highI && path[i].X >= this.rect_.right)
				{
					i++;
				}
				if (i <= highI)
				{
					if (path[i].X <= this.rect_.left)
					{
						loc = RectClip64.Location.left;
						return;
					}
					if (path[i].Y <= this.rect_.top)
					{
						loc = RectClip64.Location.top;
						return;
					}
					if (path[i].Y >= this.rect_.bottom)
					{
						loc = RectClip64.Location.bottom;
						return;
					}
					loc = RectClip64.Location.inside;
					return;
				}
				break;
			case RectClip64.Location.bottom:
				while (i <= highI && path[i].Y >= this.rect_.bottom)
				{
					i++;
				}
				if (i <= highI)
				{
					if (path[i].Y <= this.rect_.top)
					{
						loc = RectClip64.Location.top;
						return;
					}
					if (path[i].X <= this.rect_.left)
					{
						loc = RectClip64.Location.left;
						return;
					}
					if (path[i].X >= this.rect_.right)
					{
						loc = RectClip64.Location.right;
						return;
					}
					loc = RectClip64.Location.inside;
					return;
				}
				break;
			case RectClip64.Location.inside:
				while (i <= highI)
				{
					if (path[i].X < this.rect_.left)
					{
						loc = RectClip64.Location.left;
						return;
					}
					if (path[i].X > this.rect_.right)
					{
						loc = RectClip64.Location.right;
						return;
					}
					if (path[i].Y > this.rect_.bottom)
					{
						loc = RectClip64.Location.bottom;
						return;
					}
					if (path[i].Y < this.rect_.top)
					{
						loc = RectClip64.Location.top;
						return;
					}
					this.Add(path[i], false);
					i++;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000D378 File Offset: 0x0000B578
		private bool StartLocsAreClockwise(List<RectClip64.Location> startLocs)
		{
			int num = 0;
			for (int i = 1; i < startLocs.Count; i++)
			{
				switch (startLocs[i] - startLocs[i - 1])
				{
				case -3:
					num++;
					break;
				case -1:
					num--;
					break;
				case 1:
					num++;
					break;
				case 3:
					num--;
					break;
				}
			}
			return num > 0;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000D3EC File Offset: 0x0000B5EC
		private void ExecuteInternal(List<Point64> path)
		{
			if (path.Count < 3 || this.rect_.IsEmpty())
			{
				return;
			}
			List<RectClip64.Location> list = new List<RectClip64.Location>();
			RectClip64.Location location = RectClip64.Location.inside;
			RectClip64.Location location2 = location;
			RectClip64.Location location3 = location;
			int num = path.Count - 1;
			RectClip64.Location location4;
			int i;
			if (!RectClip64.GetLocation(this.rect_, path[num], out location4))
			{
				i = num - 1;
				while (i >= 0 && !RectClip64.GetLocation(this.rect_, path[i], out location3))
				{
					i--;
				}
				if (i < 0)
				{
					foreach (Point64 pt in path)
					{
						this.Add(pt, false);
					}
					return;
				}
				if (location3 == RectClip64.Location.inside)
				{
					location4 = RectClip64.Location.inside;
				}
			}
			RectClip64.Location location5 = location4;
			i = 0;
			while (i <= num)
			{
				location3 = location4;
				RectClip64.Location location6 = location2;
				this.GetNextLocation(path, ref location4, ref i, num);
				if (i > num)
				{
					break;
				}
				Point64 point = (i == 0) ? path[num] : path[i - 1];
				location2 = location4;
				Point64 point2;
				if (!RectClip64.GetIntersection(this.rectPath_, path[i], point, ref location2, out point2))
				{
					if (location6 == RectClip64.Location.inside)
					{
						bool isClockwise = RectClip64.IsClockwise(location3, location4, point, path[i], this.mp_);
						do
						{
							list.Add(location3);
							location3 = RectClip64.GetAdjacentLocation(location3, isClockwise);
						}
						while (location3 != location4);
						location2 = location6;
					}
					else if (location3 != RectClip64.Location.inside && location3 != location4)
					{
						bool isClockwise2 = RectClip64.IsClockwise(location3, location4, point, path[i], this.mp_);
						do
						{
							this.AddCorner(ref location3, isClockwise2);
						}
						while (location3 != location4);
					}
					i++;
				}
				else
				{
					if (location4 == RectClip64.Location.inside)
					{
						if (location == RectClip64.Location.inside)
						{
							location = location2;
							list.Add(location3);
						}
						else if (location3 != location2)
						{
							bool isClockwise3 = RectClip64.IsClockwise(location3, location2, point, path[i], this.mp_);
							do
							{
								this.AddCorner(ref location3, isClockwise3);
							}
							while (location3 != location2);
						}
					}
					else if (location3 != RectClip64.Location.inside)
					{
						location4 = location3;
						Point64 point3;
						RectClip64.GetIntersection(this.rectPath_, point, path[i], ref location4, out point3);
						if (location6 != RectClip64.Location.inside && location6 != location4)
						{
							this.AddCorner(location6, location4);
						}
						if (location == RectClip64.Location.inside)
						{
							location = location4;
							list.Add(location3);
						}
						location4 = location2;
						this.Add(point3, false);
						if (point2 == point3)
						{
							RectClip64.GetLocation(this.rect_, path[i], out location4);
							this.AddCorner(location2, location4);
							location2 = location4;
							continue;
						}
					}
					else
					{
						location4 = location2;
						if (location == RectClip64.Location.inside)
						{
							location = location2;
						}
					}
					this.Add(point2, false);
				}
			}
			if (location == RectClip64.Location.inside)
			{
				if (location5 != RectClip64.Location.inside && this.pathBounds_.Contains(this.rect_) && RectClip64.Path1ContainsPath2(path, this.rectPath_))
				{
					bool flag = this.StartLocsAreClockwise(list);
					for (int j = 0; j < 4; j++)
					{
						int num2 = flag ? j : (3 - j);
						this.Add(this.rectPath_[num2], false);
						RectClip64.AddToEdge(this.edges_[num2 * 2], this.results_[0]);
					}
					return;
				}
			}
			else if (location4 != RectClip64.Location.inside && (location4 != location || list.Count > 2))
			{
				if (list.Count > 0)
				{
					location3 = location4;
					foreach (RectClip64.Location location7 in list)
					{
						if (location3 != location7)
						{
							this.AddCorner(ref location3, RectClip64.HeadingClockwise(location3, location7));
							location3 = location7;
						}
					}
					location4 = location3;
				}
				if (location4 != location)
				{
					this.AddCorner(ref location4, RectClip64.HeadingClockwise(location4, location));
				}
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000D78C File Offset: 0x0000B98C
		public List<List<Point64>> Execute(List<List<Point64>> paths)
		{
			List<List<Point64>> list = new List<List<Point64>>();
			if (this.rect_.IsEmpty())
			{
				return list;
			}
			foreach (List<Point64> list2 in paths)
			{
				if (list2.Count >= 3)
				{
					this.pathBounds_ = Clipper.GetBounds(list2);
					if (this.rect_.Intersects(this.pathBounds_))
					{
						if (this.rect_.Contains(this.pathBounds_))
						{
							list.Add(list2);
						}
						else
						{
							this.ExecuteInternal(list2);
							this.CheckEdges();
							for (int i = 0; i < 4; i++)
							{
								this.TidyEdgePair(i, this.edges_[i * 2], this.edges_[i * 2 + 1]);
							}
							foreach (OutPt2 op in this.results_)
							{
								List<Point64> path = this.GetPath(op);
								if (path.Count > 0)
								{
									list.Add(path);
								}
							}
							this.results_.Clear();
							for (int j = 0; j < 8; j++)
							{
								this.edges_[j].Clear();
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000D90C File Offset: 0x0000BB0C
		private void CheckEdges()
		{
			for (int i = 0; i < this.results_.Count; i++)
			{
				OutPt2 outPt = this.results_[i];
				OutPt2 outPt2 = outPt;
				if (outPt != null)
				{
					do
					{
						if (InternalClipper.IsCollinear(outPt2.prev.pt, outPt2.pt, outPt2.next.pt))
						{
							if (outPt2 == outPt)
							{
								outPt2 = RectClip64.UnlinkOpBack(outPt2);
								if (outPt2 == null)
								{
									break;
								}
								outPt = outPt2.prev;
							}
							else
							{
								outPt2 = RectClip64.UnlinkOpBack(outPt2);
								if (outPt2 == null)
								{
									break;
								}
							}
						}
						else
						{
							outPt2 = outPt2.next;
						}
					}
					while (outPt2 != outPt);
					IL_6D:
					if (outPt2 == null)
					{
						this.results_[i] = null;
						goto IL_13C;
					}
					this.results_[i] = outPt2;
					uint num = RectClip64.GetEdgesForPt(outPt.prev.pt, this.rect_);
					outPt2 = outPt;
					for (;;)
					{
						uint edgesForPt = RectClip64.GetEdgesForPt(outPt2.pt, this.rect_);
						if (edgesForPt != 0U && outPt2.edge == null)
						{
							uint num2 = num & edgesForPt;
							for (int j = 0; j < 4; j++)
							{
								if (((ulong)num2 & (ulong)(1L << (j & 31))) != 0UL)
								{
									if (RectClip64.IsHeadingClockwise(outPt2.prev.pt, outPt2.pt, j))
									{
										RectClip64.AddToEdge(this.edges_[j * 2], outPt2);
									}
									else
									{
										RectClip64.AddToEdge(this.edges_[j * 2 + 1], outPt2);
									}
								}
							}
						}
						num = edgesForPt;
						outPt2 = outPt2.next;
						if (outPt2 == outPt)
						{
							goto IL_13C;
						}
					}
					goto IL_6D;
				}
				IL_13C:;
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000DA6C File Offset: 0x0000BC6C
		private void TidyEdgePair(int idx, [Nullable(new byte[]
		{
			1,
			2
		})] List<OutPt2> cw, [Nullable(new byte[]
		{
			1,
			2
		})] List<OutPt2> ccw)
		{
			if (ccw.Count == 0)
			{
				return;
			}
			bool flag = idx == 1 || idx == 3;
			bool flag2 = idx == 1 || idx == 2;
			int i = 0;
			int num = 0;
			while (i < cw.Count)
			{
				OutPt2 outPt = cw[i];
				if (outPt == null || outPt.next == outPt.prev)
				{
					cw[i++] = null;
					num = 0;
				}
				else
				{
					int count = ccw.Count;
					while (num < count && (ccw[num] == null || ccw[num].next == ccw[num].prev))
					{
						num++;
					}
					if (num == count)
					{
						i++;
						num = 0;
					}
					else
					{
						OutPt2 outPt2;
						OutPt2 outPt3;
						OutPt2 outPt4;
						if (flag2)
						{
							outPt = cw[i].prev;
							outPt2 = cw[i];
							outPt3 = ccw[num];
							outPt4 = ccw[num].prev;
						}
						else
						{
							outPt = cw[i];
							outPt2 = cw[i].prev;
							outPt3 = ccw[num].prev;
							outPt4 = ccw[num];
						}
						if ((flag && !RectClip64.HasHorzOverlap(outPt.pt, outPt2.pt, outPt3.pt, outPt4.pt)) || (!flag && !RectClip64.HasVertOverlap(outPt.pt, outPt2.pt, outPt3.pt, outPt4.pt)))
						{
							num++;
						}
						else
						{
							bool flag3 = cw[i].ownerIdx != ccw[num].ownerIdx;
							if (flag3)
							{
								this.results_[outPt3.ownerIdx] = null;
								RectClip64.SetNewOwner(outPt3, outPt.ownerIdx);
							}
							if (flag2)
							{
								outPt.next = outPt3;
								outPt3.prev = outPt;
								outPt2.prev = outPt4;
								outPt4.next = outPt2;
							}
							else
							{
								outPt.prev = outPt3;
								outPt3.next = outPt;
								outPt2.next = outPt4;
								outPt4.prev = outPt2;
							}
							if (!flag3)
							{
								int count2 = this.results_.Count;
								this.results_.Add(outPt2);
								RectClip64.SetNewOwner(outPt2, count2);
							}
							OutPt2 outPt5;
							OutPt2 outPt6;
							if (flag2)
							{
								outPt5 = outPt3;
								outPt6 = outPt2;
							}
							else
							{
								outPt5 = outPt;
								outPt6 = outPt4;
							}
							this.results_[outPt5.ownerIdx] = outPt5;
							this.results_[outPt6.ownerIdx] = outPt6;
							bool flag4;
							bool flag5;
							if (flag)
							{
								flag4 = (outPt5.pt.X > outPt5.prev.pt.X);
								flag5 = (outPt6.pt.X > outPt6.prev.pt.X);
							}
							else
							{
								flag4 = (outPt5.pt.Y > outPt5.prev.pt.Y);
								flag5 = (outPt6.pt.Y > outPt6.prev.pt.Y);
							}
							if (outPt5.next == outPt5.prev || outPt5.pt == outPt5.prev.pt)
							{
								if (flag5 == flag2)
								{
									cw[i] = outPt6;
									ccw[num++] = null;
								}
								else
								{
									ccw[num] = outPt6;
									cw[i++] = null;
								}
							}
							else if (outPt6.next == outPt6.prev || outPt6.pt == outPt6.prev.pt)
							{
								if (flag4 == flag2)
								{
									cw[i] = outPt5;
									ccw[num++] = null;
								}
								else
								{
									ccw[num] = outPt5;
									cw[i++] = null;
								}
							}
							else if (flag4 == flag5)
							{
								if (flag4 == flag2)
								{
									cw[i] = outPt5;
									RectClip64.UncoupleEdge(outPt6);
									RectClip64.AddToEdge(cw, outPt6);
									ccw[num++] = null;
								}
								else
								{
									cw[i++] = null;
									ccw[num] = outPt6;
									RectClip64.UncoupleEdge(outPt5);
									RectClip64.AddToEdge(ccw, outPt5);
									num = 0;
								}
							}
							else
							{
								if (flag4 == flag2)
								{
									cw[i] = outPt5;
								}
								else
								{
									ccw[num] = outPt5;
								}
								if (flag5 == flag2)
								{
									cw[i] = outPt6;
								}
								else
								{
									ccw[num] = outPt6;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000DEA8 File Offset: 0x0000C0A8
		private List<Point64> GetPath([Nullable(2)] OutPt2 op)
		{
			List<Point64> list = new List<Point64>();
			if (op == null || op.prev == op.next)
			{
				return list;
			}
			OutPt2 outPt = op.next;
			while (outPt != null && outPt != op)
			{
				if (InternalClipper.IsCollinear(outPt.prev.pt, outPt.pt, outPt.next.pt))
				{
					op = outPt.prev;
					outPt = RectClip64.UnlinkOp(outPt);
				}
				else
				{
					outPt = outPt.next;
				}
			}
			if (outPt == null)
			{
				return new List<Point64>();
			}
			list.Add(op.pt);
			for (outPt = op.next; outPt != op; outPt = outPt.next)
			{
				list.Add(outPt.pt);
			}
			return list;
		}

		// Token: 0x040000B2 RID: 178
		protected readonly Rect64 rect_;

		// Token: 0x040000B3 RID: 179
		protected readonly Point64 mp_;

		// Token: 0x040000B4 RID: 180
		protected readonly List<Point64> rectPath_;

		// Token: 0x040000B5 RID: 181
		protected Rect64 pathBounds_;

		// Token: 0x040000B6 RID: 182
		[Nullable(new byte[]
		{
			1,
			2
		})]
		protected List<OutPt2> results_;

		// Token: 0x040000B7 RID: 183
		[Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		protected List<OutPt2>[] edges_;

		// Token: 0x040000B8 RID: 184
		protected int currIdx_;

		// Token: 0x0200003A RID: 58
		[NullableContext(0)]
		protected enum Location
		{
			// Token: 0x040000C8 RID: 200
			left,
			// Token: 0x040000C9 RID: 201
			top,
			// Token: 0x040000CA RID: 202
			right,
			// Token: 0x040000CB RID: 203
			bottom,
			// Token: 0x040000CC RID: 204
			inside
		}
	}
}
