using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A1 RID: 417
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamDatagramRelayAuthTicket
	{
		// Token: 0x060009E0 RID: 2528 RVA: 0x0000F7C1 File Offset: 0x0000D9C1
		public void Clear()
		{
		}

		// Token: 0x04000AD3 RID: 2771
		private SteamNetworkingIdentity m_identityGameserver;

		// Token: 0x04000AD4 RID: 2772
		private SteamNetworkingIdentity m_identityAuthorizedClient;

		// Token: 0x04000AD5 RID: 2773
		private uint m_unPublicIP;

		// Token: 0x04000AD6 RID: 2774
		private RTime32 m_rtimeTicketExpiry;

		// Token: 0x04000AD7 RID: 2775
		private SteamDatagramHostedAddress m_routing;

		// Token: 0x04000AD8 RID: 2776
		private uint m_nAppID;

		// Token: 0x04000AD9 RID: 2777
		private int m_nRestrictToVirtualPort;

		// Token: 0x04000ADA RID: 2778
		private const int k_nMaxExtraFields = 16;

		// Token: 0x04000ADB RID: 2779
		private int m_nExtraFields;

		// Token: 0x04000ADC RID: 2780
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		private SteamDatagramRelayAuthTicket.ExtraField[] m_vecExtraFields;

		// Token: 0x020001F8 RID: 504
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		private struct ExtraField
		{
			// Token: 0x04000B81 RID: 2945
			private SteamDatagramRelayAuthTicket.ExtraField.EType m_eType;

			// Token: 0x04000B82 RID: 2946
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
			private byte[] m_szName;

			// Token: 0x04000B83 RID: 2947
			private SteamDatagramRelayAuthTicket.ExtraField.OptionValue m_val;

			// Token: 0x020001FF RID: 511
			private enum EType
			{
				// Token: 0x04000B90 RID: 2960
				k_EType_String,
				// Token: 0x04000B91 RID: 2961
				k_EType_Int,
				// Token: 0x04000B92 RID: 2962
				k_EType_Fixed64
			}

			// Token: 0x02000200 RID: 512
			[StructLayout(LayoutKind.Explicit)]
			private struct OptionValue
			{
				// Token: 0x04000B93 RID: 2963
				[FieldOffset(0)]
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
				private byte[] m_szStringValue;

				// Token: 0x04000B94 RID: 2964
				[FieldOffset(0)]
				private long m_nIntValue;

				// Token: 0x04000B95 RID: 2965
				[FieldOffset(0)]
				private ulong m_nFixed64Value;
			}
		}
	}
}
