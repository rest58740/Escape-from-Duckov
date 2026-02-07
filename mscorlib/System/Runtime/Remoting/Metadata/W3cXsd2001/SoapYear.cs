using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005FE RID: 1534
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapYear : ISoapXsd
	{
		// Token: 0x06003A1A RID: 14874 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapYear()
		{
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x000CC2FB File Offset: 0x000CA4FB
		public SoapYear(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x000CC30A File Offset: 0x000CA50A
		public SoapYear(DateTime value, int sign)
		{
			this._value = value;
			this._sign = sign;
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x000CC320 File Offset: 0x000CA520
		// (set) Token: 0x06003A1E RID: 14878 RVA: 0x000CC328 File Offset: 0x000CA528
		public int Sign
		{
			get
			{
				return this._sign;
			}
			set
			{
				this._sign = value;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06003A1F RID: 14879 RVA: 0x000CC331 File Offset: 0x000CA531
		// (set) Token: 0x06003A20 RID: 14880 RVA: 0x000CC339 File Offset: 0x000CA539
		public DateTime Value
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

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06003A21 RID: 14881 RVA: 0x000CC342 File Offset: 0x000CA542
		public static string XsdType
		{
			get
			{
				return "gYear";
			}
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x000CC349 File Offset: 0x000CA549
		public string GetXsdType()
		{
			return SoapYear.XsdType;
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x000CC350 File Offset: 0x000CA550
		public static SoapYear Parse(string value)
		{
			SoapYear soapYear = new SoapYear(DateTime.ParseExact(value, SoapYear._datetimeFormats, null, DateTimeStyles.None));
			if (value.StartsWith("-"))
			{
				soapYear.Sign = -1;
			}
			else
			{
				soapYear.Sign = 0;
			}
			return soapYear;
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x000CC38E File Offset: 0x000CA58E
		public override string ToString()
		{
			if (this._sign >= 0)
			{
				return this._value.ToString("yyyy", CultureInfo.InvariantCulture);
			}
			return this._value.ToString("'-'yyyy", CultureInfo.InvariantCulture);
		}

		// Token: 0x04002644 RID: 9796
		private static readonly string[] _datetimeFormats = new string[]
		{
			"yyyy",
			"'+'yyyy",
			"'-'yyyy",
			"yyyyzzz",
			"'+'yyyyzzz",
			"'-'yyyyzzz"
		};

		// Token: 0x04002645 RID: 9797
		private int _sign;

		// Token: 0x04002646 RID: 9798
		private DateTime _value;
	}
}
