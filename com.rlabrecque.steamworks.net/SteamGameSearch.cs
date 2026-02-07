using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000017 RID: 23
	public static class SteamGameSearch
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x00008460 File Offset: 0x00006660
		public static EGameSearchErrorCode_t AddGameSearchParams(string pchKeyToFind, string pchValuesToFind)
		{
			InteropHelp.TestIfAvailableClient();
			EGameSearchErrorCode_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKeyToFind))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValuesToFind))
				{
					result = NativeMethods.ISteamGameSearch_AddGameSearchParams(CSteamAPIContext.GetSteamGameSearch(), utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x000084C0 File Offset: 0x000066C0
		public static EGameSearchErrorCode_t SearchForGameWithLobby(CSteamID steamIDLobby, int nPlayerMin, int nPlayerMax)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_SearchForGameWithLobby(CSteamAPIContext.GetSteamGameSearch(), steamIDLobby, nPlayerMin, nPlayerMax);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x000084D4 File Offset: 0x000066D4
		public static EGameSearchErrorCode_t SearchForGameSolo(int nPlayerMin, int nPlayerMax)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_SearchForGameSolo(CSteamAPIContext.GetSteamGameSearch(), nPlayerMin, nPlayerMax);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000084E7 File Offset: 0x000066E7
		public static EGameSearchErrorCode_t AcceptGame()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_AcceptGame(CSteamAPIContext.GetSteamGameSearch());
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000084F8 File Offset: 0x000066F8
		public static EGameSearchErrorCode_t DeclineGame()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_DeclineGame(CSteamAPIContext.GetSteamGameSearch());
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000850C File Offset: 0x0000670C
		public static EGameSearchErrorCode_t RetrieveConnectionDetails(CSteamID steamIDHost, out string pchConnectionDetails, int cubConnectionDetails)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubConnectionDetails);
			EGameSearchErrorCode_t egameSearchErrorCode_t = NativeMethods.ISteamGameSearch_RetrieveConnectionDetails(CSteamAPIContext.GetSteamGameSearch(), steamIDHost, intPtr, cubConnectionDetails);
			pchConnectionDetails = ((egameSearchErrorCode_t != (EGameSearchErrorCode_t)0) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return egameSearchErrorCode_t;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00008548 File Offset: 0x00006748
		public static EGameSearchErrorCode_t EndGameSearch()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_EndGameSearch(CSteamAPIContext.GetSteamGameSearch());
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000855C File Offset: 0x0000675C
		public static EGameSearchErrorCode_t SetGameHostParams(string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			EGameSearchErrorCode_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					result = NativeMethods.ISteamGameSearch_SetGameHostParams(CSteamAPIContext.GetSteamGameSearch(), utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000085BC File Offset: 0x000067BC
		public static EGameSearchErrorCode_t SetConnectionDetails(string pchConnectionDetails, int cubConnectionDetails)
		{
			InteropHelp.TestIfAvailableClient();
			EGameSearchErrorCode_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchConnectionDetails))
			{
				result = NativeMethods.ISteamGameSearch_SetConnectionDetails(CSteamAPIContext.GetSteamGameSearch(), utf8StringHandle, cubConnectionDetails);
			}
			return result;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00008600 File Offset: 0x00006800
		public static EGameSearchErrorCode_t RequestPlayersForGame(int nPlayerMin, int nPlayerMax, int nMaxTeamSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_RequestPlayersForGame(CSteamAPIContext.GetSteamGameSearch(), nPlayerMin, nPlayerMax, nMaxTeamSize);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00008614 File Offset: 0x00006814
		public static EGameSearchErrorCode_t HostConfirmGameStart(ulong ullUniqueGameID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_HostConfirmGameStart(CSteamAPIContext.GetSteamGameSearch(), ullUniqueGameID);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00008626 File Offset: 0x00006826
		public static EGameSearchErrorCode_t CancelRequestPlayersForGame()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_CancelRequestPlayersForGame(CSteamAPIContext.GetSteamGameSearch());
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00008637 File Offset: 0x00006837
		public static EGameSearchErrorCode_t SubmitPlayerResult(ulong ullUniqueGameID, CSteamID steamIDPlayer, EPlayerResult_t EPlayerResult)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_SubmitPlayerResult(CSteamAPIContext.GetSteamGameSearch(), ullUniqueGameID, steamIDPlayer, EPlayerResult);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000864B File Offset: 0x0000684B
		public static EGameSearchErrorCode_t EndGame(ulong ullUniqueGameID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_EndGame(CSteamAPIContext.GetSteamGameSearch(), ullUniqueGameID);
		}
	}
}
