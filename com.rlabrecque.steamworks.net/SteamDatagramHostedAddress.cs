using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A0 RID: 416
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamDatagramHostedAddress
	{
		// Token: 0x060009DF RID: 2527 RVA: 0x0000F7A8 File Offset: 0x0000D9A8
		public void Clear()
		{
			this.m_cbSize = 0;
			this.m_data = new byte[128];
		}

		// Token: 0x04000AD1 RID: 2769
		public int m_cbSize;

		// Token: 0x04000AD2 RID: 2770
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] m_data;
	}
}
