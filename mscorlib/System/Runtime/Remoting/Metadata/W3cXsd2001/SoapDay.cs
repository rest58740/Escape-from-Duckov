using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005E4 RID: 1508
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapDay : ISoapXsd
	{
		// Token: 0x06003946 RID: 14662 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapDay()
		{
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x000CB51C File Offset: 0x000C971C
		public SoapDay(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06003948 RID: 14664 RVA: 0x000CB52B File Offset: 0x000C972B
		// (set) Token: 0x06003949 RID: 14665 RVA: 0x000CB533 File Offset: 0x000C9733
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

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x0600394A RID: 14666 RVA: 0x000CB53C File Offset: 0x000C973C
		public static string XsdType
		{
			get
			{
				return "gDay";
			}
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x000CB543 File Offset: 0x000C9743
		public string GetXsdType()
		{
			return SoapDay.XsdType;
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x000CB54A File Offset: 0x000C974A
		public static SoapDay Parse(string value)
		{
			return new SoapDay(DateTime.ParseExact(value, SoapDay._datetimeFormats, null, DateTimeStyles.None));
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x000CB55E File Offset: 0x000C975E
		public override string ToString()
		{
			return this._value.ToString("---dd", CultureInfo.InvariantCulture);
		}

		// Token: 0x04002625 RID: 9765
		private static readonly string[] _datetimeFormats = new string[]
		{
			"---dd",
			"---ddzzz"
		};

		// Token: 0x04002626 RID: 9766
		private DateTime _value;
	}
}
