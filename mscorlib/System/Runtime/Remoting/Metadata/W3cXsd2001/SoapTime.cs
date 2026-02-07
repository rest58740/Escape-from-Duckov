using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005FC RID: 1532
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapTime : ISoapXsd
	{
		// Token: 0x06003A09 RID: 14857 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapTime()
		{
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x000CC17C File Offset: 0x000CA37C
		public SoapTime(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06003A0B RID: 14859 RVA: 0x000CC18B File Offset: 0x000CA38B
		// (set) Token: 0x06003A0C RID: 14860 RVA: 0x000CC193 File Offset: 0x000CA393
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

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06003A0D RID: 14861 RVA: 0x000CC19C File Offset: 0x000CA39C
		public static string XsdType
		{
			get
			{
				return "time";
			}
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x000CC1A3 File Offset: 0x000CA3A3
		public string GetXsdType()
		{
			return SoapTime.XsdType;
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x000CC1AA File Offset: 0x000CA3AA
		public static SoapTime Parse(string value)
		{
			return new SoapTime(DateTime.ParseExact(value, SoapTime._datetimeFormats, null, DateTimeStyles.None));
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x000CC1BE File Offset: 0x000CA3BE
		public override string ToString()
		{
			return this._value.ToString("HH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture);
		}

		// Token: 0x04002641 RID: 9793
		private static readonly string[] _datetimeFormats = new string[]
		{
			"HH:mm:ss",
			"HH:mm:ss.f",
			"HH:mm:ss.ff",
			"HH:mm:ss.fff",
			"HH:mm:ss.ffff",
			"HH:mm:ss.fffff",
			"HH:mm:ss.ffffff",
			"HH:mm:ss.fffffff",
			"HH:mm:sszzz",
			"HH:mm:ss.fzzz",
			"HH:mm:ss.ffzzz",
			"HH:mm:ss.fffzzz",
			"HH:mm:ss.ffffzzz",
			"HH:mm:ss.fffffzzz",
			"HH:mm:ss.ffffffzzz",
			"HH:mm:ss.fffffffzzz",
			"HH:mm:ssZ",
			"HH:mm:ss.fZ",
			"HH:mm:ss.ffZ",
			"HH:mm:ss.fffZ",
			"HH:mm:ss.ffffZ",
			"HH:mm:ss.fffffZ",
			"HH:mm:ss.ffffffZ",
			"HH:mm:ss.fffffffZ"
		};

		// Token: 0x04002642 RID: 9794
		private DateTime _value;
	}
}
