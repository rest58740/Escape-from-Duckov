using System;

namespace System.Security.Policy
{
	// Token: 0x020003FA RID: 1018
	public interface IMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x060029AF RID: 10671
		bool Check(Evidence evidence);

		// Token: 0x060029B0 RID: 10672
		IMembershipCondition Copy();

		// Token: 0x060029B1 RID: 10673
		bool Equals(object obj);

		// Token: 0x060029B2 RID: 10674
		string ToString();
	}
}
