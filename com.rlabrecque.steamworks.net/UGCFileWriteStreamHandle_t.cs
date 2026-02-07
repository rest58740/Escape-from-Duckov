using System;

namespace Steamworks
{
	// Token: 0x020001C4 RID: 452
	[Serializable]
	public struct UGCFileWriteStreamHandle_t : IEquatable<UGCFileWriteStreamHandle_t>, IComparable<UGCFileWriteStreamHandle_t>
	{
		// Token: 0x06000B13 RID: 2835 RVA: 0x000108B7 File Offset: 0x0000EAB7
		public UGCFileWriteStreamHandle_t(ulong value)
		{
			this.m_UGCFileWriteStreamHandle = value;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x000108C0 File Offset: 0x0000EAC0
		public override string ToString()
		{
			return this.m_UGCFileWriteStreamHandle.ToString();
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x000108CD File Offset: 0x0000EACD
		public override bool Equals(object other)
		{
			return other is UGCFileWriteStreamHandle_t && this == (UGCFileWriteStreamHandle_t)other;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x000108EA File Offset: 0x0000EAEA
		public override int GetHashCode()
		{
			return this.m_UGCFileWriteStreamHandle.GetHashCode();
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x000108F7 File Offset: 0x0000EAF7
		public static bool operator ==(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return x.m_UGCFileWriteStreamHandle == y.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00010907 File Offset: 0x0000EB07
		public static bool operator !=(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00010913 File Offset: 0x0000EB13
		public static explicit operator UGCFileWriteStreamHandle_t(ulong value)
		{
			return new UGCFileWriteStreamHandle_t(value);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0001091B File Offset: 0x0000EB1B
		public static explicit operator ulong(UGCFileWriteStreamHandle_t that)
		{
			return that.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00010923 File Offset: 0x0000EB23
		public bool Equals(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle == other.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00010933 File Offset: 0x0000EB33
		public int CompareTo(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);
		}

		// Token: 0x04000B41 RID: 2881
		public static readonly UGCFileWriteStreamHandle_t Invalid = new UGCFileWriteStreamHandle_t(ulong.MaxValue);

		// Token: 0x04000B42 RID: 2882
		public ulong m_UGCFileWriteStreamHandle;
	}
}
