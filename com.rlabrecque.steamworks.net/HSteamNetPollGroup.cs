using System;

namespace Steamworks
{
	// Token: 0x020001B9 RID: 441
	[Serializable]
	public struct HSteamNetPollGroup : IEquatable<HSteamNetPollGroup>, IComparable<HSteamNetPollGroup>
	{
		// Token: 0x06000AAB RID: 2731 RVA: 0x00010284 File Offset: 0x0000E484
		public HSteamNetPollGroup(uint value)
		{
			this.m_HSteamNetPollGroup = value;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0001028D File Offset: 0x0000E48D
		public override string ToString()
		{
			return this.m_HSteamNetPollGroup.ToString();
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0001029A File Offset: 0x0000E49A
		public override bool Equals(object other)
		{
			return other is HSteamNetPollGroup && this == (HSteamNetPollGroup)other;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000102B7 File Offset: 0x0000E4B7
		public override int GetHashCode()
		{
			return this.m_HSteamNetPollGroup.GetHashCode();
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000102C4 File Offset: 0x0000E4C4
		public static bool operator ==(HSteamNetPollGroup x, HSteamNetPollGroup y)
		{
			return x.m_HSteamNetPollGroup == y.m_HSteamNetPollGroup;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x000102D4 File Offset: 0x0000E4D4
		public static bool operator !=(HSteamNetPollGroup x, HSteamNetPollGroup y)
		{
			return !(x == y);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000102E0 File Offset: 0x0000E4E0
		public static explicit operator HSteamNetPollGroup(uint value)
		{
			return new HSteamNetPollGroup(value);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000102E8 File Offset: 0x0000E4E8
		public static explicit operator uint(HSteamNetPollGroup that)
		{
			return that.m_HSteamNetPollGroup;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000102F0 File Offset: 0x0000E4F0
		public bool Equals(HSteamNetPollGroup other)
		{
			return this.m_HSteamNetPollGroup == other.m_HSteamNetPollGroup;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00010300 File Offset: 0x0000E500
		public int CompareTo(HSteamNetPollGroup other)
		{
			return this.m_HSteamNetPollGroup.CompareTo(other.m_HSteamNetPollGroup);
		}

		// Token: 0x04000AFD RID: 2813
		public static readonly HSteamNetPollGroup Invalid = new HSteamNetPollGroup(0U);

		// Token: 0x04000AFE RID: 2814
		public uint m_HSteamNetPollGroup;
	}
}
