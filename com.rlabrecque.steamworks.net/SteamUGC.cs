using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000024 RID: 36
	public static class SteamUGC
	{
		// Token: 0x060003ED RID: 1005 RVA: 0x0000A76A File Offset: 0x0000896A
		public static UGCQueryHandle_t CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUserUGCRequest(CSteamAPIContext.GetSteamUGC(), unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000A78A File Offset: 0x0000898A
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryAllUGCRequestPage(CSteamAPIContext.GetSteamUGC(), eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000A7A8 File Offset: 0x000089A8
		public static UGCQueryHandle_t CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, string pchCursor = null)
		{
			InteropHelp.TestIfAvailableClient();
			UGCQueryHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchCursor))
			{
				result = (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryAllUGCRequestCursor(CSteamAPIContext.GetSteamUGC(), eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000A7F4 File Offset: 0x000089F4
		public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCQueryHandle_t)NativeMethods.ISteamUGC_CreateQueryUGCDetailsRequest(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000A80C File Offset: 0x00008A0C
		public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SendQueryUGCRequest(CSteamAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000A823 File Offset: 0x00008A23
		public static bool GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCResult(CSteamAPIContext.GetSteamUGC(), handle, index, out pDetails);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000A837 File Offset: 0x00008A37
		public static uint GetQueryUGCNumTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCNumTags(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000A84C File Offset: 0x00008A4C
		public static bool GetQueryUGCTag(UGCQueryHandle_t handle, uint index, uint indexTag, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCTag(CSteamAPIContext.GetSteamUGC(), handle, index, indexTag, intPtr, cchValueSize);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000A88C File Offset: 0x00008A8C
		public static bool GetQueryUGCTagDisplayName(UGCQueryHandle_t handle, uint index, uint indexTag, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCTagDisplayName(CSteamAPIContext.GetSteamUGC(), handle, index, indexTag, intPtr, cchValueSize);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000A8CC File Offset: 0x00008ACC
		public static bool GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, out string pchURL, uint cchURLSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCPreviewURL(CSteamAPIContext.GetSteamUGC(), handle, index, intPtr, cchURLSize);
			pchURL = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000A90C File Offset: 0x00008B0C
		public static bool GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, out string pchMetadata, uint cchMetadatasize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchMetadatasize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCMetadata(CSteamAPIContext.GetSteamUGC(), handle, index, intPtr, cchMetadatasize);
			pchMetadata = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000A949 File Offset: 0x00008B49
		public static bool GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCChildren(CSteamAPIContext.GetSteamUGC(), handle, index, pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000A95E File Offset: 0x00008B5E
		public static bool GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out ulong pStatValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCStatistic(CSteamAPIContext.GetSteamUGC(), handle, index, eStatType, out pStatValue);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000A973 File Offset: 0x00008B73
		public static uint GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCNumAdditionalPreviews(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000A988 File Offset: 0x00008B88
		public static bool GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, out string pchURLOrVideoID, uint cchURLSize, out string pchOriginalFileName, uint cchOriginalFileNameSize, out EItemPreviewType pPreviewType)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchURLSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchOriginalFileNameSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCAdditionalPreview(CSteamAPIContext.GetSteamUGC(), handle, index, previewIndex, intPtr, cchURLSize, intPtr2, cchOriginalFileNameSize, out pPreviewType);
			pchURLOrVideoID = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchOriginalFileName = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000A9EA File Offset: 0x00008BEA
		public static uint GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetQueryUGCNumKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000AA00 File Offset: 0x00008C00
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, out string pchKey, uint cchKeySize, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchKeySize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchValueSize);
			bool flag = NativeMethods.ISteamUGC_GetQueryUGCKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, index, keyValueTagIndex, intPtr, cchKeySize, intPtr2, cchValueSize);
			pchKey = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000AA60 File Offset: 0x00008C60
		public static bool GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, string pchKey, out string pchValue, uint cchValueSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchValueSize);
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				bool flag = NativeMethods.ISteamUGC_GetQueryFirstUGCKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, index, utf8StringHandle, intPtr, cchValueSize);
				pchValue = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
				Marshal.FreeHGlobal(intPtr);
				result = flag;
			}
			return result;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000AAC8 File Offset: 0x00008CC8
		public static uint GetNumSupportedGameVersions(UGCQueryHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetNumSupportedGameVersions(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000AADC File Offset: 0x00008CDC
		public static bool GetSupportedGameVersionData(UGCQueryHandle_t handle, uint index, uint versionIndex, out string pchGameBranchMin, out string pchGameBranchMax, uint cchGameBranchSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchGameBranchSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)cchGameBranchSize);
			bool flag = NativeMethods.ISteamUGC_GetSupportedGameVersionData(CSteamAPIContext.GetSteamUGC(), handle, index, versionIndex, intPtr, intPtr2, cchGameBranchSize);
			pchGameBranchMin = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchGameBranchMax = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000AB3A File Offset: 0x00008D3A
		public static uint GetQueryUGCContentDescriptors(UGCQueryHandle_t handle, uint index, EUGCContentDescriptorID[] pvecDescriptors, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			if (pvecDescriptors != null && (long)pvecDescriptors.Length != (long)((ulong)cMaxEntries))
			{
				throw new ArgumentException("pvecDescriptors must be the same size as cMaxEntries!");
			}
			return NativeMethods.ISteamUGC_GetQueryUGCContentDescriptors(CSteamAPIContext.GetSteamUGC(), handle, index, pvecDescriptors, cMaxEntries);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000AB65 File Offset: 0x00008D65
		public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_ReleaseQueryUGCRequest(CSteamAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000AB78 File Offset: 0x00008D78
		public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				result = NativeMethods.ISteamUGC_AddRequiredTag(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000ABBC File Offset: 0x00008DBC
		public static bool AddRequiredTagGroup(UGCQueryHandle_t handle, IList<string> pTagGroups)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_AddRequiredTagGroup(CSteamAPIContext.GetSteamUGC(), handle, new InteropHelp.SteamParamStringArray(pTagGroups));
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000ABDC File Offset: 0x00008DDC
		public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pTagName))
			{
				result = NativeMethods.ISteamUGC_AddExcludedTag(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000AC20 File Offset: 0x00008E20
		public static bool SetReturnOnlyIDs(UGCQueryHandle_t handle, bool bReturnOnlyIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnOnlyIDs(CSteamAPIContext.GetSteamUGC(), handle, bReturnOnlyIDs);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000AC33 File Offset: 0x00008E33
		public static bool SetReturnKeyValueTags(UGCQueryHandle_t handle, bool bReturnKeyValueTags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle, bReturnKeyValueTags);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000AC46 File Offset: 0x00008E46
		public static bool SetReturnLongDescription(UGCQueryHandle_t handle, bool bReturnLongDescription)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnLongDescription(CSteamAPIContext.GetSteamUGC(), handle, bReturnLongDescription);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000AC59 File Offset: 0x00008E59
		public static bool SetReturnMetadata(UGCQueryHandle_t handle, bool bReturnMetadata)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnMetadata(CSteamAPIContext.GetSteamUGC(), handle, bReturnMetadata);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000AC6C File Offset: 0x00008E6C
		public static bool SetReturnChildren(UGCQueryHandle_t handle, bool bReturnChildren)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnChildren(CSteamAPIContext.GetSteamUGC(), handle, bReturnChildren);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000AC7F File Offset: 0x00008E7F
		public static bool SetReturnAdditionalPreviews(UGCQueryHandle_t handle, bool bReturnAdditionalPreviews)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnAdditionalPreviews(CSteamAPIContext.GetSteamUGC(), handle, bReturnAdditionalPreviews);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000AC92 File Offset: 0x00008E92
		public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnTotalOnly(CSteamAPIContext.GetSteamUGC(), handle, bReturnTotalOnly);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000ACA5 File Offset: 0x00008EA5
		public static bool SetReturnPlaytimeStats(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetReturnPlaytimeStats(CSteamAPIContext.GetSteamUGC(), handle, unDays);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000ACB8 File Offset: 0x00008EB8
		public static bool SetLanguage(UGCQueryHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				result = NativeMethods.ISteamUGC_SetLanguage(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000ACFC File Offset: 0x00008EFC
		public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetAllowCachedResponse(CSteamAPIContext.GetSteamUGC(), handle, unMaxAgeSeconds);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000AD0F File Offset: 0x00008F0F
		public static bool SetAdminQuery(UGCUpdateHandle_t handle, bool bAdminQuery)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetAdminQuery(CSteamAPIContext.GetSteamUGC(), handle, bAdminQuery);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000AD24 File Offset: 0x00008F24
		public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pMatchCloudFileName))
			{
				result = NativeMethods.ISteamUGC_SetCloudFileNameFilter(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000AD68 File Offset: 0x00008F68
		public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetMatchAnyTag(CSteamAPIContext.GetSteamUGC(), handle, bMatchAnyTag);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000AD7C File Offset: 0x00008F7C
		public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pSearchText))
			{
				result = NativeMethods.ISteamUGC_SetSearchText(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000ADC0 File Offset: 0x00008FC0
		public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetRankedByTrendDays(CSteamAPIContext.GetSteamUGC(), handle, unDays);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000ADD3 File Offset: 0x00008FD3
		public static bool SetTimeCreatedDateRange(UGCQueryHandle_t handle, uint rtStart, uint rtEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetTimeCreatedDateRange(CSteamAPIContext.GetSteamUGC(), handle, rtStart, rtEnd);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000ADE7 File Offset: 0x00008FE7
		public static bool SetTimeUpdatedDateRange(UGCQueryHandle_t handle, uint rtStart, uint rtEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetTimeUpdatedDateRange(CSteamAPIContext.GetSteamUGC(), handle, rtStart, rtEnd);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000ADFC File Offset: 0x00008FFC
		public static bool AddRequiredKeyValueTag(UGCQueryHandle_t handle, string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					result = NativeMethods.ISteamUGC_AddRequiredKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000AE60 File Offset: 0x00009060
		public static SteamAPICall_t RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RequestUGCDetails(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, unMaxAgeSeconds);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000AE78 File Offset: 0x00009078
		public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_CreateItem(CSteamAPIContext.GetSteamUGC(), nConsumerAppId, eFileType);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000AE90 File Offset: 0x00009090
		public static UGCUpdateHandle_t StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (UGCUpdateHandle_t)NativeMethods.ISteamUGC_StartItemUpdate(CSteamAPIContext.GetSteamUGC(), nConsumerAppId, nPublishedFileID);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				result = NativeMethods.ISteamUGC_SetItemTitle(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000AEEC File Offset: 0x000090EC
		public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				result = NativeMethods.ISteamUGC_SetItemDescription(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000AF30 File Offset: 0x00009130
		public static bool SetItemUpdateLanguage(UGCUpdateHandle_t handle, string pchLanguage)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLanguage))
			{
				result = NativeMethods.ISteamUGC_SetItemUpdateLanguage(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000AF74 File Offset: 0x00009174
		public static bool SetItemMetadata(UGCUpdateHandle_t handle, string pchMetaData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMetaData))
			{
				result = NativeMethods.ISteamUGC_SetItemMetadata(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000AFB8 File Offset: 0x000091B8
		public static bool SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetItemVisibility(CSteamAPIContext.GetSteamUGC(), handle, eVisibility);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000AFCB File Offset: 0x000091CB
		public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags, bool bAllowAdminTags = false)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetItemTags(CSteamAPIContext.GetSteamUGC(), updateHandle, new InteropHelp.SteamParamStringArray(pTags), bAllowAdminTags);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000AFEC File Offset: 0x000091EC
		public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszContentFolder))
			{
				result = NativeMethods.ISteamUGC_SetItemContent(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000B030 File Offset: 0x00009230
		public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				result = NativeMethods.ISteamUGC_SetItemPreview(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000B074 File Offset: 0x00009274
		public static bool SetAllowLegacyUpload(UGCUpdateHandle_t handle, bool bAllowLegacyUpload)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_SetAllowLegacyUpload(CSteamAPIContext.GetSteamUGC(), handle, bAllowLegacyUpload);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000B087 File Offset: 0x00009287
		public static bool RemoveAllItemKeyValueTags(UGCUpdateHandle_t handle)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_RemoveAllItemKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000B09C File Offset: 0x0000929C
		public static bool RemoveItemKeyValueTags(UGCUpdateHandle_t handle, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = NativeMethods.ISteamUGC_RemoveItemKeyValueTags(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000B0E0 File Offset: 0x000092E0
		public static bool AddItemKeyValueTag(UGCUpdateHandle_t handle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					result = NativeMethods.ISteamUGC_AddItemKeyValueTag(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000B144 File Offset: 0x00009344
		public static bool AddItemPreviewFile(UGCUpdateHandle_t handle, string pszPreviewFile, EItemPreviewType type)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				result = NativeMethods.ISteamUGC_AddItemPreviewFile(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle, type);
			}
			return result;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000B188 File Offset: 0x00009388
		public static bool AddItemPreviewVideo(UGCUpdateHandle_t handle, string pszVideoID)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				result = NativeMethods.ISteamUGC_AddItemPreviewVideo(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000B1CC File Offset: 0x000093CC
		public static bool UpdateItemPreviewFile(UGCUpdateHandle_t handle, uint index, string pszPreviewFile)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszPreviewFile))
			{
				result = NativeMethods.ISteamUGC_UpdateItemPreviewFile(CSteamAPIContext.GetSteamUGC(), handle, index, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000B210 File Offset: 0x00009410
		public static bool UpdateItemPreviewVideo(UGCUpdateHandle_t handle, uint index, string pszVideoID)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszVideoID))
			{
				result = NativeMethods.ISteamUGC_UpdateItemPreviewVideo(CSteamAPIContext.GetSteamUGC(), handle, index, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000B254 File Offset: 0x00009454
		public static bool RemoveItemPreview(UGCUpdateHandle_t handle, uint index)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_RemoveItemPreview(CSteamAPIContext.GetSteamUGC(), handle, index);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000B267 File Offset: 0x00009467
		public static bool AddContentDescriptor(UGCUpdateHandle_t handle, EUGCContentDescriptorID descid)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_AddContentDescriptor(CSteamAPIContext.GetSteamUGC(), handle, descid);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000B27A File Offset: 0x0000947A
		public static bool RemoveContentDescriptor(UGCUpdateHandle_t handle, EUGCContentDescriptorID descid)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_RemoveContentDescriptor(CSteamAPIContext.GetSteamUGC(), handle, descid);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000B290 File Offset: 0x00009490
		public static bool SetRequiredGameVersions(UGCUpdateHandle_t handle, string pszGameBranchMin, string pszGameBranchMax)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszGameBranchMin))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pszGameBranchMax))
				{
					result = NativeMethods.ISteamUGC_SetRequiredGameVersions(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000B2F4 File Offset: 0x000094F4
		public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchChangeNote))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUGC_SubmitItemUpdate(CSteamAPIContext.GetSteamUGC(), handle, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000B33C File Offset: 0x0000953C
		public static EItemUpdateStatus GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetItemUpdateProgress(CSteamAPIContext.GetSteamUGC(), handle, out punBytesProcessed, out punBytesTotal);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000B350 File Offset: 0x00009550
		public static SteamAPICall_t SetUserItemVote(PublishedFileId_t nPublishedFileID, bool bVoteUp)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SetUserItemVote(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, bVoteUp);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000B368 File Offset: 0x00009568
		public static SteamAPICall_t GetUserItemVote(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetUserItemVote(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000B37F File Offset: 0x0000957F
		public static SteamAPICall_t AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddItemToFavorites(CSteamAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000B397 File Offset: 0x00009597
		public static SteamAPICall_t RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveItemFromFavorites(CSteamAPIContext.GetSteamUGC(), nAppId, nPublishedFileID);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000B3AF File Offset: 0x000095AF
		public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_SubscribeItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000B3C6 File Offset: 0x000095C6
		public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_UnsubscribeItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000B3DD File Offset: 0x000095DD
		public static uint GetNumSubscribedItems()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetNumSubscribedItems(CSteamAPIContext.GetSteamUGC());
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000B3EE File Offset: 0x000095EE
		public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetSubscribedItems(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, cMaxEntries);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000B401 File Offset: 0x00009601
		public static uint GetItemState(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetItemState(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000B414 File Offset: 0x00009614
		public static bool GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, out string pchFolder, uint cchFolderSize, out uint punTimeStamp)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchFolderSize);
			bool flag = NativeMethods.ISteamUGC_GetItemInstallInfo(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, out punSizeOnDisk, intPtr, cchFolderSize, out punTimeStamp);
			pchFolder = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000B453 File Offset: 0x00009653
		public static bool GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetItemDownloadInfo(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, out punBytesDownloaded, out punBytesTotal);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000B467 File Offset: 0x00009667
		public static bool DownloadItem(PublishedFileId_t nPublishedFileID, bool bHighPriority)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_DownloadItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, bHighPriority);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000B47C File Offset: 0x0000967C
		public static bool BInitWorkshopForGameServer(DepotId_t unWorkshopDepotID, string pszFolder)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszFolder))
			{
				result = NativeMethods.ISteamUGC_BInitWorkshopForGameServer(CSteamAPIContext.GetSteamUGC(), unWorkshopDepotID, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000B4C0 File Offset: 0x000096C0
		public static void SuspendDownloads(bool bSuspend)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUGC_SuspendDownloads(CSteamAPIContext.GetSteamUGC(), bSuspend);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000B4D2 File Offset: 0x000096D2
		public static SteamAPICall_t StartPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StartPlaytimeTracking(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000B4EA File Offset: 0x000096EA
		public static SteamAPICall_t StopPlaytimeTracking(PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTracking(CSteamAPIContext.GetSteamUGC(), pvecPublishedFileID, unNumPublishedFileIDs);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000B502 File Offset: 0x00009702
		public static SteamAPICall_t StopPlaytimeTrackingForAllItems()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_StopPlaytimeTrackingForAllItems(CSteamAPIContext.GetSteamUGC());
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000B518 File Offset: 0x00009718
		public static SteamAPICall_t AddDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddDependency(CSteamAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000B530 File Offset: 0x00009730
		public static SteamAPICall_t RemoveDependency(PublishedFileId_t nParentPublishedFileID, PublishedFileId_t nChildPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveDependency(CSteamAPIContext.GetSteamUGC(), nParentPublishedFileID, nChildPublishedFileID);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000B548 File Offset: 0x00009748
		public static SteamAPICall_t AddAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_AddAppDependency(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000B560 File Offset: 0x00009760
		public static SteamAPICall_t RemoveAppDependency(PublishedFileId_t nPublishedFileID, AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_RemoveAppDependency(CSteamAPIContext.GetSteamUGC(), nPublishedFileID, nAppID);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000B578 File Offset: 0x00009778
		public static SteamAPICall_t GetAppDependencies(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetAppDependencies(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000B58F File Offset: 0x0000978F
		public static SteamAPICall_t DeleteItem(PublishedFileId_t nPublishedFileID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_DeleteItem(CSteamAPIContext.GetSteamUGC(), nPublishedFileID);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000B5A6 File Offset: 0x000097A6
		public static bool ShowWorkshopEULA()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_ShowWorkshopEULA(CSteamAPIContext.GetSteamUGC());
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000B5B7 File Offset: 0x000097B7
		public static SteamAPICall_t GetWorkshopEULAStatus()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUGC_GetWorkshopEULAStatus(CSteamAPIContext.GetSteamUGC());
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000B5CD File Offset: 0x000097CD
		public static uint GetUserContentDescriptorPreferences(EUGCContentDescriptorID[] pvecDescriptors, uint cMaxEntries)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUGC_GetUserContentDescriptorPreferences(CSteamAPIContext.GetSteamUGC(), pvecDescriptors, cMaxEntries);
		}
	}
}
