using System;

namespace Steamworks
{
	// Token: 0x020001CF RID: 463
	[Serializable]
	public struct UGCQueryHandle_t : IEquatable<UGCQueryHandle_t>, IComparable<UGCQueryHandle_t>
	{
		// Token: 0x06000B84 RID: 2948 RVA: 0x0001109A File Offset: 0x0000F29A
		public UGCQueryHandle_t(ulong value)
		{
			this.m_UGCQueryHandle = value;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000110A3 File Offset: 0x0000F2A3
		public override string ToString()
		{
			return this.m_UGCQueryHandle.ToString();
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x000110B0 File Offset: 0x0000F2B0
		public override bool Equals(object other)
		{
			return other is UGCQueryHandle_t && this == (UGCQueryHandle_t)other;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x000110CD File Offset: 0x0000F2CD
		public override int GetHashCode()
		{
			return this.m_UGCQueryHandle.GetHashCode();
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x000110DA File Offset: 0x0000F2DA
		public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return x.m_UGCQueryHandle == y.m_UGCQueryHandle;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x000110EA File Offset: 0x0000F2EA
		public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000110F6 File Offset: 0x0000F2F6
		public static explicit operator UGCQueryHandle_t(ulong value)
		{
			return new UGCQueryHandle_t(value);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x000110FE File Offset: 0x0000F2FE
		public static explicit operator ulong(UGCQueryHandle_t that)
		{
			return that.m_UGCQueryHandle;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00011106 File Offset: 0x0000F306
		public bool Equals(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle == other.m_UGCQueryHandle;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00011116 File Offset: 0x0000F316
		public int CompareTo(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);
		}

		// Token: 0x04000B56 RID: 2902
		public static readonly UGCQueryHandle_t Invalid = new UGCQueryHandle_t(ulong.MaxValue);

		// Token: 0x04000B57 RID: 2903
		public ulong m_UGCQueryHandle;
	}
}
