using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AC RID: 172
	[CallbackIdentity(1307)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileShareResult_t
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0000C7BB File Offset: 0x0000A9BB
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
		public string m_rgchFilename
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchFilename_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchFilename_, 260);
			}
		}

		// Token: 0x040001D2 RID: 466
		public const int k_iCallback = 1307;

		// Token: 0x040001D3 RID: 467
		public EResult m_eResult;

		// Token: 0x040001D4 RID: 468
		public UGCHandle_t m_hFile;

		// Token: 0x040001D5 RID: 469
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
		private byte[] m_rgchFilename_;
	}
}
