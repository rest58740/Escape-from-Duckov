using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BB RID: 443
	[Serializable]
	public struct SteamNetworkingErrMsg
	{
		// Token: 0x04000B02 RID: 2818
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		public byte[] m_SteamNetworkingErrMsg;
	}
}
