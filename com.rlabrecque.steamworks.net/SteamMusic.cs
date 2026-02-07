using System;

namespace Steamworks
{
	// Token: 0x02000019 RID: 25
	public static class SteamMusic
	{
		// Token: 0x060002FA RID: 762 RVA: 0x000087F9 File Offset: 0x000069F9
		public static bool BIsEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsEnabled(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000880A File Offset: 0x00006A0A
		public static bool BIsPlaying()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsPlaying(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000881B File Offset: 0x00006A1B
		public static AudioPlayback_Status GetPlaybackStatus()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetPlaybackStatus(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000882C File Offset: 0x00006A2C
		public static void Play()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Play(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000883D File Offset: 0x00006A3D
		public static void Pause()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Pause(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000884E File Offset: 0x00006A4E
		public static void PlayPrevious()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayPrevious(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000885F File Offset: 0x00006A5F
		public static void PlayNext()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayNext(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00008870 File Offset: 0x00006A70
		public static void SetVolume(float flVolume)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_SetVolume(CSteamAPIContext.GetSteamMusic(), flVolume);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00008882 File Offset: 0x00006A82
		public static float GetVolume()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetVolume(CSteamAPIContext.GetSteamMusic());
		}
	}
}
