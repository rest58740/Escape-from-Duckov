using System;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000034 RID: 52
	internal static class Token
	{
		// Token: 0x06000168 RID: 360 RVA: 0x00005F1C File Offset: 0x0000411C
		public static bool IsLiteral(string token)
		{
			return token.StartsWith("_", StringComparison.Ordinal) || token.StartsWith("\\", StringComparison.Ordinal) || token.StartsWith("\"", StringComparison.Ordinal) || token.StartsWith("*", StringComparison.Ordinal) || token == "," || token == "!" || token == "&" || token == "%" || token == "+" || token == "-" || token == "$" || token == "€" || token == "£" || token == "1" || token == "2" || token == "3" || token == "4" || token == "5" || token == "6" || token == "7" || token == "8" || token == "9" || token == "{" || token == "}" || token == "(" || token == ")" || token == " ";
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000060BF File Offset: 0x000042BF
		public static bool IsNumberLiteral(string token)
		{
			return Token.IsPlaceholder(token) || Token.IsLiteral(token) || token == ".";
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000060DE File Offset: 0x000042DE
		public static bool IsPlaceholder(string token)
		{
			return token == "0" || token == "#" || token == "?";
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006107 File Offset: 0x00004307
		public static bool IsGeneral(string token)
		{
			return string.Compare(token, "general", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006118 File Offset: 0x00004318
		public static bool IsDatePart(string token)
		{
			return token.StartsWith("y", StringComparison.OrdinalIgnoreCase) || token.StartsWith("m", StringComparison.OrdinalIgnoreCase) || token.StartsWith("d", StringComparison.OrdinalIgnoreCase) || token.StartsWith("s", StringComparison.OrdinalIgnoreCase) || token.StartsWith("h", StringComparison.OrdinalIgnoreCase) || (token.StartsWith("g", StringComparison.OrdinalIgnoreCase) && !Token.IsGeneral(token)) || string.Compare(token, "am/pm", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(token, "a/p", StringComparison.OrdinalIgnoreCase) == 0 || Token.IsDurationPart(token);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000061A5 File Offset: 0x000043A5
		public static bool IsDurationPart(string token)
		{
			return token.StartsWith("[h", StringComparison.OrdinalIgnoreCase) || token.StartsWith("[m", StringComparison.OrdinalIgnoreCase) || token.StartsWith("[s", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000061D1 File Offset: 0x000043D1
		public static bool IsDigit09(string token)
		{
			return token == "0" || Token.IsDigit19(token);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000061E8 File Offset: 0x000043E8
		public static bool IsDigit19(string token)
		{
			if (token != null)
			{
				int length = token.Length;
				if (length == 1)
				{
					switch (token[0])
					{
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						return true;
					}
				}
			}
			return false;
		}
	}
}
