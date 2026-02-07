using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004E6 RID: 1254
	[ComVisible(false)]
	public sealed class NTAccount : IdentityReference
	{
		// Token: 0x0600320A RID: 12810 RVA: 0x000B7D50 File Offset: 0x000B5F50
		public NTAccount(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Locale.GetText("Empty"), "name");
			}
			this._value = name;
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x000B7D8C File Offset: 0x000B5F8C
		public NTAccount(string domainName, string accountName)
		{
			if (accountName == null)
			{
				throw new ArgumentNullException("accountName");
			}
			if (accountName.Length == 0)
			{
				throw new ArgumentException(Locale.GetText("Empty"), "accountName");
			}
			if (domainName == null)
			{
				this._value = accountName;
				return;
			}
			this._value = domainName + "\\" + accountName;
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x000B7DE7 File Offset: 0x000B5FE7
		public override string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x000B7DF0 File Offset: 0x000B5FF0
		public override bool Equals(object o)
		{
			NTAccount ntaccount = o as NTAccount;
			return !(ntaccount == null) && ntaccount.Value == this.Value;
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x000B7E20 File Offset: 0x000B6020
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x000B7E2D File Offset: 0x000B602D
		public override bool IsValidTargetType(Type targetType)
		{
			return targetType == typeof(NTAccount) || targetType == typeof(SecurityIdentifier);
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x000B7E58 File Offset: 0x000B6058
		public override string ToString()
		{
			return this.Value;
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x000B7E60 File Offset: 0x000B6060
		public override IdentityReference Translate(Type targetType)
		{
			if (targetType == typeof(NTAccount))
			{
				return this;
			}
			if (!(targetType == typeof(SecurityIdentifier)))
			{
				throw new ArgumentException("Unknown type", "targetType");
			}
			WellKnownAccount wellKnownAccount = WellKnownAccount.LookupByName(this.Value);
			if (wellKnownAccount == null || wellKnownAccount.Sid == null)
			{
				throw new IdentityNotMappedException("Cannot map account name: " + this.Value);
			}
			return new SecurityIdentifier(wellKnownAccount.Sid);
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x000B7BBC File Offset: 0x000B5DBC
		public static bool operator ==(NTAccount left, NTAccount right)
		{
			if (left == null)
			{
				return right == null;
			}
			return right != null && left.Value == right.Value;
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x000B7BDC File Offset: 0x000B5DDC
		public static bool operator !=(NTAccount left, NTAccount right)
		{
			if (left == null)
			{
				return right != null;
			}
			return right == null || left.Value != right.Value;
		}

		// Token: 0x040022C1 RID: 8897
		private string _value;
	}
}
