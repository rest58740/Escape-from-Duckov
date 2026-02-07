using System;
using System.Runtime.InteropServices;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000010 RID: 16
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000A")]
	public class BadReadException : ZipException
	{
		// Token: 0x0600004D RID: 77 RVA: 0x000025AC File Offset: 0x000007AC
		public BadReadException()
		{
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000025B4 File Offset: 0x000007B4
		public BadReadException(string message) : base(message)
		{
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000025C0 File Offset: 0x000007C0
		public BadReadException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
