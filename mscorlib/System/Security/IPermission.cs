using System;

namespace System.Security
{
	// Token: 0x020003C5 RID: 965
	public interface IPermission : ISecurityEncodable
	{
		// Token: 0x0600284C RID: 10316
		IPermission Copy();

		// Token: 0x0600284D RID: 10317
		void Demand();

		// Token: 0x0600284E RID: 10318
		IPermission Intersect(IPermission target);

		// Token: 0x0600284F RID: 10319
		bool IsSubsetOf(IPermission target);

		// Token: 0x06002850 RID: 10320
		IPermission Union(IPermission target);
	}
}
