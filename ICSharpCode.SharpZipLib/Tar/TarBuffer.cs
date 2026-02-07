using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Tar
{
	// Token: 0x02000042 RID: 66
	public class TarBuffer
	{
		// Token: 0x060002F2 RID: 754 RVA: 0x00013580 File Offset: 0x00011780
		protected TarBuffer()
		{
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x000135B0 File Offset: 0x000117B0
		public int RecordSize
		{
			get
			{
				return this.recordSize;
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000135B8 File Offset: 0x000117B8
		[Obsolete("Use RecordSize property instead")]
		public int GetRecordSize()
		{
			return this.recordSize;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x000135C0 File Offset: 0x000117C0
		public int BlockFactor
		{
			get
			{
				return this.blockFactor;
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000135C8 File Offset: 0x000117C8
		[Obsolete("Use BlockFactor property instead")]
		public int GetBlockFactor()
		{
			return this.blockFactor;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000135D0 File Offset: 0x000117D0
		public static TarBuffer CreateInputTarBuffer(Stream inputStream)
		{
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			return TarBuffer.CreateInputTarBuffer(inputStream, 20);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x000135EC File Offset: 0x000117EC
		public static TarBuffer CreateInputTarBuffer(Stream inputStream, int blockFactor)
		{
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (blockFactor <= 0)
			{
				throw new ArgumentOutOfRangeException("blockFactor", "Factor cannot be negative");
			}
			TarBuffer tarBuffer = new TarBuffer();
			tarBuffer.inputStream = inputStream;
			tarBuffer.outputStream = null;
			tarBuffer.Initialize(blockFactor);
			return tarBuffer;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00013640 File Offset: 0x00011840
		public static TarBuffer CreateOutputTarBuffer(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			return TarBuffer.CreateOutputTarBuffer(outputStream, 20);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0001365C File Offset: 0x0001185C
		public static TarBuffer CreateOutputTarBuffer(Stream outputStream, int blockFactor)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (blockFactor <= 0)
			{
				throw new ArgumentOutOfRangeException("blockFactor", "Factor cannot be negative");
			}
			TarBuffer tarBuffer = new TarBuffer();
			tarBuffer.inputStream = null;
			tarBuffer.outputStream = outputStream;
			tarBuffer.Initialize(blockFactor);
			return tarBuffer;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000136B0 File Offset: 0x000118B0
		private void Initialize(int archiveBlockFactor)
		{
			this.blockFactor = archiveBlockFactor;
			this.recordSize = archiveBlockFactor * 512;
			this.recordBuffer = new byte[this.RecordSize];
			if (this.inputStream != null)
			{
				this.currentRecordIndex = -1;
				this.currentBlockIndex = this.BlockFactor;
			}
			else
			{
				this.currentRecordIndex = 0;
				this.currentBlockIndex = 0;
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00013714 File Offset: 0x00011914
		[Obsolete("Use IsEndOfArchiveBlock instead")]
		public bool IsEOFBlock(byte[] block)
		{
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			if (block.Length != 512)
			{
				throw new ArgumentException("block length is invalid");
			}
			for (int i = 0; i < 512; i++)
			{
				if (block[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0001376C File Offset: 0x0001196C
		public static bool IsEndOfArchiveBlock(byte[] block)
		{
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			if (block.Length != 512)
			{
				throw new ArgumentException("block length is invalid");
			}
			for (int i = 0; i < 512; i++)
			{
				if (block[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000137C4 File Offset: 0x000119C4
		public void SkipBlock()
		{
			if (this.inputStream == null)
			{
				throw new TarException("no input stream defined");
			}
			if (this.currentBlockIndex >= this.BlockFactor && !this.ReadRecord())
			{
				throw new TarException("Failed to read a record");
			}
			this.currentBlockIndex++;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0001381C File Offset: 0x00011A1C
		public byte[] ReadBlock()
		{
			if (this.inputStream == null)
			{
				throw new TarException("TarBuffer.ReadBlock - no input stream defined");
			}
			if (this.currentBlockIndex >= this.BlockFactor && !this.ReadRecord())
			{
				throw new TarException("Failed to read a record");
			}
			byte[] array = new byte[512];
			Array.Copy(this.recordBuffer, this.currentBlockIndex * 512, array, 0, 512);
			this.currentBlockIndex++;
			return array;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000138A0 File Offset: 0x00011AA0
		private bool ReadRecord()
		{
			if (this.inputStream == null)
			{
				throw new TarException("no input stream stream defined");
			}
			this.currentBlockIndex = 0;
			int num = 0;
			long num2;
			for (int i = this.RecordSize; i > 0; i -= (int)num2)
			{
				num2 = (long)this.inputStream.Read(this.recordBuffer, num, i);
				if (num2 <= 0L)
				{
					break;
				}
				num += (int)num2;
			}
			this.currentRecordIndex++;
			return true;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0001391C File Offset: 0x00011B1C
		public int CurrentBlock
		{
			get
			{
				return this.currentBlockIndex;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000302 RID: 770 RVA: 0x00013924 File Offset: 0x00011B24
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0001392C File Offset: 0x00011B2C
		public bool IsStreamOwner
		{
			get
			{
				return this.isStreamOwner_;
			}
			set
			{
				this.isStreamOwner_ = value;
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00013938 File Offset: 0x00011B38
		[Obsolete("Use CurrentBlock property instead")]
		public int GetCurrentBlockNum()
		{
			return this.currentBlockIndex;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00013940 File Offset: 0x00011B40
		public int CurrentRecord
		{
			get
			{
				return this.currentRecordIndex;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00013948 File Offset: 0x00011B48
		[Obsolete("Use CurrentRecord property instead")]
		public int GetCurrentRecordNum()
		{
			return this.currentRecordIndex;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00013950 File Offset: 0x00011B50
		public void WriteBlock(byte[] block)
		{
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			if (this.outputStream == null)
			{
				throw new TarException("TarBuffer.WriteBlock - no output stream defined");
			}
			if (block.Length != 512)
			{
				string message = string.Format("TarBuffer.WriteBlock - block to write has length '{0}' which is not the block size of '{1}'", block.Length, 512);
				throw new TarException(message);
			}
			if (this.currentBlockIndex >= this.BlockFactor)
			{
				this.WriteRecord();
			}
			Array.Copy(block, 0, this.recordBuffer, this.currentBlockIndex * 512, 512);
			this.currentBlockIndex++;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000139F8 File Offset: 0x00011BF8
		public void WriteBlock(byte[] buffer, int offset)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (this.outputStream == null)
			{
				throw new TarException("TarBuffer.WriteBlock - no output stream stream defined");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + 512 > buffer.Length)
			{
				string message = string.Format("TarBuffer.WriteBlock - record has length '{0}' with offset '{1}' which is less than the record size of '{2}'", buffer.Length, offset, this.recordSize);
				throw new TarException(message);
			}
			if (this.currentBlockIndex >= this.BlockFactor)
			{
				this.WriteRecord();
			}
			Array.Copy(buffer, offset, this.recordBuffer, this.currentBlockIndex * 512, 512);
			this.currentBlockIndex++;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00013AC4 File Offset: 0x00011CC4
		private void WriteRecord()
		{
			if (this.outputStream == null)
			{
				throw new TarException("TarBuffer.WriteRecord no output stream defined");
			}
			this.outputStream.Write(this.recordBuffer, 0, this.RecordSize);
			this.outputStream.Flush();
			this.currentBlockIndex = 0;
			this.currentRecordIndex++;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00013B20 File Offset: 0x00011D20
		private void WriteFinalRecord()
		{
			if (this.outputStream == null)
			{
				throw new TarException("TarBuffer.WriteFinalRecord no output stream defined");
			}
			if (this.currentBlockIndex > 0)
			{
				int num = this.currentBlockIndex * 512;
				Array.Clear(this.recordBuffer, num, this.RecordSize - num);
				this.WriteRecord();
			}
			this.outputStream.Flush();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00013B84 File Offset: 0x00011D84
		public void Close()
		{
			if (this.outputStream != null)
			{
				this.WriteFinalRecord();
				if (this.isStreamOwner_)
				{
					this.outputStream.Close();
				}
				this.outputStream = null;
			}
			else if (this.inputStream != null)
			{
				if (this.isStreamOwner_)
				{
					this.inputStream.Close();
				}
				this.inputStream = null;
			}
		}

		// Token: 0x0400026F RID: 623
		public const int BlockSize = 512;

		// Token: 0x04000270 RID: 624
		public const int DefaultBlockFactor = 20;

		// Token: 0x04000271 RID: 625
		public const int DefaultRecordSize = 10240;

		// Token: 0x04000272 RID: 626
		private Stream inputStream;

		// Token: 0x04000273 RID: 627
		private Stream outputStream;

		// Token: 0x04000274 RID: 628
		private byte[] recordBuffer;

		// Token: 0x04000275 RID: 629
		private int currentBlockIndex;

		// Token: 0x04000276 RID: 630
		private int currentRecordIndex;

		// Token: 0x04000277 RID: 631
		private int recordSize = 10240;

		// Token: 0x04000278 RID: 632
		private int blockFactor = 20;

		// Token: 0x04000279 RID: 633
		private bool isStreamOwner_ = true;
	}
}
