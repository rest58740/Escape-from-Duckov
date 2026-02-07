using System;

namespace Steamworks
{
	// Token: 0x020001A9 RID: 425
	[Serializable]
	public struct InputHandle_t : IEquatable<InputHandle_t>, IComparable<InputHandle_t>
	{
		// Token: 0x06000A2B RID: 2603 RVA: 0x0000FBE0 File Offset: 0x0000DDE0
		public InputHandle_t(ulong value)
		{
			this.m_InputHandle = value;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0000FBE9 File Offset: 0x0000DDE9
		public override string ToString()
		{
			return this.m_InputHandle.ToString();
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0000FBF6 File Offset: 0x0000DDF6
		public override bool Equals(object other)
		{
			return other is InputHandle_t && this == (InputHandle_t)other;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0000FC13 File Offset: 0x0000DE13
		public override int GetHashCode()
		{
			return this.m_InputHandle.GetHashCode();
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0000FC20 File Offset: 0x0000DE20
		public static bool operator ==(InputHandle_t x, InputHandle_t y)
		{
			return x.m_InputHandle == y.m_InputHandle;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0000FC30 File Offset: 0x0000DE30
		public static bool operator !=(InputHandle_t x, InputHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0000FC3C File Offset: 0x0000DE3C
		public static explicit operator InputHandle_t(ulong value)
		{
			return new InputHandle_t(value);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0000FC44 File Offset: 0x0000DE44
		public static explicit operator ulong(InputHandle_t that)
		{
			return that.m_InputHandle;
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0000FC4C File Offset: 0x0000DE4C
		public bool Equals(InputHandle_t other)
		{
			return this.m_InputHandle == other.m_InputHandle;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0000FC5C File Offset: 0x0000DE5C
		public int CompareTo(InputHandle_t other)
		{
			return this.m_InputHandle.CompareTo(other.m_InputHandle);
		}

		// Token: 0x04000AE8 RID: 2792
		public ulong m_InputHandle;
	}
}
