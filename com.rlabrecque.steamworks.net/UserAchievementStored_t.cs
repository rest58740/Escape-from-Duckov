using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000ED RID: 237
	[CallbackIdentity(1103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementStored_t
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0000C918 File Offset: 0x0000AB18
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0000C925 File Offset: 0x0000AB25
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

		// Token: 0x040002E4 RID: 740
		public const int k_iCallback = 1103;

		// Token: 0x040002E5 RID: 741
		public ulong m_nGameID;

		// Token: 0x040002E6 RID: 742
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bGroupAchievement;

		// Token: 0x040002E7 RID: 743
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_rgchAchievementName_;

		// Token: 0x040002E8 RID: 744
		public uint m_nCurProgress;

		// Token: 0x040002E9 RID: 745
		public uint m_nMaxProgress;
	}
}
