using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005EB RID: 1515
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapIdref : ISoapXsd
	{
		// Token: 0x06003979 RID: 14713 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapIdref()
		{
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x000CBB80 File Offset: 0x000C9D80
		public SoapIdref(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x0600397B RID: 14715 RVA: 0x000CBB94 File Offset: 0x000C9D94
		// (set) Token: 0x0600397C RID: 14716 RVA: 0x000CBB9C File Offset: 0x000C9D9C
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

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x0600397D RID: 14717 RVA: 0x000CBBA5 File Offset: 0x000C9DA5
		public static string XsdType
		{
			get
			{
				return "IDREF";
			}
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x000CBBAC File Offset: 0x000C9DAC
		public string GetXsdType()
		{
			return SoapIdref.XsdType;
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x000CBBB3 File Offset: 0x000C9DB3
		public static SoapIdref Parse(string value)
		{
			return new SoapIdref(value);
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x000CBB94 File Offset: 0x000C9D94
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x0400262C RID: 9772
		private string _value;
	}
}
