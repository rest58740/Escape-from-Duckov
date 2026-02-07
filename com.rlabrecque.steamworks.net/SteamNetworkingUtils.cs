using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200001E RID: 30
	public static class SteamNetworkingUtils
	{
		// Token: 0x0600036E RID: 878 RVA: 0x000092BE File Offset: 0x000074BE
		public static IntPtr AllocateMessage(int cbAllocateBuffer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_AllocateMessage(CSteamAPIContext.GetSteamNetworkingUtils(), cbAllocateBuffer);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000092D0 File Offset: 0x000074D0
		public static void InitRelayNetworkAccess()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingUtils_InitRelayNetworkAccess(CSteamAPIContext.GetSteamNetworkingUtils());
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000092E1 File Offset: 0x000074E1
		public static ESteamNetworkingAvailability GetRelayNetworkStatus(out SteamRelayNetworkStatus_t pDetails)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_GetRelayNetworkStatus(CSteamAPIContext.GetSteamNetworkingUtils(), out pDetails);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000092F3 File Offset: 0x000074F3
		public static float GetLocalPingLocation(out SteamNetworkPingLocation_t result)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_GetLocalPingLocation(CSteamAPIContext.GetSteamNetworkingUtils(), out result);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00009305 File Offset: 0x00007505
		public static int EstimatePingTimeBetweenTwoLocations(ref SteamNetworkPingLocation_t location1, ref SteamNetworkPingLocation_t location2)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_EstimatePingTimeBetweenTwoLocations(CSteamAPIContext.GetSteamNetworkingUtils(), ref location1, ref location2);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00009318 File Offset: 0x00007518
		public static int EstimatePingTimeFromLocalHost(ref SteamNetworkPingLocation_t remoteLocation)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_EstimatePingTimeFromLocalHost(CSteamAPIContext.GetSteamNetworkingUtils(), ref remoteLocation);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000932C File Offset: 0x0000752C
		public static void ConvertPingLocationToString(ref SteamNetworkPingLocation_t location, out string pszBuf, int cchBufSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchBufSize);
			NativeMethods.ISteamNetworkingUtils_ConvertPingLocationToString(CSteamAPIContext.GetSteamNetworkingUtils(), ref location, intPtr, cchBufSize);
			pszBuf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00009360 File Offset: 0x00007560
		public static bool ParsePingLocationString(string pszString, out SteamNetworkPingLocation_t result)
		{
			InteropHelp.TestIfAvailableClient();
			bool result2;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszString))
			{
				result2 = NativeMethods.ISteamNetworkingUtils_ParsePingLocationString(CSteamAPIContext.GetSteamNetworkingUtils(), utf8StringHandle, out result);
			}
			return result2;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000093A4 File Offset: 0x000075A4
		public static bool CheckPingDataUpToDate(float flMaxAgeSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_CheckPingDataUpToDate(CSteamAPIContext.GetSteamNetworkingUtils(), flMaxAgeSeconds);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000093B6 File Offset: 0x000075B6
		public static int GetPingToDataCenter(SteamNetworkingPOPID popID, out SteamNetworkingPOPID pViaRelayPoP)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_GetPingToDataCenter(CSteamAPIContext.GetSteamNetworkingUtils(), popID, out pViaRelayPoP);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000093C9 File Offset: 0x000075C9
		public static int GetDirectPingToPOP(SteamNetworkingPOPID popID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_GetDirectPingToPOP(CSteamAPIContext.GetSteamNetworkingUtils(), popID);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000093DB File Offset: 0x000075DB
		public static int GetPOPCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_GetPOPCount(CSteamAPIContext.GetSteamNetworkingUtils());
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000093EC File Offset: 0x000075EC
		public static int GetPOPList(out SteamNetworkingPOPID list, int nListSz)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_GetPOPList(CSteamAPIContext.GetSteamNetworkingUtils(), out list, nListSz);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000093FF File Offset: 0x000075FF
		public static SteamNetworkingMicroseconds GetLocalTimestamp()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamNetworkingMicroseconds)NativeMethods.ISteamNetworkingUtils_GetLocalTimestamp(CSteamAPIContext.GetSteamNetworkingUtils());
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00009415 File Offset: 0x00007615
		public static void SetDebugOutputFunction(ESteamNetworkingSocketsDebugOutputType eDetailLevel, FSteamNetworkingSocketsDebugOutput pfnFunc)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingUtils_SetDebugOutputFunction(CSteamAPIContext.GetSteamNetworkingUtils(), eDetailLevel, pfnFunc);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00009428 File Offset: 0x00007628
		public static bool IsFakeIPv4(uint nIPv4)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_IsFakeIPv4(CSteamAPIContext.GetSteamNetworkingUtils(), nIPv4);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000943A File Offset: 0x0000763A
		public static ESteamNetworkingFakeIPType GetIPv4FakeIPType(uint nIPv4)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_GetIPv4FakeIPType(CSteamAPIContext.GetSteamNetworkingUtils(), nIPv4);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000944C File Offset: 0x0000764C
		public static EResult GetRealIdentityForFakeIP(ref SteamNetworkingIPAddr fakeIP, out SteamNetworkingIdentity pOutRealIdentity)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_GetRealIdentityForFakeIP(CSteamAPIContext.GetSteamNetworkingUtils(), ref fakeIP, out pOutRealIdentity);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000945F File Offset: 0x0000765F
		public static bool SetConfigValue(ESteamNetworkingConfigValue eValue, ESteamNetworkingConfigScope eScopeType, IntPtr scopeObj, ESteamNetworkingConfigDataType eDataType, IntPtr pArg)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_SetConfigValue(CSteamAPIContext.GetSteamNetworkingUtils(), eValue, eScopeType, scopeObj, eDataType, pArg);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00009476 File Offset: 0x00007676
		public static ESteamNetworkingGetConfigValueResult GetConfigValue(ESteamNetworkingConfigValue eValue, ESteamNetworkingConfigScope eScopeType, IntPtr scopeObj, out ESteamNetworkingConfigDataType pOutDataType, IntPtr pResult, ref ulong cbResult)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_GetConfigValue(CSteamAPIContext.GetSteamNetworkingUtils(), eValue, eScopeType, scopeObj, out pOutDataType, pResult, ref cbResult);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000948F File Offset: 0x0000768F
		public static string GetConfigValueInfo(ESteamNetworkingConfigValue eValue, out ESteamNetworkingConfigDataType pOutDataType, out ESteamNetworkingConfigScope pOutScope)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamNetworkingUtils_GetConfigValueInfo(CSteamAPIContext.GetSteamNetworkingUtils(), eValue, out pOutDataType, out pOutScope));
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000094A8 File Offset: 0x000076A8
		public static ESteamNetworkingConfigValue IterateGenericEditableConfigValues(ESteamNetworkingConfigValue eCurrent, bool bEnumerateDevVars)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_IterateGenericEditableConfigValues(CSteamAPIContext.GetSteamNetworkingUtils(), eCurrent, bEnumerateDevVars);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x000094BC File Offset: 0x000076BC
		public static void SteamNetworkingIPAddr_ToString(ref SteamNetworkingIPAddr addr, out string buf, uint cbBuf, bool bWithPort)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cbBuf);
			NativeMethods.ISteamNetworkingUtils_SteamNetworkingIPAddr_ToString(CSteamAPIContext.GetSteamNetworkingUtils(), ref addr, intPtr, cbBuf, bWithPort);
			buf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000094F4 File Offset: 0x000076F4
		public static bool SteamNetworkingIPAddr_ParseString(out SteamNetworkingIPAddr pAddr, string pszStr)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszStr))
			{
				result = NativeMethods.ISteamNetworkingUtils_SteamNetworkingIPAddr_ParseString(CSteamAPIContext.GetSteamNetworkingUtils(), out pAddr, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00009538 File Offset: 0x00007738
		public static ESteamNetworkingFakeIPType SteamNetworkingIPAddr_GetFakeIPType(ref SteamNetworkingIPAddr addr)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingUtils_SteamNetworkingIPAddr_GetFakeIPType(CSteamAPIContext.GetSteamNetworkingUtils(), ref addr);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000954C File Offset: 0x0000774C
		public static void SteamNetworkingIdentity_ToString(ref SteamNetworkingIdentity identity, out string buf, uint cbBuf)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cbBuf);
			NativeMethods.ISteamNetworkingUtils_SteamNetworkingIdentity_ToString(CSteamAPIContext.GetSteamNetworkingUtils(), ref identity, intPtr, cbBuf);
			buf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00009580 File Offset: 0x00007780
		public static bool SteamNetworkingIdentity_ParseString(out SteamNetworkingIdentity pIdentity, string pszStr)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszStr))
			{
				result = NativeMethods.ISteamNetworkingUtils_SteamNetworkingIdentity_ParseString(CSteamAPIContext.GetSteamNetworkingUtils(), out pIdentity, utf8StringHandle);
			}
			return result;
		}
	}
}
