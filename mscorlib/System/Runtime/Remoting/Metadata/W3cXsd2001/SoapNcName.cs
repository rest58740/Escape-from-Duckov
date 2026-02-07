using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F2 RID: 1522
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNcName : ISoapXsd
	{
		// Token: 0x060039B3 RID: 14771 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNcName()
		{
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x000CBDA0 File Offset: 0x000C9FA0
		public SoapNcName(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060039B5 RID: 14773 RVA: 0x000CBDB4 File Offset: 0x000C9FB4
		// (set) Token: 0x060039B6 RID: 14774 RVA: 0x000CBDBC File Offset: 0x000C9FBC
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

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060039B7 RID: 14775 RVA: 0x000CBDC5 File Offset: 0x000C9FC5
		public static string XsdType
		{
			get
			{
				return "NCName";
			}
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x000CBDCC File Offset: 0x000C9FCC
		public string GetXsdType()
		{
			return SoapNcName.XsdType;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x000CBDD3 File Offset: 0x000C9FD3
		public static SoapNcName Parse(string value)
		{
			return new SoapNcName(value);
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x000CBDB4 File Offset: 0x000C9FB4
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x04002635 RID: 9781
		private string _value;
	}
}
