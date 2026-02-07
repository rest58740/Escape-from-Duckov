using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A7 RID: 167
	[CallbackIdentity(1281)]
	public struct SteamRelayNetworkStatus_t
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0000C77B File Offset: 0x0000A97B
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x0000C788 File Offset: 0x0000A988
		public string m_debugMsg
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_debugMsg_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_debugMsg_, 256);
			}
		}

		// Token: 0x040001C5 RID: 453
		public const int k_iCallback = 1281;

		// Token: 0x040001C6 RID: 454
		public ESteamNetworkingAvailability m_eAvail;

		// Token: 0x040001C7 RID: 455
		public int m_bPingMeasurementInProgress;

		// Token: 0x040001C8 RID: 456
		public ESteamNetworkingAvailability m_eAvailNetworkConfig;

		// Token: 0x040001C9 RID: 457
		public ESteamNetworkingAvailability m_eAvailAnyRelay;

		// Token: 0x040001CA RID: 458
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_debugMsg_;
	}
}
