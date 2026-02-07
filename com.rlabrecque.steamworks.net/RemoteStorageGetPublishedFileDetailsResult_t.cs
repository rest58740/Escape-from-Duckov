using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B5 RID: 181
	[CallbackIdentity(1318)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedFileDetailsResult_t
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0000C7FB File Offset: 0x0000A9FB
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x0000C808 File Offset: 0x0000AA08
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0000C81B File Offset: 0x0000AA1B
		// (set) Token: 0x060008AE RID: 2222 RVA: 0x0000C828 File Offset: 0x0000AA28
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0000C83B File Offset: 0x0000AA3B
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x0000C848 File Offset: 0x0000AA48
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

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0000C85B File Offset: 0x0000AA5B
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x0000C868 File Offset: 0x0000AA68
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

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0000C87B File Offset: 0x0000AA7B
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0000C888 File Offset: 0x0000AA88
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

		// Token: 0x040001F9 RID: 505
		public const int k_iCallback = 1318;

		// Token: 0x040001FA RID: 506
		public EResult m_eResult;

		// Token: 0x040001FB RID: 507
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001FC RID: 508
		public AppId_t m_nCreatorAppID;

		// Token: 0x040001FD RID: 509
		public AppId_t m_nConsumerAppID;

		// Token: 0x040001FE RID: 510
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
		private byte[] m_rgchTitle_;

		// Token: 0x040001FF RID: 511
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8000)]
		private byte[] m_rgchDescription_;

		// Token: 0x04000200 RID: 512
		public UGCHandle_t m_hFile;

		// Token: 0x04000201 RID: 513
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x04000202 RID: 514
		public ulong m_ulSteamIDOwner;

		// Token: 0x04000203 RID: 515
		public uint m_rtimeCreated;

		// Token: 0x04000204 RID: 516
		public uint m_rtimeUpdated;

		// Token: 0x04000205 RID: 517
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x04000206 RID: 518
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000207 RID: 519
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025)]
		private byte[] m_rgchTags_;

		// Token: 0x04000208 RID: 520
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x04000209 RID: 521
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
		private byte[] m_pchFileName_;

		// Token: 0x0400020A RID: 522
		public int m_nFileSize;

		// Token: 0x0400020B RID: 523
		public int m_nPreviewFileSize;

		// Token: 0x0400020C RID: 524
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchURL_;

		// Token: 0x0400020D RID: 525
		public EWorkshopFileType m_eFileType;

		// Token: 0x0400020E RID: 526
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;
	}
}
