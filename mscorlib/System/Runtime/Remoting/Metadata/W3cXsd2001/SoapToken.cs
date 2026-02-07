using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005FD RID: 1533
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapToken : ISoapXsd
	{
		// Token: 0x06003A12 RID: 14866 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapToken()
		{
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x000CC2C0 File Offset: 0x000CA4C0
		public SoapToken(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06003A14 RID: 14868 RVA: 0x000CC2D4 File Offset: 0x000CA4D4
		// (set) Token: 0x06003A15 RID: 14869 RVA: 0x000CC2DC File Offset: 0x000CA4DC
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

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06003A16 RID: 14870 RVA: 0x000CC2E5 File Offset: 0x000CA4E5
		public static string XsdType
		{
			get
			{
				return "token";
			}
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x000CC2EC File Offset: 0x000CA4EC
		public string GetXsdType()
		{
			return SoapToken.XsdType;
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x000CC2F3 File Offset: 0x000CA4F3
		public static SoapToken Parse(string value)
		{
			return new SoapToken(value);
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x000CC2D4 File Offset: 0x000CA4D4
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x04002643 RID: 9795
		private string _value;
	}
}
