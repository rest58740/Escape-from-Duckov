using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200000F RID: 15
	public static class SteamGameServerUGC
	{
		// Token: 0x06000192 RID: 402 RVA: 0x000057FF File Offset: 0x000039FF
		public static UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUserUGCRequest(CSteamGameServerAPIContext.GetSteamUGC(), unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000581F File Offset: 0x00003A1F
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryAllUGCRequestPage(CSteamGameServerAPIContext.GetSteamUGC(), eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000583C File Offset: 0x00003A3C
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, string pchCursor = null)
		{
			InteropHelp.TestIfAvailableGameServer();
			UGCQueryHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchCursor))
			{
				result = (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryAllUGCRequestCursor(CSteamGameServerAPIContext.GetSteamUGC(), eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00005888 File Offset: 0x00003A88
		public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUGCDetailsRequest(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000058A0 File Offset: 0x00003AA0
		public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SendQueryUGCRequest(CSteamGameServerAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000058B7 File Offset: 0x00003AB7
		public static bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCResult(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, out pDetails);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000058CB File Offset: 0x00003ACB
		public static uint GetQueryUGCNumTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCNumTags(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000058E0 File Offset: 0x00003AE0
		public static bool GetQueryUGCTag(UGCQueryHandle_t handle, uint index, uint indexTag, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, indexTag, intPtr, cchValueSize);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00005920 File Offset: 0x00003B20
		public static bool GetQueryUGCTagDisplayName(UGCQueryHandle_t handle, uint index, uint indexTag, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCTagDisplayName(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, indexTag, intPtr, cchValueSize);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00005960 File Offset: 0x00003B60
		public static bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCPreviewURL(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, intPtr, cchURLSize);
			pchURL = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000059A0 File Offset: 0x00003BA0
		public static bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchMetadatasize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCMetadata(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, intPtr, cchMetadatasize);
			pchMetadata = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000059DD File Offset: 0x00003BDD
		public static bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCChildren(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000059F2 File Offset: 0x00003BF2
		public static bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCStatistic(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, eStatType, out pStatValue);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00005A07 File Offset: 0x00003C07
		public static uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCNumAdditionalPreviews(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00005A1C File Offset: 0x00003C1C
		public static bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchOriginalFileNameSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCAdditionalPreview(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, previewIndex, intPtr, cchURLSize, intPtr2, cchOriginalFileNameSize, out pPreviewType);
			pchURLOrVideoID = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchOriginalFileName = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00005A7E File Offset: 0x00003C7E
		public static uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetQueryUGCNumKeyValueTags(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00005A94 File Offset: 0x00003C94
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchKeySize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCKeyValueTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, keyValueTagIndex, intPtr, cchKeySize, intPtr2, cchValueSize);
			pchKey = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00005AF4 File Offset: 0x00003CF4
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, string pchKey, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				bool flag = NativeMethods.ISteamUGC_GetQueryFirstUGCKeyValueTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, utf8StringHandle, intPtr, cchValueSize);
				pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
				Marshal.FreeHGlobal(intPtr);
				result = flag;
			}
			return result;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00005B5C File Offset: 0x00003D5C
		public static uint GetNumSupportedGameVersions(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetNumSupportedGameVersions(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00005B70 File Offset: 0x00003D70
		public static bool GetSupportedGameVersionData(UGCQueryHandle_t handle, uint index, uint versionIndex, out string pchGameBranchMin, out string pchGameBranchMax, uint cchGameBranchSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchGameBranchSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchGameBranchSize);
			bool flag = NativeMethods.ISteamUGC_GetSupportedGameVersionData(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, versionIndex, intPtr, intPtr2, cchGameBranchSize);
			pchGameBranchMin = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchGameBranchMax = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00005BCE File Offset: 0x00003DCE
		public static uint GetQueryUGCContentDescriptors(UGCQueryHandle_t handle, uint index, EUGCContentDescriptorID[] pvecDescriptors, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			if (pvecDescriptors != null && (long)pvecDescriptors.Length != (long)((ulong)cMaxEntries))
			{
				throw new ArgumentException("pvecDescriptors must be the same size as cMaxEntries!");
			}
			return NativeMethods.ISteamUGC_GetQueryUGCContentDescriptors(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, pvecDescriptors, cMaxEntries);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00005BF9 File Offset: 0x00003DF9
		public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_ReleaseQueryUGCRequest(CSteamGameServerAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00005C0C File Offset: 0x00003E0C
		public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				result = NativeMethods.ISteamUGC_AddRequiredTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00005C50 File Offset: 0x00003E50
		public static bool AddRequiredTagGroup(UGCQueryHandle_t handle, IList<string> pTagGroups)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_AddRequiredTagGroup(CSteamGameServerAPIContext.GetSteamUGC(), handle, new InteropHelp.SteamParamStringArray(pTagGroups));
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00005C70 File Offset: 0x00003E70
		public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				result = NativeMethods.ISteamUGC_AddExcludedTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005CB4 File Offset: 0x00003EB4
		public static bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnOnlyIDs(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnOnlyIDs);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00005CC7 File Offset: 0x00003EC7
		public static bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnKeyValueTags(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnKeyValueTags);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00005CDA File Offset: 0x00003EDA
		public static bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnLongDescription(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnLongDescription);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00005CED File Offset: 0x00003EED
		public static bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnMetadata(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnMetadata);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00005D00 File Offset: 0x00003F00
		public static bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnChildren(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnChildren);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00005D13 File Offset: 0x00003F13
		public static bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnAdditionalPreviews(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnAdditionalPreviews);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00005D26 File Offset: 0x00003F26
		public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnTotalOnly(CSteamGameServerAPIContext.GetSteamUGC(), handle, bReturnTotalOnly);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005D39 File Offset: 0x00003F39
		public static bool SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetReturnPlaytimeStats(CSteamGameServerAPIContext.GetSteamUGC(), handle, unDays);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00005D4C File Offset: 0x00003F4C
		public static bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				result = NativeMethods.ISteamUGC_SetLanguage(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00005D90 File Offset: 0x00003F90
		public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetAllowCachedResponse(CSteamGameServerAPIContext.GetSteamUGC(), handle, unMaxAgeSeconds);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00005DA3 File Offset: 0x00003FA3
		public static bool SetAdminQuery(UGCUpdateHandle_t handle, bool bAdminQuery)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetAdminQuery(CSteamGameServerAPIContext.GetSteamUGC(), handle, bAdminQuery);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00005DB8 File Offset: 0x00003FB8
		public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pMatchCloudFileName))
			{
				result = NativeMethods.ISteamUGC_SetCloudFileNameFilter(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00005DFC File Offset: 0x00003FFC
		public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetMatchAnyTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, bMatchAnyTag);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005E10 File Offset: 0x00004010
		public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pSearchText))
			{
				result = NativeMethods.ISteamUGC_SetSearchText(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005E54 File Offset: 0x00004054
		public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetRankedByTrendDays(CSteamGameServerAPIContext.GetSteamUGC(), handle, unDays);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00005E67 File Offset: 0x00004067
		public static bool SetTimeCreatedDateRange(UGCQueryHandle_t handle, uint rtStart, uint rtEnd)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetTimeCreatedDateRange(CSteamGameServerAPIContext.GetSteamUGC(), handle, rtStart, rtEnd);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00005E7B File Offset: 0x0000407B
		public static bool SetTimeUpdatedDateRange(UGCQueryHandle_t handle, uint rtStart, uint rtEnd)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetTimeUpdatedDateRange(CSteamGameServerAPIContext.GetSteamUGC(), handle, rtStart, rtEnd);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00005E90 File Offset: 0x00004090
		public static bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					result = NativeMethods.ISteamUGC_AddRequiredKeyValueTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00005EF4 File Offset: 0x000040F4
		public static SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RequestUGCDetails(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, unMaxAgeSeconds);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00005F0C File Offset: 0x0000410C
		public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_CreateItem(CSteamGameServerAPIContext.GetSteamUGC(), nConsumerAppId, eFileType);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00005F24 File Offset: 0x00004124
		public static UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (UGCUpdateHandle_t)NativeMethods.ISteamUGC_StartItemUpdate(CSteamGameServerAPIContext.GetSteamUGC(), nConsumerAppId, nPublishedFileID);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00005F3C File Offset: 0x0000413C
		public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				result = NativeMethods.ISteamUGC_SetItemTitle(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00005F80 File Offset: 0x00004180
		public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				result = NativeMethods.ISteamUGC_SetItemDescription(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00005FC4 File Offset: 0x000041C4
		public static bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				result = NativeMethods.ISteamUGC_SetItemUpdateLanguage(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006008 File Offset: 0x00004208
		public static bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMetaData))
			{
				result = NativeMethods.ISteamUGC_SetItemMetadata(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000604C File Offset: 0x0000424C
		public static bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetItemVisibility(CSteamGameServerAPIContext.GetSteamUGC(), handle, eVisibility);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000605F File Offset: 0x0000425F
		public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags, bool bAllowAdminTags = false)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetItemTags(CSteamGameServerAPIContext.GetSteamUGC(), updateHandle, new InteropHelp.SteamParamStringArray(pTags), bAllowAdminTags);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006080 File Offset: 0x00004280
		public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszContentFolder))
			{
				result = NativeMethods.ISteamUGC_SetItemContent(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000060C4 File Offset: 0x000042C4
		public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				result = NativeMethods.ISteamUGC_SetItemPreview(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00006108 File Offset: 0x00004308
		public static bool SetAllowLegacyUpload(UGCUpdateHandle_t handle, bool bAllowLegacyUpload)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_SetAllowLegacyUpload(CSteamGameServerAPIContext.GetSteamUGC(), handle, bAllowLegacyUpload);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000611B File Offset: 0x0000431B
		public static bool RemoveAllItemKeyValueTags(UGCUpdateHandle_t handle)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_RemoveAllItemKeyValueTags(CSteamGameServerAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00006130 File Offset: 0x00004330
		public static bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = NativeMethods.ISteamUGC_RemoveItemKeyValueTags(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00006174 File Offset: 0x00004374
		public static bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					result = NativeMethods.ISteamUGC_AddItemKeyValueTag(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000061D8 File Offset: 0x000043D8
		public static bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				result = NativeMethods.ISteamUGC_AddItemPreviewFile(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle, type);
			}
			return result;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000621C File Offset: 0x0000441C
		public static bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				result = NativeMethods.ISteamUGC_AddItemPreviewVideo(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00006260 File Offset: 0x00004460
		public static bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				result = NativeMethods.ISteamUGC_UpdateItemPreviewFile(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000062A4 File Offset: 0x000044A4
		public static bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				result = NativeMethods.ISteamUGC_UpdateItemPreviewVideo(CSteamGameServerAPIContext.GetSteamUGC(), handle, index, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000062E8 File Offset: 0x000044E8
		public static bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_RemoveItemPreview(CSteamGameServerAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000062FB File Offset: 0x000044FB
		public static bool AddContentDescriptor(UGCUpdateHandle_t handle, EUGCContentDescriptorID descid)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_AddContentDescriptor(CSteamGameServerAPIContext.GetSteamUGC(), handle, descid);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000630E File Offset: 0x0000450E
		public static bool RemoveContentDescriptor(UGCUpdateHandle_t handle, EUGCContentDescriptorID descid)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_RemoveContentDescriptor(CSteamGameServerAPIContext.GetSteamUGC(), handle, descid);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00006324 File Offset: 0x00004524
		public static bool SetRequiredGameVersions(UGCUpdateHandle_t handle, string pszGameBranchMin, string pszGameBranchMax)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszGameBranchMin))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pszGameBranchMax))
				{
					result = NativeMethods.ISteamUGC_SetRequiredGameVersions(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00006388 File Offset: 0x00004588
		public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
		{
			InteropHelp.TestIfAvailableGameServer();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchChangeNote))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUGC_SubmitItemUpdate(CSteamGameServerAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000063D0 File Offset: 0x000045D0
		public static EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetItemUpdateProgress(CSteamGameServerAPIContext.GetSteamUGC(), handle, out punBytesProcessed, out punBytesTotal);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000063E4 File Offset: 0x000045E4
		public static SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SetUserItemVote(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, bVoteUp);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000063FC File Offset: 0x000045FC
		public static SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetUserItemVote(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00006413 File Offset: 0x00004613
		public static SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddItemToFavorites(CSteamGameServerAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000642B File Offset: 0x0000462B
		public static SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveItemFromFavorites(CSteamGameServerAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00006443 File Offset: 0x00004643
		public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SubscribeItem(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000645A File Offset: 0x0000465A
		public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_UnsubscribeItem(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00006471 File Offset: 0x00004671
		public static uint GetNumSubscribedItems()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetNumSubscribedItems(CSteamGameServerAPIContext.GetSteamUGC());
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00006482 File Offset: 0x00004682
		public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetSubscribedItems(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00006495 File Offset: 0x00004695
		public static uint GetItemState(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetItemState(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000064A8 File Offset: 0x000046A8
		public static bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchFolderSize);
			bool flag = NativeMethods.ISteamUGC_GetItemInstallInfo(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, out punSizeOnDisk, intPtr, cchFolderSize, out punTimeStamp);
			pchFolder = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000064E7 File Offset: 0x000046E7
		public static bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetItemDownloadInfo(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000064FB File Offset: 0x000046FB
		public static bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_DownloadItem(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, bHighPriority);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00006510 File Offset: 0x00004710
		public static bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszFolder))
			{
				result = NativeMethods.ISteamUGC_BInitWorkshopForGameServer(CSteamGameServerAPIContext.GetSteamUGC(), unWorkshopDepotID, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00006554 File Offset: 0x00004754
		public static void SuspendDownloads(bool bSuspend)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamUGC_SuspendDownloads(CSteamGameServerAPIContext.GetSteamUGC(), bSuspend);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00006566 File Offset: 0x00004766
		public static SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StartPlaytimeTracking(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000657E File Offset: 0x0000477E
		public static SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTracking(CSteamGameServerAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00006596 File Offset: 0x00004796
		public static SteamAPICall_t StopPlaytimeTrackingForAllItems()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTrackingForAllItems(CSteamGameServerAPIContext.GetSteamUGC());
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000065AC File Offset: 0x000047AC
		public static SteamAPICall_t AddDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddDependency(CSteamGameServerAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000065C4 File Offset: 0x000047C4
		public static SteamAPICall_t RemoveDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveDependency(CSteamGameServerAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000065DC File Offset: 0x000047DC
		public static SteamAPICall_t AddAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddAppDependency(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000065F4 File Offset: 0x000047F4
		public static SteamAPICall_t RemoveAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveAppDependency(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000660C File Offset: 0x0000480C
		public static SteamAPICall_t GetAppDependencies(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetAppDependencies(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00006623 File Offset: 0x00004823
		public static SteamAPICall_t DeleteItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_DeleteItem(CSteamGameServerAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000663A File Offset: 0x0000483A
		public static bool ShowWorkshopEULA()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_ShowWorkshopEULA(CSteamGameServerAPIContext.GetSteamUGC());
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000664B File Offset: 0x0000484B
		public static SteamAPICall_t GetWorkshopEULAStatus()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetWorkshopEULAStatus(CSteamGameServerAPIContext.GetSteamUGC());
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00006661 File Offset: 0x00004861
		public static uint GetUserContentDescriptorPreferences(EUGCContentDescriptorID[] pvecDescriptors, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamUGC_GetUserContentDescriptorPreferences(CSteamGameServerAPIContext.GetSteamUGC(), pvecDescriptors, cMaxEntries);
		}
	}
}
