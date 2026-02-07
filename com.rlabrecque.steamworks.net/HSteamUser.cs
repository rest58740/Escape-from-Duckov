using System;

namespace Steamworks
{
	// Token: 0x020001D4 RID: 468
	[Serializable]
	public struct HSteamUser : IEquatable<HSteamUser>, IComparable<HSteamUser>
	{
		// Token: 0x06000BB8 RID: 3000 RVA: 0x00011381 File Offset: 0x0000F581
		public HSteamUser(int value)
		{
			this.m_HSteamUser = value;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0001138A File Offset: 0x0000F58A
		public override string ToString()
		{
			return this.m_HSteamUser.ToString();
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x00011397 File Offset: 0x0000F597
		public override bool Equals(object other)
		{
			return other is HSteamUser && this == (HSteamUser)other;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000113B4 File Offset: 0x0000F5B4
		public override int GetHashCode()
		{
			return this.m_HSteamUser.GetHashCode();
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000113C1 File Offset: 0x0000F5C1
		public static bool operator ==(HSteamUser x, HSteamUser y)
		{
			return x.m_HSteamUser == y.m_HSteamUser;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x000113D1 File Offset: 0x0000F5D1
		public static bool operator !=(HSteamUser x, HSteamUser y)
		{
			return !(x == y);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000113DD File Offset: 0x0000F5DD
		public static explicit operator HSteamUser(int value)
		{
			return new HSteamUser(value);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x000113E5 File Offset: 0x0000F5E5
		public static explicit operator int(HSteamUser that)
		{
			return that.m_HSteamUser;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x000113ED File Offset: 0x0000F5ED
		public bool Equals(HSteamUser other)
		{
			return this.m_HSteamUser == other.m_HSteamUser;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x000113FD File Offset: 0x0000F5FD
		public int CompareTo(HSteamUser other)
		{
			return this.m_HSteamUser.CompareTo(other.m_HSteamUser);
		}

		// Token: 0x04000B5D RID: 2909
		public int m_HSteamUser;
	}
}
