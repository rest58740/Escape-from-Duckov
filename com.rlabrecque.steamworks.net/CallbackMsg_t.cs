using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000180 RID: 384
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CallbackMsg_t
	{
		// Token: 0x04000A3E RID: 2622
		public int m_hSteamUser;

		// Token: 0x04000A3F RID: 2623
		public int m_iCallback;

		// Token: 0x04000A40 RID: 2624
		public IntPtr m_pubParam;

		// Token: 0x04000A41 RID: 2625
		public int m_cubParam;
	}
}
