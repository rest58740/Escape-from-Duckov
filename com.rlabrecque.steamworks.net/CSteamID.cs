using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200019E RID: 414
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CSteamID : IEquatable<CSteamID>, IComparable<CSteamID>
	{
		// Token: 0x060009AD RID: 2477 RVA: 0x0000F302 File Offset: 0x0000D502
		public CSteamID(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.Set(unAccountID, eUniverse, eAccountType);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0000F315 File Offset: 0x0000D515
		public CSteamID(AccountID_t unAccountID, uint unAccountInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0000F32A File Offset: 0x0000D52A
		public CSteamID(ulong ulSteamID)
		{
			this.m_SteamID = ulSteamID;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0000F333 File Offset: 0x0000D533
		public void Set(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			if (eAccountType == EAccountType.k_EAccountTypeClan || eAccountType == EAccountType.k_EAccountTypeGameServer)
			{
				this.SetAccountInstance(0U);
				return;
			}
			this.SetAccountInstance(1U);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0000F361 File Offset: 0x0000D561
		public void InstancedSet(AccountID_t unAccountID, uint unInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			this.SetAccountInstance(unInstance);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0000F380 File Offset: 0x0000D580
		public void Clear()
		{
			this.m_SteamID = 0UL;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0000F38A File Offset: 0x0000D58A
		public void CreateBlankAnonLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonGameServer);
			this.SetAccountInstance(0U);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0000F3AD File Offset: 0x0000D5AD
		public void CreateBlankAnonUserLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonUser);
			this.SetAccountInstance(0U);
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0000F3D1 File Offset: 0x0000D5D1
		public bool BBlankAnonAccount()
		{
			return this.GetAccountID() == new AccountID_t(0U) && this.BAnonAccount() && this.GetUnAccountInstance() == 0U;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0000F3F9 File Offset: 0x0000D5F9
		public bool BGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0000F40F File Offset: 0x0000D60F
		public bool BPersistentGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0000F41A File Offset: 0x0000D61A
		public bool BAnonGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0000F425 File Offset: 0x0000D625
		public bool BContentServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeContentServer;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0000F430 File Offset: 0x0000D630
		public bool BClanAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeClan;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0000F43B File Offset: 0x0000D63B
		public bool BChatAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0000F446 File Offset: 0x0000D646
		public bool IsLobby()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat && (this.GetUnAccountInstance() & 262144U) > 0U;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0000F462 File Offset: 0x0000D662
		public bool BIndividualAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeIndividual || this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0000F479 File Offset: 0x0000D679
		public bool BAnonAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0000F490 File Offset: 0x0000D690
		public bool BAnonUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0000F49C File Offset: 0x0000D69C
		public bool BConsoleUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0000F4A8 File Offset: 0x0000D6A8
		public void SetAccountID(AccountID_t other)
		{
			this.m_SteamID = ((this.m_SteamID & 18446744069414584320UL) | ((ulong)((uint)other) & (ulong)-1));
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0000F4CB File Offset: 0x0000D6CB
		public void SetAccountInstance(uint other)
		{
			this.m_SteamID = ((this.m_SteamID & 18442240478377148415UL) | ((ulong)other & 1048575UL) << 32);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0000F4F0 File Offset: 0x0000D6F0
		public void SetEAccountType(EAccountType other)
		{
			this.m_SteamID = ((this.m_SteamID & 18379190079298994175UL) | (ulong)((ulong)((long)other & 15L) << 52));
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0000F512 File Offset: 0x0000D712
		public void SetEUniverse(EUniverse other)
		{
			this.m_SteamID = ((this.m_SteamID & 72057594037927935UL) | (ulong)((ulong)((long)other & 255L) << 56));
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0000F537 File Offset: 0x0000D737
		public AccountID_t GetAccountID()
		{
			return new AccountID_t((uint)(this.m_SteamID & (ulong)-1));
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0000F548 File Offset: 0x0000D748
		public uint GetUnAccountInstance()
		{
			return (uint)(this.m_SteamID >> 32 & 1048575UL);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0000F55B File Offset: 0x0000D75B
		public EAccountType GetEAccountType()
		{
			return (EAccountType)(this.m_SteamID >> 52 & 15UL);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0000F56B File Offset: 0x0000D76B
		public EUniverse GetEUniverse()
		{
			return (EUniverse)(this.m_SteamID >> 56 & 255UL);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0000F580 File Offset: 0x0000D780
		public bool IsValid()
		{
			return this.GetEAccountType() > EAccountType.k_EAccountTypeInvalid && this.GetEAccountType() < EAccountType.k_EAccountTypeMax && this.GetEUniverse() > EUniverse.k_EUniverseInvalid && this.GetEUniverse() < EUniverse.k_EUniverseMax && (this.GetEAccountType() != EAccountType.k_EAccountTypeIndividual || (!(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() <= 1U)) && (this.GetEAccountType() != EAccountType.k_EAccountTypeClan || (!(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() == 0U)) && (this.GetEAccountType() != EAccountType.k_EAccountTypeGameServer || !(this.GetAccountID() == new AccountID_t(0U)));
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0000F622 File Offset: 0x0000D822
		public override string ToString()
		{
			return this.m_SteamID.ToString();
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0000F62F File Offset: 0x0000D82F
		public override bool Equals(object other)
		{
			return other is CSteamID && this == (CSteamID)other;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0000F64C File Offset: 0x0000D84C
		public override int GetHashCode()
		{
			return this.m_SteamID.GetHashCode();
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0000F659 File Offset: 0x0000D859
		public static bool operator ==(CSteamID x, CSteamID y)
		{
			return x.m_SteamID == y.m_SteamID;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0000F669 File Offset: 0x0000D869
		public static bool operator !=(CSteamID x, CSteamID y)
		{
			return !(x == y);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0000F675 File Offset: 0x0000D875
		public static explicit operator CSteamID(ulong value)
		{
			return new CSteamID(value);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0000F67D File Offset: 0x0000D87D
		public static explicit operator ulong(CSteamID that)
		{
			return that.m_SteamID;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0000F685 File Offset: 0x0000D885
		public bool Equals(CSteamID other)
		{
			return this.m_SteamID == other.m_SteamID;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0000F695 File Offset: 0x0000D895
		public int CompareTo(CSteamID other)
		{
			return this.m_SteamID.CompareTo(other.m_SteamID);
		}

		// Token: 0x04000AC9 RID: 2761
		public static readonly CSteamID Nil = default(CSteamID);

		// Token: 0x04000ACA RID: 2762
		public static readonly CSteamID OutofDateGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000ACB RID: 2763
		public static readonly CSteamID LanModeGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000ACC RID: 2764
		public static readonly CSteamID NotInitYetGS = new CSteamID(new AccountID_t(1U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000ACD RID: 2765
		public static readonly CSteamID NonSteamGS = new CSteamID(new AccountID_t(2U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000ACE RID: 2766
		public ulong m_SteamID;
	}
}
