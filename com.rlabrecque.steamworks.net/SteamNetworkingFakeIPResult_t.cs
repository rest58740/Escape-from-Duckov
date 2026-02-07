using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000104 RID: 260
	[CallbackIdentity(1223)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetworkingFakeIPResult_t
	{
		// Token: 0x04000328 RID: 808
		public const int k_iCallback = 1223;

		// Token: 0x04000329 RID: 809
		public EResult m_eResult;

		// Token: 0x0400032A RID: 810
		public SteamNetworkingIdentity m_identity;

		// Token: 0x0400032B RID: 811
		public uint m_unIP;

		// Token: 0x0400032C RID: 812
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public ushort[] m_unPorts;
	}
}
