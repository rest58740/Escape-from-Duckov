using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F3 RID: 1523
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNegativeInteger : ISoapXsd
	{
		// Token: 0x060039BB RID: 14779 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNegativeInteger()
		{
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x000CBDDB File Offset: 0x000C9FDB
		public SoapNegativeInteger(decimal value)
		{
			if (value >= 0m)
			{
				throw SoapHelper.GetException(this, "invalid " + value.ToString());
			}
			this._value = value;
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060039BD RID: 14781 RVA: 0x000CBE0F File Offset: 0x000CA00F
		// (set) Token: 0x060039BE RID: 14782 RVA: 0x000CBE17 File Offset: 0x000CA017
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

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060039BF RID: 14783 RVA: 0x000CBE20 File Offset: 0x000CA020
		public static string XsdType
		{
			get
			{
				return "negativeInteger";
			}
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x000CBE27 File Offset: 0x000CA027
		public string GetXsdType()
		{
			return SoapNegativeInteger.XsdType;
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x000CBE2E File Offset: 0x000CA02E
		public static SoapNegativeInteger Parse(string value)
		{
			return new SoapNegativeInteger(decimal.Parse(value));
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x000CBE3B File Offset: 0x000CA03B
		public override string ToString()
		{
			return this._value.ToString();
		}

		// Token: 0x04002636 RID: 9782
		private decimal _value;
	}
}
