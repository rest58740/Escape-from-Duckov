using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200008A RID: 138
	[CallbackIdentity(5301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct JoinPartyCallback_t
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x0000C73B File Offset: 0x0000A93B
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x0000C748 File Offset: 0x0000A948
		public string m_rgchConnectString
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchConnectString_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchConnectString_, 256);
			}
		}

		// Token: 0x0400018A RID: 394
		public const int k_iCallback = 5301;

		// Token: 0x0400018B RID: 395
		public EResult m_eResult;

		// Token: 0x0400018C RID: 396
		public PartyBeaconID_t m_ulBeaconID;

		// Token: 0x0400018D RID: 397
		public CSteamID m_SteamIDBeaconOwner;

		// Token: 0x0400018E RID: 398
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchConnectString_;
	}
}
