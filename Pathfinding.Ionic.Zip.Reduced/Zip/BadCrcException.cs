using System;
using System.Runtime.InteropServices;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000011 RID: 17
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00009")]
	public class BadCrcException : ZipException
	{
		// Token: 0x06000050 RID: 80 RVA: 0x000025CC File Offset: 0x000007CC
		public BadCrcException()
		{
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000025D4 File Offset: 0x000007D4
		public BadCrcException(string message) : base(message)
		{
		}
	}
}
