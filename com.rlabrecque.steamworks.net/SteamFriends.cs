using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000005 RID: 5
	public static class SteamFriends
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00002C5C File Offset: 0x00000E5C
		public static string GetPersonaName()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPersonaName(CSteamAPIContext.GetSteamFriends()));
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002C74 File Offset: 0x00000E74
		public static SteamAPICall_t SetPersonaName(string pchPersonaName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPersonaName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamFriends_SetPersonaName(CSteamAPIContext.GetSteamFriends(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002CBC File Offset: 0x00000EBC
		public static EPersonaState GetPersonaState()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetPersonaState(CSteamAPIContext.GetSteamFriends());
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002CCD File Offset: 0x00000ECD
		public static int GetFriendCount(EFriendFlags iFriendFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendCount(CSteamAPIContext.GetSteamFriends(), iFriendFlags);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002CDF File Offset: 0x00000EDF
		public static CSteamID GetFriendByIndex(int iFriend, EFriendFlags iFriendFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetFriendByIndex(CSteamAPIContext.GetSteamFriends(), iFriend, iFriendFlags);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002CF7 File Offset: 0x00000EF7
		public static EFriendRelationship GetFriendRelationship(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendRelationship(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002D09 File Offset: 0x00000F09
		public static EPersonaState GetFriendPersonaState(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendPersonaState(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002D1B File Offset: 0x00000F1B
		public static string GetFriendPersonaName(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaName(CSteamAPIContext.GetSteamFriends(), steamIDFriend));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002D32 File Offset: 0x00000F32
		public static bool GetFriendGamePlayed(CSteamID steamIDFriend, out FriendGameInfo_t pFriendGameInfo)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendGamePlayed(CSteamAPIContext.GetSteamFriends(), steamIDFriend, out pFriendGameInfo);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002D45 File Offset: 0x00000F45
		public static string GetFriendPersonaNameHistory(CSteamID steamIDFriend, int iPersonaName)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaNameHistory(CSteamAPIContext.GetSteamFriends(), steamIDFriend, iPersonaName));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002D5D File Offset: 0x00000F5D
		public static int GetFriendSteamLevel(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendSteamLevel(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002D6F File Offset: 0x00000F6F
		public static string GetPlayerNickname(CSteamID steamIDPlayer)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPlayerNickname(CSteamAPIContext.GetSteamFriends(), steamIDPlayer));
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002D86 File Offset: 0x00000F86
		public static int GetFriendsGroupCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendsGroupCount(CSteamAPIContext.GetSteamFriends());
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002D97 File Offset: 0x00000F97
		public static FriendsGroupID_t GetFriendsGroupIDByIndex(int iFG)
		{
			InteropHelp.TestIfAvailableClient();
			return (FriendsGroupID_t)NativeMethods.ISteamFriends_GetFriendsGroupIDByIndex(CSteamAPIContext.GetSteamFriends(), iFG);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002DAE File Offset: 0x00000FAE
		public static string GetFriendsGroupName(FriendsGroupID_t friendsGroupID)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendsGroupName(CSteamAPIContext.GetSteamFriends(), friendsGroupID));
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002DC5 File Offset: 0x00000FC5
		public static int GetFriendsGroupMembersCount(FriendsGroupID_t friendsGroupID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendsGroupMembersCount(CSteamAPIContext.GetSteamFriends(), friendsGroupID);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002DD7 File Offset: 0x00000FD7
		public static void GetFriendsGroupMembersList(FriendsGroupID_t friendsGroupID, CSteamID[] pOutSteamIDMembers, int nMembersCount)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_GetFriendsGroupMembersList(CSteamAPIContext.GetSteamFriends(), friendsGroupID, pOutSteamIDMembers, nMembersCount);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002DEB File Offset: 0x00000FEB
		public static bool HasFriend(CSteamID steamIDFriend, EFriendFlags iFriendFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_HasFriend(CSteamAPIContext.GetSteamFriends(), steamIDFriend, iFriendFlags);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002DFE File Offset: 0x00000FFE
		public static int GetClanCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanCount(CSteamAPIContext.GetSteamFriends());
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E0F File Offset: 0x0000100F
		public static CSteamID GetClanByIndex(int iClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetClanByIndex(CSteamAPIContext.GetSteamFriends(), iClan);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002E26 File Offset: 0x00001026
		public static string GetClanName(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanName(CSteamAPIContext.GetSteamFriends(), steamIDClan));
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002E3D File Offset: 0x0000103D
		public static string GetClanTag(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanTag(CSteamAPIContext.GetSteamFriends(), steamIDClan));
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002E54 File Offset: 0x00001054
		public static bool GetClanActivityCounts(CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanActivityCounts(CSteamAPIContext.GetSteamFriends(), steamIDClan, out pnOnline, out pnInGame, out pnChatting);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002E69 File Offset: 0x00001069
		public static SteamAPICall_t DownloadClanActivityCounts(CSteamID[] psteamIDClans, int cClansToRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_DownloadClanActivityCounts(CSteamAPIContext.GetSteamFriends(), psteamIDClans, cClansToRequest);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002E81 File Offset: 0x00001081
		public static int GetFriendCountFromSource(CSteamID steamIDSource)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendCountFromSource(CSteamAPIContext.GetSteamFriends(), steamIDSource);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002E93 File Offset: 0x00001093
		public static CSteamID GetFriendFromSourceByIndex(CSteamID steamIDSource, int iFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetFriendFromSourceByIndex(CSteamAPIContext.GetSteamFriends(), steamIDSource, iFriend);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002EAB File Offset: 0x000010AB
		public static bool IsUserInSource(CSteamID steamIDUser, CSteamID steamIDSource)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsUserInSource(CSteamAPIContext.GetSteamFriends(), steamIDUser, steamIDSource);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002EBE File Offset: 0x000010BE
		public static void SetInGameVoiceSpeaking(CSteamID steamIDUser, bool bSpeaking)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_SetInGameVoiceSpeaking(CSteamAPIContext.GetSteamFriends(), steamIDUser, bSpeaking);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002ED4 File Offset: 0x000010D4
		public static void ActivateGameOverlay(string pchDialog)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDialog))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlay(CSteamAPIContext.GetSteamFriends(), utf8StringHandle);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002F14 File Offset: 0x00001114
		public static void ActivateGameOverlayToUser(string pchDialog, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDialog))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlayToUser(CSteamAPIContext.GetSteamFriends(), utf8StringHandle, steamID);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002F58 File Offset: 0x00001158
		public static void ActivateGameOverlayToWebPage(string pchURL, EActivateGameOverlayToWebPageMode eMode = EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchURL))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlayToWebPage(CSteamAPIContext.GetSteamFriends(), utf8StringHandle, eMode);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002F9C File Offset: 0x0000119C
		public static void ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ActivateGameOverlayToStore(CSteamAPIContext.GetSteamFriends(), nAppID, eFlag);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002FAF File Offset: 0x000011AF
		public static void SetPlayedWith(CSteamID steamIDUserPlayedWith)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_SetPlayedWith(CSteamAPIContext.GetSteamFriends(), steamIDUserPlayedWith);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002FC1 File Offset: 0x000011C1
		public static void ActivateGameOverlayInviteDialog(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ActivateGameOverlayInviteDialog(CSteamAPIContext.GetSteamFriends(), steamIDLobby);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002FD3 File Offset: 0x000011D3
		public static int GetSmallFriendAvatar(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetSmallFriendAvatar(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002FE5 File Offset: 0x000011E5
		public static int GetMediumFriendAvatar(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetMediumFriendAvatar(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002FF7 File Offset: 0x000011F7
		public static int GetLargeFriendAvatar(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetLargeFriendAvatar(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003009 File Offset: 0x00001209
		public static bool RequestUserInformation(CSteamID steamIDUser, bool bRequireNameOnly)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_RequestUserInformation(CSteamAPIContext.GetSteamFriends(), steamIDUser, bRequireNameOnly);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000301C File Offset: 0x0000121C
		public static SteamAPICall_t RequestClanOfficerList(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_RequestClanOfficerList(CSteamAPIContext.GetSteamFriends(), steamIDClan);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003033 File Offset: 0x00001233
		public static CSteamID GetClanOwner(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetClanOwner(CSteamAPIContext.GetSteamFriends(), steamIDClan);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000304A File Offset: 0x0000124A
		public static int GetClanOfficerCount(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanOfficerCount(CSteamAPIContext.GetSteamFriends(), steamIDClan);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000305C File Offset: 0x0000125C
		public static CSteamID GetClanOfficerByIndex(CSteamID steamIDClan, int iOfficer)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetClanOfficerByIndex(CSteamAPIContext.GetSteamFriends(), steamIDClan, iOfficer);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003074 File Offset: 0x00001274
		public static uint GetUserRestrictions()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetUserRestrictions(CSteamAPIContext.GetSteamFriends());
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003088 File Offset: 0x00001288
		public static bool SetRichPresence(string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					result = NativeMethods.ISteamFriends_SetRichPresence(CSteamAPIContext.GetSteamFriends(), utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000030E8 File Offset: 0x000012E8
		public static void ClearRichPresence()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ClearRichPresence(CSteamAPIContext.GetSteamFriends());
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000030FC File Offset: 0x000012FC
		public static string GetFriendRichPresence(CSteamID steamIDFriend, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresence(CSteamAPIContext.GetSteamFriends(), steamIDFriend, utf8StringHandle));
			}
			return result;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003144 File Offset: 0x00001344
		public static int GetFriendRichPresenceKeyCount(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendRichPresenceKeyCount(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003156 File Offset: 0x00001356
		public static string GetFriendRichPresenceKeyByIndex(CSteamID steamIDFriend, int iKey)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresenceKeyByIndex(CSteamAPIContext.GetSteamFriends(), steamIDFriend, iKey));
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000316E File Offset: 0x0000136E
		public static void RequestFriendRichPresence(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_RequestFriendRichPresence(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003180 File Offset: 0x00001380
		public static bool InviteUserToGame(CSteamID steamIDFriend, string pchConnectString)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchConnectString))
			{
				result = NativeMethods.ISteamFriends_InviteUserToGame(CSteamAPIContext.GetSteamFriends(), steamIDFriend, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000031C4 File Offset: 0x000013C4
		public static int GetCoplayFriendCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetCoplayFriendCount(CSteamAPIContext.GetSteamFriends());
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000031D5 File Offset: 0x000013D5
		public static CSteamID GetCoplayFriend(int iCoplayFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetCoplayFriend(CSteamAPIContext.GetSteamFriends(), iCoplayFriend);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000031EC File Offset: 0x000013EC
		public static int GetFriendCoplayTime(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendCoplayTime(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000031FE File Offset: 0x000013FE
		public static AppId_t GetFriendCoplayGame(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return (AppId_t)NativeMethods.ISteamFriends_GetFriendCoplayGame(CSteamAPIContext.GetSteamFriends(), steamIDFriend);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003215 File Offset: 0x00001415
		public static SteamAPICall_t JoinClanChatRoom(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_JoinClanChatRoom(CSteamAPIContext.GetSteamFriends(), steamIDClan);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000322C File Offset: 0x0000142C
		public static bool LeaveClanChatRoom(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_LeaveClanChatRoom(CSteamAPIContext.GetSteamFriends(), steamIDClan);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000323E File Offset: 0x0000143E
		public static int GetClanChatMemberCount(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanChatMemberCount(CSteamAPIContext.GetSteamFriends(), steamIDClan);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003250 File Offset: 0x00001450
		public static CSteamID GetChatMemberByIndex(CSteamID steamIDClan, int iUser)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetChatMemberByIndex(CSteamAPIContext.GetSteamFriends(), steamIDClan, iUser);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003268 File Offset: 0x00001468
		public static bool SendClanChatMessage(CSteamID steamIDClanChat, string pchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchText))
			{
				result = NativeMethods.ISteamFriends_SendClanChatMessage(CSteamAPIContext.GetSteamFriends(), steamIDClanChat, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000032AC File Offset: 0x000014AC
		public static int GetClanChatMessage(CSteamID steamIDClanChat, int iMessage, out string prgchText, int cchTextMax, out EChatEntryType peChatEntryType, out CSteamID psteamidChatter)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchTextMax);
			int num = NativeMethods.ISteamFriends_GetClanChatMessage(CSteamAPIContext.GetSteamFriends(), steamIDClanChat, iMessage, intPtr, cchTextMax, out peChatEntryType, out psteamidChatter);
			prgchText = ((num != 0) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000032ED File Offset: 0x000014ED
		public static bool IsClanChatAdmin(CSteamID steamIDClanChat, CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsClanChatAdmin(CSteamAPIContext.GetSteamFriends(), steamIDClanChat, steamIDUser);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003300 File Offset: 0x00001500
		public static bool IsClanChatWindowOpenInSteam(CSteamID steamIDClanChat)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsClanChatWindowOpenInSteam(CSteamAPIContext.GetSteamFriends(), steamIDClanChat);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003312 File Offset: 0x00001512
		public static bool OpenClanChatWindowInSteam(CSteamID steamIDClanChat)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_OpenClanChatWindowInSteam(CSteamAPIContext.GetSteamFriends(), steamIDClanChat);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003324 File Offset: 0x00001524
		public static bool CloseClanChatWindowInSteam(CSteamID steamIDClanChat)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_CloseClanChatWindowInSteam(CSteamAPIContext.GetSteamFriends(), steamIDClanChat);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003336 File Offset: 0x00001536
		public static bool SetListenForFriendsMessages(bool bInterceptEnabled)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_SetListenForFriendsMessages(CSteamAPIContext.GetSteamFriends(), bInterceptEnabled);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003348 File Offset: 0x00001548
		public static bool ReplyToFriendMessage(CSteamID steamIDFriend, string pchMsgToSend)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMsgToSend))
			{
				result = NativeMethods.ISteamFriends_ReplyToFriendMessage(CSteamAPIContext.GetSteamFriends(), steamIDFriend, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000338C File Offset: 0x0000158C
		public static int GetFriendMessage(CSteamID steamIDFriend, int iMessageID, out string pvData, int cubData, out EChatEntryType peChatEntryType)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubData);
			int num = NativeMethods.ISteamFriends_GetFriendMessage(CSteamAPIContext.GetSteamFriends(), steamIDFriend, iMessageID, intPtr, cubData, out peChatEntryType);
			pvData = ((num != 0) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000033CB File Offset: 0x000015CB
		public static SteamAPICall_t GetFollowerCount(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_GetFollowerCount(CSteamAPIContext.GetSteamFriends(), steamID);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000033E2 File Offset: 0x000015E2
		public static SteamAPICall_t IsFollowing(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_IsFollowing(CSteamAPIContext.GetSteamFriends(), steamID);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000033F9 File Offset: 0x000015F9
		public static SteamAPICall_t EnumerateFollowingList(uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_EnumerateFollowingList(CSteamAPIContext.GetSteamFriends(), unStartIndex);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003410 File Offset: 0x00001610
		public static bool IsClanPublic(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsClanPublic(CSteamAPIContext.GetSteamFriends(), steamIDClan);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003422 File Offset: 0x00001622
		public static bool IsClanOfficialGameGroup(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsClanOfficialGameGroup(CSteamAPIContext.GetSteamFriends(), steamIDClan);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003434 File Offset: 0x00001634
		public static int GetNumChatsWithUnreadPriorityMessages()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetNumChatsWithUnreadPriorityMessages(CSteamAPIContext.GetSteamFriends());
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003445 File Offset: 0x00001645
		public static void ActivateGameOverlayRemotePlayTogetherInviteDialog(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ActivateGameOverlayRemotePlayTogetherInviteDialog(CSteamAPIContext.GetSteamFriends(), steamIDLobby);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003458 File Offset: 0x00001658
		public static bool RegisterProtocolInOverlayBrowser(string pchProtocol)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchProtocol))
			{
				result = NativeMethods.ISteamFriends_RegisterProtocolInOverlayBrowser(CSteamAPIContext.GetSteamFriends(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000349C File Offset: 0x0000169C
		public static void ActivateGameOverlayInviteDialogConnectString(string pchConnectString)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchConnectString))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlayInviteDialogConnectString(CSteamAPIContext.GetSteamFriends(), utf8StringHandle);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000034DC File Offset: 0x000016DC
		public static SteamAPICall_t RequestEquippedProfileItems(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_RequestEquippedProfileItems(CSteamAPIContext.GetSteamFriends(), steamID);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000034F3 File Offset: 0x000016F3
		public static bool BHasEquippedProfileItem(CSteamID steamID, ECommunityProfileItemType itemType)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_BHasEquippedProfileItem(CSteamAPIContext.GetSteamFriends(), steamID, itemType);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003506 File Offset: 0x00001706
		public static string GetProfileItemPropertyString(CSteamID steamID, ECommunityProfileItemType itemType, ECommunityProfileItemProperty prop)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetProfileItemPropertyString(CSteamAPIContext.GetSteamFriends(), steamID, itemType, prop));
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000351F File Offset: 0x0000171F
		public static uint GetProfileItemPropertyUint(CSteamID steamID, ECommunityProfileItemType itemType, ECommunityProfileItemProperty prop)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetProfileItemPropertyUint(CSteamAPIContext.GetSteamFriends(), steamID, itemType, prop);
		}
	}
}
