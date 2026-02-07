using System;

namespace Steamworks
{
	// Token: 0x020001A7 RID: 423
	[Serializable]
	public struct InputAnalogActionHandle_t : IEquatable<InputAnalogActionHandle_t>, IComparable<InputAnalogActionHandle_t>
	{
		// Token: 0x06000A17 RID: 2583 RVA: 0x0000FAC2 File Offset: 0x0000DCC2
		public InputAnalogActionHandle_t(ulong value)
		{
			this.m_InputAnalogActionHandle = value;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0000FACB File Offset: 0x0000DCCB
		public override string ToString()
		{
			return this.m_InputAnalogActionHandle.ToString();
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0000FAD8 File Offset: 0x0000DCD8
		public override bool Equals(object other)
		{
			return other is InputAnalogActionHandle_t && this == (InputAnalogActionHandle_t)other;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0000FAF5 File Offset: 0x0000DCF5
		public override int GetHashCode()
		{
			return this.m_InputAnalogActionHandle.GetHashCode();
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0000FB02 File Offset: 0x0000DD02
		public static bool operator ==(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y)
		{
			return x.m_InputAnalogActionHandle == y.m_InputAnalogActionHandle;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0000FB12 File Offset: 0x0000DD12
		public static bool operator !=(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0000FB1E File Offset: 0x0000DD1E
		public static explicit operator InputAnalogActionHandle_t(ulong value)
		{
			return new InputAnalogActionHandle_t(value);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0000FB26 File Offset: 0x0000DD26
		public static explicit operator ulong(InputAnalogActionHandle_t that)
		{
			return that.m_InputAnalogActionHandle;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0000FB2E File Offset: 0x0000DD2E
		public bool Equals(InputAnalogActionHandle_t other)
		{
			return this.m_InputAnalogActionHandle == other.m_InputAnalogActionHandle;
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0000FB3E File Offset: 0x0000DD3E
		public int CompareTo(InputAnalogActionHandle_t other)
		{
			return this.m_InputAnalogActionHandle.CompareTo(other.m_InputAnalogActionHandle);
		}

		// Token: 0x04000AE6 RID: 2790
		public ulong m_InputAnalogActionHandle;
	}
}
