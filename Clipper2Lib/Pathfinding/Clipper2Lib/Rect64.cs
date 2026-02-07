using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000009 RID: 9
	public struct Rect64
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002458 File Offset: 0x00000658
		public Rect64(long l, long t, long r, long b)
		{
			this.left = l;
			this.top = t;
			this.right = r;
			this.bottom = b;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002478 File Offset: 0x00000678
		public Rect64(bool isValid)
		{
			if (isValid)
			{
				this.left = 0L;
				this.top = 0L;
				this.right = 0L;
				this.bottom = 0L;
				return;
			}
			this.left = long.MaxValue;
			this.top = long.MaxValue;
			this.right = long.MinValue;
			this.bottom = long.MinValue;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000024E5 File Offset: 0x000006E5
		public Rect64(Rect64 rec)
		{
			this.left = rec.left;
			this.top = rec.top;
			this.right = rec.right;
			this.bottom = rec.bottom;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002517 File Offset: 0x00000717
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002526 File Offset: 0x00000726
		public long Width
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002536 File Offset: 0x00000736
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002545 File Offset: 0x00000745
		public long Height
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

		// Token: 0x06000027 RID: 39 RVA: 0x00002555 File Offset: 0x00000755
		public readonly bool IsEmpty()
		{
			return this.bottom <= this.top || this.right <= this.left;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002578 File Offset: 0x00000778
		public readonly bool IsValid()
		{
			return this.left < long.MaxValue;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000258B File Offset: 0x0000078B
		public readonly Point64 MidPoint()
		{
			return new Point64((this.left + this.right) / 2L, (this.top + this.bottom) / 2L);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000025B2 File Offset: 0x000007B2
		public readonly bool Contains(Point64 pt)
		{
			return pt.X > this.left && pt.X < this.right && pt.Y > this.top && pt.Y < this.bottom;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000025EE File Offset: 0x000007EE
		public readonly bool Contains(Rect64 rec)
		{
			return rec.left >= this.left && rec.right <= this.right && rec.top >= this.top && rec.bottom <= this.bottom;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002630 File Offset: 0x00000830
		public readonly bool Intersects(Rect64 rec)
		{
			return Math.Max(this.left, rec.left) <= Math.Min(this.right, rec.right) && Math.Max(this.top, rec.top) <= Math.Min(this.bottom, rec.bottom);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000268C File Offset: 0x0000088C
		[NullableContext(1)]
		public readonly List<Point64> AsPath()
		{
			return new List<Point64>(4)
			{
				new Point64(this.left, this.top),
				new Point64(this.right, this.top),
				new Point64(this.right, this.bottom),
				new Point64(this.left, this.bottom)
			};
		}

		// Token: 0x04000007 RID: 7
		public long left;

		// Token: 0x04000008 RID: 8
		public long top;

		// Token: 0x04000009 RID: 9
		public long right;

		// Token: 0x0400000A RID: 10
		public long bottom;
	}
}
