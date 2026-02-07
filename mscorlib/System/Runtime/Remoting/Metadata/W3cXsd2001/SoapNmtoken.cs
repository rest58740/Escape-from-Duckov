using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F4 RID: 1524
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNmtoken : ISoapXsd
	{
		// Token: 0x060039C3 RID: 14787 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapNmtoken()
		{
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x000CBE48 File Offset: 0x000CA048
		public SoapNmtoken(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x060039C5 RID: 14789 RVA: 0x000CBE5C File Offset: 0x000CA05C
		// (set) Token: 0x060039C6 RID: 14790 RVA: 0x000CBE64 File Offset: 0x000CA064
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

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060039C7 RID: 14791 RVA: 0x000CBE6D File Offset: 0x000CA06D
		public static string XsdType
		{
			get
			{
				return "NMTOKEN";
			}
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x000CBE74 File Offset: 0x000CA074
		public string GetXsdType()
		{
			return SoapNmtoken.XsdType;
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x000CBE7B File Offset: 0x000CA07B
		public static SoapNmtoken Parse(string value)
		{
			return new SoapNmtoken(value);
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x000CBE5C File Offset: 0x000CA05C
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x04002637 RID: 9783
		private string _value;
	}
}
