using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000042 RID: 66
	[CallbackIdentity(349)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct OverlayBrowserProtocolNavigation_t
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x0000C6BF File Offset: 0x0000A8BF
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x0000C6CC File Offset: 0x0000A8CC
		public string rgchURI
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.rgchURI_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.rgchURI_, 1024);
			}
		}

		// Token: 0x04000058 RID: 88
		public const int k_iCallback = 349;

		// Token: 0x04000059 RID: 89
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		private byte[] rgchURI_;
	}
}
