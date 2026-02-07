using System;

namespace Steamworks
{
	// Token: 0x020001D3 RID: 467
	[Serializable]
	public struct HSteamPipe : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
	{
		// Token: 0x06000BAE RID: 2990 RVA: 0x000112F2 File Offset: 0x0000F4F2
		public HSteamPipe(int value)
		{
			this.m_HSteamPipe = value;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x000112FB File Offset: 0x0000F4FB
		public override string ToString()
		{
			return this.m_HSteamPipe.ToString();
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00011308 File Offset: 0x0000F508
		public override bool Equals(object other)
		{
			return other is HSteamPipe && this == (HSteamPipe)other;
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00011325 File Offset: 0x0000F525
		public override int GetHashCode()
		{
			return this.m_HSteamPipe.GetHashCode();
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00011332 File Offset: 0x0000F532
		public static bool operator ==(HSteamPipe x, HSteamPipe y)
		{
			return x.m_HSteamPipe == y.m_HSteamPipe;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00011342 File Offset: 0x0000F542
		public static bool operator !=(HSteamPipe x, HSteamPipe y)
		{
			return !(x == y);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0001134E File Offset: 0x0000F54E
		public static explicit operator HSteamPipe(int value)
		{
			return new HSteamPipe(value);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00011356 File Offset: 0x0000F556
		public static explicit operator int(HSteamPipe that)
		{
			return that.m_HSteamPipe;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0001135E File Offset: 0x0000F55E
		public bool Equals(HSteamPipe other)
		{
			return this.m_HSteamPipe == other.m_HSteamPipe;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0001136E File Offset: 0x0000F56E
		public int CompareTo(HSteamPipe other)
		{
			return this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);
		}

		// Token: 0x04000B5C RID: 2908
		public int m_HSteamPipe;
	}
}
