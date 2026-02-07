using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000F3 RID: 243
	[CallbackIdentity(1109)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementIconFetched_t
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0000C938 File Offset: 0x0000AB38
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x0000C945 File Offset: 0x0000AB45
		public string m_rgchAchievementName
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchAchievementName_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchAchievementName_, 128);
			}
		}

		// Token: 0x040002FD RID: 765
		public const int k_iCallback = 1109;

		// Token: 0x040002FE RID: 766
		public CGameID m_nGameID;

		// Token: 0x040002FF RID: 767
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_rgchAchievementName_;

		// Token: 0x04000300 RID: 768
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAchieved;

		// Token: 0x04000301 RID: 769
		public int m_nIconHandle;
	}
}
