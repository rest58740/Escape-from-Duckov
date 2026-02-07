using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005EA RID: 1514
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapId : ISoapXsd
	{
		// Token: 0x06003971 RID: 14705 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapId()
		{
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x000CBB45 File Offset: 0x000C9D45
		public SoapId(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06003973 RID: 14707 RVA: 0x000CBB59 File Offset: 0x000C9D59
		// (set) Token: 0x06003974 RID: 14708 RVA: 0x000CBB61 File Offset: 0x000C9D61
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

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06003975 RID: 14709 RVA: 0x000CBB6A File Offset: 0x000C9D6A
		public static string XsdType
		{
			get
			{
				return "ID";
			}
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x000CBB71 File Offset: 0x000C9D71
		public string GetXsdType()
		{
			return SoapId.XsdType;
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x000CBB78 File Offset: 0x000C9D78
		public static SoapId Parse(string value)
		{
			return new SoapId(value);
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x000CBB59 File Offset: 0x000C9D59
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x0400262B RID: 9771
		private string _value;
	}
}
