using System;
using System.Collections.Generic;

namespace Steamworks
{
	// Token: 0x02000021 RID: 33
	public static class SteamRemoteStorage
	{
		// Token: 0x06000397 RID: 919 RVA: 0x000096D0 File Offset: 0x000078D0
		public static bool FileWrite(string pchFile, byte[] pvData, int cubData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileWrite(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle, pvData, cubData);
			}
			return result;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00009714 File Offset: 0x00007914
		public static int FileRead(string pchFile, byte[] pvData, int cubDataToRead)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileRead(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle, pvData, cubDataToRead);
			}
			return result;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00009758 File Offset: 0x00007958
		public static SteamAPICall_t FileWriteAsync(string pchFile, byte[] pvData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_FileWriteAsync(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle, pvData, cubData);
			}
			return result;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000097A4 File Offset: 0x000079A4
		public static SteamAPICall_t FileReadAsync(string pchFile, uint nOffset, uint cubToRead)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_FileReadAsync(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle, nOffset, cubToRead);
			}
			return result;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000097F0 File Offset: 0x000079F0
		public static bool FileReadAsyncComplete(SteamAPICall_t hReadCall, byte[] pvBuffer, uint cubToRead)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileReadAsyncComplete(CSteamAPIContext.GetSteamRemoteStorage(), hReadCall, pvBuffer, cubToRead);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00009804 File Offset: 0x00007A04
		public static bool FileForget(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileForget(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00009848 File Offset: 0x00007A48
		public static bool FileDelete(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileDelete(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000988C File Offset: 0x00007A8C
		public static SteamAPICall_t FileShare(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_FileShare(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x000098D4 File Offset: 0x00007AD4
		public static bool SetSyncPlatforms(string pchFile, ERemoteStoragePlatform eRemoteStoragePlatform)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_SetSyncPlatforms(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle, eRemoteStoragePlatform);
			}
			return result;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00009918 File Offset: 0x00007B18
		public static UGCFileWriteStreamHandle_t FileWriteStreamOpen(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			UGCFileWriteStreamHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = (UGCFileWriteStreamHandle_t)NativeMethods.ISteamRemoteStorage_FileWriteStreamOpen(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00009960 File Offset: 0x00007B60
		public static bool FileWriteStreamWriteChunk(UGCFileWriteStreamHandle_t writeHandle, byte[] pvData, int cubData)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileWriteStreamWriteChunk(CSteamAPIContext.GetSteamRemoteStorage(), writeHandle, pvData, cubData);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00009974 File Offset: 0x00007B74
		public static bool FileWriteStreamClose(UGCFileWriteStreamHandle_t writeHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileWriteStreamClose(CSteamAPIContext.GetSteamRemoteStorage(), writeHandle);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00009986 File Offset: 0x00007B86
		public static bool FileWriteStreamCancel(UGCFileWriteStreamHandle_t writeHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_FileWriteStreamCancel(CSteamAPIContext.GetSteamRemoteStorage(), writeHandle);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00009998 File Offset: 0x00007B98
		public static bool FileExists(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FileExists(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x000099DC File Offset: 0x00007BDC
		public static bool FilePersisted(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_FilePersisted(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00009A20 File Offset: 0x00007C20
		public static int GetFileSize(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_GetFileSize(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00009A64 File Offset: 0x00007C64
		public static long GetFileTimestamp(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			long result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_GetFileTimestamp(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00009AA8 File Offset: 0x00007CA8
		public static ERemoteStoragePlatform GetSyncPlatforms(string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			ERemoteStoragePlatform result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_GetSyncPlatforms(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00009AEC File Offset: 0x00007CEC
		public static int GetFileCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetFileCount(CSteamAPIContext.GetSteamRemoteStorage());
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00009AFD File Offset: 0x00007CFD
		public static string GetFileNameAndSize(int iFile, out int pnFileSizeInBytes)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamRemoteStorage_GetFileNameAndSize(CSteamAPIContext.GetSteamRemoteStorage(), iFile, out pnFileSizeInBytes));
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00009B15 File Offset: 0x00007D15
		public static bool GetQuota(out ulong pnTotalBytes, out ulong puAvailableBytes)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetQuota(CSteamAPIContext.GetSteamRemoteStorage(), out pnTotalBytes, out puAvailableBytes);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00009B28 File Offset: 0x00007D28
		public static bool IsCloudEnabledForAccount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForAccount(CSteamAPIContext.GetSteamRemoteStorage());
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00009B39 File Offset: 0x00007D39
		public static bool IsCloudEnabledForApp()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForApp(CSteamAPIContext.GetSteamRemoteStorage());
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00009B4A File Offset: 0x00007D4A
		public static void SetCloudEnabledForApp(bool bEnabled)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamRemoteStorage_SetCloudEnabledForApp(CSteamAPIContext.GetSteamRemoteStorage(), bEnabled);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00009B5C File Offset: 0x00007D5C
		public static SteamAPICall_t UGCDownload(UGCHandle_t hContent, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UGCDownload(CSteamAPIContext.GetSteamRemoteStorage(), hContent, unPriority);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00009B74 File Offset: 0x00007D74
		public static bool GetUGCDownloadProgress(UGCHandle_t hContent, out int pnBytesDownloaded, out int pnBytesExpected)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetUGCDownloadProgress(CSteamAPIContext.GetSteamRemoteStorage(), hContent, out pnBytesDownloaded, out pnBytesExpected);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00009B88 File Offset: 0x00007D88
		public static bool GetUGCDetails(UGCHandle_t hContent, out AppId_t pnAppID, out string ppchName, out int pnFileSizeInBytes, out CSteamID pSteamIDOwner)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr nativeUtf;
			bool flag = NativeMethods.ISteamRemoteStorage_GetUGCDetails(CSteamAPIContext.GetSteamRemoteStorage(), hContent, out pnAppID, out nativeUtf, out pnFileSizeInBytes, out pSteamIDOwner);
			ppchName = (flag ? InteropHelp.PtrToStringUTF8(nativeUtf) : null);
			return flag;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00009BBB File Offset: 0x00007DBB
		public static int UGCRead(UGCHandle_t hContent, byte[] pvData, int cubDataToRead, uint cOffset, EUGCReadAction eAction)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_UGCRead(CSteamAPIContext.GetSteamRemoteStorage(), hContent, pvData, cubDataToRead, cOffset, eAction);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00009BD2 File Offset: 0x00007DD2
		public static int GetCachedUGCCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetCachedUGCCount(CSteamAPIContext.GetSteamRemoteStorage());
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00009BE3 File Offset: 0x00007DE3
		public static UGCHandle_t GetCachedUGCHandle(int iCachedContent)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCHandle_t)NativeMethods.ISteamRemoteStorage_GetCachedUGCHandle(CSteamAPIContext.GetSteamRemoteStorage(), iCachedContent);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00009BFC File Offset: 0x00007DFC
		public static SteamAPICall_t PublishWorkshopFile(string pchFile, string pchPreviewFile, AppId_t nConsumerAppId, string pchTitle, string pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IList<string> pTags, EWorkshopFileType eWorkshopFileType)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchTitle))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchDescription))
						{
							result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_PublishWorkshopFile(CSteamAPIContext.GetSteamRemoteStorage(), utf8StringHandle, utf8StringHandle2, nConsumerAppId, utf8StringHandle3, utf8StringHandle4, eVisibility, new InteropHelp.SteamParamStringArray(pTags), eWorkshopFileType);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00009CB4 File Offset: 0x00007EB4
		public static PublishedFileUpdateHandle_t CreatePublishedFileUpdateRequest(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (PublishedFileUpdateHandle_t)NativeMethods.ISteamRemoteStorage_CreatePublishedFileUpdateRequest(CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00009CCC File Offset: 0x00007ECC
		public static bool UpdatePublishedFileFile(PublishedFileUpdateHandle_t updateHandle, string pchFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchFile))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileFile(CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00009D10 File Offset: 0x00007F10
		public static bool UpdatePublishedFilePreviewFile(PublishedFileUpdateHandle_t updateHandle, string pchPreviewFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPreviewFile))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFilePreviewFile(CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00009D54 File Offset: 0x00007F54
		public static bool UpdatePublishedFileTitle(PublishedFileUpdateHandle_t updateHandle, string pchTitle)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTitle(CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00009D98 File Offset: 0x00007F98
		public static bool UpdatePublishedFileDescription(PublishedFileUpdateHandle_t updateHandle, string pchDescription)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileDescription(CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00009DDC File Offset: 0x00007FDC
		public static bool UpdatePublishedFileVisibility(PublishedFileUpdateHandle_t updateHandle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileVisibility(CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, eVisibility);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00009DEF File Offset: 0x00007FEF
		public static bool UpdatePublishedFileTags(PublishedFileUpdateHandle_t updateHandle, IList<string> pTags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTags(CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, new InteropHelp.SteamParamStringArray(pTags));
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00009E0C File Offset: 0x0000800C
		public static SteamAPICall_t CommitPublishedFileUpdate(PublishedFileUpdateHandle_t updateHandle)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_CommitPublishedFileUpdate(CSteamAPIContext.GetSteamRemoteStorage(), updateHandle);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00009E23 File Offset: 0x00008023
		public static SteamAPICall_t GetPublishedFileDetails(PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_GetPublishedFileDetails(CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId, unMaxSecondsOld);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00009E3B File Offset: 0x0000803B
		public static SteamAPICall_t DeletePublishedFile(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_DeletePublishedFile(CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00009E52 File Offset: 0x00008052
		public static SteamAPICall_t EnumerateUserPublishedFiles(uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumerateUserPublishedFiles(CSteamAPIContext.GetSteamRemoteStorage(), unStartIndex);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00009E69 File Offset: 0x00008069
		public static SteamAPICall_t SubscribePublishedFile(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_SubscribePublishedFile(CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00009E80 File Offset: 0x00008080
		public static SteamAPICall_t EnumerateUserSubscribedFiles(uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumerateUserSubscribedFiles(CSteamAPIContext.GetSteamRemoteStorage(), unStartIndex);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00009E97 File Offset: 0x00008097
		public static SteamAPICall_t UnsubscribePublishedFile(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UnsubscribePublishedFile(CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00009EB0 File Offset: 0x000080B0
		public static bool UpdatePublishedFileSetChangeDescription(PublishedFileUpdateHandle_t updateHandle, string pchChangeDescription)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchChangeDescription))
			{
				result = NativeMethods.ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00009EF4 File Offset: 0x000080F4
		public static SteamAPICall_t GetPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_GetPublishedItemVoteDetails(CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00009F0B File Offset: 0x0000810B
		public static SteamAPICall_t UpdateUserPublishedItemVote(PublishedFileId_t unPublishedFileId, bool bVoteUp)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UpdateUserPublishedItemVote(CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId, bVoteUp);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00009F23 File Offset: 0x00008123
		public static SteamAPICall_t GetUserPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_GetUserPublishedItemVoteDetails(CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00009F3A File Offset: 0x0000813A
		public static SteamAPICall_t EnumerateUserSharedWorkshopFiles(CSteamID steamId, uint unStartIndex, IList<string> pRequiredTags, IList<string> pExcludedTags)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(CSteamAPIContext.GetSteamRemoteStorage(), steamId, unStartIndex, new InteropHelp.SteamParamStringArray(pRequiredTags), new InteropHelp.SteamParamStringArray(pExcludedTags));
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00009F68 File Offset: 0x00008168
		public static SteamAPICall_t PublishVideo(EWorkshopVideoProvider eVideoProvider, string pchVideoAccount, string pchVideoIdentifier, string pchPreviewFile, AppId_t nConsumerAppId, string pchTitle, string pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IList<string> pTags)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVideoAccount))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchVideoIdentifier))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchTitle))
						{
							using (InteropHelp.UTF8StringHandle utf8StringHandle5 = new InteropHelp.UTF8StringHandle(pchDescription))
							{
								result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_PublishVideo(CSteamAPIContext.GetSteamRemoteStorage(), eVideoProvider, utf8StringHandle, utf8StringHandle2, utf8StringHandle3, nConsumerAppId, utf8StringHandle4, utf8StringHandle5, eVisibility, new InteropHelp.SteamParamStringArray(pTags));
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000A040 File Offset: 0x00008240
		public static SteamAPICall_t SetUserPublishedFileAction(PublishedFileId_t unPublishedFileId, EWorkshopFileAction eAction)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_SetUserPublishedFileAction(CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId, eAction);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000A058 File Offset: 0x00008258
		public static SteamAPICall_t EnumeratePublishedFilesByUserAction(EWorkshopFileAction eAction, uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(CSteamAPIContext.GetSteamRemoteStorage(), eAction, unStartIndex);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000A070 File Offset: 0x00008270
		public static SteamAPICall_t EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, IList<string> pTags, IList<string> pUserTags)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(CSteamAPIContext.GetSteamRemoteStorage(), eEnumerationType, unStartIndex, unCount, unDays, new InteropHelp.SteamParamStringArray(pTags), new InteropHelp.SteamParamStringArray(pUserTags));
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000A0A4 File Offset: 0x000082A4
		public static SteamAPICall_t UGCDownloadToLocation(UGCHandle_t hContent, string pchLocation, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLocation))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamRemoteStorage_UGCDownloadToLocation(CSteamAPIContext.GetSteamRemoteStorage(), hContent, utf8StringHandle, unPriority);
			}
			return result;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000A0F0 File Offset: 0x000082F0
		public static int GetLocalFileChangeCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_GetLocalFileChangeCount(CSteamAPIContext.GetSteamRemoteStorage());
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000A101 File Offset: 0x00008301
		public static string GetLocalFileChange(int iFile, out ERemoteStorageLocalFileChange pEChangeType, out ERemoteStorageFilePathType pEFilePathType)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamRemoteStorage_GetLocalFileChange(CSteamAPIContext.GetSteamRemoteStorage(), iFile, out pEChangeType, out pEFilePathType));
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000A11A File Offset: 0x0000831A
		public static bool BeginFileWriteBatch()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_BeginFileWriteBatch(CSteamAPIContext.GetSteamRemoteStorage());
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000A12B File Offset: 0x0000832B
		public static bool EndFileWriteBatch()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_EndFileWriteBatch(CSteamAPIContext.GetSteamRemoteStorage());
		}
	}
}
