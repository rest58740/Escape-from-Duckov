using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005F0 RID: 1520
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapMonthDay : ISoapXsd
	{
		// Token: 0x060039A2 RID: 14754 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapMonthDay()
		{
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x000CBCEF File Offset: 0x000C9EEF
		public SoapMonthDay(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x060039A4 RID: 14756 RVA: 0x000CBCFE File Offset: 0x000C9EFE
		// (set) Token: 0x060039A5 RID: 14757 RVA: 0x000CBD06 File Offset: 0x000C9F06
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

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x060039A6 RID: 14758 RVA: 0x000CBD0F File Offset: 0x000C9F0F
		public static string XsdType
		{
			get
			{
				return "gMonthDay";
			}
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x000CBD16 File Offset: 0x000C9F16
		public string GetXsdType()
		{
			return SoapMonthDay.XsdType;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x000CBD1D File Offset: 0x000C9F1D
		public static SoapMonthDay Parse(string value)
		{
			return new SoapMonthDay(DateTime.ParseExact(value, SoapMonthDay._datetimeFormats, null, DateTimeStyles.None));
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x000CBD31 File Offset: 0x000C9F31
		public override string ToString()
		{
			return this._value.ToString("--MM-dd", CultureInfo.InvariantCulture);
		}

		// Token: 0x04002632 RID: 9778
		private static readonly string[] _datetimeFormats = new string[]
		{
			"--MM-dd",
			"--MM-ddzzz"
		};

		// Token: 0x04002633 RID: 9779
		private DateTime _value;
	}
}
