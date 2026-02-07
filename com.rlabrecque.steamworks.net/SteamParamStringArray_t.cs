using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017C RID: 380
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamParamStringArray_t
	{
		// Token: 0x04000A1A RID: 2586
		public IntPtr m_ppStrings;

		// Token: 0x04000A1B RID: 2587
		public int m_nNumStrings;
	}
}
