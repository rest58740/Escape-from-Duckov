using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B4 RID: 180
	[CallbackIdentity(1317)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDownloadUGCResult_t
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0000C7DB File Offset: 0x0000A9DB
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x0000C7E8 File Offset: 0x0000A9E8
		public string m_pchFileName
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_pchFileName_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_pchFileName_, 260);
			}
		}

		// Token: 0x040001F2 RID: 498
		public const int k_iCallback = 1317;

		// Token: 0x040001F3 RID: 499
		public EResult m_eResult;

		// Token: 0x040001F4 RID: 500
		public UGCHandle_t m_hFile;

		// Token: 0x040001F5 RID: 501
		public AppId_t m_nAppID;

		// Token: 0x040001F6 RID: 502
		public int m_nSizeInBytes;

		// Token: 0x040001F7 RID: 503
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
		private byte[] m_pchFileName_;

		// Token: 0x040001F8 RID: 504
		public ulong m_ulSteamIDOwner;
	}
}
