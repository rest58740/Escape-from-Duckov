using System;

namespace Steamworks
{
	// Token: 0x020001C2 RID: 450
	[Serializable]
	public struct PublishedFileId_t : IEquatable<PublishedFileId_t>, IComparable<PublishedFileId_t>
	{
		// Token: 0x06000AFD RID: 2813 RVA: 0x0001077D File Offset: 0x0000E97D
		public PublishedFileId_t(ulong value)
		{
			this.m_PublishedFileId = value;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00010786 File Offset: 0x0000E986
		public override string ToString()
		{
			return this.m_PublishedFileId.ToString();
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00010793 File Offset: 0x0000E993
		public override bool Equals(object other)
		{
			return other is PublishedFileId_t && this == (PublishedFileId_t)other;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000107B0 File Offset: 0x0000E9B0
		public override int GetHashCode()
		{
			return this.m_PublishedFileId.GetHashCode();
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x000107BD File Offset: 0x0000E9BD
		public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y)
		{
			return x.m_PublishedFileId == y.m_PublishedFileId;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000107CD File Offset: 0x0000E9CD
		public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x000107D9 File Offset: 0x0000E9D9
		public static explicit operator PublishedFileId_t(ulong value)
		{
			return new PublishedFileId_t(value);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x000107E1 File Offset: 0x0000E9E1
		public static explicit operator ulong(PublishedFileId_t that)
		{
			return that.m_PublishedFileId;
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x000107E9 File Offset: 0x0000E9E9
		public bool Equals(PublishedFileId_t other)
		{
			return this.m_PublishedFileId == other.m_PublishedFileId;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x000107F9 File Offset: 0x0000E9F9
		public int CompareTo(PublishedFileId_t other)
		{
			return this.m_PublishedFileId.CompareTo(other.m_PublishedFileId);
		}

		// Token: 0x04000B3D RID: 2877
		public static readonly PublishedFileId_t Invalid = new PublishedFileId_t(0UL);

		// Token: 0x04000B3E RID: 2878
		public ulong m_PublishedFileId;
	}
}
