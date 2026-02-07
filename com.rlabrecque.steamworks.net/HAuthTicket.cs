using System;

namespace Steamworks
{
	// Token: 0x0200019F RID: 415
	[Serializable]
	public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
	{
		// Token: 0x060009D4 RID: 2516 RVA: 0x0000F70C File Offset: 0x0000D90C
		public HAuthTicket(uint value)
		{
			this.m_HAuthTicket = value;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0000F715 File Offset: 0x0000D915
		public override string ToString()
		{
			return this.m_HAuthTicket.ToString();
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0000F722 File Offset: 0x0000D922
		public override bool Equals(object other)
		{
			return other is HAuthTicket && this == (HAuthTicket)other;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0000F73F File Offset: 0x0000D93F
		public override int GetHashCode()
		{
			return this.m_HAuthTicket.GetHashCode();
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0000F74C File Offset: 0x0000D94C
		public static bool operator ==(HAuthTicket x, HAuthTicket y)
		{
			return x.m_HAuthTicket == y.m_HAuthTicket;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0000F75C File Offset: 0x0000D95C
		public static bool operator !=(HAuthTicket x, HAuthTicket y)
		{
			return !(x == y);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0000F768 File Offset: 0x0000D968
		public static explicit operator HAuthTicket(uint value)
		{
			return new HAuthTicket(value);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0000F770 File Offset: 0x0000D970
		public static explicit operator uint(HAuthTicket that)
		{
			return that.m_HAuthTicket;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0000F778 File Offset: 0x0000D978
		public bool Equals(HAuthTicket other)
		{
			return this.m_HAuthTicket == other.m_HAuthTicket;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0000F788 File Offset: 0x0000D988
		public int CompareTo(HAuthTicket other)
		{
			return this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);
		}

		// Token: 0x04000ACF RID: 2767
		public static readonly HAuthTicket Invalid = new HAuthTicket(0U);

		// Token: 0x04000AD0 RID: 2768
		public uint m_HAuthTicket;
	}
}
