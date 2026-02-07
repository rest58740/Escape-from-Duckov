using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F9 RID: 1529
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNotation : ISoapXsd
	{
		// Token: 0x060039EB RID: 14827 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNotation()
		{
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x000CBFD3 File Offset: 0x000CA1D3
		public SoapNotation(string value)
		{
			this._value = value;
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x000CBFE2 File Offset: 0x000CA1E2
		// (set) Token: 0x060039EE RID: 14830 RVA: 0x000CBFEA File Offset: 0x000CA1EA
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

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x060039EF RID: 14831 RVA: 0x000CBFF3 File Offset: 0x000CA1F3
		public static string XsdType
		{
			get
			{
				return "NOTATION";
			}
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x000CBFFA File Offset: 0x000CA1FA
		public string GetXsdType()
		{
			return SoapNotation.XsdType;
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x000CC001 File Offset: 0x000CA201
		public static SoapNotation Parse(string value)
		{
			return new SoapNotation(value);
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x000CBFE2 File Offset: 0x000CA1E2
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x0400263C RID: 9788
		private string _value;
	}
}
