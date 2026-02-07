using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000196 RID: 406
	public static class SteamEncryptedAppTicket
	{
		// Token: 0x0600093B RID: 2363 RVA: 0x0000E2F3 File Offset: 0x0000C4F3
		public static bool BDecryptTicket(byte[] rgubTicketEncrypted, uint cubTicketEncrypted, byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, byte[] rgubKey, int cubKey)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamEncryptedAppTicket_BDecryptTicket(rgubTicketEncrypted, cubTicketEncrypted, rgubTicketDecrypted, ref pcubTicketDecrypted, rgubKey, cubKey);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0000E307 File Offset: 0x0000C507
		public static bool BIsTicketForApp(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamEncryptedAppTicket_BIsTicketForApp(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0000E316 File Offset: 0x0000C516
		public static uint GetTicketIssueTime(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamEncryptedAppTicket_GetTicketIssueTime(rgubTicketDecrypted, cubTicketDecrypted);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0000E324 File Offset: 0x0000C524
		public static void GetTicketSteamID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out CSteamID psteamID)
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamEncryptedAppTicket_GetTicketSteamID(rgubTicketDecrypted, cubTicketDecrypted, out psteamID);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0000E333 File Offset: 0x0000C533
		public static uint GetTicketAppID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamEncryptedAppTicket_GetTicketAppID(rgubTicketDecrypted, cubTicketDecrypted);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0000E341 File Offset: 0x0000C541
		public static bool BUserOwnsAppInTicket(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamEncryptedAppTicket_BUserOwnsAppInTicket(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0000E350 File Offset: 0x0000C550
		public static bool BUserIsVacBanned(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamEncryptedAppTicket_BUserIsVacBanned(rgubTicketDecrypted, cubTicketDecrypted);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0000E360 File Offset: 0x0000C560
		public static byte[] GetUserVariableData(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData)
		{
			InteropHelp.TestIfPlatformSupported();
			IntPtr source = NativeMethods.SteamEncryptedAppTicket_GetUserVariableData(rgubTicketDecrypted, cubTicketDecrypted, out pcubUserData);
			byte[] array = new byte[pcubUserData];
			Marshal.Copy(source, array, 0, (int)pcubUserData);
			return array;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0000E38C File Offset: 0x0000C58C
		public static bool BIsTicketSigned(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, byte[] pubRSAKey, uint cubRSAKey)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamEncryptedAppTicket_BIsTicketSigned(rgubTicketDecrypted, cubTicketDecrypted, pubRSAKey, cubRSAKey);
		}
	}
}
