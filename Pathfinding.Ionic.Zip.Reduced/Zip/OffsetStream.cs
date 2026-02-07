using System;
using System.IO;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000023 RID: 35
	internal class OffsetStream : Stream, IDisposable
	{
		// Token: 0x06000093 RID: 147 RVA: 0x00003EA4 File Offset: 0x000020A4
		public OffsetStream(Stream s)
		{
			this._originalPosition = s.Position;
			this._innerStream = s;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003EC0 File Offset: 0x000020C0
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003EC8 File Offset: 0x000020C8
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._innerStream.Read(buffer, offset, count);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003ED8 File Offset: 0x000020D8
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003EE0 File Offset: 0x000020E0
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003EF0 File Offset: 0x000020F0
		public override bool CanSeek
		{
			get
			{
				return this._innerStream.CanSeek;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00003F00 File Offset: 0x00002100
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003F04 File Offset: 0x00002104
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00003F14 File Offset: 0x00002114
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003F24 File Offset: 0x00002124
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00003F38 File Offset: 0x00002138
		public override long Position
		{
			get
			{
				return this._innerStream.Position - this._originalPosition;
			}
			set
			{
				this._innerStream.Position = this._originalPosition + value;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003F50 File Offset: 0x00002150
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(this._originalPosition + offset, origin) - this._originalPosition;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003F70 File Offset: 0x00002170
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003F78 File Offset: 0x00002178
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x0400005A RID: 90
		private long _originalPosition;

		// Token: 0x0400005B RID: 91
		private Stream _innerStream;
	}
}
