using System;

namespace Steamworks
{
	// Token: 0x020001BF RID: 447
	[Serializable]
	public struct SteamNetworkingMicroseconds : IEquatable<SteamNetworkingMicroseconds>, IComparable<SteamNetworkingMicroseconds>
	{
		// Token: 0x06000ADF RID: 2783 RVA: 0x000105D0 File Offset: 0x0000E7D0
		public SteamNetworkingMicroseconds(long value)
		{
			this.m_SteamNetworkingMicroseconds = value;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000105D9 File Offset: 0x0000E7D9
		public override string ToString()
		{
			return this.m_SteamNetworkingMicroseconds.ToString();
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000105E6 File Offset: 0x0000E7E6
		public override bool Equals(object other)
		{
			return other is SteamNetworkingMicroseconds && this == (SteamNetworkingMicroseconds)other;
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00010603 File Offset: 0x0000E803
		public override int GetHashCode()
		{
			return this.m_SteamNetworkingMicroseconds.GetHashCode();
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00010610 File Offset: 0x0000E810
		public static bool operator ==(SteamNetworkingMicroseconds x, SteamNetworkingMicroseconds y)
		{
			return x.m_SteamNetworkingMicroseconds == y.m_SteamNetworkingMicroseconds;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00010620 File Offset: 0x0000E820
		public static bool operator !=(SteamNetworkingMicroseconds x, SteamNetworkingMicroseconds y)
		{
			return !(x == y);
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0001062C File Offset: 0x0000E82C
		public static explicit operator SteamNetworkingMicroseconds(long value)
		{
			return new SteamNetworkingMicroseconds(value);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00010634 File Offset: 0x0000E834
		public static explicit operator long(SteamNetworkingMicroseconds that)
		{
			return that.m_SteamNetworkingMicroseconds;
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0001063C File Offset: 0x0000E83C
		public bool Equals(SteamNetworkingMicroseconds other)
		{
			return this.m_SteamNetworkingMicroseconds == other.m_SteamNetworkingMicroseconds;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0001064C File Offset: 0x0000E84C
		public int CompareTo(SteamNetworkingMicroseconds other)
		{
			return this.m_SteamNetworkingMicroseconds.CompareTo(other.m_SteamNetworkingMicroseconds);
		}

		// Token: 0x04000B3A RID: 2874
		public long m_SteamNetworkingMicroseconds;
	}
}
