using System;

namespace Steamworks
{
	// Token: 0x020001A2 RID: 418
	[Serializable]
	public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
	{
		// Token: 0x060009E1 RID: 2529 RVA: 0x0000F7C3 File Offset: 0x0000D9C3
		public FriendsGroupID_t(short value)
		{
			this.m_FriendsGroupID = value;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0000F7CC File Offset: 0x0000D9CC
		public override string ToString()
		{
			return this.m_FriendsGroupID.ToString();
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0000F7D9 File Offset: 0x0000D9D9
		public override bool Equals(object other)
		{
			return other is FriendsGroupID_t && this == (FriendsGroupID_t)other;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0000F7F6 File Offset: 0x0000D9F6
		public override int GetHashCode()
		{
			return this.m_FriendsGroupID.GetHashCode();
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0000F803 File Offset: 0x0000DA03
		public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return x.m_FriendsGroupID == y.m_FriendsGroupID;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0000F813 File Offset: 0x0000DA13
		public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return !(x == y);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0000F81F File Offset: 0x0000DA1F
		public static explicit operator FriendsGroupID_t(short value)
		{
			return new FriendsGroupID_t(value);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0000F827 File Offset: 0x0000DA27
		public static explicit operator short(FriendsGroupID_t that)
		{
			return that.m_FriendsGroupID;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0000F82F File Offset: 0x0000DA2F
		public bool Equals(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID == other.m_FriendsGroupID;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0000F83F File Offset: 0x0000DA3F
		public int CompareTo(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);
		}

		// Token: 0x04000ADD RID: 2781
		public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t(-1);

		// Token: 0x04000ADE RID: 2782
		public short m_FriendsGroupID;
	}
}
