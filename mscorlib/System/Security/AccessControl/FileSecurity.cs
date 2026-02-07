using System;
using System.Runtime.InteropServices;

namespace System.Security.AccessControl
{
	// Token: 0x02000527 RID: 1319
	public sealed class FileSecurity : FileSystemSecurity
	{
		// Token: 0x06003436 RID: 13366 RVA: 0x000BEA60 File Offset: 0x000BCC60
		public FileSecurity() : base(false)
		{
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000BEA69 File Offset: 0x000BCC69
		public FileSecurity(string fileName, AccessControlSections includeSections) : base(false, fileName, includeSections)
		{
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000BEA74 File Offset: 0x000BCC74
		internal FileSecurity(SafeHandle handle, AccessControlSections includeSections) : base(false, handle, includeSections)
		{
		}
	}
}
