using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000007 RID: 7
	public struct Point64
	{
		// Token: 0x06000007 RID: 7 RVA: 0x0000209E File Offset: 0x0000029E
		public Point64(Point64 pt)
		{
			this.X = pt.X;
			this.Y = pt.Y;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020B8 File Offset: 0x000002B8
		public Point64(long x, long y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020C8 File Offset: 0x000002C8
		public Point64(double x, double y)
		{
			this.X = (long)Math.Round(x, MidpointRounding.AwayFromZero);
			this.Y = (long)Math.Round(y, MidpointRounding.AwayFromZero);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020E6 File Offset: 0x000002E6
		public Point64(PointD pt)
		{
			this.X = (long)Math.Round(pt.x, MidpointRounding.AwayFromZero);
			this.Y = (long)Math.Round(pt.y, MidpointRounding.AwayFromZero);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000210E File Offset: 0x0000030E
		public Point64(Point64 pt, double scale)
		{
			this.X = (long)Math.Round((double)pt.X * scale, MidpointRounding.AwayFromZero);
			this.Y = (long)Math.Round((double)pt.Y * scale, MidpointRounding.AwayFromZero);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000213C File Offset: 0x0000033C
		public Point64(PointD pt, double scale)
		{
			this.X = (long)Math.Round(pt.x * scale, MidpointRounding.AwayFromZero);
			this.Y = (long)Math.Round(pt.y * scale, MidpointRounding.AwayFromZero);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002168 File Offset: 0x00000368
		public static bool operator ==(Point64 lhs, Point64 rhs)
		{
			return lhs.X == rhs.X && lhs.Y == rhs.Y;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002188 File Offset: 0x00000388
		public static bool operator !=(Point64 lhs, Point64 rhs)
		{
			return lhs.X != rhs.X || lhs.Y != rhs.Y;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021AB File Offset: 0x000003AB
		public static Point64 operator +(Point64 lhs, Point64 rhs)
		{
			return new Point64(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021CC File Offset: 0x000003CC
		public static Point64 operator -(Point64 lhs, Point64 rhs)
		{
			return new Point64(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021ED File Offset: 0x000003ED
		[NullableContext(1)]
		public override readonly string ToString()
		{
			return string.Format("{0},{1} ", new object[]
			{
				this.X,
				this.Y
			});
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000221C File Offset: 0x0000041C
		[NullableContext(2)]
		public override readonly bool Equals(object obj)
		{
			if (obj != null && obj is Point64)
			{
				Point64 rhs = (Point64)obj;
				return this == rhs;
			}
			return false;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000224C File Offset: 0x0000044C
		public override readonly int GetHashCode()
		{
			int num = 1861411795 * -1521134295;
			long num2 = this.X;
			int num3 = (num + num2.GetHashCode()) * -1521134295;
			num2 = this.Y;
			return num3 + num2.GetHashCode();
		}

		// Token: 0x04000003 RID: 3
		public long X;

		// Token: 0x04000004 RID: 4
		public long Y;
	}
}
