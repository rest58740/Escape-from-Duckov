using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Policy
{
	// Token: 0x020003FD RID: 1021
	[Serializable]
	public sealed class PublisherMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
	{
		// Token: 0x060029BA RID: 10682 RVA: 0x0000259F File Offset: 0x0000079F
		public PublisherMembershipCondition(X509Certificate certificate)
		{
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060029BB RID: 10683 RVA: 0x00097E47 File Offset: 0x00096047
		// (set) Token: 0x060029BC RID: 10684 RVA: 0x00097E4F File Offset: 0x0009604F
		public X509Certificate Certificate { get; set; }

		// Token: 0x060029BD RID: 10685 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool Check(Evidence evidence)
		{
			return false;
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x0000270D File Offset: 0x0000090D
		public IMembershipCondition Copy()
		{
			return this;
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x00097E36 File Offset: 0x00096036
		public override bool Equals(object o)
		{
			return base.Equals(o);
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void FromXml(SecurityElement e)
		{
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000930F4 File Offset: 0x000912F4
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x00097E3F File Offset: 0x0009603F
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x0000AF5E File Offset: 0x0000915E
		public SecurityElement ToXml()
		{
			return null;
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x0000AF5E File Offset: 0x0000915E
		public SecurityElement ToXml(PolicyLevel level)
		{
			return null;
		}
	}
}
