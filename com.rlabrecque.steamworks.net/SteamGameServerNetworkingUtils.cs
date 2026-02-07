using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200000D RID: 13
	public static class SteamGameServerNetworkingUtils
	{
		// Token: 0x0600016D RID: 365 RVA: 0x000052A6 File Offset: 0x000034A6
		public static IntPtr AllocateMessage(int cbAllocateBuffer)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_AllocateMessage(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), cbAllocateBuffer);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000052B8 File Offset: 0x000034B8
		public static void InitRelayNetworkAccess()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamNetworkingUtils_InitRelayNetworkAccess(CSteamGameServerAPIContext.GetSteamNetworkingUtils());
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000052C9 File Offset: 0x000034C9
		public static ESteamNetworkingAvailability GetRelayNetworkStatus(out SteamRelayNetworkStatus_t pDetails)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_GetRelayNetworkStatus(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), out pDetails);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000052DB File Offset: 0x000034DB
		public static float GetLocalPingLocation(out SteamNetworkPingLocation_t result)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_GetLocalPingLocation(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), out result);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000052ED File Offset: 0x000034ED
		public static int EstimatePingTimeBetweenTwoLocations(ref SteamNetworkPingLocation_t location1, ref SteamNetworkPingLocation_t location2)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_EstimatePingTimeBetweenTwoLocations(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref location1, ref location2);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00005300 File Offset: 0x00003500
		public static int EstimatePingTimeFromLocalHost(ref SteamNetworkPingLocation_t remoteLocation)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_EstimatePingTimeFromLocalHost(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref remoteLocation);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00005314 File Offset: 0x00003514
		public static void ConvertPingLocationToString(ref SteamNetworkPingLocation_t location, out string pszBuf, int cchBufSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal(cchBufSize);
			NativeMethods.ISteamNetworkingUtils_ConvertPingLocationToString(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref location, intPtr, cchBufSize);
			pszBuf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005348 File Offset: 0x00003548
		public static bool ParsePingLocationString(string pszString, out SteamNetworkPingLocation_t result)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result2;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszString))
			{
				result2 = NativeMethods.ISteamNetworkingUtils_ParsePingLocationString(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), utf8StringHandle, out result);
			}
			return result2;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000538C File Offset: 0x0000358C
		public static bool CheckPingDataUpToDate(float flMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_CheckPingDataUpToDate(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), flMaxAgeSeconds);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000539E File Offset: 0x0000359E
		public static int GetPingToDataCenter(SteamNetworkingPOPID popID, out SteamNetworkingPOPID pViaRelayPoP)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_GetPingToDataCenter(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), popID, out pViaRelayPoP);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000053B1 File Offset: 0x000035B1
		public static int GetDirectPingToPOP(SteamNetworkingPOPID popID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_GetDirectPingToPOP(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), popID);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000053C3 File Offset: 0x000035C3
		public static int GetPOPCount()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_GetPOPCount(CSteamGameServerAPIContext.GetSteamNetworkingUtils());
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000053D4 File Offset: 0x000035D4
		public static int GetPOPList(out SteamNetworkingPOPID list, int nListSz)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_GetPOPList(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), out list, nListSz);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000053E7 File Offset: 0x000035E7
		public static SteamNetworkingMicroseconds GetLocalTimestamp()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamNetworkingMicroseconds)NativeMethods.ISteamNetworkingUtils_GetLocalTimestamp(CSteamGameServerAPIContext.GetSteamNetworkingUtils());
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000053FD File Offset: 0x000035FD
		public static void SetDebugOutputFunction(ESteamNetworkingSocketsDebugOutputType eDetailLevel, FSteamNetworkingSocketsDebugOutput pfnFunc)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamNetworkingUtils_SetDebugOutputFunction(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), eDetailLevel, pfnFunc);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00005410 File Offset: 0x00003610
		public static bool IsFakeIPv4(uint nIPv4)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_IsFakeIPv4(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), nIPv4);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00005422 File Offset: 0x00003622
		public static ESteamNetworkingFakeIPType GetIPv4FakeIPType(uint nIPv4)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_GetIPv4FakeIPType(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), nIPv4);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00005434 File Offset: 0x00003634
		public static EResult GetRealIdentityForFakeIP(ref SteamNetworkingIPAddr fakeIP, out SteamNetworkingIdentity pOutRealIdentity)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_GetRealIdentityForFakeIP(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref fakeIP, out pOutRealIdentity);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00005447 File Offset: 0x00003647
		public static bool SetConfigValue(ESteamNetworkingConfigValue eValue, ESteamNetworkingConfigScope eScopeType, IntPtr scopeObj, ESteamNetworkingConfigDataType eDataType, IntPtr pArg)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_SetConfigValue(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), eValue, eScopeType, scopeObj, eDataType, pArg);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000545E File Offset: 0x0000365E
		public static ESteamNetworkingGetConfigValueResult GetConfigValue(ESteamNetworkingConfigValue eValue, ESteamNetworkingConfigScope eScopeType, IntPtr scopeObj, out ESteamNetworkingConfigDataType pOutDataType, IntPtr pResult, ref ulong cbResult)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_GetConfigValue(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), eValue, eScopeType, scopeObj, out pOutDataType, pResult, ref cbResult);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00005477 File Offset: 0x00003677
		public static string GetConfigValueInfo(ESteamNetworkingConfigValue eValue, out ESteamNetworkingConfigDataType pOutDataType, out ESteamNetworkingConfigScope pOutScope)
		{
			InteropHelp.TestIfAvailableGameServer();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamNetworkingUtils_GetConfigValueInfo(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), eValue, out pOutDataType, out pOutScope));
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00005490 File Offset: 0x00003690
		public static ESteamNetworkingConfigValue IterateGenericEditableConfigValues(ESteamNetworkingConfigValue eCurrent, bool bEnumerateDevVars)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_IterateGenericEditableConfigValues(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), eCurrent, bEnumerateDevVars);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000054A4 File Offset: 0x000036A4
		public static void SteamNetworkingIPAddr_ToString(ref SteamNetworkingIPAddr addr, out string buf, uint cbBuf, bool bWithPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cbBuf);
			NativeMethods.ISteamNetworkingUtils_SteamNetworkingIPAddr_ToString(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref addr, intPtr, cbBuf, bWithPort);
			buf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000054DC File Offset: 0x000036DC
		public static bool SteamNetworkingIPAddr_ParseString(out SteamNetworkingIPAddr pAddr, string pszStr)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszStr))
			{
				result = NativeMethods.ISteamNetworkingUtils_SteamNetworkingIPAddr_ParseString(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), out pAddr, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00005520 File Offset: 0x00003720
		public static ESteamNetworkingFakeIPType SteamNetworkingIPAddr_GetFakeIPType(ref SteamNetworkingIPAddr addr)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingUtils_SteamNetworkingIPAddr_GetFakeIPType(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref addr);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00005534 File Offset: 0x00003734
		public static void SteamNetworkingIdentity_ToString(ref SteamNetworkingIdentity identity, out string buf, uint cbBuf)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cbBuf);
			NativeMethods.ISteamNetworkingUtils_SteamNetworkingIdentity_ToString(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref identity, intPtr, cbBuf);
			buf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00005568 File Offset: 0x00003768
		public static bool SteamNetworkingIdentity_ParseString(out SteamNetworkingIdentity pIdentity, string pszStr)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszStr))
			{
				result = NativeMethods.ISteamNetworkingUtils_SteamNetworkingIdentity_ParseString(CSteamGameServerAPIContext.GetSteamNetworkingUtils(), out pIdentity, utf8StringHandle);
			}
			return result;
		}
	}
}
