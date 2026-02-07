using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BC RID: 444
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SteamNetworkingIdentity : IEquatable<SteamNetworkingIdentity>
	{
		// Token: 0x06000AB6 RID: 2742 RVA: 0x00010320 File Offset: 0x0000E520
		public void Clear()
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_Clear(ref this);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00010328 File Offset: 0x0000E528
		public bool IsInvalid()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_IsInvalid(ref this);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00010330 File Offset: 0x0000E530
		public void SetSteamID(CSteamID steamID)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetSteamID(ref this, (ulong)steamID);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0001033E File Offset: 0x0000E53E
		public CSteamID GetSteamID()
		{
			return (CSteamID)NativeMethods.SteamAPI_SteamNetworkingIdentity_GetSteamID(ref this);
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0001034B File Offset: 0x0000E54B
		public void SetSteamID64(ulong steamID)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetSteamID64(ref this, steamID);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00010354 File Offset: 0x0000E554
		public ulong GetSteamID64()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_GetSteamID64(ref this);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0001035C File Offset: 0x0000E55C
		public bool SetXboxPairwiseID(string pszString)
		{
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszString))
			{
				result = NativeMethods.SteamAPI_SteamNetworkingIdentity_SetXboxPairwiseID(ref this, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00010398 File Offset: 0x0000E598
		public string GetXboxPairwiseID()
		{
			return InteropHelp.PtrToStringUTF8(NativeMethods.SteamAPI_SteamNetworkingIdentity_GetXboxPairwiseID(ref this));
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x000103A5 File Offset: 0x0000E5A5
		public void SetPSNID(ulong id)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetPSNID(ref this, id);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x000103AE File Offset: 0x0000E5AE
		public ulong GetPSNID()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_GetPSNID(ref this);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x000103B6 File Offset: 0x0000E5B6
		public void SetIPAddr(SteamNetworkingIPAddr addr)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetIPAddr(ref this, ref addr);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x000103C1 File Offset: 0x0000E5C1
		public SteamNetworkingIPAddr GetIPAddr()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x000103C8 File Offset: 0x0000E5C8
		public void SetIPv4Addr(uint nIPv4, ushort nPort)
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetIPv4Addr(ref this, nIPv4, nPort);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000103D2 File Offset: 0x0000E5D2
		public uint GetIPv4()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_GetIPv4(ref this);
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x000103DA File Offset: 0x0000E5DA
		public ESteamNetworkingFakeIPType GetFakeIPType()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_GetFakeIPType(ref this);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000103E2 File Offset: 0x0000E5E2
		public bool IsFakeIP()
		{
			return this.GetFakeIPType() > ESteamNetworkingFakeIPType.k_ESteamNetworkingFakeIPType_NotFake;
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x000103ED File Offset: 0x0000E5ED
		public void SetLocalHost()
		{
			NativeMethods.SteamAPI_SteamNetworkingIdentity_SetLocalHost(ref this);
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x000103F5 File Offset: 0x0000E5F5
		public bool IsLocalHost()
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_IsLocalHost(ref this);
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00010400 File Offset: 0x0000E600
		public bool SetGenericString(string pszString)
		{
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszString))
			{
				result = NativeMethods.SteamAPI_SteamNetworkingIdentity_SetGenericString(ref this, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0001043C File Offset: 0x0000E63C
		public string GetGenericString()
		{
			return InteropHelp.PtrToStringUTF8(NativeMethods.SteamAPI_SteamNetworkingIdentity_GetGenericString(ref this));
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00010449 File Offset: 0x0000E649
		public bool SetGenericBytes(byte[] data, uint cbLen)
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_SetGenericBytes(ref this, data, cbLen);
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00010453 File Offset: 0x0000E653
		public byte[] GetGenericBytes(out int cbLen)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0001045A File Offset: 0x0000E65A
		public bool Equals(SteamNetworkingIdentity x)
		{
			return NativeMethods.SteamAPI_SteamNetworkingIdentity_IsEqualTo(ref this, ref x);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00010464 File Offset: 0x0000E664
		public void ToString(out string buf)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(128);
			NativeMethods.SteamAPI_SteamNetworkingIdentity_ToString(ref this, intPtr, 128U);
			buf = InteropHelp.PtrToStringUTF8(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00010498 File Offset: 0x0000E698
		public bool ParseString(string pszStr)
		{
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszStr))
			{
				result = NativeMethods.SteamAPI_SteamNetworkingIdentity_ParseString(ref this, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x04000B03 RID: 2819
		public ESteamNetworkingIdentityType m_eType;

		// Token: 0x04000B04 RID: 2820
		private int m_cbSize;

		// Token: 0x04000B05 RID: 2821
		private uint m_reserved0;

		// Token: 0x04000B06 RID: 2822
		private uint m_reserved1;

		// Token: 0x04000B07 RID: 2823
		private uint m_reserved2;

		// Token: 0x04000B08 RID: 2824
		private uint m_reserved3;

		// Token: 0x04000B09 RID: 2825
		private uint m_reserved4;

		// Token: 0x04000B0A RID: 2826
		private uint m_reserved5;

		// Token: 0x04000B0B RID: 2827
		private uint m_reserved6;

		// Token: 0x04000B0C RID: 2828
		private uint m_reserved7;

		// Token: 0x04000B0D RID: 2829
		private uint m_reserved8;

		// Token: 0x04000B0E RID: 2830
		private uint m_reserved9;

		// Token: 0x04000B0F RID: 2831
		private uint m_reserved10;

		// Token: 0x04000B10 RID: 2832
		private uint m_reserved11;

		// Token: 0x04000B11 RID: 2833
		private uint m_reserved12;

		// Token: 0x04000B12 RID: 2834
		private uint m_reserved13;

		// Token: 0x04000B13 RID: 2835
		private uint m_reserved14;

		// Token: 0x04000B14 RID: 2836
		private uint m_reserved15;

		// Token: 0x04000B15 RID: 2837
		private uint m_reserved16;

		// Token: 0x04000B16 RID: 2838
		private uint m_reserved17;

		// Token: 0x04000B17 RID: 2839
		private uint m_reserved18;

		// Token: 0x04000B18 RID: 2840
		private uint m_reserved19;

		// Token: 0x04000B19 RID: 2841
		private uint m_reserved20;

		// Token: 0x04000B1A RID: 2842
		private uint m_reserved21;

		// Token: 0x04000B1B RID: 2843
		private uint m_reserved22;

		// Token: 0x04000B1C RID: 2844
		private uint m_reserved23;

		// Token: 0x04000B1D RID: 2845
		private uint m_reserved24;

		// Token: 0x04000B1E RID: 2846
		private uint m_reserved25;

		// Token: 0x04000B1F RID: 2847
		private uint m_reserved26;

		// Token: 0x04000B20 RID: 2848
		private uint m_reserved27;

		// Token: 0x04000B21 RID: 2849
		private uint m_reserved28;

		// Token: 0x04000B22 RID: 2850
		private uint m_reserved29;

		// Token: 0x04000B23 RID: 2851
		private uint m_reserved30;

		// Token: 0x04000B24 RID: 2852
		private uint m_reserved31;

		// Token: 0x04000B25 RID: 2853
		public const int k_cchMaxString = 128;

		// Token: 0x04000B26 RID: 2854
		public const int k_cchMaxGenericString = 32;

		// Token: 0x04000B27 RID: 2855
		public const int k_cchMaxXboxPairwiseID = 33;

		// Token: 0x04000B28 RID: 2856
		public const int k_cbMaxGenericBytes = 32;
	}
}
