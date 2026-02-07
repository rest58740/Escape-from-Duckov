using System;
using System.IO;
using System.Text;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x02000049 RID: 73
	internal class MiniExcelStreamWriter : IDisposable
	{
		// Token: 0x0600021D RID: 541 RVA: 0x0000A70B File Offset: 0x0000890B
		public MiniExcelStreamWriter(Stream stream, Encoding encoding, int bufferSize)
		{
			this._stream = stream;
			this._encoding = encoding;
			this._streamWriter = new StreamWriter(stream, this._encoding, bufferSize);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A734 File Offset: 0x00008934
		public void Write(string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				return;
			}
			this._streamWriter.Write(content);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000A74B File Offset: 0x0000894B
		public long WriteAndFlush(string content)
		{
			this.Write(content);
			this._streamWriter.Flush();
			return this._streamWriter.BaseStream.Position;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A76F File Offset: 0x0000896F
		public void WriteWhitespace(int length)
		{
			this._streamWriter.Write(new string(' ', length));
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000A784 File Offset: 0x00008984
		public long Flush()
		{
			this._streamWriter.Flush();
			return this._streamWriter.BaseStream.Position;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000A7A1 File Offset: 0x000089A1
		public void SetPosition(long position)
		{
			this._streamWriter.BaseStream.Position = position;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000A7B4 File Offset: 0x000089B4
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

		// Token: 0x06000224 RID: 548 RVA: 0x0000A7D6 File Offset: 0x000089D6
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x040000BE RID: 190
		private readonly Stream _stream;

		// Token: 0x040000BF RID: 191
		private readonly Encoding _encoding;

		// Token: 0x040000C0 RID: 192
		private readonly StreamWriter _streamWriter;

		// Token: 0x040000C1 RID: 193
		private bool disposedValue;
	}
}
