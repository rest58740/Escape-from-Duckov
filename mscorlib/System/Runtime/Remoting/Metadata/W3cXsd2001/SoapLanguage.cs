using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005EE RID: 1518
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapLanguage : ISoapXsd
	{
		// Token: 0x06003991 RID: 14737 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapLanguage()
		{
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x000CBC3E File Offset: 0x000C9E3E
		public SoapLanguage(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06003993 RID: 14739 RVA: 0x000CBC52 File Offset: 0x000C9E52
		// (set) Token: 0x06003994 RID: 14740 RVA: 0x000CBC5A File Offset: 0x000C9E5A
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

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x000CBC63 File Offset: 0x000C9E63
		public static string XsdType
		{
			get
			{
				return "language";
			}
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x000CBC6A File Offset: 0x000C9E6A
		public string GetXsdType()
		{
			return SoapLanguage.XsdType;
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x000CBC71 File Offset: 0x000C9E71
		public static SoapLanguage Parse(string value)
		{
			return new SoapLanguage(value);
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x000CBC52 File Offset: 0x000C9E52
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x0400262F RID: 9775
		private string _value;
	}
}
