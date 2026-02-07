using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x02000048 RID: 72
	internal class MiniExcelAsyncStreamWriter : IDisposable
	{
		// Token: 0x06000215 RID: 533 RVA: 0x0000A56D File Offset: 0x0000876D
		public MiniExcelAsyncStreamWriter(Stream stream, Encoding encoding, int bufferSize, CancellationToken cancellationToken)
		{
			this._stream = stream;
			this._encoding = encoding;
			this._cancellationToken = cancellationToken;
			this._streamWriter = new StreamWriter(stream, this._encoding, bufferSize);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000A5A0 File Offset: 0x000087A0
		public Task WriteAsync(string content)
		{
			MiniExcelAsyncStreamWriter.<WriteAsync>d__6 <WriteAsync>d__;
			<WriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsync>d__.<>4__this = this;
			<WriteAsync>d__.content = content;
			<WriteAsync>d__.<>1__state = -1;
			<WriteAsync>d__.<>t__builder.Start<MiniExcelAsyncStreamWriter.<WriteAsync>d__6>(ref <WriteAsync>d__);
			return <WriteAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000A5EC File Offset: 0x000087EC
		public Task<long> WriteAndFlushAsync(string content)
		{
			MiniExcelAsyncStreamWriter.<WriteAndFlushAsync>d__7 <WriteAndFlushAsync>d__;
			<WriteAndFlushAsync>d__.<>t__builder = AsyncTaskMethodBuilder<long>.Create();
			<WriteAndFlushAsync>d__.<>4__this = this;
			<WriteAndFlushAsync>d__.content = content;
			<WriteAndFlushAsync>d__.<>1__state = -1;
			<WriteAndFlushAsync>d__.<>t__builder.Start<MiniExcelAsyncStreamWriter.<WriteAndFlushAsync>d__7>(ref <WriteAndFlushAsync>d__);
			return <WriteAndFlushAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A638 File Offset: 0x00008838
		public Task WriteWhitespaceAsync(int length)
		{
			MiniExcelAsyncStreamWriter.<WriteWhitespaceAsync>d__8 <WriteWhitespaceAsync>d__;
			<WriteWhitespaceAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteWhitespaceAsync>d__.<>4__this = this;
			<WriteWhitespaceAsync>d__.length = length;
			<WriteWhitespaceAsync>d__.<>1__state = -1;
			<WriteWhitespaceAsync>d__.<>t__builder.Start<MiniExcelAsyncStreamWriter.<WriteWhitespaceAsync>d__8>(ref <WriteWhitespaceAsync>d__);
			return <WriteWhitespaceAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000A684 File Offset: 0x00008884
		public Task<long> FlushAsync()
		{
			MiniExcelAsyncStreamWriter.<FlushAsync>d__9 <FlushAsync>d__;
			<FlushAsync>d__.<>t__builder = AsyncTaskMethodBuilder<long>.Create();
			<FlushAsync>d__.<>4__this = this;
			<FlushAsync>d__.<>1__state = -1;
			<FlushAsync>d__.<>t__builder.Start<MiniExcelAsyncStreamWriter.<FlushAsync>d__9>(ref <FlushAsync>d__);
			return <FlushAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000A6C7 File Offset: 0x000088C7
		public void SetPosition(long position)
		{
			this._streamWriter.BaseStream.Position = position;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000A6DA File Offset: 0x000088DA
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				StreamWriter streamWriter = this._streamWriter;
				if (streamWriter != null)
				{
					streamWriter.Dispose();
				}
				this.disposedValue = true;
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000A6FC File Offset: 0x000088FC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x040000B9 RID: 185
		private readonly Stream _stream;

		// Token: 0x040000BA RID: 186
		private readonly Encoding _encoding;

		// Token: 0x040000BB RID: 187
		private readonly CancellationToken _cancellationToken;

		// Token: 0x040000BC RID: 188
		private readonly StreamWriter _streamWriter;

		// Token: 0x040000BD RID: 189
		private bool disposedValue;
	}
}
