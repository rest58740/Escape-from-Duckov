using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000181 RID: 385
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetConnectionInfo_t
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x0000CA28 File Offset: 0x0000AC28
		// (set) Token: 0x060008CF RID: 2255 RVA: 0x0000CA35 File Offset: 0x0000AC35
		public string m_szEndDebug
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szEndDebug_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szEndDebug_, 128);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0000CA48 File Offset: 0x0000AC48
		// (set) Token: 0x060008D1 RID: 2257 RVA: 0x0000CA55 File Offset: 0x0000AC55
		public string m_szConnectionDescription
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szConnectionDescription_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szConnectionDescription_, 128);
			}
		}

		// Token: 0x04000A42 RID: 2626
		public SteamNetworkingIdentity m_identityRemote;

		// Token: 0x04000A43 RID: 2627
		public long m_nUserData;

		// Token: 0x04000A44 RID: 2628
		public HSteamListenSocket m_hListenSocket;

		// Token: 0x04000A45 RID: 2629
		public SteamNetworkingIPAddr m_addrRemote;

		// Token: 0x04000A46 RID: 2630
		public ushort m__pad1;

		// Token: 0x04000A47 RID: 2631
		public SteamNetworkingPOPID m_idPOPRemote;

		// Token: 0x04000A48 RID: 2632
		public SteamNetworkingPOPID m_idPOPRelay;

		// Token: 0x04000A49 RID: 2633
		public ESteamNetworkingConnectionState m_eState;

		// Token: 0x04000A4A RID: 2634
		public int m_eEndReason;

		// Token: 0x04000A4B RID: 2635
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szEndDebug_;

		// Token: 0x04000A4C RID: 2636
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szConnectionDescription_;

		// Token: 0x04000A4D RID: 2637
		public int m_nFlags;

		// Token: 0x04000A4E RID: 2638
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63)]
		public uint[] reserved;
	}
}
