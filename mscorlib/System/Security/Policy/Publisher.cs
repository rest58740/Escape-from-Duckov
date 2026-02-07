using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Policy
{
	// Token: 0x020003FC RID: 1020
	[Serializable]
	public sealed class Publisher : EvidenceBase, IIdentityPermissionFactory
	{
		// Token: 0x060029B3 RID: 10675 RVA: 0x00097E2E File Offset: 0x0009602E
		public Publisher(X509Certificate cert)
		{
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x0000AF5E File Offset: 0x0000915E
		public X509Certificate Certificate
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x0000AF5E File Offset: 0x0000915E
		public object Copy()
		{
			return null;
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x0000AF5E File Offset: 0x0000915E
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return null;
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x00097E36 File Offset: 0x00096036
		public override bool Equals(object o)
		{
			return base.Equals(o);
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x000930F4 File Offset: 0x000912F4
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x00097E3F File Offset: 0x0009603F
		public override string ToString()
		{
			return base.ToString();
		}
	}
}
