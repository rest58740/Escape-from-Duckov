using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml.Constants;
using MiniExcelLibs.OpenXml.Models;
using MiniExcelLibs.OpenXml.Styles;
using MiniExcelLibs.Utils;
using MiniExcelLibs.WriteAdapter;
using MiniExcelLibs.Zip;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x02000040 RID: 64
	internal class ExcelOpenXmlSheetWriter : IExcelWriter
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x0000775C File Offset: 0x0000595C
		public Task SaveAsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ExcelOpenXmlSheetWriter.<SaveAsAsync>d__0 <SaveAsAsync>d__;
			<SaveAsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsAsync>d__.<>4__this = this;
			<SaveAsAsync>d__.cancellationToken = cancellationToken;
			<SaveAsAsync>d__.<>1__state = -1;
			<SaveAsAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<SaveAsAsync>d__0>(ref <SaveAsAsync>d__);
			return <SaveAsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000077A8 File Offset: 0x000059A8
		public Task InsertAsync(bool overwriteSheet = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			ExcelOpenXmlSheetWriter.<InsertAsync>d__1 <InsertAsync>d__;
			<InsertAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InsertAsync>d__.<>4__this = this;
			<InsertAsync>d__.overwriteSheet = overwriteSheet;
			<InsertAsync>d__.cancellationToken = cancellationToken;
			<InsertAsync>d__.<>1__state = -1;
			<InsertAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<InsertAsync>d__1>(ref <InsertAsync>d__);
			return <InsertAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000077FC File Offset: 0x000059FC
		internal Task GenerateDefaultOpenXmlAsync(CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<GenerateDefaultOpenXmlAsync>d__2 <GenerateDefaultOpenXmlAsync>d__;
			<GenerateDefaultOpenXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateDefaultOpenXmlAsync>d__.<>4__this = this;
			<GenerateDefaultOpenXmlAsync>d__.cancellationToken = cancellationToken;
			<GenerateDefaultOpenXmlAsync>d__.<>1__state = -1;
			<GenerateDefaultOpenXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<GenerateDefaultOpenXmlAsync>d__2>(ref <GenerateDefaultOpenXmlAsync>d__);
			return <GenerateDefaultOpenXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00007848 File Offset: 0x00005A48
		private Task CreateSheetXmlAsync(object values, string sheetPath, CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<CreateSheetXmlAsync>d__3 <CreateSheetXmlAsync>d__;
			<CreateSheetXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateSheetXmlAsync>d__.<>4__this = this;
			<CreateSheetXmlAsync>d__.values = values;
			<CreateSheetXmlAsync>d__.sheetPath = sheetPath;
			<CreateSheetXmlAsync>d__.cancellationToken = cancellationToken;
			<CreateSheetXmlAsync>d__.<>1__state = -1;
			<CreateSheetXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<CreateSheetXmlAsync>d__3>(ref <CreateSheetXmlAsync>d__);
			return <CreateSheetXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000078A4 File Offset: 0x00005AA4
		private Task WriteEmptySheetAsync(MiniExcelAsyncStreamWriter writer)
		{
			ExcelOpenXmlSheetWriter.<WriteEmptySheetAsync>d__4 <WriteEmptySheetAsync>d__;
			<WriteEmptySheetAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteEmptySheetAsync>d__.writer = writer;
			<WriteEmptySheetAsync>d__.<>1__state = -1;
			<WriteEmptySheetAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<WriteEmptySheetAsync>d__4>(ref <WriteEmptySheetAsync>d__);
			return <WriteEmptySheetAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000078E8 File Offset: 0x00005AE8
		private Task<long> WriteDimensionPlaceholderAsync(MiniExcelAsyncStreamWriter writer)
		{
			ExcelOpenXmlSheetWriter.<WriteDimensionPlaceholderAsync>d__5 <WriteDimensionPlaceholderAsync>d__;
			<WriteDimensionPlaceholderAsync>d__.<>t__builder = AsyncTaskMethodBuilder<long>.Create();
			<WriteDimensionPlaceholderAsync>d__.writer = writer;
			<WriteDimensionPlaceholderAsync>d__.<>1__state = -1;
			<WriteDimensionPlaceholderAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<WriteDimensionPlaceholderAsync>d__5>(ref <WriteDimensionPlaceholderAsync>d__);
			return <WriteDimensionPlaceholderAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000792C File Offset: 0x00005B2C
		private Task WriteDimensionAsync(MiniExcelAsyncStreamWriter writer, int maxRowIndex, int maxColumnIndex, long placeholderPosition)
		{
			ExcelOpenXmlSheetWriter.<WriteDimensionAsync>d__6 <WriteDimensionAsync>d__;
			<WriteDimensionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteDimensionAsync>d__.<>4__this = this;
			<WriteDimensionAsync>d__.writer = writer;
			<WriteDimensionAsync>d__.maxRowIndex = maxRowIndex;
			<WriteDimensionAsync>d__.maxColumnIndex = maxColumnIndex;
			<WriteDimensionAsync>d__.placeholderPosition = placeholderPosition;
			<WriteDimensionAsync>d__.<>1__state = -1;
			<WriteDimensionAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<WriteDimensionAsync>d__6>(ref <WriteDimensionAsync>d__);
			return <WriteDimensionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007990 File Offset: 0x00005B90
		private Task WriteValuesAsync(MiniExcelAsyncStreamWriter writer, object values, CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<WriteValuesAsync>d__7 <WriteValuesAsync>d__;
			<WriteValuesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteValuesAsync>d__.<>4__this = this;
			<WriteValuesAsync>d__.writer = writer;
			<WriteValuesAsync>d__.values = values;
			<WriteValuesAsync>d__.cancellationToken = cancellationToken;
			<WriteValuesAsync>d__.<>1__state = -1;
			<WriteValuesAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<WriteValuesAsync>d__7>(ref <WriteValuesAsync>d__);
			return <WriteValuesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000079EC File Offset: 0x00005BEC
		private Task<long> WriteColumnWidthPlaceholdersAsync(MiniExcelAsyncStreamWriter writer, ICollection<ExcelColumnInfo> props)
		{
			ExcelOpenXmlSheetWriter.<WriteColumnWidthPlaceholdersAsync>d__8 <WriteColumnWidthPlaceholdersAsync>d__;
			<WriteColumnWidthPlaceholdersAsync>d__.<>t__builder = AsyncTaskMethodBuilder<long>.Create();
			<WriteColumnWidthPlaceholdersAsync>d__.writer = writer;
			<WriteColumnWidthPlaceholdersAsync>d__.props = props;
			<WriteColumnWidthPlaceholdersAsync>d__.<>1__state = -1;
			<WriteColumnWidthPlaceholdersAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<WriteColumnWidthPlaceholdersAsync>d__8>(ref <WriteColumnWidthPlaceholdersAsync>d__);
			return <WriteColumnWidthPlaceholdersAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007A38 File Offset: 0x00005C38
		private Task OverWriteColumnWidthPlaceholdersAsync(MiniExcelAsyncStreamWriter writer, long placeholderPosition, IEnumerable<ExcelColumnWidth> columnWidths)
		{
			ExcelOpenXmlSheetWriter.<OverWriteColumnWidthPlaceholdersAsync>d__9 <OverWriteColumnWidthPlaceholdersAsync>d__;
			<OverWriteColumnWidthPlaceholdersAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OverWriteColumnWidthPlaceholdersAsync>d__.<>4__this = this;
			<OverWriteColumnWidthPlaceholdersAsync>d__.writer = writer;
			<OverWriteColumnWidthPlaceholdersAsync>d__.placeholderPosition = placeholderPosition;
			<OverWriteColumnWidthPlaceholdersAsync>d__.columnWidths = columnWidths;
			<OverWriteColumnWidthPlaceholdersAsync>d__.<>1__state = -1;
			<OverWriteColumnWidthPlaceholdersAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<OverWriteColumnWidthPlaceholdersAsync>d__9>(ref <OverWriteColumnWidthPlaceholdersAsync>d__);
			return <OverWriteColumnWidthPlaceholdersAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007A94 File Offset: 0x00005C94
		private Task WriteColumnsWidthsAsync(MiniExcelAsyncStreamWriter writer, IEnumerable<ExcelColumnWidth> columnWidths)
		{
			ExcelOpenXmlSheetWriter.<WriteColumnsWidthsAsync>d__10 <WriteColumnsWidthsAsync>d__;
			<WriteColumnsWidthsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteColumnsWidthsAsync>d__.writer = writer;
			<WriteColumnsWidthsAsync>d__.columnWidths = columnWidths;
			<WriteColumnsWidthsAsync>d__.<>1__state = -1;
			<WriteColumnsWidthsAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<WriteColumnsWidthsAsync>d__10>(ref <WriteColumnsWidthsAsync>d__);
			return <WriteColumnsWidthsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007AE0 File Offset: 0x00005CE0
		private Task PrintHeaderAsync(MiniExcelAsyncStreamWriter writer, List<ExcelColumnInfo> props)
		{
			ExcelOpenXmlSheetWriter.<PrintHeaderAsync>d__11 <PrintHeaderAsync>d__;
			<PrintHeaderAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PrintHeaderAsync>d__.<>4__this = this;
			<PrintHeaderAsync>d__.writer = writer;
			<PrintHeaderAsync>d__.props = props;
			<PrintHeaderAsync>d__.<>1__state = -1;
			<PrintHeaderAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<PrintHeaderAsync>d__11>(ref <PrintHeaderAsync>d__);
			return <PrintHeaderAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007B34 File Offset: 0x00005D34
		private Task WriteCellAsync(MiniExcelAsyncStreamWriter writer, string cellReference, string columnName)
		{
			ExcelOpenXmlSheetWriter.<WriteCellAsync>d__12 <WriteCellAsync>d__;
			<WriteCellAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteCellAsync>d__.<>4__this = this;
			<WriteCellAsync>d__.writer = writer;
			<WriteCellAsync>d__.cellReference = cellReference;
			<WriteCellAsync>d__.columnName = columnName;
			<WriteCellAsync>d__.<>1__state = -1;
			<WriteCellAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<WriteCellAsync>d__12>(ref <WriteCellAsync>d__);
			return <WriteCellAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007B90 File Offset: 0x00005D90
		private Task WriteCellAsync(MiniExcelAsyncStreamWriter writer, int rowIndex, int cellIndex, object value, ExcelColumnInfo p, ExcelWidthCollection widthCollection)
		{
			ExcelOpenXmlSheetWriter.<WriteCellAsync>d__13 <WriteCellAsync>d__;
			<WriteCellAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteCellAsync>d__.<>4__this = this;
			<WriteCellAsync>d__.writer = writer;
			<WriteCellAsync>d__.rowIndex = rowIndex;
			<WriteCellAsync>d__.cellIndex = cellIndex;
			<WriteCellAsync>d__.value = value;
			<WriteCellAsync>d__.p = p;
			<WriteCellAsync>d__.widthCollection = widthCollection;
			<WriteCellAsync>d__.<>1__state = -1;
			<WriteCellAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<WriteCellAsync>d__13>(ref <WriteCellAsync>d__);
			return <WriteCellAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00007C08 File Offset: 0x00005E08
		private Task GenerateEndXmlAsync(CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<GenerateEndXmlAsync>d__14 <GenerateEndXmlAsync>d__;
			<GenerateEndXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateEndXmlAsync>d__.<>4__this = this;
			<GenerateEndXmlAsync>d__.cancellationToken = cancellationToken;
			<GenerateEndXmlAsync>d__.<>1__state = -1;
			<GenerateEndXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<GenerateEndXmlAsync>d__14>(ref <GenerateEndXmlAsync>d__);
			return <GenerateEndXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007C54 File Offset: 0x00005E54
		private Task AddFilesToZipAsync(CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<AddFilesToZipAsync>d__15 <AddFilesToZipAsync>d__;
			<AddFilesToZipAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AddFilesToZipAsync>d__.<>4__this = this;
			<AddFilesToZipAsync>d__.cancellationToken = cancellationToken;
			<AddFilesToZipAsync>d__.<>1__state = -1;
			<AddFilesToZipAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<AddFilesToZipAsync>d__15>(ref <AddFilesToZipAsync>d__);
			return <AddFilesToZipAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00007CA0 File Offset: 0x00005EA0
		private Task GenerateStylesXmlAsync(CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<GenerateStylesXmlAsync>d__16 <GenerateStylesXmlAsync>d__;
			<GenerateStylesXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateStylesXmlAsync>d__.<>4__this = this;
			<GenerateStylesXmlAsync>d__.cancellationToken = cancellationToken;
			<GenerateStylesXmlAsync>d__.<>1__state = -1;
			<GenerateStylesXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<GenerateStylesXmlAsync>d__16>(ref <GenerateStylesXmlAsync>d__);
			return <GenerateStylesXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00007CEC File Offset: 0x00005EEC
		private Task GenerateDrawinRelXmlAsync(CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<GenerateDrawinRelXmlAsync>d__17 <GenerateDrawinRelXmlAsync>d__;
			<GenerateDrawinRelXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateDrawinRelXmlAsync>d__.<>4__this = this;
			<GenerateDrawinRelXmlAsync>d__.cancellationToken = cancellationToken;
			<GenerateDrawinRelXmlAsync>d__.<>1__state = -1;
			<GenerateDrawinRelXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<GenerateDrawinRelXmlAsync>d__17>(ref <GenerateDrawinRelXmlAsync>d__);
			return <GenerateDrawinRelXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00007D38 File Offset: 0x00005F38
		private Task GenerateDrawinRelXmlAsync(int sheetIndex, CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<GenerateDrawinRelXmlAsync>d__18 <GenerateDrawinRelXmlAsync>d__;
			<GenerateDrawinRelXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateDrawinRelXmlAsync>d__.<>4__this = this;
			<GenerateDrawinRelXmlAsync>d__.sheetIndex = sheetIndex;
			<GenerateDrawinRelXmlAsync>d__.cancellationToken = cancellationToken;
			<GenerateDrawinRelXmlAsync>d__.<>1__state = -1;
			<GenerateDrawinRelXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<GenerateDrawinRelXmlAsync>d__18>(ref <GenerateDrawinRelXmlAsync>d__);
			return <GenerateDrawinRelXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00007D8C File Offset: 0x00005F8C
		private Task GenerateDrawingXmlAsync(CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<GenerateDrawingXmlAsync>d__19 <GenerateDrawingXmlAsync>d__;
			<GenerateDrawingXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateDrawingXmlAsync>d__.<>4__this = this;
			<GenerateDrawingXmlAsync>d__.cancellationToken = cancellationToken;
			<GenerateDrawingXmlAsync>d__.<>1__state = -1;
			<GenerateDrawingXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<GenerateDrawingXmlAsync>d__19>(ref <GenerateDrawingXmlAsync>d__);
			return <GenerateDrawingXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007DD8 File Offset: 0x00005FD8
		private Task GenerateDrawingXmlAsync(int sheetIndex, CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<GenerateDrawingXmlAsync>d__20 <GenerateDrawingXmlAsync>d__;
			<GenerateDrawingXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateDrawingXmlAsync>d__.<>4__this = this;
			<GenerateDrawingXmlAsync>d__.sheetIndex = sheetIndex;
			<GenerateDrawingXmlAsync>d__.cancellationToken = cancellationToken;
			<GenerateDrawingXmlAsync>d__.<>1__state = -1;
			<GenerateDrawingXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<GenerateDrawingXmlAsync>d__20>(ref <GenerateDrawingXmlAsync>d__);
			return <GenerateDrawingXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00007E2C File Offset: 0x0000602C
		private Task GenerateWorkbookXmlAsync(CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<GenerateWorkbookXmlAsync>d__21 <GenerateWorkbookXmlAsync>d__;
			<GenerateWorkbookXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateWorkbookXmlAsync>d__.<>4__this = this;
			<GenerateWorkbookXmlAsync>d__.cancellationToken = cancellationToken;
			<GenerateWorkbookXmlAsync>d__.<>1__state = -1;
			<GenerateWorkbookXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<GenerateWorkbookXmlAsync>d__21>(ref <GenerateWorkbookXmlAsync>d__);
			return <GenerateWorkbookXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00007E78 File Offset: 0x00006078
		private Task GenerateContentTypesXmlAsync(CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<GenerateContentTypesXmlAsync>d__22 <GenerateContentTypesXmlAsync>d__;
			<GenerateContentTypesXmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GenerateContentTypesXmlAsync>d__.<>4__this = this;
			<GenerateContentTypesXmlAsync>d__.cancellationToken = cancellationToken;
			<GenerateContentTypesXmlAsync>d__.<>1__state = -1;
			<GenerateContentTypesXmlAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<GenerateContentTypesXmlAsync>d__22>(ref <GenerateContentTypesXmlAsync>d__);
			return <GenerateContentTypesXmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00007EC4 File Offset: 0x000060C4
		private Task CreateZipEntryAsync(string path, string contentType, string content, CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<CreateZipEntryAsync>d__23 <CreateZipEntryAsync>d__;
			<CreateZipEntryAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateZipEntryAsync>d__.<>4__this = this;
			<CreateZipEntryAsync>d__.path = path;
			<CreateZipEntryAsync>d__.contentType = contentType;
			<CreateZipEntryAsync>d__.content = content;
			<CreateZipEntryAsync>d__.cancellationToken = cancellationToken;
			<CreateZipEntryAsync>d__.<>1__state = -1;
			<CreateZipEntryAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<CreateZipEntryAsync>d__23>(ref <CreateZipEntryAsync>d__);
			return <CreateZipEntryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00007F28 File Offset: 0x00006128
		private Task CreateZipEntryAsync(string path, byte[] content, CancellationToken cancellationToken)
		{
			ExcelOpenXmlSheetWriter.<CreateZipEntryAsync>d__24 <CreateZipEntryAsync>d__;
			<CreateZipEntryAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateZipEntryAsync>d__.<>4__this = this;
			<CreateZipEntryAsync>d__.path = path;
			<CreateZipEntryAsync>d__.content = content;
			<CreateZipEntryAsync>d__.cancellationToken = cancellationToken;
			<CreateZipEntryAsync>d__.<>1__state = -1;
			<CreateZipEntryAsync>d__.<>t__builder.Start<ExcelOpenXmlSheetWriter.<CreateZipEntryAsync>d__24>(ref <CreateZipEntryAsync>d__);
			return <CreateZipEntryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007F84 File Offset: 0x00006184
		public ExcelOpenXmlSheetWriter(Stream stream, object value, string sheetName, IConfiguration configuration, bool printHeader)
		{
			this._stream = stream;
			this._configuration = ((configuration as OpenXmlConfiguration) ?? OpenXmlConfiguration.DefaultConfig);
			if (this._configuration.EnableAutoWidth && !this._configuration.FastMode)
			{
				throw new InvalidOperationException("Auto width requires fast mode to be enabled");
			}
			if (this._configuration.FastMode)
			{
				this._archive = new MiniExcelZipArchive(this._stream, ZipArchiveMode.Update, true, ExcelOpenXmlSheetWriter._utf8WithBom);
			}
			else
			{
				this._archive = new MiniExcelZipArchive(this._stream, ZipArchiveMode.Create, true, ExcelOpenXmlSheetWriter._utf8WithBom);
			}
			this._printHeader = printHeader;
			this._value = value;
			this._defaultSheetName = sheetName;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000804F File Offset: 0x0000624F
		public ExcelOpenXmlSheetWriter()
		{
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008078 File Offset: 0x00006278
		public void SaveAs()
		{
			this.GenerateDefaultOpenXml();
			foreach (Tuple<SheetDto, object> tuple in this.GetSheets())
			{
				this._sheets.Add(tuple.Item1);
				this.currentSheetIndex = tuple.Item1.SheetIdx;
				this.CreateSheetXml(tuple.Item2, tuple.Item1.Path);
			}
			this.GenerateEndXml();
			this._archive.Dispose();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008110 File Offset: 0x00006310
		public void Insert(bool overwriteSheet = false)
		{
			if (!this._configuration.FastMode)
			{
				throw new InvalidOperationException("Insert requires fast mode to be enabled");
			}
			SheetRecord[] source = new ExcelOpenXmlSheetReader(this._stream, this._configuration).GetWorkbookRels(this._archive.Entries).ToArray();
			foreach (SheetRecord sheetRecord in from o in source
			orderby o.Id
			select o)
			{
				this._sheets.Add(new SheetDto
				{
					Name = sheetRecord.Name,
					SheetIdx = (int)sheetRecord.Id,
					State = sheetRecord.State
				});
			}
			SheetDto existSheetDto = this._sheets.SingleOrDefault((SheetDto s) => s.Name == this._defaultSheetName);
			if (existSheetDto != null && !overwriteSheet)
			{
				throw new Exception("Sheet “" + this._defaultSheetName + "” already exist");
			}
			this.GenerateStylesXml();
			if (existSheetDto == null)
			{
				this.currentSheetIndex = (int)(source.Max((SheetRecord m) => m.Id) + 1U);
				SheetDto sheetDto = this.GetSheetInfos(this._defaultSheetName).ToDto(this.currentSheetIndex);
				this._sheets.Add(sheetDto);
				this.CreateSheetXml(this._value, sheetDto.Path);
			}
			else
			{
				this.currentSheetIndex = existSheetDto.SheetIdx;
				this._archive.Entries.Single((ZipArchiveEntry s) => s.FullName == existSheetDto.Path).Delete();
				ZipArchiveEntry zipArchiveEntry = this._archive.Entries.SingleOrDefault((ZipArchiveEntry s) => s.FullName == ExcelFileNames.DrawingRels(this.currentSheetIndex));
				if (zipArchiveEntry != null)
				{
					zipArchiveEntry.Delete();
				}
				ZipArchiveEntry zipArchiveEntry2 = this._archive.Entries.SingleOrDefault((ZipArchiveEntry s) => s.FullName == ExcelFileNames.Drawing(this.currentSheetIndex));
				if (zipArchiveEntry2 != null)
				{
					zipArchiveEntry2.Delete();
				}
				this.CreateSheetXml(this._value, existSheetDto.Path);
			}
			this.AddFilesToZip();
			this.GenerateDrawinRelXml(this.currentSheetIndex);
			this.GenerateDrawingXml(this.currentSheetIndex);
			StringBuilder stringBuilder;
			StringBuilder stringBuilder2;
			Dictionary<int, string> dictionary;
			this.GenerateWorkBookXmls(out stringBuilder, out stringBuilder2, out dictionary);
			foreach (KeyValuePair<int, string> keyValuePair in dictionary)
			{
				string sheetRelsXmlPath = ExcelFileNames.SheetRels(keyValuePair.Key);
				ZipArchiveEntry zipArchiveEntry3 = this._archive.Entries.SingleOrDefault((ZipArchiveEntry s) => s.FullName == sheetRelsXmlPath);
				if (zipArchiveEntry3 != null)
				{
					zipArchiveEntry3.Delete();
				}
				this.CreateZipEntry(sheetRelsXmlPath, null, ExcelXml.DefaultSheetRelXml.Replace("{{format}}", keyValuePair.Value));
			}
			ZipArchiveEntry zipArchiveEntry4 = this._archive.Entries.SingleOrDefault((ZipArchiveEntry s) => s.FullName == "xl/workbook.xml");
			if (zipArchiveEntry4 != null)
			{
				zipArchiveEntry4.Delete();
			}
			this.CreateZipEntry("xl/workbook.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", ExcelXml.DefaultWorkbookXml.Replace("{{sheets}}", stringBuilder.ToString()));
			ZipArchiveEntry zipArchiveEntry5 = this._archive.Entries.SingleOrDefault((ZipArchiveEntry s) => s.FullName == "xl/_rels/workbook.xml.rels");
			if (zipArchiveEntry5 != null)
			{
				zipArchiveEntry5.Delete();
			}
			this.CreateZipEntry("xl/_rels/workbook.xml.rels", null, ExcelXml.DefaultWorkbookXmlRels.Replace("{{sheets}}", stringBuilder2.ToString()));
			this._archive.Dispose();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000084E0 File Offset: 0x000066E0
		internal void GenerateDefaultOpenXml()
		{
			this.CreateZipEntry("_rels/.rels", "application/vnd.openxmlformats-package.relationships+xml", ExcelXml.DefaultRels);
			this.CreateZipEntry("xl/sharedStrings.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml", ExcelXml.DefaultSharedString);
			this.GenerateStylesXml();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008514 File Offset: 0x00006714
		private void CreateSheetXml(object values, string sheetPath)
		{
			ZipArchiveEntry zipArchiveEntry = this._archive.CreateEntry(sheetPath, CompressionLevel.Fastest);
			using (Stream stream = zipArchiveEntry.Open())
			{
				using (MiniExcelStreamWriter miniExcelStreamWriter = new MiniExcelStreamWriter(stream, ExcelOpenXmlSheetWriter._utf8WithBom, this._configuration.BufferSize))
				{
					if (values == null)
					{
						this.WriteEmptySheet(miniExcelStreamWriter);
					}
					else
					{
						this.WriteValues(miniExcelStreamWriter, values);
					}
				}
			}
			this._zipDictionary.Add(sheetPath, new ZipPackageInfo(zipArchiveEntry, "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"));
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000085AC File Offset: 0x000067AC
		private void WriteEmptySheet(MiniExcelStreamWriter writer)
		{
			writer.Write(ExcelXml.EmptySheetXml);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000085B9 File Offset: 0x000067B9
		private long WriteDimensionPlaceholder(MiniExcelStreamWriter writer)
		{
			long result = writer.WriteAndFlush("<x:dimension ref=\"");
			writer.Write("                              />");
			return result;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000085D4 File Offset: 0x000067D4
		private void WriteDimension(MiniExcelStreamWriter writer, int maxRowIndex, int maxColumnIndex, long placeholderPosition)
		{
			long position = writer.Flush();
			writer.SetPosition(placeholderPosition);
			writer.WriteAndFlush(this.GetDimensionRef(maxRowIndex, maxColumnIndex) + "\"");
			writer.SetPosition(position);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008610 File Offset: 0x00006810
		private void WriteValues(MiniExcelStreamWriter writer, object values)
		{
			IMiniExcelWriteAdapter writeAdapter = MiniExcelWriteAdapterFactory.GetWriteAdapter(values, this._configuration);
			int num;
			bool flag = writeAdapter.TryGetKnownCount(out num);
			List<ExcelColumnInfo> columns = writeAdapter.GetColumns();
			if (columns == null)
			{
				this.WriteEmptySheet(writer);
				return;
			}
			int count = columns.Count;
			writer.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?><x:worksheet xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" xmlns:x=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" >");
			long num2 = 0L;
			int maxRowIndex;
			if (this._configuration.FastMode && !flag)
			{
				num2 = this.WriteDimensionPlaceholder(writer);
			}
			else if (flag)
			{
				maxRowIndex = num + ((this._printHeader > false) ? 1 : 0);
				writer.Write(WorksheetXml.Dimension(this.GetDimensionRef(maxRowIndex, columns.Count)));
			}
			writer.Write(this.GetSheetViews());
			ExcelWidthCollection excelWidthCollection = null;
			long placeholderPosition = 0L;
			if (this._configuration.EnableAutoWidth)
			{
				placeholderPosition = this.WriteColumnWidthPlaceholders(writer, columns);
				excelWidthCollection = new ExcelWidthCollection(this._configuration.MinWidth, this._configuration.MaxWidth, columns);
			}
			else
			{
				this.WriteColumnsWidths(writer, ExcelColumnWidth.FromProps(columns, null));
			}
			writer.Write("<x:sheetData>");
			int num3 = 0;
			if (this._printHeader)
			{
				this.PrintHeader(writer, columns);
				num3++;
			}
			foreach (IEnumerable<CellWriteInfo> enumerable in writeAdapter.GetRows(columns, default(CancellationToken)))
			{
				writer.Write(WorksheetXml.StartRow(++num3));
				foreach (CellWriteInfo cellWriteInfo in enumerable)
				{
					this.WriteCell(writer, num3, cellWriteInfo.CellIndex, cellWriteInfo.Value, cellWriteInfo.Prop, excelWidthCollection);
				}
				writer.Write("</x:row>");
			}
			maxRowIndex = num3;
			writer.Write("</x:sheetData>");
			if (this._configuration.AutoFilter)
			{
				writer.Write(WorksheetXml.Autofilter(this.GetDimensionRef(maxRowIndex, count)));
			}
			writer.Write(WorksheetXml.Drawing(this.currentSheetIndex));
			writer.Write("</x:worksheet>");
			if (this._configuration.FastMode && num2 != 0L)
			{
				this.WriteDimension(writer, maxRowIndex, count, num2);
			}
			if (this._configuration.EnableAutoWidth)
			{
				this.OverWriteColumnWidthPlaceholders(writer, placeholderPosition, excelWidthCollection.Columns);
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008868 File Offset: 0x00006A68
		private long WriteColumnWidthPlaceholders(MiniExcelStreamWriter writer, ICollection<ExcelColumnInfo> props)
		{
			long result = writer.Flush();
			writer.WriteWhitespace(WorksheetXml.GetColumnPlaceholderLength(props.Count));
			return result;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008884 File Offset: 0x00006A84
		private void OverWriteColumnWidthPlaceholders(MiniExcelStreamWriter writer, long placeholderPosition, IEnumerable<ExcelColumnWidth> columnWidths)
		{
			long position = writer.Flush();
			writer.SetPosition(placeholderPosition);
			this.WriteColumnsWidths(writer, columnWidths);
			writer.Flush();
			writer.SetPosition(position);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000088B8 File Offset: 0x00006AB8
		private void WriteColumnsWidths(MiniExcelStreamWriter writer, IEnumerable<ExcelColumnWidth> columnWidths)
		{
			bool flag = false;
			foreach (ExcelColumnWidth excelColumnWidth in columnWidths)
			{
				if (!flag)
				{
					writer.Write("<x:cols>");
					flag = true;
				}
				writer.Write(WorksheetXml.Column(excelColumnWidth.Index, excelColumnWidth.Width));
			}
			if (!flag)
			{
				return;
			}
			writer.Write("</x:cols>");
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00008934 File Offset: 0x00006B34
		private void PrintHeader(MiniExcelStreamWriter writer, List<ExcelColumnInfo> props)
		{
			int num = 1;
			int num2 = 1;
			writer.Write(WorksheetXml.StartRow(num2));
			foreach (ExcelColumnInfo excelColumnInfo in props)
			{
				if (excelColumnInfo == null)
				{
					num++;
				}
				else
				{
					string cellReference = ExcelOpenXmlUtils.ConvertXyToCell(num, num2);
					this.WriteCell(writer, cellReference, excelColumnInfo.ExcelColumnName);
					num++;
				}
			}
			writer.Write("</x:row>");
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000089BC File Offset: 0x00006BBC
		private void WriteCell(MiniExcelStreamWriter writer, int rowIndex, int cellIndex, object value, ExcelColumnInfo columnInfo, ExcelWidthCollection widthCollection)
		{
			string cellReference = ExcelOpenXmlUtils.ConvertXyToCell(cellIndex, rowIndex);
			bool flag = value == null || value is DBNull;
			if (this._configuration.EnableWriteNullValueCell && flag)
			{
				writer.Write(WorksheetXml.EmptyCell(cellReference, this.GetCellXfId("2")));
				return;
			}
			Tuple<string, string, string> cellValue = this.GetCellValue(rowIndex, cellIndex, value, columnInfo, flag);
			string item = cellValue.Item1;
			string item2 = cellValue.Item2;
			string text = cellValue.Item3;
			if (columnInfo != null && columnInfo.CustomFormatter != null)
			{
				try
				{
					text = columnInfo.CustomFormatter(text);
				}
				catch
				{
				}
			}
			ColumnType columnType = (columnInfo != null) ? columnInfo.ExcelColumnType : ColumnType.Value;
			bool preserveSpace = text != null && (text.StartsWith(" ", StringComparison.Ordinal) || text.EndsWith(" ", StringComparison.Ordinal));
			writer.Write(WorksheetXml.Cell(cellReference, item2, this.GetCellXfId(item), text, preserveSpace, columnType));
			if (widthCollection != null)
			{
				widthCollection.AdjustWidth(cellIndex, text);
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008AC0 File Offset: 0x00006CC0
		private void WriteCell(MiniExcelStreamWriter writer, string cellReference, string columnName)
		{
			writer.Write(WorksheetXml.Cell(cellReference, "str", this.GetCellXfId("1"), ExcelOpenXmlUtils.EncodeXML(columnName), false, ColumnType.Value));
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008AE6 File Offset: 0x00006CE6
		private void GenerateEndXml()
		{
			this.AddFilesToZip();
			this.GenerateDrawinRelXml();
			this.GenerateDrawingXml();
			this.GenerateWorkbookXml();
			this.GenerateContentTypesXml();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008B08 File Offset: 0x00006D08
		private void AddFilesToZip()
		{
			foreach (FileDto fileDto in this._files)
			{
				this.CreateZipEntry(fileDto.Path, fileDto.Byte);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008B68 File Offset: 0x00006D68
		private void GenerateStylesXml()
		{
			using (SheetStyleBuildContext sheetStyleBuildContext = new SheetStyleBuildContext(this._zipDictionary, this._archive, ExcelOpenXmlSheetWriter._utf8WithBom, this._configuration.DynamicColumns))
			{
				ISheetStyleBuilder sheetStyleBuilder = null;
				TableStyles tableStyles = this._configuration.TableStyles;
				if (tableStyles != TableStyles.None)
				{
					if (tableStyles == TableStyles.Default)
					{
						sheetStyleBuilder = new DefaultSheetStyleBuilder(sheetStyleBuildContext);
					}
				}
				else
				{
					sheetStyleBuilder = new MinimalSheetStyleBuilder(sheetStyleBuildContext);
				}
				SheetStyleBuildResult sheetStyleBuildResult = sheetStyleBuilder.Build();
				this.cellXfIdMap = sheetStyleBuildResult.CellXfIdMap;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008BF0 File Offset: 0x00006DF0
		private void GenerateDrawinRelXml()
		{
			for (int i = 0; i < this._sheets.Count; i++)
			{
				this.GenerateDrawinRelXml(i);
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00008C1C File Offset: 0x00006E1C
		private void GenerateDrawinRelXml(int sheetIndex)
		{
			string drawingRelationshipXml = this.GetDrawingRelationshipXml(sheetIndex);
			this.CreateZipEntry(ExcelFileNames.DrawingRels(sheetIndex), null, ExcelXml.DefaultDrawingXmlRels.Replace("{{format}}", drawingRelationshipXml));
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008C50 File Offset: 0x00006E50
		private void GenerateDrawingXml()
		{
			for (int i = 0; i < this._sheets.Count; i++)
			{
				this.GenerateDrawingXml(i);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00008C7C File Offset: 0x00006E7C
		private void GenerateDrawingXml(int sheetIndex)
		{
			string drawingXml = this.GetDrawingXml(sheetIndex);
			this.CreateZipEntry(ExcelFileNames.Drawing(sheetIndex), "application/vnd.openxmlformats-officedocument.drawing+xml", ExcelXml.DefaultDrawing.Replace("{{format}}", drawingXml));
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00008CB4 File Offset: 0x00006EB4
		private void GenerateWorkbookXml()
		{
			StringBuilder stringBuilder;
			StringBuilder stringBuilder2;
			Dictionary<int, string> dictionary;
			this.GenerateWorkBookXmls(out stringBuilder, out stringBuilder2, out dictionary);
			foreach (KeyValuePair<int, string> keyValuePair in dictionary)
			{
				this.CreateZipEntry(ExcelFileNames.SheetRels(keyValuePair.Key), null, ExcelXml.DefaultSheetRelXml.Replace("{{format}}", keyValuePair.Value));
			}
			this.CreateZipEntry("xl/workbook.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", ExcelXml.DefaultWorkbookXml.Replace("{{sheets}}", stringBuilder.ToString()));
			this.CreateZipEntry("xl/_rels/workbook.xml.rels", null, ExcelXml.DefaultWorkbookXmlRels.Replace("{{sheets}}", stringBuilder2.ToString()));
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00008D78 File Offset: 0x00006F78
		private void GenerateContentTypesXml()
		{
			string contentTypesXml = this.GetContentTypesXml();
			this.CreateZipEntry("[Content_Types].xml", null, contentTypesXml);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008D9C File Offset: 0x00006F9C
		private void CreateZipEntry(string path, string contentType, string content)
		{
			ZipArchiveEntry zipArchiveEntry = this._archive.CreateEntry(path, CompressionLevel.Fastest);
			using (Stream stream = zipArchiveEntry.Open())
			{
				using (MiniExcelStreamWriter miniExcelStreamWriter = new MiniExcelStreamWriter(stream, ExcelOpenXmlSheetWriter._utf8WithBom, this._configuration.BufferSize))
				{
					miniExcelStreamWriter.Write(content);
				}
			}
			if (!string.IsNullOrEmpty(contentType))
			{
				this._zipDictionary.Add(path, new ZipPackageInfo(zipArchiveEntry, contentType));
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00008E2C File Offset: 0x0000702C
		private void CreateZipEntry(string path, byte[] content)
		{
			using (Stream stream = this._archive.CreateEntry(path, CompressionLevel.Fastest).Open())
			{
				stream.Write(content, 0, content.Length);
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00008E74 File Offset: 0x00007074
		private IEnumerable<Tuple<SheetDto, object>> GetSheets()
		{
			int sheetId = 0;
			IDictionary<string, object> dictionary = this._value as IDictionary<string, object>;
			int num;
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					num = sheetId;
					sheetId = num + 1;
					ExcellSheetInfo sheetInfos = this.GetSheetInfos(keyValuePair.Key);
					yield return Tuple.Create<SheetDto, object>(sheetInfos.ToDto(sheetId), keyValuePair.Value);
				}
				IEnumerator<KeyValuePair<string, object>> enumerator = null;
				yield break;
			}
			DataSet dataSet = this._value as DataSet;
			if (dataSet != null)
			{
				foreach (object obj in dataSet.Tables)
				{
					DataTable dataTable = (DataTable)obj;
					num = sheetId;
					sheetId = num + 1;
					ExcellSheetInfo sheetInfos2 = this.GetSheetInfos(dataTable.TableName);
					yield return Tuple.Create<SheetDto, object>(sheetInfos2.ToDto(sheetId), dataTable);
				}
				IEnumerator enumerator2 = null;
				yield break;
			}
			num = sheetId;
			sheetId = num + 1;
			ExcellSheetInfo sheetInfos3 = this.GetSheetInfos(this._defaultSheetName);
			yield return Tuple.Create<SheetDto, object>(sheetInfos3.ToDto(sheetId), this._value);
			yield break;
			yield break;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008E84 File Offset: 0x00007084
		private ExcellSheetInfo GetSheetInfos(string sheetName)
		{
			ExcellSheetInfo excellSheetInfo = new ExcellSheetInfo
			{
				ExcelSheetName = sheetName,
				Key = sheetName,
				ExcelSheetState = SheetState.Visible
			};
			if (this._configuration.DynamicSheets == null || this._configuration.DynamicSheets.Length == 0)
			{
				return excellSheetInfo;
			}
			DynamicExcelSheet dynamicExcelSheet = this._configuration.DynamicSheets.SingleOrDefault((DynamicExcelSheet _) => _.Key == sheetName);
			if (dynamicExcelSheet == null)
			{
				return excellSheetInfo;
			}
			excellSheetInfo.ExcelSheetState = dynamicExcelSheet.State;
			if (dynamicExcelSheet.Name != null)
			{
				excellSheetInfo.ExcelSheetName = dynamicExcelSheet.Name;
			}
			return excellSheetInfo;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00008F24 File Offset: 0x00007124
		private string GetSheetViews()
		{
			if (this._configuration.FreezeRowCount <= 0 && this._configuration.FreezeColumnCount <= 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<x:sheetViews>");
			stringBuilder.Append(WorksheetXml.StartSheetView(0, 0));
			stringBuilder.Append(this.GetPanes());
			stringBuilder.Append("</x:sheetView>");
			stringBuilder.Append("</x:sheetViews>");
			return stringBuilder.ToString();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008F9C File Offset: 0x0000719C
		private string GetPanes()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string activePane;
			if (this._configuration.FreezeColumnCount > 0 && this._configuration.FreezeRowCount > 0)
			{
				activePane = "bottomRight";
			}
			else if (this._configuration.FreezeColumnCount > 0)
			{
				activePane = "topRight";
			}
			else
			{
				activePane = "bottomLeft";
			}
			stringBuilder.Append(WorksheetXml.StartPane((this._configuration.FreezeColumnCount > 0) ? new int?(this._configuration.FreezeColumnCount) : null, (this._configuration.FreezeRowCount > 0) ? new int?(this._configuration.FreezeRowCount) : null, ExcelOpenXmlUtils.ConvertXyToCell(this._configuration.FreezeColumnCount + 1, this._configuration.FreezeRowCount + 1), activePane, "frozen"));
			if (this._configuration.FreezeColumnCount > 0 && this._configuration.FreezeRowCount > 0)
			{
				string text = ExcelOpenXmlUtils.ConvertXyToCell(this._configuration.FreezeColumnCount + 1, 1);
				stringBuilder.Append(WorksheetXml.PaneSelection("topRight", text, text));
				string text2 = ExcelOpenXmlUtils.ConvertXyToCell(1, this._configuration.FreezeRowCount + 1);
				stringBuilder.Append(WorksheetXml.PaneSelection("bottomLeft", text2, text2));
				string text3 = ExcelOpenXmlUtils.ConvertXyToCell(this._configuration.FreezeColumnCount + 1, this._configuration.FreezeRowCount + 1);
				stringBuilder.Append(WorksheetXml.PaneSelection("bottomRight", text3, text3));
			}
			else if (this._configuration.FreezeColumnCount > 0)
			{
				string text4 = ExcelOpenXmlUtils.ConvertXyToCell(this._configuration.FreezeColumnCount, 1);
				stringBuilder.Append(WorksheetXml.PaneSelection("topRight", text4, text4));
			}
			else
			{
				stringBuilder.Append(WorksheetXml.PaneSelection("bottomLeft", null, null));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009170 File Offset: 0x00007370
		private Tuple<string, string, string> GetCellValue(int rowIndex, int cellIndex, object value, ExcelColumnInfo columnInfo, bool valueIsNull)
		{
			if (valueIsNull)
			{
				return Tuple.Create<string, string, string>("2", "str", string.Empty);
			}
			string text = value as string;
			if (text != null)
			{
				return Tuple.Create<string, string, string>("2", "str", ExcelOpenXmlUtils.EncodeXML(text));
			}
			Type valueType = ExcelOpenXmlSheetWriter.GetValueType(value, columnInfo);
			if (columnInfo != null && columnInfo.ExcelFormat != null && columnInfo != null && columnInfo.ExcelFormatId == -1)
			{
				IFormattable formattable = value as IFormattable;
				if (formattable != null)
				{
					string value2 = formattable.ToString(columnInfo.ExcelFormat, this._configuration.Culture);
					return Tuple.Create<string, string, string>("2", "str", ExcelOpenXmlUtils.EncodeXML(value2));
				}
			}
			if (valueType == typeof(DateTime))
			{
				return this.GetDateTimeValue((DateTime)value, columnInfo);
			}
			if (valueType.IsEnum)
			{
				string text2 = CustomPropertyHelper.DescriptionAttr(valueType, value);
				return Tuple.Create<string, string, string>("2", "str", text2 ?? value.ToString());
			}
			if (TypeHelper.IsNumericType(valueType, false))
			{
				string item = (this._configuration.Culture == CultureInfo.InvariantCulture) ? "n" : "str";
				string numericValue = this.GetNumericValue(value, valueType);
				return Tuple.Create<string, string, string>("2", item, numericValue);
			}
			if (valueType == typeof(bool))
			{
				return Tuple.Create<string, string, string>("2", "b", ((bool)value) ? "1" : "0");
			}
			if (valueType == typeof(byte[]) && this._configuration.EnableConvertByteArray)
			{
				string fileValue = this.GetFileValue(rowIndex, cellIndex, value);
				return Tuple.Create<string, string, string>("4", "str", ExcelOpenXmlUtils.EncodeXML(fileValue));
			}
			return Tuple.Create<string, string, string>("2", "str", ExcelOpenXmlUtils.EncodeXML(value.ToString()));
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009334 File Offset: 0x00007534
		private static Type GetValueType(object value, ExcelColumnInfo columnInfo)
		{
			Type type;
			if (columnInfo == null || columnInfo.Key != null)
			{
				type = value.GetType();
				type = (Nullable.GetUnderlyingType(type) ?? type);
			}
			else
			{
				type = columnInfo.ExcludeNullableType;
			}
			return type;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000936C File Offset: 0x0000756C
		private string GetNumericValue(object value, Type type)
		{
			if (type.IsAssignableFrom(typeof(decimal)))
			{
				return ((decimal)value).ToString(this._configuration.Culture);
			}
			if (type.IsAssignableFrom(typeof(int)))
			{
				return ((int)value).ToString(this._configuration.Culture);
			}
			if (type.IsAssignableFrom(typeof(double)))
			{
				return ((double)value).ToString(this._configuration.Culture);
			}
			if (type.IsAssignableFrom(typeof(long)))
			{
				return ((long)value).ToString(this._configuration.Culture);
			}
			if (type.IsAssignableFrom(typeof(uint)))
			{
				return ((uint)value).ToString(this._configuration.Culture);
			}
			if (type.IsAssignableFrom(typeof(ushort)))
			{
				return ((ushort)value).ToString(this._configuration.Culture);
			}
			if (type.IsAssignableFrom(typeof(ulong)))
			{
				return ((ulong)value).ToString(this._configuration.Culture);
			}
			if (type.IsAssignableFrom(typeof(short)))
			{
				return ((short)value).ToString(this._configuration.Culture);
			}
			if (type.IsAssignableFrom(typeof(float)))
			{
				return ((float)value).ToString(this._configuration.Culture);
			}
			return decimal.Parse(value.ToString()).ToString(this._configuration.Culture);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009528 File Offset: 0x00007728
		private string GetFileValue(int rowIndex, int cellIndex, object value)
		{
			byte[] array = (byte[])value;
			ImageHelper.ImageFormat imageFormat = ImageHelper.GetImageFormat(array);
			FileDto fileDto = new FileDto
			{
				Byte = array,
				RowIndex = rowIndex,
				CellIndex = cellIndex,
				SheetId = this.currentSheetIndex
			};
			if (imageFormat != ImageHelper.ImageFormat.unknown)
			{
				fileDto.Extension = imageFormat.ToString();
				fileDto.IsImage = true;
			}
			else
			{
				fileDto.Extension = "bin";
			}
			this._files.Add(fileDto);
			return "@@@fileid@@@," + fileDto.Path;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000095B4 File Offset: 0x000077B4
		private Tuple<string, string, string> GetDateTimeValue(DateTime value, ExcelColumnInfo columnInfo)
		{
			string item;
			if (this._configuration.Culture != CultureInfo.InvariantCulture)
			{
				item = value.ToString(this._configuration.Culture);
				return Tuple.Create<string, string, string>("2", "str", item);
			}
			item = ExcelOpenXmlSheetWriter.CorrectDateTimeValue(value).ToString(CultureInfo.InvariantCulture);
			if (columnInfo == null || columnInfo.ExcelFormat == null)
			{
				return Tuple.Create<string, string, string>("3", null, item);
			}
			return Tuple.Create<string, string, string>(columnInfo.ExcelFormatId.ToString(), null, item);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000963C File Offset: 0x0000783C
		private static double CorrectDateTimeValue(DateTime value)
		{
			double num = value.ToOADate();
			if (num <= 60.0)
			{
				num -= 1.0;
			}
			return num;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000966C File Offset: 0x0000786C
		private string GetDimensionRef(int maxRowIndex, int maxColumnIndex)
		{
			string result;
			if (maxRowIndex == 0 && maxColumnIndex == 0)
			{
				result = "A1";
			}
			else if (maxColumnIndex <= 1)
			{
				result = string.Format("A{0}", maxRowIndex);
			}
			else if (maxRowIndex == 0)
			{
				result = "A1:" + ColumnHelper.GetAlphabetColumnName(maxColumnIndex - 1) + "1";
			}
			else
			{
				result = string.Format("A1:{0}{1}", ColumnHelper.GetAlphabetColumnName(maxColumnIndex - 1), maxRowIndex);
			}
			return result;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000096D8 File Offset: 0x000078D8
		private string GetDrawingRelationshipXml(int sheetIndex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<FileDto> files = this._files;
			Func<FileDto, bool> <>9__0;
			Func<FileDto, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((FileDto w) => w.IsImage && w.SheetId == sheetIndex + 1));
			}
			foreach (FileDto image in files.Where(predicate))
			{
				stringBuilder.AppendLine(ExcelXml.ImageRelationship(image));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000976C File Offset: 0x0000796C
		private string GetDrawingXml(int sheetIndex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this._files.Count; i++)
			{
				FileDto fileDto = this._files[i];
				if (fileDto.IsImage && fileDto.SheetId == sheetIndex + 1)
				{
					stringBuilder.Append(ExcelXml.DrawingXml(fileDto, i));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000097CC File Offset: 0x000079CC
		private void GenerateWorkBookXmls(out StringBuilder workbookXml, out StringBuilder workbookRelsXml, out Dictionary<int, string> sheetsRelsXml)
		{
			workbookXml = new StringBuilder();
			workbookRelsXml = new StringBuilder();
			sheetsRelsXml = new Dictionary<int, string>();
			int num = 0;
			foreach (SheetDto sheetDto in this._sheets)
			{
				num++;
				workbookXml.AppendLine(ExcelXml.Sheet(sheetDto, num));
				workbookRelsXml.AppendLine(ExcelXml.WorksheetRelationship(sheetDto));
				sheetsRelsXml.Add(sheetDto.SheetIdx, ExcelXml.DrawingRelationship(num));
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009864 File Offset: 0x00007A64
		private string GetContentTypesXml()
		{
			StringBuilder stringBuilder = new StringBuilder(ExcelXml.StartTypes);
			foreach (KeyValuePair<string, ZipPackageInfo> keyValuePair in this._zipDictionary)
			{
				stringBuilder.Append(ExcelXml.ContentType(keyValuePair.Value.ContentType, keyValuePair.Key));
			}
			stringBuilder.Append(ExcelXml.EndTypes);
			return stringBuilder.ToString();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000098EC File Offset: 0x00007AEC
		private string GetCellXfId(string styleIndex)
		{
			string text;
			if (this.cellXfIdMap.TryGetValue(styleIndex, out text))
			{
				return text.ToString();
			}
			return styleIndex.ToString();
		}

		// Token: 0x0400009D RID: 157
		private readonly MiniExcelZipArchive _archive;

		// Token: 0x0400009E RID: 158
		private static readonly UTF8Encoding _utf8WithBom = new UTF8Encoding(true);

		// Token: 0x0400009F RID: 159
		private readonly OpenXmlConfiguration _configuration;

		// Token: 0x040000A0 RID: 160
		private readonly Stream _stream;

		// Token: 0x040000A1 RID: 161
		private readonly bool _printHeader;

		// Token: 0x040000A2 RID: 162
		private readonly object _value;

		// Token: 0x040000A3 RID: 163
		private readonly string _defaultSheetName;

		// Token: 0x040000A4 RID: 164
		private readonly List<SheetDto> _sheets = new List<SheetDto>();

		// Token: 0x040000A5 RID: 165
		private readonly List<FileDto> _files = new List<FileDto>();

		// Token: 0x040000A6 RID: 166
		private int currentSheetIndex;

		// Token: 0x040000A7 RID: 167
		private readonly Dictionary<string, ZipPackageInfo> _zipDictionary = new Dictionary<string, ZipPackageInfo>();

		// Token: 0x040000A8 RID: 168
		private Dictionary<string, string> cellXfIdMap;
	}
}
