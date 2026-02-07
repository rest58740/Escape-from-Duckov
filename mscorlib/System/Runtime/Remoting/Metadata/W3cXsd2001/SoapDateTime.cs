using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005E3 RID: 1507
	[ComVisible(true)]
	public sealed class SoapDateTime
	{
		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06003942 RID: 14658 RVA: 0x000CB409 File Offset: 0x000C9609
		public static string XsdType
		{
			get
			{
				return "dateTime";
			}
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x000CB410 File Offset: 0x000C9610
		public static DateTime Parse(string value)
		{
			return DateTime.ParseExact(value, SoapDateTime._datetimeFormats, null, DateTimeStyles.None);
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x000CB41F File Offset: 0x000C961F
		public static string ToString(DateTime value)
		{
			return value.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture);
		}

		// Token: 0x04002624 RID: 9764
		private static readonly string[] _datetimeFormats = new string[]
		{
			"yyyy-MM-ddTHH:mm:ss",
			"yyyy-MM-ddTHH:mm:ss.f",
			"yyyy-MM-ddTHH:mm:ss.ff",
			"yyyy-MM-ddTHH:mm:ss.fff",
			"yyyy-MM-ddTHH:mm:ss.ffff",
			"yyyy-MM-ddTHH:mm:ss.fffff",
			"yyyy-MM-ddTHH:mm:ss.ffffff",
			"yyyy-MM-ddTHH:mm:ss.fffffff",
			"yyyy-MM-ddTHH:mm:sszzz",
			"yyyy-MM-ddTHH:mm:ss.fzzz",
			"yyyy-MM-ddTHH:mm:ss.ffzzz",
			"yyyy-MM-ddTHH:mm:ss.fffzzz",
			"yyyy-MM-ddTHH:mm:ss.ffffzzz",
			"yyyy-MM-ddTHH:mm:ss.fffffzzz",
			"yyyy-MM-ddTHH:mm:ss.ffffffzzz",
			"yyyy-MM-ddTHH:mm:ss.fffffffzzz",
			"yyyy-MM-ddTHH:mm:ssZ",
			"yyyy-MM-ddTHH:mm:ss.fZ",
			"yyyy-MM-ddTHH:mm:ss.ffZ",
			"yyyy-MM-ddTHH:mm:ss.fffZ",
			"yyyy-MM-ddTHH:mm:ss.ffffZ",
			"yyyy-MM-ddTHH:mm:ss.fffffZ",
			"yyyy-MM-ddTHH:mm:ss.ffffffZ",
			"yyyy-MM-ddTHH:mm:ss.fffffffZ"
		};
	}
}
