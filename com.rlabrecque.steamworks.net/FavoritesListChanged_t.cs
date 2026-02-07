using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000078 RID: 120
	[CallbackIdentity(502)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FavoritesListChanged_t
	{
		// Token: 0x04000134 RID: 308
		public const int k_iCallback = 502;

		// Token: 0x04000135 RID: 309
		public uint m_nIP;

		// Token: 0x04000136 RID: 310
		public uint m_nQueryPort;

		// Token: 0x04000137 RID: 311
		public uint m_nConnPort;

		// Token: 0x04000138 RID: 312
		public uint m_nAppID;

		// Token: 0x04000139 RID: 313
		public uint m_nFlags;

		// Token: 0x0400013A RID: 314
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAdd;

		// Token: 0x0400013B RID: 315
		public AccountID_t m_unAccountId;
	}
}
