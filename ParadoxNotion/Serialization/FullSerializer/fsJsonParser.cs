using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000A8 RID: 168
	public class fsJsonParser
	{
		// Token: 0x06000636 RID: 1590 RVA: 0x00011CAC File Offset: 0x0000FEAC
		private fsResult MakeFailure(string message)
		{
			int num = Math.Max(0, this._start - 20);
			int num2 = Math.Min(50, this._input.Length - num);
			return fsResult.Fail(string.Concat(new string[]
			{
				"Error while parsing: ",
				message,
				"; context = <",
				this._input.Substring(num, num2),
				">"
			}));
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00011D1A File Offset: 0x0000FF1A
		private bool TryMoveNext()
		{
			if (this._start < this._input.Length)
			{
				this._start++;
				return true;
			}
			return false;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00011D40 File Offset: 0x0000FF40
		private bool HasValue()
		{
			return this.HasValue(0);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00011D49 File Offset: 0x0000FF49
		private bool HasValue(int offset)
		{
			return this._start + offset >= 0 && this._start + offset < this._input.Length;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00011D6D File Offset: 0x0000FF6D
		private char Character()
		{
			return this.Character(0);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00011D76 File Offset: 0x0000FF76
		private char Character(int offset)
		{
			return this._input.get_Chars(this._start + offset);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00011D8C File Offset: 0x0000FF8C
		private void SkipSpace()
		{
			while (this.HasValue())
			{
				if (char.IsWhiteSpace(this.Character()))
				{
					this.TryMoveNext();
				}
				else
				{
					if (!this.HasValue(1) || this.Character(0) != '/')
					{
						break;
					}
					if (this.Character(1) == '/')
					{
						while (this.HasValue())
						{
							if (Environment.NewLine.Contains(this.Character().ToString() ?? ""))
							{
								break;
							}
							this.TryMoveNext();
						}
					}
					else if (this.Character(1) == '*')
					{
						this.TryMoveNext();
						this.TryMoveNext();
						while (this.HasValue(1))
						{
							if (this.Character(0) == '*' && this.Character(1) == '/')
							{
								this.TryMoveNext();
								this.TryMoveNext();
								this.TryMoveNext();
								break;
							}
							this.TryMoveNext();
						}
					}
				}
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00011E75 File Offset: 0x00010075
		private bool IsHex(char c)
		{
			return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00011E9C File Offset: 0x0001009C
		private uint ParseSingleChar(char c1, uint multipliyer)
		{
			uint result = 0U;
			if (c1 >= '0' && c1 <= '9')
			{
				result = (uint)(c1 - '0') * multipliyer;
			}
			else if (c1 >= 'A' && c1 <= 'F')
			{
				result = (uint)(c1 - 'A' + '\n') * multipliyer;
			}
			else if (c1 >= 'a' && c1 <= 'f')
			{
				result = (uint)(c1 - 'a' + '\n') * multipliyer;
			}
			return result;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00011EEC File Offset: 0x000100EC
		private uint ParseUnicode(char c1, char c2, char c3, char c4)
		{
			uint num = this.ParseSingleChar(c1, 4096U);
			uint num2 = this.ParseSingleChar(c2, 256U);
			uint num3 = this.ParseSingleChar(c3, 16U);
			uint num4 = this.ParseSingleChar(c4, 1U);
			return num + num2 + num3 + num4;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00011F2C File Offset: 0x0001012C
		private fsResult TryUnescapeChar(out char escaped)
		{
			this.TryMoveNext();
			if (!this.HasValue())
			{
				escaped = ' ';
				return this.MakeFailure("Unexpected end of input after \\");
			}
			char c = this.Character();
			if (c <= '\\')
			{
				if (c <= '/')
				{
					if (c == '"')
					{
						this.TryMoveNext();
						escaped = '"';
						return fsResult.Success;
					}
					if (c == '/')
					{
						this.TryMoveNext();
						escaped = '/';
						return fsResult.Success;
					}
				}
				else
				{
					if (c == '0')
					{
						this.TryMoveNext();
						escaped = '\0';
						return fsResult.Success;
					}
					if (c == '\\')
					{
						this.TryMoveNext();
						escaped = '\\';
						return fsResult.Success;
					}
				}
			}
			else if (c <= 'b')
			{
				if (c == 'a')
				{
					this.TryMoveNext();
					escaped = '\a';
					return fsResult.Success;
				}
				if (c == 'b')
				{
					this.TryMoveNext();
					escaped = '\b';
					return fsResult.Success;
				}
			}
			else
			{
				if (c == 'f')
				{
					this.TryMoveNext();
					escaped = '\f';
					return fsResult.Success;
				}
				if (c == 'n')
				{
					this.TryMoveNext();
					escaped = '\n';
					return fsResult.Success;
				}
				switch (c)
				{
				case 'r':
					this.TryMoveNext();
					escaped = '\r';
					return fsResult.Success;
				case 't':
					this.TryMoveNext();
					escaped = '\t';
					return fsResult.Success;
				case 'u':
					this.TryMoveNext();
					if (this.IsHex(this.Character(0)) && this.IsHex(this.Character(1)) && this.IsHex(this.Character(2)) && this.IsHex(this.Character(3)))
					{
						uint num = this.ParseUnicode(this.Character(0), this.Character(1), this.Character(2), this.Character(3));
						this.TryMoveNext();
						this.TryMoveNext();
						this.TryMoveNext();
						this.TryMoveNext();
						escaped = (char)num;
						return fsResult.Success;
					}
					escaped = '\0';
					return this.MakeFailure(string.Format("invalid escape sequence '\\u{0}{1}{2}{3}'\n", new object[]
					{
						this.Character(0),
						this.Character(1),
						this.Character(2),
						this.Character(3)
					}));
				}
			}
			escaped = '\0';
			return this.MakeFailure(string.Format("Invalid escape sequence \\{0}", this.Character()));
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00012170 File Offset: 0x00010370
		private fsResult TryParseExact(string content)
		{
			for (int i = 0; i < content.Length; i++)
			{
				if (this.Character() != content.get_Chars(i))
				{
					return this.MakeFailure("Expected " + content.get_Chars(i).ToString());
				}
				if (!this.TryMoveNext())
				{
					return this.MakeFailure("Unexpected end of content when parsing " + content);
				}
			}
			return fsResult.Success;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000121DC File Offset: 0x000103DC
		private fsResult TryParseTrue(out fsData data)
		{
			fsResult result = this.TryParseExact("true");
			if (result.Succeeded)
			{
				data = new fsData(true);
				return fsResult.Success;
			}
			data = null;
			return result;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00012210 File Offset: 0x00010410
		private fsResult TryParseFalse(out fsData data)
		{
			fsResult result = this.TryParseExact("false");
			if (result.Succeeded)
			{
				data = new fsData(false);
				return fsResult.Success;
			}
			data = null;
			return result;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00012244 File Offset: 0x00010444
		private fsResult TryParseNull(out fsData data)
		{
			fsResult result = this.TryParseExact("null");
			if (result.Succeeded)
			{
				data = new fsData();
				return fsResult.Success;
			}
			data = null;
			return result;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00012277 File Offset: 0x00010477
		private bool IsSeparator(char c)
		{
			return char.IsWhiteSpace(c) || c == ',' || c == '}' || c == ']';
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00012294 File Offset: 0x00010494
		private fsResult TryParseNumber(out fsData data)
		{
			int start = this._start;
			while (this.TryMoveNext() && this.HasValue() && !this.IsSeparator(this.Character()))
			{
			}
			string text = this._input.Substring(start, this._start - start);
			if (text.Contains(".") || text.Contains("e") || text.Contains("E") || text == "Infinity" || text == "-Infinity" || text == "NaN")
			{
				double f;
				if (!double.TryParse(text, 511, CultureInfo.InvariantCulture, ref f))
				{
					data = null;
					return this.MakeFailure("Bad double format with " + text);
				}
				data = new fsData(f);
				return fsResult.Success;
			}
			else
			{
				long i;
				if (!long.TryParse(text, 511, CultureInfo.InvariantCulture, ref i))
				{
					data = null;
					return this.MakeFailure("Bad Int64 format with " + text);
				}
				data = new fsData(i);
				return fsResult.Success;
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00012398 File Offset: 0x00010598
		private fsResult TryParseString(out string str)
		{
			this._cachedStringBuilder.Length = 0;
			if (this.Character() != '"' || !this.TryMoveNext())
			{
				str = string.Empty;
				return this.MakeFailure("Expected initial \" when parsing a string");
			}
			while (this.HasValue() && this.Character() != '"')
			{
				char c = this.Character();
				if (c == '\\')
				{
					char c2;
					fsResult result = this.TryUnescapeChar(out c2);
					if (result.Failed)
					{
						str = string.Empty;
						return result;
					}
					this._cachedStringBuilder.Append(c2);
				}
				else
				{
					this._cachedStringBuilder.Append(c);
					if (!this.TryMoveNext())
					{
						str = string.Empty;
						return this.MakeFailure("Unexpected end of input when reading a string");
					}
				}
			}
			if (!this.HasValue() || this.Character() != '"' || !this.TryMoveNext())
			{
				str = string.Empty;
				return this.MakeFailure("No closing \" when parsing a string");
			}
			str = this._cachedStringBuilder.ToString();
			return fsResult.Success;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00012488 File Offset: 0x00010688
		private fsResult TryParseArray(out fsData arr)
		{
			if (this.Character() != '[')
			{
				arr = null;
				return this.MakeFailure("Expected initial [ when parsing an array");
			}
			if (!this.TryMoveNext())
			{
				arr = null;
				return this.MakeFailure("Unexpected end of input when parsing an array");
			}
			this.SkipSpace();
			List<fsData> list = new List<fsData>();
			while (this.HasValue() && this.Character() != ']')
			{
				fsData fsData;
				fsResult result = this.RunParse(out fsData);
				if (result.Failed)
				{
					arr = null;
					return result;
				}
				list.Add(fsData);
				this.SkipSpace();
				if (this.HasValue() && this.Character() == ',')
				{
					if (!this.TryMoveNext())
					{
						break;
					}
					this.SkipSpace();
				}
			}
			if (!this.HasValue() || this.Character() != ']' || !this.TryMoveNext())
			{
				arr = null;
				return this.MakeFailure("No closing ] for array");
			}
			arr = new fsData(list);
			return fsResult.Success;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00012560 File Offset: 0x00010760
		private fsResult TryParseObject(out fsData obj)
		{
			if (this.Character() != '{')
			{
				obj = null;
				return this.MakeFailure("Expected initial { when parsing an object");
			}
			if (!this.TryMoveNext())
			{
				obj = null;
				return this.MakeFailure("Unexpected end of input when parsing an object");
			}
			this.SkipSpace();
			Dictionary<string, fsData> dictionary = new Dictionary<string, fsData>(fsGlobalConfig.IsCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
			while (this.HasValue() && this.Character() != '}')
			{
				this.SkipSpace();
				string text;
				fsResult result = this.TryParseString(out text);
				if (result.Failed)
				{
					obj = null;
					return result;
				}
				this.SkipSpace();
				if (!this.HasValue() || this.Character() != ':' || !this.TryMoveNext())
				{
					obj = null;
					return this.MakeFailure("Expected : after key \"" + text + "\"");
				}
				this.SkipSpace();
				fsData fsData;
				result = this.RunParse(out fsData);
				if (result.Failed)
				{
					obj = null;
					return result;
				}
				dictionary.Add(text, fsData);
				this.SkipSpace();
				if (this.HasValue() && this.Character() == ',')
				{
					if (!this.TryMoveNext())
					{
						break;
					}
					this.SkipSpace();
				}
			}
			if (!this.HasValue() || this.Character() != '}' || !this.TryMoveNext())
			{
				obj = null;
				return this.MakeFailure("No closing } for object");
			}
			obj = new fsData(dictionary);
			return fsResult.Success;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x000126B0 File Offset: 0x000108B0
		private fsResult RunParse(out fsData data)
		{
			this.SkipSpace();
			if (!this.HasValue())
			{
				data = null;
				return this.MakeFailure("Unexpected end of input");
			}
			char c = this.Character();
			if (c <= '[')
			{
				if (c <= 'I')
				{
					switch (c)
					{
					case '"':
					{
						string str;
						fsResult result = this.TryParseString(out str);
						if (result.Failed)
						{
							data = null;
							return result;
						}
						data = new fsData(str);
						return fsResult.Success;
					}
					case '#':
					case '$':
					case '%':
					case '&':
					case '\'':
					case '(':
					case ')':
					case '*':
					case ',':
					case '/':
						goto IL_11F;
					case '+':
					case '-':
					case '.':
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						break;
					default:
						if (c != 'I')
						{
							goto IL_11F;
						}
						break;
					}
				}
				else if (c != 'N')
				{
					if (c != '[')
					{
						goto IL_11F;
					}
					return this.TryParseArray(out data);
				}
				return this.TryParseNumber(out data);
			}
			if (c <= 'n')
			{
				if (c == 'f')
				{
					return this.TryParseFalse(out data);
				}
				if (c == 'n')
				{
					return this.TryParseNull(out data);
				}
			}
			else
			{
				if (c == 't')
				{
					return this.TryParseTrue(out data);
				}
				if (c == '{')
				{
					return this.TryParseObject(out data);
				}
			}
			IL_11F:
			data = null;
			return this.MakeFailure("unable to parse; invalid token \"" + this.Character().ToString() + "\"");
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00012802 File Offset: 0x00010A02
		public static fsResult Parse(string input, out fsData data)
		{
			if (string.IsNullOrEmpty(input))
			{
				data = null;
				return fsResult.Fail("No input");
			}
			return new fsJsonParser(input).RunParse(out data);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00012828 File Offset: 0x00010A28
		public static fsData Parse(string input)
		{
			fsData result;
			fsJsonParser.Parse(input, out result).AssertSuccess();
			return result;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00012847 File Offset: 0x00010A47
		private fsJsonParser(string input)
		{
			this._input = input;
			this._start = 0;
		}

		// Token: 0x040001F2 RID: 498
		private int _start;

		// Token: 0x040001F3 RID: 499
		private string _input;

		// Token: 0x040001F4 RID: 500
		private readonly StringBuilder _cachedStringBuilder = new StringBuilder(256);
	}
}
