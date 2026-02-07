using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200000A RID: 10
	public struct RectD
	{
		// Token: 0x0600002E RID: 46 RVA: 0x000026FB File Offset: 0x000008FB
		public RectD(double l, double t, double r, double b)
		{
			this.left = l;
			this.top = t;
			this.right = r;
			this.bottom = b;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000271A File Offset: 0x0000091A
		public RectD(RectD rec)
		{
			this.left = rec.left;
			this.top = rec.top;
			this.right = rec.right;
			this.bottom = rec.bottom;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000274C File Offset: 0x0000094C
		public RectD(bool isValid)
		{
			if (isValid)
			{
				this.left = 0.0;
				this.top = 0.0;
				this.right = 0.0;
				this.bottom = 0.0;
				return;
			}
			this.left = double.MaxValue;
			this.top = double.MaxValue;
			this.right = double.MinValue;
			this.bottom = double.MinValue;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000027D5 File Offset: 0x000009D5
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000027E4 File Offset: 0x000009E4
		public double Width
		{
			readonly get
			{
				return this.right - this.left;
			}
			set
			{
				this.right = this.left + value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000027F4 File Offset: 0x000009F4
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002803 File Offset: 0x00000A03
		public double Height
		{
			readonly get
			{
				return this.bottom - this.top;
			}
			set
			{
				this.bottom = this.top + value;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002813 File Offset: 0x00000A13
		public readonly bool IsEmpty()
		{
			return this.bottom <= this.top || this.right <= this.left;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002836 File Offset: 0x00000A36
		public readonly PointD MidPoint()
		{
			return new PointD((this.left + this.right) / 2.0, (this.top + this.bottom) / 2.0);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000286B File Offset: 0x00000A6B
		public readonly bool Contains(PointD pt)
		{
			return pt.x > this.left && pt.x < this.right && pt.y > this.top && pt.y < this.bottom;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000028A7 File Offset: 0x00000AA7
		public readonly bool Contains(RectD rec)
		{
			return rec.left >= this.left && rec.right <= this.right && rec.top >= this.top && rec.bottom <= this.bottom;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000028E8 File Offset: 0x00000AE8
		public readonly bool Intersects(RectD rec)
		{
			return Math.Max(this.left, rec.left) < Math.Min(this.right, rec.right) && Math.Max(this.top, rec.top) < Math.Min(this.bottom, rec.bottom);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002940 File Offset: 0x00000B40
		[NullableContext(1)]
		public readonly PathD AsPath()
		{
			return new PathD(4)
			{
				new PointD(this.left, this.top),
				new PointD(this.right, this.top),
				new PointD(this.right, this.bottom),
				new PointD(this.left, this.bottom)
			};
		}

		// Token: 0x0400000B RID: 11
		public double left;

		// Token: 0x0400000C RID: 12
		public double top;

		// Token: 0x0400000D RID: 13
		public double right;

		// Token: 0x0400000E RID: 14
		public double bottom;
	}
}
