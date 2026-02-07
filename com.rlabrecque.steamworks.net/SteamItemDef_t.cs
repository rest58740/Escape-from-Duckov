using System;

namespace Steamworks
{
	// Token: 0x020001AE RID: 430
	[Serializable]
	public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t>
	{
		// Token: 0x06000A4F RID: 2639 RVA: 0x0000FDA8 File Offset: 0x0000DFA8
		public SteamItemDef_t(int value)
		{
			this.m_SteamItemDef = value;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0000FDB1 File Offset: 0x0000DFB1
		public override string ToString()
		{
			return this.m_SteamItemDef.ToString();
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0000FDBE File Offset: 0x0000DFBE
		public override bool Equals(object other)
		{
			return other is SteamItemDef_t && this == (SteamItemDef_t)other;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0000FDDB File Offset: 0x0000DFDB
		public override int GetHashCode()
		{
			return this.m_SteamItemDef.GetHashCode();
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0000FDE8 File Offset: 0x0000DFE8
		public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y)
		{
			return x.m_SteamItemDef == y.m_SteamItemDef;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0000FDF8 File Offset: 0x0000DFF8
		public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0000FE04 File Offset: 0x0000E004
		public static explicit operator SteamItemDef_t(int value)
		{
			return new SteamItemDef_t(value);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0000FE0C File Offset: 0x0000E00C
		public static explicit operator int(SteamItemDef_t that)
		{
			return that.m_SteamItemDef;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0000FE14 File Offset: 0x0000E014
		public bool Equals(SteamItemDef_t other)
		{
			return this.m_SteamItemDef == other.m_SteamItemDef;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0000FE24 File Offset: 0x0000E024
		public int CompareTo(SteamItemDef_t other)
		{
			return this.m_SteamItemDef.CompareTo(other.m_SteamItemDef);
		}

		// Token: 0x04000AF0 RID: 2800
		public int m_SteamItemDef;
	}
}
