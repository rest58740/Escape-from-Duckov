using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A6 RID: 166
	[CallbackIdentity(1222)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetAuthenticationStatus_t
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0000C75B File Offset: 0x0000A95B
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x0000C768 File Offset: 0x0000A968
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

		// Token: 0x040001C2 RID: 450
		public const int k_iCallback = 1222;

		// Token: 0x040001C3 RID: 451
		public ESteamNetworkingAvailability m_eAvail;

		// Token: 0x040001C4 RID: 452
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_debugMsg_;
	}
}
