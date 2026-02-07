using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000082 RID: 130
	[CallbackIdentity(516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListAccountsUpdated_t
	{
		// Token: 0x04000161 RID: 353
		public const int k_iCallback = 516;

		// Token: 0x04000162 RID: 354
		public EResult m_eResult;
	}
}
