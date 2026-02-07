using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F7 RID: 1527
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNonPositiveInteger : ISoapXsd
	{
		// Token: 0x060039DB RID: 14811 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNonPositiveInteger()
		{
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x000CBF2B File Offset: 0x000CA12B
		public SoapNonPositiveInteger(decimal value)
		{
			if (value > 0m)
			{
				throw SoapHelper.GetException(this, "invalid " + value.ToString());
			}
			this._value = value;
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x000CBF5F File Offset: 0x000CA15F
		// (set) Token: 0x060039DE RID: 14814 RVA: 0x000CBF67 File Offset: 0x000CA167
		public decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x060039DF RID: 14815 RVA: 0x000CBF70 File Offset: 0x000CA170
		public static string XsdType
		{
			get
			{
				return "nonPositiveInteger";
			}
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x000CBF77 File Offset: 0x000CA177
		public string GetXsdType()
		{
			return SoapNonPositiveInteger.XsdType;
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x000CBF7E File Offset: 0x000CA17E
		public static SoapNonPositiveInteger Parse(string value)
		{
			return new SoapNonPositiveInteger(decimal.Parse(value));
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x000CBF8B File Offset: 0x000CA18B
		public override string ToString()
		{
			return this._value.ToString();
		}

		// Token: 0x0400263A RID: 9786
		private decimal _value;
	}
}
