using System;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x02000060 RID: 96
	public struct IntBounds
	{
		// Token: 0x06000362 RID: 866 RVA: 0x0001036A File Offset: 0x0000E56A
		public IntBounds(int xmin, int ymin, int zmin, int xmax, int ymax, int zmax)
		{
			this.min = new int3(xmin, ymin, zmin);
			this.max = new int3(xmax, ymax, zmax);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0001038B File Offset: 0x0000E58B
		public IntBounds(int3 min, int3 max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0001039B File Offset: 0x0000E59B
		public int3 size
		{
			get
			{
				return this.max - this.min;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000365 RID: 869 RVA: 0x000103B0 File Offset: 0x0000E5B0
		public int volume
		{
			get
			{
				int3 size = this.size;
				return size.x * size.y * size.z;
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x000103D8 File Offset: 0x0000E5D8
		public static IntBounds Intersection(IntBounds a, IntBounds b)
		{
			return new IntBounds(math.max(a.min, b.min), math.min(a.max, b.max));
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00010401 File Offset: 0x0000E601
		public static bool Intersects(IntBounds a, IntBounds b)
		{
			return math.all(a.min < b.max & a.max > b.min);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0001042F File Offset: 0x0000E62F
		public IntBounds Offset(int3 offset)
		{
			return new IntBounds(this.min + offset, this.max + offset);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0001044E File Offset: 0x0000E64E
		public bool Contains(IntBounds other)
		{
			return math.all(other.min >= this.min & other.max <= this.max);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0001047C File Offset: 0x0000E67C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.min.ToString(),
				" <= x < ",
				this.max.ToString(),
				")"
			});
		}

		// Token: 0x0600036B RID: 875 RVA: 0x000104D4 File Offset: 0x0000E6D4
		public override bool Equals(object _b)
		{
			IntBounds b = (IntBounds)_b;
			return this == b;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x000104F4 File Offset: 0x0000E6F4
		public override int GetHashCode()
		{
			return this.min.GetHashCode() ^ this.max.GetHashCode() << 2;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0001051B File Offset: 0x0000E71B
		public static bool operator ==(IntBounds a, IntBounds b)
		{
			return math.all(a.min == b.min & a.max == b.max);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00010549 File Offset: 0x0000E749
		public static bool operator !=(IntBounds a, IntBounds b)
		{
			return !(a == b);
		}

		// Token: 0x04000205 RID: 517
		public int3 min;

		// Token: 0x04000206 RID: 518
		public int3 max;
	}
}
