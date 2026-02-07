using System;
using System.Collections.Generic;
using System.Xml;
using MiniExcelLibs.Utils;
using MiniExcelLibs.Zip;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x02000041 RID: 65
	internal class ExcelOpenXmlStyles
	{
		// Token: 0x060001ED RID: 493 RVA: 0x00009924 File Offset: 0x00007B24
		public ExcelOpenXmlStyles(ExcelOpenXmlZip zip)
		{
			using (XmlReader xmlReader = zip.GetXmlReader("xl/styles.xml"))
			{
				if (XmlReaderHelper.IsStartElement(xmlReader, "styleSheet", ExcelOpenXmlStyles._ns))
				{
					if (XmlReaderHelper.ReadFirstContent(xmlReader))
					{
						while (!xmlReader.EOF)
						{
							if (XmlReaderHelper.IsStartElement(xmlReader, "cellXfs", ExcelOpenXmlStyles._ns))
							{
								if (XmlReaderHelper.ReadFirstContent(xmlReader))
								{
									int num = 0;
									while (!xmlReader.EOF)
									{
										if (XmlReaderHelper.IsStartElement(xmlReader, "xf", ExcelOpenXmlStyles._ns))
										{
											int xfId;
											int.TryParse(xmlReader.GetAttribute("xfId"), out xfId);
											int numFmtId;
											int.TryParse(xmlReader.GetAttribute("numFmtId"), out numFmtId);
											this._cellXfs.Add(num, new StyleRecord
											{
												XfId = xfId,
												NumFmtId = numFmtId
											});
											xmlReader.Skip();
											num++;
										}
										else if (!XmlReaderHelper.SkipContent(xmlReader))
										{
											break;
										}
									}
								}
							}
							else if (XmlReaderHelper.IsStartElement(xmlReader, "cellStyleXfs", ExcelOpenXmlStyles._ns))
							{
								if (XmlReaderHelper.ReadFirstContent(xmlReader))
								{
									int num2 = 0;
									while (!xmlReader.EOF)
									{
										if (XmlReaderHelper.IsStartElement(xmlReader, "xf", ExcelOpenXmlStyles._ns))
										{
											int xfId2;
											int.TryParse(xmlReader.GetAttribute("xfId"), out xfId2);
											int numFmtId2;
											int.TryParse(xmlReader.GetAttribute("numFmtId"), out numFmtId2);
											this._cellStyleXfs.Add(num2, new StyleRecord
											{
												XfId = xfId2,
												NumFmtId = numFmtId2
											});
											xmlReader.Skip();
											num2++;
										}
										else if (!XmlReaderHelper.SkipContent(xmlReader))
										{
											break;
										}
									}
								}
							}
							else if (XmlReaderHelper.IsStartElement(xmlReader, "numFmts", ExcelOpenXmlStyles._ns))
							{
								if (XmlReaderHelper.ReadFirstContent(xmlReader))
								{
									while (!xmlReader.EOF)
									{
										if (XmlReaderHelper.IsStartElement(xmlReader, "numFmt", ExcelOpenXmlStyles._ns))
										{
											int key;
											int.TryParse(xmlReader.GetAttribute("numFmtId"), out key);
											string attribute = xmlReader.GetAttribute("formatCode");
											Type typeFromHandle = typeof(string);
											if (DateTimeHelper.IsDateTimeFormat(attribute))
											{
												typeFromHandle = typeof(DateTime?);
											}
											this._customFormats.Add(key, new NumberFormatString(attribute, typeFromHandle, false));
											xmlReader.Skip();
										}
										else if (!XmlReaderHelper.SkipContent(xmlReader))
										{
											break;
										}
									}
								}
							}
							else if (!XmlReaderHelper.SkipContent(xmlReader))
							{
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009BB8 File Offset: 0x00007DB8
		public NumberFormatString GetStyleFormat(int index)
		{
			StyleRecord styleRecord;
			if (!this._cellXfs.TryGetValue(index, out styleRecord))
			{
				return null;
			}
			NumberFormatString result;
			if (ExcelOpenXmlStyles.Formats.TryGetValue(styleRecord.NumFmtId, out result))
			{
				return result;
			}
			NumberFormatString result2;
			if (this._customFormats.TryGetValue(styleRecord.NumFmtId, out result2))
			{
				return result2;
			}
			return null;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009C08 File Offset: 0x00007E08
		public object ConvertValueByStyleFormat(int index, object value)
		{
			NumberFormatString styleFormat = this.GetStyleFormat(index);
			if (styleFormat == null)
			{
				return value;
			}
			if (styleFormat.Type == null)
			{
				return value;
			}
			double value2;
			if (styleFormat.Type == typeof(DateTime?))
			{
				double num;
				if (double.TryParse((value != null) ? value.ToString() : null, out num))
				{
					if (num >= 2958466.0 || num <= -657435.0)
					{
						return value;
					}
					return DateTimeHelper.FromOADate(num);
				}
			}
			else if (styleFormat.Type == typeof(TimeSpan?) && double.TryParse((value != null) ? value.ToString() : null, out value2))
			{
				return TimeSpan.FromDays(value2);
			}
			return value;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00009CBD File Offset: 0x00007EBD
		private static Dictionary<int, NumberFormatString> Formats { get; } = new Dictionary<int, NumberFormatString>
		{
			{
				0,
				new NumberFormatString("General", typeof(string), false)
			},
			{
				1,
				new NumberFormatString("0", typeof(decimal?), false)
			},
			{
				2,
				new NumberFormatString("0.00", typeof(decimal?), false)
			},
			{
				3,
				new NumberFormatString("#,##0", typeof(decimal?), false)
			},
			{
				4,
				new NumberFormatString("#,##0.00", typeof(decimal?), false)
			},
			{
				5,
				new NumberFormatString("\"$\"#,##0_);(\"$\"#,##0)", typeof(decimal?), false)
			},
			{
				6,
				new NumberFormatString("\"$\"#,##0_);[Red](\"$\"#,##0)", typeof(decimal?), false)
			},
			{
				7,
				new NumberFormatString("\"$\"#,##0.00_);(\"$\"#,##0.00)", typeof(decimal?), false)
			},
			{
				8,
				new NumberFormatString("\"$\"#,##0.00_);[Red](\"$\"#,##0.00)", typeof(string), false)
			},
			{
				9,
				new NumberFormatString("0%", typeof(decimal?), false)
			},
			{
				10,
				new NumberFormatString("0.00%", typeof(string), false)
			},
			{
				11,
				new NumberFormatString("0.00E+00", typeof(string), false)
			},
			{
				12,
				new NumberFormatString("# ?/?", typeof(string), false)
			},
			{
				13,
				new NumberFormatString("# ??/??", typeof(string), false)
			},
			{
				14,
				new NumberFormatString("d/m/yyyy", typeof(DateTime?), false)
			},
			{
				15,
				new NumberFormatString("d-mmm-yy", typeof(DateTime?), false)
			},
			{
				16,
				new NumberFormatString("d-mmm", typeof(DateTime?), false)
			},
			{
				17,
				new NumberFormatString("mmm-yy", typeof(TimeSpan), false)
			},
			{
				18,
				new NumberFormatString("h:mm AM/PM", typeof(TimeSpan?), false)
			},
			{
				19,
				new NumberFormatString("h:mm:ss AM/PM", typeof(TimeSpan?), false)
			},
			{
				20,
				new NumberFormatString("h:mm", typeof(TimeSpan?), false)
			},
			{
				21,
				new NumberFormatString("h:mm:ss", typeof(TimeSpan?), false)
			},
			{
				22,
				new NumberFormatString("m/d/yy h:mm", typeof(DateTime?), false)
			},
			{
				37,
				new NumberFormatString("#,##0_);(#,##0)", typeof(string), false)
			},
			{
				38,
				new NumberFormatString("#,##0_);[Red](#,##0)", typeof(string), false)
			},
			{
				39,
				new NumberFormatString("#,##0.00_);(#,##0.00)", typeof(string), false)
			},
			{
				40,
				new NumberFormatString("#,##0.00_);[Red](#,##0.00)", typeof(string), false)
			},
			{
				41,
				new NumberFormatString("_(\"$\"* #,##0_);_(\"$\"* (#,##0);_(\"$\"* \"-\"_);_(@_)", typeof(string), false)
			},
			{
				42,
				new NumberFormatString("_(* #,##0_);_(* (#,##0);_(* \"-\"_);_(@_)", typeof(string), false)
			},
			{
				43,
				new NumberFormatString("_(\"$\"* #,##0.00_);_(\"$\"* (#,##0.00);_(\"$\"* \"-\"??_);_(@_)", typeof(string), false)
			},
			{
				44,
				new NumberFormatString("_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)", typeof(string), false)
			},
			{
				45,
				new NumberFormatString("mm:ss", typeof(TimeSpan?), false)
			},
			{
				46,
				new NumberFormatString("[h]:mm:ss", typeof(TimeSpan?), false)
			},
			{
				47,
				new NumberFormatString("mm:ss.0", typeof(TimeSpan?), false)
			},
			{
				48,
				new NumberFormatString("##0.0E+0", typeof(string), false)
			},
			{
				49,
				new NumberFormatString("@", typeof(string), false)
			},
			{
				58,
				new NumberFormatString("m/d", typeof(DateTime?), false)
			}
		};

		// Token: 0x040000A9 RID: 169
		private static readonly string[] _ns = new string[]
		{
			"http://schemas.openxmlformats.org/spreadsheetml/2006/main",
			"http://purl.oclc.org/ooxml/spreadsheetml/main"
		};

		// Token: 0x040000AA RID: 170
		private readonly Dictionary<int, StyleRecord> _cellXfs = new Dictionary<int, StyleRecord>();

		// Token: 0x040000AB RID: 171
		private readonly Dictionary<int, StyleRecord> _cellStyleXfs = new Dictionary<int, StyleRecord>();

		// Token: 0x040000AC RID: 172
		private readonly Dictionary<int, NumberFormatString> _customFormats = new Dictionary<int, NumberFormatString>();
	}
}
