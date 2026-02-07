using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BA RID: 442
	[Serializable]
	public struct SteamNetworkingConfigValue_t
	{
		// Token: 0x04000AFF RID: 2815
		public ESteamNetworkingConfigValue m_eValue;

		// Token: 0x04000B00 RID: 2816
		public ESteamNetworkingConfigDataType m_eDataType;

		// Token: 0x04000B01 RID: 2817
		public SteamNetworkingConfigValue_t.OptionValue m_val;

		// Token: 0x020001FC RID: 508
		[StructLayout(LayoutKind.Explicit)]
		public struct OptionValue
		{
			// Token: 0x04000B8A RID: 2954
			[FieldOffset(0)]
			public int m_int32;

			// Token: 0x04000B8B RID: 2955
			[FieldOffset(0)]
			public long m_int64;

			// Token: 0x04000B8C RID: 2956
			[FieldOffset(0)]
			public float m_float;

			// Token: 0x04000B8D RID: 2957
			[FieldOffset(0)]
			public IntPtr m_string;

			// Token: 0x04000B8E RID: 2958
			[FieldOffset(0)]
			public IntPtr m_functionPtr;
		}
	}
}
