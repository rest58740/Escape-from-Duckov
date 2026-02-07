using System;
using System.IO;
using Pathfinding.Ionic.Crc;

namespace Pathfinding.Ionic.BZip2
{
	// Token: 0x0200003E RID: 62
	public class BZip2InputStream : Stream
	{
		// Token: 0x06000308 RID: 776 RVA: 0x00012EA0 File Offset: 0x000110A0
		public BZip2InputStream(Stream input) : this(input, false)
		{
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00012EAC File Offset: 0x000110AC
		public BZip2InputStream(Stream input, bool leaveOpen)
		{
			this.input = input;
			this._leaveOpen = leaveOpen;
			this.init();
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00012EF0 File Offset: 0x000110F0
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (offset < 0)
			{
				throw new IndexOutOfRangeException(string.Format("offset ({0}) must be > 0", offset));
			}
			if (count < 0)
			{
				throw new IndexOutOfRangeException(string.Format("count ({0}) must be > 0", count));
			}
			if (offset + count > buffer.Length)
			{
				throw new IndexOutOfRangeException(string.Format("offset({0}) count({1}) bLength({2})", offset, count, buffer.Length));
			}
			if (this.input == null)
			{
				throw new IOException("the stream is not open");
			}
			int num = offset + count;
			int num2 = offset;
			int num3;
			while (num2 < num && (num3 = this.ReadByte()) >= 0)
			{
				buffer[num2++] = (byte)num3;
			}
			return (num2 != offset) ? (num2 - offset) : -1;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00012FB8 File Offset: 0x000111B8
		private void MakeMaps()
		{
			bool[] inUse = this.data.inUse;
			byte[] seqToUnseq = this.data.seqToUnseq;
			int num = 0;
			for (int i = 0; i < 256; i++)
			{
				if (inUse[i])
				{
					seqToUnseq[num++] = (byte)i;
				}
			}
			this.nInUse = num;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00013010 File Offset: 0x00011210
		public override int ReadByte()
		{
			int result = this.currentChar;
			this.totalBytesRead += 1L;
			switch (this.currentState)
			{
			case BZip2InputStream.CState.EOF:
				return -1;
			case BZip2InputStream.CState.START_BLOCK:
				throw new IOException("bad state");
			case BZip2InputStream.CState.RAND_PART_A:
				throw new IOException("bad state");
			case BZip2InputStream.CState.RAND_PART_B:
				this.SetupRandPartB();
				break;
			case BZip2InputStream.CState.RAND_PART_C:
				this.SetupRandPartC();
				break;
			case BZip2InputStream.CState.NO_RAND_PART_A:
				throw new IOException("bad state");
			case BZip2InputStream.CState.NO_RAND_PART_B:
				this.SetupNoRandPartB();
				break;
			case BZip2InputStream.CState.NO_RAND_PART_C:
				this.SetupNoRandPartC();
				break;
			default:
				throw new IOException("bad state");
			}
			return result;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000130C0 File Offset: 0x000112C0
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.input.CanRead;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600030E RID: 782 RVA: 0x000130E4 File Offset: 0x000112E4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000130E8 File Offset: 0x000112E8
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("BZip2Stream");
				}
				return this.input.CanWrite;
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0001310C File Offset: 0x0001130C
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("BZip2Stream");
			}
			this.input.Flush();
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00013130 File Offset: 0x00011330
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00013138 File Offset: 0x00011338
		// (set) Token: 0x06000313 RID: 787 RVA: 0x00013140 File Offset: 0x00011340
		public override long Position
		{
			get
			{
				return this.totalBytesRead;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00013148 File Offset: 0x00011348
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00013150 File Offset: 0x00011350
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00013158 File Offset: 0x00011358
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00013160 File Offset: 0x00011360
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this.input != null)
					{
						this.input.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000131C4 File Offset: 0x000113C4
		private void init()
		{
			if (this.input == null)
			{
				throw new IOException("No input Stream");
			}
			if (!this.input.CanRead)
			{
				throw new IOException("Unreadable input Stream");
			}
			this.CheckMagicChar('B', 0);
			this.CheckMagicChar('Z', 1);
			this.CheckMagicChar('h', 2);
			int num = this.input.ReadByte();
			if (num < 49 || num > 57)
			{
				throw new IOException("Stream is not BZip2 formatted: illegal blocksize " + (char)num);
			}
			this.blockSize100k = num - 48;
			this.InitBlock();
			this.SetupBlock();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00013268 File Offset: 0x00011468
		private void CheckMagicChar(char expected, int position)
		{
			int num = this.input.ReadByte();
			if (num != (int)expected)
			{
				string text = string.Format("Not a valid BZip2 stream. byte {0}, expected '{1}', got '{2}'", position, (int)expected, num);
				throw new IOException(text);
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000132AC File Offset: 0x000114AC
		private void InitBlock()
		{
			char c = this.bsGetUByte();
			char c2 = this.bsGetUByte();
			char c3 = this.bsGetUByte();
			char c4 = this.bsGetUByte();
			char c5 = this.bsGetUByte();
			char c6 = this.bsGetUByte();
			if (c == '\u0017' && c2 == 'r' && c3 == 'E' && c4 == '8' && c5 == 'P' && c6 == '\u0090')
			{
				this.complete();
			}
			else
			{
				if (c != '1' || c2 != 'A' || c3 != 'Y' || c4 != '&' || c5 != 'S' || c6 != 'Y')
				{
					this.currentState = BZip2InputStream.CState.EOF;
					string text = string.Format("bad block header at offset 0x{0:X}", this.input.Position);
					throw new IOException(text);
				}
				this.storedBlockCRC = this.bsGetInt();
				this.blockRandomised = (this.GetBits(1) == 1);
				if (this.data == null)
				{
					this.data = new BZip2InputStream.DecompressionState(this.blockSize100k);
				}
				this.getAndMoveToFrontDecode();
				this.crc.Reset();
				this.currentState = BZip2InputStream.CState.START_BLOCK;
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000133D4 File Offset: 0x000115D4
		private void EndBlock()
		{
			this.computedBlockCRC = (uint)this.crc.Crc32Result;
			if (this.storedBlockCRC != this.computedBlockCRC)
			{
				string text = string.Format("BZip2 CRC error (expected {0:X8}, computed {1:X8})", this.storedBlockCRC, this.computedBlockCRC);
				throw new IOException(text);
			}
			this.computedCombinedCRC = (this.computedCombinedCRC << 1 | this.computedCombinedCRC >> 31);
			this.computedCombinedCRC ^= this.computedBlockCRC;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00013458 File Offset: 0x00011658
		private void complete()
		{
			this.storedCombinedCRC = this.bsGetInt();
			this.currentState = BZip2InputStream.CState.EOF;
			this.data = null;
			if (this.storedCombinedCRC != this.computedCombinedCRC)
			{
				string text = string.Format("BZip2 CRC error (expected {0:X8}, computed {1:X8})", this.storedCombinedCRC, this.computedCombinedCRC);
				throw new IOException(text);
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000134B8 File Offset: 0x000116B8
		public override void Close()
		{
			Stream stream = this.input;
			if (stream != null)
			{
				try
				{
					if (!this._leaveOpen)
					{
						stream.Close();
					}
				}
				finally
				{
					this.data = null;
					this.input = null;
				}
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00013514 File Offset: 0x00011714
		private int GetBits(int n)
		{
			int num = this.bsLive;
			int num2 = this.bsBuff;
			if (num < n)
			{
				for (;;)
				{
					int num3 = this.input.ReadByte();
					if (num3 < 0)
					{
						break;
					}
					num2 = (num2 << 8 | num3);
					num += 8;
					if (num >= n)
					{
						goto Block_2;
					}
				}
				throw new IOException("unexpected end of stream");
				Block_2:
				this.bsBuff = num2;
			}
			this.bsLive = num - n;
			return num2 >> num - n & (1 << n) - 1;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00013588 File Offset: 0x00011788
		private bool bsGetBit()
		{
			int bits = this.GetBits(1);
			return bits != 0;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000135A4 File Offset: 0x000117A4
		private char bsGetUByte()
		{
			return (char)this.GetBits(8);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000135B0 File Offset: 0x000117B0
		private uint bsGetInt()
		{
			return (uint)(((this.GetBits(8) << 8 | this.GetBits(8)) << 8 | this.GetBits(8)) << 8 | this.GetBits(8));
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000135E4 File Offset: 0x000117E4
		private static void hbCreateDecodeTables(int[] limit, int[] bbase, int[] perm, char[] length, int minLen, int maxLen, int alphaSize)
		{
			int i = minLen;
			int num = 0;
			while (i <= maxLen)
			{
				for (int j = 0; j < alphaSize; j++)
				{
					if ((int)length[j] == i)
					{
						perm[num++] = j;
					}
				}
				i++;
			}
			int num2 = BZip2.MaxCodeLength;
			while (--num2 > 0)
			{
				bbase[num2] = 0;
				limit[num2] = 0;
			}
			for (int k = 0; k < alphaSize; k++)
			{
				bbase[(int)(length[k] + '\u0001')]++;
			}
			int l = 1;
			int num3 = bbase[0];
			while (l < BZip2.MaxCodeLength)
			{
				num3 += bbase[l];
				bbase[l] = num3;
				l++;
			}
			int m = minLen;
			int num4 = 0;
			int num5 = bbase[m];
			while (m <= maxLen)
			{
				int num6 = bbase[m + 1];
				num4 += num6 - num5;
				num5 = num6;
				limit[m] = num4 - 1;
				num4 <<= 1;
				m++;
			}
			for (int n = minLen + 1; n <= maxLen; n++)
			{
				bbase[n] = (limit[n - 1] + 1 << 1) - bbase[n];
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00013714 File Offset: 0x00011914
		private void recvDecodingTables()
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			bool[] inUse = decompressionState.inUse;
			byte[] recvDecodingTables_pos = decompressionState.recvDecodingTables_pos;
			int num = 0;
			for (int i = 0; i < 16; i++)
			{
				if (this.bsGetBit())
				{
					num |= 1 << i;
				}
			}
			int num2 = 256;
			while (--num2 >= 0)
			{
				inUse[num2] = false;
			}
			for (int j = 0; j < 16; j++)
			{
				if ((num & 1 << j) != 0)
				{
					int num3 = j << 4;
					for (int k = 0; k < 16; k++)
					{
						if (this.bsGetBit())
						{
							inUse[num3 + k] = true;
						}
					}
				}
			}
			this.MakeMaps();
			int num4 = this.nInUse + 2;
			int bits = this.GetBits(3);
			int bits2 = this.GetBits(15);
			for (int l = 0; l < bits2; l++)
			{
				int num5 = 0;
				while (this.bsGetBit())
				{
					num5++;
				}
				decompressionState.selectorMtf[l] = (byte)num5;
			}
			int num6 = bits;
			while (--num6 >= 0)
			{
				recvDecodingTables_pos[num6] = (byte)num6;
			}
			for (int m = 0; m < bits2; m++)
			{
				int n = (int)decompressionState.selectorMtf[m];
				byte b = recvDecodingTables_pos[n];
				while (n > 0)
				{
					recvDecodingTables_pos[n] = recvDecodingTables_pos[n - 1];
					n--;
				}
				recvDecodingTables_pos[0] = b;
				decompressionState.selector[m] = b;
			}
			char[][] temp_charArray2d = decompressionState.temp_charArray2d;
			for (int num7 = 0; num7 < bits; num7++)
			{
				int num8 = this.GetBits(5);
				char[] array = temp_charArray2d[num7];
				for (int num9 = 0; num9 < num4; num9++)
				{
					while (this.bsGetBit())
					{
						num8 += ((!this.bsGetBit()) ? 1 : -1);
					}
					array[num9] = (char)num8;
				}
			}
			this.createHuffmanDecodingTables(num4, bits);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00013928 File Offset: 0x00011B28
		private void createHuffmanDecodingTables(int alphaSize, int nGroups)
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			char[][] temp_charArray2d = decompressionState.temp_charArray2d;
			for (int i = 0; i < nGroups; i++)
			{
				int num = 32;
				int num2 = 0;
				char[] array = temp_charArray2d[i];
				int num3 = alphaSize;
				while (--num3 >= 0)
				{
					char c = array[num3];
					if ((int)c > num2)
					{
						num2 = (int)c;
					}
					if ((int)c < num)
					{
						num = (int)c;
					}
				}
				BZip2InputStream.hbCreateDecodeTables(decompressionState.gLimit[i], decompressionState.gBase[i], decompressionState.gPerm[i], temp_charArray2d[i], num, num2, alphaSize);
				decompressionState.gMinlen[i] = num;
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000139C4 File Offset: 0x00011BC4
		private void getAndMoveToFrontDecode()
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			this.origPtr = this.GetBits(24);
			if (this.origPtr < 0)
			{
				throw new IOException("BZ_DATA_ERROR");
			}
			if (this.origPtr > 10 + BZip2.BlockSizeMultiple * this.blockSize100k)
			{
				throw new IOException("BZ_DATA_ERROR");
			}
			this.recvDecodingTables();
			byte[] getAndMoveToFrontDecode_yy = decompressionState.getAndMoveToFrontDecode_yy;
			int num = this.blockSize100k * BZip2.BlockSizeMultiple;
			int num2 = 256;
			while (--num2 >= 0)
			{
				getAndMoveToFrontDecode_yy[num2] = (byte)num2;
				decompressionState.unzftab[num2] = 0;
			}
			int num3 = 0;
			int num4 = BZip2.G_SIZE - 1;
			int num5 = this.nInUse + 1;
			int num6 = this.getAndMoveToFrontDecode0(0);
			int num7 = this.bsBuff;
			int i = this.bsLive;
			int num8 = -1;
			int num9 = (int)(decompressionState.selector[num3] & byte.MaxValue);
			int[] array = decompressionState.gBase[num9];
			int[] array2 = decompressionState.gLimit[num9];
			int[] array3 = decompressionState.gPerm[num9];
			int num10 = decompressionState.gMinlen[num9];
			while (num6 != num5)
			{
				if (num6 == (int)BZip2.RUNA || num6 == (int)BZip2.RUNB)
				{
					int num11 = -1;
					int num12 = 1;
					for (;;)
					{
						if (num6 == (int)BZip2.RUNA)
						{
							num11 += num12;
						}
						else
						{
							if (num6 != (int)BZip2.RUNB)
							{
								break;
							}
							num11 += num12 << 1;
						}
						if (num4 == 0)
						{
							num4 = BZip2.G_SIZE - 1;
							num9 = (int)(decompressionState.selector[++num3] & byte.MaxValue);
							array = decompressionState.gBase[num9];
							array2 = decompressionState.gLimit[num9];
							array3 = decompressionState.gPerm[num9];
							num10 = decompressionState.gMinlen[num9];
						}
						else
						{
							num4--;
						}
						int num13 = num10;
						while (i < num13)
						{
							int num14 = this.input.ReadByte();
							if (num14 < 0)
							{
								goto IL_1EC;
							}
							num7 = (num7 << 8 | num14);
							i += 8;
						}
						int j = num7 >> i - num13 & (1 << num13) - 1;
						i -= num13;
						while (j > array2[num13])
						{
							num13++;
							while (i < 1)
							{
								int num15 = this.input.ReadByte();
								if (num15 < 0)
								{
									goto IL_257;
								}
								num7 = (num7 << 8 | num15);
								i += 8;
							}
							i--;
							j = (j << 1 | (num7 >> i & 1));
						}
						num6 = array3[j - array[num13]];
						num12 <<= 1;
					}
					byte b = decompressionState.seqToUnseq[(int)getAndMoveToFrontDecode_yy[0]];
					decompressionState.unzftab[(int)(b & byte.MaxValue)] += num11 + 1;
					while (num11-- >= 0)
					{
						decompressionState.ll8[++num8] = b;
					}
					if (num8 >= num)
					{
						throw new IOException("block overrun");
					}
					continue;
					IL_1EC:
					throw new IOException("unexpected end of stream");
					IL_257:
					throw new IOException("unexpected end of stream");
				}
				if (++num8 >= num)
				{
					throw new IOException("block overrun");
				}
				byte b2 = getAndMoveToFrontDecode_yy[num6 - 1];
				decompressionState.unzftab[(int)(decompressionState.seqToUnseq[(int)b2] & byte.MaxValue)]++;
				decompressionState.ll8[num8] = decompressionState.seqToUnseq[(int)b2];
				if (num6 <= 16)
				{
					int k = num6 - 1;
					while (k > 0)
					{
						getAndMoveToFrontDecode_yy[k] = getAndMoveToFrontDecode_yy[--k];
					}
				}
				else
				{
					Buffer.BlockCopy(getAndMoveToFrontDecode_yy, 0, getAndMoveToFrontDecode_yy, 1, num6 - 1);
				}
				getAndMoveToFrontDecode_yy[0] = b2;
				if (num4 == 0)
				{
					num4 = BZip2.G_SIZE - 1;
					num9 = (int)(decompressionState.selector[++num3] & byte.MaxValue);
					array = decompressionState.gBase[num9];
					array2 = decompressionState.gLimit[num9];
					array3 = decompressionState.gPerm[num9];
					num10 = decompressionState.gMinlen[num9];
				}
				else
				{
					num4--;
				}
				int num16 = num10;
				while (i < num16)
				{
					int num17 = this.input.ReadByte();
					if (num17 < 0)
					{
						throw new IOException("unexpected end of stream");
					}
					num7 = (num7 << 8 | num17);
					i += 8;
				}
				int l = num7 >> i - num16 & (1 << num16) - 1;
				i -= num16;
				while (l > array2[num16])
				{
					num16++;
					while (i < 1)
					{
						int num18 = this.input.ReadByte();
						if (num18 < 0)
						{
							throw new IOException("unexpected end of stream");
						}
						num7 = (num7 << 8 | num18);
						i += 8;
					}
					i--;
					l = (l << 1 | (num7 >> i & 1));
				}
				num6 = array3[l - array[num16]];
			}
			this.last = num8;
			this.bsLive = i;
			this.bsBuff = num7;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00013EC8 File Offset: 0x000120C8
		private int getAndMoveToFrontDecode0(int groupNo)
		{
			BZip2InputStream.DecompressionState decompressionState = this.data;
			int num = (int)(decompressionState.selector[groupNo] & byte.MaxValue);
			int[] array = decompressionState.gLimit[num];
			int num2 = decompressionState.gMinlen[num];
			int i = this.GetBits(num2);
			int j = this.bsLive;
			int num3 = this.bsBuff;
			while (i > array[num2])
			{
				num2++;
				while (j < 1)
				{
					int num4 = this.input.ReadByte();
					if (num4 < 0)
					{
						throw new IOException("unexpected end of stream");
					}
					num3 = (num3 << 8 | num4);
					j += 8;
				}
				j--;
				i = (i << 1 | (num3 >> j & 1));
			}
			this.bsLive = j;
			this.bsBuff = num3;
			return decompressionState.gPerm[num][i - decompressionState.gBase[num][num2]];
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00013FA8 File Offset: 0x000121A8
		private void SetupBlock()
		{
			if (this.data == null)
			{
				return;
			}
			BZip2InputStream.DecompressionState decompressionState = this.data;
			int[] array = decompressionState.initTT(this.last + 1);
			int i;
			for (i = 0; i <= 255; i++)
			{
				if (decompressionState.unzftab[i] < 0 || decompressionState.unzftab[i] > this.last)
				{
					throw new Exception("BZ_DATA_ERROR");
				}
			}
			decompressionState.cftab[0] = 0;
			for (i = 1; i <= 256; i++)
			{
				decompressionState.cftab[i] = decompressionState.unzftab[i - 1];
			}
			for (i = 1; i <= 256; i++)
			{
				decompressionState.cftab[i] += decompressionState.cftab[i - 1];
			}
			for (i = 0; i <= 256; i++)
			{
				if (decompressionState.cftab[i] < 0 || decompressionState.cftab[i] > this.last + 1)
				{
					string text = string.Format("BZ_DATA_ERROR: cftab[{0}]={1} last={2}", i, decompressionState.cftab[i], this.last);
					throw new Exception(text);
				}
			}
			for (i = 1; i <= 256; i++)
			{
				if (decompressionState.cftab[i - 1] > decompressionState.cftab[i])
				{
					throw new Exception("BZ_DATA_ERROR");
				}
			}
			i = 0;
			int num = this.last;
			while (i <= num)
			{
				array[decompressionState.cftab[(int)(decompressionState.ll8[i] & byte.MaxValue)]++] = i;
				i++;
			}
			if (this.origPtr < 0 || this.origPtr >= array.Length)
			{
				throw new IOException("stream corrupted");
			}
			this.su_tPos = array[this.origPtr];
			this.su_count = 0;
			this.su_i2 = 0;
			this.su_ch2 = 256;
			if (this.blockRandomised)
			{
				this.su_rNToGo = 0;
				this.su_rTPos = 0;
				this.SetupRandPartA();
			}
			else
			{
				this.SetupNoRandPartA();
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x000141D4 File Offset: 0x000123D4
		private void SetupRandPartA()
		{
			if (this.su_i2 <= this.last)
			{
				this.su_chPrev = this.su_ch2;
				int num = (int)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_tPos = this.data.tt[this.su_tPos];
				if (this.su_rNToGo == 0)
				{
					this.su_rNToGo = Rand.Rnums(this.su_rTPos) - 1;
					if (++this.su_rTPos == 512)
					{
						this.su_rTPos = 0;
					}
				}
				else
				{
					this.su_rNToGo--;
				}
				num = (this.su_ch2 = (num ^ ((this.su_rNToGo != 1) ? 0 : 1)));
				this.su_i2++;
				this.currentChar = num;
				this.currentState = BZip2InputStream.CState.RAND_PART_B;
				this.crc.UpdateCRC((byte)num);
			}
			else
			{
				this.EndBlock();
				this.InitBlock();
				this.SetupBlock();
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x000142E0 File Offset: 0x000124E0
		private void SetupNoRandPartA()
		{
			if (this.su_i2 <= this.last)
			{
				this.su_chPrev = this.su_ch2;
				int num = (int)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_ch2 = num;
				this.su_tPos = this.data.tt[this.su_tPos];
				this.su_i2++;
				this.currentChar = num;
				this.currentState = BZip2InputStream.CState.NO_RAND_PART_B;
				this.crc.UpdateCRC((byte)num);
			}
			else
			{
				this.currentState = BZip2InputStream.CState.NO_RAND_PART_A;
				this.EndBlock();
				this.InitBlock();
				this.SetupBlock();
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0001438C File Offset: 0x0001258C
		private void SetupRandPartB()
		{
			if (this.su_ch2 != this.su_chPrev)
			{
				this.currentState = BZip2InputStream.CState.RAND_PART_A;
				this.su_count = 1;
				this.SetupRandPartA();
			}
			else if (++this.su_count >= 4)
			{
				this.su_z = (char)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_tPos = this.data.tt[this.su_tPos];
				if (this.su_rNToGo == 0)
				{
					this.su_rNToGo = Rand.Rnums(this.su_rTPos) - 1;
					if (++this.su_rTPos == 512)
					{
						this.su_rTPos = 0;
					}
				}
				else
				{
					this.su_rNToGo--;
				}
				this.su_j2 = 0;
				this.currentState = BZip2InputStream.CState.RAND_PART_C;
				if (this.su_rNToGo == 1)
				{
					this.su_z ^= '\u0001';
				}
				this.SetupRandPartC();
			}
			else
			{
				this.currentState = BZip2InputStream.CState.RAND_PART_A;
				this.SetupRandPartA();
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x000144A8 File Offset: 0x000126A8
		private void SetupRandPartC()
		{
			if (this.su_j2 < (int)this.su_z)
			{
				this.currentChar = this.su_ch2;
				this.crc.UpdateCRC((byte)this.su_ch2);
				this.su_j2++;
			}
			else
			{
				this.currentState = BZip2InputStream.CState.RAND_PART_A;
				this.su_i2++;
				this.su_count = 0;
				this.SetupRandPartA();
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0001451C File Offset: 0x0001271C
		private void SetupNoRandPartB()
		{
			if (this.su_ch2 != this.su_chPrev)
			{
				this.su_count = 1;
				this.SetupNoRandPartA();
			}
			else if (++this.su_count >= 4)
			{
				this.su_z = (char)(this.data.ll8[this.su_tPos] & byte.MaxValue);
				this.su_tPos = this.data.tt[this.su_tPos];
				this.su_j2 = 0;
				this.SetupNoRandPartC();
			}
			else
			{
				this.SetupNoRandPartA();
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000145B4 File Offset: 0x000127B4
		private void SetupNoRandPartC()
		{
			if (this.su_j2 < (int)this.su_z)
			{
				int num = this.su_ch2;
				this.currentChar = num;
				this.crc.UpdateCRC((byte)num);
				this.su_j2++;
				this.currentState = BZip2InputStream.CState.NO_RAND_PART_C;
			}
			else
			{
				this.su_i2++;
				this.su_count = 0;
				this.SetupNoRandPartA();
			}
		}

		// Token: 0x040001A3 RID: 419
		private bool _disposed;

		// Token: 0x040001A4 RID: 420
		private bool _leaveOpen;

		// Token: 0x040001A5 RID: 421
		private long totalBytesRead;

		// Token: 0x040001A6 RID: 422
		private int last;

		// Token: 0x040001A7 RID: 423
		private int origPtr;

		// Token: 0x040001A8 RID: 424
		private int blockSize100k;

		// Token: 0x040001A9 RID: 425
		private bool blockRandomised;

		// Token: 0x040001AA RID: 426
		private int bsBuff;

		// Token: 0x040001AB RID: 427
		private int bsLive;

		// Token: 0x040001AC RID: 428
		private readonly CRC32 crc = new CRC32(true);

		// Token: 0x040001AD RID: 429
		private int nInUse;

		// Token: 0x040001AE RID: 430
		private Stream input;

		// Token: 0x040001AF RID: 431
		private int currentChar = -1;

		// Token: 0x040001B0 RID: 432
		private BZip2InputStream.CState currentState = BZip2InputStream.CState.START_BLOCK;

		// Token: 0x040001B1 RID: 433
		private uint storedBlockCRC;

		// Token: 0x040001B2 RID: 434
		private uint storedCombinedCRC;

		// Token: 0x040001B3 RID: 435
		private uint computedBlockCRC;

		// Token: 0x040001B4 RID: 436
		private uint computedCombinedCRC;

		// Token: 0x040001B5 RID: 437
		private int su_count;

		// Token: 0x040001B6 RID: 438
		private int su_ch2;

		// Token: 0x040001B7 RID: 439
		private int su_chPrev;

		// Token: 0x040001B8 RID: 440
		private int su_i2;

		// Token: 0x040001B9 RID: 441
		private int su_j2;

		// Token: 0x040001BA RID: 442
		private int su_rNToGo;

		// Token: 0x040001BB RID: 443
		private int su_rTPos;

		// Token: 0x040001BC RID: 444
		private int su_tPos;

		// Token: 0x040001BD RID: 445
		private char su_z;

		// Token: 0x040001BE RID: 446
		private BZip2InputStream.DecompressionState data;

		// Token: 0x0200003F RID: 63
		private enum CState
		{
			// Token: 0x040001C0 RID: 448
			EOF,
			// Token: 0x040001C1 RID: 449
			START_BLOCK,
			// Token: 0x040001C2 RID: 450
			RAND_PART_A,
			// Token: 0x040001C3 RID: 451
			RAND_PART_B,
			// Token: 0x040001C4 RID: 452
			RAND_PART_C,
			// Token: 0x040001C5 RID: 453
			NO_RAND_PART_A,
			// Token: 0x040001C6 RID: 454
			NO_RAND_PART_B,
			// Token: 0x040001C7 RID: 455
			NO_RAND_PART_C
		}

		// Token: 0x02000040 RID: 64
		private sealed class DecompressionState
		{
			// Token: 0x0600032E RID: 814 RVA: 0x00014624 File Offset: 0x00012824
			public DecompressionState(int blockSize100k)
			{
				this.unzftab = new int[256];
				this.gLimit = BZip2.InitRectangularArray<int>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.gBase = BZip2.InitRectangularArray<int>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.gPerm = BZip2.InitRectangularArray<int>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.gMinlen = new int[BZip2.NGroups];
				this.cftab = new int[257];
				this.getAndMoveToFrontDecode_yy = new byte[256];
				this.temp_charArray2d = BZip2.InitRectangularArray<char>(BZip2.NGroups, BZip2.MaxAlphaSize);
				this.recvDecodingTables_pos = new byte[BZip2.NGroups];
				this.ll8 = new byte[blockSize100k * BZip2.BlockSizeMultiple];
			}

			// Token: 0x0600032F RID: 815 RVA: 0x00014730 File Offset: 0x00012930
			public int[] initTT(int length)
			{
				int[] array = this.tt;
				if (array == null || array.Length < length)
				{
					array = (this.tt = new int[length]);
				}
				return array;
			}

			// Token: 0x040001C8 RID: 456
			public readonly bool[] inUse = new bool[256];

			// Token: 0x040001C9 RID: 457
			public readonly byte[] seqToUnseq = new byte[256];

			// Token: 0x040001CA RID: 458
			public readonly byte[] selector = new byte[BZip2.MaxSelectors];

			// Token: 0x040001CB RID: 459
			public readonly byte[] selectorMtf = new byte[BZip2.MaxSelectors];

			// Token: 0x040001CC RID: 460
			public readonly int[] unzftab;

			// Token: 0x040001CD RID: 461
			public readonly int[][] gLimit;

			// Token: 0x040001CE RID: 462
			public readonly int[][] gBase;

			// Token: 0x040001CF RID: 463
			public readonly int[][] gPerm;

			// Token: 0x040001D0 RID: 464
			public readonly int[] gMinlen;

			// Token: 0x040001D1 RID: 465
			public readonly int[] cftab;

			// Token: 0x040001D2 RID: 466
			public readonly byte[] getAndMoveToFrontDecode_yy;

			// Token: 0x040001D3 RID: 467
			public readonly char[][] temp_charArray2d;

			// Token: 0x040001D4 RID: 468
			public readonly byte[] recvDecodingTables_pos;

			// Token: 0x040001D5 RID: 469
			public int[] tt;

			// Token: 0x040001D6 RID: 470
			public byte[] ll8;
		}
	}
}
