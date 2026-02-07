using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000025 RID: 37
	public static class SteamUser
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x0000B5E0 File Offset: 0x000097E0
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamUser_GetHSteamUser(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000B5F6 File Offset: 0x000097F6
		public static bool BLoggedOn()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BLoggedOn(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000B607 File Offset: 0x00009807
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamUser_GetSteamID(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000B61D File Offset: 0x0000981D
		public static int InitiateGameConnection_DEPRECATED(byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, bool bSecure)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_InitiateGameConnection_DEPRECATED(CSteamAPIContext.GetSteamUser(), pAuthBlob, cbMaxAuthBlob, steamIDGameServer, unIPServer, usPortServer, bSecure);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000B636 File Offset: 0x00009836
		public static void TerminateGameConnection_DEPRECATED(uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_TerminateGameConnection_DEPRECATED(CSteamAPIContext.GetSteamUser(), unIPServer, usPortServer);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000B64C File Offset: 0x0000984C
		public static void TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, string pchExtraInfo = "")
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchExtraInfo))
			{
				NativeMethods.ISteamUser_TrackAppUsageEvent(CSteamAPIContext.GetSteamUser(), gameID, eAppUsageEvent, utf8StringHandle);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000B690 File Offset: 0x00009890
		public static bool GetUserDataFolder(out string pchBuffer, int cubBuffer)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubBuffer);
			bool flag = NativeMethods.ISteamUser_GetUserDataFolder(CSteamAPIContext.GetSteamUser(), intPtr, cubBuffer);
			pchBuffer = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000B6CB File Offset: 0x000098CB
		public static void StartVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StartVoiceRecording(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000B6DC File Offset: 0x000098DC
		public static void StopVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StopVoiceRecording(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000B6ED File Offset: 0x000098ED
		public static EVoiceResult GetAvailableVoice(out uint pcbCompressed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetAvailableVoice(CSteamAPIContext.GetSteamUser(), out pcbCompressed, IntPtr.Zero, 0U);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000B708 File Offset: 0x00009908
		public static EVoiceResult GetVoice(bool bWantCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoice(CSteamAPIContext.GetSteamUser(), bWantCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, false, IntPtr.Zero, 0U, IntPtr.Zero, 0U);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000B735 File Offset: 0x00009935
		public static EVoiceResult DecompressVoice(byte[] pCompressed, uint cbCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_DecompressVoice(CSteamAPIContext.GetSteamUser(), pCompressed, cbCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, nDesiredSampleRate);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000B74E File Offset: 0x0000994E
		public static uint GetVoiceOptimalSampleRate()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoiceOptimalSampleRate(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000B75F File Offset: 0x0000995F
		public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket, ref SteamNetworkingIdentity pSteamNetworkingIdentity)
		{
			InteropHelp.TestIfAvailableClient();
			return (HAuthTicket)NativeMethods.ISteamUser_GetAuthSessionTicket(CSteamAPIContext.GetSteamUser(), pTicket, cbMaxTicket, out pcbTicket, ref pSteamNetworkingIdentity);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000B77C File Offset: 0x0000997C
		public static HAuthTicket GetAuthTicketForWebApi(string pchIdentity)
		{
			InteropHelp.TestIfAvailableClient();
			HAuthTicket result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchIdentity))
			{
				result = (HAuthTicket)NativeMethods.ISteamUser_GetAuthTicketForWebApi(CSteamAPIContext.GetSteamUser(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000B7C4 File Offset: 0x000099C4
		public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BeginAuthSession(CSteamAPIContext.GetSteamUser(), pAuthTicket, cbAuthTicket, steamID);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000B7D8 File Offset: 0x000099D8
		public static void EndAuthSession(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_EndAuthSession(CSteamAPIContext.GetSteamUser(), steamID);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000B7EA File Offset: 0x000099EA
		public static void CancelAuthTicket(HAuthTicket hAuthTicket)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_CancelAuthTicket(CSteamAPIContext.GetSteamUser(), hAuthTicket);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000B7FC File Offset: 0x000099FC
		public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_UserHasLicenseForApp(CSteamAPIContext.GetSteamUser(), steamID, appID);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000B80F File Offset: 0x00009A0F
		public static bool BIsBehindNAT()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsBehindNAT(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000B820 File Offset: 0x00009A20
		public static void AdvertiseGame(CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_AdvertiseGame(CSteamAPIContext.GetSteamUser(), steamIDGameServer, unIPServer, usPortServer);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000B834 File Offset: 0x00009A34
		public static SteamAPICall_t RequestEncryptedAppTicket(byte[] pDataToInclude, int cbDataToInclude)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUser_RequestEncryptedAppTicket(CSteamAPIContext.GetSteamUser(), pDataToInclude, cbDataToInclude);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000B84C File Offset: 0x00009A4C
		public static bool GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetEncryptedAppTicket(CSteamAPIContext.GetSteamUser(), pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000B860 File Offset: 0x00009A60
		public static int GetGameBadgeLevel(int nSeries, bool bFoil)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetGameBadgeLevel(CSteamAPIContext.GetSteamUser(), nSeries, bFoil);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000B873 File Offset: 0x00009A73
		public static int GetPlayerSteamLevel()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetPlayerSteamLevel(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000B884 File Offset: 0x00009A84
		public static SteamAPICall_t RequestStoreAuthURL(string pchRedirectURL)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchRedirectURL))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUser_RequestStoreAuthURL(CSteamAPIContext.GetSteamUser(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000B8CC File Offset: 0x00009ACC
		public static bool BIsPhoneVerified()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneVerified(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000B8DD File Offset: 0x00009ADD
		public static bool BIsTwoFactorEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsTwoFactorEnabled(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000B8EE File Offset: 0x00009AEE
		public static bool BIsPhoneIdentifying()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneIdentifying(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000B8FF File Offset: 0x00009AFF
		public static bool BIsPhoneRequiringVerification()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneRequiringVerification(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000B910 File Offset: 0x00009B10
		public static SteamAPICall_t GetMarketEligibility()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUser_GetMarketEligibility(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000B926 File Offset: 0x00009B26
		public static SteamAPICall_t GetDurationControl()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUser_GetDurationControl(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000B93C File Offset: 0x00009B3C
		public static bool BSetDurationControlOnlineState(EDurationControlOnlineState eNewState)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BSetDurationControlOnlineState(CSteamAPIContext.GetSteamUser(), eNewState);
		}
	}
}
