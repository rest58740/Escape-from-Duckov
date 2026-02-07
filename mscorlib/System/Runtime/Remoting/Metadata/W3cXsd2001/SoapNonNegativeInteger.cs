using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F6 RID: 1526
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNonNegativeInteger : ISoapXsd
	{
		// Token: 0x060039D3 RID: 14803 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNonNegativeInteger()
		{
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x000CBEBE File Offset: 0x000CA0BE
		public SoapNonNegativeInteger(decimal value)
		{
			if (value < 0m)
			{
				throw SoapHelper.GetException(this, "invalid " + value.ToString());
			}
			this._value = value;
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060039D5 RID: 14805 RVA: 0x000CBEF2 File Offset: 0x000CA0F2
		// (set) Token: 0x060039D6 RID: 14806 RVA: 0x000CBEFA File Offset: 0x000CA0FA
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

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x060039D7 RID: 14807 RVA: 0x000CBF03 File Offset: 0x000CA103
		public static string XsdType
		{
			get
			{
				return "nonNegativeInteger";
			}
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x000CBF0A File Offset: 0x000CA10A
		public string GetXsdType()
		{
			return SoapNonNegativeInteger.XsdType;
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x000CBF11 File Offset: 0x000CA111
		public static SoapNonNegativeInteger Parse(string value)
		{
			return new SoapNonNegativeInteger(decimal.Parse(value));
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x000CBF1E File Offset: 0x000CA11E
		public override string ToString()
		{
			return this._value.ToString();
		}

		// Token: 0x04002639 RID: 9785
		private decimal _value;
	}
}
