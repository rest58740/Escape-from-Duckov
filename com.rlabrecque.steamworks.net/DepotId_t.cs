using System;

namespace Steamworks
{
	// Token: 0x020001CA RID: 458
	[Serializable]
	public struct DepotId_t : IEquatable<DepotId_t>, IComparable<DepotId_t>
	{
		// Token: 0x06000B54 RID: 2900 RVA: 0x00010C54 File Offset: 0x0000EE54
		public DepotId_t(uint value)
		{
			this.m_DepotId = value;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00010C5D File Offset: 0x0000EE5D
		public override string ToString()
		{
			return this.m_DepotId.ToString();
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00010C6A File Offset: 0x0000EE6A
		public override bool Equals(object other)
		{
			return other is DepotId_t && this == (DepotId_t)other;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00010C87 File Offset: 0x0000EE87
		public override int GetHashCode()
		{
			return this.m_DepotId.GetHashCode();
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00010C94 File Offset: 0x0000EE94
		public static bool operator ==(DepotId_t x, DepotId_t y)
		{
			return x.m_DepotId == y.m_DepotId;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00010CA4 File Offset: 0x0000EEA4
		public static bool operator !=(DepotId_t x, DepotId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00010CB0 File Offset: 0x0000EEB0
		public static explicit operator DepotId_t(uint value)
		{
			return new DepotId_t(value);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00010CB8 File Offset: 0x0000EEB8
		public static explicit operator uint(DepotId_t that)
		{
			return that.m_DepotId;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00010CC0 File Offset: 0x0000EEC0
		public bool Equals(DepotId_t other)
		{
			return this.m_DepotId == other.m_DepotId;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00010CD0 File Offset: 0x0000EED0
		public int CompareTo(DepotId_t other)
		{
			return this.m_DepotId.CompareTo(other.m_DepotId);
		}

		// Token: 0x04000B4C RID: 2892
		public static readonly DepotId_t Invalid = new DepotId_t(0U);

		// Token: 0x04000B4D RID: 2893
		public uint m_DepotId;
	}
}
