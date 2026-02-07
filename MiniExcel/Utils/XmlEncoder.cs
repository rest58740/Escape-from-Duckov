using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace MiniExcelLibs.Utils
{
	// Token: 0x0200003C RID: 60
	internal static class XmlEncoder
	{
		// Token: 0x06000185 RID: 389 RVA: 0x00006C54 File Offset: 0x00004E54
		public static StringBuilder EncodeString(string encodeStr)
		{
			if (encodeStr == null)
			{
				return null;
			}
			encodeStr = XmlEncoder.xHHHHRegex.Replace(encodeStr, "_x005F_$1_");
			StringBuilder stringBuilder = new StringBuilder(encodeStr.Length);
			foreach (char c in encodeStr)
			{
				if (XmlConvert.IsXmlChar(c))
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append(XmlConvert.EncodeName(c.ToString()));
				}
			}
			return stringBuilder;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006CC5 File Offset: 0x00004EC5
		public static string DecodeString(string decodeStr)
		{
			if (string.IsNullOrEmpty(decodeStr))
			{
				return string.Empty;
			}
			decodeStr = XmlEncoder.Uppercase_X_HHHHRegex.Replace(decodeStr, "_x005F_$1_");
			return XmlConvert.DecodeName(decodeStr);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006CED File Offset: 0x00004EED
		public static string ConvertEscapeChars(string input)
		{
			return XmlEncoder.EscapeRegex.Replace(input, (Match m) => ((char)uint.Parse(m.Groups[1].Value, NumberStyles.HexNumber)).ToString());
		}

		// Token: 0x0400008B RID: 139
		private static readonly Regex xHHHHRegex = new Regex("_(x[\\dA-Fa-f]{4})_", RegexOptions.Compiled);

		// Token: 0x0400008C RID: 140
		private static readonly Regex Uppercase_X_HHHHRegex = new Regex("_(X[\\dA-Fa-f]{4})_", RegexOptions.Compiled);

		// Token: 0x0400008D RID: 141
		private static readonly Regex EscapeRegex = new Regex("_x([0-9A-F]{4,4})_");
	}
}
