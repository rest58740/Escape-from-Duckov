using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004E4 RID: 1252
	[ComVisible(false)]
	public abstract class IdentityReference
	{
		// Token: 0x060031F2 RID: 12786 RVA: 0x0000259F File Offset: 0x0000079F
		internal IdentityReference()
		{
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060031F3 RID: 12787
		public abstract string Value { get; }

		// Token: 0x060031F4 RID: 12788
		public abstract override bool Equals(object o);

		// Token: 0x060031F5 RID: 12789
		public abstract override int GetHashCode();

		// Token: 0x060031F6 RID: 12790
		public abstract bool IsValidTargetType(Type targetType);

		// Token: 0x060031F7 RID: 12791
		public abstract override string ToString();

		// Token: 0x060031F8 RID: 12792
		public abstract IdentityReference Translate(Type targetType);

		// Token: 0x060031F9 RID: 12793 RVA: 0x000B7BBC File Offset: 0x000B5DBC
		public static bool operator ==(IdentityReference left, IdentityReference right)
		{
			if (left == null)
			{
				return right == null;
			}
			return right != null && left.Value == right.Value;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x000B7BDC File Offset: 0x000B5DDC
		public static bool operator !=(IdentityReference left, IdentityReference right)
		{
			if (left == null)
			{
				return right != null;
			}
			return right == null || left.Value != right.Value;
		}
	}
}
