using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiniExcelLibs.Utils;
using MiniExcelLibs.WriteAdapter;

namespace MiniExcelLibs.Csv
{
	// Token: 0x02000067 RID: 103
	internal class CsvWriter : IExcelWriter, IDisposable
	{
		// Token: 0x0600036B RID: 875 RVA: 0x00012EEC File Offset: 0x000110EC
		public CsvWriter(Stream stream, object value, IConfiguration configuration, bool printHeader)
		{
			this._stream = stream;
			this._configuration = ((configuration == null) ? CsvConfiguration.DefaultConfiguration : ((CsvConfiguration)configuration));
			this._printHeader = printHeader;
			this._value = value;
			this._writer = this._configuration.StreamWriterFunc(this._stream);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00012F48 File Offset: 0x00011148
		public void SaveAs()
		{
			if (this._value == null)
			{
				this._writer.Write("");
				this._writer.Flush();
				return;
			}
			this.WriteValues(this._writer, this._value);
			this._writer.Flush();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00012F96 File Offset: 0x00011196
		public void Insert(bool overwriteSheet = false)
		{
			this.SaveAs();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00012FA0 File Offset: 0x000111A0
		private void AppendColumn(StringBuilder rowBuilder, CellWriteInfo column)
		{
			rowBuilder.Append(CsvHelpers.ConvertToCsvValue(this.ToCsvString(column.Value, column.Prop), this._configuration.AlwaysQuote, this._configuration.Seperator));
			rowBuilder.Append(this._configuration.Seperator);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00012FF5 File Offset: 0x000111F5
		private void RemoveTrailingSeparator(StringBuilder rowBuilder)
		{
			if (rowBuilder.Length == 0)
			{
				return;
			}
			rowBuilder.Remove(rowBuilder.Length - 1, 1);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00013010 File Offset: 0x00011210
		private string GetHeader(List<ExcelColumnInfo> props)
		{
			return string.Join(this._configuration.Seperator.ToString(), from s in props
			select CsvHelpers.ConvertToCsvValue((s != null) ? s.ExcelColumnName : null, this._configuration.AlwaysQuote, this._configuration.Seperator));
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00013048 File Offset: 0x00011248
		private void WriteValues(StreamWriter writer, object values)
		{
			IMiniExcelWriteAdapter writeAdapter = MiniExcelWriteAdapterFactory.GetWriteAdapter(values, this._configuration);
			List<ExcelColumnInfo> columns = writeAdapter.GetColumns();
			if (columns == null)
			{
				this._writer.Write(this._configuration.NewLine);
				this._writer.Flush();
				return;
			}
			if (this._printHeader)
			{
				this._writer.Write(this.GetHeader(columns));
				this._writer.Write(this._configuration.NewLine);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (writeAdapter != null)
			{
				foreach (IEnumerable<CellWriteInfo> enumerable in writeAdapter.GetRows(columns, default(CancellationToken)))
				{
					stringBuilder.Clear();
					foreach (CellWriteInfo column in enumerable)
					{
						this.AppendColumn(stringBuilder, column);
					}
					this.RemoveTrailingSeparator(stringBuilder);
					this._writer.Write(stringBuilder.ToString());
					this._writer.Write(this._configuration.NewLine);
				}
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00013180 File Offset: 0x00011380
		private Task WriteValuesAsync(StreamWriter writer, object values, string seperator, string newLine, CancellationToken cancellationToken)
		{
			CsvWriter.<WriteValuesAsync>d__13 <WriteValuesAsync>d__;
			<WriteValuesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteValuesAsync>d__.<>4__this = this;
			<WriteValuesAsync>d__.values = values;
			<WriteValuesAsync>d__.newLine = newLine;
			<WriteValuesAsync>d__.cancellationToken = cancellationToken;
			<WriteValuesAsync>d__.<>1__state = -1;
			<WriteValuesAsync>d__.<>t__builder.Start<CsvWriter.<WriteValuesAsync>d__13>(ref <WriteValuesAsync>d__);
			return <WriteValuesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000131E0 File Offset: 0x000113E0
		public Task SaveAsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			CsvWriter.<SaveAsAsync>d__14 <SaveAsAsync>d__;
			<SaveAsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsAsync>d__.<>4__this = this;
			<SaveAsAsync>d__.cancellationToken = cancellationToken;
			<SaveAsAsync>d__.<>1__state = -1;
			<SaveAsAsync>d__.<>t__builder.Start<CsvWriter.<SaveAsAsync>d__14>(ref <SaveAsAsync>d__);
			return <SaveAsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001322C File Offset: 0x0001142C
		public Task InsertAsync(bool overwriteSheet = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			CsvWriter.<InsertAsync>d__15 <InsertAsync>d__;
			<InsertAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InsertAsync>d__.<>4__this = this;
			<InsertAsync>d__.cancellationToken = cancellationToken;
			<InsertAsync>d__.<>1__state = -1;
			<InsertAsync>d__.<>t__builder.Start<CsvWriter.<InsertAsync>d__15>(ref <InsertAsync>d__);
			return <InsertAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00013278 File Offset: 0x00011478
		public string ToCsvString(object value, ExcelColumnInfo p)
		{
			if (value == null)
			{
				return "";
			}
			if (!(value is DateTime))
			{
				if (p != null && p.ExcelFormat != null)
				{
					IFormattable formattable = value as IFormattable;
					if (formattable != null)
					{
						return formattable.ToString(p.ExcelFormat, this._configuration.Culture);
					}
				}
				return Convert.ToString(value, this._configuration.Culture);
			}
			DateTime dateTime = (DateTime)value;
			if (p != null && p.ExcelFormat != null)
			{
				return dateTime.ToString(p.ExcelFormat, this._configuration.Culture);
			}
			if (!this._configuration.Culture.Equals(CultureInfo.InvariantCulture))
			{
				return dateTime.ToString(this._configuration.Culture);
			}
			return dateTime.ToString("yyyy-MM-dd HH:mm:ss", this._configuration.Culture);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00013341 File Offset: 0x00011541
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this._writer.Dispose();
				}
				this.disposedValue = true;
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00013360 File Offset: 0x00011560
		~CsvWriter()
		{
			this.Dispose(false);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00013390 File Offset: 0x00011590
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400015D RID: 349
		private readonly Stream _stream;

		// Token: 0x0400015E RID: 350
		private readonly CsvConfiguration _configuration;

		// Token: 0x0400015F RID: 351
		private readonly bool _printHeader;

		// Token: 0x04000160 RID: 352
		private object _value;

		// Token: 0x04000161 RID: 353
		private readonly StreamWriter _writer;

		// Token: 0x04000162 RID: 354
		private bool disposedValue;
	}
}
