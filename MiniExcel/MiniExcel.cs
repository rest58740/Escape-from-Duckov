using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MiniExcelLibs.OpenXml;
using MiniExcelLibs.Utils;
using MiniExcelLibs.Zip;

namespace MiniExcelLibs
{
	// Token: 0x02000014 RID: 20
	public static class MiniExcel
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00002784 File Offset: 0x00000984
		public static Task InsertAsync(string path, object value, string sheetName = "Sheet1", ExcelType excelType = ExcelType.UNKNOWN, IConfiguration configuration = null, bool printHeader = true, bool overwriteSheet = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<InsertAsync>d__0 <InsertAsync>d__;
			<InsertAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InsertAsync>d__.path = path;
			<InsertAsync>d__.value = value;
			<InsertAsync>d__.sheetName = sheetName;
			<InsertAsync>d__.excelType = excelType;
			<InsertAsync>d__.configuration = configuration;
			<InsertAsync>d__.printHeader = printHeader;
			<InsertAsync>d__.overwriteSheet = overwriteSheet;
			<InsertAsync>d__.cancellationToken = cancellationToken;
			<InsertAsync>d__.<>1__state = -1;
			<InsertAsync>d__.<>t__builder.Start<MiniExcel.<InsertAsync>d__0>(ref <InsertAsync>d__);
			return <InsertAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002804 File Offset: 0x00000A04
		public static Task InsertAsync(this Stream stream, object value, string sheetName = "Sheet1", ExcelType excelType = ExcelType.XLSX, IConfiguration configuration = null, bool printHeader = true, bool overwriteSheet = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<InsertAsync>d__1 <InsertAsync>d__;
			<InsertAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InsertAsync>d__.stream = stream;
			<InsertAsync>d__.value = value;
			<InsertAsync>d__.sheetName = sheetName;
			<InsertAsync>d__.excelType = excelType;
			<InsertAsync>d__.configuration = configuration;
			<InsertAsync>d__.printHeader = printHeader;
			<InsertAsync>d__.overwriteSheet = overwriteSheet;
			<InsertAsync>d__.<>1__state = -1;
			<InsertAsync>d__.<>t__builder.Start<MiniExcel.<InsertAsync>d__1>(ref <InsertAsync>d__);
			return <InsertAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000287C File Offset: 0x00000A7C
		public static Task SaveAsAsync(string path, object value, bool printHeader = true, string sheetName = "Sheet1", ExcelType excelType = ExcelType.UNKNOWN, IConfiguration configuration = null, bool overwriteFile = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<SaveAsAsync>d__2 <SaveAsAsync>d__;
			<SaveAsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsAsync>d__.path = path;
			<SaveAsAsync>d__.value = value;
			<SaveAsAsync>d__.printHeader = printHeader;
			<SaveAsAsync>d__.sheetName = sheetName;
			<SaveAsAsync>d__.excelType = excelType;
			<SaveAsAsync>d__.configuration = configuration;
			<SaveAsAsync>d__.overwriteFile = overwriteFile;
			<SaveAsAsync>d__.<>1__state = -1;
			<SaveAsAsync>d__.<>t__builder.Start<MiniExcel.<SaveAsAsync>d__2>(ref <SaveAsAsync>d__);
			return <SaveAsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000028F4 File Offset: 0x00000AF4
		public static Task SaveAsAsync(this Stream stream, object value, bool printHeader = true, string sheetName = "Sheet1", ExcelType excelType = ExcelType.XLSX, IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<SaveAsAsync>d__3 <SaveAsAsync>d__;
			<SaveAsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsAsync>d__.stream = stream;
			<SaveAsAsync>d__.value = value;
			<SaveAsAsync>d__.printHeader = printHeader;
			<SaveAsAsync>d__.sheetName = sheetName;
			<SaveAsAsync>d__.excelType = excelType;
			<SaveAsAsync>d__.configuration = configuration;
			<SaveAsAsync>d__.cancellationToken = cancellationToken;
			<SaveAsAsync>d__.<>1__state = -1;
			<SaveAsAsync>d__.<>t__builder.Start<MiniExcel.<SaveAsAsync>d__3>(ref <SaveAsAsync>d__);
			return <SaveAsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000296C File Offset: 0x00000B6C
		public static Task MergeSameCellsAsync(string mergedFilePath, string path, ExcelType excelType = ExcelType.UNKNOWN, IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<MergeSameCellsAsync>d__4 <MergeSameCellsAsync>d__;
			<MergeSameCellsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<MergeSameCellsAsync>d__.mergedFilePath = mergedFilePath;
			<MergeSameCellsAsync>d__.path = path;
			<MergeSameCellsAsync>d__.excelType = excelType;
			<MergeSameCellsAsync>d__.configuration = configuration;
			<MergeSameCellsAsync>d__.cancellationToken = cancellationToken;
			<MergeSameCellsAsync>d__.<>1__state = -1;
			<MergeSameCellsAsync>d__.<>t__builder.Start<MiniExcel.<MergeSameCellsAsync>d__4>(ref <MergeSameCellsAsync>d__);
			return <MergeSameCellsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000029D0 File Offset: 0x00000BD0
		public static Task MergeSameCellsAsync(this Stream stream, string path, ExcelType excelType = ExcelType.XLSX, IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<MergeSameCellsAsync>d__5 <MergeSameCellsAsync>d__;
			<MergeSameCellsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<MergeSameCellsAsync>d__.stream = stream;
			<MergeSameCellsAsync>d__.path = path;
			<MergeSameCellsAsync>d__.excelType = excelType;
			<MergeSameCellsAsync>d__.configuration = configuration;
			<MergeSameCellsAsync>d__.cancellationToken = cancellationToken;
			<MergeSameCellsAsync>d__.<>1__state = -1;
			<MergeSameCellsAsync>d__.<>t__builder.Start<MiniExcel.<MergeSameCellsAsync>d__5>(ref <MergeSameCellsAsync>d__);
			return <MergeSameCellsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002A34 File Offset: 0x00000C34
		public static Task MergeSameCellsAsync(this Stream stream, byte[] fileBytes, ExcelType excelType = ExcelType.XLSX, IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<MergeSameCellsAsync>d__6 <MergeSameCellsAsync>d__;
			<MergeSameCellsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<MergeSameCellsAsync>d__.stream = stream;
			<MergeSameCellsAsync>d__.fileBytes = fileBytes;
			<MergeSameCellsAsync>d__.excelType = excelType;
			<MergeSameCellsAsync>d__.configuration = configuration;
			<MergeSameCellsAsync>d__.cancellationToken = cancellationToken;
			<MergeSameCellsAsync>d__.<>1__state = -1;
			<MergeSameCellsAsync>d__.<>t__builder.Start<MiniExcel.<MergeSameCellsAsync>d__6>(ref <MergeSameCellsAsync>d__);
			return <MergeSameCellsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002A98 File Offset: 0x00000C98
		[return: Dynamic(new bool[]
		{
			false,
			false,
			true
		})]
		public static Task<IEnumerable<dynamic>> QueryAsync(string path, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<QueryAsync>d__7 <QueryAsync>d__;
			<QueryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IEnumerable<object>>.Create();
			<QueryAsync>d__.path = path;
			<QueryAsync>d__.useHeaderRow = useHeaderRow;
			<QueryAsync>d__.sheetName = sheetName;
			<QueryAsync>d__.excelType = excelType;
			<QueryAsync>d__.startCell = startCell;
			<QueryAsync>d__.configuration = configuration;
			<QueryAsync>d__.cancellationToken = cancellationToken;
			<QueryAsync>d__.<>1__state = -1;
			<QueryAsync>d__.<>t__builder.Start<MiniExcel.<QueryAsync>d__7>(ref <QueryAsync>d__);
			return <QueryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002B10 File Offset: 0x00000D10
		public static Task<IEnumerable<T>> QueryAsync<T>(this Stream stream, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new()
		{
			MiniExcel.<QueryAsync>d__8<T> <QueryAsync>d__;
			<QueryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IEnumerable<T>>.Create();
			<QueryAsync>d__.stream = stream;
			<QueryAsync>d__.sheetName = sheetName;
			<QueryAsync>d__.excelType = excelType;
			<QueryAsync>d__.startCell = startCell;
			<QueryAsync>d__.configuration = configuration;
			<QueryAsync>d__.cancellationToken = cancellationToken;
			<QueryAsync>d__.<>1__state = -1;
			<QueryAsync>d__.<>t__builder.Start<MiniExcel.<QueryAsync>d__8<T>>(ref <QueryAsync>d__);
			return <QueryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002B80 File Offset: 0x00000D80
		public static Task<IEnumerable<T>> QueryAsync<T>(string path, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new()
		{
			MiniExcel.<QueryAsync>d__9<T> <QueryAsync>d__;
			<QueryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IEnumerable<T>>.Create();
			<QueryAsync>d__.path = path;
			<QueryAsync>d__.sheetName = sheetName;
			<QueryAsync>d__.excelType = excelType;
			<QueryAsync>d__.startCell = startCell;
			<QueryAsync>d__.configuration = configuration;
			<QueryAsync>d__.cancellationToken = cancellationToken;
			<QueryAsync>d__.<>1__state = -1;
			<QueryAsync>d__.<>t__builder.Start<MiniExcel.<QueryAsync>d__9<T>>(ref <QueryAsync>d__);
			return <QueryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002BF0 File Offset: 0x00000DF0
		[return: Dynamic(new bool[]
		{
			false,
			false,
			true
		})]
		public static Task<IEnumerable<dynamic>> QueryAsync(this Stream stream, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<QueryAsync>d__10 <QueryAsync>d__;
			<QueryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IEnumerable<object>>.Create();
			<QueryAsync>d__.stream = stream;
			<QueryAsync>d__.useHeaderRow = useHeaderRow;
			<QueryAsync>d__.sheetName = sheetName;
			<QueryAsync>d__.excelType = excelType;
			<QueryAsync>d__.startCell = startCell;
			<QueryAsync>d__.configuration = configuration;
			<QueryAsync>d__.cancellationToken = cancellationToken;
			<QueryAsync>d__.<>1__state = -1;
			<QueryAsync>d__.<>t__builder.Start<MiniExcel.<QueryAsync>d__10>(ref <QueryAsync>d__);
			return <QueryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002C68 File Offset: 0x00000E68
		public static Task SaveAsByTemplateAsync(this Stream stream, string templatePath, object value, IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<SaveAsByTemplateAsync>d__11 <SaveAsByTemplateAsync>d__;
			<SaveAsByTemplateAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsByTemplateAsync>d__.stream = stream;
			<SaveAsByTemplateAsync>d__.templatePath = templatePath;
			<SaveAsByTemplateAsync>d__.value = value;
			<SaveAsByTemplateAsync>d__.configuration = configuration;
			<SaveAsByTemplateAsync>d__.cancellationToken = cancellationToken;
			<SaveAsByTemplateAsync>d__.<>1__state = -1;
			<SaveAsByTemplateAsync>d__.<>t__builder.Start<MiniExcel.<SaveAsByTemplateAsync>d__11>(ref <SaveAsByTemplateAsync>d__);
			return <SaveAsByTemplateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002CCC File Offset: 0x00000ECC
		public static Task SaveAsByTemplateAsync(this Stream stream, byte[] templateBytes, object value, IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<SaveAsByTemplateAsync>d__12 <SaveAsByTemplateAsync>d__;
			<SaveAsByTemplateAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsByTemplateAsync>d__.stream = stream;
			<SaveAsByTemplateAsync>d__.templateBytes = templateBytes;
			<SaveAsByTemplateAsync>d__.value = value;
			<SaveAsByTemplateAsync>d__.configuration = configuration;
			<SaveAsByTemplateAsync>d__.cancellationToken = cancellationToken;
			<SaveAsByTemplateAsync>d__.<>1__state = -1;
			<SaveAsByTemplateAsync>d__.<>t__builder.Start<MiniExcel.<SaveAsByTemplateAsync>d__12>(ref <SaveAsByTemplateAsync>d__);
			return <SaveAsByTemplateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002D30 File Offset: 0x00000F30
		public static Task SaveAsByTemplateAsync(string path, string templatePath, object value, IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<SaveAsByTemplateAsync>d__13 <SaveAsByTemplateAsync>d__;
			<SaveAsByTemplateAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsByTemplateAsync>d__.path = path;
			<SaveAsByTemplateAsync>d__.templatePath = templatePath;
			<SaveAsByTemplateAsync>d__.value = value;
			<SaveAsByTemplateAsync>d__.configuration = configuration;
			<SaveAsByTemplateAsync>d__.cancellationToken = cancellationToken;
			<SaveAsByTemplateAsync>d__.<>1__state = -1;
			<SaveAsByTemplateAsync>d__.<>t__builder.Start<MiniExcel.<SaveAsByTemplateAsync>d__13>(ref <SaveAsByTemplateAsync>d__);
			return <SaveAsByTemplateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002D94 File Offset: 0x00000F94
		public static Task SaveAsByTemplateAsync(string path, byte[] templateBytes, object value, IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<SaveAsByTemplateAsync>d__14 <SaveAsByTemplateAsync>d__;
			<SaveAsByTemplateAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsByTemplateAsync>d__.path = path;
			<SaveAsByTemplateAsync>d__.templateBytes = templateBytes;
			<SaveAsByTemplateAsync>d__.value = value;
			<SaveAsByTemplateAsync>d__.configuration = configuration;
			<SaveAsByTemplateAsync>d__.cancellationToken = cancellationToken;
			<SaveAsByTemplateAsync>d__.<>1__state = -1;
			<SaveAsByTemplateAsync>d__.<>t__builder.Start<MiniExcel.<SaveAsByTemplateAsync>d__14>(ref <SaveAsByTemplateAsync>d__);
			return <SaveAsByTemplateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002DF8 File Offset: 0x00000FF8
		[Obsolete("QueryAsDataTable is not recommended, because it'll load all data into memory.")]
		public static Task<DataTable> QueryAsDataTableAsync(string path, bool useHeaderRow = true, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<QueryAsDataTableAsync>d__15 <QueryAsDataTableAsync>d__;
			<QueryAsDataTableAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataTable>.Create();
			<QueryAsDataTableAsync>d__.path = path;
			<QueryAsDataTableAsync>d__.useHeaderRow = useHeaderRow;
			<QueryAsDataTableAsync>d__.sheetName = sheetName;
			<QueryAsDataTableAsync>d__.excelType = excelType;
			<QueryAsDataTableAsync>d__.startCell = startCell;
			<QueryAsDataTableAsync>d__.configuration = configuration;
			<QueryAsDataTableAsync>d__.cancellationToken = cancellationToken;
			<QueryAsDataTableAsync>d__.<>1__state = -1;
			<QueryAsDataTableAsync>d__.<>t__builder.Start<MiniExcel.<QueryAsDataTableAsync>d__15>(ref <QueryAsDataTableAsync>d__);
			return <QueryAsDataTableAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002E70 File Offset: 0x00001070
		[Obsolete("QueryAsDataTable is not recommended, because it'll load all data into memory.")]
		public static Task<DataTable> QueryAsDataTableAsync(this Stream stream, bool useHeaderRow = true, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MiniExcel.<QueryAsDataTableAsync>d__16 <QueryAsDataTableAsync>d__;
			<QueryAsDataTableAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataTable>.Create();
			<QueryAsDataTableAsync>d__.stream = stream;
			<QueryAsDataTableAsync>d__.useHeaderRow = useHeaderRow;
			<QueryAsDataTableAsync>d__.sheetName = sheetName;
			<QueryAsDataTableAsync>d__.excelType = excelType;
			<QueryAsDataTableAsync>d__.startCell = startCell;
			<QueryAsDataTableAsync>d__.configuration = configuration;
			<QueryAsDataTableAsync>d__.cancellationToken = cancellationToken;
			<QueryAsDataTableAsync>d__.<>1__state = -1;
			<QueryAsDataTableAsync>d__.<>t__builder.Start<MiniExcel.<QueryAsDataTableAsync>d__16>(ref <QueryAsDataTableAsync>d__);
			return <QueryAsDataTableAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002EE6 File Offset: 0x000010E6
		public static MiniExcelDataReader GetReader(string path, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null)
		{
			return new MiniExcelDataReader(FileHelper.OpenSharedRead(path), useHeaderRow, sheetName, excelType, startCell, configuration);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002EFA File Offset: 0x000010FA
		public static MiniExcelDataReader GetReader(this Stream stream, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null)
		{
			return new MiniExcelDataReader(stream, useHeaderRow, sheetName, excelType, startCell, configuration);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002F0C File Offset: 0x0000110C
		public static void Insert(string path, object value, string sheetName = "Sheet1", ExcelType excelType = ExcelType.UNKNOWN, IConfiguration configuration = null, bool printHeader = true, bool overwriteSheet = false)
		{
			if (Path.GetExtension(path).ToLowerInvariant() == ".xlsm")
			{
				throw new NotSupportedException("MiniExcel Insert not support xlsm");
			}
			if (!File.Exists(path))
			{
				MiniExcel.SaveAs(path, value, printHeader, sheetName, excelType, null, false);
				return;
			}
			if (excelType == ExcelType.CSV)
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan))
				{
					fileStream.Insert(value, sheetName, ExcelTypeHelper.GetExcelType(path, excelType), configuration, printHeader, overwriteSheet);
					return;
				}
			}
			using (FileStream fileStream2 = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.SequentialScan))
			{
				fileStream2.Insert(value, sheetName, ExcelTypeHelper.GetExcelType(path, excelType), configuration, printHeader, overwriteSheet);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002FDC File Offset: 0x000011DC
		public static void Insert(this Stream stream, object value, string sheetName = "Sheet1", ExcelType excelType = ExcelType.XLSX, IConfiguration configuration = null, bool printHeader = true, bool overwriteSheet = false)
		{
			stream.Seek(0L, SeekOrigin.End);
			if (excelType == ExcelType.CSV)
			{
				object value2;
				if (!(value is IEnumerable) && !(value is IDataReader) && !(value is IDictionary<string, object>) && !(value is IDictionary))
				{
					value2 = from s in Enumerable.Range(0, 1)
					select value;
				}
				else
				{
					value2 = value;
				}
				ExcelWriterFactory.GetProvider(stream, value2, sheetName, excelType, configuration, false).Insert(overwriteSheet);
				return;
			}
			if (configuration == null)
			{
				configuration = new OpenXmlConfiguration
				{
					FastMode = true
				};
			}
			ExcelWriterFactory.GetProvider(stream, value, sheetName, excelType, configuration, printHeader).Insert(overwriteSheet);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000309C File Offset: 0x0000129C
		public static void SaveAs(string path, object value, bool printHeader = true, string sheetName = "Sheet1", ExcelType excelType = ExcelType.UNKNOWN, IConfiguration configuration = null, bool overwriteFile = false)
		{
			if (Path.GetExtension(path).ToLowerInvariant() == ".xlsm")
			{
				throw new NotSupportedException("MiniExcel SaveAs not support xlsm");
			}
			using (FileStream fileStream = overwriteFile ? File.Create(path) : new FileStream(path, FileMode.CreateNew))
			{
				fileStream.SaveAs(value, printHeader, sheetName, ExcelTypeHelper.GetExcelType(path, excelType), configuration);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003110 File Offset: 0x00001310
		public static void SaveAs(this Stream stream, object value, bool printHeader = true, string sheetName = "Sheet1", ExcelType excelType = ExcelType.XLSX, IConfiguration configuration = null)
		{
			ExcelWriterFactory.GetProvider(stream, value, sheetName, excelType, configuration, printHeader).SaveAs();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003124 File Offset: 0x00001324
		public static IEnumerable<T> Query<T>(string path, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null) where T : class, new()
		{
			using (FileStream stream = FileHelper.OpenSharedRead(path))
			{
				foreach (T t in stream.Query(sheetName, ExcelTypeHelper.GetExcelType(path, excelType), startCell, configuration))
				{
					yield return t;
				}
				IEnumerator<T> enumerator = null;
			}
			FileStream stream = null;
			yield break;
			yield break;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003151 File Offset: 0x00001351
		public static IEnumerable<T> Query<T>(this Stream stream, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null) where T : class, new()
		{
			using (IExcelReader excelReader = ExcelReaderFactory.GetProvider(stream, ExcelTypeHelper.GetExcelType(stream, excelType), configuration))
			{
				foreach (T t in excelReader.Query<T>(sheetName, startCell))
				{
					yield return t;
				}
				IEnumerator<T> enumerator = null;
			}
			IExcelReader excelReader = null;
			yield break;
			yield break;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000317E File Offset: 0x0000137E
		[return: Dynamic(new bool[]
		{
			false,
			true
		})]
		public static IEnumerable<dynamic> Query(string path, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null)
		{
			using (FileStream stream = FileHelper.OpenSharedRead(path))
			{
				foreach (object obj in stream.Query(useHeaderRow, sheetName, ExcelTypeHelper.GetExcelType(path, excelType), startCell, configuration))
				{
					yield return obj;
				}
				IEnumerator<object> enumerator = null;
			}
			FileStream stream = null;
			yield break;
			yield break;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000031B3 File Offset: 0x000013B3
		[return: Dynamic(new bool[]
		{
			false,
			true
		})]
		public static IEnumerable<dynamic> Query(this Stream stream, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null)
		{
			using (IExcelReader excelReader = ExcelReaderFactory.GetProvider(stream, ExcelTypeHelper.GetExcelType(stream, excelType), configuration))
			{
				foreach (IDictionary<string, object> source in excelReader.Query(useHeaderRow, sheetName, startCell))
				{
					yield return source.Aggregate(new ExpandoObject(), delegate(IDictionary<string, object> dict, KeyValuePair<string, object> p)
					{
						dict.Add(p);
						return dict;
					});
				}
				IEnumerator<IDictionary<string, object>> enumerator = null;
			}
			IExcelReader excelReader = null;
			yield break;
			yield break;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000031E8 File Offset: 0x000013E8
		[return: Dynamic(new bool[]
		{
			false,
			true
		})]
		public static IEnumerable<dynamic> QueryRange(string path, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "a1", string endCell = "", IConfiguration configuration = null)
		{
			using (FileStream stream = FileHelper.OpenSharedRead(path))
			{
				foreach (object obj in stream.QueryRange(useHeaderRow, sheetName, ExcelTypeHelper.GetExcelType(path, excelType), (startCell == "") ? "a1" : startCell, endCell, configuration))
				{
					yield return obj;
				}
				IEnumerator<object> enumerator = null;
			}
			FileStream stream = null;
			yield break;
			yield break;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003225 File Offset: 0x00001425
		[return: Dynamic(new bool[]
		{
			false,
			true
		})]
		public static IEnumerable<dynamic> QueryRange(this Stream stream, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "a1", string endCell = "", IConfiguration configuration = null)
		{
			using (IExcelReader excelReader = ExcelReaderFactory.GetProvider(stream, ExcelTypeHelper.GetExcelType(stream, excelType), configuration))
			{
				foreach (IDictionary<string, object> source in excelReader.QueryRange(useHeaderRow, sheetName, (startCell == "") ? "a1" : startCell, endCell))
				{
					yield return source.Aggregate(new ExpandoObject(), delegate(IDictionary<string, object> dict, KeyValuePair<string, object> p)
					{
						dict.Add(p);
						return dict;
					});
				}
				IEnumerator<IDictionary<string, object>> enumerator = null;
			}
			IExcelReader excelReader = null;
			yield break;
			yield break;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003264 File Offset: 0x00001464
		public static void SaveAsByTemplate(string path, string templatePath, object value, IConfiguration configuration = null)
		{
			using (FileStream fileStream = File.Create(path))
			{
				fileStream.SaveAsByTemplate(templatePath, value, configuration);
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000032A0 File Offset: 0x000014A0
		public static void SaveAsByTemplate(string path, byte[] templateBytes, object value, IConfiguration configuration = null)
		{
			using (FileStream fileStream = File.Create(path))
			{
				fileStream.SaveAsByTemplate(templateBytes, value, configuration);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000032DC File Offset: 0x000014DC
		public static void SaveAsByTemplate(this Stream stream, string templatePath, object value, IConfiguration configuration = null)
		{
			ExcelTemplateFactory.GetProvider(stream, configuration, ExcelType.XLSX).SaveAsByTemplate(templatePath, value);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000032ED File Offset: 0x000014ED
		public static void SaveAsByTemplate(this Stream stream, byte[] templateBytes, object value, IConfiguration configuration = null)
		{
			ExcelTemplateFactory.GetProvider(stream, configuration, ExcelType.XLSX).SaveAsByTemplate(templateBytes, value);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003300 File Offset: 0x00001500
		public static void MergeSameCells(string mergedFilePath, string path, ExcelType excelType = ExcelType.XLSX, IConfiguration configuration = null)
		{
			using (FileStream fileStream = File.Create(mergedFilePath))
			{
				fileStream.MergeSameCells(path, excelType, configuration);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000333C File Offset: 0x0000153C
		public static void MergeSameCells(this Stream stream, string path, ExcelType excelType = ExcelType.XLSX, IConfiguration configuration = null)
		{
			ExcelTemplateFactory.GetProvider(stream, configuration, excelType).MergeSameCells(path);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000334C File Offset: 0x0000154C
		public static void MergeSameCells(this Stream stream, byte[] filePath, ExcelType excelType = ExcelType.XLSX, IConfiguration configuration = null)
		{
			ExcelTemplateFactory.GetProvider(stream, configuration, excelType).MergeSameCells(filePath);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000335C File Offset: 0x0000155C
		[Obsolete("QueryAsDataTable is not recommended, because it'll load all data into memory.")]
		public static DataTable QueryAsDataTable(string path, bool useHeaderRow = true, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null)
		{
			DataTable result;
			using (FileStream fileStream = FileHelper.OpenSharedRead(path))
			{
				result = fileStream.QueryAsDataTable(useHeaderRow, sheetName, ExcelTypeHelper.GetExcelType(path, excelType), startCell, configuration);
			}
			return result;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000033A4 File Offset: 0x000015A4
		public static DataTable QueryAsDataTable(this Stream stream, bool useHeaderRow = true, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null)
		{
			if (sheetName == null && excelType != ExcelType.CSV)
			{
				sheetName = stream.GetSheetNames(configuration as OpenXmlConfiguration).First<string>();
			}
			DataTable dataTable = new DataTable(sheetName);
			bool flag = true;
			IEnumerable<IDictionary<string, object>> enumerable = ExcelReaderFactory.GetProvider(stream, ExcelTypeHelper.GetExcelType(stream, excelType), configuration).Query(false, sheetName, startCell);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (IDictionary<string, object> dictionary2 in enumerable)
			{
				if (flag)
				{
					foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
					{
						string text;
						if (!useHeaderRow)
						{
							text = keyValuePair.Key;
						}
						else
						{
							object value = keyValuePair.Value;
							text = ((value != null) ? value.ToString() : null);
						}
						string text2 = text;
						if (!string.IsNullOrWhiteSpace(text2))
						{
							DataColumn column = new DataColumn(text2, typeof(object))
							{
								Caption = text2
							};
							dataTable.Columns.Add(column);
							dictionary.Add(keyValuePair.Key, text2);
						}
					}
					dataTable.BeginLoadData();
					flag = false;
					if (useHeaderRow)
					{
						continue;
					}
				}
				DataRow dataRow = dataTable.NewRow();
				foreach (KeyValuePair<string, string> keyValuePair2 in dictionary)
				{
					dataRow[keyValuePair2.Value] = dictionary2[keyValuePair2.Key];
				}
				dataTable.Rows.Add(dataRow);
			}
			dataTable.EndLoadData();
			return dataTable;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000356C File Offset: 0x0000176C
		public static List<string> GetSheetNames(string path, OpenXmlConfiguration config = null)
		{
			List<string> sheetNames;
			using (FileStream fileStream = FileHelper.OpenSharedRead(path))
			{
				sheetNames = fileStream.GetSheetNames(config);
			}
			return sheetNames;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000035A8 File Offset: 0x000017A8
		public static List<string> GetSheetNames(this Stream stream, OpenXmlConfiguration config = null)
		{
			config = (config ?? OpenXmlConfiguration.DefaultConfig);
			ExcelOpenXmlZip excelOpenXmlZip = new ExcelOpenXmlZip(stream, ZipArchiveMode.Read, false, null);
			return (from s in new ExcelOpenXmlSheetReader(stream, config).GetWorkbookRels(excelOpenXmlZip.entries)
			select s.Name).ToList<string>();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003608 File Offset: 0x00001808
		public static List<SheetInfo> GetSheetInformations(string path, OpenXmlConfiguration config = null)
		{
			List<SheetInfo> sheetInformations;
			using (FileStream fileStream = FileHelper.OpenSharedRead(path))
			{
				sheetInformations = fileStream.GetSheetInformations(config);
			}
			return sheetInformations;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003644 File Offset: 0x00001844
		public static List<SheetInfo> GetSheetInformations(this Stream stream, OpenXmlConfiguration config = null)
		{
			config = (config ?? OpenXmlConfiguration.DefaultConfig);
			ExcelOpenXmlZip excelOpenXmlZip = new ExcelOpenXmlZip(stream, ZipArchiveMode.Read, false, null);
			return new ExcelOpenXmlSheetReader(stream, config).GetWorkbookRels(excelOpenXmlZip.entries).Select((SheetRecord s, int i) => s.ToSheetInfo((uint)i)).ToList<SheetInfo>();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000036A4 File Offset: 0x000018A4
		public static ICollection<string> GetColumns(string path, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null)
		{
			ICollection<string> columns;
			using (FileStream fileStream = FileHelper.OpenSharedRead(path))
			{
				columns = fileStream.GetColumns(useHeaderRow, sheetName, excelType, startCell, configuration);
			}
			return columns;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000036E4 File Offset: 0x000018E4
		public static ICollection<string> GetColumns(this Stream stream, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null)
		{
			IDictionary<string, object> dictionary = stream.Query(useHeaderRow, sheetName, excelType, startCell, configuration).FirstOrDefault<object>() as IDictionary<string, object>;
			if (dictionary == null)
			{
				return null;
			}
			return dictionary.Keys;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003708 File Offset: 0x00001908
		public static void ConvertCsvToXlsx(string csv, string xlsx)
		{
			using (FileStream fileStream = FileHelper.OpenSharedRead(csv))
			{
				using (FileStream fileStream2 = new FileStream(xlsx, FileMode.CreateNew))
				{
					MiniExcel.ConvertCsvToXlsx(fileStream, fileStream2);
				}
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003760 File Offset: 0x00001960
		public static void ConvertCsvToXlsx(Stream csv, Stream xlsx)
		{
			IEnumerable<object> value = csv.Query(false, null, ExcelType.CSV, "A1", null);
			xlsx.SaveAs(value, false, "Sheet1", ExcelType.XLSX, null);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000378C File Offset: 0x0000198C
		public static void ConvertXlsxToCsv(string xlsx, string csv)
		{
			using (FileStream fileStream = FileHelper.OpenSharedRead(xlsx))
			{
				using (FileStream fileStream2 = new FileStream(csv, FileMode.CreateNew))
				{
					MiniExcel.ConvertXlsxToCsv(fileStream, fileStream2);
				}
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000037E4 File Offset: 0x000019E4
		public static void ConvertXlsxToCsv(Stream xlsx, Stream csv)
		{
			IEnumerable<object> value = xlsx.Query(false, null, ExcelType.XLSX, "A1", null);
			csv.SaveAs(value, false, "Sheet1", ExcelType.CSV, null);
		}

		// Token: 0x04000015 RID: 21
		public static string LISENCE_CODE;
	}
}
