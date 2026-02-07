using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;

namespace ICSharpCode.SharpZipLib.BZip2
{
	// Token: 0x0200003B RID: 59
	public class BZip2InputStream : Stream
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000EC60 File Offset: 0x0000CE60
		public BZip2InputStream(Stream stream)
		{
			for (int i = 0; i < 6; i++)
			{
				this.limit[i] = new int[258];
				this.baseArray[i] = new int[258];
				this.perm[i] = new int[258];
			}
			this.BsSetStream(stream);
			this.Initialize();
			this.InitBlock();
			this.SetupBlock();
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000ED84 File Offset: 0x0000CF84
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000ED8C File Offset: 0x0000CF8C
		public bool IsStreamOwner
		{
			get
			{
				return this.isStreamOwner;
			}
			set
			{
				this.isStreamOwner = value;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000ED98 File Offset: 0x0000CF98
		public override bool CanRead
		{
			get
			{
				return this.baseStream.CanRead;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
		public override bool CanSeek
		{
			get
			{
				return this.baseStream.CanSeek;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000EDBC File Offset: 0x0000CFBC
		public override long Length
		{
			get
			{
				return this.baseStream.Length;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000EDCC File Offset: 0x0000CFCC
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000EDDC File Offset: 0x0000CFDC
		public override long Position
		{
			get
			{
				return this.baseStream.Position;
			}
			set
			{
				throw new NotSupportedException("BZip2InputStream position cannot be set");
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000EDE8 File Offset: 0x0000CFE8
		public override void Flush()
		{
			if (this.baseStream != null)
			{
				this.baseStream.Flush();
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000EE00 File Offset: 0x0000D000
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("BZip2InputStream Seek not supported");
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000EE0C File Offset: 0x0000D00C
		public override void SetLength(long value)
		{
			throw new NotSupportedException("BZip2InputStream SetLength not supported");
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000EE18 File Offset: 0x0000D018
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("BZip2InputStream Write not supported");
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000EE24 File Offset: 0x0000D024
		public override void WriteByte(byte value)
		{
			throw new NotSupportedException("BZip2InputStream WriteByte not supported");
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000EE30 File Offset: 0x0000D030
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			for (int i = 0; i < count; i++)
			{
				int num = this.ReadByte();
				if (num == -1)
				{
					return i;
				}
				buffer[offset + i] = (byte)num;
			}
			return count;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000EE78 File Offset: 0x0000D078
		public override void Close()
		{
			if (this.IsStreamOwner && this.baseStream != null)
			{
				this.baseStream.Close();
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000EE9C File Offset: 0x0000D09C
		public override int ReadByte()
		{
			if (this.streamEnd)
			{
				return -1;
			}
			int result = this.currentChar;
			switch (this.currentState)
			{
			case 3:
				this.SetupRandPartB();
				break;
			case 4:
				this.SetupRandPartC();
				break;
			case 6:
				this.SetupNoRandPartB();
				break;
			case 7:
				this.SetupNoRandPartC();
				break;
			}
			return result;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000EF24 File Offset: 0x0000D124
		private void MakeMaps()
		{
			this.nInUse = 0;
			for (int i = 0; i < 256; i++)
			{
				if (this.inUse[i])
				{
					this.seqToUnseq[this.nInUse] = (byte)i;
					this.unseqToSeq[i] = (byte)this.nInUse;
					this.nInUse++;
				}
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000EF88 File Offset: 0x0000D188
		private void Initialize()
		{
			char c = this.BsGetUChar();
			char c2 = this.BsGetUChar();
			char c3 = this.BsGetUChar();
			char c4 = this.BsGetUChar();
			if (c != 'B' || c2 != 'Z' || c3 != 'h' || c4 < '1' || c4 > '9')
			{
				this.streamEnd = true;
				return;
			}
			this.SetDecompressStructureSizes((int)(c4 - '0'));
			this.computedCombinedCRC = 0U;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000EFF4 File Offset: 0x0000D1F4
		private void InitBlock()
		{
			char c = this.BsGetUChar();
			char c2 = this.BsGetUChar();
			char c3 = this.BsGetUChar();
			char c4 = this.BsGetUChar();
			char c5 = this.BsGetUChar();
			char c6 = this.BsGetUChar();
			if (c == '\u0017' && c2 == 'r' && c3 == 'E' && c4 == '8' && c5 == 'P' && c6 == '\u0090')
			{
				this.Complete();
				return;
			}
			if (c != '1' || c2 != 'A' || c3 != 'Y' || c4 != '&' || c5 != 'S' || c6 != 'Y')
			{
				BZip2InputStream.BadBlockHeader();
				this.streamEnd = true;
				return;
			}
			this.storedBlockCRC = this.BsGetInt32();
			this.blockRandomised = (this.BsR(1) == 1);
			this.GetAndMoveToFrontDecode();
			this.mCrc.Reset();
			this.currentState = 1;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000F0DC File Offset: 0x0000D2DC
		private void EndBlock()
		{
			this.computedBlockCRC = (int)this.mCrc.Value;
			if (this.storedBlockCRC != this.computedBlockCRC)
			{
				BZip2InputStream.CrcError();
			}
			this.computedCombinedCRC = ((this.computedCombinedCRC << 1 & uint.MaxValue) | this.computedCombinedCRC >> 31);
			this.computedCombinedCRC ^= (uint)this.computedBlockCRC;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000F140 File Offset: 0x0000D340
		private void Complete()
		{
			this.storedCombinedCRC = this.BsGetInt32();
			if (this.storedCombinedCRC != (int)this.computedCombinedCRC)
			{
				BZip2InputStream.CrcError();
			}
			this.streamEnd = true;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000F16C File Offset: 0x0000D36C
		private void BsSetStream(Stream stream)
		{
			this.baseStream = stream;
			this.bsLive = 0;
			this.bsBuff = 0;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000F184 File Offset: 0x0000D384
		private void FillBuffer()
		{
			int num = 0;
			try
			{
				num = this.baseStream.ReadByte();
			}
			catch (Exception)
			{
				BZip2InputStream.CompressedStreamEOF();
			}
			if (num == -1)
			{
				BZip2InputStream.CompressedStreamEOF();
			}
			this.bsBuff = (this.bsBuff << 8 | (num & 255));
			this.bsLive += 8;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000F1FC File Offset: 0x0000D3FC
		private int BsR(int n)
		{
			while (this.bsLive < n)
			{
				this.FillBuffer();
			}
			int result = this.bsBuff >> this.bsLive - n & (1 << n) - 1;
			this.bsLive -= n;
			return result;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000F24C File Offset: 0x0000D44C
		private char BsGetUChar()
		{
			return (char)this.BsR(8);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000F258 File Offset: 0x0000D458
		private int BsGetIntVS(int numBits)
		{
			return this.BsR(numBits);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000F264 File Offset: 0x0000D464
		private int BsGetInt32()
		{
			int num = this.BsR(8);
			num = (num << 8 | this.BsR(8));
			num = (num << 8 | this.BsR(8));
			return num << 8 | this.BsR(8);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000F2A0 File Offset: 0x0000D4A0
		private void RecvDecodingTables()
		{
			char[][] array = new char[6][];
			for (int i = 0; i < 6; i++)
			{
				array[i] = new char[258];
			}
			bool[] array2 = new bool[16];
			for (int j = 0; j < 16; j++)
			{
				array2[j] = (this.BsR(1) == 1);
			}
			for (int k = 0; k < 16; k++)
			{
				if (array2[k])
				{
					for (int l = 0; l < 16; l++)
					{
						this.inUse[k * 16 + l] = (this.BsR(1) == 1);
					}
				}
				else
				{
					for (int m = 0; m < 16; m++)
					{
						this.inUse[k * 16 + m] = false;
					}
				}
			}
			this.MakeMaps();
			int num = this.nInUse + 2;
			int num2 = this.BsR(3);
			int num3 = this.BsR(15);
			for (int n = 0; n < num3; n++)
			{
				int num4 = 0;
				while (this.BsR(1) == 1)
				{
					num4++;
				}
				this.selectorMtf[n] = (byte)num4;
			}
			byte[] array3 = new byte[6];
			for (int num5 = 0; num5 < num2; num5++)
			{
				array3[num5] = (byte)num5;
			}
			for (int num6 = 0; num6 < num3; num6++)
			{
				int num7 = (int)this.selectorMtf[num6];
				byte b = array3[num7];
				while (num7 > 0)
				{
					array3[num7] = array3[num7 - 1];
					num7--;
				}
				array3[0] = b;
				this.selector[num6] = b;
			}
			for (int num8 = 0; num8 < num2; num8++)
			{
				int num9 = this.BsR(5);
				for (int num10 = 0; num10 < num; num10++)
				{
					while (this.BsR(1) == 1)
					{
						if (this.BsR(1) == 0)
						{
							num9++;
						}
						else
						{
							num9--;
						}
					}
					array[num8][num10] = (char)num9;
				}
			}
			for (int num11 = 0; num11 < num2; num11++)
			{
				int num12 = 32;
				int num13 = 0;
				for (int num14 = 0; num14 < num; num14++)
				{
					num13 = Math.Max(num13, (int)array[num11][num14]);
					num12 = Math.Min(num12, (int)array[num11][num14]);
				}
				BZip2InputStream.HbCreateDecodeTables(this.limit[num11], this.baseArray[num11], this.perm[num11], array[num11], num12, num13, num);
				this.minLens[num11] = num12;
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000F554 File Offset: 0x0000D754
		private void GetAndMoveToFrontDecode()
		{
			byte[] array = new byte[256];
			int num = 100000 * this.blockSize100k;
			this.origPtr = this.BsGetIntVS(24);
			this.RecvDecodingTables();
			int num2 = this.nInUse + 1;
			int num3 = -1;
			int num4 = 0;
			for (int i = 0; i <= 255; i++)
			{
				this.unzftab[i] = 0;
			}
			for (int j = 0; j <= 255; j++)
			{
				array[j] = (byte)j;
			}
			this.last = -1;
			if (num4 == 0)
			{
				num3++;
				num4 = 50;
			}
			num4--;
			int num5 = (int)this.selector[num3];
			int num6 = this.minLens[num5];
			int k;
			int num7;
			for (k = this.BsR(num6); k > this.limit[num5][num6]; k = (k << 1 | num7))
			{
				if (num6 > 20)
				{
					throw new BZip2Exception("Bzip data error");
				}
				num6++;
				while (this.bsLive < 1)
				{
					this.FillBuffer();
				}
				num7 = (this.bsBuff >> this.bsLive - 1 & 1);
				this.bsLive--;
			}
			if (k - this.baseArray[num5][num6] < 0 || k - this.baseArray[num5][num6] >= 258)
			{
				throw new BZip2Exception("Bzip data error");
			}
			int num8 = this.perm[num5][k - this.baseArray[num5][num6]];
			while (num8 != num2)
			{
				if (num8 == 0 || num8 == 1)
				{
					int l = -1;
					int num9 = 1;
					do
					{
						if (num8 == 0)
						{
							l += 1 * num9;
						}
						else if (num8 == 1)
						{
							l += 2 * num9;
						}
						num9 <<= 1;
						if (num4 == 0)
						{
							num3++;
							num4 = 50;
						}
						num4--;
						num5 = (int)this.selector[num3];
						num6 = this.minLens[num5];
						for (k = this.BsR(num6); k > this.limit[num5][num6]; k = (k << 1 | num7))
						{
							num6++;
							while (this.bsLive < 1)
							{
								this.FillBuffer();
							}
							num7 = (this.bsBuff >> this.bsLive - 1 & 1);
							this.bsLive--;
						}
						num8 = this.perm[num5][k - this.baseArray[num5][num6]];
					}
					while (num8 == 0 || num8 == 1);
					l++;
					byte b = this.seqToUnseq[(int)array[0]];
					this.unzftab[(int)b] += l;
					while (l > 0)
					{
						this.last++;
						this.ll8[this.last] = b;
						l--;
					}
					if (this.last >= num)
					{
						BZip2InputStream.BlockOverrun();
					}
				}
				else
				{
					this.last++;
					if (this.last >= num)
					{
						BZip2InputStream.BlockOverrun();
					}
					byte b2 = array[num8 - 1];
					this.unzftab[(int)this.seqToUnseq[(int)b2]]++;
					this.ll8[this.last] = this.seqToUnseq[(int)b2];
					for (int m = num8 - 1; m > 0; m--)
					{
						array[m] = array[m - 1];
					}
					array[0] = b2;
					if (num4 == 0)
					{
						num3++;
						num4 = 50;
					}
					num4--;
					num5 = (int)this.selector[num3];
					num6 = this.minLens[num5];
					for (k = this.BsR(num6); k > this.limit[num5][num6]; k = (k << 1 | num7))
					{
						num6++;
						while (this.bsLive < 1)
						{
							this.FillBuffer();
						}
						num7 = (this.bsBuff >> this.bsLive - 1 & 1);
						this.bsLive--;
					}
					num8 = this.perm[num5][k - this.baseArray[num5][num6]];
				}
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000F998 File Offset: 0x0000DB98
		private void SetupBlock()
		{
			int[] array = new int[257];
			array[0] = 0;
			Array.Copy(this.unzftab, 0, array, 1, 256);
			for (int i = 1; i <= 256; i++)
			{
				array[i] += array[i - 1];
			}
			for (int j = 0; j <= this.last; j++)
			{
				byte b = this.ll8[j];
				this.tt[array[(int)b]] = j;
				array[(int)b]++;
			}
			this.tPos = this.tt[this.origPtr];
			this.count = 0;
			this.i2 = 0;
			this.ch2 = 256;
			if (this.blockRandomised)
			{
				this.rNToGo = 0;
				this.rTPos = 0;
				this.SetupRandPartA();
			}
			else
			{
				this.SetupNoRandPartA();
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000FA7C File Offset: 0x0000DC7C
		private void SetupRandPartA()
		{
			if (this.i2 <= this.last)
			{
				this.chPrev = this.ch2;
				this.ch2 = (int)this.ll8[this.tPos];
				this.tPos = this.tt[this.tPos];
				if (this.rNToGo == 0)
				{
					this.rNToGo = BZip2Constants.RandomNumbers[this.rTPos];
					this.rTPos++;
					if (this.rTPos == 512)
					{
						this.rTPos = 0;
					}
				}
				this.rNToGo--;
				this.ch2 ^= ((this.rNToGo != 1) ? 0 : 1);
				this.i2++;
				this.currentChar = this.ch2;
				this.currentState = 3;
				this.mCrc.Update(this.ch2);
			}
			else
			{
				this.EndBlock();
				this.InitBlock();
				this.SetupBlock();
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000FB88 File Offset: 0x0000DD88
		private void SetupNoRandPartA()
		{
			if (this.i2 <= this.last)
			{
				this.chPrev = this.ch2;
				this.ch2 = (int)this.ll8[this.tPos];
				this.tPos = this.tt[this.tPos];
				this.i2++;
				this.currentChar = this.ch2;
				this.currentState = 6;
				this.mCrc.Update(this.ch2);
			}
			else
			{
				this.EndBlock();
				this.InitBlock();
				this.SetupBlock();
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000FC24 File Offset: 0x0000DE24
		private void SetupRandPartB()
		{
			if (this.ch2 != this.chPrev)
			{
				this.currentState = 2;
				this.count = 1;
				this.SetupRandPartA();
			}
			else
			{
				this.count++;
				if (this.count >= 4)
				{
					this.z = this.ll8[this.tPos];
					this.tPos = this.tt[this.tPos];
					if (this.rNToGo == 0)
					{
						this.rNToGo = BZip2Constants.RandomNumbers[this.rTPos];
						this.rTPos++;
						if (this.rTPos == 512)
						{
							this.rTPos = 0;
						}
					}
					this.rNToGo--;
					this.z ^= ((this.rNToGo != 1) ? 0 : 1);
					this.j2 = 0;
					this.currentState = 4;
					this.SetupRandPartC();
				}
				else
				{
					this.currentState = 2;
					this.SetupRandPartA();
				}
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000FD34 File Offset: 0x0000DF34
		private void SetupRandPartC()
		{
			if (this.j2 < (int)this.z)
			{
				this.currentChar = this.ch2;
				this.mCrc.Update(this.ch2);
				this.j2++;
			}
			else
			{
				this.currentState = 2;
				this.i2++;
				this.count = 0;
				this.SetupRandPartA();
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
		private void SetupNoRandPartB()
		{
			if (this.ch2 != this.chPrev)
			{
				this.currentState = 5;
				this.count = 1;
				this.SetupNoRandPartA();
			}
			else
			{
				this.count++;
				if (this.count >= 4)
				{
					this.z = this.ll8[this.tPos];
					this.tPos = this.tt[this.tPos];
					this.currentState = 7;
					this.j2 = 0;
					this.SetupNoRandPartC();
				}
				else
				{
					this.currentState = 5;
					this.SetupNoRandPartA();
				}
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000FE44 File Offset: 0x0000E044
		private void SetupNoRandPartC()
		{
			if (this.j2 < (int)this.z)
			{
				this.currentChar = this.ch2;
				this.mCrc.Update(this.ch2);
				this.j2++;
			}
			else
			{
				this.currentState = 5;
				this.i2++;
				this.count = 0;
				this.SetupNoRandPartA();
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000FEB4 File Offset: 0x0000E0B4
		private void SetDecompressStructureSizes(int newSize100k)
		{
			if (0 > newSize100k || newSize100k > 9 || 0 > this.blockSize100k || this.blockSize100k > 9)
			{
				throw new BZip2Exception("Invalid block size");
			}
			this.blockSize100k = newSize100k;
			if (newSize100k == 0)
			{
				return;
			}
			int num = 100000 * newSize100k;
			this.ll8 = new byte[num];
			this.tt = new int[num];
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000FF24 File Offset: 0x0000E124
		private static void CompressedStreamEOF()
		{
			throw new EndOfStreamException("BZip2 input stream end of compressed stream");
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000FF30 File Offset: 0x0000E130
		private static void BlockOverrun()
		{
			throw new BZip2Exception("BZip2 input stream block overrun");
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000FF3C File Offset: 0x0000E13C
		private static void BadBlockHeader()
		{
			throw new BZip2Exception("BZip2 input stream bad block header");
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000FF48 File Offset: 0x0000E148
		private static void CrcError()
		{
			throw new BZip2Exception("BZip2 input stream crc error");
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000FF54 File Offset: 0x0000E154
		private static void HbCreateDecodeTables(int[] limit, int[] baseArray, int[] perm, char[] length, int minLen, int maxLen, int alphaSize)
		{
			int num = 0;
			for (int i = minLen; i <= maxLen; i++)
			{
				for (int j = 0; j < alphaSize; j++)
				{
					if ((int)length[j] == i)
					{
						perm[num] = j;
						num++;
					}
				}
			}
			for (int k = 0; k < 23; k++)
			{
				baseArray[k] = 0;
			}
			for (int l = 0; l < alphaSize; l++)
			{
				baseArray[(int)(length[l] + '\u0001')]++;
			}
			for (int m = 1; m < 23; m++)
			{
				baseArray[m] += baseArray[m - 1];
			}
			for (int n = 0; n < 23; n++)
			{
				limit[n] = 0;
			}
			int num2 = 0;
			for (int num3 = minLen; num3 <= maxLen; num3++)
			{
				num2 += baseArray[num3 + 1] - baseArray[num3];
				limit[num3] = num2 - 1;
				num2 <<= 1;
			}
			for (int num4 = minLen + 1; num4 <= maxLen; num4++)
			{
				baseArray[num4] = (limit[num4 - 1] + 1 << 1) - baseArray[num4];
			}
		}

		// Token: 0x040001CC RID: 460
		private const int START_BLOCK_STATE = 1;

		// Token: 0x040001CD RID: 461
		private const int RAND_PART_A_STATE = 2;

		// Token: 0x040001CE RID: 462
		private const int RAND_PART_B_STATE = 3;

		// Token: 0x040001CF RID: 463
		private const int RAND_PART_C_STATE = 4;

		// Token: 0x040001D0 RID: 464
		private const int NO_RAND_PART_A_STATE = 5;

		// Token: 0x040001D1 RID: 465
		private const int NO_RAND_PART_B_STATE = 6;

		// Token: 0x040001D2 RID: 466
		private const int NO_RAND_PART_C_STATE = 7;

		// Token: 0x040001D3 RID: 467
		private int last;

		// Token: 0x040001D4 RID: 468
		private int origPtr;

		// Token: 0x040001D5 RID: 469
		private int blockSize100k;

		// Token: 0x040001D6 RID: 470
		private bool blockRandomised;

		// Token: 0x040001D7 RID: 471
		private int bsBuff;

		// Token: 0x040001D8 RID: 472
		private int bsLive;

		// Token: 0x040001D9 RID: 473
		private IChecksum mCrc = new StrangeCRC();

		// Token: 0x040001DA RID: 474
		private bool[] inUse = new bool[256];

		// Token: 0x040001DB RID: 475
		private int nInUse;

		// Token: 0x040001DC RID: 476
		private byte[] seqToUnseq = new byte[256];

		// Token: 0x040001DD RID: 477
		private byte[] unseqToSeq = new byte[256];

		// Token: 0x040001DE RID: 478
		private byte[] selector = new byte[18002];

		// Token: 0x040001DF RID: 479
		private byte[] selectorMtf = new byte[18002];

		// Token: 0x040001E0 RID: 480
		private int[] tt;

		// Token: 0x040001E1 RID: 481
		private byte[] ll8;

		// Token: 0x040001E2 RID: 482
		private int[] unzftab = new int[256];

		// Token: 0x040001E3 RID: 483
		private int[][] limit = new int[6][];

		// Token: 0x040001E4 RID: 484
		private int[][] baseArray = new int[6][];

		// Token: 0x040001E5 RID: 485
		private int[][] perm = new int[6][];

		// Token: 0x040001E6 RID: 486
		private int[] minLens = new int[6];

		// Token: 0x040001E7 RID: 487
		private Stream baseStream;

		// Token: 0x040001E8 RID: 488
		private bool streamEnd;

		// Token: 0x040001E9 RID: 489
		private int currentChar = -1;

		// Token: 0x040001EA RID: 490
		private int currentState = 1;

		// Token: 0x040001EB RID: 491
		private int storedBlockCRC;

		// Token: 0x040001EC RID: 492
		private int storedCombinedCRC;

		// Token: 0x040001ED RID: 493
		private int computedBlockCRC;

		// Token: 0x040001EE RID: 494
		private uint computedCombinedCRC;

		// Token: 0x040001EF RID: 495
		private int count;

		// Token: 0x040001F0 RID: 496
		private int chPrev;

		// Token: 0x040001F1 RID: 497
		private int ch2;

		// Token: 0x040001F2 RID: 498
		private int tPos;

		// Token: 0x040001F3 RID: 499
		private int rNToGo;

		// Token: 0x040001F4 RID: 500
		private int rTPos;

		// Token: 0x040001F5 RID: 501
		private int i2;

		// Token: 0x040001F6 RID: 502
		private int j2;

		// Token: 0x040001F7 RID: 503
		private byte z;

		// Token: 0x040001F8 RID: 504
		private bool isStreamOwner = true;
	}
}
