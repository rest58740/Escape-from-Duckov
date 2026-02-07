using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000077 RID: 119
	[CallbackIdentity(4705)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryRequestPricesResult_t
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x0000C71F File Offset: 0x0000A91F
		// (set) Token: 0x0600089E RID: 2206 RVA: 0x0000C72C File Offset: 0x0000A92C
		public string m_rgchCurrency
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchCurrency_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchCurrency_, 4);
			}
		}

		// Token: 0x04000131 RID: 305
		public const int k_iCallback = 4705;

		// Token: 0x04000132 RID: 306
		public EResult m_result;

		// Token: 0x04000133 RID: 307
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] m_rgchCurrency_;
	}
}
