using System;

namespace Steamworks
{
	// Token: 0x020001C3 RID: 451
	[Serializable]
	public struct PublishedFileUpdateHandle_t : IEquatable<PublishedFileUpdateHandle_t>, IComparable<PublishedFileUpdateHandle_t>
	{
		// Token: 0x06000B08 RID: 2824 RVA: 0x0001081A File Offset: 0x0000EA1A
		public PublishedFileUpdateHandle_t(ulong value)
		{
			this.m_PublishedFileUpdateHandle = value;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00010823 File Offset: 0x0000EA23
		public override string ToString()
		{
			return this.m_PublishedFileUpdateHandle.ToString();
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00010830 File Offset: 0x0000EA30
		public override bool Equals(object other)
		{
			return other is PublishedFileUpdateHandle_t && this == (PublishedFileUpdateHandle_t)other;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0001084D File Offset: 0x0000EA4D
		public override int GetHashCode()
		{
			return this.m_PublishedFileUpdateHandle.GetHashCode();
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0001085A File Offset: 0x0000EA5A
		public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return x.m_PublishedFileUpdateHandle == y.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0001086A File Offset: 0x0000EA6A
		public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00010876 File Offset: 0x0000EA76
		public static explicit operator PublishedFileUpdateHandle_t(ulong value)
		{
			return new PublishedFileUpdateHandle_t(value);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0001087E File Offset: 0x0000EA7E
		public static explicit operator ulong(PublishedFileUpdateHandle_t that)
		{
			return that.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00010886 File Offset: 0x0000EA86
		public bool Equals(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle == other.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00010896 File Offset: 0x0000EA96
		public int CompareTo(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
		}

		// Token: 0x04000B3F RID: 2879
		public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000B40 RID: 2880
		public ulong m_PublishedFileUpdateHandle;
	}
}
