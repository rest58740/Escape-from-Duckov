using System;
using System.Globalization;
using System.Security.Principal;
using System.Text;
using Unity;

namespace System.Security.AccessControl
{
	// Token: 0x02000530 RID: 1328
	public abstract class KnownAce : GenericAce
	{
		// Token: 0x0600349A RID: 13466 RVA: 0x000BF89E File Offset: 0x000BDA9E
		internal KnownAce(AceType type, AceFlags flags) : base(type, flags)
		{
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000BF8A8 File Offset: 0x000BDAA8
		internal KnownAce(byte[] binaryForm, int offset) : base(binaryForm, offset)
		{
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x0600349C RID: 13468 RVA: 0x000BF8B2 File Offset: 0x000BDAB2
		// (set) Token: 0x0600349D RID: 13469 RVA: 0x000BF8BA File Offset: 0x000BDABA
		public int AccessMask
		{
			get
			{
				return this.access_mask;
			}
			set
			{
				this.access_mask = value;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600349E RID: 13470 RVA: 0x000BF8C3 File Offset: 0x000BDAC3
		// (set) Token: 0x0600349F RID: 13471 RVA: 0x000BF8CB File Offset: 0x000BDACB
		public SecurityIdentifier SecurityIdentifier
		{
			get
			{
				return this.identifier;
			}
			set
			{
				this.identifier = value;
			}
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000BF8D4 File Offset: 0x000BDAD4
		internal static string GetSddlAccessRights(int accessMask)
		{
			string sddlAliasRights = KnownAce.GetSddlAliasRights(accessMask);
			if (!string.IsNullOrEmpty(sddlAliasRights))
			{
				return sddlAliasRights;
			}
			return string.Format(CultureInfo.InvariantCulture, "0x{0:x}", accessMask);
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000BF908 File Offset: 0x000BDB08
		private static string GetSddlAliasRights(int accessMask)
		{
			SddlAccessRight[] array = SddlAccessRight.Decompose(accessMask);
			if (array == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SddlAccessRight sddlAccessRight in array)
			{
				stringBuilder.Append(sddlAccessRight.Name);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000173AD File Offset: 0x000155AD
		internal KnownAce()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x040024AE RID: 9390
		private int access_mask;

		// Token: 0x040024AF RID: 9391
		private SecurityIdentifier identifier;
	}
}
