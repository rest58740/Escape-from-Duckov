using System;

namespace Steamworks
{
	// Token: 0x020001A3 RID: 419
	[Serializable]
	public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
	{
		// Token: 0x060009EC RID: 2540 RVA: 0x0000F85F File Offset: 0x0000DA5F
		public HHTMLBrowser(uint value)
		{
			this.m_HHTMLBrowser = value;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0000F868 File Offset: 0x0000DA68
		public override string ToString()
		{
			return this.m_HHTMLBrowser.ToString();
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0000F875 File Offset: 0x0000DA75
		public override bool Equals(object other)
		{
			return other is HHTMLBrowser && this == (HHTMLBrowser)other;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0000F892 File Offset: 0x0000DA92
		public override int GetHashCode()
		{
			return this.m_HHTMLBrowser.GetHashCode();
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0000F89F File Offset: 0x0000DA9F
		public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y)
		{
			return x.m_HHTMLBrowser == y.m_HHTMLBrowser;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0000F8AF File Offset: 0x0000DAAF
		public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y)
		{
			return !(x == y);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0000F8BB File Offset: 0x0000DABB
		public static explicit operator HHTMLBrowser(uint value)
		{
			return new HHTMLBrowser(value);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0000F8C3 File Offset: 0x0000DAC3
		public static explicit operator uint(HHTMLBrowser that)
		{
			return that.m_HHTMLBrowser;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0000F8CB File Offset: 0x0000DACB
		public bool Equals(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser == other.m_HHTMLBrowser;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0000F8DB File Offset: 0x0000DADB
		public int CompareTo(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);
		}

		// Token: 0x04000ADF RID: 2783
		public static readonly HHTMLBrowser Invalid = new HHTMLBrowser(0U);

		// Token: 0x04000AE0 RID: 2784
		public uint m_HHTMLBrowser;
	}
}
