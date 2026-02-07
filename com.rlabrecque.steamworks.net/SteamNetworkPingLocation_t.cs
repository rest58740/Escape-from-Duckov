using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000184 RID: 388
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetworkPingLocation_t
	{
		// Token: 0x04000A63 RID: 2659
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		public byte[] m_data;
	}
}
