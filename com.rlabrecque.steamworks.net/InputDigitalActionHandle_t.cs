using System;

namespace Steamworks
{
	// Token: 0x020001A8 RID: 424
	[Serializable]
	public struct InputDigitalActionHandle_t : IEquatable<InputDigitalActionHandle_t>, IComparable<InputDigitalActionHandle_t>
	{
		// Token: 0x06000A21 RID: 2593 RVA: 0x0000FB51 File Offset: 0x0000DD51
		public InputDigitalActionHandle_t(ulong value)
		{
			this.m_InputDigitalActionHandle = value;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0000FB5A File Offset: 0x0000DD5A
		public override string ToString()
		{
			return this.m_InputDigitalActionHandle.ToString();
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0000FB67 File Offset: 0x0000DD67
		public override bool Equals(object other)
		{
			return other is InputDigitalActionHandle_t && this == (InputDigitalActionHandle_t)other;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0000FB84 File Offset: 0x0000DD84
		public override int GetHashCode()
		{
			return this.m_InputDigitalActionHandle.GetHashCode();
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0000FB91 File Offset: 0x0000DD91
		public static bool operator ==(InputDigitalActionHandle_t x, InputDigitalActionHandle_t y)
		{
			return x.m_InputDigitalActionHandle == y.m_InputDigitalActionHandle;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0000FBA1 File Offset: 0x0000DDA1
		public static bool operator !=(InputDigitalActionHandle_t x, InputDigitalActionHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0000FBAD File Offset: 0x0000DDAD
		public static explicit operator InputDigitalActionHandle_t(ulong value)
		{
			return new InputDigitalActionHandle_t(value);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0000FBB5 File Offset: 0x0000DDB5
		public static explicit operator ulong(InputDigitalActionHandle_t that)
		{
			return that.m_InputDigitalActionHandle;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0000FBBD File Offset: 0x0000DDBD
		public bool Equals(InputDigitalActionHandle_t other)
		{
			return this.m_InputDigitalActionHandle == other.m_InputDigitalActionHandle;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0000FBCD File Offset: 0x0000DDCD
		public int CompareTo(InputDigitalActionHandle_t other)
		{
			return this.m_InputDigitalActionHandle.CompareTo(other.m_InputDigitalActionHandle);
		}

		// Token: 0x04000AE7 RID: 2791
		public ulong m_InputDigitalActionHandle;
	}
}
