using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.Utils;
using MiniExcelLibs.Zip;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x0200003F RID: 63
	internal class ExcelOpenXmlSheetReader : IExcelReader, IDisposable
	{
		// Token: 0x06000191 RID: 401 RVA: 0x00006E2F File Offset: 0x0000502F
		public ExcelOpenXmlSheetReader(Stream stream, IConfiguration configuration)
		{
			this._archive = new ExcelOpenXmlZip(stream, ZipArchiveMode.Read, false, null);
			this._config = (((OpenXmlConfiguration)configuration) ?? OpenXmlConfiguration.DefaultConfig);
			this.SetSharedStrings();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006E61 File Offset: 0x00005061
		public IEnumerable<IDictionary<string, object>> Query(bool useHeaderRow, string sheetName, string startCell)
		{
			int startColumnIndex;
			int startRowIndex;
			if (!ReferenceHelper.ParseReference(startCell, out startColumnIndex, out startRowIndex))
			{
				throw new InvalidDataException("startCell " + startCell + " is Invalid");
			}
			int num = startColumnIndex;
			startColumnIndex = num - 1;
			num = startRowIndex;
			startRowIndex = num - 1;
			ZipArchiveEntry[] source = (from w in this._archive.entries
			where w.FullName.StartsWith("xl/worksheets/sheet", StringComparison.OrdinalIgnoreCase) || w.FullName.StartsWith("/xl/worksheets/sheet", StringComparison.OrdinalIgnoreCase)
			select w).ToArray<ZipArchiveEntry>();
			ZipArchiveEntry zipArchiveEntry = null;
			if (sheetName != null)
			{
				this.SetWorkbookRels(this._archive.entries);
				SheetRecord sheetRecord = this._sheetRecords.SingleOrDefault((SheetRecord _) => _.Name == sheetName);
				if (sheetRecord == null && this._config.DynamicSheets != null)
				{
					DynamicExcelSheet sheetConfig = this._config.DynamicSheets.FirstOrDefault((DynamicExcelSheet ds) => ds.Key == sheetName);
					if (sheetConfig != null)
					{
						sheetRecord = this._sheetRecords.SingleOrDefault((SheetRecord _) => _.Name == sheetConfig.Name);
					}
				}
				if (sheetRecord == null)
				{
					throw new InvalidOperationException("Please check sheetName/Index is correct");
				}
				zipArchiveEntry = source.Single((ZipArchiveEntry w) => w.FullName == "xl/" + sheetRecord.Path || w.FullName == "/xl/" + sheetRecord.Path || w.FullName == sheetRecord.Path || sheetRecord.Path == "/" + w.FullName);
			}
			else if (source.Count<ZipArchiveEntry>() > 1)
			{
				this.SetWorkbookRels(this._archive.entries);
				SheetRecord s = this._sheetRecords[0];
				zipArchiveEntry = source.Single((ZipArchiveEntry w) => w.FullName == "xl/" + s.Path || w.FullName == "/xl/" + s.Path || w.FullName.TrimStart(new char[]
				{
					'/'
				}) == s.Path.TrimStart(new char[]
				{
					'/'
				}));
			}
			else
			{
				zipArchiveEntry = source.Single<ZipArchiveEntry>();
			}
			if (this._config.FillMergedCells)
			{
				this._mergeCells = new MergeCells();
				using (Stream stream = zipArchiveEntry.Open())
				{
					using (XmlReader xmlReader = XmlReader.Create(stream, ExcelOpenXmlSheetReader._xmlSettings))
					{
						if (!XmlReaderHelper.IsStartElement(xmlReader, "worksheet", ExcelOpenXmlSheetReader._ns))
						{
							yield break;
						}
						while (xmlReader.Read())
						{
							if (XmlReaderHelper.IsStartElement(xmlReader, "mergeCells", ExcelOpenXmlSheetReader._ns))
							{
								if (!XmlReaderHelper.ReadFirstContent(xmlReader))
								{
									yield break;
								}
								while (!xmlReader.EOF)
								{
									if (XmlReaderHelper.IsStartElement(xmlReader, "mergeCell", ExcelOpenXmlSheetReader._ns))
									{
										string[] array = xmlReader.GetAttribute("ref").Split(new char[]
										{
											':'
										});
										if (array.Length != 1)
										{
											int num2;
											int num3;
											ReferenceHelper.ParseReference(array[0], out num2, out num3);
											int num4;
											int num5;
											ReferenceHelper.ParseReference(array[1], out num4, out num5);
											this._mergeCells.MergesValues.Add(array[0], null);
											bool flag = true;
											for (int j = num2; j <= num4; j++)
											{
												for (int k = num3; k <= num5; k++)
												{
													if (!flag)
													{
														this._mergeCells.MergesMap.Add(ReferenceHelper.ConvertXyToCell(j, k), array[0]);
													}
													flag = false;
												}
											}
											XmlReaderHelper.SkipContent(xmlReader);
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
			}
			bool withoutCR = false;
			int num6 = -1;
			int maxColumnIndex = -1;
			using (Stream stream2 = zipArchiveEntry.Open())
			{
				using (XmlReader xmlReader2 = XmlReader.Create(stream2, ExcelOpenXmlSheetReader._xmlSettings))
				{
					while (xmlReader2.Read())
					{
						if (XmlReaderHelper.IsStartElement(xmlReader2, "c", ExcelOpenXmlSheetReader._ns))
						{
							string attribute = xmlReader2.GetAttribute("r");
							if (attribute == null)
							{
								withoutCR = true;
								break;
							}
							int num7;
							int num8;
							if (ReferenceHelper.ParseReference(attribute, out num7, out num8))
							{
								num7--;
								num8--;
								num6 = Math.Max(num6, num8);
								maxColumnIndex = Math.Max(maxColumnIndex, num7);
							}
						}
						else if (XmlReaderHelper.IsStartElement(xmlReader2, "dimension", ExcelOpenXmlSheetReader._ns))
						{
							string attribute2 = xmlReader2.GetAttribute("ref");
							if (string.IsNullOrEmpty(attribute2))
							{
								throw new InvalidOperationException("Without sheet dimension data");
							}
							string[] array2 = attribute2.Split(new char[]
							{
								':'
							});
							int num9;
							int num10;
							if (ReferenceHelper.ParseReference((array2.Length == 2) ? array2[1] : array2[0], out num9, out num10))
							{
								maxColumnIndex = num9 - 1;
								num6 = num10 - 1;
								break;
							}
							throw new InvalidOperationException("Invaild sheet dimension start data");
						}
					}
				}
			}
			if (withoutCR)
			{
				using (Stream stream3 = zipArchiveEntry.Open())
				{
					using (XmlReader xmlReader3 = XmlReader.Create(stream3, ExcelOpenXmlSheetReader._xmlSettings))
					{
						if (!XmlReaderHelper.IsStartElement(xmlReader3, "worksheet", ExcelOpenXmlSheetReader._ns))
						{
							yield break;
						}
						if (!XmlReaderHelper.ReadFirstContent(xmlReader3))
						{
							yield break;
						}
						while (!xmlReader3.EOF)
						{
							if (XmlReaderHelper.IsStartElement(xmlReader3, "sheetData", ExcelOpenXmlSheetReader._ns))
							{
								if (XmlReaderHelper.ReadFirstContent(xmlReader3))
								{
									while (!xmlReader3.EOF)
									{
										if (XmlReaderHelper.IsStartElement(xmlReader3, "row", ExcelOpenXmlSheetReader._ns))
										{
											num6++;
											if (XmlReaderHelper.ReadFirstContent(xmlReader3))
											{
												int num11 = -1;
												while (!xmlReader3.EOF)
												{
													if (XmlReaderHelper.IsStartElement(xmlReader3, "c", ExcelOpenXmlSheetReader._ns))
													{
														num11++;
														maxColumnIndex = Math.Max(maxColumnIndex, num11);
													}
													if (!XmlReaderHelper.SkipContent(xmlReader3))
													{
														break;
													}
												}
											}
										}
										else if (!XmlReaderHelper.SkipContent(xmlReader3))
										{
											break;
										}
									}
								}
							}
							else if (!XmlReaderHelper.SkipContent(xmlReader3))
							{
								break;
							}
						}
					}
				}
			}
			using (Stream sheetStream = zipArchiveEntry.Open())
			{
				using (XmlReader reader = XmlReader.Create(sheetStream, ExcelOpenXmlSheetReader._xmlSettings))
				{
					if (!XmlReaderHelper.IsStartElement(reader, "worksheet", ExcelOpenXmlSheetReader._ns))
					{
						yield break;
					}
					if (!XmlReaderHelper.ReadFirstContent(reader))
					{
						yield break;
					}
					while (!reader.EOF)
					{
						if (XmlReaderHelper.IsStartElement(reader, "sheetData", ExcelOpenXmlSheetReader._ns))
						{
							if (XmlReaderHelper.ReadFirstContent(reader))
							{
								Dictionary<int, string> headRows = new Dictionary<int, string>();
								int rowIndex = -1;
								bool isFirstRow = true;
								while (!reader.EOF)
								{
									if (XmlReaderHelper.IsStartElement(reader, "row", ExcelOpenXmlSheetReader._ns))
									{
										int num12 = rowIndex + 1;
										int num13;
										if (int.TryParse(reader.GetAttribute("r"), out num13))
										{
											rowIndex = num13 - 1;
										}
										else
										{
											num = rowIndex;
											rowIndex = num + 1;
										}
										if (!XmlReaderHelper.ReadFirstContent(reader))
										{
											yield return this.GetCell(useHeaderRow, maxColumnIndex, headRows, startColumnIndex);
										}
										else if (rowIndex < startRowIndex)
										{
											XmlReaderHelper.SkipToNextSameLevelDom(reader);
										}
										else
										{
											int num14 = isFirstRow ? startRowIndex : num12;
											if (num14 >= startRowIndex && num14 < rowIndex)
											{
												for (int i = num14; i < rowIndex; i = num + 1)
												{
													yield return this.GetCell(useHeaderRow, maxColumnIndex, headRows, startColumnIndex);
													num = i;
												}
											}
											IDictionary<string, object> cell = this.GetCell(useHeaderRow, maxColumnIndex, headRows, startColumnIndex);
											int num15 = withoutCR ? -1 : 0;
											while (!reader.EOF)
											{
												if (XmlReaderHelper.IsStartElement(reader, "c", ExcelOpenXmlSheetReader._ns))
												{
													string attribute3 = reader.GetAttribute("s");
													string attribute4 = reader.GetAttribute("r");
													string attribute5 = reader.GetAttribute("t");
													object obj = this.ReadCellAndSetColumnIndex(reader, ref num15, withoutCR, startColumnIndex, attribute4, attribute5);
													if (this._config.FillMergedCells)
													{
														if (this._mergeCells.MergesValues.ContainsKey(attribute4))
														{
															this._mergeCells.MergesValues[attribute4] = obj;
														}
														else if (this._mergeCells.MergesMap.ContainsKey(attribute4))
														{
															string key = this._mergeCells.MergesMap[attribute4];
															object obj2 = null;
															if (this._mergeCells.MergesValues.ContainsKey(key))
															{
																obj2 = this._mergeCells.MergesValues[key];
															}
															obj = obj2;
														}
													}
													if (num15 >= startColumnIndex)
													{
														if (!string.IsNullOrEmpty(attribute3))
														{
															int index = -1;
															int num16;
															if (int.TryParse(attribute3, NumberStyles.Any, CultureInfo.InvariantCulture, out num16))
															{
																index = num16;
															}
															if (this._style == null)
															{
																this._style = new ExcelOpenXmlStyles(this._archive);
															}
															obj = this._style.ConvertValueByStyleFormat(index, obj);
															this.SetCellsValueAndHeaders(obj, useHeaderRow, ref headRows, ref isFirstRow, ref cell, num15);
														}
														else
														{
															this.SetCellsValueAndHeaders(obj, useHeaderRow, ref headRows, ref isFirstRow, ref cell, num15);
														}
													}
												}
												else if (!XmlReaderHelper.SkipContent(reader))
												{
													break;
												}
											}
											if (isFirstRow)
											{
												isFirstRow = false;
												if (useHeaderRow)
												{
													continue;
												}
											}
											yield return cell;
										}
									}
									else if (!XmlReaderHelper.SkipContent(reader))
									{
										break;
									}
								}
								headRows = null;
							}
						}
						else if (!XmlReaderHelper.SkipContent(reader))
						{
							break;
						}
					}
				}
				XmlReader reader = null;
			}
			Stream sheetStream = null;
			yield break;
			yield break;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006E86 File Offset: 0x00005086
		private IDictionary<string, object> GetCell(bool useHeaderRow, int maxColumnIndex, Dictionary<int, string> headRows, int startColumnIndex)
		{
			if (!useHeaderRow)
			{
				return CustomPropertyHelper.GetEmptyExpandoObject(maxColumnIndex, startColumnIndex);
			}
			return CustomPropertyHelper.GetEmptyExpandoObject(headRows);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006E9C File Offset: 0x0000509C
		private void SetCellsValueAndHeaders(object cellValue, bool useHeaderRow, ref Dictionary<int, string> headRows, ref bool isFirstRow, ref IDictionary<string, object> cell, int columnIndex)
		{
			if (useHeaderRow)
			{
				if (isFirstRow)
				{
					string value = (cellValue != null) ? cellValue.ToString() : null;
					if (!string.IsNullOrWhiteSpace(value))
					{
						headRows.Add(columnIndex, value);
						return;
					}
				}
				else if (headRows.ContainsKey(columnIndex))
				{
					string key = headRows[columnIndex];
					cell[key] = cellValue;
					return;
				}
			}
			else
			{
				cell[ColumnHelper.GetAlphabetColumnName(columnIndex)] = cellValue;
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006F04 File Offset: 0x00005104
		public IEnumerable<T> Query<T>(string sheetName, string startCell) where T : class, new()
		{
			if (sheetName == null)
			{
				ExcellSheetInfo excellSheetInfo = CustomPropertyHelper.GetExcellSheetInfo(typeof(T), this._config);
				if (excellSheetInfo != null)
				{
					sheetName = excellSheetInfo.ExcelSheetName;
				}
			}
			return ExcelOpenXmlSheetReader.QueryImpl<T>(this.Query(false, sheetName, startCell), startCell, this._config);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006F4A File Offset: 0x0000514A
		public static IEnumerable<T> QueryImpl<T>(IEnumerable<IDictionary<string, object>> values, string startCell, Configuration configuration) where T : class, new()
		{
			Type type = typeof(T);
			List<ExcelColumnInfo> props = null;
			Dictionary<string, int> headersDic = null;
			string[] keys = null;
			bool first = true;
			int rowIndex = 0;
			foreach (IDictionary<string, object> dictionary in values)
			{
				if (first)
				{
					keys = dictionary.Keys.ToArray<string>();
					string[] array;
					if (dictionary == null)
					{
						array = null;
					}
					else
					{
						ICollection<object> values2 = dictionary.Values;
						if (values2 == null)
						{
							array = null;
						}
						else
						{
							IEnumerable<string> enumerable = values2.Select(delegate(object s)
							{
								if (s == null)
								{
									return null;
								}
								return s.ToString();
							});
							array = ((enumerable != null) ? enumerable.ToArray<string>() : null);
						}
					}
					string[] source = array;
					int j;
					headersDic = (from x in source.Select((string o, int i) => new
					{
						o = ((o == null) ? "" : o),
						i = i
					})
					orderby x.i
					group x by x.o into @group
					select new
					{
						Group = @group,
						Count = @group.Count()
					}).SelectMany(groupWithCount => (from b in groupWithCount.Group
					select b).Zip(Enumerable.Range(1, groupWithCount.Count), (j, int i) => new
					{
						key = ((i == 1) ? j.o : string.Format("{0}_____{1}", j.o, i)),
						idx = j.i,
						RowNumber = i
					})).ToDictionary(_ => _.key, _ => _.idx);
					props = CustomPropertyHelper.GetExcelCustomPropertyInfos(type, keys, configuration);
					first = false;
				}
				else
				{
					T t = Activator.CreateInstance<T>();
					foreach (ExcelColumnInfo excelColumnInfo in props)
					{
						if (excelColumnInfo.ExcelColumnAliases != null)
						{
							foreach (string key in excelColumnInfo.ExcelColumnAliases)
							{
								if (headersDic.ContainsKey(key))
								{
									object newValue = null;
									int num = headersDic[key];
									string key2 = keys[num];
									object obj;
									dictionary.TryGetValue(key2, out obj);
									if (obj != null)
									{
										newValue = TypeHelper.TypeMapping<T>(t, excelColumnInfo, newValue, obj, rowIndex, startCell, configuration);
									}
								}
							}
						}
						object newValue2 = null;
						object obj2 = null;
						if (excelColumnInfo.ExcelIndexName != null && keys.Contains(excelColumnInfo.ExcelIndexName))
						{
							dictionary.TryGetValue(excelColumnInfo.ExcelIndexName, out obj2);
						}
						else if (headersDic.ContainsKey(excelColumnInfo.ExcelColumnName))
						{
							int num2 = headersDic[excelColumnInfo.ExcelColumnName];
							string key3 = keys[num2];
							dictionary.TryGetValue(key3, out obj2);
						}
						if (obj2 != null)
						{
							newValue2 = TypeHelper.TypeMapping<T>(t, excelColumnInfo, newValue2, obj2, rowIndex, startCell, configuration);
						}
					}
					int j = rowIndex;
					rowIndex = j + 1;
					yield return t;
				}
			}
			IEnumerator<IDictionary<string, object>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006F68 File Offset: 0x00005168
		private void SetSharedStrings()
		{
			if (this._sharedStrings != null)
			{
				return;
			}
			ZipArchiveEntry entry = this._archive.GetEntry("xl/sharedStrings.xml");
			if (entry == null)
			{
				return;
			}
			using (Stream stream = entry.Open())
			{
				int idx = 0;
				if (this._config.EnableSharedStringCache && entry.Length >= this._config.SharedStringCacheSize)
				{
					this._sharedStrings = new SharedStringsDiskCache();
					using (IEnumerator<string> enumerator = XmlReaderHelper.GetSharedStrings(stream, ExcelOpenXmlSheetReader._ns).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string value = enumerator.Current;
							IDictionary<int, string> sharedStrings = this._sharedStrings;
							int idx3 = idx;
							idx = idx3 + 1;
							sharedStrings[idx3] = value;
						}
						return;
					}
				}
				if (this._sharedStrings == null)
				{
					this._sharedStrings = XmlReaderHelper.GetSharedStrings(stream, ExcelOpenXmlSheetReader._ns).ToDictionary(delegate(string x)
					{
						int idx2 = idx;
						idx = idx2 + 1;
						return idx2;
					}, (string x) => x);
				}
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007094 File Offset: 0x00005294
		private void SetWorkbookRels(ReadOnlyCollection<ZipArchiveEntry> entries)
		{
			if (this._sheetRecords != null)
			{
				return;
			}
			this._sheetRecords = this.GetWorkbookRels(entries);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000070AC File Offset: 0x000052AC
		internal IEnumerable<SheetRecord> ReadWorkbook(ReadOnlyCollection<ZipArchiveEntry> entries)
		{
			using (Stream stream = entries.Single((ZipArchiveEntry w) => w.FullName == "xl/workbook.xml").Open())
			{
				using (XmlReader reader = XmlReader.Create(stream, ExcelOpenXmlSheetReader._xmlSettings))
				{
					if (!XmlReaderHelper.IsStartElement(reader, "workbook", ExcelOpenXmlSheetReader._ns))
					{
						yield break;
					}
					if (!XmlReaderHelper.ReadFirstContent(reader))
					{
						yield break;
					}
					while (!reader.EOF)
					{
						if (XmlReaderHelper.IsStartElement(reader, "sheets", ExcelOpenXmlSheetReader._ns))
						{
							if (XmlReaderHelper.ReadFirstContent(reader))
							{
								while (!reader.EOF)
								{
									if (XmlReaderHelper.IsStartElement(reader, "sheet", ExcelOpenXmlSheetReader._ns))
									{
										yield return new SheetRecord(reader.GetAttribute("name"), reader.GetAttribute("state"), uint.Parse(reader.GetAttribute("sheetId")), XmlReaderHelper.GetAttribute(reader, "id", ExcelOpenXmlSheetReader._relationshiopNs));
										reader.Skip();
									}
									else if (!XmlReaderHelper.SkipContent(reader))
									{
										break;
									}
								}
							}
						}
						else if (!XmlReaderHelper.SkipContent(reader))
						{
							yield break;
						}
					}
				}
				XmlReader reader = null;
			}
			Stream stream = null;
			yield break;
			yield break;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000070BC File Offset: 0x000052BC
		internal List<SheetRecord> GetWorkbookRels(ReadOnlyCollection<ZipArchiveEntry> entries)
		{
			List<SheetRecord> list = this.ReadWorkbook(entries).ToList<SheetRecord>();
			using (Stream stream = entries.Single((ZipArchiveEntry w) => w.FullName == "xl/_rels/workbook.xml.rels").Open())
			{
				using (XmlReader xmlReader = XmlReader.Create(stream, ExcelOpenXmlSheetReader._xmlSettings))
				{
					if (!XmlReaderHelper.IsStartElement(xmlReader, "Relationships", new string[]
					{
						"http://schemas.openxmlformats.org/package/2006/relationships"
					}))
					{
						return null;
					}
					if (!XmlReaderHelper.ReadFirstContent(xmlReader))
					{
						return null;
					}
					while (!xmlReader.EOF)
					{
						if (XmlReaderHelper.IsStartElement(xmlReader, "Relationship", new string[]
						{
							"http://schemas.openxmlformats.org/package/2006/relationships"
						}))
						{
							string attribute = xmlReader.GetAttribute("Id");
							foreach (SheetRecord sheetRecord in list)
							{
								if (sheetRecord.Rid == attribute)
								{
									sheetRecord.Path = xmlReader.GetAttribute("Target");
									break;
								}
							}
							xmlReader.Skip();
						}
						else if (!XmlReaderHelper.SkipContent(xmlReader))
						{
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007218 File Offset: 0x00005418
		private object ReadCellAndSetColumnIndex(XmlReader reader, ref int columnIndex, bool withoutCR, int startColumnIndex, string aR, string aT)
		{
			int xfIndex = -1;
			int num;
			int num2;
			int num3;
			if (withoutCR)
			{
				num = columnIndex + 1;
			}
			else if (ReferenceHelper.ParseReference(aR, out num2, out num3))
			{
				num = num2 - 1;
			}
			else
			{
				num = columnIndex;
			}
			columnIndex = num;
			if (columnIndex < startColumnIndex)
			{
				if (!XmlReaderHelper.ReadFirstContent(reader))
				{
					return null;
				}
				while (!reader.EOF && XmlReaderHelper.SkipContent(reader))
				{
				}
				return null;
			}
			else
			{
				if (!XmlReaderHelper.ReadFirstContent(reader))
				{
					return null;
				}
				object result = null;
				while (!reader.EOF)
				{
					if (XmlReaderHelper.IsStartElement(reader, "v", ExcelOpenXmlSheetReader._ns))
					{
						string text = reader.ReadElementContentAsString();
						if (!string.IsNullOrEmpty(text))
						{
							this.ConvertCellValue(text, aT, xfIndex, out result);
						}
					}
					else if (XmlReaderHelper.IsStartElement(reader, "is", ExcelOpenXmlSheetReader._ns))
					{
						string text2 = StringHelper.ReadStringItem(reader);
						if (!string.IsNullOrEmpty(text2))
						{
							this.ConvertCellValue(text2, aT, xfIndex, out result);
						}
					}
					else if (!XmlReaderHelper.SkipContent(reader))
					{
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000072F4 File Offset: 0x000054F4
		private void ConvertCellValue(string rawValue, string aT, int xfIndex, out object value)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			if (!(aT == "s"))
			{
				if (!(aT == "inlineStr") && !(aT == "str"))
				{
					if (aT == "b")
					{
						value = (rawValue == "1");
						return;
					}
					if (!(aT == "d"))
					{
						if (aT == "e")
						{
							value = rawValue;
							return;
						}
						double num;
						if (double.TryParse(rawValue, NumberStyles.Any, invariantCulture, out num))
						{
							value = num;
							return;
						}
						value = rawValue;
						return;
					}
					else
					{
						DateTime dateTime;
						if (DateTime.TryParseExact(rawValue, "yyyy-MM-dd", invariantCulture, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out dateTime))
						{
							value = dateTime;
							return;
						}
						value = rawValue;
						return;
					}
				}
				else
				{
					string text = XmlEncoder.DecodeString(rawValue);
					if (!this._config.EnableConvertByteArray)
					{
						value = text;
						return;
					}
					if (text != null && text.StartsWith("@@@fileid@@@,", StringComparison.Ordinal))
					{
						string path = text.Substring(13);
						ZipArchiveEntry entry = this._archive.GetEntry(path);
						byte[] array = new byte[entry.Length];
						using (Stream stream = entry.Open())
						{
							using (MemoryStream memoryStream = new MemoryStream(array))
							{
								stream.CopyTo(memoryStream);
							}
						}
						value = array;
						return;
					}
					value = text;
					return;
				}
			}
			else
			{
				int num2;
				if (int.TryParse(rawValue, NumberStyles.Any, invariantCulture, out num2) && num2 >= 0 && num2 < this._sharedStrings.Count)
				{
					value = XmlEncoder.DecodeString(this._sharedStrings[num2]);
					return;
				}
				value = null;
				return;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000074A0 File Offset: 0x000056A0
		public Task<IEnumerable<IDictionary<string, object>>> QueryAsync(bool UseHeaderRow, string sheetName, string startCell, CancellationToken cancellationToken = default(CancellationToken))
		{
			ExcelOpenXmlSheetReader.<QueryAsync>d__22 <QueryAsync>d__;
			<QueryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IEnumerable<IDictionary<string, object>>>.Create();
			<QueryAsync>d__.<>4__this = this;
			<QueryAsync>d__.UseHeaderRow = UseHeaderRow;
			<QueryAsync>d__.sheetName = sheetName;
			<QueryAsync>d__.startCell = startCell;
			<QueryAsync>d__.cancellationToken = cancellationToken;
			<QueryAsync>d__.<>1__state = -1;
			<QueryAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetReader.<QueryAsync>d__22>(ref <QueryAsync>d__);
			return <QueryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007504 File Offset: 0x00005704
		public Task<IEnumerable<T>> QueryAsync<T>(string sheetName, string startCell, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new()
		{
			ExcelOpenXmlSheetReader.<QueryAsync>d__23<T> <QueryAsync>d__;
			<QueryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IEnumerable<T>>.Create();
			<QueryAsync>d__.<>4__this = this;
			<QueryAsync>d__.sheetName = sheetName;
			<QueryAsync>d__.startCell = startCell;
			<QueryAsync>d__.cancellationToken = cancellationToken;
			<QueryAsync>d__.<>1__state = -1;
			<QueryAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetReader.<QueryAsync>d__23<T>>(ref <QueryAsync>d__);
			return <QueryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007560 File Offset: 0x00005760
		~ExcelOpenXmlSheetReader()
		{
			this.Dispose(false);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007590 File Offset: 0x00005790
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000075A0 File Offset: 0x000057A0
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					SharedStringsDiskCache sharedStringsDiskCache = this._sharedStrings as SharedStringsDiskCache;
					if (sharedStringsDiskCache != null)
					{
						sharedStringsDiskCache.Dispose();
					}
				}
				this._disposed = true;
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000075D4 File Offset: 0x000057D4
		public IEnumerable<IDictionary<string, object>> QueryRange(bool useHeaderRow, string sheetName, string startCell, string endCell)
		{
			int startColumnIndex;
			int startRowIndex;
			if (ReferenceHelper.ParseReference(startCell, out startColumnIndex, out startRowIndex) || true)
			{
				int num = startColumnIndex;
				startColumnIndex = num - 1;
				num = startRowIndex;
				startRowIndex = num - 1;
				if (startRowIndex < 0)
				{
					startRowIndex = 0;
				}
				if (startColumnIndex < 0)
				{
					startColumnIndex = 0;
				}
			}
			int endColumnIndex;
			int endRowIndex;
			if (ReferenceHelper.ParseReference(endCell, out endColumnIndex, out endRowIndex) || true)
			{
				int num = endColumnIndex;
				endColumnIndex = num - 1;
				num = endRowIndex;
				endRowIndex = num - 1;
			}
			IEnumerable<ZipArchiveEntry> source = from w in this._archive.entries
			where w.FullName.StartsWith("xl/worksheets/sheet", StringComparison.OrdinalIgnoreCase) || w.FullName.StartsWith("/xl/worksheets/sheet", StringComparison.OrdinalIgnoreCase)
			select w;
			ZipArchiveEntry zipArchiveEntry = null;
			if (sheetName != null)
			{
				this.SetWorkbookRels(this._archive.entries);
				SheetRecord s = this._sheetRecords.SingleOrDefault((SheetRecord _) => _.Name == sheetName);
				if (s == null)
				{
					throw new InvalidOperationException("Please check sheetName/Index is correct");
				}
				zipArchiveEntry = source.Single((ZipArchiveEntry w) => w.FullName == "xl/" + s.Path || w.FullName == "/xl/" + s.Path || w.FullName == s.Path || s.Path == "/" + w.FullName);
			}
			else if (source.Count<ZipArchiveEntry>() > 1)
			{
				this.SetWorkbookRels(this._archive.entries);
				SheetRecord s = this._sheetRecords[0];
				zipArchiveEntry = source.Single((ZipArchiveEntry w) => w.FullName == "xl/" + s.Path || w.FullName == "/xl/" + s.Path);
			}
			else
			{
				zipArchiveEntry = source.Single<ZipArchiveEntry>();
			}
			if (this._config.FillMergedCells)
			{
				this._mergeCells = new MergeCells();
				using (Stream stream = zipArchiveEntry.Open())
				{
					using (XmlReader xmlReader = XmlReader.Create(stream, ExcelOpenXmlSheetReader._xmlSettings))
					{
						if (!XmlReaderHelper.IsStartElement(xmlReader, "worksheet", ExcelOpenXmlSheetReader._ns))
						{
							yield break;
						}
						while (xmlReader.Read())
						{
							if (XmlReaderHelper.IsStartElement(xmlReader, "mergeCells", ExcelOpenXmlSheetReader._ns))
							{
								if (!XmlReaderHelper.ReadFirstContent(xmlReader))
								{
									yield break;
								}
								while (!xmlReader.EOF)
								{
									if (XmlReaderHelper.IsStartElement(xmlReader, "mergeCell", ExcelOpenXmlSheetReader._ns))
									{
										string[] array = xmlReader.GetAttribute("ref").Split(new char[]
										{
											':'
										});
										if (array.Length != 1)
										{
											int num2;
											int num3;
											ReferenceHelper.ParseReference(array[0], out num2, out num3);
											int num4;
											int num5;
											ReferenceHelper.ParseReference(array[1], out num4, out num5);
											this._mergeCells.MergesValues.Add(array[0], null);
											bool flag = true;
											for (int j = num2; j <= num4; j++)
											{
												for (int k = num3; k <= num5; k++)
												{
													if (!flag)
													{
														this._mergeCells.MergesMap.Add(ReferenceHelper.ConvertXyToCell(j, k), array[0]);
													}
													flag = false;
												}
											}
											XmlReaderHelper.SkipContent(xmlReader);
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
			}
			bool withoutCR = false;
			int num6 = -1;
			int maxColumnIndex = -1;
			using (Stream stream2 = zipArchiveEntry.Open())
			{
				using (XmlReader xmlReader2 = XmlReader.Create(stream2, ExcelOpenXmlSheetReader._xmlSettings))
				{
					while (xmlReader2.Read())
					{
						if (XmlReaderHelper.IsStartElement(xmlReader2, "c", ExcelOpenXmlSheetReader._ns))
						{
							string attribute = xmlReader2.GetAttribute("r");
							if (attribute == null)
							{
								withoutCR = true;
								break;
							}
							int num7;
							int num8;
							if (ReferenceHelper.ParseReference(attribute, out num7, out num8))
							{
								num7--;
								num8--;
								num6 = Math.Max(num6, num8);
								maxColumnIndex = Math.Max(maxColumnIndex, num7);
							}
						}
						else if (XmlReaderHelper.IsStartElement(xmlReader2, "dimension", ExcelOpenXmlSheetReader._ns))
						{
							string text = startCell + ":" + endCell;
							if (endCell == "" || startCell == "")
							{
								text = xmlReader2.GetAttribute("ref");
							}
							if (string.IsNullOrEmpty(text))
							{
								throw new InvalidOperationException("Without sheet dimension data");
							}
							string[] array2 = text.Split(new char[]
							{
								':'
							});
							int num9;
							int num10;
							if (!ReferenceHelper.ParseReference((array2.Length == 2) ? array2[1] : array2[0], out num9, out num10) || true)
							{
								maxColumnIndex = num9 - 1;
								num6 = num10 - 1;
								break;
							}
							throw new InvalidOperationException("Invaild sheet dimension start data");
						}
					}
				}
			}
			if (withoutCR)
			{
				using (Stream stream3 = zipArchiveEntry.Open())
				{
					using (XmlReader xmlReader3 = XmlReader.Create(stream3, ExcelOpenXmlSheetReader._xmlSettings))
					{
						if (!XmlReaderHelper.IsStartElement(xmlReader3, "worksheet", ExcelOpenXmlSheetReader._ns))
						{
							yield break;
						}
						if (!XmlReaderHelper.ReadFirstContent(xmlReader3))
						{
							yield break;
						}
						while (!xmlReader3.EOF)
						{
							if (XmlReaderHelper.IsStartElement(xmlReader3, "sheetData", ExcelOpenXmlSheetReader._ns))
							{
								if (XmlReaderHelper.ReadFirstContent(xmlReader3))
								{
									while (!xmlReader3.EOF)
									{
										if (XmlReaderHelper.IsStartElement(xmlReader3, "row", ExcelOpenXmlSheetReader._ns))
										{
											num6++;
											if (XmlReaderHelper.ReadFirstContent(xmlReader3))
											{
												int num11 = -1;
												while (!xmlReader3.EOF)
												{
													if (XmlReaderHelper.IsStartElement(xmlReader3, "c", ExcelOpenXmlSheetReader._ns))
													{
														num11++;
														maxColumnIndex = Math.Max(maxColumnIndex, num11);
													}
													if (!XmlReaderHelper.SkipContent(xmlReader3))
													{
														break;
													}
												}
											}
										}
										else if (!XmlReaderHelper.SkipContent(xmlReader3))
										{
											break;
										}
									}
								}
							}
							else if (!XmlReaderHelper.SkipContent(xmlReader3))
							{
								break;
							}
						}
					}
				}
			}
			using (Stream sheetStream = zipArchiveEntry.Open())
			{
				using (XmlReader reader = XmlReader.Create(sheetStream, ExcelOpenXmlSheetReader._xmlSettings))
				{
					if (!XmlReaderHelper.IsStartElement(reader, "worksheet", ExcelOpenXmlSheetReader._ns))
					{
						yield break;
					}
					if (!XmlReaderHelper.ReadFirstContent(reader))
					{
						yield break;
					}
					while (!reader.EOF)
					{
						if (XmlReaderHelper.IsStartElement(reader, "sheetData", ExcelOpenXmlSheetReader._ns))
						{
							if (XmlReaderHelper.ReadFirstContent(reader))
							{
								Dictionary<int, string> headRows = new Dictionary<int, string>();
								int rowIndex = -1;
								bool isFirstRow = true;
								while (!reader.EOF)
								{
									if (XmlReaderHelper.IsStartElement(reader, "row", ExcelOpenXmlSheetReader._ns))
									{
										int num12 = rowIndex + 1;
										int num13;
										if (int.TryParse(reader.GetAttribute("r"), out num13))
										{
											rowIndex = num13 - 1;
										}
										else
										{
											int num = rowIndex;
											rowIndex = num + 1;
										}
										if (XmlReaderHelper.ReadFirstContent(reader))
										{
											if (rowIndex > endRowIndex && endRowIndex > 0)
											{
												break;
											}
											if (rowIndex < startRowIndex)
											{
												XmlReaderHelper.SkipToNextSameLevelDom(reader);
											}
											else
											{
												if (num12 >= startRowIndex && num12 < rowIndex)
												{
													int num;
													for (int i = num12; i < rowIndex; i = num + 1)
													{
														yield return this.GetCell(useHeaderRow, maxColumnIndex, headRows, startColumnIndex);
														num = i;
													}
												}
												IDictionary<string, object> cell = this.GetCell(useHeaderRow, maxColumnIndex, headRows, startColumnIndex);
												int num14 = withoutCR ? -1 : 0;
												while (!reader.EOF)
												{
													if (XmlReaderHelper.IsStartElement(reader, "c", ExcelOpenXmlSheetReader._ns))
													{
														string attribute2 = reader.GetAttribute("s");
														string attribute3 = reader.GetAttribute("r");
														string attribute4 = reader.GetAttribute("t");
														object obj = this.ReadCellAndSetColumnIndex(reader, ref num14, withoutCR, startColumnIndex, attribute3, attribute4);
														if (this._config.FillMergedCells)
														{
															if (this._mergeCells.MergesValues.ContainsKey(attribute3))
															{
																this._mergeCells.MergesValues[attribute3] = obj;
															}
															else if (this._mergeCells.MergesMap.ContainsKey(attribute3))
															{
																string key = this._mergeCells.MergesMap[attribute3];
																object obj2 = null;
																if (this._mergeCells.MergesValues.ContainsKey(key))
																{
																	obj2 = this._mergeCells.MergesValues[key];
																}
																obj = obj2;
															}
														}
														if (num14 >= startColumnIndex && (num14 <= endColumnIndex || endColumnIndex <= 0))
														{
															if (!string.IsNullOrEmpty(attribute2))
															{
																int index = -1;
																int num15;
																if (int.TryParse(attribute2, NumberStyles.Any, CultureInfo.InvariantCulture, out num15))
																{
																	index = num15;
																}
																if (this._style == null)
																{
																	this._style = new ExcelOpenXmlStyles(this._archive);
																}
																obj = this._style.ConvertValueByStyleFormat(index, obj);
																this.SetCellsValueAndHeaders(obj, useHeaderRow, ref headRows, ref isFirstRow, ref cell, num14);
															}
															else
															{
																this.SetCellsValueAndHeaders(obj, useHeaderRow, ref headRows, ref isFirstRow, ref cell, num14);
															}
														}
													}
													else if (!XmlReaderHelper.SkipContent(reader))
													{
														break;
													}
												}
												if (isFirstRow)
												{
													isFirstRow = false;
													if (useHeaderRow)
													{
														continue;
													}
												}
												yield return cell;
											}
										}
									}
									else if (!XmlReaderHelper.SkipContent(reader))
									{
										break;
									}
								}
								headRows = null;
							}
						}
						else if (!XmlReaderHelper.SkipContent(reader))
						{
							break;
						}
					}
				}
				XmlReader reader = null;
			}
			Stream sheetStream = null;
			yield break;
			yield break;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007601 File Offset: 0x00005801
		public IEnumerable<T> QueryRange<T>(string sheetName, string startCell, string endCell) where T : class, new()
		{
			return ExcelOpenXmlSheetReader.QueryImplRange<T>(this.QueryRange(false, sheetName, startCell, endCell), startCell, endCell, this._config);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000761A File Offset: 0x0000581A
		public static IEnumerable<T> QueryImplRange<T>(IEnumerable<IDictionary<string, object>> values, string startCell, string endCell, Configuration configuration) where T : class, new()
		{
			Type type = typeof(T);
			List<ExcelColumnInfo> props = null;
			Dictionary<string, int> headersDic = null;
			string[] keys = null;
			bool first = true;
			int rowIndex = 0;
			foreach (IDictionary<string, object> dictionary in values)
			{
				if (first)
				{
					keys = dictionary.Keys.ToArray<string>();
					string[] array;
					if (dictionary == null)
					{
						array = null;
					}
					else
					{
						ICollection<object> values2 = dictionary.Values;
						if (values2 == null)
						{
							array = null;
						}
						else
						{
							IEnumerable<string> enumerable = values2.Select(delegate(object s)
							{
								if (s == null)
								{
									return null;
								}
								return s.ToString();
							});
							array = ((enumerable != null) ? enumerable.ToArray<string>() : null);
						}
					}
					string[] source = array;
					int j;
					headersDic = (from x in source.Select((string o, int i) => new
					{
						o = ((o == null) ? string.Empty : o),
						i = i
					})
					orderby x.i
					group x by x.o into @group
					select new
					{
						Group = @group,
						Count = @group.Count()
					}).SelectMany(groupWithCount => (from b in groupWithCount.Group
					select b).Zip(Enumerable.Range(1, groupWithCount.Count), (j, int i) => new
					{
						key = ((i == 1) ? j.o : string.Format("{0}_____{1}", j.o, i)),
						idx = j.i,
						RowNumber = i
					})).ToDictionary(_ => _.key, _ => _.idx);
					props = CustomPropertyHelper.GetExcelCustomPropertyInfos(type, keys, configuration);
					first = false;
				}
				else
				{
					T t = Activator.CreateInstance<T>();
					foreach (ExcelColumnInfo excelColumnInfo in props)
					{
						if (excelColumnInfo.ExcelColumnAliases != null)
						{
							foreach (string key in excelColumnInfo.ExcelColumnAliases)
							{
								if (headersDic.ContainsKey(key))
								{
									object newValue = null;
									object obj = dictionary[keys[headersDic[key]]];
									if (obj != null)
									{
										newValue = TypeHelper.TypeMapping<T>(t, excelColumnInfo, newValue, obj, rowIndex, startCell, configuration);
									}
								}
							}
						}
						object newValue2 = null;
						object obj2 = null;
						if (excelColumnInfo.ExcelIndexName != null && keys.Contains(excelColumnInfo.ExcelIndexName))
						{
							obj2 = dictionary[excelColumnInfo.ExcelIndexName];
						}
						else if (headersDic.ContainsKey(excelColumnInfo.ExcelColumnName))
						{
							obj2 = dictionary[keys[headersDic[excelColumnInfo.ExcelColumnName]]];
						}
						if (obj2 != null)
						{
							newValue2 = TypeHelper.TypeMapping<T>(t, excelColumnInfo, newValue2, obj2, rowIndex, startCell, configuration);
						}
					}
					int j = rowIndex;
					rowIndex = j + 1;
					yield return t;
				}
			}
			IEnumerator<IDictionary<string, object>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007638 File Offset: 0x00005838
		public Task<IEnumerable<IDictionary<string, object>>> QueryAsyncRange(bool UseHeaderRow, string sheetName, string startCell, string endCell, CancellationToken cancellationToken = default(CancellationToken))
		{
			ExcelOpenXmlSheetReader.<QueryAsyncRange>d__30 <QueryAsyncRange>d__;
			<QueryAsyncRange>d__.<>t__builder = AsyncTaskMethodBuilder<IEnumerable<IDictionary<string, object>>>.Create();
			<QueryAsyncRange>d__.<>4__this = this;
			<QueryAsyncRange>d__.UseHeaderRow = UseHeaderRow;
			<QueryAsyncRange>d__.sheetName = sheetName;
			<QueryAsyncRange>d__.startCell = startCell;
			<QueryAsyncRange>d__.cancellationToken = cancellationToken;
			<QueryAsyncRange>d__.<>1__state = -1;
			<QueryAsyncRange>d__.<>t__builder.Start<ExcelOpenXmlSheetReader.<QueryAsyncRange>d__30>(ref <QueryAsyncRange>d__);
			return <QueryAsyncRange>d__.<>t__builder.Task;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000769C File Offset: 0x0000589C
		public Task<IEnumerable<T>> QueryAsyncRange<T>(string sheetName, string startCell, string endCell, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new()
		{
			ExcelOpenXmlSheetReader.<QueryAsyncRange>d__31<T> <QueryAsyncRange>d__;
			<QueryAsyncRange>d__.<>t__builder = AsyncTaskMethodBuilder<IEnumerable<T>>.Create();
			<QueryAsyncRange>d__.<>4__this = this;
			<QueryAsyncRange>d__.sheetName = sheetName;
			<QueryAsyncRange>d__.startCell = startCell;
			<QueryAsyncRange>d__.cancellationToken = cancellationToken;
			<QueryAsyncRange>d__.<>1__state = -1;
			<QueryAsyncRange>d__.<>t__builder.Start<ExcelOpenXmlSheetReader.<QueryAsyncRange>d__31<T>>(ref <QueryAsyncRange>d__);
			return <QueryAsyncRange>d__.<>t__builder.Task;
		}

		// Token: 0x04000093 RID: 147
		private bool _disposed;

		// Token: 0x04000094 RID: 148
		private static readonly string[] _ns = new string[]
		{
			"http://schemas.openxmlformats.org/spreadsheetml/2006/main",
			"http://purl.oclc.org/ooxml/spreadsheetml/main"
		};

		// Token: 0x04000095 RID: 149
		private static readonly string[] _relationshiopNs = new string[]
		{
			"http://schemas.openxmlformats.org/officeDocument/2006/relationships",
			"http://purl.oclc.org/ooxml/officeDocument/relationships"
		};

		// Token: 0x04000096 RID: 150
		private List<SheetRecord> _sheetRecords;

		// Token: 0x04000097 RID: 151
		internal IDictionary<int, string> _sharedStrings;

		// Token: 0x04000098 RID: 152
		private MergeCells _mergeCells;

		// Token: 0x04000099 RID: 153
		private ExcelOpenXmlStyles _style;

		// Token: 0x0400009A RID: 154
		private readonly ExcelOpenXmlZip _archive;

		// Token: 0x0400009B RID: 155
		private readonly OpenXmlConfiguration _config;

		// Token: 0x0400009C RID: 156
		private static readonly XmlReaderSettings _xmlSettings = new XmlReaderSettings
		{
			IgnoreComments = true,
			IgnoreWhitespace = true,
			XmlResolver = null
		};
	}
}
