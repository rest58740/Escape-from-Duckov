using System;
using System.Runtime.InteropServices;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x0200000F RID: 15
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000B")]
	public class BadPasswordException : ZipException
	{
		// Token: 0x0600004A RID: 74 RVA: 0x0000258C File Offset: 0x0000078C
		public BadPasswordException()
		{
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002594 File Offset: 0x00000794
		public BadPasswordException(string message) : base(message)
		{
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000025A0 File Offset: 0x000007A0
		public BadPasswordException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
