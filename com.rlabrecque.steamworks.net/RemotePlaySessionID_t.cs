using System;

namespace Steamworks
{
	// Token: 0x020001C1 RID: 449
	[Serializable]
	public struct RemotePlaySessionID_t : IEquatable<RemotePlaySessionID_t>, IComparable<RemotePlaySessionID_t>
	{
		// Token: 0x06000AF3 RID: 2803 RVA: 0x000106EE File Offset: 0x0000E8EE
		public RemotePlaySessionID_t(uint value)
		{
			this.m_RemotePlaySessionID = value;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x000106F7 File Offset: 0x0000E8F7
		public override string ToString()
		{
			return this.m_RemotePlaySessionID.ToString();
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00010704 File Offset: 0x0000E904
		public override bool Equals(object other)
		{
			return other is RemotePlaySessionID_t && this == (RemotePlaySessionID_t)other;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00010721 File Offset: 0x0000E921
		public override int GetHashCode()
		{
			return this.m_RemotePlaySessionID.GetHashCode();
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0001072E File Offset: 0x0000E92E
		public static bool operator ==(RemotePlaySessionID_t x, RemotePlaySessionID_t y)
		{
			return x.m_RemotePlaySessionID == y.m_RemotePlaySessionID;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0001073E File Offset: 0x0000E93E
		public static bool operator !=(RemotePlaySessionID_t x, RemotePlaySessionID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0001074A File Offset: 0x0000E94A
		public static explicit operator RemotePlaySessionID_t(uint value)
		{
			return new RemotePlaySessionID_t(value);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00010752 File Offset: 0x0000E952
		public static explicit operator uint(RemotePlaySessionID_t that)
		{
			return that.m_RemotePlaySessionID;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0001075A File Offset: 0x0000E95A
		public bool Equals(RemotePlaySessionID_t other)
		{
			return this.m_RemotePlaySessionID == other.m_RemotePlaySessionID;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0001076A File Offset: 0x0000E96A
		public int CompareTo(RemotePlaySessionID_t other)
		{
			return this.m_RemotePlaySessionID.CompareTo(other.m_RemotePlaySessionID);
		}

		// Token: 0x04000B3C RID: 2876
		public uint m_RemotePlaySessionID;
	}
}
