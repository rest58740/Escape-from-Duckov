using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x020003FE RID: 1022
	[ComVisible(true)]
	[Serializable]
	public sealed class AllMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable, IConstantMembershipCondition
	{
		// Token: 0x060029C7 RID: 10695 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool Check(Evidence evidence)
		{
			return true;
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x00097E67 File Offset: 0x00096067
		public IMembershipCondition Copy()
		{
			return new AllMembershipCondition();
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x00097E6E File Offset: 0x0009606E
		public override bool Equals(object o)
		{
			return o is AllMembershipCondition;
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x00097E79 File Offset: 0x00096079
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x00097E83 File Offset: 0x00096083
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x00097E9D File Offset: 0x0009609D
		public override int GetHashCode()
		{
			return typeof(AllMembershipCondition).GetHashCode();
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x00097EAE File Offset: 0x000960AE
		public override string ToString()
		{
			return "All code";
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x00097EB5 File Offset: 0x000960B5
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x00097EBE File Offset: 0x000960BE
		public SecurityElement ToXml(PolicyLevel level)
		{
			return MembershipConditionHelper.Element(typeof(AllMembershipCondition), this.version);
		}

		// Token: 0x04001F52 RID: 8018
		private readonly int version = 1;
	}
}
