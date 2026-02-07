using System;

namespace Steamworks
{
	// Token: 0x020001D0 RID: 464
	[Serializable]
	public struct UGCUpdateHandle_t : IEquatable<UGCUpdateHandle_t>, IComparable<UGCUpdateHandle_t>
	{
		// Token: 0x06000B8F RID: 2959 RVA: 0x00011137 File Offset: 0x0000F337
		public UGCUpdateHandle_t(ulong value)
		{
			this.m_UGCUpdateHandle = value;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00011140 File Offset: 0x0000F340
		public override string ToString()
		{
			return this.m_UGCUpdateHandle.ToString();
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0001114D File Offset: 0x0000F34D
		public override bool Equals(object other)
		{
			return other is UGCUpdateHandle_t && this == (UGCUpdateHandle_t)other;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0001116A File Offset: 0x0000F36A
		public override int GetHashCode()
		{
			return this.m_UGCUpdateHandle.GetHashCode();
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00011177 File Offset: 0x0000F377
		public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return x.m_UGCUpdateHandle == y.m_UGCUpdateHandle;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00011187 File Offset: 0x0000F387
		public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00011193 File Offset: 0x0000F393
		public static explicit operator UGCUpdateHandle_t(ulong value)
		{
			return new UGCUpdateHandle_t(value);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0001119B File Offset: 0x0000F39B
		public static explicit operator ulong(UGCUpdateHandle_t that)
		{
			return that.m_UGCUpdateHandle;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000111A3 File Offset: 0x0000F3A3
		public bool Equals(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle == other.m_UGCUpdateHandle;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x000111B3 File Offset: 0x0000F3B3
		public int CompareTo(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);
		}

		// Token: 0x04000B58 RID: 2904
		public static readonly UGCUpdateHandle_t Invalid = new UGCUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000B59 RID: 2905
		public ulong m_UGCUpdateHandle;
	}
}
