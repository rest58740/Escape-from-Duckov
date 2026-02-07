using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F8 RID: 1528
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNormalizedString : ISoapXsd
	{
		// Token: 0x060039E3 RID: 14819 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNormalizedString()
		{
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x000CBF98 File Offset: 0x000CA198
		public SoapNormalizedString(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x000CBFAC File Offset: 0x000CA1AC
		// (set) Token: 0x060039E6 RID: 14822 RVA: 0x000CBFB4 File Offset: 0x000CA1B4
		public string Value
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

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x060039E7 RID: 14823 RVA: 0x000CBFBD File Offset: 0x000CA1BD
		public static string XsdType
		{
			get
			{
				return "normalizedString";
			}
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x000CBFC4 File Offset: 0x000CA1C4
		public string GetXsdType()
		{
			return SoapNormalizedString.XsdType;
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x000CBFCB File Offset: 0x000CA1CB
		public static SoapNormalizedString Parse(string value)
		{
			return new SoapNormalizedString(value);
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x000CBFAC File Offset: 0x000CA1AC
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x0400263B RID: 9787
		private string _value;
	}
}
