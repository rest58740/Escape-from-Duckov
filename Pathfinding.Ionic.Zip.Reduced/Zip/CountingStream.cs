using System;
using System.IO;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000025 RID: 37
	public class CountingStream : Stream
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00004900 File Offset: 0x00002B00
		public CountingStream(Stream stream)
		{
			this._s = stream;
			try
			{
				this._initialOffset = this._s.Position;
			}
			catch
			{
				this._initialOffset = 0L;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000495C File Offset: 0x00002B5C
		public Stream WrappedStream
		{
			get
			{
				return this._s;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004964 File Offset: 0x00002B64
		public long BytesWritten
		{
			get
			{
				return this._bytesWritten;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000BA RID: 186 RVA: 0x0000496C File Offset: 0x00002B6C
		public long BytesRead
		{
			get
			{
				return this._bytesRead;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004974 File Offset: 0x00002B74
		public void Adjust(long delta)
		{
			this._bytesWritten -= delta;
			if (this._bytesWritten < 0L)
			{
				throw new InvalidOperationException();
			}
			if (this._s is CountingStream)
			{
				((CountingStream)this._s).Adjust(delta);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000049C4 File Offset: 0x00002BC4
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this._s.Read(buffer, offset, count);
			this._bytesRead += (long)num;
			return num;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000049F0 File Offset: 0x00002BF0
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			this._s.Write(buffer, offset, count);
			this._bytesWritten += (long)count;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004A24 File Offset: 0x00002C24
		public override bool CanRead
		{
			get
			{
				return this._s.CanRead;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004A34 File Offset: 0x00002C34
		public override bool CanSeek
		{
			get
			{
				return this._s.CanSeek;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004A44 File Offset: 0x00002C44
		public override bool CanWrite
		{
			get
			{
				return this._s.CanWrite;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004A54 File Offset: 0x00002C54
		public override void Flush()
		{
			this._s.Flush();
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004A64 File Offset: 0x00002C64
		public override long Length
		{
			get
			{
				return this._s.Length;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004A74 File Offset: 0x00002C74
		public long ComputedPosition
		{
			get
			{
				return this._initialOffset + this._bytesWritten;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004A84 File Offset: 0x00002C84
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004A94 File Offset: 0x00002C94
		public override long Position
		{
			get
			{
				return this._s.Position;
			}
			set
			{
				this._s.Seek(value, 0);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004AA4 File Offset: 0x00002CA4
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._s.Seek(offset, origin);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004AB4 File Offset: 0x00002CB4
		public override void SetLength(long value)
		{
			this._s.SetLength(value);
		}

		// Token: 0x0400005F RID: 95
		private Stream _s;

		// Token: 0x04000060 RID: 96
		private long _bytesWritten;

		// Token: 0x04000061 RID: 97
		private long _bytesRead;

		// Token: 0x04000062 RID: 98
		private long _initialOffset;
	}
}
