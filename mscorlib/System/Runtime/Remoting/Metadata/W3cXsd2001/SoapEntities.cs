using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005E6 RID: 1510
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapEntities : ISoapXsd
	{
		// Token: 0x06003953 RID: 14675 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapEntities()
		{
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x000CB925 File Offset: 0x000C9B25
		public SoapEntities(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x000CB939 File Offset: 0x000C9B39
		// (set) Token: 0x06003956 RID: 14678 RVA: 0x000CB941 File Offset: 0x000C9B41
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

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06003957 RID: 14679 RVA: 0x000CB94A File Offset: 0x000C9B4A
		public static string XsdType
		{
			get
			{
				return "ENTITIES";
			}
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x000CB951 File Offset: 0x000C9B51
		public string GetXsdType()
		{
			return SoapEntities.XsdType;
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x000CB958 File Offset: 0x000C9B58
		public static SoapEntities Parse(string value)
		{
			return new SoapEntities(value);
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x000CB939 File Offset: 0x000C9B39
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x04002627 RID: 9767
		private string _value;
	}
}
