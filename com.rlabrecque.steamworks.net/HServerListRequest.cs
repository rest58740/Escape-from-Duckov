using System;

namespace Steamworks
{
	// Token: 0x020001B0 RID: 432
	[Serializable]
	public struct HServerListRequest : IEquatable<HServerListRequest>
	{
		// Token: 0x06000A64 RID: 2660 RVA: 0x0000FED4 File Offset: 0x0000E0D4
		public HServerListRequest(IntPtr value)
		{
			this.m_HServerListRequest = value;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0000FEDD File Offset: 0x0000E0DD
		public override string ToString()
		{
			return this.m_HServerListRequest.ToString();
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0000FEEA File Offset: 0x0000E0EA
		public override bool Equals(object other)
		{
			return other is HServerListRequest && this == (HServerListRequest)other;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0000FF07 File Offset: 0x0000E107
		public override int GetHashCode()
		{
			return this.m_HServerListRequest.GetHashCode();
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0000FF14 File Offset: 0x0000E114
		public static bool operator ==(HServerListRequest x, HServerListRequest y)
		{
			return x.m_HServerListRequest == y.m_HServerListRequest;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0000FF27 File Offset: 0x0000E127
		public static bool operator !=(HServerListRequest x, HServerListRequest y)
		{
			return !(x == y);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0000FF33 File Offset: 0x0000E133
		public static explicit operator HServerListRequest(IntPtr value)
		{
			return new HServerListRequest(value);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0000FF3B File Offset: 0x0000E13B
		public static explicit operator IntPtr(HServerListRequest that)
		{
			return that.m_HServerListRequest;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0000FF43 File Offset: 0x0000E143
		public bool Equals(HServerListRequest other)
		{
			return this.m_HServerListRequest == other.m_HServerListRequest;
		}

		// Token: 0x04000AF3 RID: 2803
		public static readonly HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);

		// Token: 0x04000AF4 RID: 2804
		public IntPtr m_HServerListRequest;
	}
}
