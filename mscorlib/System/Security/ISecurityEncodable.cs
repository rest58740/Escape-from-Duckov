using System;

namespace System.Security
{
	// Token: 0x020003C6 RID: 966
	public interface ISecurityEncodable
	{
		// Token: 0x06002851 RID: 10321
		void FromXml(SecurityElement e);

		// Token: 0x06002852 RID: 10322
		SecurityElement ToXml();
	}
}
