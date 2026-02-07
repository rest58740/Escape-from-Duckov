using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x02000199 RID: 409
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 372)]
	public class gameserveritem_t
	{
		// Token: 0x0600096F RID: 2415 RVA: 0x0000ED65 File Offset: 0x0000CF65
		public string GetGameDir()
		{
			return Encoding.UTF8.GetString(this.m_szGameDir, 0, Array.IndexOf<byte>(this.m_szGameDir, 0));
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0000ED84 File Offset: 0x0000CF84
		public void SetGameDir(string dir)
		{
			this.m_szGameDir = Encoding.UTF8.GetBytes(dir + "\0");
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0000EDA1 File Offset: 0x0000CFA1
		public string GetMap()
		{
			return Encoding.UTF8.GetString(this.m_szMap, 0, Array.IndexOf<byte>(this.m_szMap, 0));
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0000EDC0 File Offset: 0x0000CFC0
		public void SetMap(string map)
		{
			this.m_szMap = Encoding.UTF8.GetBytes(map + "\0");
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0000EDDD File Offset: 0x0000CFDD
		public string GetGameDescription()
		{
			return Encoding.UTF8.GetString(this.m_szGameDescription, 0, Array.IndexOf<byte>(this.m_szGameDescription, 0));
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0000EDFC File Offset: 0x0000CFFC
		public void SetGameDescription(string desc)
		{
			this.m_szGameDescription = Encoding.UTF8.GetBytes(desc + "\0");
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0000EE19 File Offset: 0x0000D019
		public string GetServerName()
		{
			if (this.m_szServerName[0] == 0)
			{
				return this.m_NetAdr.GetConnectionAddressString();
			}
			return Encoding.UTF8.GetString(this.m_szServerName, 0, Array.IndexOf<byte>(this.m_szServerName, 0));
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0000EE4E File Offset: 0x0000D04E
		public void SetServerName(string name)
		{
			this.m_szServerName = Encoding.UTF8.GetBytes(name + "\0");
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0000EE6B File Offset: 0x0000D06B
		public string GetGameTags()
		{
			return Encoding.UTF8.GetString(this.m_szGameTags, 0, Array.IndexOf<byte>(this.m_szGameTags, 0));
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0000EE8A File Offset: 0x0000D08A
		public void SetGameTags(string tags)
		{
			this.m_szGameTags = Encoding.UTF8.GetBytes(tags + "\0");
		}

		// Token: 0x04000AB3 RID: 2739
		public servernetadr_t m_NetAdr;

		// Token: 0x04000AB4 RID: 2740
		public int m_nPing;

		// Token: 0x04000AB5 RID: 2741
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHadSuccessfulResponse;

		// Token: 0x04000AB6 RID: 2742
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bDoNotRefresh;

		// Token: 0x04000AB7 RID: 2743
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szGameDir;

		// Token: 0x04000AB8 RID: 2744
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szMap;

		// Token: 0x04000AB9 RID: 2745
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szGameDescription;

		// Token: 0x04000ABA RID: 2746
		public uint m_nAppID;

		// Token: 0x04000ABB RID: 2747
		public int m_nPlayers;

		// Token: 0x04000ABC RID: 2748
		public int m_nMaxPlayers;

		// Token: 0x04000ABD RID: 2749
		public int m_nBotPlayers;

		// Token: 0x04000ABE RID: 2750
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPassword;

		// Token: 0x04000ABF RID: 2751
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSecure;

		// Token: 0x04000AC0 RID: 2752
		public uint m_ulTimeLastPlayed;

		// Token: 0x04000AC1 RID: 2753
		public int m_nServerVersion;

		// Token: 0x04000AC2 RID: 2754
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szServerName;

		// Token: 0x04000AC3 RID: 2755
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szGameTags;

		// Token: 0x04000AC4 RID: 2756
		public CSteamID m_steamID;
	}
}
