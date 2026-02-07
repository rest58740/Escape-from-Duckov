using System;

namespace Steamworks
{
	// Token: 0x020001B2 RID: 434
	[Serializable]
	public struct SNetListenSocket_t : IEquatable<SNetListenSocket_t>, IComparable<SNetListenSocket_t>
	{
		// Token: 0x06000A79 RID: 2681 RVA: 0x00010003 File Offset: 0x0000E203
		public SNetListenSocket_t(uint value)
		{
			this.m_SNetListenSocket = value;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0001000C File Offset: 0x0000E20C
		public override string ToString()
		{
			return this.m_SNetListenSocket.ToString();
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00010019 File Offset: 0x0000E219
		public override bool Equals(object other)
		{
			return other is SNetListenSocket_t && this == (SNetListenSocket_t)other;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00010036 File Offset: 0x0000E236
		public override int GetHashCode()
		{
			return this.m_SNetListenSocket.GetHashCode();
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00010043 File Offset: 0x0000E243
		public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return x.m_SNetListenSocket == y.m_SNetListenSocket;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00010053 File Offset: 0x0000E253
		public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0001005F File Offset: 0x0000E25F
		public static explicit operator SNetListenSocket_t(uint value)
		{
			return new SNetListenSocket_t(value);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00010067 File Offset: 0x0000E267
		public static explicit operator uint(SNetListenSocket_t that)
		{
			return that.m_SNetListenSocket;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0001006F File Offset: 0x0000E26F
		public bool Equals(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket == other.m_SNetListenSocket;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0001007F File Offset: 0x0000E27F
		public int CompareTo(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
		}

		// Token: 0x04000AF7 RID: 2807
		public uint m_SNetListenSocket;
	}
}
