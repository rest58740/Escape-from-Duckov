using System;

namespace Steamworks
{
	// Token: 0x020001B7 RID: 439
	[Serializable]
	public struct HSteamListenSocket : IEquatable<HSteamListenSocket>, IComparable<HSteamListenSocket>
	{
		// Token: 0x06000A95 RID: 2709 RVA: 0x0001014C File Offset: 0x0000E34C
		public HSteamListenSocket(uint value)
		{
			this.m_HSteamListenSocket = value;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00010155 File Offset: 0x0000E355
		public override string ToString()
		{
			return this.m_HSteamListenSocket.ToString();
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00010162 File Offset: 0x0000E362
		public override bool Equals(object other)
		{
			return other is HSteamListenSocket && this == (HSteamListenSocket)other;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0001017F File Offset: 0x0000E37F
		public override int GetHashCode()
		{
			return this.m_HSteamListenSocket.GetHashCode();
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0001018C File Offset: 0x0000E38C
		public static bool operator ==(HSteamListenSocket x, HSteamListenSocket y)
		{
			return x.m_HSteamListenSocket == y.m_HSteamListenSocket;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0001019C File Offset: 0x0000E39C
		public static bool operator !=(HSteamListenSocket x, HSteamListenSocket y)
		{
			return !(x == y);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000101A8 File Offset: 0x0000E3A8
		public static explicit operator HSteamListenSocket(uint value)
		{
			return new HSteamListenSocket(value);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x000101B0 File Offset: 0x0000E3B0
		public static explicit operator uint(HSteamListenSocket that)
		{
			return that.m_HSteamListenSocket;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000101B8 File Offset: 0x0000E3B8
		public bool Equals(HSteamListenSocket other)
		{
			return this.m_HSteamListenSocket == other.m_HSteamListenSocket;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000101C8 File Offset: 0x0000E3C8
		public int CompareTo(HSteamListenSocket other)
		{
			return this.m_HSteamListenSocket.CompareTo(other.m_HSteamListenSocket);
		}

		// Token: 0x04000AF9 RID: 2809
		public static readonly HSteamListenSocket Invalid = new HSteamListenSocket(0U);

		// Token: 0x04000AFA RID: 2810
		public uint m_HSteamListenSocket;
	}
}
