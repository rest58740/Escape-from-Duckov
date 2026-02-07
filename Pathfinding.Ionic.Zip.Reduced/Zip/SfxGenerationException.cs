using System;
using System.Runtime.InteropServices;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000012 RID: 18
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00008")]
	public class SfxGenerationException : ZipException
	{
		// Token: 0x06000052 RID: 82 RVA: 0x000025E0 File Offset: 0x000007E0
		public SfxGenerationException()
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000025E8 File Offset: 0x000007E8
		public SfxGenerationException(string message) : base(message)
		{
		}
	}
}
