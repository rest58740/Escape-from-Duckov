using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017D RID: 381
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCDetails_t
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0000C978 File Offset: 0x0000AB78
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x0000C985 File Offset: 0x0000AB85
		public string m_rgchTitle
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchTitle_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchTitle_, 129);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0000C998 File Offset: 0x0000AB98
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x0000C9A5 File Offset: 0x0000ABA5
		public string m_rgchDescription
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchDescription_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchDescription_, 8000);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x0000C9C5 File Offset: 0x0000ABC5
		public string m_rgchTags
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchTags_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchTags_, 1025);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0000C9D8 File Offset: 0x0000ABD8
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x0000C9E5 File Offset: 0x0000ABE5
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x0000CA05 File Offset: 0x0000AC05
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

		// Token: 0x04000A1C RID: 2588
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000A1D RID: 2589
		public EResult m_eResult;

		// Token: 0x04000A1E RID: 2590
		public EWorkshopFileType m_eFileType;

		// Token: 0x04000A1F RID: 2591
		public AppId_t m_nCreatorAppID;

		// Token: 0x04000A20 RID: 2592
		public AppId_t m_nConsumerAppID;

		// Token: 0x04000A21 RID: 2593
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
		private byte[] m_rgchTitle_;

		// Token: 0x04000A22 RID: 2594
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8000)]
		private byte[] m_rgchDescription_;

		// Token: 0x04000A23 RID: 2595
		public ulong m_ulSteamIDOwner;

		// Token: 0x04000A24 RID: 2596
		public uint m_rtimeCreated;

		// Token: 0x04000A25 RID: 2597
		public uint m_rtimeUpdated;

		// Token: 0x04000A26 RID: 2598
		public uint m_rtimeAddedToUserList;

		// Token: 0x04000A27 RID: 2599
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x04000A28 RID: 2600
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000A29 RID: 2601
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;

		// Token: 0x04000A2A RID: 2602
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x04000A2B RID: 2603
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025)]
		private byte[] m_rgchTags_;

		// Token: 0x04000A2C RID: 2604
		public UGCHandle_t m_hFile;

		// Token: 0x04000A2D RID: 2605
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x04000A2E RID: 2606
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
		private byte[] m_pchFileName_;

		// Token: 0x04000A2F RID: 2607
		public int m_nFileSize;

		// Token: 0x04000A30 RID: 2608
		public int m_nPreviewFileSize;

		// Token: 0x04000A31 RID: 2609
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchURL_;

		// Token: 0x04000A32 RID: 2610
		public uint m_unVotesUp;

		// Token: 0x04000A33 RID: 2611
		public uint m_unVotesDown;

		// Token: 0x04000A34 RID: 2612
		public float m_flScore;

		// Token: 0x04000A35 RID: 2613
		public uint m_unNumChildren;

		// Token: 0x04000A36 RID: 2614
		public ulong m_ulTotalFilesSize;
	}
}
