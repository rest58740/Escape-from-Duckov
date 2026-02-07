using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005FF RID: 1535
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapYearMonth : ISoapXsd
	{
		// Token: 0x06003A26 RID: 14886 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapYearMonth()
		{
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x000CC401 File Offset: 0x000CA601
		public SoapYearMonth(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x000CC410 File Offset: 0x000CA610
		public SoapYearMonth(DateTime value, int sign)
		{
			this._value = value;
			this._sign = sign;
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06003A29 RID: 14889 RVA: 0x000CC426 File Offset: 0x000CA626
		// (set) Token: 0x06003A2A RID: 14890 RVA: 0x000CC42E File Offset: 0x000CA62E
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

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06003A2B RID: 14891 RVA: 0x000CC437 File Offset: 0x000CA637
		// (set) Token: 0x06003A2C RID: 14892 RVA: 0x000CC43F File Offset: 0x000CA63F
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

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06003A2D RID: 14893 RVA: 0x000CC448 File Offset: 0x000CA648
		public static string XsdType
		{
			get
			{
				return "gYearMonth";
			}
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x000CC44F File Offset: 0x000CA64F
		public string GetXsdType()
		{
			return SoapYearMonth.XsdType;
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x000CC458 File Offset: 0x000CA658
		public static SoapYearMonth Parse(string value)
		{
			SoapYearMonth soapYearMonth = new SoapYearMonth(DateTime.ParseExact(value, SoapYearMonth._datetimeFormats, null, DateTimeStyles.None));
			if (value.StartsWith("-"))
			{
				soapYearMonth.Sign = -1;
			}
			else
			{
				soapYearMonth.Sign = 0;
			}
			return soapYearMonth;
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x000CC496 File Offset: 0x000CA696
		public override string ToString()
		{
			if (this._sign >= 0)
			{
				return this._value.ToString("yyyy-MM", CultureInfo.InvariantCulture);
			}
			return this._value.ToString("'-'yyyy-MM", CultureInfo.InvariantCulture);
		}

		// Token: 0x04002647 RID: 9799
		private static readonly string[] _datetimeFormats = new string[]
		{
			"yyyy-MM",
			"'+'yyyy-MM",
			"'-'yyyy-MM",
			"yyyy-MMzzz",
			"'+'yyyy-MMzzz",
			"'-'yyyy-MMzzz"
		};

		// Token: 0x04002648 RID: 9800
		private int _sign;

		// Token: 0x04002649 RID: 9801
		private DateTime _value;
	}
}
