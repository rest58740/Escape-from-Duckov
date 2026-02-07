using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;

namespace FlexFramework.Excel
{
	// Token: 0x0200001A RID: 26
	public sealed class WorkBook : KeyedCollection<string, WorkSheet>
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00003FE4 File Offset: 0x000021E4
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003FEC File Offset: 0x000021EC
		public SharedStringCollection SharedStrings { get; private set; }

		// Token: 0x060000B9 RID: 185 RVA: 0x00003FF8 File Offset: 0x000021F8
		public WorkBook(byte[] buffer)
		{
			using (ZipFile zipFile = new ZipFile(new MemoryStream(buffer)))
			{
				this.Read(zipFile);
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000403C File Offset: 0x0000223C
		public WorkBook(Stream stream)
		{
			ZipFile zip = new ZipFile(stream);
			this.Read(zip);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004060 File Offset: 0x00002260
		public WorkBook(string fileName)
		{
			using (ZipFile zipFile = new ZipFile(fileName))
			{
				this.Read(zipFile);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000040A0 File Offset: 0x000022A0
		private void Read(ZipFile zip)
		{
			List<string> list = new List<string>();
			ZipEntry entry = zip.GetEntry("xl/sharedStrings.xml");
			if (entry != null)
			{
				using (XmlReader xmlReader = XmlReader.Create(zip.GetInputStream(entry)))
				{
					if (xmlReader.ReadToFollowing("sst"))
					{
						while (xmlReader.ReadToFollowing("si"))
						{
							using (XmlReader xmlReader2 = xmlReader.ReadSubtree())
							{
								StringBuilder stringBuilder = new StringBuilder();
								while (!xmlReader2.EOF)
								{
									if (xmlReader2.NodeType != XmlNodeType.Element)
									{
										xmlReader2.Read();
									}
									else
									{
										string name = xmlReader2.Name;
										if (!(name == "t"))
										{
											if (!(name == "r") && !(name == "si"))
											{
												xmlReader2.Skip();
											}
											else
											{
												xmlReader2.Read();
											}
										}
										else
										{
											stringBuilder.Append(xmlReader2.ReadElementContentAsString());
										}
									}
								}
								list.Add(stringBuilder.ToString());
							}
						}
					}
				}
			}
			using (XmlReader xmlReader3 = XmlReader.Create(zip.GetInputStream(zip.GetEntry("xl/workbook.xml"))))
			{
				if (xmlReader3.ReadToFollowing("sheets") && xmlReader3.ReadToDescendant("sheet"))
				{
					int num = 1;
					do
					{
						string attribute = xmlReader3.GetAttribute("name");
						string attribute2 = xmlReader3.GetAttribute("sheetId");
						using (XmlReader xmlReader4 = XmlReader.Create(zip.GetInputStream(zip.GetEntry(string.Format("xl/worksheets/sheet{0}.xml", num++)))))
						{
							List<Row> list2 = new List<Row>();
							List<Range> list3 = new List<Range>();
							if (xmlReader4.ReadToFollowing("sheetData"))
							{
								if (xmlReader4.ReadToDescendant("row"))
								{
									do
									{
										if (xmlReader4.ReadToDescendant("c"))
										{
											List<Cell> list4 = new List<Cell>();
											do
											{
												string attribute3 = xmlReader4.GetAttribute("r");
												string attribute4 = xmlReader4.GetAttribute("t");
												Cell cell;
												if (xmlReader4.ReadToDescendant("v"))
												{
													if (attribute4 == "s")
													{
														int index = xmlReader4.ReadElementContentAsInt();
														cell = new Cell(list[index], new Address(attribute3));
													}
													else if (attribute4 == "b")
													{
														cell = new Cell(xmlReader4.ReadElementContentAsBoolean(), new Address(attribute3));
													}
													else if (attribute4 == "str" || attribute4 == "inlineStr")
													{
														string value = xmlReader4.ReadElementContentAsString();
														cell = new Cell(value, new Address(attribute3));
													}
													else
													{
														string text = xmlReader4.ReadElementContentAsString();
														if (Regex.IsMatch(text, "^[-+]?\\d*(\\.\\d+)?$"))
														{
															if (Regex.IsMatch(text, "^[-+]?\\d+$"))
															{
																if (Regex.Matches(text, "\\d").Count <= 11)
																{
																	cell = new Cell(int.Parse(text), new Address(attribute3));
																}
																else
																{
																	cell = new Cell(long.Parse(text), new Address(attribute3));
																}
															}
															else if (Regex.Matches(text, "\\d").Count <= 7)
															{
																cell = new Cell(float.Parse(text), new Address(attribute3));
															}
															else
															{
																cell = new Cell(double.Parse(text), new Address(attribute3));
															}
														}
														else
														{
															cell = new Cell(text, new Address(attribute3));
														}
													}
												}
												else
												{
													cell = new Cell(new Address(attribute3))
													{
														IsSpan = true
													};
												}
												Cell last = list4.LastOrDefault<Cell>();
												if (last != null)
												{
													int num2 = cell.Address.Column - last.Address.Column - 1;
													if (num2 > 0)
													{
														list4.AddRange(from i in Enumerable.Range(1, num2)
														select new Cell(new Address(last.Address.Column + i, last.Address.Row)));
													}
												}
												else if (cell.Address.Column > 0)
												{
													int count = cell.Address.Column - 1;
													list4.AddRange(from i in Enumerable.Range(0, count)
													select new Cell(new Address(i, cell.Address.Row)));
												}
												list4.Add(cell);
											}
											while (xmlReader4.ReadToNextSibling("c"));
											list2.Add(new Row(list4));
										}
									}
									while (xmlReader4.ReadToNextSibling("row"));
								}
								if (xmlReader4.ReadToFollowing("mergeCells") && xmlReader4.ReadToDescendant("mergeCell"))
								{
									do
									{
										string attribute5 = xmlReader4.GetAttribute("ref");
										list3.Add(new Range(attribute5));
									}
									while (xmlReader4.ReadToNextSibling("mergeCell"));
								}
								WorkSheet item = new WorkSheet(attribute2, attribute, list2, list3);
								base.Add(item);
							}
						}
					}
					while (xmlReader3.ReadToNextSibling("sheet"));
				}
			}
			this.SharedStrings = new SharedStringCollection(list);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004628 File Offset: 0x00002828
		protected override string GetKeyForItem(WorkSheet item)
		{
			return item.Name;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004630 File Offset: 0x00002830
		public void Merge()
		{
			foreach (WorkSheet workSheet in this)
			{
				workSheet.Merge();
			}
		}
	}
}
