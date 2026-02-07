using System;

namespace Steamworks
{
	// Token: 0x020001B8 RID: 440
	[Serializable]
	public struct HSteamNetConnection : IEquatable<HSteamNetConnection>, IComparable<HSteamNetConnection>
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x000101E8 File Offset: 0x0000E3E8
		public HSteamNetConnection(uint value)
		{
			this.m_HSteamNetConnection = value;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000101F1 File Offset: 0x0000E3F1
		public override string ToString()
		{
			return this.m_HSteamNetConnection.ToString();
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x000101FE File Offset: 0x0000E3FE
		public override bool Equals(object other)
		{
			return other is HSteamNetConnection && this == (HSteamNetConnection)other;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0001021B File Offset: 0x0000E41B
		public override int GetHashCode()
		{
			return this.m_HSteamNetConnection.GetHashCode();
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00010228 File Offset: 0x0000E428
		public static bool operator ==(HSteamNetConnection x, HSteamNetConnection y)
		{
			return x.m_HSteamNetConnection == y.m_HSteamNetConnection;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00010238 File Offset: 0x0000E438
		public static bool operator !=(HSteamNetConnection x, HSteamNetConnection y)
		{
			return !(x == y);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00010244 File Offset: 0x0000E444
		public static explicit operator HSteamNetConnection(uint value)
		{
			return new HSteamNetConnection(value);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0001024C File Offset: 0x0000E44C
		public static explicit operator uint(HSteamNetConnection that)
		{
			return that.m_HSteamNetConnection;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00010254 File Offset: 0x0000E454
		public bool Equals(HSteamNetConnection other)
		{
			return this.m_HSteamNetConnection == other.m_HSteamNetConnection;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00010264 File Offset: 0x0000E464
		public int CompareTo(HSteamNetConnection other)
		{
			return this.m_HSteamNetConnection.CompareTo(other.m_HSteamNetConnection);
		}

		// Token: 0x04000AFB RID: 2811
		public static readonly HSteamNetConnection Invalid = new HSteamNetConnection(0U);

		// Token: 0x04000AFC RID: 2812
		public uint m_HSteamNetConnection;
	}
}
