using System;
using System.Runtime.InteropServices;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x0200005F RID: 95
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x0001D82C File Offset: 0x0001BA2C
		public ZlibException()
		{
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001D834 File Offset: 0x0001BA34
		public ZlibException(string s) : base(s)
		{
		}
	}
}
