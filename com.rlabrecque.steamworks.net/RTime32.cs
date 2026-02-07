using System;

namespace Steamworks
{
	// Token: 0x020001CC RID: 460
	[Serializable]
	public struct RTime32 : IEquatable<RTime32>, IComparable<RTime32>
	{
		// Token: 0x06000B6A RID: 2922 RVA: 0x00010D8D File Offset: 0x0000EF8D
		public RTime32(uint value)
		{
			this.m_RTime32 = value;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00010D96 File Offset: 0x0000EF96
		public override string ToString()
		{
			return this.m_RTime32.ToString();
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00010DA3 File Offset: 0x0000EFA3
		public override bool Equals(object other)
		{
			return other is RTime32 && this == (RTime32)other;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00010DC0 File Offset: 0x0000EFC0
		public override int GetHashCode()
		{
			return this.m_RTime32.GetHashCode();
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00010DCD File Offset: 0x0000EFCD
		public static bool operator ==(RTime32 x, RTime32 y)
		{
			return x.m_RTime32 == y.m_RTime32;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00010DDD File Offset: 0x0000EFDD
		public static bool operator !=(RTime32 x, RTime32 y)
		{
			return !(x == y);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00010DE9 File Offset: 0x0000EFE9
		public static explicit operator RTime32(uint value)
		{
			return new RTime32(value);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00010DF1 File Offset: 0x0000EFF1
		public static explicit operator uint(RTime32 that)
		{
			return that.m_RTime32;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00010DF9 File Offset: 0x0000EFF9
		public bool Equals(RTime32 other)
		{
			return this.m_RTime32 == other.m_RTime32;
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00010E09 File Offset: 0x0000F009
		public int CompareTo(RTime32 other)
		{
			return this.m_RTime32.CompareTo(other.m_RTime32);
		}

		// Token: 0x04000B50 RID: 2896
		public uint m_RTime32;
	}
}
