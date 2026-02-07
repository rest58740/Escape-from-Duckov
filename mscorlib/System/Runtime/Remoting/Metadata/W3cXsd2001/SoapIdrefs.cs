using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005EC RID: 1516
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapIdrefs : ISoapXsd
	{
		// Token: 0x06003981 RID: 14721 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapIdrefs()
		{
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x000CBBBB File Offset: 0x000C9DBB
		public SoapIdrefs(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06003983 RID: 14723 RVA: 0x000CBBCF File Offset: 0x000C9DCF
		// (set) Token: 0x06003984 RID: 14724 RVA: 0x000CBBD7 File Offset: 0x000C9DD7
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

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06003985 RID: 14725 RVA: 0x000CBBE0 File Offset: 0x000C9DE0
		public static string XsdType
		{
			get
			{
				return "IDREFS";
			}
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x000CBBE7 File Offset: 0x000C9DE7
		public string GetXsdType()
		{
			return SoapIdrefs.XsdType;
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x000CBBEE File Offset: 0x000C9DEE
		public static SoapIdrefs Parse(string value)
		{
			return new SoapIdrefs(value);
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x000CBBCF File Offset: 0x000C9DCF
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x0400262D RID: 9773
		private string _value;
	}
}
