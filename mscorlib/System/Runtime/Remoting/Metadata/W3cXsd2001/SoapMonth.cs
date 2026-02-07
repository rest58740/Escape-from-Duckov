using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005EF RID: 1519
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapMonth : ISoapXsd
	{
		// Token: 0x06003999 RID: 14745 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapMonth()
		{
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x000CBC79 File Offset: 0x000C9E79
		public SoapMonth(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x0600399B RID: 14747 RVA: 0x000CBC88 File Offset: 0x000C9E88
		// (set) Token: 0x0600399C RID: 14748 RVA: 0x000CBC90 File Offset: 0x000C9E90
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

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x000CBC99 File Offset: 0x000C9E99
		public static string XsdType
		{
			get
			{
				return "gMonth";
			}
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x000CBCA0 File Offset: 0x000C9EA0
		public string GetXsdType()
		{
			return SoapMonth.XsdType;
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x000CBCA7 File Offset: 0x000C9EA7
		public static SoapMonth Parse(string value)
		{
			return new SoapMonth(DateTime.ParseExact(value, SoapMonth._datetimeFormats, null, DateTimeStyles.None));
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x000CBCBB File Offset: 0x000C9EBB
		public override string ToString()
		{
			return this._value.ToString("--MM--", CultureInfo.InvariantCulture);
		}

		// Token: 0x04002630 RID: 9776
		private static readonly string[] _datetimeFormats = new string[]
		{
			"--MM--",
			"--MM--zzz"
		};

		// Token: 0x04002631 RID: 9777
		private DateTime _value;
	}
}
