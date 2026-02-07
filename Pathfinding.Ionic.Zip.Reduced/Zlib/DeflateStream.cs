using System;
using System.IO;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x0200004E RID: 78
	public class DeflateStream : Stream
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x00018120 File Offset: 0x00016320
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001812C File Offset: 0x0001632C
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00018138 File Offset: 0x00016338
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00018144 File Offset: 0x00016344
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00018174 File Offset: 0x00016374
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00018184 File Offset: 0x00016384
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
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x000181B4 File Offset: 0x000163B4
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x000181C4 File Offset: 0x000163C4
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
					throw new ObjectDisposedException("DeflateStream");
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0001823C File Offset: 0x0001643C
		// (set) Token: 0x060003AB RID: 939 RVA: 0x0001824C File Offset: 0x0001644C
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0001827C File Offset: 0x0001647C
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00018290 File Offset: 0x00016490
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x000182A4 File Offset: 0x000164A4
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00018308 File Offset: 0x00016508
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0001833C File Offset: 0x0001653C
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00018340 File Offset: 0x00016540
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00018374 File Offset: 0x00016574
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00018398 File Offset: 0x00016598
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x000183A0 File Offset: 0x000165A0
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x000183F4 File Offset: 0x000165F4
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
				throw new NotImplementedException();
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000183FC File Offset: 0x000165FC
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00018430 File Offset: 0x00016630
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00018438 File Offset: 0x00016638
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00018440 File Offset: 0x00016640
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00018474 File Offset: 0x00016674
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000184D4 File Offset: 0x000166D4
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00018534 File Offset: 0x00016734
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001858C File Offset: 0x0001678C
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04000272 RID: 626
		internal ZlibBaseStream _baseStream;

		// Token: 0x04000273 RID: 627
		internal Stream _innerStream;

		// Token: 0x04000274 RID: 628
		private bool _disposed;
	}
}
