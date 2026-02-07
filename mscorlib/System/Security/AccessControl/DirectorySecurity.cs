using System;

namespace System.Security.AccessControl
{
	// Token: 0x02000521 RID: 1313
	public sealed class DirectorySecurity : FileSystemSecurity
	{
		// Token: 0x06003409 RID: 13321 RVA: 0x000BE75C File Offset: 0x000BC95C
		public DirectorySecurity() : base(true)
		{
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x000BE765 File Offset: 0x000BC965
		public DirectorySecurity(string name, AccessControlSections includeSections) : base(true, name, includeSections)
		{
		}
	}
}
