using System;

namespace Steamworks
{
	// Token: 0x020001C5 RID: 453
	[Serializable]
	public struct UGCHandle_t : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
	{
		// Token: 0x06000B1E RID: 2846 RVA: 0x00010954 File Offset: 0x0000EB54
		public UGCHandle_t(ulong value)
		{
			this.m_UGCHandle = value;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0001095D File Offset: 0x0000EB5D
		public override string ToString()
		{
			return this.m_UGCHandle.ToString();
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0001096A File Offset: 0x0000EB6A
		public override bool Equals(object other)
		{
			return other is UGCHandle_t && this == (UGCHandle_t)other;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00010987 File Offset: 0x0000EB87
		public override int GetHashCode()
		{
			return this.m_UGCHandle.GetHashCode();
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00010994 File Offset: 0x0000EB94
		public static bool operator ==(UGCHandle_t x, UGCHandle_t y)
		{
			return x.m_UGCHandle == y.m_UGCHandle;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x000109A4 File Offset: 0x0000EBA4
		public static bool operator !=(UGCHandle_t x, UGCHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x000109B0 File Offset: 0x0000EBB0
		public static explicit operator UGCHandle_t(ulong value)
		{
			return new UGCHandle_t(value);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x000109B8 File Offset: 0x0000EBB8
		public static explicit operator ulong(UGCHandle_t that)
		{
			return that.m_UGCHandle;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x000109C0 File Offset: 0x0000EBC0
		public bool Equals(UGCHandle_t other)
		{
			return this.m_UGCHandle == other.m_UGCHandle;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x000109D0 File Offset: 0x0000EBD0
		public int CompareTo(UGCHandle_t other)
		{
			return this.m_UGCHandle.CompareTo(other.m_UGCHandle);
		}

		// Token: 0x04000B43 RID: 2883
		public static readonly UGCHandle_t Invalid = new UGCHandle_t(ulong.MaxValue);

		// Token: 0x04000B44 RID: 2884
		public ulong m_UGCHandle;
	}
}
