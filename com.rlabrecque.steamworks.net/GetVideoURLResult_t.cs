using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000100 RID: 256
	[CallbackIdentity(4611)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetVideoURLResult_t
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0000C958 File Offset: 0x0000AB58
		// (set) Token: 0x060008C2 RID: 2242 RVA: 0x0000C965 File Offset: 0x0000AB65
		public string m_rgchURL
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchURL_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchURL_, 256);
			}
		}

		// Token: 0x0400031D RID: 797
		public const int k_iCallback = 4611;

		// Token: 0x0400031E RID: 798
		public EResult m_eResult;

		// Token: 0x0400031F RID: 799
		public AppId_t m_unVideoAppID;

		// Token: 0x04000320 RID: 800
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchURL_;
	}
}
