using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AB RID: 171
	[CallbackIdentity(5703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamRemotePlayTogetherGuestInvite_t
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0000C79B File Offset: 0x0000A99B
		// (set) Token: 0x060008A6 RID: 2214 RVA: 0x0000C7A8 File Offset: 0x0000A9A8
		public string m_szConnectURL
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szConnectURL_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szConnectURL_, 1024);
			}
		}

		// Token: 0x040001D0 RID: 464
		public const int k_iCallback = 5703;

		// Token: 0x040001D1 RID: 465
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		private byte[] m_szConnectURL_;
	}
}
