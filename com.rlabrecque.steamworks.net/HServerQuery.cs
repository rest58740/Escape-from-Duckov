using System;

namespace Steamworks
{
	// Token: 0x020001B1 RID: 433
	[Serializable]
	public struct HServerQuery : IEquatable<HServerQuery>, IComparable<HServerQuery>
	{
		// Token: 0x06000A6E RID: 2670 RVA: 0x0000FF67 File Offset: 0x0000E167
		public HServerQuery(int value)
		{
			this.m_HServerQuery = value;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0000FF70 File Offset: 0x0000E170
		public override string ToString()
		{
			return this.m_HServerQuery.ToString();
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0000FF7D File Offset: 0x0000E17D
		public override bool Equals(object other)
		{
			return other is HServerQuery && this == (HServerQuery)other;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0000FF9A File Offset: 0x0000E19A
		public override int GetHashCode()
		{
			return this.m_HServerQuery.GetHashCode();
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0000FFA7 File Offset: 0x0000E1A7
		public static bool operator ==(HServerQuery x, HServerQuery y)
		{
			return x.m_HServerQuery == y.m_HServerQuery;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0000FFB7 File Offset: 0x0000E1B7
		public static bool operator !=(HServerQuery x, HServerQuery y)
		{
			return !(x == y);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0000FFC3 File Offset: 0x0000E1C3
		public static explicit operator HServerQuery(int value)
		{
			return new HServerQuery(value);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0000FFCB File Offset: 0x0000E1CB
		public static explicit operator int(HServerQuery that)
		{
			return that.m_HServerQuery;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0000FFD3 File Offset: 0x0000E1D3
		public bool Equals(HServerQuery other)
		{
			return this.m_HServerQuery == other.m_HServerQuery;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0000FFE3 File Offset: 0x0000E1E3
		public int CompareTo(HServerQuery other)
		{
			return this.m_HServerQuery.CompareTo(other.m_HServerQuery);
		}

		// Token: 0x04000AF5 RID: 2805
		public static readonly HServerQuery Invalid = new HServerQuery(-1);

		// Token: 0x04000AF6 RID: 2806
		public int m_HServerQuery;
	}
}
