using System;

namespace System.Security.Permissions
{
	// Token: 0x0200042C RID: 1068
	public enum IsolatedStorageContainment
	{
		// Token: 0x04001FDF RID: 8159
		None,
		// Token: 0x04001FE0 RID: 8160
		DomainIsolationByUser = 16,
		// Token: 0x04001FE1 RID: 8161
		ApplicationIsolationByUser = 21,
		// Token: 0x04001FE2 RID: 8162
		AssemblyIsolationByUser = 32,
		// Token: 0x04001FE3 RID: 8163
		DomainIsolationByMachine = 48,
		// Token: 0x04001FE4 RID: 8164
		AssemblyIsolationByMachine = 64,
		// Token: 0x04001FE5 RID: 8165
		ApplicationIsolationByMachine = 69,
		// Token: 0x04001FE6 RID: 8166
		DomainIsolationByRoamingUser = 80,
		// Token: 0x04001FE7 RID: 8167
		AssemblyIsolationByRoamingUser = 96,
		// Token: 0x04001FE8 RID: 8168
		ApplicationIsolationByRoamingUser = 101,
		// Token: 0x04001FE9 RID: 8169
		AdministerIsolatedStorageByUser = 112,
		// Token: 0x04001FEA RID: 8170
		UnrestrictedIsolatedStorage = 240
	}
}
