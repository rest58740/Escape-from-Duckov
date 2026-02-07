using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000031 RID: 49
	[CallbackIdentity(332)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameServerChangeRequested_t
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x0000C665 File Offset: 0x0000A865
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x0000C672 File Offset: 0x0000A872
		public string m_rgchServer
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchServer_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchServer_, 64);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x0000C682 File Offset: 0x0000A882
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x0000C68F File Offset: 0x0000A88F
		public string m_rgchPassword
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchPassword_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchPassword_, 64);
			}
		}

		// Token: 0x0400001D RID: 29
		public const int k_iCallback = 332;

		// Token: 0x0400001E RID: 30
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_rgchServer_;

		// Token: 0x0400001F RID: 31
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_rgchPassword_;
	}
}
