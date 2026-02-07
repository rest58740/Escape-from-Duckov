using System;
using System.Security.Policy;

namespace System.Security
{
	// Token: 0x020003C8 RID: 968
	public interface ISecurityPolicyEncodable
	{
		// Token: 0x06002854 RID: 10324
		void FromXml(SecurityElement e, PolicyLevel level);

		// Token: 0x06002855 RID: 10325
		SecurityElement ToXml(PolicyLevel level);
	}
}
