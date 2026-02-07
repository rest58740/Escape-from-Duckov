using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200044A RID: 1098
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum KeyContainerPermissionFlags
	{
		// Token: 0x0400205C RID: 8284
		NoFlags = 0,
		// Token: 0x0400205D RID: 8285
		Create = 1,
		// Token: 0x0400205E RID: 8286
		Open = 2,
		// Token: 0x0400205F RID: 8287
		Delete = 4,
		// Token: 0x04002060 RID: 8288
		Import = 16,
		// Token: 0x04002061 RID: 8289
		Export = 32,
		// Token: 0x04002062 RID: 8290
		Sign = 256,
		// Token: 0x04002063 RID: 8291
		Decrypt = 512,
		// Token: 0x04002064 RID: 8292
		ViewAcl = 4096,
		// Token: 0x04002065 RID: 8293
		ChangeAcl = 8192,
		// Token: 0x04002066 RID: 8294
		AllFlags = 13111
	}
}
