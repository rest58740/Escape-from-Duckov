using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F5 RID: 1525
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNmtokens : ISoapXsd
	{
		// Token: 0x060039CB RID: 14795 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNmtokens()
		{
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x000CBE83 File Offset: 0x000CA083
		public SoapNmtokens(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060039CD RID: 14797 RVA: 0x000CBE97 File Offset: 0x000CA097
		// (set) Token: 0x060039CE RID: 14798 RVA: 0x000CBE9F File Offset: 0x000CA09F
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

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060039CF RID: 14799 RVA: 0x000CBEA8 File Offset: 0x000CA0A8
		public static string XsdType
		{
			get
			{
				return "NMTOKENS";
			}
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x000CBEAF File Offset: 0x000CA0AF
		public string GetXsdType()
		{
			return SoapNmtokens.XsdType;
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x000CBEB6 File Offset: 0x000CA0B6
		public static SoapNmtokens Parse(string value)
		{
			return new SoapNmtokens(value);
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x000CBE97 File Offset: 0x000CA097
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x04002638 RID: 9784
		private string _value;
	}
}
