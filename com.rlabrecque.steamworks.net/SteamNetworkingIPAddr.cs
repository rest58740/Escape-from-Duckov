using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BD RID: 445
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SteamNetworkingIPAddr : IEquatable<SteamNetworkingIPAddr>
	{
		// Token: 0x06000ACF RID: 2767 RVA: 0x000104D4 File Offset: 0x0000E6D4
		public void Clear()
		{
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_Clear(ref this);
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x000104DC File Offset: 0x0000E6DC
		public bool IsIPv6AllZeros()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_IsIPv6AllZeros(ref this);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x000104E4 File Offset: 0x0000E6E4
		public void SetIPv6(byte[] ipv6, ushort nPort)
		{
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_SetIPv6(ref this, ipv6, nPort);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x000104EE File Offset: 0x0000E6EE
		public void SetIPv4(uint nIP, ushort nPort)
		{
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_SetIPv4(ref this, nIP, nPort);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x000104F8 File Offset: 0x0000E6F8
		public bool IsIPv4()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_IsIPv4(ref this);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00010500 File Offset: 0x0000E700
		public uint GetIPv4()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_GetIPv4(ref this);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00010508 File Offset: 0x0000E708
		public void SetIPv6LocalHost(ushort nPort = 0)
		{
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_SetIPv6LocalHost(ref this, nPort);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00010511 File Offset: 0x0000E711
		public bool IsLocalHost()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_IsLocalHost(ref this);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0001051C File Offset: 0x0000E71C
		public void ToString(out string buf, bool bWithPort)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(48);
			NativeMethods.SteamAPI_SteamNetworkingIPAddr_ToString(ref this, intPtr, 48U, bWithPort);
			buf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0001054C File Offset: 0x0000E74C
		public bool ParseString(string pszStr)
		{
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszStr))
			{
				result = NativeMethods.SteamAPI_SteamNetworkingIPAddr_ParseString(ref this, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00010588 File Offset: 0x0000E788
		public bool Equals(SteamNetworkingIPAddr x)
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_IsEqualTo(ref this, ref x);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00010592 File Offset: 0x0000E792
		public ESteamNetworkingFakeIPType GetFakeIPType()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIPAddr_GetFakeIPType(ref this);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0001059A File Offset: 0x0000E79A
		public bool IsFakeIP()
		{
			return this.GetFakeIPType() > ESteamNetworkingFakeIPType.k_ESteamNetworkingFakeIPType_NotFake;
		}

		// Token: 0x04000B29 RID: 2857
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] m_ipv6;

		// Token: 0x04000B2A RID: 2858
		public ushort m_port;

		// Token: 0x04000B2B RID: 2859
		public const int k_cchMaxString = 48;
	}
}
