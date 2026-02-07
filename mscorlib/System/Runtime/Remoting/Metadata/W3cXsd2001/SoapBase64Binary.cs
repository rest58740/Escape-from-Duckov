using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005E1 RID: 1505
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapBase64Binary : ISoapXsd
	{
		// Token: 0x0600392D RID: 14637 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapBase64Binary()
		{
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x000CB2B9 File Offset: 0x000C94B9
		public SoapBase64Binary(byte[] value)
		{
			this._value = value;
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x0600392F RID: 14639 RVA: 0x000CB2C8 File Offset: 0x000C94C8
		// (set) Token: 0x06003930 RID: 14640 RVA: 0x000CB2D0 File Offset: 0x000C94D0
		public byte[] Value
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

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x000CB2D9 File Offset: 0x000C94D9
		public static string XsdType
		{
			get
			{
				return "base64Binary";
			}
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x000CB2E0 File Offset: 0x000C94E0
		public string GetXsdType()
		{
			return SoapBase64Binary.XsdType;
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x000CB2E7 File Offset: 0x000C94E7
		public static SoapBase64Binary Parse(string value)
		{
			return new SoapBase64Binary(Convert.FromBase64String(value));
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x000CB2F4 File Offset: 0x000C94F4
		public override string ToString()
		{
			return Convert.ToBase64String(this._value);
		}

		// Token: 0x04002620 RID: 9760
		private byte[] _value;
	}
}
