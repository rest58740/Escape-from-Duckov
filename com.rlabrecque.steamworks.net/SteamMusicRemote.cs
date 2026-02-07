using System;

namespace Steamworks
{
	// Token: 0x0200001A RID: 26
	public static class SteamMusicRemote
	{
		// Token: 0x06000303 RID: 771 RVA: 0x00008894 File Offset: 0x00006A94
		public static bool RegisterSteamMusicRemote(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamMusicRemote_RegisterSteamMusicRemote(CSteamAPIContext.GetSteamMusicRemote(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000088D8 File Offset: 0x00006AD8
		public static bool DeregisterSteamMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_DeregisterSteamMusicRemote(CSteamAPIContext.GetSteamMusicRemote());
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000088E9 File Offset: 0x00006AE9
		public static bool BIsCurrentMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BIsCurrentMusicRemote(CSteamAPIContext.GetSteamMusicRemote());
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000088FA File Offset: 0x00006AFA
		public static bool BActivationSuccess(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BActivationSuccess(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000890C File Offset: 0x00006B0C
		public static bool SetDisplayName(string pchDisplayName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDisplayName))
			{
				result = NativeMethods.ISteamMusicRemote_SetDisplayName(CSteamAPIContext.GetSteamMusicRemote(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00008950 File Offset: 0x00006B50
		public static bool SetPNGIcon_64x64(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetPNGIcon_64x64(CSteamAPIContext.GetSteamMusicRemote(), pvBuffer, cbBufferLength);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00008963 File Offset: 0x00006B63
		public static bool EnablePlayPrevious(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayPrevious(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00008975 File Offset: 0x00006B75
		public static bool EnablePlayNext(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayNext(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00008987 File Offset: 0x00006B87
		public static bool EnableShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableShuffled(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00008999 File Offset: 0x00006B99
		public static bool EnableLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableLooped(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000089AB File Offset: 0x00006BAB
		public static bool EnableQueue(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableQueue(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x000089BD File Offset: 0x00006BBD
		public static bool EnablePlaylists(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlaylists(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000089CF File Offset: 0x00006BCF
		public static bool UpdatePlaybackStatus(AudioPlayback_Status nStatus)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdatePlaybackStatus(CSteamAPIContext.GetSteamMusicRemote(), nStatus);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x000089E1 File Offset: 0x00006BE1
		public static bool UpdateShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateShuffled(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000089F3 File Offset: 0x00006BF3
		public static bool UpdateLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateLooped(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00008A05 File Offset: 0x00006C05
		public static bool UpdateVolume(float flValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateVolume(CSteamAPIContext.GetSteamMusicRemote(), flValue);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00008A17 File Offset: 0x00006C17
		public static bool CurrentEntryWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryWillChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00008A28 File Offset: 0x00006C28
		public static bool CurrentEntryIsAvailable(bool bAvailable)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryIsAvailable(CSteamAPIContext.GetSteamMusicRemote(), bAvailable);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00008A3C File Offset: 0x00006C3C
		public static bool UpdateCurrentEntryText(string pchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchText))
			{
				result = NativeMethods.ISteamMusicRemote_UpdateCurrentEntryText(CSteamAPIContext.GetSteamMusicRemote(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00008A80 File Offset: 0x00006C80
		public static bool UpdateCurrentEntryElapsedSeconds(int nValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(CSteamAPIContext.GetSteamMusicRemote(), nValue);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00008A92 File Offset: 0x00006C92
		public static bool UpdateCurrentEntryCoverArt(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryCoverArt(CSteamAPIContext.GetSteamMusicRemote(), pvBuffer, cbBufferLength);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00008AA5 File Offset: 0x00006CA5
		public static bool CurrentEntryDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryDidChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00008AB6 File Offset: 0x00006CB6
		public static bool QueueWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueWillChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00008AC7 File Offset: 0x00006CC7
		public static bool ResetQueueEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetQueueEntries(CSteamAPIContext.GetSteamMusicRemote());
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00008AD8 File Offset: 0x00006CD8
		public static bool SetQueueEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = NativeMethods.ISteamMusicRemote_SetQueueEntry(CSteamAPIContext.GetSteamMusicRemote(), nID, nPosition, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00008B1C File Offset: 0x00006D1C
		public static bool SetCurrentQueueEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentQueueEntry(CSteamAPIContext.GetSteamMusicRemote(), nID);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00008B2E File Offset: 0x00006D2E
		public static bool QueueDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueDidChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00008B3F File Offset: 0x00006D3F
		public static bool PlaylistWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistWillChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00008B50 File Offset: 0x00006D50
		public static bool ResetPlaylistEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetPlaylistEntries(CSteamAPIContext.GetSteamMusicRemote());
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00008B64 File Offset: 0x00006D64
		public static bool SetPlaylistEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = NativeMethods.ISteamMusicRemote_SetPlaylistEntry(CSteamAPIContext.GetSteamMusicRemote(), nID, nPosition, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00008BA8 File Offset: 0x00006DA8
		public static bool SetCurrentPlaylistEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentPlaylistEntry(CSteamAPIContext.GetSteamMusicRemote(), nID);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00008BBA File Offset: 0x00006DBA
		public static bool PlaylistDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistDidChange(CSteamAPIContext.GetSteamMusicRemote());
		}
	}
}
