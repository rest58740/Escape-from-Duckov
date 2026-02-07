using System;
using System.Text;

namespace Sirenix.Utilities
{
	// Token: 0x02000036 RID: 54
	public static class StringUtilities
	{
		// Token: 0x0600023A RID: 570 RVA: 0x0000D0FC File Offset: 0x0000B2FC
		public static string NicifyByteSize(int bytes, int decimals = 1)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (bytes < 0)
			{
				stringBuilder.Append('-');
				bytes = Math.Abs(bytes);
			}
			int num;
			string text;
			if (bytes > 1000000000)
			{
				stringBuilder.Append(bytes / 1000000000);
				bytes -= bytes / 1000000000 * 1000000000;
				num = 9;
				text = " GB";
			}
			else if (bytes > 1000000)
			{
				stringBuilder.Append(bytes / 1000000);
				bytes -= bytes / 1000000 * 1000000;
				num = 6;
				text = " MB";
			}
			else if (bytes > 1000)
			{
				stringBuilder.Append(bytes / 1000);
				bytes -= bytes / 1000 * 1000;
				num = 3;
				text = " KB";
			}
			else
			{
				stringBuilder.Append(bytes);
				decimals = 0;
				num = 0;
				text = " bytes";
			}
			if (decimals > 0 && num > 0 && bytes > 0)
			{
				string text2 = bytes.ToString().PadLeft(num, '0');
				text2 = text2.Substring(0, (decimals < text2.Length) ? decimals : text2.Length).TrimEnd(new char[]
				{
					'0'
				});
				if (text2.Length > 0)
				{
					stringBuilder.Append('.');
					stringBuilder.Append(text2);
				}
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000D240 File Offset: 0x0000B440
		public static bool FastEndsWith(this string str, string endsWith)
		{
			if (str.Length < endsWith.Length)
			{
				return false;
			}
			for (int i = 0; i < endsWith.Length; i++)
			{
				if (str.get_Chars(str.Length - (1 + i)) != endsWith.get_Chars(endsWith.Length - (1 + i)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000D294 File Offset: 0x0000B494
		public static int NumberAwareStringCompare(string a, string b, bool ignoreLeadingZeroes = true, bool ignoreWhiteSpace = true, bool ignoreCase = false)
		{
			int length = a.Length;
			int length2 = b.Length;
			int i = 0;
			int num = 0;
			char c;
			char c2;
			int num3;
			int num4;
			char c3;
			char c4;
			for (;;)
			{
				bool flag = i == length;
				bool flag2 = num == length2;
				if (flag && flag2)
				{
					break;
				}
				if (flag)
				{
					return -1;
				}
				if (flag2)
				{
					return 1;
				}
				if (ignoreWhiteSpace)
				{
					while (i < length)
					{
						if (!char.IsWhiteSpace(a.get_Chars(i)))
						{
							break;
						}
						i++;
					}
					while (num < length2 && char.IsWhiteSpace(b.get_Chars(num)))
					{
						num++;
					}
				}
				c = a.get_Chars(i);
				c2 = b.get_Chars(num);
				if (char.IsDigit(c) && char.IsDigit(c2))
				{
					if (ignoreLeadingZeroes)
					{
						while (i < length)
						{
							if (a.get_Chars(i) != '0')
							{
								break;
							}
							i++;
						}
						while (num < length2 && b.get_Chars(num) == '0')
						{
							num++;
						}
					}
					int j = i;
					int num2 = num;
					while (j < length)
					{
						if (!char.IsDigit(a.get_Chars(j)))
						{
							break;
						}
						j++;
					}
					while (num2 < length2 && char.IsDigit(b.get_Chars(num2)))
					{
						num2++;
					}
					num3 = j - i;
					num4 = num2 - num;
					if (num3 != num4)
					{
						goto Block_19;
					}
					while (i < j)
					{
						if (a.get_Chars(i) != b.get_Chars(num))
						{
							goto Block_20;
						}
						i++;
						num++;
					}
				}
				else
				{
					if (ignoreCase)
					{
						if (c != c2)
						{
							c = char.ToLower(c);
							c2 = char.ToLower(c2);
							if (c != c2)
							{
								goto Block_24;
							}
						}
					}
					else
					{
						c3 = char.ToLower(c);
						c4 = char.ToLower(c2);
						if (c3 != c4)
						{
							goto IL_19D;
						}
						if (c != c2)
						{
							goto Block_26;
						}
					}
					i++;
					num++;
				}
			}
			if (length == length2)
			{
				return 0;
			}
			if (length < length2)
			{
				return -1;
			}
			return 1;
			Block_19:
			return num3 - num4;
			Block_20:
			return (int)(a.get_Chars(i) - b.get_Chars(num));
			Block_24:
			return (int)(c - c2);
			Block_26:
			return (int)(c2 - c);
			IL_19D:
			return (int)(c3 - c4);
		}
	}
}
