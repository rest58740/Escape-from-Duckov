using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C9 RID: 201
	[CallbackIdentity(3401)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCQueryCompleted_t
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0000C8B8 File Offset: 0x0000AAB8
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x0000C8C5 File Offset: 0x0000AAC5
		public string m_rgchNextCursor
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchNextCursor_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchNextCursor_, 256);
			}
		}

		// Token: 0x0400025A RID: 602
		public const int k_iCallback = 3401;

		// Token: 0x0400025B RID: 603
		public UGCQueryHandle_t m_handle;

		// Token: 0x0400025C RID: 604
		public EResult m_eResult;

		// Token: 0x0400025D RID: 605
		public uint m_unNumResultsReturned;

		// Token: 0x0400025E RID: 606
		public uint m_unTotalMatchingResults;

		// Token: 0x0400025F RID: 607
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;

		// Token: 0x04000260 RID: 608
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchNextCursor_;
	}
}
