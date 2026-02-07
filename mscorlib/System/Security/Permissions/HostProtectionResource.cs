using System;

namespace System.Security.Permissions
{
	// Token: 0x0200042A RID: 1066
	[Flags]
	public enum HostProtectionResource
	{
		// Token: 0x04001FD3 RID: 8147
		All = 511,
		// Token: 0x04001FD4 RID: 8148
		ExternalProcessMgmt = 4,
		// Token: 0x04001FD5 RID: 8149
		ExternalThreading = 16,
		// Token: 0x04001FD6 RID: 8150
		MayLeakOnAbort = 256,
		// Token: 0x04001FD7 RID: 8151
		None = 0,
		// Token: 0x04001FD8 RID: 8152
		SecurityInfrastructure = 64,
		// Token: 0x04001FD9 RID: 8153
		SelfAffectingProcessMgmt = 8,
		// Token: 0x04001FDA RID: 8154
		SelfAffectingThreading = 32,
		// Token: 0x04001FDB RID: 8155
		SharedState = 2,
		// Token: 0x04001FDC RID: 8156
		Synchronization = 1,
		// Token: 0x04001FDD RID: 8157
		UI = 128
	}
}
