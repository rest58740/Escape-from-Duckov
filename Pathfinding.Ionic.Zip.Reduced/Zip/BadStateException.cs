using System;
using System.Runtime.InteropServices;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000013 RID: 19
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00007")]
	public class BadStateException : ZipException
	{
		// Token: 0x06000054 RID: 84 RVA: 0x000025F4 File Offset: 0x000007F4
		public BadStateException()
		{
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000025FC File Offset: 0x000007FC
		public BadStateException(string message) : base(message)
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002608 File Offset: 0x00000808
		public BadStateException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
