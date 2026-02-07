using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E6 RID: 230
	[CallbackIdentity(164)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameWebCallback_t
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		// (set) Token: 0x060008BA RID: 2234 RVA: 0x0000C8E5 File Offset: 0x0000AAE5
		public string m_szURL
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szURL_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szURL_, 256);
			}
		}

		// Token: 0x040002C5 RID: 709
		public const int k_iCallback = 164;

		// Token: 0x040002C6 RID: 710
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_szURL_;
	}
}
