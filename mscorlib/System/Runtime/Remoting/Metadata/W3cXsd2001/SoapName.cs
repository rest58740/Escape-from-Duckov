using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F1 RID: 1521
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapName : ISoapXsd
	{
		// Token: 0x060039AB RID: 14763 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapName()
		{
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x000CBD65 File Offset: 0x000C9F65
		public SoapName(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060039AD RID: 14765 RVA: 0x000CBD79 File Offset: 0x000C9F79
		// (set) Token: 0x060039AE RID: 14766 RVA: 0x000CBD81 File Offset: 0x000C9F81
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

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060039AF RID: 14767 RVA: 0x000CBD8A File Offset: 0x000C9F8A
		public static string XsdType
		{
			get
			{
				return "Name";
			}
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x000CBD91 File Offset: 0x000C9F91
		public string GetXsdType()
		{
			return SoapName.XsdType;
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x000CBD98 File Offset: 0x000C9F98
		public static SoapName Parse(string value)
		{
			return new SoapName(value);
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x000CBD79 File Offset: 0x000C9F79
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x04002634 RID: 9780
		private string _value;
	}
}
