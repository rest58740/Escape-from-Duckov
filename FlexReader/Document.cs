using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FlexFramework.Excel
{
	// Token: 0x02000018 RID: 24
	public sealed class Document : Table
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00003D58 File Offset: 0x00001F58
		public Document(IList<Row> list) : base(list)
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003D61 File Offset: 0x00001F61
		public Document() : this(new List<Row>())
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003D6E File Offset: 0x00001F6E
		public static Document LoadAt(string path)
		{
			return Document.Load(File.ReadAllText(path));
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003D7C File Offset: 0x00001F7C
		public static Document Load(string content)
		{
			Document result;
			using (StringReader stringReader = new StringReader(content))
			{
				result = Document.Load(stringReader);
			}
			return result;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public static Document Load(Stream stream)
		{
			return Document.Load(new StreamReader(stream));
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003DC4 File Offset: 0x00001FC4
		public static Document Load(byte[] buffer)
		{
			Document result;
			using (MemoryStream memoryStream = new MemoryStream(buffer))
			{
				result = Document.Load(memoryStream);
			}
			return result;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003DFC File Offset: 0x00001FFC
		public static Document Load(TextReader reader)
		{
			Document document = new Document();
			Row row = new Row();
			int num = 1;
			int num2 = 1;
			int num3 = 0;
			StringBuilder stringBuilder = new StringBuilder();
			while (reader.Peek() != -1)
			{
				char c = (char)reader.Read();
				num3++;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r' && reader.Peek() == 10)
					{
						reader.Read();
						num3++;
					}
					row.Cells.Add(new Cell(stringBuilder.ToString(), new Address(num, num2)));
					document.Items.Add(row);
					num = 1;
					num3 = 0;
					stringBuilder = new StringBuilder();
					num2++;
					row = new Row();
				}
				else if (c == Document.Delimiter)
				{
					row.Cells.Add(new Cell(stringBuilder.ToString(), new Address(num, num2)));
					num++;
					stringBuilder = new StringBuilder();
				}
				else if (c == Document.Enclose)
				{
					while (reader.Peek() != -1)
					{
						c = (char)reader.Read();
						num3++;
						if (c != Document.Enclose)
						{
							stringBuilder.Append(c);
						}
						else
						{
							if (reader.Peek() != (int)Document.Enclose)
							{
								break;
							}
							reader.Read();
							num3++;
							stringBuilder.Append(c);
						}
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			row.Cells.Add(new Cell(stringBuilder.ToString(), new Address(num, num2)));
			document.Items.Add(row);
			return document;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003F78 File Offset: 0x00002178
		public static void Reset()
		{
			Document.Delimiter = ',';
			Document.Enclose = '"';
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003F88 File Offset: 0x00002188
		public override Table DeepClone()
		{
			return new Document((from row in base.Items
			select row.DeepClone()).ToList<Row>());
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003FBE File Offset: 0x000021BE
		public override Table ShallowClone()
		{
			return (Table)base.MemberwiseClone();
		}

		// Token: 0x04000009 RID: 9
		public static char Delimiter = ',';

		// Token: 0x0400000A RID: 10
		public static char Enclose = '"';
	}
}
