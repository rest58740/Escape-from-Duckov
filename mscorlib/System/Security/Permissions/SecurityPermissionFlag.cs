using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000459 RID: 1113
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum SecurityPermissionFlag
	{
		// Token: 0x04002099 RID: 8345
		NoFlags = 0,
		// Token: 0x0400209A RID: 8346
		Assertion = 1,
		// Token: 0x0400209B RID: 8347
		UnmanagedCode = 2,
		// Token: 0x0400209C RID: 8348
		SkipVerification = 4,
		// Token: 0x0400209D RID: 8349
		Execution = 8,
		// Token: 0x0400209E RID: 8350
		ControlThread = 16,
		// Token: 0x0400209F RID: 8351
		ControlEvidence = 32,
		// Token: 0x040020A0 RID: 8352
		ControlPolicy = 64,
		// Token: 0x040020A1 RID: 8353
		SerializationFormatter = 128,
		// Token: 0x040020A2 RID: 8354
		ControlDomainPolicy = 256,
		// Token: 0x040020A3 RID: 8355
		ControlPrincipal = 512,
		// Token: 0x040020A4 RID: 8356
		ControlAppDomain = 1024,
		// Token: 0x040020A5 RID: 8357
		RemotingConfiguration = 2048,
		// Token: 0x040020A6 RID: 8358
		Infrastructure = 4096,
		// Token: 0x040020A7 RID: 8359
		BindingRedirects = 8192,
		// Token: 0x040020A8 RID: 8360
		AllFlags = 16383
	}
}
