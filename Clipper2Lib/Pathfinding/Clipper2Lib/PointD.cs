using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000008 RID: 8
	public struct PointD
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002288 File Offset: 0x00000488
		public PointD(PointD pt)
		{
			this.x = pt.x;
			this.y = pt.y;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022A2 File Offset: 0x000004A2
		public PointD(Point64 pt)
		{
			this.x = (double)pt.X;
			this.y = (double)pt.Y;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022BE File Offset: 0x000004BE
		public PointD(PointD pt, double scale)
		{
			this.x = pt.x * scale;
			this.y = pt.y * scale;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000022DC File Offset: 0x000004DC
		public PointD(Point64 pt, double scale)
		{
			this.x = (double)pt.X * scale;
			this.y = (double)pt.Y * scale;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000022FC File Offset: 0x000004FC
		public PointD(long x, long y)
		{
			this.x = (double)x;
			this.y = (double)y;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000230E File Offset: 0x0000050E
		public PointD(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002320 File Offset: 0x00000520
		[NullableContext(1)]
		public readonly string ToString(int precision = 2)
		{
			return string.Format(string.Format("{{0:F{0}}},{{1:F{1}}}", new object[]
			{
				precision,
				precision
			}), new object[]
			{
				this.x,
				this.y
			});
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002376 File Offset: 0x00000576
		public static bool operator ==(PointD lhs, PointD rhs)
		{
			return InternalClipper.IsAlmostZero(lhs.x - rhs.x) && InternalClipper.IsAlmostZero(lhs.y - rhs.y);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023A0 File Offset: 0x000005A0
		public static bool operator !=(PointD lhs, PointD rhs)
		{
			return !InternalClipper.IsAlmostZero(lhs.x - rhs.x) || !InternalClipper.IsAlmostZero(lhs.y - rhs.y);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023D0 File Offset: 0x000005D0
		[NullableContext(2)]
		public override readonly bool Equals(object obj)
		{
			if (obj != null && obj is PointD)
			{
				PointD rhs = (PointD)obj;
				return this == rhs;
			}
			return false;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023FD File Offset: 0x000005FD
		public void Negate()
		{
			this.x = -this.x;
			this.y = -this.y;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000241C File Offset: 0x0000061C
		public override readonly int GetHashCode()
		{
			int num = 1861411795 * -1521134295;
			double num2 = this.x;
			int num3 = (num + num2.GetHashCode()) * -1521134295;
			num2 = this.y;
			return num3 + num2.GetHashCode();
		}

		// Token: 0x04000005 RID: 5
		public double x;

		// Token: 0x04000006 RID: 6
		public double y;
	}
}
