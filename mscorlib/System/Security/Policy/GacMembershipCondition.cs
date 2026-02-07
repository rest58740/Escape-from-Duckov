using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000411 RID: 1041
	[ComVisible(true)]
	[Serializable]
	public sealed class GacMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable, IConstantMembershipCondition
	{
		// Token: 0x06002A93 RID: 10899 RVA: 0x0009A0EC File Offset: 0x000982EC
		public bool Check(Evidence evidence)
		{
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				if (hostEnumerator.Current is GacInstalled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x0009A11F File Offset: 0x0009831F
		public IMembershipCondition Copy()
		{
			return new GacMembershipCondition();
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x0009A126 File Offset: 0x00098326
		public override bool Equals(object o)
		{
			return o != null && o is GacMembershipCondition;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x0009A136 File Offset: 0x00098336
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x0009A140 File Offset: 0x00098340
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x0009A15A File Offset: 0x0009835A
		public override string ToString()
		{
			return "GAC";
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x0009A161 File Offset: 0x00098361
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x0009A16A File Offset: 0x0009836A
		public SecurityElement ToXml(PolicyLevel level)
		{
			return MembershipConditionHelper.Element(typeof(GacMembershipCondition), this.version);
		}

		// Token: 0x04001F97 RID: 8087
		private readonly int version = 1;
	}
}
