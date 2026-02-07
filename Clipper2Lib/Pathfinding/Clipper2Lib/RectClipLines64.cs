using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000031 RID: 49
	[NullableContext(1)]
	[Nullable(0)]
	public class RectClipLines64 : RectClip64
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x0000DF4F File Offset: 0x0000C14F
		internal RectClipLines64(Rect64 rect) : base(rect)
		{
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000DF58 File Offset: 0x0000C158
		public new List<List<Point64>> Execute(List<List<Point64>> paths)
		{
			List<List<Point64>> list = new List<List<Point64>>();
			if (this.rect_.IsEmpty())
			{
				return list;
			}
			foreach (List<Point64> list2 in paths)
			{
				if (list2.Count >= 2)
				{
					this.pathBounds_ = Clipper.GetBounds(list2);
					if (this.rect_.Intersects(this.pathBounds_))
					{
						this.ExecuteInternal(list2);
						foreach (OutPt2 op in this.results_)
						{
							List<Point64> path = this.GetPath(op);
							if (path.Count > 0)
							{
								list.Add(path);
							}
						}
						this.results_.Clear();
						for (int i = 0; i < 8; i++)
						{
							this.edges_[i].Clear();
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000E070 File Offset: 0x0000C270
		private List<Point64> GetPath([Nullable(2)] OutPt2 op)
		{
			List<Point64> list = new List<Point64>();
			if (op == null || op == op.next)
			{
				return list;
			}
			op = op.next;
			list.Add(op.pt);
			for (OutPt2 next = op.next; next != op; next = next.next)
			{
				list.Add(next.pt);
			}
			return list;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E0C8 File Offset: 0x0000C2C8
		private void ExecuteInternal(List<Point64> path)
		{
			this.results_.Clear();
			if (path.Count < 2 || this.rect_.IsEmpty())
			{
				return;
			}
			RectClip64.Location location = RectClip64.Location.inside;
			int i = 1;
			int num = path.Count - 1;
			RectClip64.Location location2;
			if (!RectClip64.GetLocation(this.rect_, path[0], out location2))
			{
				while (i <= num && !RectClip64.GetLocation(this.rect_, path[i], out location))
				{
					i++;
				}
				if (i > num)
				{
					foreach (Point64 pt in path)
					{
						base.Add(pt, false);
					}
					return;
				}
				if (location == RectClip64.Location.inside)
				{
					location2 = RectClip64.Location.inside;
				}
				i = 1;
			}
			if (location2 == RectClip64.Location.inside)
			{
				base.Add(path[0], false);
			}
			while (i <= num)
			{
				location = location2;
				base.GetNextLocation(path, ref location2, ref i, num);
				if (i > num)
				{
					break;
				}
				Point64 point = path[i - 1];
				RectClip64.Location location3 = location2;
				Point64 pt2;
				if (!RectClip64.GetIntersection(this.rectPath_, path[i], point, ref location3, out pt2))
				{
					i++;
				}
				else if (location2 == RectClip64.Location.inside)
				{
					base.Add(pt2, true);
				}
				else if (location != RectClip64.Location.inside)
				{
					location3 = location;
					Point64 pt3;
					RectClip64.GetIntersection(this.rectPath_, point, path[i], ref location3, out pt3);
					base.Add(pt3, true);
					base.Add(pt2, false);
				}
				else
				{
					base.Add(pt2, false);
				}
			}
		}
	}
}
