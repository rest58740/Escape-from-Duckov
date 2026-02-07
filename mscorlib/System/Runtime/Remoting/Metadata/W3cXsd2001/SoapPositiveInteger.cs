using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005FA RID: 1530
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapPositiveInteger : ISoapXsd
	{
		// Token: 0x060039F3 RID: 14835 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapPositiveInteger()
		{
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x000CC009 File Offset: 0x000CA209
		public SoapPositiveInteger(decimal value)
		{
			if (value <= 0m)
			{
				throw SoapHelper.GetException(this, "invalid " + value.ToString());
			}
			this._value = value;
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x060039F5 RID: 14837 RVA: 0x000CC03D File Offset: 0x000CA23D
		// (set) Token: 0x060039F6 RID: 14838 RVA: 0x000CC045 File Offset: 0x000CA245
		public decimal Value
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

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x060039F7 RID: 14839 RVA: 0x000CC04E File Offset: 0x000CA24E
		public static string XsdType
		{
			get
			{
				return "positiveInteger";
			}
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x000CC055 File Offset: 0x000CA255
		public string GetXsdType()
		{
			return SoapPositiveInteger.XsdType;
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x000CC05C File Offset: 0x000CA25C
		public static SoapPositiveInteger Parse(string value)
		{
			return new SoapPositiveInteger(decimal.Parse(value));
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x000CC069 File Offset: 0x000CA269
		public override string ToString()
		{
			return this._value.ToString();
		}

		// Token: 0x0400263D RID: 9789
		private decimal _value;
	}
}
