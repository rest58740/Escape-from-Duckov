using System;

namespace Steamworks
{
	// Token: 0x020001A5 RID: 421
	[Serializable]
	public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle>
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x0000F997 File Offset: 0x0000DB97
		public HTTPRequestHandle(uint value)
		{
			this.m_HTTPRequestHandle = value;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0000F9A0 File Offset: 0x0000DBA0
		public override string ToString()
		{
			return this.m_HTTPRequestHandle.ToString();
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0000F9AD File Offset: 0x0000DBAD
		public override bool Equals(object other)
		{
			return other is HTTPRequestHandle && this == (HTTPRequestHandle)other;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0000F9CA File Offset: 0x0000DBCA
		public override int GetHashCode()
		{
			return this.m_HTTPRequestHandle.GetHashCode();
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0000F9D7 File Offset: 0x0000DBD7
		public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return x.m_HTTPRequestHandle == y.m_HTTPRequestHandle;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0000F9E7 File Offset: 0x0000DBE7
		public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0000F9F3 File Offset: 0x0000DBF3
		public static explicit operator HTTPRequestHandle(uint value)
		{
			return new HTTPRequestHandle(value);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0000F9FB File Offset: 0x0000DBFB
		public static explicit operator uint(HTTPRequestHandle that)
		{
			return that.m_HTTPRequestHandle;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0000FA03 File Offset: 0x0000DC03
		public bool Equals(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle == other.m_HTTPRequestHandle;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0000FA13 File Offset: 0x0000DC13
		public int CompareTo(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
		}

		// Token: 0x04000AE3 RID: 2787
		public static readonly HTTPRequestHandle Invalid = new HTTPRequestHandle(0U);

		// Token: 0x04000AE4 RID: 2788
		public uint m_HTTPRequestHandle;
	}
}
