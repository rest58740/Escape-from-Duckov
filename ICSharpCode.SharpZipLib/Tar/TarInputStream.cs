using System;
using System.IO;
using System.Text;

namespace ICSharpCode.SharpZipLib.Tar
{
	// Token: 0x02000043 RID: 67
	public class TarInputStream : Stream
	{
		// Token: 0x0600030C RID: 780 RVA: 0x00013BEC File Offset: 0x00011DEC
		public TarInputStream(Stream inputStream) : this(inputStream, 20)
		{
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00013BF8 File Offset: 0x00011DF8
		public TarInputStream(Stream inputStream, int blockFactor)
		{
			this.inputStream = inputStream;
			this.tarBuffer = TarBuffer.CreateInputTarBuffer(inputStream, blockFactor);
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600030E RID: 782 RVA: 0x00013C14 File Offset: 0x00011E14
		// (set) Token: 0x0600030F RID: 783 RVA: 0x00013C24 File Offset: 0x00011E24
		public bool IsStreamOwner
		{
			get
			{
				return this.tarBuffer.IsStreamOwner;
			}
			set
			{
				this.tarBuffer.IsStreamOwner = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000310 RID: 784 RVA: 0x00013C34 File Offset: 0x00011E34
		public override bool CanRead
		{
			get
			{
				return this.inputStream.CanRead;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00013C44 File Offset: 0x00011E44
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00013C48 File Offset: 0x00011E48
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00013C4C File Offset: 0x00011E4C
		public override long Length
		{
			get
			{
				return this.inputStream.Length;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000314 RID: 788 RVA: 0x00013C5C File Offset: 0x00011E5C
		// (set) Token: 0x06000315 RID: 789 RVA: 0x00013C6C File Offset: 0x00011E6C
		public override long Position
		{
			get
			{
				return this.inputStream.Position;
			}
			set
			{
				throw new NotSupportedException("TarInputStream Seek not supported");
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00013C78 File Offset: 0x00011E78
		public override void Flush()
		{
			this.inputStream.Flush();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00013C88 File Offset: 0x00011E88
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("TarInputStream Seek not supported");
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00013C94 File Offset: 0x00011E94
		public override void SetLength(long value)
		{
			throw new NotSupportedException("TarInputStream SetLength not supported");
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00013CA0 File Offset: 0x00011EA0
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("TarInputStream Write not supported");
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00013CAC File Offset: 0x00011EAC
		public override void WriteByte(byte value)
		{
			throw new NotSupportedException("TarInputStream WriteByte not supported");
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00013CB8 File Offset: 0x00011EB8
		public override int ReadByte()
		{
			byte[] array = new byte[1];
			int num = this.Read(array, 0, 1);
			if (num <= 0)
			{
				return -1;
			}
			return (int)array[0];
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00013CE4 File Offset: 0x00011EE4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = 0;
			if (this.entryOffset >= this.entrySize)
			{
				return 0;
			}
			long num2 = (long)count;
			if (num2 + this.entryOffset > this.entrySize)
			{
				num2 = this.entrySize - this.entryOffset;
			}
			if (this.readBuffer != null)
			{
				int num3 = (num2 <= (long)this.readBuffer.Length) ? ((int)num2) : this.readBuffer.Length;
				Array.Copy(this.readBuffer, 0, buffer, offset, num3);
				if (num3 >= this.readBuffer.Length)
				{
					this.readBuffer = null;
				}
				else
				{
					int num4 = this.readBuffer.Length - num3;
					byte[] destinationArray = new byte[num4];
					Array.Copy(this.readBuffer, num3, destinationArray, 0, num4);
					this.readBuffer = destinationArray;
				}
				num += num3;
				num2 -= (long)num3;
				offset += num3;
			}
			while (num2 > 0L)
			{
				byte[] array = this.tarBuffer.ReadBlock();
				if (array == null)
				{
					throw new TarException("unexpected EOF with " + num2 + " bytes unread");
				}
				int num5 = (int)num2;
				int num6 = array.Length;
				if (num6 > num5)
				{
					Array.Copy(array, 0, buffer, offset, num5);
					this.readBuffer = new byte[num6 - num5];
					Array.Copy(array, num5, this.readBuffer, 0, num6 - num5);
				}
				else
				{
					num5 = num6;
					Array.Copy(array, 0, buffer, offset, num6);
				}
				num += num5;
				num2 -= (long)num5;
				offset += num5;
			}
			this.entryOffset += (long)num;
			return num;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00013E80 File Offset: 0x00012080
		public override void Close()
		{
			this.tarBuffer.Close();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00013E90 File Offset: 0x00012090
		public void SetEntryFactory(TarInputStream.IEntryFactory factory)
		{
			this.entryFactory = factory;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00013E9C File Offset: 0x0001209C
		public int RecordSize
		{
			get
			{
				return this.tarBuffer.RecordSize;
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00013EAC File Offset: 0x000120AC
		[Obsolete("Use RecordSize property instead")]
		public int GetRecordSize()
		{
			return this.tarBuffer.RecordSize;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00013EBC File Offset: 0x000120BC
		public long Available
		{
			get
			{
				return this.entrySize - this.entryOffset;
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00013ECC File Offset: 0x000120CC
		public void Skip(long skipCount)
		{
			byte[] array = new byte[8192];
			int num2;
			for (long num = skipCount; num > 0L; num -= (long)num2)
			{
				int count = (num <= (long)array.Length) ? ((int)num) : array.Length;
				num2 = this.Read(array, 0, count);
				if (num2 == -1)
				{
					break;
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00013F24 File Offset: 0x00012124
		public bool IsMarkSupported
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00013F28 File Offset: 0x00012128
		public void Mark(int markLimit)
		{
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00013F2C File Offset: 0x0001212C
		public void Reset()
		{
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00013F30 File Offset: 0x00012130
		public TarEntry GetNextEntry()
		{
			if (this.hasHitEOF)
			{
				return null;
			}
			if (this.currentEntry != null)
			{
				this.SkipToNextEntry();
			}
			byte[] array = this.tarBuffer.ReadBlock();
			if (array == null)
			{
				this.hasHitEOF = true;
			}
			else if (TarBuffer.IsEndOfArchiveBlock(array))
			{
				this.hasHitEOF = true;
			}
			if (this.hasHitEOF)
			{
				this.currentEntry = null;
			}
			else
			{
				try
				{
					TarHeader tarHeader = new TarHeader();
					tarHeader.ParseBuffer(array);
					if (!tarHeader.IsChecksumValid)
					{
						throw new TarException("Header checksum is invalid");
					}
					this.entryOffset = 0L;
					this.entrySize = tarHeader.Size;
					StringBuilder stringBuilder = null;
					if (tarHeader.TypeFlag == 76)
					{
						byte[] array2 = new byte[512];
						long num = this.entrySize;
						stringBuilder = new StringBuilder();
						while (num > 0L)
						{
							int num2 = this.Read(array2, 0, (num <= (long)array2.Length) ? ((int)num) : array2.Length);
							if (num2 == -1)
							{
								throw new InvalidHeaderException("Failed to read long name entry");
							}
							stringBuilder.Append(TarHeader.ParseName(array2, 0, num2).ToString());
							num -= (long)num2;
						}
						this.SkipToNextEntry();
						array = this.tarBuffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag == 103)
					{
						this.SkipToNextEntry();
						array = this.tarBuffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag == 120)
					{
						this.SkipToNextEntry();
						array = this.tarBuffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag == 86)
					{
						this.SkipToNextEntry();
						array = this.tarBuffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag != 48 && tarHeader.TypeFlag != 0 && tarHeader.TypeFlag != 53)
					{
						this.SkipToNextEntry();
						array = this.tarBuffer.ReadBlock();
					}
					if (this.entryFactory == null)
					{
						this.currentEntry = new TarEntry(array);
						if (stringBuilder != null)
						{
							this.currentEntry.Name = stringBuilder.ToString();
						}
					}
					else
					{
						this.currentEntry = this.entryFactory.CreateEntry(array);
					}
					this.entryOffset = 0L;
					this.entrySize = this.currentEntry.Size;
				}
				catch (InvalidHeaderException ex)
				{
					this.entrySize = 0L;
					this.entryOffset = 0L;
					this.currentEntry = null;
					string message = string.Format("Bad header in record {0} block {1} {2}", this.tarBuffer.CurrentRecord, this.tarBuffer.CurrentBlock, ex.Message);
					throw new InvalidHeaderException(message);
				}
			}
			return this.currentEntry;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000141F8 File Offset: 0x000123F8
		public void CopyEntryContents(Stream outputStream)
		{
			byte[] array = new byte[32768];
			for (;;)
			{
				int num = this.Read(array, 0, array.Length);
				if (num <= 0)
				{
					break;
				}
				outputStream.Write(array, 0, num);
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00014238 File Offset: 0x00012438
		private void SkipToNextEntry()
		{
			long num = this.entrySize - this.entryOffset;
			if (num > 0L)
			{
				this.Skip(num);
			}
			this.readBuffer = null;
		}

		// Token: 0x0400027A RID: 634
		protected bool hasHitEOF;

		// Token: 0x0400027B RID: 635
		protected long entrySize;

		// Token: 0x0400027C RID: 636
		protected long entryOffset;

		// Token: 0x0400027D RID: 637
		protected byte[] readBuffer;

		// Token: 0x0400027E RID: 638
		protected TarBuffer tarBuffer;

		// Token: 0x0400027F RID: 639
		private TarEntry currentEntry;

		// Token: 0x04000280 RID: 640
		protected TarInputStream.IEntryFactory entryFactory;

		// Token: 0x04000281 RID: 641
		private readonly Stream inputStream;

		// Token: 0x02000044 RID: 68
		public interface IEntryFactory
		{
			// Token: 0x06000329 RID: 809
			TarEntry CreateEntry(string name);

			// Token: 0x0600032A RID: 810
			TarEntry CreateEntryFromFile(string fileName);

			// Token: 0x0600032B RID: 811
			TarEntry CreateEntry(byte[] headerBuffer);
		}

		// Token: 0x02000045 RID: 69
		public class EntryFactoryAdapter : TarInputStream.IEntryFactory
		{
			// Token: 0x0600032D RID: 813 RVA: 0x00014274 File Offset: 0x00012474
			public TarEntry CreateEntry(string name)
			{
				return TarEntry.CreateTarEntry(name);
			}

			// Token: 0x0600032E RID: 814 RVA: 0x0001427C File Offset: 0x0001247C
			public TarEntry CreateEntryFromFile(string fileName)
			{
				return TarEntry.CreateEntryFromFile(fileName);
			}

			// Token: 0x0600032F RID: 815 RVA: 0x00014284 File Offset: 0x00012484
			public TarEntry CreateEntry(byte[] headerBuffer)
			{
				return new TarEntry(headerBuffer);
			}
		}
	}
}
