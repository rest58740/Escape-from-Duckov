using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BE RID: 446
	[Serializable]
	public struct SteamNetworkingMessage_t
	{
		// Token: 0x06000ADC RID: 2780 RVA: 0x000105A5 File Offset: 0x0000E7A5
		public void Release()
		{
			throw new NotImplementedException("Please use the static Release function instead which takes an IntPtr.");
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x000105B1 File Offset: 0x0000E7B1
		public static void Release(IntPtr pointer)
		{
			NativeMethods.SteamAPI_SteamNetworkingMessage_t_Release(pointer);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x000105B9 File Offset: 0x0000E7B9
		public static SteamNetworkingMessage_t FromIntPtr(IntPtr pointer)
		{
			return (SteamNetworkingMessage_t)Marshal.PtrToStructure(pointer, typeof(SteamNetworkingMessage_t));
		}

		// Token: 0x04000B2C RID: 2860
		public IntPtr m_pData;

		// Token: 0x04000B2D RID: 2861
		public int m_cbSize;

		// Token: 0x04000B2E RID: 2862
		public HSteamNetConnection m_conn;

		// Token: 0x04000B2F RID: 2863
		public SteamNetworkingIdentity m_identityPeer;

		// Token: 0x04000B30 RID: 2864
		public long m_nConnUserData;

		// Token: 0x04000B31 RID: 2865
		public SteamNetworkingMicroseconds m_usecTimeReceived;

		// Token: 0x04000B32 RID: 2866
		public long m_nMessageNumber;

		// Token: 0x04000B33 RID: 2867
		public IntPtr m_pfnFreeData;

		// Token: 0x04000B34 RID: 2868
		internal IntPtr m_pfnRelease;

		// Token: 0x04000B35 RID: 2869
		public int m_nChannel;

		// Token: 0x04000B36 RID: 2870
		public int m_nFlags;

		// Token: 0x04000B37 RID: 2871
		public long m_nUserData;

		// Token: 0x04000B38 RID: 2872
		public ushort m_idxLane;

		// Token: 0x04000B39 RID: 2873
		public ushort _pad1__;
	}
}
