using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000FF RID: 255
	[CallbackIdentity(739)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FilterTextDictionaryChanged_t
	{
		// Token: 0x0400031B RID: 795
		public const int k_iCallback = 739;

		// Token: 0x0400031C RID: 796
		public int m_eLanguage;
	}
}
