using System;

namespace Steamworks
{
	// Token: 0x020001AD RID: 429
	[Serializable]
	public struct SteamInventoryUpdateHandle_t : IEquatable<SteamInventoryUpdateHandle_t>, IComparable<SteamInventoryUpdateHandle_t>
	{
		// Token: 0x06000A44 RID: 2628 RVA: 0x0000FD0B File Offset: 0x0000DF0B
		public SteamInventoryUpdateHandle_t(ulong value)
		{
			this.m_SteamInventoryUpdateHandle = value;
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0000FD14 File Offset: 0x0000DF14
		public override string ToString()
		{
			return this.m_SteamInventoryUpdateHandle.ToString();
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0000FD21 File Offset: 0x0000DF21
		public override bool Equals(object other)
		{
			return other is SteamInventoryUpdateHandle_t && this == (SteamInventoryUpdateHandle_t)other;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0000FD3E File Offset: 0x0000DF3E
		public override int GetHashCode()
		{
			return this.m_SteamInventoryUpdateHandle.GetHashCode();
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0000FD4B File Offset: 0x0000DF4B
		public static bool operator ==(SteamInventoryUpdateHandle_t x, SteamInventoryUpdateHandle_t y)
		{
			return x.m_SteamInventoryUpdateHandle == y.m_SteamInventoryUpdateHandle;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0000FD5B File Offset: 0x0000DF5B
		public static bool operator !=(SteamInventoryUpdateHandle_t x, SteamInventoryUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0000FD67 File Offset: 0x0000DF67
		public static explicit operator SteamInventoryUpdateHandle_t(ulong value)
		{
			return new SteamInventoryUpdateHandle_t(value);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0000FD6F File Offset: 0x0000DF6F
		public static explicit operator ulong(SteamInventoryUpdateHandle_t that)
		{
			return that.m_SteamInventoryUpdateHandle;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0000FD77 File Offset: 0x0000DF77
		public bool Equals(SteamInventoryUpdateHandle_t other)
		{
			return this.m_SteamInventoryUpdateHandle == other.m_SteamInventoryUpdateHandle;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0000FD87 File Offset: 0x0000DF87
		public int CompareTo(SteamInventoryUpdateHandle_t other)
		{
			return this.m_SteamInventoryUpdateHandle.CompareTo(other.m_SteamInventoryUpdateHandle);
		}

		// Token: 0x04000AEE RID: 2798
		public static readonly SteamInventoryUpdateHandle_t Invalid = new SteamInventoryUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000AEF RID: 2799
		public ulong m_SteamInventoryUpdateHandle;
	}
}
