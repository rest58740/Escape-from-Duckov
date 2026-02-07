using System;
using System.Runtime.InteropServices;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000014 RID: 20
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00006")]
	public class ZipException : Exception
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00002614 File Offset: 0x00000814
		public ZipException()
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000261C File Offset: 0x0000081C
		public ZipException(string message) : base(message)
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002628 File Offset: 0x00000828
		public ZipException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
