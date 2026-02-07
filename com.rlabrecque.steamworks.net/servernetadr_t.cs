using System;

namespace Steamworks
{
	// Token: 0x0200019A RID: 410
	[Serializable]
	public struct servernetadr_t
	{
		// Token: 0x0600097A RID: 2426 RVA: 0x0000EEAF File Offset: 0x0000D0AF
		public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort)
		{
			this.m_unIP = ip;
			this.m_usQueryPort = usQueryPort;
			this.m_usConnectionPort = usConnectionPort;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0000EEC6 File Offset: 0x0000D0C6
		public ushort GetQueryPort()
		{
			return this.m_usQueryPort;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0000EECE File Offset: 0x0000D0CE
		public void SetQueryPort(ushort usPort)
		{
			this.m_usQueryPort = usPort;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0000EED7 File Offset: 0x0000D0D7
		public ushort GetConnectionPort()
		{
			return this.m_usConnectionPort;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0000EEDF File Offset: 0x0000D0DF
		public void SetConnectionPort(ushort usPort)
		{
			this.m_usConnectionPort = usPort;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0000EEE8 File Offset: 0x0000D0E8
		public uint GetIP()
		{
			return this.m_unIP;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
		public void SetIP(uint unIP)
		{
			this.m_unIP = unIP;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0000EEF9 File Offset: 0x0000D0F9
		public string GetConnectionAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usConnectionPort);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0000EF0C File Offset: 0x0000D10C
		public string GetQueryAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usQueryPort);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0000EF20 File Offset: 0x0000D120
		public static string ToString(uint unIP, ushort usPort)
		{
			return string.Format("{0}.{1}.{2}.{3}:{4}", new object[]
			{
				(ulong)(unIP >> 24) & 255UL,
				(ulong)(unIP >> 16) & 255UL,
				(ulong)(unIP >> 8) & 255UL,
				(ulong)unIP & 255UL,
				usPort
			});
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0000EF92 File Offset: 0x0000D192
		public static bool operator <(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP < y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort < y.m_usQueryPort);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0000EFC2 File Offset: 0x0000D1C2
		public static bool operator >(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP > y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort > y.m_usQueryPort);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0000EFF2 File Offset: 0x0000D1F2
		public override bool Equals(object other)
		{
			return other is servernetadr_t && this == (servernetadr_t)other;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0000F00F File Offset: 0x0000D20F
		public override int GetHashCode()
		{
			return this.m_unIP.GetHashCode() + this.m_usQueryPort.GetHashCode() + this.m_usConnectionPort.GetHashCode();
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0000F034 File Offset: 0x0000D234
		public static bool operator ==(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP == y.m_unIP && x.m_usQueryPort == y.m_usQueryPort && x.m_usConnectionPort == y.m_usConnectionPort;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0000F062 File Offset: 0x0000D262
		public static bool operator !=(servernetadr_t x, servernetadr_t y)
		{
			return !(x == y);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0000F06E File Offset: 0x0000D26E
		public bool Equals(servernetadr_t other)
		{
			return this.m_unIP == other.m_unIP && this.m_usQueryPort == other.m_usQueryPort && this.m_usConnectionPort == other.m_usConnectionPort;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0000F09C File Offset: 0x0000D29C
		public int CompareTo(servernetadr_t other)
		{
			return this.m_unIP.CompareTo(other.m_unIP) + this.m_usQueryPort.CompareTo(other.m_usQueryPort) + this.m_usConnectionPort.CompareTo(other.m_usConnectionPort);
		}

		// Token: 0x04000AC5 RID: 2757
		private ushort m_usConnectionPort;

		// Token: 0x04000AC6 RID: 2758
		private ushort m_usQueryPort;

		// Token: 0x04000AC7 RID: 2759
		private uint m_unIP;
	}
}
