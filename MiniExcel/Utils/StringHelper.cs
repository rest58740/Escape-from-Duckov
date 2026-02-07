using System;
using System.Linq;
using System.Text;
using System.Xml;

namespace MiniExcelLibs.Utils
{
	// Token: 0x0200003A RID: 58
	internal static class StringHelper
	{
		// Token: 0x0600017B RID: 379 RVA: 0x0000660B File Offset: 0x0000480B
		public static string GetLetter(string content)
		{
			return new string(content.Where(new Func<char, bool>(char.IsLetter)).ToArray<char>());
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006629 File Offset: 0x00004829
		public static int GetNumber(string content)
		{
			return int.Parse(new string(content.Where(new Func<char, bool>(char.IsNumber)).ToArray<char>()));
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000664C File Offset: 0x0000484C
		public static string ReadStringItem(XmlReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!XmlReaderHelper.ReadFirstContent(reader))
			{
				return string.Empty;
			}
			while (!reader.EOF)
			{
				if (XmlReaderHelper.IsStartElement(reader, "t", StringHelper._ns))
				{
					stringBuilder.Append(reader.ReadElementContentAsString());
				}
				else if (XmlReaderHelper.IsStartElement(reader, "r", StringHelper._ns))
				{
					stringBuilder.Append(StringHelper.ReadRichTextRun(reader));
				}
				else if (!XmlReaderHelper.SkipContent(reader))
				{
					break;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000066C8 File Offset: 0x000048C8
		private static string ReadRichTextRun(XmlReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!XmlReaderHelper.ReadFirstContent(reader))
			{
				return string.Empty;
			}
			while (!reader.EOF)
			{
				if (XmlReaderHelper.IsStartElement(reader, "t", StringHelper._ns))
				{
					stringBuilder.Append(reader.ReadElementContentAsString());
				}
				else if (!XmlReaderHelper.SkipContent(reader))
				{
					break;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400008A RID: 138
		private static readonly string[] _ns = new string[]
		{
			"http://schemas.openxmlformats.org/spreadsheetml/2006/main",
			"http://purl.oclc.org/ooxml/spreadsheetml/main"
		};
	}
}
