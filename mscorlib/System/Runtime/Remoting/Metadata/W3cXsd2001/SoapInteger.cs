using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005ED RID: 1517
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapInteger : ISoapXsd
	{
		// Token: 0x06003989 RID: 14729 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapInteger()
		{
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x000CBBF6 File Offset: 0x000C9DF6
		public SoapInteger(decimal value)
		{
			this._value = value;
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x0600398B RID: 14731 RVA: 0x000CBC05 File Offset: 0x000C9E05
		// (set) Token: 0x0600398C RID: 14732 RVA: 0x000CBC0D File Offset: 0x000C9E0D
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

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x0600398D RID: 14733 RVA: 0x000CBC16 File Offset: 0x000C9E16
		public static string XsdType
		{
			get
			{
				return "integer";
			}
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x000CBC1D File Offset: 0x000C9E1D
		public string GetXsdType()
		{
			return SoapInteger.XsdType;
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x000CBC24 File Offset: 0x000C9E24
		public static SoapInteger Parse(string value)
		{
			return new SoapInteger(decimal.Parse(value));
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x000CBC31 File Offset: 0x000C9E31
		public override string ToString()
		{
			return this._value.ToString();
		}

		// Token: 0x0400262E RID: 9774
		private decimal _value;
	}
}
