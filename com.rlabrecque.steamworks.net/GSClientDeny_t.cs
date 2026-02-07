using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000048 RID: 72
	[CallbackIdentity(202)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientDeny_t
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0000C6DF File Offset: 0x0000A8DF
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x0000C6EC File Offset: 0x0000A8EC
		public string m_rgchOptionalText
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchOptionalText_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchOptionalText_, 128);
			}
		}

		// Token: 0x0400006B RID: 107
		public const int k_iCallback = 202;

		// Token: 0x0400006C RID: 108
		public CSteamID m_SteamID;

		// Token: 0x0400006D RID: 109
		public EDenyReason m_eDenyReason;

		// Token: 0x0400006E RID: 110
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_rgchOptionalText_;
	}
}
