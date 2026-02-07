using System;
using System.IO;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x02000069 RID: 105
	public class ZlibStream : Stream
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x0001EF54 File Offset: 0x0001D154
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001EF60 File Offset: 0x0001D160
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001EF6C File Offset: 0x0001D16C
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001EF78 File Offset: 0x0001D178
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0001EFA0 File Offset: 0x0001D1A0
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x0001EFB0 File Offset: 0x0001D1B0
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001EFE0 File Offset: 0x0001D1E0
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x0001EFF0 File Offset: 0x0001D1F0
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0001F068 File Offset: 0x0001D268
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x0001F07C File Offset: 0x0001D27C
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001F090 File Offset: 0x0001D290
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0001F0F4 File Offset: 0x0001D2F4
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0001F128 File Offset: 0x0001D328
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x0001F12C File Offset: 0x0001D32C
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001F160 File Offset: 0x0001D360
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0001F184 File Offset: 0x0001D384
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0001F18C File Offset: 0x0001D38C
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x0001F1E0 File Offset: 0x0001D3E0
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001F1E8 File Offset: 0x0001D3E8
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001F21C File Offset: 0x0001D41C
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001F224 File Offset: 0x0001D424
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001F22C File Offset: 0x0001D42C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001F260 File Offset: 0x0001D460
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001F2C0 File Offset: 0x0001D4C0
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001F320 File Offset: 0x0001D520
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001F378 File Offset: 0x0001D578
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04000396 RID: 918
		internal ZlibBaseStream _baseStream;

		// Token: 0x04000397 RID: 919
		private bool _disposed;
	}
}
