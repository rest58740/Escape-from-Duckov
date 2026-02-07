using System;
using System.IO;
using System.Text;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x0200004F RID: 79
	public class GZipStream : Stream
	{
		// Token: 0x060003BE RID: 958 RVA: 0x000185E4 File Offset: 0x000167E4
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000185F0 File Offset: 0x000167F0
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000185FC File Offset: 0x000167FC
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00018608 File Offset: 0x00016808
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00018664 File Offset: 0x00016864
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x0001866C File Offset: 0x0001686C
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0001868C File Offset: 0x0001688C
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x00018694 File Offset: 0x00016894
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00018744 File Offset: 0x00016944
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0001874C File Offset: 0x0001694C
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x0001875C File Offset: 0x0001695C
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
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0001878C File Offset: 0x0001698C
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0001879C File Offset: 0x0001699C
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
					throw new ObjectDisposedException("GZipStream");
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003CC RID: 972 RVA: 0x00018814 File Offset: 0x00016A14
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00018828 File Offset: 0x00016A28
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001883C File Offset: 0x00016A3C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003CF RID: 975 RVA: 0x000188B4 File Offset: 0x00016AB4
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x000188E8 File Offset: 0x00016AE8
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x000188EC File Offset: 0x00016AEC
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00018920 File Offset: 0x00016B20
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00018944 File Offset: 0x00016B44
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0001894C File Offset: 0x00016B4C
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x000189B4 File Offset: 0x00016BB4
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000189BC File Offset: 0x00016BBC
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00018A24 File Offset: 0x00016C24
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00018A2C File Offset: 0x00016C2C
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00018A34 File Offset: 0x00016C34
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00018AA0 File Offset: 0x00016CA0
		private int EmitHeader()
		{
			byte[] array = (this.Comment != null) ? GZipStream.iso8859dash1.GetBytes(this.Comment) : null;
			byte[] array2 = (this.FileName != null) ? GZipStream.iso8859dash1.GetBytes(this.FileName) : null;
			int num = (this.Comment != null) ? (array.Length + 1) : 0;
			int num2 = (this.FileName != null) ? (array2.Length + 1) : 0;
			int num3 = 10 + num + num2;
			byte[] array3 = new byte[num3];
			int num4 = 0;
			array3[num4++] = 31;
			array3[num4++] = 139;
			array3[num4++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num4++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			int num5 = (int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds;
			Array.Copy(BitConverter.GetBytes(num5), 0, array3, num4, 4);
			num4 += 4;
			array3[num4++] = 0;
			array3[num4++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num4, num2 - 1);
				num4 += num2 - 1;
				array3[num4++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num4, num - 1);
				num4 += num - 1;
				array3[num4++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00018C6C File Offset: 0x00016E6C
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00018CCC File Offset: 0x00016ECC
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00018D2C File Offset: 0x00016F2C
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00018D84 File Offset: 0x00016F84
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04000275 RID: 629
		public DateTime? LastModified;

		// Token: 0x04000276 RID: 630
		private int _headerByteCount;

		// Token: 0x04000277 RID: 631
		internal ZlibBaseStream _baseStream;

		// Token: 0x04000278 RID: 632
		private bool _disposed;

		// Token: 0x04000279 RID: 633
		private bool _firstReadDone;

		// Token: 0x0400027A RID: 634
		private string _FileName;

		// Token: 0x0400027B RID: 635
		private string _Comment;

		// Token: 0x0400027C RID: 636
		private int _Crc32;

		// Token: 0x0400027D RID: 637
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 1);

		// Token: 0x0400027E RID: 638
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("UTF-8");
	}
}
