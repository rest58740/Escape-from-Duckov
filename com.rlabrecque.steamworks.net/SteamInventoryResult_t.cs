using System;

namespace Steamworks
{
	// Token: 0x020001AC RID: 428
	[Serializable]
	public struct SteamInventoryResult_t : IEquatable<SteamInventoryResult_t>, IComparable<SteamInventoryResult_t>
	{
		// Token: 0x06000A39 RID: 2617 RVA: 0x0000FC6F File Offset: 0x0000DE6F
		public SteamInventoryResult_t(int value)
		{
			this.m_SteamInventoryResult = value;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0000FC78 File Offset: 0x0000DE78
		public override string ToString()
		{
			return this.m_SteamInventoryResult.ToString();
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0000FC85 File Offset: 0x0000DE85
		public override bool Equals(object other)
		{
			return other is SteamInventoryResult_t && this == (SteamInventoryResult_t)other;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0000FCA2 File Offset: 0x0000DEA2
		public override int GetHashCode()
		{
			return this.m_SteamInventoryResult.GetHashCode();
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0000FCAF File Offset: 0x0000DEAF
		public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return x.m_SteamInventoryResult == y.m_SteamInventoryResult;
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0000FCBF File Offset: 0x0000DEBF
		public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0000FCCB File Offset: 0x0000DECB
		public static explicit operator SteamInventoryResult_t(int value)
		{
			return new SteamInventoryResult_t(value);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0000FCD3 File Offset: 0x0000DED3
		public static explicit operator int(SteamInventoryResult_t that)
		{
			return that.m_SteamInventoryResult;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0000FCDB File Offset: 0x0000DEDB
		public bool Equals(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult == other.m_SteamInventoryResult;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0000FCEB File Offset: 0x0000DEEB
		public int CompareTo(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
		}

		// Token: 0x04000AEC RID: 2796
		public static readonly SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);

		// Token: 0x04000AED RID: 2797
		public int m_SteamInventoryResult;
	}
}
