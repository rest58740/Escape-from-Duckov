using System;

namespace Steamworks
{
	// Token: 0x020001A4 RID: 420
	[Serializable]
	public struct HTTPCookieContainerHandle : IEquatable<HTTPCookieContainerHandle>, IComparable<HTTPCookieContainerHandle>
	{
		// Token: 0x060009F7 RID: 2551 RVA: 0x0000F8FB File Offset: 0x0000DAFB
		public HTTPCookieContainerHandle(uint value)
		{
			this.m_HTTPCookieContainerHandle = value;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0000F904 File Offset: 0x0000DB04
		public override string ToString()
		{
			return this.m_HTTPCookieContainerHandle.ToString();
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0000F911 File Offset: 0x0000DB11
		public override bool Equals(object other)
		{
			return other is HTTPCookieContainerHandle && this == (HTTPCookieContainerHandle)other;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0000F92E File Offset: 0x0000DB2E
		public override int GetHashCode()
		{
			return this.m_HTTPCookieContainerHandle.GetHashCode();
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0000F93B File Offset: 0x0000DB3B
		public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return x.m_HTTPCookieContainerHandle == y.m_HTTPCookieContainerHandle;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0000F94B File Offset: 0x0000DB4B
		public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return !(x == y);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0000F957 File Offset: 0x0000DB57
		public static explicit operator HTTPCookieContainerHandle(uint value)
		{
			return new HTTPCookieContainerHandle(value);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0000F95F File Offset: 0x0000DB5F
		public static explicit operator uint(HTTPCookieContainerHandle that)
		{
			return that.m_HTTPCookieContainerHandle;
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0000F967 File Offset: 0x0000DB67
		public bool Equals(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle == other.m_HTTPCookieContainerHandle;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0000F977 File Offset: 0x0000DB77
		public int CompareTo(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
		}

		// Token: 0x04000AE1 RID: 2785
		public static readonly HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0U);

		// Token: 0x04000AE2 RID: 2786
		public uint m_HTTPCookieContainerHandle;
	}
}
