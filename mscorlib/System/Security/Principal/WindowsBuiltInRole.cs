using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004EB RID: 1259
	[ComVisible(true)]
	[Serializable]
	public enum WindowsBuiltInRole
	{
		// Token: 0x04002333 RID: 9011
		Administrator = 544,
		// Token: 0x04002334 RID: 9012
		User,
		// Token: 0x04002335 RID: 9013
		Guest,
		// Token: 0x04002336 RID: 9014
		PowerUser,
		// Token: 0x04002337 RID: 9015
		AccountOperator,
		// Token: 0x04002338 RID: 9016
		SystemOperator,
		// Token: 0x04002339 RID: 9017
		PrintOperator,
		// Token: 0x0400233A RID: 9018
		BackupOperator,
		// Token: 0x0400233B RID: 9019
		Replicator
	}
}
