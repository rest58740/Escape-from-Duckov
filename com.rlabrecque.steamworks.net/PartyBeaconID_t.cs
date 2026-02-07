using System;

namespace Steamworks
{
	// Token: 0x020001CB RID: 459
	[Serializable]
	public struct PartyBeaconID_t : IEquatable<PartyBeaconID_t>, IComparable<PartyBeaconID_t>
	{
		// Token: 0x06000B5F RID: 2911 RVA: 0x00010CF0 File Offset: 0x0000EEF0
		public PartyBeaconID_t(ulong value)
		{
			this.m_PartyBeaconID = value;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00010CF9 File Offset: 0x0000EEF9
		public override string ToString()
		{
			return this.m_PartyBeaconID.ToString();
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00010D06 File Offset: 0x0000EF06
		public override bool Equals(object other)
		{
			return other is PartyBeaconID_t && this == (PartyBeaconID_t)other;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00010D23 File Offset: 0x0000EF23
		public override int GetHashCode()
		{
			return this.m_PartyBeaconID.GetHashCode();
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00010D30 File Offset: 0x0000EF30
		public static bool operator ==(PartyBeaconID_t x, PartyBeaconID_t y)
		{
			return x.m_PartyBeaconID == y.m_PartyBeaconID;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00010D40 File Offset: 0x0000EF40
		public static bool operator !=(PartyBeaconID_t x, PartyBeaconID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00010D4C File Offset: 0x0000EF4C
		public static explicit operator PartyBeaconID_t(ulong value)
		{
			return new PartyBeaconID_t(value);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00010D54 File Offset: 0x0000EF54
		public static explicit operator ulong(PartyBeaconID_t that)
		{
			return that.m_PartyBeaconID;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00010D5C File Offset: 0x0000EF5C
		public bool Equals(PartyBeaconID_t other)
		{
			return this.m_PartyBeaconID == other.m_PartyBeaconID;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00010D6C File Offset: 0x0000EF6C
		public int CompareTo(PartyBeaconID_t other)
		{
			return this.m_PartyBeaconID.CompareTo(other.m_PartyBeaconID);
		}

		// Token: 0x04000B4E RID: 2894
		public static readonly PartyBeaconID_t Invalid = new PartyBeaconID_t(0UL);

		// Token: 0x04000B4F RID: 2895
		public ulong m_PartyBeaconID;
	}
}
