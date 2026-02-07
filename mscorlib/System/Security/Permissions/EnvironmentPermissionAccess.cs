using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000434 RID: 1076
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum EnvironmentPermissionAccess
	{
		// Token: 0x04002009 RID: 8201
		NoAccess = 0,
		// Token: 0x0400200A RID: 8202
		Read = 1,
		// Token: 0x0400200B RID: 8203
		Write = 2,
		// Token: 0x0400200C RID: 8204
		AllAccess = 3
	}
}
