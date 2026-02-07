using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005E7 RID: 1511
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapEntity : ISoapXsd
	{
		// Token: 0x0600395B RID: 14683 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapEntity()
		{
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x000CB960 File Offset: 0x000C9B60
		public SoapEntity(string value)
		{
			this._value = SoapHelper.Normalize(value);
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x0600395D RID: 14685 RVA: 0x000CB974 File Offset: 0x000C9B74
		// (set) Token: 0x0600395E RID: 14686 RVA: 0x000CB97C File Offset: 0x000C9B7C
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

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x0600395F RID: 14687 RVA: 0x000CB985 File Offset: 0x000C9B85
		public static string XsdType
		{
			get
			{
				return "ENTITY";
			}
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x000CB98C File Offset: 0x000C9B8C
		public string GetXsdType()
		{
			return SoapEntity.XsdType;
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x000CB993 File Offset: 0x000C9B93
		public static SoapEntity Parse(string value)
		{
			return new SoapEntity(value);
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x000CB974 File Offset: 0x000C9B74
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x04002628 RID: 9768
		private string _value;
	}
}
