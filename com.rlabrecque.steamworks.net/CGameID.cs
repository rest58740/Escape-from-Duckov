using System;

namespace Steamworks
{
	// Token: 0x0200019D RID: 413
	[Serializable]
	public struct CGameID : IEquatable<CGameID>, IComparable<CGameID>
	{
		// Token: 0x06000994 RID: 2452 RVA: 0x0000F0D3 File Offset: 0x0000D2D3
		public CGameID(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0000F0DC File Offset: 0x0000D2DC
		public CGameID(AppId_t nAppID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0000F0ED File Offset: 0x0000D2ED
		public CGameID(AppId_t nAppID, uint nModID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
			this.SetType(CGameID.EGameIDType.k_EGameIDTypeGameMod);
			this.SetModID(nModID);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0000F10C File Offset: 0x0000D30C
		public bool IsSteamApp()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeApp;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0000F117 File Offset: 0x0000D317
		public bool IsMod()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeGameMod;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0000F122 File Offset: 0x0000D322
		public bool IsShortcut()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeShortcut;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0000F12D File Offset: 0x0000D32D
		public bool IsP2PFile()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeP2P;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0000F138 File Offset: 0x0000D338
		public AppId_t AppID()
		{
			return new AppId_t((uint)(this.m_GameID & 16777215UL));
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0000F14D File Offset: 0x0000D34D
		public CGameID.EGameIDType Type()
		{
			return (CGameID.EGameIDType)(this.m_GameID >> 24 & 255UL);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0000F160 File Offset: 0x0000D360
		public uint ModID()
		{
			return (uint)(this.m_GameID >> 32 & (ulong)-1);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0000F170 File Offset: 0x0000D370
		public bool IsValid()
		{
			switch (this.Type())
			{
			case CGameID.EGameIDType.k_EGameIDTypeApp:
				return this.AppID() != AppId_t.Invalid;
			case CGameID.EGameIDType.k_EGameIDTypeGameMod:
				return this.AppID() != AppId_t.Invalid && (this.ModID() & 2147483648U) > 0U;
			case CGameID.EGameIDType.k_EGameIDTypeShortcut:
				return (this.ModID() & 2147483648U) > 0U;
			case CGameID.EGameIDType.k_EGameIDTypeP2P:
				return this.AppID() == AppId_t.Invalid && (this.ModID() & 2147483648U) > 0U;
			default:
				return false;
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0000F206 File Offset: 0x0000D406
		public void Reset()
		{
			this.m_GameID = 0UL;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0000F210 File Offset: 0x0000D410
		public void Set(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0000F219 File Offset: 0x0000D419
		private void SetAppID(AppId_t other)
		{
			this.m_GameID = ((this.m_GameID & 18446744073692774400UL) | ((ulong)((uint)other) & 16777215UL));
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0000F23D File Offset: 0x0000D43D
		private void SetType(CGameID.EGameIDType other)
		{
			this.m_GameID = ((this.m_GameID & 18446744069431361535UL) | (ulong)((ulong)((long)other & 255L) << 24));
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0000F262 File Offset: 0x0000D462
		private void SetModID(uint other)
		{
			this.m_GameID = ((this.m_GameID & (ulong)-1) | ((ulong)other & (ulong)-1) << 32);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0000F27C File Offset: 0x0000D47C
		public override string ToString()
		{
			return this.m_GameID.ToString();
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0000F289 File Offset: 0x0000D489
		public override bool Equals(object other)
		{
			return other is CGameID && this == (CGameID)other;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0000F2A6 File Offset: 0x0000D4A6
		public override int GetHashCode()
		{
			return this.m_GameID.GetHashCode();
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0000F2B3 File Offset: 0x0000D4B3
		public static bool operator ==(CGameID x, CGameID y)
		{
			return x.m_GameID == y.m_GameID;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0000F2C3 File Offset: 0x0000D4C3
		public static bool operator !=(CGameID x, CGameID y)
		{
			return !(x == y);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0000F2CF File Offset: 0x0000D4CF
		public static explicit operator CGameID(ulong value)
		{
			return new CGameID(value);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0000F2D7 File Offset: 0x0000D4D7
		public static explicit operator ulong(CGameID that)
		{
			return that.m_GameID;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0000F2DF File Offset: 0x0000D4DF
		public bool Equals(CGameID other)
		{
			return this.m_GameID == other.m_GameID;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0000F2EF File Offset: 0x0000D4EF
		public int CompareTo(CGameID other)
		{
			return this.m_GameID.CompareTo(other.m_GameID);
		}

		// Token: 0x04000AC8 RID: 2760
		public ulong m_GameID;

		// Token: 0x020001F7 RID: 503
		public enum EGameIDType
		{
			// Token: 0x04000B7D RID: 2941
			k_EGameIDTypeApp,
			// Token: 0x04000B7E RID: 2942
			k_EGameIDTypeGameMod,
			// Token: 0x04000B7F RID: 2943
			k_EGameIDTypeShortcut,
			// Token: 0x04000B80 RID: 2944
			k_EGameIDTypeP2P
		}
	}
}
