using System;

namespace Steamworks
{
	// Token: 0x020001C0 RID: 448
	[Serializable]
	public struct SteamNetworkingPOPID : IEquatable<SteamNetworkingPOPID>, IComparable<SteamNetworkingPOPID>
	{
		// Token: 0x06000AE9 RID: 2793 RVA: 0x0001065F File Offset: 0x0000E85F
		public SteamNetworkingPOPID(uint value)
		{
			this.m_SteamNetworkingPOPID = value;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00010668 File Offset: 0x0000E868
		public override string ToString()
		{
			return this.m_SteamNetworkingPOPID.ToString();
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00010675 File Offset: 0x0000E875
		public override bool Equals(object other)
		{
			return other is SteamNetworkingPOPID && this == (SteamNetworkingPOPID)other;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00010692 File Offset: 0x0000E892
		public override int GetHashCode()
		{
			return this.m_SteamNetworkingPOPID.GetHashCode();
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0001069F File Offset: 0x0000E89F
		public static bool operator ==(SteamNetworkingPOPID x, SteamNetworkingPOPID y)
		{
			return x.m_SteamNetworkingPOPID == y.m_SteamNetworkingPOPID;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x000106AF File Offset: 0x0000E8AF
		public static bool operator !=(SteamNetworkingPOPID x, SteamNetworkingPOPID y)
		{
			return !(x == y);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x000106BB File Offset: 0x0000E8BB
		public static explicit operator SteamNetworkingPOPID(uint value)
		{
			return new SteamNetworkingPOPID(value);
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x000106C3 File Offset: 0x0000E8C3
		public static explicit operator uint(SteamNetworkingPOPID that)
		{
			return that.m_SteamNetworkingPOPID;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000106CB File Offset: 0x0000E8CB
		public bool Equals(SteamNetworkingPOPID other)
		{
			return this.m_SteamNetworkingPOPID == other.m_SteamNetworkingPOPID;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x000106DB File Offset: 0x0000E8DB
		public int CompareTo(SteamNetworkingPOPID other)
		{
			return this.m_SteamNetworkingPOPID.CompareTo(other.m_SteamNetworkingPOPID);
		}

		// Token: 0x04000B3B RID: 2875
		public uint m_SteamNetworkingPOPID;
	}
}
