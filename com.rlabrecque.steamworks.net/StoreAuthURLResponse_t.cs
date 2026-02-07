using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E7 RID: 231
	[CallbackIdentity(165)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StoreAuthURLResponse_t
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0000C8F8 File Offset: 0x0000AAF8
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x0000C905 File Offset: 0x0000AB05
		public string m_szURL
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szURL_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szURL_, 512);
			}
		}

		// Token: 0x040002C7 RID: 711
		public const int k_iCallback = 165;

		// Token: 0x040002C8 RID: 712
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		private byte[] m_szURL_;
	}
}
