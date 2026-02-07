using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MiniExcelLibs.Exceptions;
using MiniExcelLibs.OpenXml;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.Csv
{
	// Token: 0x02000066 RID: 102
	internal class CsvReader : IExcelReader, IDisposable
	{
		// Token: 0x06000360 RID: 864 RVA: 0x00012D16 File Offset: 0x00010F16
		public CsvReader(Stream stream, IConfiguration configuration)
		{
			this._stream = stream;
			this._config = ((configuration == null) ? CsvConfiguration.DefaultConfiguration : ((CsvConfiguration)configuration));
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00012D3B File Offset: 0x00010F3B
		public IEnumerable<IDictionary<string, object>> Query(bool useHeaderRow, string sheetName, string startCell)
		{
			if (startCell != "A1")
			{
				throw new NotImplementedException("CSV not Implement startCell");
			}
			if (this._stream.CanSeek)
			{
				this._stream.Position = 0L;
			}
			StreamReader reader = this._config.StreamReaderFunc(this._stream);
			CsvReader.<>c__DisplayClass3_0 CS$<>8__locals1 = new CsvReader.<>c__DisplayClass3_0();
			bool firstRow = true;
			CS$<>8__locals1.headRows = new Dictionary<int, string>();
			int rowIndex = 1;
			string text;
			while ((text = reader.ReadLine()) != null)
			{
				string text2 = text;
				if (this._config.ReadLineBreaksWithinQuotes)
				{
					for (;;)
					{
						if (text2.Count((char c) => c == '"') % 2 == 0)
						{
							break;
						}
						string text3 = reader.ReadLine();
						if (text3 == null)
						{
							break;
						}
						text2 = text2 + this._config.NewLine + text3;
					}
				}
				string[] array = this.Split(text2);
				if (array.Length < CS$<>8__locals1.headRows.Count)
				{
					int num = array.Length;
					Dictionary<string, int> headers = CS$<>8__locals1.headRows.ToDictionary((KeyValuePair<int, string> x) => x.Value, (KeyValuePair<int, string> x) => x.Key);
					IEnumerable<string> source = array;
					var selector;
					if ((selector = CS$<>8__locals1.<>9__3) == null)
					{
						selector = (CS$<>8__locals1.<>9__3 = ((string x, int i) => new
						{
							Key = CS$<>8__locals1.headRows[i],
							Value = x
						}));
					}
					Dictionary<string, object> value = source.Select(selector).ToDictionary(x => x.Key, x => x.Value);
					throw new ExcelColumnNotFoundException(null, CS$<>8__locals1.headRows[num], null, rowIndex, headers, value, string.Format("Csv read error, Column: {0} not found in Row: {1}", num, rowIndex));
				}
				if (useHeaderRow)
				{
					if (firstRow)
					{
						firstRow = false;
						for (int n = 0; n <= array.Length - 1; n++)
						{
							CS$<>8__locals1.headRows.Add(n, array[n]);
						}
					}
					else
					{
						IDictionary<string, object> emptyExpandoObject = CustomPropertyHelper.GetEmptyExpandoObject(CS$<>8__locals1.headRows);
						for (int j = 0; j <= array.Length - 1; j++)
						{
							emptyExpandoObject[CS$<>8__locals1.headRows[j]] = array[j];
						}
						yield return emptyExpandoObject;
					}
				}
				else
				{
					if (firstRow)
					{
						firstRow = false;
						for (int k = 0; k <= array.Length - 1; k++)
						{
							CS$<>8__locals1.headRows.Add(k, string.Format("c{0}", k + 1));
						}
					}
					IDictionary<string, object> emptyExpandoObject2 = CustomPropertyHelper.GetEmptyExpandoObject(array.Length - 1, 0);
					if (this._config.ReadEmptyStringAsNull)
					{
						for (int l = 0; l <= array.Length - 1; l++)
						{
							IDictionary<string, object> dictionary = emptyExpandoObject2;
							string alphabetColumnName = ColumnHelper.GetAlphabetColumnName(l);
							string text4 = array[l];
							dictionary[alphabetColumnName] = ((text4 != null && text4.Length == 0) ? null : array[l]);
						}
					}
					else
					{
						for (int m = 0; m <= array.Length - 1; m++)
						{
							emptyExpandoObject2[ColumnHelper.GetAlphabetColumnName(m)] = array[m];
						}
					}
					yield return emptyExpandoObject2;
				}
				int num2 = rowIndex;
				rowIndex = num2 + 1;
			}
			CS$<>8__locals1 = null;
			yield break;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00012D59 File Offset: 0x00010F59
		public IEnumerable<T> Query<T>(string sheetName, string startCell) where T : class, new()
		{
			return ExcelOpenXmlSheetReader.QueryImpl<T>(this.Query(false, sheetName, startCell), startCell, this._config);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00012D70 File Offset: 0x00010F70
		private string[] Split(string row)
		{
			if (this._config.SplitFn != null)
			{
				return this._config.SplitFn(row);
			}
			return (from s in Regex.Split(row, string.Format("[\t{0}](?=(?:[^\"]|\"[^\"]*\")*$)", this._config.Seperator))
			select Regex.Replace(s.Replace("\"\"", "\""), "^\"|\"$", "")).ToArray<string>();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00012DE5 File Offset: 0x00010FE5
		public Task<IEnumerable<IDictionary<string, object>>> QueryAsync(bool UseHeaderRow, string sheetName, string startCell, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.Run<IEnumerable<IDictionary<string, object>>>(() => this.Query(UseHeaderRow, sheetName, startCell), cancellationToken);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00012E1A File Offset: 0x0001101A
		public Task<IEnumerable<T>> QueryAsync<T>(string sheetName, string startCell, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new()
		{
			return Task.Run<IEnumerable<T>>(() => this.Query<T>(sheetName, startCell), cancellationToken);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00012E47 File Offset: 0x00011047
		public void Dispose()
		{
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00012E49 File Offset: 0x00011049
		public IEnumerable<IDictionary<string, object>> QueryRange(bool useHeaderRow, string sheetName, string startCell, string endCell)
		{
			if (startCell != "A1")
			{
				throw new NotImplementedException("CSV not Implement startCell");
			}
			if (this._stream.CanSeek)
			{
				this._stream.Position = 0L;
			}
			StreamReader reader = this._config.StreamReaderFunc(this._stream);
			string row = string.Empty;
			bool firstRow = true;
			Dictionary<int, string> headRows = new Dictionary<int, string>();
			while ((row = reader.ReadLine()) != null)
			{
				string[] array = this.Split(row);
				if (useHeaderRow)
				{
					if (firstRow)
					{
						firstRow = false;
						for (int i = 0; i <= array.Length - 1; i++)
						{
							headRows.Add(i, array[i]);
						}
					}
					else
					{
						IDictionary<string, object> emptyExpandoObject = CustomPropertyHelper.GetEmptyExpandoObject(headRows);
						for (int j = 0; j <= array.Length - 1; j++)
						{
							emptyExpandoObject[headRows[j]] = array[j];
						}
						yield return emptyExpandoObject;
					}
				}
				else
				{
					IDictionary<string, object> emptyExpandoObject2 = CustomPropertyHelper.GetEmptyExpandoObject(array.Length - 1, 0);
					for (int k = 0; k <= array.Length - 1; k++)
					{
						emptyExpandoObject2[ColumnHelper.GetAlphabetColumnName(k)] = array[k];
					}
					yield return emptyExpandoObject2;
				}
			}
			headRows = null;
			yield break;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00012E67 File Offset: 0x00011067
		public IEnumerable<T> QueryRange<T>(string sheetName, string startCell, string endCel) where T : class, new()
		{
			return ExcelOpenXmlSheetReader.QueryImplRange<T>(this.QueryRange(false, sheetName, startCell, endCel), startCell, endCel, this._config);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00012E80 File Offset: 0x00011080
		public Task<IEnumerable<IDictionary<string, object>>> QueryAsyncRange(bool UseHeaderRow, string sheetName, string startCell, string endCel, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Task.Run<IEnumerable<IDictionary<string, object>>>(() => this.QueryRange(UseHeaderRow, sheetName, startCell, endCel), cancellationToken);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00012EBD File Offset: 0x000110BD
		public Task<IEnumerable<T>> QueryAsyncRange<T>(string sheetName, string startCell, string endCel, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new()
		{
			return Task.Run<IEnumerable<T>>(() => this.Query<T>(sheetName, startCell), cancellationToken);
		}

		// Token: 0x0400015B RID: 347
		private Stream _stream;

		// Token: 0x0400015C RID: 348
		private CsvConfiguration _config;
	}
}
