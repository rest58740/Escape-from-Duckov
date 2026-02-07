using System;

namespace System.Security
{
	// Token: 0x020003C9 RID: 969
	public interface IStackWalk
	{
		// Token: 0x06002856 RID: 10326
		void Assert();

		// Token: 0x06002857 RID: 10327
		void Demand();

		// Token: 0x06002858 RID: 10328
		void Deny();

		// Token: 0x06002859 RID: 10329
		void PermitOnly();
	}
}
