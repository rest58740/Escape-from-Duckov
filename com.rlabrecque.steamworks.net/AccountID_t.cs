using System;

namespace Steamworks
{
	// Token: 0x020001C8 RID: 456
	[Serializable]
	public struct AccountID_t : IEquatable<AccountID_t>, IComparable<AccountID_t>
	{
		// Token: 0x06000B3E RID: 2878 RVA: 0x00010B1C File Offset: 0x0000ED1C
		public AccountID_t(uint value)
		{
			this.m_AccountID = value;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00010B25 File Offset: 0x0000ED25
		public override string ToString()
		{
			return this.m_AccountID.ToString();
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00010B32 File Offset: 0x0000ED32
		public override bool Equals(object other)
		{
			return other is AccountID_t && this == (AccountID_t)other;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00010B4F File Offset: 0x0000ED4F
		public override int GetHashCode()
		{
			return this.m_AccountID.GetHashCode();
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00010B5C File Offset: 0x0000ED5C
		public static bool operator ==(AccountID_t x, AccountID_t y)
		{
			return x.m_AccountID == y.m_AccountID;
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00010B6C File Offset: 0x0000ED6C
		public static bool operator !=(AccountID_t x, AccountID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00010B78 File Offset: 0x0000ED78
		public static explicit operator AccountID_t(uint value)
		{
			return new AccountID_t(value);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00010B80 File Offset: 0x0000ED80
		public static explicit operator uint(AccountID_t that)
		{
			return that.m_AccountID;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00010B88 File Offset: 0x0000ED88
		public bool Equals(AccountID_t other)
		{
			return this.m_AccountID == other.m_AccountID;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00010B98 File Offset: 0x0000ED98
		public int CompareTo(AccountID_t other)
		{
			return this.m_AccountID.CompareTo(other.m_AccountID);
		}

		// Token: 0x04000B48 RID: 2888
		public static readonly AccountID_t Invalid = new AccountID_t(0U);

		// Token: 0x04000B49 RID: 2889
		public uint m_AccountID;
	}
}
