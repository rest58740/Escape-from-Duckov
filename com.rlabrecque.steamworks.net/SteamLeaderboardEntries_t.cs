using System;

namespace Steamworks
{
	// Token: 0x020001D1 RID: 465
	[Serializable]
	public struct SteamLeaderboardEntries_t : IEquatable<SteamLeaderboardEntries_t>, IComparable<SteamLeaderboardEntries_t>
	{
		// Token: 0x06000B9A RID: 2970 RVA: 0x000111D4 File Offset: 0x0000F3D4
		public SteamLeaderboardEntries_t(ulong value)
		{
			this.m_SteamLeaderboardEntries = value;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x000111DD File Offset: 0x0000F3DD
		public override string ToString()
		{
			return this.m_SteamLeaderboardEntries.ToString();
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x000111EA File Offset: 0x0000F3EA
		public override bool Equals(object other)
		{
			return other is SteamLeaderboardEntries_t && this == (SteamLeaderboardEntries_t)other;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00011207 File Offset: 0x0000F407
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboardEntries.GetHashCode();
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00011214 File Offset: 0x0000F414
		public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return x.m_SteamLeaderboardEntries == y.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00011224 File Offset: 0x0000F424
		public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00011230 File Offset: 0x0000F430
		public static explicit operator SteamLeaderboardEntries_t(ulong value)
		{
			return new SteamLeaderboardEntries_t(value);
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00011238 File Offset: 0x0000F438
		public static explicit operator ulong(SteamLeaderboardEntries_t that)
		{
			return that.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00011240 File Offset: 0x0000F440
		public bool Equals(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries == other.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00011250 File Offset: 0x0000F450
		public int CompareTo(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
		}

		// Token: 0x04000B5A RID: 2906
		public ulong m_SteamLeaderboardEntries;
	}
}
