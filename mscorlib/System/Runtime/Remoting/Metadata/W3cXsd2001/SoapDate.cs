using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005E2 RID: 1506
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapDate : ISoapXsd
	{
		// Token: 0x06003935 RID: 14645 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapDate()
		{
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x000CB301 File Offset: 0x000C9501
		public SoapDate(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x000CB310 File Offset: 0x000C9510
		public SoapDate(DateTime value, int sign)
		{
			this._value = value;
			this._sign = sign;
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06003938 RID: 14648 RVA: 0x000CB326 File Offset: 0x000C9526
		// (set) Token: 0x06003939 RID: 14649 RVA: 0x000CB32E File Offset: 0x000C952E
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

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x0600393A RID: 14650 RVA: 0x000CB337 File Offset: 0x000C9537
		// (set) Token: 0x0600393B RID: 14651 RVA: 0x000CB33F File Offset: 0x000C953F
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

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x0600393C RID: 14652 RVA: 0x000CB348 File Offset: 0x000C9548
		public static string XsdType
		{
			get
			{
				return "date";
			}
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x000CB34F File Offset: 0x000C954F
		public string GetXsdType()
		{
			return SoapDate.XsdType;
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x000CB358 File Offset: 0x000C9558
		public static SoapDate Parse(string value)
		{
			SoapDate soapDate = new SoapDate(DateTime.ParseExact(value, SoapDate._datetimeFormats, null, DateTimeStyles.None));
			if (value.StartsWith("-"))
			{
				soapDate.Sign = -1;
			}
			else
			{
				soapDate.Sign = 0;
			}
			return soapDate;
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x000CB396 File Offset: 0x000C9596
		public override string ToString()
		{
			if (this._sign >= 0)
			{
				return this._value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			}
			return this._value.ToString("'-'yyyy-MM-dd", CultureInfo.InvariantCulture);
		}

		// Token: 0x04002621 RID: 9761
		private static readonly string[] _datetimeFormats = new string[]
		{
			"yyyy-MM-dd",
			"'+'yyyy-MM-dd",
			"'-'yyyy-MM-dd",
			"yyyy-MM-ddzzz",
			"'+'yyyy-MM-ddzzz",
			"'-'yyyy-MM-ddzzz"
		};

		// Token: 0x04002622 RID: 9762
		private int _sign;

		// Token: 0x04002623 RID: 9763
		private DateTime _value;
	}
}
