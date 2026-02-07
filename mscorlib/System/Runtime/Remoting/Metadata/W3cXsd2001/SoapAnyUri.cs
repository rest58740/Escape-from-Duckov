using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005E0 RID: 1504
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapAnyUri : ISoapXsd
	{
		// Token: 0x06003925 RID: 14629 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapAnyUri()
		{
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x000CB283 File Offset: 0x000C9483
		public SoapAnyUri(string value)
		{
			this._value = value;
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06003927 RID: 14631 RVA: 0x000CB292 File Offset: 0x000C9492
		// (set) Token: 0x06003928 RID: 14632 RVA: 0x000CB29A File Offset: 0x000C949A
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

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06003929 RID: 14633 RVA: 0x000CB2A3 File Offset: 0x000C94A3
		public static string XsdType
		{
			get
			{
				return "anyUri";
			}
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x000CB2AA File Offset: 0x000C94AA
		public string GetXsdType()
		{
			return SoapAnyUri.XsdType;
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x000CB2B1 File Offset: 0x000C94B1
		public static SoapAnyUri Parse(string value)
		{
			return new SoapAnyUri(value);
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x000CB292 File Offset: 0x000C9492
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x0400261F RID: 9759
		private string _value;
	}
}
