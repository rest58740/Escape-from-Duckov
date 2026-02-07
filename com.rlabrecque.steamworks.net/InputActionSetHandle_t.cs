using System;

namespace Steamworks
{
	// Token: 0x020001A6 RID: 422
	[Serializable]
	public struct InputActionSetHandle_t : IEquatable<InputActionSetHandle_t>, IComparable<InputActionSetHandle_t>
	{
		// Token: 0x06000A0D RID: 2573 RVA: 0x0000FA33 File Offset: 0x0000DC33
		public InputActionSetHandle_t(ulong value)
		{
			this.m_InputActionSetHandle = value;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0000FA3C File Offset: 0x0000DC3C
		public override string ToString()
		{
			return this.m_InputActionSetHandle.ToString();
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0000FA49 File Offset: 0x0000DC49
		public override bool Equals(object other)
		{
			return other is InputActionSetHandle_t && this == (InputActionSetHandle_t)other;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0000FA66 File Offset: 0x0000DC66
		public override int GetHashCode()
		{
			return this.m_InputActionSetHandle.GetHashCode();
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0000FA73 File Offset: 0x0000DC73
		public static bool operator ==(InputActionSetHandle_t x, InputActionSetHandle_t y)
		{
			return x.m_InputActionSetHandle == y.m_InputActionSetHandle;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0000FA83 File Offset: 0x0000DC83
		public static bool operator !=(InputActionSetHandle_t x, InputActionSetHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0000FA8F File Offset: 0x0000DC8F
		public static explicit operator InputActionSetHandle_t(ulong value)
		{
			return new InputActionSetHandle_t(value);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0000FA97 File Offset: 0x0000DC97
		public static explicit operator ulong(InputActionSetHandle_t that)
		{
			return that.m_InputActionSetHandle;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0000FA9F File Offset: 0x0000DC9F
		public bool Equals(InputActionSetHandle_t other)
		{
			return this.m_InputActionSetHandle == other.m_InputActionSetHandle;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0000FAAF File Offset: 0x0000DCAF
		public int CompareTo(InputActionSetHandle_t other)
		{
			return this.m_InputActionSetHandle.CompareTo(other.m_InputActionSetHandle);
		}

		// Token: 0x04000AE5 RID: 2789
		public ulong m_InputActionSetHandle;
	}
}
