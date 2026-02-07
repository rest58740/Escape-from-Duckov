using System;

namespace Steamworks
{
	// Token: 0x020001B3 RID: 435
	[Serializable]
	public struct SNetSocket_t : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t>
	{
		// Token: 0x06000A83 RID: 2691 RVA: 0x00010092 File Offset: 0x0000E292
		public SNetSocket_t(uint value)
		{
			this.m_SNetSocket = value;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0001009B File Offset: 0x0000E29B
		public override string ToString()
		{
			return this.m_SNetSocket.ToString();
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x000100A8 File Offset: 0x0000E2A8
		public override bool Equals(object other)
		{
			return other is SNetSocket_t && this == (SNetSocket_t)other;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x000100C5 File Offset: 0x0000E2C5
		public override int GetHashCode()
		{
			return this.m_SNetSocket.GetHashCode();
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x000100D2 File Offset: 0x0000E2D2
		public static bool operator ==(SNetSocket_t x, SNetSocket_t y)
		{
			return x.m_SNetSocket == y.m_SNetSocket;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x000100E2 File Offset: 0x0000E2E2
		public static bool operator !=(SNetSocket_t x, SNetSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x000100EE File Offset: 0x0000E2EE
		public static explicit operator SNetSocket_t(uint value)
		{
			return new SNetSocket_t(value);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x000100F6 File Offset: 0x0000E2F6
		public static explicit operator uint(SNetSocket_t that)
		{
			return that.m_SNetSocket;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x000100FE File Offset: 0x0000E2FE
		public bool Equals(SNetSocket_t other)
		{
			return this.m_SNetSocket == other.m_SNetSocket;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0001010E File Offset: 0x0000E30E
		public int CompareTo(SNetSocket_t other)
		{
			return this.m_SNetSocket.CompareTo(other.m_SNetSocket);
		}

		// Token: 0x04000AF8 RID: 2808
		public uint m_SNetSocket;
	}
}
