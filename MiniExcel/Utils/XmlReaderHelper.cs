using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace MiniExcelLibs.Utils
{
	// Token: 0x0200003D RID: 61
	internal static class XmlReaderHelper
	{
		// Token: 0x06000189 RID: 393 RVA: 0x00006D4A File Offset: 0x00004F4A
		public static void PassXmlDeclartionAndWorksheet(this XmlReader reader)
		{
			reader.MoveToContent();
			reader.Read();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006D5A File Offset: 0x00004F5A
		public static void SkipToNextSameLevelDom(XmlReader reader)
		{
			while (!reader.EOF && XmlReaderHelper.SkipContent(reader))
			{
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006D6E File Offset: 0x00004F6E
		public static bool ReadFirstContent(XmlReader reader)
		{
			if (reader.IsEmptyElement)
			{
				reader.Read();
				return false;
			}
			reader.MoveToContent();
			reader.Read();
			return true;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006D90 File Offset: 0x00004F90
		public static bool SkipContent(XmlReader reader)
		{
			if (reader.NodeType == XmlNodeType.EndElement)
			{
				reader.Read();
				return false;
			}
			reader.Skip();
			return true;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006DAC File Offset: 0x00004FAC
		public static bool IsStartElement(XmlReader reader, string name, params string[] nss)
		{
			return nss.Any((string s) => reader.IsStartElement(name, s));
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006DE0 File Offset: 0x00004FE0
		public static string GetAttribute(XmlReader reader, string name, params string[] nss)
		{
			foreach (string namespaceURI in nss)
			{
				string attribute = reader.GetAttribute(name, namespaceURI);
				if (attribute != null)
				{
					return attribute;
				}
			}
			return null;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006E10 File Offset: 0x00005010
		public static IEnumerable<string> GetSharedStrings(Stream stream, params string[] nss)
		{
			using (XmlReader reader = XmlReader.Create(stream))
			{
				if (!XmlReaderHelper.IsStartElement(reader, "sst", nss))
				{
					yield break;
				}
				if (!XmlReaderHelper.ReadFirstContent(reader))
				{
					yield break;
				}
				while (!reader.EOF)
				{
					if (XmlReaderHelper.IsStartElement(reader, "si", nss))
					{
						string text = StringHelper.ReadStringItem(reader);
						yield return text;
					}
					else if (!XmlReaderHelper.SkipContent(reader))
					{
						break;
					}
				}
			}
			XmlReader reader = null;
			yield break;
			yield break;
		}
	}
}
