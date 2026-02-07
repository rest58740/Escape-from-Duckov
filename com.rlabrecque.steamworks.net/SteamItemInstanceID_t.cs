using System;

namespace Steamworks
{
	// Token: 0x020001AF RID: 431
	[Serializable]
	public struct SteamItemInstanceID_t : IEquatable<SteamItemInstanceID_t>, IComparable<SteamItemInstanceID_t>
	{
		// Token: 0x06000A59 RID: 2649 RVA: 0x0000FE37 File Offset: 0x0000E037
		public SteamItemInstanceID_t(ulong value)
		{
			this.m_SteamItemInstanceID = value;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0000FE40 File Offset: 0x0000E040
		public override string ToString()
		{
			return this.m_SteamItemInstanceID.ToString();
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0000FE4D File Offset: 0x0000E04D
		public override bool Equals(object other)
		{
			return other is SteamItemInstanceID_t && this == (SteamItemInstanceID_t)other;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0000FE6A File Offset: 0x0000E06A
		public override int GetHashCode()
		{
			return this.m_SteamItemInstanceID.GetHashCode();
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0000FE77 File Offset: 0x0000E077
		public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return x.m_SteamItemInstanceID == y.m_SteamItemInstanceID;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0000FE87 File Offset: 0x0000E087
		public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0000FE93 File Offset: 0x0000E093
		public static explicit operator SteamItemInstanceID_t(ulong value)
		{
			return new SteamItemInstanceID_t(value);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0000FE9B File Offset: 0x0000E09B
		public static explicit operator ulong(SteamItemInstanceID_t that)
		{
			return that.m_SteamItemInstanceID;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0000FEA3 File Offset: 0x0000E0A3
		public bool Equals(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID == other.m_SteamItemInstanceID;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0000FEB3 File Offset: 0x0000E0B3
		public int CompareTo(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);
		}

		// Token: 0x04000AF1 RID: 2801
		public static readonly SteamItemInstanceID_t Invalid = new SteamItemInstanceID_t(ulong.MaxValue);

		// Token: 0x04000AF2 RID: 2802
		public ulong m_SteamItemInstanceID;
	}
}
