using System;

namespace Steamworks
{
	// Token: 0x020001D2 RID: 466
	[Serializable]
	public struct SteamLeaderboard_t : IEquatable<SteamLeaderboard_t>, IComparable<SteamLeaderboard_t>
	{
		// Token: 0x06000BA4 RID: 2980 RVA: 0x00011263 File Offset: 0x0000F463
		public SteamLeaderboard_t(ulong value)
		{
			this.m_SteamLeaderboard = value;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0001126C File Offset: 0x0000F46C
		public override string ToString()
		{
			return this.m_SteamLeaderboard.ToString();
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00011279 File Offset: 0x0000F479
		public override bool Equals(object other)
		{
			return other is SteamLeaderboard_t && this == (SteamLeaderboard_t)other;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00011296 File Offset: 0x0000F496
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboard.GetHashCode();
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x000112A3 File Offset: 0x0000F4A3
		public static bool operator ==(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return x.m_SteamLeaderboard == y.m_SteamLeaderboard;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x000112B3 File Offset: 0x0000F4B3
		public static bool operator !=(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x000112BF File Offset: 0x0000F4BF
		public static explicit operator SteamLeaderboard_t(ulong value)
		{
			return new SteamLeaderboard_t(value);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x000112C7 File Offset: 0x0000F4C7
		public static explicit operator ulong(SteamLeaderboard_t that)
		{
			return that.m_SteamLeaderboard;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000112CF File Offset: 0x0000F4CF
		public bool Equals(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard == other.m_SteamLeaderboard;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x000112DF File Offset: 0x0000F4DF
		public int CompareTo(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);
		}

		// Token: 0x04000B5B RID: 2907
		public ulong m_SteamLeaderboard;
	}
}
