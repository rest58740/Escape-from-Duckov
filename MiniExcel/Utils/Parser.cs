using System;
using System.Collections.Generic;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000032 RID: 50
	internal static class Parser
	{
		// Token: 0x06000156 RID: 342 RVA: 0x00005990 File Offset: 0x00003B90
		public static List<Section> ParseSections(string formatString, out bool syntaxError)
		{
			Tokenizer reader = new Tokenizer(formatString);
			List<Section> list = new List<Section>();
			syntaxError = false;
			for (;;)
			{
				bool flag;
				Section section = Parser.ParseSection(reader, list.Count, out flag);
				if (flag)
				{
					syntaxError = true;
				}
				if (section == null)
				{
					break;
				}
				list.Add(section);
			}
			return list;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000059D0 File Offset: 0x00003BD0
		private static Section ParseSection(Tokenizer reader, int index, out bool syntaxError)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			List<string> list = new List<string>();
			syntaxError = false;
			string text;
			while ((text = Parser.ReadToken(reader, out syntaxError)) != null && !(text == ";"))
			{
				flag5 |= Token.IsPlaceholder(text);
				if (Token.IsDatePart(text))
				{
					flag |= true;
					flag2 |= Token.IsDurationPart(text);
					list.Add(text);
				}
				else
				{
					list.Add(text);
				}
			}
			if (syntaxError || list.Count == 0)
			{
				return null;
			}
			if ((flag && (flag3 || flag4)) || (flag3 && (flag || flag4)) || (flag4 && (flag3 || flag)))
			{
				syntaxError = true;
				return null;
			}
			List<string> generalTextDateDurationParts = null;
			SectionType type;
			if (flag)
			{
				if (flag2)
				{
					type = SectionType.Duration;
				}
				else
				{
					type = SectionType.Date;
				}
				Parser.ParseMilliseconds(list, out generalTextDateDurationParts);
			}
			else if (flag3)
			{
				type = SectionType.General;
				generalTextDateDurationParts = list;
			}
			else
			{
				if (!flag4 && flag5)
				{
					syntaxError = true;
					return null;
				}
				type = SectionType.Text;
				generalTextDateDurationParts = list;
			}
			return new Section
			{
				Type = type,
				SectionIndex = index,
				GeneralTextDateDurationParts = generalTextDateDurationParts
			};
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005ACC File Offset: 0x00003CCC
		internal static int ParseNumberTokens(List<string> tokens, int startPosition, out List<string> beforeDecimal, out bool decimalSeparator, out List<string> afterDecimal)
		{
			beforeDecimal = null;
			afterDecimal = null;
			decimalSeparator = false;
			List<string> list = new List<string>();
			int i;
			for (i = 0; i < tokens.Count; i++)
			{
				string text = tokens[i];
				if (text == "." && beforeDecimal == null)
				{
					decimalSeparator = true;
					beforeDecimal = tokens.GetRange(0, i);
					list = new List<string>();
				}
				else if (Token.IsNumberLiteral(text))
				{
					list.Add(text);
				}
				else if (!text.StartsWith("["))
				{
					break;
				}
			}
			if (list.Count > 0)
			{
				if (beforeDecimal != null)
				{
					afterDecimal = list;
				}
				else
				{
					beforeDecimal = list;
				}
			}
			return i;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005B60 File Offset: 0x00003D60
		private static void ParseMilliseconds(List<string> tokens, out List<string> result)
		{
			result = new List<string>();
			for (int i = 0; i < tokens.Count; i++)
			{
				string text = tokens[i];
				if (text == ".")
				{
					int num = 0;
					while (i + 1 < tokens.Count && tokens[i + 1] == "0")
					{
						i++;
						num++;
					}
					if (num > 0)
					{
						result.Add("." + new string('0', num));
					}
					else
					{
						result.Add(".");
					}
				}
				else
				{
					result.Add(text);
				}
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005C00 File Offset: 0x00003E00
		private static string ReadToken(Tokenizer reader, out bool syntaxError)
		{
			int position = reader.Position;
			if (Parser.ReadLiteral(reader) || reader.ReadEnclosed('[', ']') || reader.ReadOneOf("#?,!&%+-$€£0123456789{}():;/.@ ") || reader.ReadString("e+", true) || reader.ReadString("e-", true) || reader.ReadString("General", true) || reader.ReadString("am/pm", true) || reader.ReadString("a/p", true) || reader.ReadOneOrMore(121) || reader.ReadOneOrMore(89) || reader.ReadOneOrMore(109) || reader.ReadOneOrMore(77) || reader.ReadOneOrMore(100) || reader.ReadOneOrMore(68) || reader.ReadOneOrMore(104) || reader.ReadOneOrMore(72) || reader.ReadOneOrMore(115) || reader.ReadOneOrMore(83) || reader.ReadOneOrMore(103) || reader.ReadOneOrMore(71))
			{
				syntaxError = false;
				int length = reader.Position - position;
				return reader.Substring(position, length);
			}
			syntaxError = (reader.Position < reader.Length);
			return null;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005D2E File Offset: 0x00003F2E
		private static bool ReadLiteral(Tokenizer reader)
		{
			if (reader.Peek(0) == 92 || reader.Peek(0) == 42 || reader.Peek(0) == 95)
			{
				reader.Advance(2);
				return true;
			}
			return reader.ReadEnclosed('"', '"');
		}
	}
}
