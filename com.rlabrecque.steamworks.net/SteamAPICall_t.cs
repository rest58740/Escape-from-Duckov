using System;

namespace Steamworks
{
	// Token: 0x020001CD RID: 461
	[Serializable]
	public struct SteamAPICall_t : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t>
	{
		// Token: 0x06000B74 RID: 2932 RVA: 0x00010E1C File Offset: 0x0000F01C
		public SteamAPICall_t(ulong value)
		{
			this.m_SteamAPICall = value;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00010E25 File Offset: 0x0000F025
		public override string ToString()
		{
			return this.m_SteamAPICall.ToString();
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00010E32 File Offset: 0x0000F032
		public override bool Equals(object other)
		{
			return other is SteamAPICall_t && this == (SteamAPICall_t)other;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00010E4F File Offset: 0x0000F04F
		public override int GetHashCode()
		{
			return this.m_SteamAPICall.GetHashCode();
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00010E5C File Offset: 0x0000F05C
		public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y)
		{
			return x.m_SteamAPICall == y.m_SteamAPICall;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00010E6C File Offset: 0x0000F06C
		public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00010E78 File Offset: 0x0000F078
		public static explicit operator SteamAPICall_t(ulong value)
		{
			return new SteamAPICall_t(value);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00010E80 File Offset: 0x0000F080
		public static explicit operator ulong(SteamAPICall_t that)
		{
			return that.m_SteamAPICall;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00010E88 File Offset: 0x0000F088
		public bool Equals(SteamAPICall_t other)
		{
			return this.m_SteamAPICall == other.m_SteamAPICall;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00010E98 File Offset: 0x0000F098
		public int CompareTo(SteamAPICall_t other)
		{
			return this.m_SteamAPICall.CompareTo(other.m_SteamAPICall);
		}

		// Token: 0x04000B51 RID: 2897
		public static readonly SteamAPICall_t Invalid = new SteamAPICall_t(0UL);

		// Token: 0x04000B52 RID: 2898
		public ulong m_SteamAPICall;
	}
}
