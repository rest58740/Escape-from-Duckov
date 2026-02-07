using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.Utils;
using MiniExcelLibs.Zip;

namespace MiniExcelLibs.OpenXml.SaveByTemplate
{
	// Token: 0x02000050 RID: 80
	internal class ExcelOpenXmlTemplate : IExcelTemplate, IExcelTemplateAsync
	{
		// Token: 0x0600026A RID: 618 RVA: 0x0000AD88 File Offset: 0x00008F88
		static ExcelOpenXmlTemplate()
		{
			ExcelOpenXmlTemplate._ns = new XmlNamespaceManager(new NameTable());
			ExcelOpenXmlTemplate._ns.AddNamespace("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
			ExcelOpenXmlTemplate._ns.AddNamespace("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000ADDC File Offset: 0x00008FDC
		public ExcelOpenXmlTemplate(Stream stream, IConfiguration configuration, InputValueExtractor inputValueExtractor)
		{
			this._stream = stream;
			this._configuration = (((OpenXmlConfiguration)configuration) ?? OpenXmlConfiguration.DefaultConfig);
			this._inputValueExtractor = inputValueExtractor;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000AE28 File Offset: 0x00009028
		public void SaveAsByTemplate(string templatePath, object value)
		{
			using (FileStream fileStream = FileHelper.OpenSharedRead(templatePath))
			{
				this.SaveAsByTemplateImpl(fileStream, value);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000AE60 File Offset: 0x00009060
		public void SaveAsByTemplate(byte[] templateBtyes, object value)
		{
			using (Stream stream = new MemoryStream(templateBtyes))
			{
				this.SaveAsByTemplateImpl(stream, value);
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000AE98 File Offset: 0x00009098
		public void SaveAsByTemplateImpl(Stream templateStream, object value)
		{
			templateStream.CopyTo(this._stream);
			ExcelOpenXmlSheetReader excelOpenXmlSheetReader = new ExcelOpenXmlSheetReader(this._stream, null);
			ExcelOpenXmlZip excelOpenXmlZip = new ExcelOpenXmlZip(this._stream, ZipArchiveMode.Update, true, Encoding.UTF8);
			IDictionary<int, string> sharedStrings = excelOpenXmlSheetReader._sharedStrings;
			new StringBuilder();
			List<ZipArchiveEntry> list = (from w in excelOpenXmlZip.zipFile.Entries
			where w.FullName.StartsWith("xl/worksheets/sheet", StringComparison.OrdinalIgnoreCase) || w.FullName.StartsWith("/xl/worksheets/sheet", StringComparison.OrdinalIgnoreCase)
			select w).ToList<ZipArchiveEntry>();
			int num = 0;
			foreach (ZipArchiveEntry zipArchiveEntry in list)
			{
				this.XRowInfos = new List<ExcelOpenXmlTemplate.XRowInfo>();
				this.XMergeCellInfos = new Dictionary<string, ExcelOpenXmlTemplate.XMergeCell>();
				this.NewXMergeCellInfos = new List<ExcelOpenXmlTemplate.XMergeCell>();
				Stream sheetStream = zipArchiveEntry.Open();
				string fullName = zipArchiveEntry.FullName;
				IDictionary<string, object> inputMaps = this._inputValueExtractor.ToValueDictionary(value);
				ZipArchiveEntry zipArchiveEntry2 = excelOpenXmlZip.zipFile.CreateEntry(fullName);
				using (Stream stream = zipArchiveEntry2.Open())
				{
					this.GenerateSheetXmlImpl(zipArchiveEntry, stream, sheetStream, inputMaps, sharedStrings, false);
				}
				using (zipArchiveEntry2.Open())
				{
					num++;
					this._calcChainContent.Append(CalcChainHelper.GetCalcChainContent(this.CalcChainCellRefs, num));
				}
			}
			ZipArchiveEntry zipArchiveEntry3 = excelOpenXmlZip.zipFile.Entries.FirstOrDefault((ZipArchiveEntry e) => e.FullName.Contains("xl/calcChain.xml"));
			if (zipArchiveEntry3 != null)
			{
				string fullName2 = zipArchiveEntry3.FullName;
				zipArchiveEntry3.Delete();
				using (Stream stream3 = excelOpenXmlZip.zipFile.CreateEntry(fullName2).Open())
				{
					CalcChainHelper.GenerateCalcChainSheet(stream3, this._calcChainContent.ToString());
				}
			}
			excelOpenXmlZip.zipFile.Dispose();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000B0A4 File Offset: 0x000092A4
		public Task SaveAsByTemplateAsync(string templatePath, object value, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.Run(delegate()
			{
				this.SaveAsByTemplate(templatePath, value);
			}, cancellationToken);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000B0D1 File Offset: 0x000092D1
		public Task SaveAsByTemplateAsync(byte[] templateBtyes, object value, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.Run(delegate()
			{
				this.SaveAsByTemplate(templateBtyes, value);
			}, cancellationToken);
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000B0FE File Offset: 0x000092FE
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000B106 File Offset: 0x00009306
		private List<ExcelOpenXmlTemplate.XRowInfo> XRowInfos { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000B10F File Offset: 0x0000930F
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000B117 File Offset: 0x00009317
		private Dictionary<string, ExcelOpenXmlTemplate.XMergeCell> XMergeCellInfos { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000B120 File Offset: 0x00009320
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000B128 File Offset: 0x00009328
		public List<ExcelOpenXmlTemplate.XMergeCell> NewXMergeCellInfos { get; private set; }

		// Token: 0x06000277 RID: 631 RVA: 0x0000B134 File Offset: 0x00009334
		private void GenerateSheetXmlImpl(ZipArchiveEntry sheetZipEntry, Stream stream, Stream sheetStream, IDictionary<string, object> inputMaps, IDictionary<int, string> sharedStrings, bool mergeCells = false)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(sheetStream);
			sheetStream.Dispose();
			sheetZipEntry.Delete();
			XmlNode worksheet = xmlDocument.SelectSingleNode("/x:worksheet", ExcelOpenXmlTemplate._ns);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("/x:worksheet/x:sheetData", ExcelOpenXmlTemplate._ns);
			XmlNodeList xmlNodeList = xmlNode.Clone().SelectNodes("x:row", ExcelOpenXmlTemplate._ns);
			this.ReplaceSharedStringsToStr(sharedStrings, ref xmlNodeList);
			this.GetMercells(xmlDocument, worksheet);
			this.UpdateDimensionAndGetRowsInfo(inputMaps, ref xmlDocument, ref xmlNodeList, !mergeCells);
			this.WriteSheetXml(stream, xmlDocument, xmlNode, mergeCells);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000B1C0 File Offset: 0x000093C0
		private void GetMercells(XmlDocument doc, XmlNode worksheet)
		{
			XmlNode xmlNode = doc.SelectSingleNode("/x:worksheet/x:mergeCells", ExcelOpenXmlTemplate._ns);
			if (xmlNode != null)
			{
				XmlNode xmlNode2 = xmlNode.Clone();
				worksheet.RemoveChild(xmlNode);
				foreach (object obj in xmlNode2)
				{
					XmlElement xmlElement = (XmlElement)obj;
					string value = xmlElement.Attributes["ref"].Value;
					ExcelOpenXmlTemplate.XMergeCell xmergeCell = new ExcelOpenXmlTemplate.XMergeCell(xmlElement);
					this.XMergeCellInfos[xmergeCell.XY1] = xmergeCell;
				}
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000B25C File Offset: 0x0000945C
		private void WriteSheetXml(Stream stream, XmlDocument doc, XmlNode sheetData, bool mergeCells = false)
		{
			sheetData.RemoveAll();
			sheetData.InnerText = "{{{{{{split}}}}}}";
			string text = string.IsNullOrEmpty(sheetData.Prefix) ? "" : (sheetData.Prefix + ":");
			string endPrefix = string.IsNullOrEmpty(sheetData.Prefix) ? "" : (":" + sheetData.Prefix);
			string[] array = doc.InnerXml.Split(new string[]
			{
				string.Concat(new string[]
				{
					"<",
					text,
					"sheetData>{{{{{{split}}}}}}</",
					text,
					"sheetData>"
				})
			}, StringSplitOptions.None);
			using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
			{
				streamWriter.Write(array[0]);
				streamWriter.Write("<" + text + "sheetData>");
				if (mergeCells)
				{
					Dictionary<ExcelOpenXmlTemplate.XChildNode, ExcelOpenXmlTemplate.XChildNode> dictionary = new Dictionary<ExcelOpenXmlTemplate.XChildNode, ExcelOpenXmlTemplate.XChildNode>();
					List<ExcelOpenXmlTemplate.XChildNode> source = (from x in (from s in this.XRowInfos.SelectMany((ExcelOpenXmlTemplate.XRowInfo s) => s.Row.Cast<XmlElement>())
					where !string.IsNullOrEmpty(s.InnerText)
					select s).Select(delegate(XmlElement s)
					{
						string attribute2 = s.GetAttribute("r");
						return new ExcelOpenXmlTemplate.XChildNode
						{
							InnerText = s.InnerText,
							ColIndex = StringHelper.GetLetter(attribute2),
							RowIndex = StringHelper.GetNumber(attribute2)
						};
					})
					orderby x.RowIndex
					select x).ToList<ExcelOpenXmlTemplate.XChildNode>();
					List<ExcelOpenXmlTemplate.XChildNode> list = (from s in source
					where s.InnerText.Contains("@merge")
					select s).ToList<ExcelOpenXmlTemplate.XChildNode>();
					List<ExcelOpenXmlTemplate.XChildNode> source2 = (from s in source
					where s.InnerText.Contains("@endmerge")
					select s).ToList<ExcelOpenXmlTemplate.XChildNode>();
					ExcelOpenXmlTemplate.XChildNode mergeLimitColumn = list.FirstOrDefault((ExcelOpenXmlTemplate.XChildNode x) => x.InnerText.Contains("@mergelimit"));
					using (List<ExcelOpenXmlTemplate.XChildNode>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ExcelOpenXmlTemplate.XChildNode mergeColumn = enumerator.Current;
							ExcelOpenXmlTemplate.XChildNode xchildNode = source2.FirstOrDefault((ExcelOpenXmlTemplate.XChildNode s) => s.ColIndex == mergeColumn.ColIndex && s.RowIndex > mergeColumn.RowIndex);
							if (xchildNode != null)
							{
								dictionary[mergeColumn] = xchildNode;
							}
						}
					}
					List<ExcelOpenXmlTemplate.XChildNode> list2 = new List<ExcelOpenXmlTemplate.XChildNode>();
					if (dictionary.Count > 0)
					{
						using (Dictionary<ExcelOpenXmlTemplate.XChildNode, ExcelOpenXmlTemplate.XChildNode>.Enumerator enumerator2 = dictionary.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								KeyValuePair<ExcelOpenXmlTemplate.XChildNode, ExcelOpenXmlTemplate.XChildNode> taggedColumn = enumerator2.Current;
								list2.AddRange(from x in source
								where x.ColIndex == taggedColumn.Key.ColIndex && x.RowIndex > taggedColumn.Key.RowIndex && x.RowIndex < taggedColumn.Value.RowIndex
								select x);
							}
						}
						Dictionary<int, ExcelOpenXmlTemplate.MergeCellIndex> dictionary2 = new Dictionary<int, ExcelOpenXmlTemplate.MergeCellIndex>();
						for (int i = 0; i < this.XRowInfos.Count; i++)
						{
							ExcelOpenXmlTemplate.XRowInfo xrowInfo = this.XRowInfos[i];
							using (List<XmlElement>.Enumerator enumerator3 = xrowInfo.Row.ChildNodes.Cast<XmlElement>().ToList<XmlElement>().GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									XmlElement childNode = enumerator3.Current;
									string attribute = childNode.GetAttribute("r");
									string childNodeLetter = StringHelper.GetLetter(attribute);
									int childNodeNumber = StringHelper.GetNumber(attribute);
									if (!string.IsNullOrEmpty(childNode.InnerText))
									{
										List<ExcelOpenXmlTemplate.XChildNode> xmlNodes = (from j in list2
										where j.InnerText == childNode.InnerText && j.ColIndex == childNodeLetter
										select j into s
										orderby s.RowIndex
										select s).ToList<ExcelOpenXmlTemplate.XChildNode>();
										if (xmlNodes.Count > 1)
										{
											if (mergeLimitColumn != null)
											{
												ExcelOpenXmlTemplate.XChildNode limitedNode = list2.First((ExcelOpenXmlTemplate.XChildNode j) => j.ColIndex == mergeLimitColumn.ColIndex && j.RowIndex == childNodeNumber);
												ExcelOpenXmlTemplate.XChildNode limitedMaxNode = list2.Last((ExcelOpenXmlTemplate.XChildNode j) => j.ColIndex == mergeLimitColumn.ColIndex && j.InnerText == limitedNode.InnerText);
												xmlNodes = (from j in xmlNodes
												where j.RowIndex >= limitedNode.RowIndex && j.RowIndex <= limitedMaxNode.RowIndex
												select j).ToList<ExcelOpenXmlTemplate.XChildNode>();
											}
											ExcelOpenXmlTemplate.XChildNode firstRow = xmlNodes.FirstOrDefault<ExcelOpenXmlTemplate.XChildNode>();
											ExcelOpenXmlTemplate.XChildNode xchildNode2 = xmlNodes.LastOrDefault(delegate(ExcelOpenXmlTemplate.XChildNode s)
											{
												int rowIndex = s.RowIndex;
												ExcelOpenXmlTemplate.XChildNode firstRow = firstRow;
												int? num18 = ((firstRow != null) ? new int?(firstRow.RowIndex) : null) + xmlNodes.Count;
												if (rowIndex <= num18.GetValueOrDefault() & num18 != null)
												{
													int rowIndex2 = s.RowIndex;
													ExcelOpenXmlTemplate.XChildNode firstRow2 = firstRow;
													num18 = ((firstRow2 != null) ? new int?(firstRow2.RowIndex) : null);
													return !(rowIndex2 == num18.GetValueOrDefault() & num18 != null);
												}
												return false;
											});
											if (firstRow != null && xchildNode2 != null)
											{
												ExcelOpenXmlTemplate.XMergeCell xmergeCell = new ExcelOpenXmlTemplate.XMergeCell(firstRow.ColIndex, firstRow.RowIndex, xchildNode2.ColIndex, xchildNode2.RowIndex);
												ExcelOpenXmlTemplate.MergeCellIndex mergeCellIndex;
												if (!dictionary2.TryGetValue(xmergeCell.X1, out mergeCellIndex) || xmergeCell.Y1 < mergeCellIndex.RowStart || xmergeCell.Y2 > mergeCellIndex.RowEnd)
												{
													dictionary2[xmergeCell.X1] = new ExcelOpenXmlTemplate.MergeCellIndex(xmergeCell.Y1, xmergeCell.Y2);
													if (xrowInfo.RowMercells == null)
													{
														xrowInfo.RowMercells = new List<ExcelOpenXmlTemplate.XMergeCell>();
													}
													xrowInfo.RowMercells.Add(xmergeCell);
												}
											}
										}
									}
									childNode.SetAttribute("r", childNodeLetter + "{{$rowindex}}");
								}
							}
						}
					}
				}
				int num = 0;
				StringBuilder stringBuilder = new StringBuilder();
				int num2 = -1;
				int num3 = -1;
				bool flag = false;
				bool flag2 = false;
				int num4 = 0;
				IList<object> list3 = null;
				bool flag3 = false;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				bool flag4 = true;
				string text2 = "";
				string b = "";
				bool flag5 = false;
				int num8 = 0;
				for (int j2 = 0; j2 < this.XRowInfos.Count; j2++)
				{
					flag5 = false;
					text2 = "";
					ExcelOpenXmlTemplate.XRowInfo xrowInfo2 = this.XRowInfos[j2];
					XmlElement row = xrowInfo2.Row;
					if (row.InnerText.Contains("@group"))
					{
						flag = true;
						flag2 = true;
						num4 = j2;
						flag4 = true;
						b = "";
					}
					else if (row.InnerText.Contains("@endgroup"))
					{
						if (num5 >= list3.Count - 1)
						{
							flag = false;
							num4 = 0;
							list3 = null;
							flag3 = false;
							num7++;
						}
						else
						{
							j2 = num4;
							num5++;
							flag4 = false;
						}
					}
					else
					{
						if (row.InnerText.Contains("@header"))
						{
							flag5 = true;
						}
						else
						{
							if (row.InnerText.Contains("@merge") && mergeCells)
							{
								num8++;
								goto IL_13FA;
							}
							if (row.InnerText.Contains("@endmerge") && mergeCells)
							{
								num8++;
								goto IL_13FA;
							}
						}
						if (flag && !flag3 && xrowInfo2.CellIlListValues != null)
						{
							list3 = xrowInfo2.CellIlListValues;
							flag3 = true;
						}
						int num9 = flag2 ? (-1 + num5 * num6 - num7) : 0;
						if (flag)
						{
							if (flag4)
							{
								num6++;
							}
							if (list3 != null)
							{
								xrowInfo2.CellIEnumerableValuesCount = 1;
								xrowInfo2.CellIEnumerableValues = list3.Skip(num5).Take(1).ToList<object>();
							}
						}
						int num10 = int.Parse(row.GetAttribute("r")) + num + num9 - num8;
						string innerXml = row.InnerXml;
						stringBuilder.Clear().AppendFormat("<{0}", row.Name);
						foreach (XmlAttribute xmlAttribute in from XmlAttribute e in row.Attributes
						where e.Name != "r"
						select e)
						{
							stringBuilder.AppendFormat(" {0}=\"{1}\"", xmlAttribute.Name, xmlAttribute.Value);
						}
						string value = stringBuilder.ToString();
						if (xrowInfo2.CellIEnumerableValues != null)
						{
							bool flag6 = true;
							int num11 = 0;
							num2 = num10;
							foreach (object obj in xrowInfo2.CellIEnumerableValues)
							{
								num11++;
								stringBuilder.Clear().Append(value).AppendFormat(" r=\"{0}\">", num10).Append(innerXml).Replace("{{$rowindex}}", num10.ToString()).AppendFormat("</{0}>", row.Name);
								string text3 = stringBuilder.ToString();
								string text4 = "";
								string text5 = "";
								int num12 = text3.IndexOf("@if", StringComparison.Ordinal);
								int num13 = text3.IndexOf("@endif", StringComparison.Ordinal);
								if (num12 != -1 && num13 != -1)
								{
									text4 = text3.Substring(num12, num13 - num12 + 6);
								}
								string[] array2 = text4.Split(new char[]
								{
									'\n'
								});
								if (xrowInfo2.IsDictionary)
								{
									IDictionary<string, object> dictionary3 = obj as IDictionary<string, object>;
									for (int k = 0; k < array2.Length; k++)
									{
										if (array2[k].Contains("@if") || array2[k].Contains("@elseif"))
										{
											string[] array3 = array2[k].Replace("@elseif(", "").Replace("@if(", "").Replace(")", "").Split(new char[]
											{
												' '
											});
											if (ExcelOpenXmlTemplate.EvaluateStatement(dictionary3[array3[0]], array3[1], array3[2]))
											{
												text5 += array2[k + 1];
												break;
											}
										}
										else if (array2[k].Contains("@else"))
										{
											text5 += array2[k + 1];
											break;
										}
									}
									if (!string.IsNullOrEmpty(text5))
									{
										stringBuilder.Replace(text4, text5);
									}
									using (IEnumerator<KeyValuePair<string, ExcelOpenXmlTemplate.PropInfo>> enumerator6 = xrowInfo2.PropsMap.GetEnumerator())
									{
										while (enumerator6.MoveNext())
										{
											KeyValuePair<string, ExcelOpenXmlTemplate.PropInfo> propInfo = enumerator6.Current;
											string text6 = string.Concat(new string[]
											{
												"{{",
												xrowInfo2.IEnumerablePropName,
												".",
												propInfo.Key,
												"}}"
											});
											if (obj == null)
											{
												stringBuilder.Replace(text6, "");
											}
											else if (dictionary3.ContainsKey(propInfo.Key))
											{
												object obj2 = dictionary3[propInfo.Key];
												if (obj2 == null)
												{
													stringBuilder.Replace(text6, "");
												}
												else
												{
													string text7 = ExcelOpenXmlUtils.EncodeXML((obj2 != null) ? obj2.ToString() : null);
													Type underlyingTypePropType = propInfo.Value.UnderlyingTypePropType;
													if (underlyingTypePropType == typeof(bool))
													{
														text7 = (((bool)obj2) ? "1" : "0");
													}
													else if (underlyingTypePropType == typeof(DateTime))
													{
														text7 = ExcelOpenXmlTemplate.ConvertToDateTimeString(propInfo, obj2);
													}
													stringBuilder.Replace("@header" + text6, text7);
													stringBuilder.Replace(text6, text7);
													if (flag5 && row.InnerText.Contains(text6))
													{
														text2 += text7;
													}
												}
											}
										}
										goto IL_1084;
									}
									goto IL_B4E;
								}
								goto IL_B4E;
								IL_1084:
								if (flag5)
								{
									if (text2 == b)
									{
										num7++;
										continue;
									}
									b = text2;
								}
								if (!flag6)
								{
									int num14 = num;
									ExcelOpenXmlTemplate.XMergeCell ienumerableMercell = xrowInfo2.IEnumerableMercell;
									num = num14 + ((ienumerableMercell != null) ? ienumerableMercell.Height : 1);
								}
								flag6 = false;
								int num15 = num10;
								int num16 = num10;
								ExcelOpenXmlTemplate.XMergeCell ienumerableMercell2 = xrowInfo2.IEnumerableMercell;
								num10 = num16 + ((ienumerableMercell2 != null) ? ienumerableMercell2.Height : 1);
								this.ProcessFormulas(stringBuilder, num10);
								streamWriter.Write(ExcelOpenXmlTemplate.CleanXml(stringBuilder, endPrefix));
								if (xrowInfo2.RowMercells == null)
								{
									continue;
								}
								foreach (ExcelOpenXmlTemplate.XMergeCell mergeCell in xrowInfo2.RowMercells)
								{
									ExcelOpenXmlTemplate.XMergeCell xmergeCell2 = new ExcelOpenXmlTemplate.XMergeCell(mergeCell);
									xmergeCell2.Y1 = xmergeCell2.Y1 + num + num9 - num8;
									xmergeCell2.Y2 = xmergeCell2.Y2 + num + num9 - num8;
									this.NewXMergeCellInfos.Add(xmergeCell2);
								}
								if (num11 == xrowInfo2.CellIEnumerableValuesCount || xrowInfo2.IEnumerableMercell != null || xrowInfo2 == null)
								{
									continue;
								}
								ExcelOpenXmlTemplate.XMergeCell ienumerableMercell3 = xrowInfo2.IEnumerableMercell;
								bool flag7;
								if (ienumerableMercell3 == null)
								{
									flag7 = false;
								}
								else
								{
									int height = ienumerableMercell3.Height;
									flag7 = true;
								}
								if (flag7)
								{
									for (int l = 1; l < xrowInfo2.IEnumerableMercell.Height; l++)
									{
										num15++;
										XmlElement xmlElement = row.Clone() as XmlElement;
										xmlElement.SetAttribute("r", num15.ToString());
										foreach (object obj3 in xmlElement.SelectNodes("x:c", ExcelOpenXmlTemplate._ns))
										{
											XmlElement xmlElement2 = (XmlElement)obj3;
											xmlElement2.RemoveAttribute("t");
											foreach (object obj4 in xmlElement2.ChildNodes)
											{
												XmlNode oldChild = (XmlNode)obj4;
												xmlElement2.RemoveChild(oldChild);
											}
										}
										xmlElement.InnerXml = new StringBuilder(xmlElement.InnerXml).Replace("{{$rowindex}}", num15.ToString()).ToString();
										streamWriter.Write(ExcelOpenXmlTemplate.CleanXml(xmlElement.OuterXml, endPrefix));
									}
									continue;
								}
								continue;
								IL_B4E:
								if (xrowInfo2.IsDataTable)
								{
									DataRow dataRow = obj as DataRow;
									for (int m = 0; m < array2.Length; m++)
									{
										if (array2[m].Contains("@if") || array2[m].Contains("@elseif"))
										{
											string[] array4 = array2[m].Replace("@elseif(", "").Replace("@if(", "").Replace(")", "").Split(new char[]
											{
												' '
											});
											if (ExcelOpenXmlTemplate.EvaluateStatement(dataRow[array4[0]], array4[1], array4[2]))
											{
												text5 += array2[m + 1];
												break;
											}
										}
										else if (array2[m].Contains("@else"))
										{
											text5 += array2[m + 1];
											break;
										}
									}
									if (!string.IsNullOrEmpty(text5))
									{
										stringBuilder.Replace(text4, text5);
									}
									using (IEnumerator<KeyValuePair<string, ExcelOpenXmlTemplate.PropInfo>> enumerator6 = xrowInfo2.PropsMap.GetEnumerator())
									{
										while (enumerator6.MoveNext())
										{
											KeyValuePair<string, ExcelOpenXmlTemplate.PropInfo> propInfo2 = enumerator6.Current;
											string text8 = string.Concat(new string[]
											{
												"{{",
												xrowInfo2.IEnumerablePropName,
												".",
												propInfo2.Key,
												"}}"
											});
											if (obj == null)
											{
												stringBuilder.Replace(text8, "");
											}
											else
											{
												object obj5 = dataRow[propInfo2.Key];
												if (obj5 == null)
												{
													stringBuilder.Replace(text8, "");
												}
												else
												{
													string text9 = ExcelOpenXmlUtils.EncodeXML((obj5 != null) ? obj5.ToString() : null);
													Type underlyingTypePropType2 = propInfo2.Value.UnderlyingTypePropType;
													if (underlyingTypePropType2 == typeof(bool))
													{
														text9 = (((bool)obj5) ? "1" : "0");
													}
													else if (underlyingTypePropType2 == typeof(DateTime))
													{
														text9 = ExcelOpenXmlTemplate.ConvertToDateTimeString(propInfo2, obj5);
													}
													stringBuilder.Replace("@header" + text8, text9);
													stringBuilder.Replace(text8, text9);
													if (flag5 && row.InnerText.Contains(text8))
													{
														text2 += text9;
													}
												}
											}
										}
										goto IL_1084;
									}
								}
								for (int n = 0; n < array2.Length; n++)
								{
									if (array2[n].Contains("@if") || array2[n].Contains("@elseif"))
									{
										string[] array5 = array2[n].Replace("@elseif(", "").Replace("@if(", "").Replace(")", "").Split(new char[]
										{
											' '
										});
										ExcelOpenXmlTemplate.PropInfo propInfo3 = xrowInfo2.PropsMap[array5[0]];
										object tagValue = string.Empty;
										if (propInfo3.PropertyInfoOrFieldInfo == ExcelOpenXmlTemplate.PropertyInfoOrFieldInfo.PropertyInfo)
										{
											tagValue = xrowInfo2.PropsMap[array5[0]].PropertyInfo.GetValue(obj);
										}
										else if (propInfo3.PropertyInfoOrFieldInfo == ExcelOpenXmlTemplate.PropertyInfoOrFieldInfo.FieldInfo)
										{
											tagValue = xrowInfo2.PropsMap[array5[0]].FieldInfo.GetValue(obj);
										}
										if (ExcelOpenXmlTemplate.EvaluateStatement(tagValue, array5[1], array5[2]))
										{
											text5 += array2[n + 1];
											break;
										}
									}
									else if (array2[n].Contains("@else"))
									{
										text5 += array2[n + 1];
										break;
									}
								}
								if (!string.IsNullOrEmpty(text5))
								{
									stringBuilder.Replace(text4, text5);
								}
								foreach (KeyValuePair<string, ExcelOpenXmlTemplate.PropInfo> propInfo4 in xrowInfo2.PropsMap)
								{
									PropertyInfo propertyInfo = propInfo4.Value.PropertyInfo;
									string text10 = string.Concat(new string[]
									{
										"{{",
										xrowInfo2.IEnumerablePropName,
										".",
										propertyInfo.Name,
										"}}"
									});
									if (obj == null)
									{
										stringBuilder.Replace(text10, "");
									}
									else
									{
										object value2 = propertyInfo.GetValue(obj);
										if (value2 == null)
										{
											stringBuilder.Replace(text10, "");
										}
										else
										{
											string text11 = ExcelOpenXmlUtils.EncodeXML((value2 != null) ? value2.ToString() : null);
											Type type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
											decimal num17;
											if (type == typeof(bool))
											{
												text11 = (((bool)value2) ? "1" : "0");
											}
											else if (type == typeof(DateTime))
											{
												text11 = ExcelOpenXmlTemplate.ConvertToDateTimeString(propInfo4, value2);
											}
											else if (TypeHelper.IsNumericType(type, false) && decimal.TryParse(text11, out num17))
											{
												text11 = num17.ToString(CultureInfo.InvariantCulture);
											}
											stringBuilder.Replace("@header" + text10, text11);
											stringBuilder.Replace(text10, text11);
											if (flag5 && row.InnerText.Contains(text10))
											{
												text2 += text11;
											}
										}
									}
								}
								goto IL_1084;
							}
							num3 = num10 - 1;
						}
						else
						{
							stringBuilder.Clear().Append(value).AppendFormat(" r=\"{0}\">", num10).Append(innerXml).Replace("{{$rowindex}}", num10.ToString()).Replace("{{$enumrowstart}}", num2.ToString()).Replace("{{$enumrowend}}", num3.ToString()).AppendFormat("</{0}>", row.Name);
							this.ProcessFormulas(stringBuilder, num10);
							streamWriter.Write(ExcelOpenXmlTemplate.CleanXml(stringBuilder, endPrefix));
							if (xrowInfo2.RowMercells != null)
							{
								foreach (ExcelOpenXmlTemplate.XMergeCell mergeCell2 in xrowInfo2.RowMercells)
								{
									ExcelOpenXmlTemplate.XMergeCell xmergeCell3 = new ExcelOpenXmlTemplate.XMergeCell(mergeCell2);
									xmergeCell3.Y1 = xmergeCell3.Y1 + num + num9 - num8;
									xmergeCell3.Y2 = xmergeCell3.Y2 + num + num9 - num8;
									this.NewXMergeCellInfos.Add(xmergeCell3);
								}
							}
						}
					}
					IL_13FA:;
				}
				streamWriter.Write("</" + text + "sheetData>");
				if (this.NewXMergeCellInfos.Count != 0)
				{
					streamWriter.Write(string.Format("<{0}mergeCells count=\"{1}\">", text, this.NewXMergeCellInfos.Count));
					foreach (ExcelOpenXmlTemplate.XMergeCell xmergeCell4 in this.NewXMergeCellInfos)
					{
						streamWriter.Write(xmergeCell4.ToXmlString(text));
					}
					streamWriter.Write("</" + text + "mergeCells>");
				}
				streamWriter.Write(array[1]);
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000C880 File Offset: 0x0000AA80
		private void ProcessFormulas(StringBuilder rowXml, int rowIndex)
		{
			string text = rowXml.ToString();
			if (!text.Contains("$="))
			{
				return;
			}
			XmlReaderSettings settings = new XmlReaderSettings
			{
				NameTable = ExcelOpenXmlTemplate._ns.NameTable
			};
			XmlParserContext inputContext = new XmlParserContext(null, ExcelOpenXmlTemplate._ns, "", XmlSpace.Default);
			XmlReader reader = XmlReader.Create(new StringReader(text), settings, inputContext);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(reader);
			XmlElement xmlElement = xmlDocument.FirstChild as XmlElement;
			XmlNodeList xmlNodeList = xmlElement.SelectNodes("x:c", ExcelOpenXmlTemplate._ns);
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				XmlElement xmlElement2 = xmlNodeList.Item(i) as XmlElement;
				if (xmlElement2 != null)
				{
					foreach (object obj in xmlElement2.SelectNodes("x:v", ExcelOpenXmlTemplate._ns))
					{
						XmlElement xmlElement3 = (XmlElement)obj;
						if (xmlElement3.InnerText.StartsWith("$="))
						{
							XmlElement xmlElement4 = xmlElement2.OwnerDocument.CreateElement("f", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
							xmlElement4.InnerText = xmlElement3.InnerText.Substring(2);
							xmlElement2.InsertBefore(xmlElement4, xmlElement3);
							xmlElement2.RemoveChild(xmlElement3);
							string item = ExcelOpenXmlUtils.ConvertXyToCell(i + 1, rowIndex);
							this.CalcChainCellRefs.Add(item);
						}
					}
				}
			}
			rowXml.Clear();
			rowXml.Append(xmlElement.OuterXml);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000CA14 File Offset: 0x0000AC14
		private static string ConvertToDateTimeString(KeyValuePair<string, ExcelOpenXmlTemplate.PropInfo> propInfo, object cellValue)
		{
			string format = string.Empty;
			if (propInfo.Value.PropertyInfo == null)
			{
				format = "yyyy-MM-dd HH:mm:ss";
			}
			else
			{
				string text;
				if ((text = propInfo.Value.PropertyInfo.GetAttributeValue((ExcelFormatAttribute x) => x.Format, true)) == null)
				{
					text = (propInfo.Value.PropertyInfo.GetAttributeValue((ExcelColumnAttribute x) => x.Format, true) ?? "yyyy-MM-dd HH:mm:ss");
				}
				format = text;
			}
			if (!(cellValue is DateTime?))
			{
				return null;
			}
			DateTime? dateTime;
			return dateTime.GetValueOrDefault().ToString(format);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000CADA File Offset: 0x0000ACDA
		private static StringBuilder CleanXml(StringBuilder xml, string endPrefix)
		{
			return xml.Replace("xmlns:x14ac=\"http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac\"", "").Replace("xmlns" + endPrefix + "=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"", "");
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000CB06 File Offset: 0x0000AD06
		private static string CleanXml(string xml, string endPrefix)
		{
			return ExcelOpenXmlTemplate.CleanXml(new StringBuilder(xml), endPrefix).ToString();
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		private void ReplaceSharedStringsToStr(IDictionary<int, string> sharedStrings, ref XmlNodeList rows)
		{
			foreach (object obj in rows)
			{
				foreach (object obj2 in ((XmlElement)obj).SelectNodes("x:c", ExcelOpenXmlTemplate._ns))
				{
					XmlElement xmlElement = (XmlElement)obj2;
					string attribute = xmlElement.GetAttribute("t");
					XmlNode xmlNode = xmlElement.SelectSingleNode("x:v", ExcelOpenXmlTemplate._ns);
					if (xmlNode != null && xmlNode.InnerText != null && attribute == "s" && sharedStrings.ContainsKey(int.Parse(xmlNode.InnerText)))
					{
						xmlNode.InnerText = sharedStrings[int.Parse(xmlNode.InnerText)];
						xmlElement.SetAttribute("t", "str");
					}
				}
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000CC3C File Offset: 0x0000AE3C
		private void UpdateDimensionAndGetRowsInfo(IDictionary<string, object> inputMaps, ref XmlDocument doc, ref XmlNodeList rows, bool changeRowIndex = true)
		{
			XmlElement xmlElement = doc.SelectSingleNode("/x:worksheet/x:dimension", ExcelOpenXmlTemplate._ns) as XmlElement;
			if (xmlElement == null)
			{
				throw new NotImplementedException("Excel Dimension Xml is null, please issue file for me. https://github.com/shps951023/MiniExcel/issues");
			}
			int num = 0;
			foreach (object obj in rows)
			{
				XmlElement xmlElement2 = (XmlElement)obj;
				ExcelOpenXmlTemplate.XRowInfo xrowInfo = new ExcelOpenXmlTemplate.XRowInfo
				{
					Row = xmlElement2
				};
				this.XRowInfos.Add(xrowInfo);
				foreach (object obj2 in xmlElement2.SelectNodes("x:c", ExcelOpenXmlTemplate._ns))
				{
					XmlElement xmlElement3 = (XmlElement)obj2;
					string attribute = xmlElement3.GetAttribute("r");
					if (this.XMergeCellInfos.ContainsKey(attribute))
					{
						if (xrowInfo.RowMercells == null)
						{
							xrowInfo.RowMercells = new List<ExcelOpenXmlTemplate.XMergeCell>();
						}
						xrowInfo.RowMercells.Add(this.XMergeCellInfos[attribute]);
					}
					if (changeRowIndex)
					{
						xmlElement3.SetAttribute("r", StringHelper.GetLetter(attribute) + "{{$rowindex}}");
					}
					XmlNode xmlNode = xmlElement3.SelectSingleNode("x:v", ExcelOpenXmlTemplate._ns);
					if (xmlNode != null && xmlNode.InnerText != null)
					{
						string[] array = (from Match x in ExcelOpenXmlTemplate._isExpressionRegex.Matches(xmlNode.InnerText)
						group x by x.Value into varGroup
						select varGroup.First<Match>().Value).ToArray<string>();
						int num2 = array.Length;
						bool flag = num2 > 1 || (num2 == 1 && xmlNode.InnerText != "{{" + array[0] + "}}");
						foreach (string text in array)
						{
							xrowInfo.FormatText = text;
							string[] array3 = text.Split(new char[]
							{
								'.'
							});
							if (!array3[0].StartsWith("$"))
							{
								if (!inputMaps.ContainsKey(array3[0]))
								{
									if (this._configuration.IgnoreTemplateParameterMissing)
									{
										xmlNode.InnerText = xmlNode.InnerText.Replace("{{" + array3[0] + "}}", "");
										break;
									}
									throw new KeyNotFoundException("Please check " + array3[0] + " parameter, it's not exist.");
								}
								else
								{
									object obj3 = inputMaps[array3[0]];
									if ((obj3 is IEnumerable || obj3 is IList<object>) && !(obj3 is string))
									{
										if (this.XMergeCellInfos.ContainsKey(attribute) && xrowInfo.IEnumerableMercell == null)
										{
											xrowInfo.IEnumerableMercell = this.XMergeCellInfos[attribute];
										}
										xrowInfo.CellIEnumerableValues = (obj3 as IEnumerable);
										xrowInfo.CellIlListValues = (obj3 as IList<object>);
										if (xrowInfo.IEnumerableGenricType == null)
										{
											bool flag2 = true;
											foreach (object obj4 in xrowInfo.CellIEnumerableValues)
											{
												ExcelOpenXmlTemplate.XRowInfo xrowInfo2 = xrowInfo;
												int j = xrowInfo2.CellIEnumerableValuesCount;
												xrowInfo2.CellIEnumerableValuesCount = j + 1;
												if (xrowInfo.IEnumerableGenricType == null && obj4 != null)
												{
													xrowInfo.IEnumerablePropName = array3[0];
													xrowInfo.IEnumerableGenricType = obj4.GetType();
													if (obj4 is IDictionary<string, object>)
													{
														xrowInfo.IsDictionary = true;
														IDictionary<string, object> dic = obj4 as IDictionary<string, object>;
														xrowInfo.PropsMap = dic.Keys.ToDictionary((string key) => key, delegate(string key)
														{
															if (dic[key] == null)
															{
																return new ExcelOpenXmlTemplate.PropInfo
																{
																	UnderlyingTypePropType = typeof(object)
																};
															}
															return new ExcelOpenXmlTemplate.PropInfo
															{
																UnderlyingTypePropType = (Nullable.GetUnderlyingType(dic[key].GetType()) ?? dic[key].GetType())
															};
														});
													}
													else
													{
														Dictionary<string, ExcelOpenXmlTemplate.PropInfo> dictionary = new Dictionary<string, ExcelOpenXmlTemplate.PropInfo>();
														foreach (PropertyInfo propertyInfo in xrowInfo.IEnumerableGenricType.GetProperties())
														{
															dictionary.Add(propertyInfo.Name, new ExcelOpenXmlTemplate.PropInfo
															{
																PropertyInfo = propertyInfo,
																PropertyInfoOrFieldInfo = ExcelOpenXmlTemplate.PropertyInfoOrFieldInfo.PropertyInfo,
																UnderlyingTypePropType = (Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType)
															});
														}
														foreach (FieldInfo fieldInfo in xrowInfo.IEnumerableGenricType.GetFields())
														{
															if (!dictionary.ContainsKey(fieldInfo.Name))
															{
																dictionary.Add(fieldInfo.Name, new ExcelOpenXmlTemplate.PropInfo
																{
																	FieldInfo = fieldInfo,
																	PropertyInfoOrFieldInfo = ExcelOpenXmlTemplate.PropertyInfoOrFieldInfo.FieldInfo,
																	UnderlyingTypePropType = (Nullable.GetUnderlyingType(fieldInfo.FieldType) ?? fieldInfo.FieldType)
																});
															}
														}
														xrowInfo.PropsMap = dictionary;
													}
												}
												if (!flag2)
												{
													num += ((xrowInfo.IEnumerableMercell == null) ? 1 : xrowInfo.IEnumerableMercell.Height);
												}
												flag2 = false;
											}
										}
										if (xrowInfo.PropsMap == null)
										{
											xmlNode.InnerText = xmlNode.InnerText.Replace("{{" + array3[0] + "}}", array3[1]);
											break;
										}
										if (!xrowInfo.PropsMap.ContainsKey(array3[1]))
										{
											xmlNode.InnerText = xmlNode.InnerText.Replace(string.Concat(new string[]
											{
												"{{",
												array3[0],
												".",
												array3[1],
												"}}"
											}), "");
										}
										else
										{
											Type underlyingTypePropType = xrowInfo.PropsMap[array3[1]].UnderlyingTypePropType;
											if (flag)
											{
												xmlElement3.SetAttribute("t", "str");
												break;
											}
											if (TypeHelper.IsNumericType(underlyingTypePropType, false))
											{
												xmlElement3.SetAttribute("t", "n");
												break;
											}
											if (Type.GetTypeCode(underlyingTypePropType) == TypeCode.Boolean)
											{
												xmlElement3.SetAttribute("t", "b");
												break;
											}
											if (Type.GetTypeCode(underlyingTypePropType) == TypeCode.DateTime)
											{
												xmlElement3.SetAttribute("t", "str");
												break;
											}
											break;
										}
									}
									else if (obj3 is DataTable)
									{
										DataTable dataTable = obj3 as DataTable;
										if (xrowInfo.CellIEnumerableValues == null)
										{
											xrowInfo.IEnumerablePropName = array3[0];
											xrowInfo.IEnumerableGenricType = typeof(DataRow);
											xrowInfo.IsDataTable = true;
											xrowInfo.CellIEnumerableValues = dataTable.Rows.Cast<object>().ToList<object>();
											xrowInfo.CellIlListValues = dataTable.Rows.Cast<object>().ToList<object>();
											bool flag3 = true;
											foreach (object obj5 in xrowInfo.CellIEnumerableValues)
											{
												if (!flag3)
												{
													num++;
												}
												flag3 = false;
											}
											xrowInfo.PropsMap = dataTable.Columns.Cast<DataColumn>().ToDictionary((DataColumn col) => col.ColumnName, (DataColumn col) => new ExcelOpenXmlTemplate.PropInfo
											{
												UnderlyingTypePropType = Nullable.GetUnderlyingType(col.DataType)
											});
										}
										DataColumn dataColumn = dataTable.Columns[array3[1]];
										Type type = Nullable.GetUnderlyingType(dataColumn.DataType) ?? dataColumn.DataType;
										if (!xrowInfo.PropsMap.ContainsKey(array3[1]))
										{
											throw new InvalidDataException(array3[0] + " doesn't have " + array3[1] + " property");
										}
										if (flag)
										{
											xmlElement3.SetAttribute("t", "str");
										}
										else if (TypeHelper.IsNumericType(type, false))
										{
											xmlElement3.SetAttribute("t", "n");
										}
										else if (Type.GetTypeCode(type) == TypeCode.Boolean)
										{
											xmlElement3.SetAttribute("t", "b");
										}
										else if (Type.GetTypeCode(type) == TypeCode.DateTime)
										{
											xmlElement3.SetAttribute("t", "str");
										}
									}
									else
									{
										string text2 = (obj3 != null) ? obj3.ToString() : null;
										decimal num3;
										if (flag || obj3 is string)
										{
											xmlElement3.SetAttribute("t", "str");
										}
										else if (decimal.TryParse(text2, out num3))
										{
											xmlElement3.SetAttribute("t", "n");
											text2 = num3.ToString(CultureInfo.InvariantCulture);
										}
										else if (obj3 is bool)
										{
											xmlElement3.SetAttribute("t", "b");
											text2 = (((bool)obj3) ? "1" : "0");
										}
										else if (obj3 is DateTime || obj3 is DateTime?)
										{
											text2 = ((DateTime)obj3).ToString("yyyy-MM-dd HH:mm:ss");
										}
										xmlNode.InnerText = xmlNode.InnerText.Replace("{{" + array3[0] + "}}", text2);
									}
								}
							}
						}
					}
				}
			}
			string[] array4 = xmlElement.GetAttribute("ref").Split(new char[]
			{
				':'
			});
			if (array4.Length == 2)
			{
				string arg = new string(array4[1].Where(new Func<char, bool>(char.IsLetter)).ToArray<char>());
				int num4 = int.Parse(new string(array4[1].Where(new Func<char, bool>(char.IsDigit)).ToArray<char>()));
				xmlElement.SetAttribute("ref", string.Format("{0}:{1}{2}", array4[0], arg, num4 + num));
				return;
			}
			string arg2 = new string(array4[0].Where(new Func<char, bool>(char.IsLetter)).ToArray<char>());
			int num5 = int.Parse(new string(array4[0].Where(new Func<char, bool>(char.IsDigit)).ToArray<char>()));
			xmlElement.SetAttribute("ref", string.Format("A1:{0}{1}", arg2, num5 + num));
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000D6D4 File Offset: 0x0000B8D4
		private static bool EvaluateStatement(object tagValue, string comparisonOperator, string value)
		{
			bool result = false;
			if (tagValue is double)
			{
				double num = (double)tagValue;
				double num2 = num;
				double num3;
				if (double.TryParse(value, out num3))
				{
					if (!(comparisonOperator == "=="))
					{
						if (!(comparisonOperator == "!="))
						{
							if (!(comparisonOperator == ">"))
							{
								if (!(comparisonOperator == "<"))
								{
									if (!(comparisonOperator == ">="))
									{
										if (comparisonOperator == "<=")
										{
											result = (num2 <= num3);
										}
									}
									else
									{
										result = (num2 >= num3);
									}
								}
								else
								{
									result = (num2 < num3);
								}
							}
							else
							{
								result = (num2 > num3);
							}
						}
						else
						{
							result = !num2.Equals(num3);
						}
					}
					else
					{
						result = num2.Equals(num3);
					}
				}
			}
			else if (tagValue is int)
			{
				int num4 = (int)tagValue;
				int num5 = num4;
				int num6;
				if (int.TryParse(value, out num6))
				{
					if (!(comparisonOperator == "=="))
					{
						if (!(comparisonOperator == "!="))
						{
							if (!(comparisonOperator == ">"))
							{
								if (!(comparisonOperator == "<"))
								{
									if (!(comparisonOperator == ">="))
									{
										if (comparisonOperator == "<=")
										{
											result = (num5 <= num6);
										}
									}
									else
									{
										result = (num5 >= num6);
									}
								}
								else
								{
									result = (num5 < num6);
								}
							}
							else
							{
								result = (num5 > num6);
							}
						}
						else
						{
							result = !num5.Equals(num6);
						}
					}
					else
					{
						result = num5.Equals(num6);
					}
				}
			}
			else if (tagValue is DateTime)
			{
				DateTime dateTime = (DateTime)tagValue;
				DateTime t = dateTime;
				DateTime dateTime2;
				if (DateTime.TryParse(value, out dateTime2))
				{
					if (!(comparisonOperator == "=="))
					{
						if (!(comparisonOperator == "!="))
						{
							if (!(comparisonOperator == ">"))
							{
								if (!(comparisonOperator == "<"))
								{
									if (!(comparisonOperator == ">="))
									{
										if (comparisonOperator == "<=")
										{
											result = (t <= dateTime2);
										}
									}
									else
									{
										result = (t >= dateTime2);
									}
								}
								else
								{
									result = (t < dateTime2);
								}
							}
							else
							{
								result = (t > dateTime2);
							}
						}
						else
						{
							result = !t.Equals(dateTime2);
						}
					}
					else
					{
						result = t.Equals(dateTime2);
					}
				}
			}
			else
			{
				string text = tagValue as string;
				if (text != null)
				{
					string a = text;
					if (!(comparisonOperator == "=="))
					{
						if (comparisonOperator == "!=")
						{
							result = (a != value);
						}
					}
					else
					{
						result = (a == value);
					}
				}
			}
			return result;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000D984 File Offset: 0x0000BB84
		public void MergeSameCells(string path)
		{
			using (FileStream fileStream = FileHelper.OpenSharedRead(path))
			{
				this.MergeSameCellsImpl(fileStream);
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000D9BC File Offset: 0x0000BBBC
		public void MergeSameCells(byte[] fileInBytes)
		{
			using (Stream stream = new MemoryStream(fileInBytes))
			{
				this.MergeSameCellsImpl(stream);
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000D9F4 File Offset: 0x0000BBF4
		private void MergeSameCellsImpl(Stream stream)
		{
			stream.CopyTo(this._stream);
			ExcelOpenXmlSheetReader excelOpenXmlSheetReader = new ExcelOpenXmlSheetReader(this._stream, null);
			ExcelOpenXmlZip excelOpenXmlZip = new ExcelOpenXmlZip(this._stream, ZipArchiveMode.Update, true, Encoding.UTF8);
			IDictionary<int, string> sharedStrings = excelOpenXmlSheetReader._sharedStrings;
			foreach (ZipArchiveEntry zipArchiveEntry in (from w in excelOpenXmlZip.zipFile.Entries
			where w.FullName.StartsWith("xl/worksheets/sheet", StringComparison.OrdinalIgnoreCase) || w.FullName.StartsWith("/xl/worksheets/sheet", StringComparison.OrdinalIgnoreCase)
			select w).ToList<ZipArchiveEntry>())
			{
				this.XRowInfos = new List<ExcelOpenXmlTemplate.XRowInfo>();
				this.XMergeCellInfos = new Dictionary<string, ExcelOpenXmlTemplate.XMergeCell>();
				this.NewXMergeCellInfos = new List<ExcelOpenXmlTemplate.XMergeCell>();
				Stream sheetStream = zipArchiveEntry.Open();
				string fullName = zipArchiveEntry.FullName;
				using (Stream stream2 = excelOpenXmlZip.zipFile.CreateEntry(fullName).Open())
				{
					this.GenerateSheetXmlImpl(zipArchiveEntry, stream2, sheetStream, new Dictionary<string, object>(), sharedStrings, true);
				}
			}
			excelOpenXmlZip.zipFile.Dispose();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000DB24 File Offset: 0x0000BD24
		public Task MergeSameCellsAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.Run(delegate()
			{
				this.MergeSameCells(path);
			}, cancellationToken);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000DB4A File Offset: 0x0000BD4A
		public Task MergeSameCellsAsync(byte[] fileInBytes, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.Run(delegate()
			{
				this.MergeSameCells(fileInBytes);
			}, cancellationToken);
		}

		// Token: 0x040000E7 RID: 231
		private static readonly XmlNamespaceManager _ns;

		// Token: 0x040000E8 RID: 232
		private static readonly Regex _isExpressionRegex = new Regex("(?<={{).*?(?=}})");

		// Token: 0x040000E9 RID: 233
		private readonly Stream _stream;

		// Token: 0x040000EA RID: 234
		private readonly OpenXmlConfiguration _configuration;

		// Token: 0x040000EB RID: 235
		private readonly IInputValueExtractor _inputValueExtractor;

		// Token: 0x040000EC RID: 236
		private readonly StringBuilder _calcChainContent = new StringBuilder();

		// Token: 0x040000EE RID: 238
		private readonly List<string> CalcChainCellRefs = new List<string>();

		// Token: 0x020000F1 RID: 241
		public class XRowInfo
		{
			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x0600058E RID: 1422 RVA: 0x0001E4D9 File Offset: 0x0001C6D9
			// (set) Token: 0x0600058F RID: 1423 RVA: 0x0001E4E1 File Offset: 0x0001C6E1
			public string FormatText { get; set; }

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x06000590 RID: 1424 RVA: 0x0001E4EA File Offset: 0x0001C6EA
			// (set) Token: 0x06000591 RID: 1425 RVA: 0x0001E4F2 File Offset: 0x0001C6F2
			public string IEnumerablePropName { get; set; }

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x06000592 RID: 1426 RVA: 0x0001E4FB File Offset: 0x0001C6FB
			// (set) Token: 0x06000593 RID: 1427 RVA: 0x0001E503 File Offset: 0x0001C703
			public XmlElement Row { get; set; }

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x06000594 RID: 1428 RVA: 0x0001E50C File Offset: 0x0001C70C
			// (set) Token: 0x06000595 RID: 1429 RVA: 0x0001E514 File Offset: 0x0001C714
			public Type IEnumerableGenricType { get; set; }

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x06000596 RID: 1430 RVA: 0x0001E51D File Offset: 0x0001C71D
			// (set) Token: 0x06000597 RID: 1431 RVA: 0x0001E525 File Offset: 0x0001C725
			public IDictionary<string, ExcelOpenXmlTemplate.PropInfo> PropsMap { get; set; }

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001E52E File Offset: 0x0001C72E
			// (set) Token: 0x06000599 RID: 1433 RVA: 0x0001E536 File Offset: 0x0001C736
			public bool IsDictionary { get; set; }

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x0600059A RID: 1434 RVA: 0x0001E53F File Offset: 0x0001C73F
			// (set) Token: 0x0600059B RID: 1435 RVA: 0x0001E547 File Offset: 0x0001C747
			public bool IsDataTable { get; set; }

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001E550 File Offset: 0x0001C750
			// (set) Token: 0x0600059D RID: 1437 RVA: 0x0001E558 File Offset: 0x0001C758
			public int CellIEnumerableValuesCount { get; set; }

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x0600059E RID: 1438 RVA: 0x0001E561 File Offset: 0x0001C761
			// (set) Token: 0x0600059F RID: 1439 RVA: 0x0001E569 File Offset: 0x0001C769
			public IList<object> CellIlListValues { get; set; }

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001E572 File Offset: 0x0001C772
			// (set) Token: 0x060005A1 RID: 1441 RVA: 0x0001E57A File Offset: 0x0001C77A
			public IEnumerable CellIEnumerableValues { get; set; }

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0001E583 File Offset: 0x0001C783
			// (set) Token: 0x060005A3 RID: 1443 RVA: 0x0001E58B File Offset: 0x0001C78B
			public ExcelOpenXmlTemplate.XMergeCell IEnumerableMercell { get; set; }

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0001E594 File Offset: 0x0001C794
			// (set) Token: 0x060005A5 RID: 1445 RVA: 0x0001E59C File Offset: 0x0001C79C
			public List<ExcelOpenXmlTemplate.XMergeCell> RowMercells { get; set; }
		}

		// Token: 0x020000F2 RID: 242
		public class PropInfo
		{
			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0001E5AD File Offset: 0x0001C7AD
			// (set) Token: 0x060005A8 RID: 1448 RVA: 0x0001E5B5 File Offset: 0x0001C7B5
			public PropertyInfo PropertyInfo { get; set; }

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0001E5BE File Offset: 0x0001C7BE
			// (set) Token: 0x060005AA RID: 1450 RVA: 0x0001E5C6 File Offset: 0x0001C7C6
			public FieldInfo FieldInfo { get; set; }

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x060005AB RID: 1451 RVA: 0x0001E5CF File Offset: 0x0001C7CF
			// (set) Token: 0x060005AC RID: 1452 RVA: 0x0001E5D7 File Offset: 0x0001C7D7
			public Type UnderlyingTypePropType { get; set; }

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001E5E0 File Offset: 0x0001C7E0
			// (set) Token: 0x060005AE RID: 1454 RVA: 0x0001E5E8 File Offset: 0x0001C7E8
			public ExcelOpenXmlTemplate.PropertyInfoOrFieldInfo PropertyInfoOrFieldInfo { get; set; }
		}

		// Token: 0x020000F3 RID: 243
		public enum PropertyInfoOrFieldInfo
		{
			// Token: 0x040004E0 RID: 1248
			None,
			// Token: 0x040004E1 RID: 1249
			PropertyInfo,
			// Token: 0x040004E2 RID: 1250
			FieldInfo
		}

		// Token: 0x020000F4 RID: 244
		public class XMergeCell
		{
			// Token: 0x060005B0 RID: 1456 RVA: 0x0001E5FC File Offset: 0x0001C7FC
			public XMergeCell(ExcelOpenXmlTemplate.XMergeCell mergeCell)
			{
				this.Width = mergeCell.Width;
				this.Height = mergeCell.Height;
				this.X1 = mergeCell.X1;
				this.Y1 = mergeCell.Y1;
				this.X2 = mergeCell.X2;
				this.Y2 = mergeCell.Y2;
				this.MergeCell = mergeCell.MergeCell;
			}

			// Token: 0x060005B1 RID: 1457 RVA: 0x0001E664 File Offset: 0x0001C864
			public XMergeCell(XmlElement mergeCell)
			{
				string[] array = mergeCell.Attributes["ref"].Value.Split(new char[]
				{
					':'
				});
				string content = array[0];
				this.X1 = ColumnHelper.GetColumnIndex(StringHelper.GetLetter(array[0]));
				this.Y1 = StringHelper.GetNumber(content);
				string content2 = array[1];
				this.X2 = ColumnHelper.GetColumnIndex(StringHelper.GetLetter(array[1]));
				this.Y2 = StringHelper.GetNumber(content2);
				this.Width = Math.Abs(this.X1 - this.X2) + 1;
				this.Height = Math.Abs(this.Y1 - this.Y2) + 1;
			}

			// Token: 0x060005B2 RID: 1458 RVA: 0x0001E718 File Offset: 0x0001C918
			public XMergeCell(string x1, int y1, string x2, int y2)
			{
				this.X1 = ColumnHelper.GetColumnIndex(x1);
				this.Y1 = y1;
				this.X2 = ColumnHelper.GetColumnIndex(x2);
				this.Y2 = y2;
				this.Width = Math.Abs(this.X1 - this.X2) + 1;
				this.Height = Math.Abs(this.Y1 - this.Y2) + 1;
			}

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0001E786 File Offset: 0x0001C986
			public string XY1
			{
				get
				{
					return string.Format("{0}{1}", ColumnHelper.GetAlphabetColumnName(this.X1), this.Y1);
				}
			}

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0001E7A8 File Offset: 0x0001C9A8
			// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0001E7B0 File Offset: 0x0001C9B0
			public int X1 { get; set; }

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0001E7B9 File Offset: 0x0001C9B9
			// (set) Token: 0x060005B7 RID: 1463 RVA: 0x0001E7C1 File Offset: 0x0001C9C1
			public int Y1 { get; set; }

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0001E7CA File Offset: 0x0001C9CA
			public string XY2
			{
				get
				{
					return string.Format("{0}{1}", ColumnHelper.GetAlphabetColumnName(this.X2), this.Y2);
				}
			}

			// Token: 0x170000FC RID: 252
			// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0001E7EC File Offset: 0x0001C9EC
			// (set) Token: 0x060005BA RID: 1466 RVA: 0x0001E7F4 File Offset: 0x0001C9F4
			public int X2 { get; set; }

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x060005BB RID: 1467 RVA: 0x0001E7FD File Offset: 0x0001C9FD
			// (set) Token: 0x060005BC RID: 1468 RVA: 0x0001E805 File Offset: 0x0001CA05
			public int Y2 { get; set; }

			// Token: 0x170000FE RID: 254
			// (get) Token: 0x060005BD RID: 1469 RVA: 0x0001E810 File Offset: 0x0001CA10
			public string Ref
			{
				get
				{
					return string.Format("{0}{1}:{2}{3}", new object[]
					{
						ColumnHelper.GetAlphabetColumnName(this.X1),
						this.Y1,
						ColumnHelper.GetAlphabetColumnName(this.X2),
						this.Y2
					});
				}
			}

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001E865 File Offset: 0x0001CA65
			// (set) Token: 0x060005BF RID: 1471 RVA: 0x0001E86D File Offset: 0x0001CA6D
			public XmlElement MergeCell { get; set; }

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0001E876 File Offset: 0x0001CA76
			// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0001E87E File Offset: 0x0001CA7E
			public int Width { get; internal set; }

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0001E887 File Offset: 0x0001CA87
			// (set) Token: 0x060005C3 RID: 1475 RVA: 0x0001E88F File Offset: 0x0001CA8F
			public int Height { get; internal set; }

			// Token: 0x060005C4 RID: 1476 RVA: 0x0001E898 File Offset: 0x0001CA98
			public string ToXmlString(string prefix)
			{
				return string.Format("<{0}mergeCell ref=\"{1}{2}:{3}{4}\"/>", new object[]
				{
					prefix,
					ColumnHelper.GetAlphabetColumnName(this.X1),
					this.Y1,
					ColumnHelper.GetAlphabetColumnName(this.X2),
					this.Y2
				});
			}
		}

		// Token: 0x020000F5 RID: 245
		private class MergeCellIndex
		{
			// Token: 0x17000102 RID: 258
			// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001E8F1 File Offset: 0x0001CAF1
			// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0001E8F9 File Offset: 0x0001CAF9
			public int RowStart { get; set; }

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0001E902 File Offset: 0x0001CB02
			// (set) Token: 0x060005C8 RID: 1480 RVA: 0x0001E90A File Offset: 0x0001CB0A
			public int RowEnd { get; set; }

			// Token: 0x060005C9 RID: 1481 RVA: 0x0001E913 File Offset: 0x0001CB13
			public MergeCellIndex(int rowStart, int rowEnd)
			{
				this.RowStart = rowStart;
				this.RowEnd = rowEnd;
			}
		}

		// Token: 0x020000F6 RID: 246
		private class XChildNode
		{
			// Token: 0x17000104 RID: 260
			// (get) Token: 0x060005CA RID: 1482 RVA: 0x0001E929 File Offset: 0x0001CB29
			// (set) Token: 0x060005CB RID: 1483 RVA: 0x0001E931 File Offset: 0x0001CB31
			public string InnerText { get; set; }

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x060005CC RID: 1484 RVA: 0x0001E93A File Offset: 0x0001CB3A
			// (set) Token: 0x060005CD RID: 1485 RVA: 0x0001E942 File Offset: 0x0001CB42
			public string ColIndex { get; set; }

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001E94B File Offset: 0x0001CB4B
			// (set) Token: 0x060005CF RID: 1487 RVA: 0x0001E953 File Offset: 0x0001CB53
			public int RowIndex { get; set; }
		}
	}
}
