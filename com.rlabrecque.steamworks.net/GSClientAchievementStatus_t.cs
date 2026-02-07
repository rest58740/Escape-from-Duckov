using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004A RID: 74
	[CallbackIdentity(206)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientAchievementStatus_t
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x0000C6FF File Offset: 0x0000A8FF
		// (set) Token: 0x0600089C RID: 2204 RVA: 0x0000C70C File Offset: 0x0000A90C
		public string m_pchAchievement
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_pchAchievement_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_pchAchievement_, 128);
			}
		}

		// Token: 0x04000072 RID: 114
		public const int k_iCallback = 206;

		// Token: 0x04000073 RID: 115
		public ulong m_SteamID;

		// Token: 0x04000074 RID: 116
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_pchAchievement_;

		// Token: 0x04000075 RID: 117
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUnlocked;
	}
}
