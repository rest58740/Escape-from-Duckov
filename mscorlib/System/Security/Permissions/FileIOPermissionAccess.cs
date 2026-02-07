using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000439 RID: 1081
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum FileIOPermissionAccess
	{
		// Token: 0x0400201E RID: 8222
		NoAccess = 0,
		// Token: 0x0400201F RID: 8223
		Read = 1,
		// Token: 0x04002020 RID: 8224
		Write = 2,
		// Token: 0x04002021 RID: 8225
		Append = 4,
		// Token: 0x04002022 RID: 8226
		PathDiscovery = 8,
		// Token: 0x04002023 RID: 8227
		AllAccess = 15
	}
}
