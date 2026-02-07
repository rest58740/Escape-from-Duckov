using System;
using System.IO;
using System.Text;
using Pathfinding.Ionic.Crc;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000037 RID: 55
	public class ZipInputStream : Stream
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0000F35C File Offset: 0x0000D55C
		public ZipInputStream(Stream stream) : this(stream, false)
		{
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000F368 File Offset: 0x0000D568
		public ZipInputStream(string fileName)
		{
			Stream stream = File.Open(fileName, 3, 1, 1);
			this._Init(stream, false, fileName);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000F390 File Offset: 0x0000D590
		public ZipInputStream(Stream stream, bool leaveOpen)
		{
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000F3A4 File Offset: 0x0000D5A4
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._inputStream = stream;
			if (!this._inputStream.CanRead)
			{
				throw new ZipException("The stream must be readable.");
			}
			this._container = new ZipContainer(this);
			this._provisionalAlternateEncoding = Encoding.UTF8;
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this._findRequired = true;
			this._name = (name ?? "(stream)");
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000F40C File Offset: 0x0000D60C
		public override string ToString()
		{
			return string.Format("ZipInputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000F42C File Offset: 0x0000D62C
		// (set) Token: 0x0600026B RID: 619 RVA: 0x0000F434 File Offset: 0x0000D634
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				return this._provisionalAlternateEncoding;
			}
			set
			{
				this._provisionalAlternateEncoding = value;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000F440 File Offset: 0x0000D640
		// (set) Token: 0x0600026D RID: 621 RVA: 0x0000F448 File Offset: 0x0000D648
		public int CodecBufferSize { get; set; }

		// Token: 0x1700008A RID: 138
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000F454 File Offset: 0x0000D654
		public string Password
		{
			set
			{
				if (this._closed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._Password = value;
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000F488 File Offset: 0x0000D688
		private void SetupStream()
		{
			this._crcStream = this._currentEntry.InternalOpenReader(this._Password);
			this._LeftToRead = this._crcStream.Length;
			this._needSetup = false;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000F4BC File Offset: 0x0000D6BC
		internal Stream ReadStream
		{
			get
			{
				return this._inputStream;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._closed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (this._needSetup)
			{
				this.SetupStream();
			}
			if (this._LeftToRead == 0L)
			{
				return 0;
			}
			int num = (this._LeftToRead <= (long)count) ? ((int)this._LeftToRead) : count;
			int num2 = this._crcStream.Read(buffer, offset, num);
			this._LeftToRead -= (long)num2;
			if (this._LeftToRead == 0L)
			{
				int crc = this._crcStream.Crc;
				this._currentEntry.VerifyCrcAfterExtract(crc);
				this._inputStream.Seek(this._endOfEntry, 0);
			}
			return num2;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000F580 File Offset: 0x0000D780
		public ZipEntry GetNextEntry()
		{
			if (this._findRequired)
			{
				long num = SharedUtilities.FindSignature(this._inputStream, 67324752);
				if (num == -1L)
				{
					return null;
				}
				this._inputStream.Seek(-4L, 1);
			}
			else if (this._firstEntry)
			{
				this._inputStream.Seek(this._endOfEntry, 0);
			}
			this._currentEntry = ZipEntry.ReadEntry(this._container, !this._firstEntry);
			this._endOfEntry = this._inputStream.Position;
			this._firstEntry = true;
			this._needSetup = true;
			this._findRequired = false;
			return this._currentEntry;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000F62C File Offset: 0x0000D82C
		protected override void Dispose(bool disposing)
		{
			if (this._closed)
			{
				return;
			}
			if (disposing)
			{
				if (this._exceptionPending)
				{
					return;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					this._inputStream.Dispose();
				}
			}
			this._closed = true;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000F66C File Offset: 0x0000D86C
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000F670 File Offset: 0x0000D870
		public override bool CanSeek
		{
			get
			{
				return this._inputStream.CanSeek;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000F680 File Offset: 0x0000D880
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000F684 File Offset: 0x0000D884
		public override long Length
		{
			get
			{
				return this._inputStream.Length;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000F694 File Offset: 0x0000D894
		// (set) Token: 0x06000279 RID: 633 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
		public override long Position
		{
			get
			{
				return this._inputStream.Position;
			}
			set
			{
				this.Seek(value, 0);
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000F6B0 File Offset: 0x0000D8B0
		public override void Flush()
		{
			throw new NotSupportedException("Flush");
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000F6BC File Offset: 0x0000D8BC
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Write");
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000F6C8 File Offset: 0x0000D8C8
		public override long Seek(long offset, SeekOrigin origin)
		{
			this._findRequired = true;
			return this._inputStream.Seek(offset, origin);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000F6EC File Offset: 0x0000D8EC
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400012E RID: 302
		private Stream _inputStream;

		// Token: 0x0400012F RID: 303
		private Encoding _provisionalAlternateEncoding;

		// Token: 0x04000130 RID: 304
		private ZipEntry _currentEntry;

		// Token: 0x04000131 RID: 305
		private bool _firstEntry;

		// Token: 0x04000132 RID: 306
		private bool _needSetup;

		// Token: 0x04000133 RID: 307
		private ZipContainer _container;

		// Token: 0x04000134 RID: 308
		private CrcCalculatorStream _crcStream;

		// Token: 0x04000135 RID: 309
		private long _LeftToRead;

		// Token: 0x04000136 RID: 310
		internal string _Password;

		// Token: 0x04000137 RID: 311
		private long _endOfEntry;

		// Token: 0x04000138 RID: 312
		private string _name;

		// Token: 0x04000139 RID: 313
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x0400013A RID: 314
		private bool _closed;

		// Token: 0x0400013B RID: 315
		private bool _findRequired;

		// Token: 0x0400013C RID: 316
		private bool _exceptionPending;
	}
}
