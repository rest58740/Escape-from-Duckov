using System;

namespace Steamworks
{
	// Token: 0x0200000E RID: 14
	public static class SteamGameServerStats
	{
		// Token: 0x06000188 RID: 392 RVA: 0x000055AC File Offset: 0x000037AC
		public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerStats_RequestUserStats(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000055C4 File Offset: 0x000037C4
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_GetUserStatInt32(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00005608 File Offset: 0x00003808
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_GetUserStatFloat(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000564C File Offset: 0x0000384C
		public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_GetUserAchievement(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, out pbAchieved);
			}
			return result;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00005690 File Offset: 0x00003890
		public static bool SetUserStat(CSteamID steamIDUser, string pchName, int nData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_SetUserStatInt32(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, nData);
			}
			return result;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000056D4 File Offset: 0x000038D4
		public static bool SetUserStat(CSteamID steamIDUser, string pchName, float fData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_SetUserStatFloat(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, fData);
			}
			return result;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00005718 File Offset: 0x00003918
		public static bool UpdateUserAvgRateStat(CSteamID steamIDUser, string pchName, float flCountThisSession, double dSessionLength)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_UpdateUserAvgRateStat(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, flCountThisSession, dSessionLength);
			}
			return result;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00005760 File Offset: 0x00003960
		public static bool SetUserAchievement(CSteamID steamIDUser, string pchName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_SetUserAchievement(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000057A4 File Offset: 0x000039A4
		public static bool ClearUserAchievement(CSteamID steamIDUser, string pchName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_ClearUserAchievement(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000057E8 File Offset: 0x000039E8
		public static SteamAPICall_t StoreUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerStats_StoreUserStats(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser);
		}
	}
}
