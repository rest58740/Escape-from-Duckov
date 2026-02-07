using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Tar
{
	// Token: 0x02000046 RID: 70
	public class TarOutputStream : Stream
	{
		// Token: 0x06000330 RID: 816 RVA: 0x0001428C File Offset: 0x0001248C
		public TarOutputStream(Stream outputStream) : this(outputStream, 20)
		{
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00014298 File Offset: 0x00012498
		public TarOutputStream(Stream outputStream, int blockFactor)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			this.outputStream = outputStream;
			this.buffer = TarBuffer.CreateOutputTarBuffer(outputStream, blockFactor);
			this.assemblyBuffer = new byte[512];
			this.blockBuffer = new byte[512];
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000332 RID: 818 RVA: 0x000142F0 File Offset: 0x000124F0
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00014300 File Offset: 0x00012500
		public bool IsStreamOwner
		{
			get
			{
				return this.buffer.IsStreamOwner;
			}
			set
			{
				this.buffer.IsStreamOwner = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00014310 File Offset: 0x00012510
		public override bool CanRead
		{
			get
			{
				return this.outputStream.CanRead;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00014320 File Offset: 0x00012520
		public override bool CanSeek
		{
			get
			{
				return this.outputStream.CanSeek;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00014330 File Offset: 0x00012530
		public override bool CanWrite
		{
			get
			{
				return this.outputStream.CanWrite;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00014340 File Offset: 0x00012540
		public override long Length
		{
			get
			{
				return this.outputStream.Length;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00014350 File Offset: 0x00012550
		// (set) Token: 0x06000339 RID: 825 RVA: 0x00014360 File Offset: 0x00012560
		public override long Position
		{
			get
			{
				return this.outputStream.Position;
			}
			set
			{
				this.outputStream.Position = value;
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00014370 File Offset: 0x00012570
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.outputStream.Seek(offset, origin);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00014380 File Offset: 0x00012580
		public override void SetLength(long value)
		{
			this.outputStream.SetLength(value);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00014390 File Offset: 0x00012590
		public override int ReadByte()
		{
			return this.outputStream.ReadByte();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000143A0 File Offset: 0x000125A0
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.outputStream.Read(buffer, offset, count);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000143B0 File Offset: 0x000125B0
		public override void Flush()
		{
			this.outputStream.Flush();
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000143C0 File Offset: 0x000125C0
		public void Finish()
		{
			if (this.IsEntryOpen)
			{
				this.CloseEntry();
			}
			this.WriteEofBlock();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000143DC File Offset: 0x000125DC
		public override void Close()
		{
			if (!this.isClosed)
			{
				this.isClosed = true;
				this.Finish();
				this.buffer.Close();
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00014404 File Offset: 0x00012604
		public int RecordSize
		{
			get
			{
				return this.buffer.RecordSize;
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00014414 File Offset: 0x00012614
		[Obsolete("Use RecordSize property instead")]
		public int GetRecordSize()
		{
			return this.buffer.RecordSize;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00014424 File Offset: 0x00012624
		private bool IsEntryOpen
		{
			get
			{
				return this.currBytes < this.currSize;
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00014434 File Offset: 0x00012634
		public void PutNextEntry(TarEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (entry.TarHeader.Name.Length >= 100)
			{
				TarHeader tarHeader = new TarHeader();
				tarHeader.TypeFlag = 76;
				tarHeader.Name += "././@LongLink";
				tarHeader.UserId = 0;
				tarHeader.GroupId = 0;
				tarHeader.GroupName = string.Empty;
				tarHeader.UserName = string.Empty;
				tarHeader.LinkName = string.Empty;
				tarHeader.Size = (long)(entry.TarHeader.Name.Length + 1);
				tarHeader.WriteHeader(this.blockBuffer);
				this.buffer.WriteBlock(this.blockBuffer);
				int i = 0;
				while (i < entry.TarHeader.Name.Length)
				{
					Array.Clear(this.blockBuffer, 0, this.blockBuffer.Length);
					TarHeader.GetAsciiBytes(entry.TarHeader.Name, i, this.blockBuffer, 0, 512);
					i += 512;
					this.buffer.WriteBlock(this.blockBuffer);
				}
			}
			entry.WriteEntryHeader(this.blockBuffer);
			this.buffer.WriteBlock(this.blockBuffer);
			this.currBytes = 0L;
			this.currSize = ((!entry.IsDirectory) ? entry.Size : 0L);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000145A0 File Offset: 0x000127A0
		public void CloseEntry()
		{
			if (this.assemblyBufferLength > 0)
			{
				Array.Clear(this.assemblyBuffer, this.assemblyBufferLength, this.assemblyBuffer.Length - this.assemblyBufferLength);
				this.buffer.WriteBlock(this.assemblyBuffer);
				this.currBytes += (long)this.assemblyBufferLength;
				this.assemblyBufferLength = 0;
			}
			if (this.currBytes < this.currSize)
			{
				string message = string.Format("Entry closed at '{0}' before the '{1}' bytes specified in the header were written", this.currBytes, this.currSize);
				throw new TarException(message);
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00014640 File Offset: 0x00012840
		public override void WriteByte(byte value)
		{
			this.Write(new byte[]
			{
				value
			}, 0, 1);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00014654 File Offset: 0x00012854
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Cannot be negative");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("offset and count combination is invalid");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Cannot be negative");
			}
			if (this.currBytes + (long)count > this.currSize)
			{
				string message = string.Format("request to write '{0}' bytes exceeds size in header of '{1}' bytes", count, this.currSize);
				throw new ArgumentOutOfRangeException("count", message);
			}
			if (this.assemblyBufferLength > 0)
			{
				if (this.assemblyBufferLength + count >= this.blockBuffer.Length)
				{
					int num = this.blockBuffer.Length - this.assemblyBufferLength;
					Array.Copy(this.assemblyBuffer, 0, this.blockBuffer, 0, this.assemblyBufferLength);
					Array.Copy(buffer, offset, this.blockBuffer, this.assemblyBufferLength, num);
					this.buffer.WriteBlock(this.blockBuffer);
					this.currBytes += (long)this.blockBuffer.Length;
					offset += num;
					count -= num;
					this.assemblyBufferLength = 0;
				}
				else
				{
					Array.Copy(buffer, offset, this.assemblyBuffer, this.assemblyBufferLength, count);
					offset += count;
					this.assemblyBufferLength += count;
					count -= count;
				}
			}
			while (count > 0)
			{
				if (count < this.blockBuffer.Length)
				{
					Array.Copy(buffer, offset, this.assemblyBuffer, this.assemblyBufferLength, count);
					this.assemblyBufferLength += count;
					break;
				}
				this.buffer.WriteBlock(buffer, offset);
				int num2 = this.blockBuffer.Length;
				this.currBytes += (long)num2;
				count -= num2;
				offset += num2;
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0001482C File Offset: 0x00012A2C
		private void WriteEofBlock()
		{
			Array.Clear(this.blockBuffer, 0, this.blockBuffer.Length);
			this.buffer.WriteBlock(this.blockBuffer);
		}

		// Token: 0x04000282 RID: 642
		private long currBytes;

		// Token: 0x04000283 RID: 643
		private int assemblyBufferLength;

		// Token: 0x04000284 RID: 644
		private bool isClosed;

		// Token: 0x04000285 RID: 645
		protected long currSize;

		// Token: 0x04000286 RID: 646
		protected byte[] blockBuffer;

		// Token: 0x04000287 RID: 647
		protected byte[] assemblyBuffer;

		// Token: 0x04000288 RID: 648
		protected TarBuffer buffer;

		// Token: 0x04000289 RID: 649
		protected Stream outputStream;
	}
}
