using System;

namespace Steamworks
{
	// Token: 0x020001C6 RID: 454
	[Serializable]
	public struct ScreenshotHandle : IEquatable<ScreenshotHandle>, IComparable<ScreenshotHandle>
	{
		// Token: 0x06000B29 RID: 2857 RVA: 0x000109F1 File Offset: 0x0000EBF1
		public ScreenshotHandle(uint value)
		{
			this.m_ScreenshotHandle = value;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000109FA File Offset: 0x0000EBFA
		public override string ToString()
		{
			return this.m_ScreenshotHandle.ToString();
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00010A07 File Offset: 0x0000EC07
		public override bool Equals(object other)
		{
			return other is ScreenshotHandle && this == (ScreenshotHandle)other;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00010A24 File Offset: 0x0000EC24
		public override int GetHashCode()
		{
			return this.m_ScreenshotHandle.GetHashCode();
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00010A31 File Offset: 0x0000EC31
		public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y)
		{
			return x.m_ScreenshotHandle == y.m_ScreenshotHandle;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00010A41 File Offset: 0x0000EC41
		public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00010A4D File Offset: 0x0000EC4D
		public static explicit operator ScreenshotHandle(uint value)
		{
			return new ScreenshotHandle(value);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00010A55 File Offset: 0x0000EC55
		public static explicit operator uint(ScreenshotHandle that)
		{
			return that.m_ScreenshotHandle;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00010A5D File Offset: 0x0000EC5D
		public bool Equals(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle == other.m_ScreenshotHandle;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00010A6D File Offset: 0x0000EC6D
		public int CompareTo(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);
		}

		// Token: 0x04000B45 RID: 2885
		public static readonly ScreenshotHandle Invalid = new ScreenshotHandle(0U);

		// Token: 0x04000B46 RID: 2886
		public uint m_ScreenshotHandle;
	}
}
