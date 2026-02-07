using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public struct IntRect
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000A2F8 File Offset: 0x000084F8
		public IntRect(int xmin, int ymin, int xmax, int ymax)
		{
			this.xmin = xmin;
			this.xmax = xmax;
			this.ymin = ymin;
			this.ymax = ymax;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A317 File Offset: 0x00008517
		public bool Contains(int x, int y)
		{
			return x >= this.xmin && y >= this.ymin && x <= this.xmax && y <= this.ymax;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000A342 File Offset: 0x00008542
		public bool Contains(IntRect other)
		{
			return this.xmin <= other.xmin && this.xmax >= other.xmax && this.ymin <= other.ymin && this.ymax >= other.ymax;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000A381 File Offset: 0x00008581
		public Vector2Int Min
		{
			get
			{
				return new Vector2Int(this.xmin, this.ymin);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000A394 File Offset: 0x00008594
		public Vector2Int Max
		{
			get
			{
				return new Vector2Int(this.xmax, this.ymax);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000A3A7 File Offset: 0x000085A7
		public int Width
		{
			get
			{
				return this.xmax - this.xmin + 1;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000A3B8 File Offset: 0x000085B8
		public int Height
		{
			get
			{
				return this.ymax - this.ymin + 1;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000A3C9 File Offset: 0x000085C9
		public int Area
		{
			get
			{
				return this.Width * this.Height;
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000A3D8 File Offset: 0x000085D8
		public bool IsValid()
		{
			return this.xmin <= this.xmax && this.ymin <= this.ymax;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000A3FB File Offset: 0x000085FB
		public static bool operator ==(IntRect a, IntRect b)
		{
			return a.xmin == b.xmin && a.xmax == b.xmax && a.ymin == b.ymin && a.ymax == b.ymax;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000A437 File Offset: 0x00008637
		public static bool operator !=(IntRect a, IntRect b)
		{
			return a.xmin != b.xmin || a.xmax != b.xmax || a.ymin != b.ymin || a.ymax != b.ymax;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000A476 File Offset: 0x00008676
		public static explicit operator Rect(IntRect r)
		{
			return new Rect((float)r.xmin, (float)r.ymin, (float)r.Width, (float)r.Height);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000A49C File Offset: 0x0000869C
		public override bool Equals(object obj)
		{
			if (!(obj is IntRect))
			{
				return false;
			}
			IntRect intRect = (IntRect)obj;
			return this.xmin == intRect.xmin && this.xmax == intRect.xmax && this.ymin == intRect.ymin && this.ymax == intRect.ymax;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000A4F4 File Offset: 0x000086F4
		public override int GetHashCode()
		{
			return this.xmin * 131071 ^ this.xmax * 3571 ^ this.ymin * 3109 ^ this.ymax * 7;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000A528 File Offset: 0x00008728
		public static IntRect Intersection(IntRect a, IntRect b)
		{
			return new IntRect(Math.Max(a.xmin, b.xmin), Math.Max(a.ymin, b.ymin), Math.Min(a.xmax, b.xmax), Math.Min(a.ymax, b.ymax));
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A57E File Offset: 0x0000877E
		public static bool Intersects(IntRect a, IntRect b)
		{
			return a.xmin <= b.xmax && a.ymin <= b.ymax && a.xmax >= b.xmin && a.ymax >= b.ymin;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000A5C0 File Offset: 0x000087C0
		public static IntRect Union(IntRect a, IntRect b)
		{
			return new IntRect(Math.Min(a.xmin, b.xmin), Math.Min(a.ymin, b.ymin), Math.Max(a.xmax, b.xmax), Math.Max(a.ymax, b.ymax));
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000A618 File Offset: 0x00008818
		public static IntRect Exclude(IntRect a, IntRect b)
		{
			if (!b.IsValid() || !a.IsValid())
			{
				return a;
			}
			IntRect intRect = IntRect.Intersection(a, b);
			if (!intRect.IsValid())
			{
				return a;
			}
			if (a.xmin == intRect.xmin && a.xmax == intRect.xmax)
			{
				if (a.ymin == intRect.ymin)
				{
					a.ymin = intRect.ymax + 1;
					return a;
				}
				if (a.ymax == intRect.ymax)
				{
					a.ymax = intRect.ymin - 1;
					return a;
				}
				throw new ArgumentException("B splits A into two disjoint parts");
			}
			else
			{
				if (a.ymin != intRect.ymin || a.ymax != intRect.ymax)
				{
					throw new ArgumentException("B covers either a corner of A, or does not touch the edges of A at all");
				}
				if (a.xmin == intRect.xmin)
				{
					a.xmin = intRect.xmax + 1;
					return a;
				}
				if (a.xmax == intRect.xmax)
				{
					a.xmax = intRect.xmin - 1;
					return a;
				}
				throw new ArgumentException("B splits A into two disjoint parts");
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000A720 File Offset: 0x00008920
		public IntRect ExpandToContain(int x, int y)
		{
			return new IntRect(Math.Min(this.xmin, x), Math.Min(this.ymin, y), Math.Max(this.xmax, x), Math.Max(this.ymax, y));
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000A757 File Offset: 0x00008957
		public IntRect Offset(Vector2Int offset)
		{
			return new IntRect(this.xmin + offset.x, this.ymin + offset.y, this.xmax + offset.x, this.ymax + offset.y);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000A796 File Offset: 0x00008996
		public IntRect Expand(int range)
		{
			return new IntRect(this.xmin - range, this.ymin - range, this.xmax + range, this.ymax + range);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A7C0 File Offset: 0x000089C0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[x: ",
				this.xmin.ToString(),
				"...",
				this.xmax.ToString(),
				", y: ",
				this.ymin.ToString(),
				"...",
				this.ymax.ToString(),
				"]"
			});
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000A83C File Offset: 0x00008A3C
		public List<Vector2Int> GetInnerCoordinates()
		{
			List<Vector2Int> list = ListPool<Vector2Int>.Claim(this.Width * this.Height);
			for (int i = this.ymin; i <= this.ymax; i++)
			{
				for (int j = this.xmin; j <= this.xmax; j++)
				{
					list.Add(new Vector2Int(j, i));
				}
			}
			return list;
		}

		// Token: 0x04000172 RID: 370
		public int xmin;

		// Token: 0x04000173 RID: 371
		public int ymin;

		// Token: 0x04000174 RID: 372
		public int xmax;

		// Token: 0x04000175 RID: 373
		public int ymax;
	}
}
