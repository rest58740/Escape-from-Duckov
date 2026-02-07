using System;
using ICSharpCode.SharpZipLib.Checksums;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x0200000C RID: 12
	public class DeflaterEngine : DeflaterConstants
	{
		// Token: 0x06000079 RID: 121 RVA: 0x0000482C File Offset: 0x00002A2C
		public DeflaterEngine(DeflaterPending pending)
		{
			this.pending = pending;
			this.huffman = new DeflaterHuffman(pending);
			this.adler = new Adler32();
			this.window = new byte[65536];
			this.head = new short[32768];
			this.prev = new short[32768];
			this.blockStart = (this.strstart = 1);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000048A0 File Offset: 0x00002AA0
		public bool Deflate(bool flush, bool finish)
		{
			for (;;)
			{
				this.FillWindow();
				bool flush2 = flush && this.inputOff == this.inputEnd;
				bool flag;
				switch (this.compressionFunction)
				{
				case 0:
					flag = this.DeflateStored(flush2, finish);
					goto IL_71;
				case 1:
					flag = this.DeflateFast(flush2, finish);
					goto IL_71;
				case 2:
					flag = this.DeflateSlow(flush2, finish);
					goto IL_71;
				}
				break;
				IL_71:
				if (!this.pending.IsFlushed || !flag)
				{
					return flag;
				}
			}
			throw new InvalidOperationException("unknown compressionFunction");
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004938 File Offset: 0x00002B38
		public void SetInput(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.inputOff < this.inputEnd)
			{
				throw new InvalidOperationException("Old input was not completely processed");
			}
			int num = offset + count;
			if (offset > num || num > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.inputBuf = buffer;
			this.inputOff = offset;
			this.inputEnd = num;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000049CC File Offset: 0x00002BCC
		public bool NeedsInput()
		{
			return this.inputEnd == this.inputOff;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000049DC File Offset: 0x00002BDC
		public void SetDictionary(byte[] buffer, int offset, int length)
		{
			this.adler.Update(buffer, offset, length);
			if (length < 3)
			{
				return;
			}
			if (length > 32506)
			{
				offset += length - 32506;
				length = 32506;
			}
			Array.Copy(buffer, offset, this.window, this.strstart, length);
			this.UpdateHash();
			length--;
			while (--length > 0)
			{
				this.InsertString();
				this.strstart++;
			}
			this.strstart += 2;
			this.blockStart = this.strstart;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004A7C File Offset: 0x00002C7C
		public void Reset()
		{
			this.huffman.Reset();
			this.adler.Reset();
			this.blockStart = (this.strstart = 1);
			this.lookahead = 0;
			this.totalIn = 0L;
			this.prevAvailable = false;
			this.matchLen = 2;
			for (int i = 0; i < 32768; i++)
			{
				this.head[i] = 0;
			}
			for (int j = 0; j < 32768; j++)
			{
				this.prev[j] = 0;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004B0C File Offset: 0x00002D0C
		public void ResetAdler()
		{
			this.adler.Reset();
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004B1C File Offset: 0x00002D1C
		public int Adler
		{
			get
			{
				return (int)this.adler.Value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00004B2C File Offset: 0x00002D2C
		public long TotalIn
		{
			get
			{
				return this.totalIn;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004B34 File Offset: 0x00002D34
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00004B3C File Offset: 0x00002D3C
		public DeflateStrategy Strategy
		{
			get
			{
				return this.strategy;
			}
			set
			{
				this.strategy = value;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004B48 File Offset: 0x00002D48
		public void SetLevel(int level)
		{
			if (level < 0 || level > 9)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			this.goodLength = DeflaterConstants.GOOD_LENGTH[level];
			this.max_lazy = DeflaterConstants.MAX_LAZY[level];
			this.niceLength = DeflaterConstants.NICE_LENGTH[level];
			this.max_chain = DeflaterConstants.MAX_CHAIN[level];
			if (DeflaterConstants.COMPR_FUNC[level] != this.compressionFunction)
			{
				switch (this.compressionFunction)
				{
				case 0:
					if (this.strstart > this.blockStart)
					{
						this.huffman.FlushStoredBlock(this.window, this.blockStart, this.strstart - this.blockStart, false);
						this.blockStart = this.strstart;
					}
					this.UpdateHash();
					break;
				case 1:
					if (this.strstart > this.blockStart)
					{
						this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, false);
						this.blockStart = this.strstart;
					}
					break;
				case 2:
					if (this.prevAvailable)
					{
						this.huffman.TallyLit((int)(this.window[this.strstart - 1] & byte.MaxValue));
					}
					if (this.strstart > this.blockStart)
					{
						this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, false);
						this.blockStart = this.strstart;
					}
					this.prevAvailable = false;
					this.matchLen = 2;
					break;
				}
				this.compressionFunction = DeflaterConstants.COMPR_FUNC[level];
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public void FillWindow()
		{
			if (this.strstart >= 65274)
			{
				this.SlideWindow();
			}
			while (this.lookahead < 262 && this.inputOff < this.inputEnd)
			{
				int num = 65536 - this.lookahead - this.strstart;
				if (num > this.inputEnd - this.inputOff)
				{
					num = this.inputEnd - this.inputOff;
				}
				Array.Copy(this.inputBuf, this.inputOff, this.window, this.strstart + this.lookahead, num);
				this.adler.Update(this.inputBuf, this.inputOff, num);
				this.inputOff += num;
				this.totalIn += (long)num;
				this.lookahead += num;
			}
			if (this.lookahead >= 3)
			{
				this.UpdateHash();
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004DF0 File Offset: 0x00002FF0
		private void UpdateHash()
		{
			this.ins_h = ((int)this.window[this.strstart] << 5 ^ (int)this.window[this.strstart + 1]);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004E18 File Offset: 0x00003018
		private int InsertString()
		{
			int num = (this.ins_h << 5 ^ (int)this.window[this.strstart + 2]) & 32767;
			short num2 = this.prev[this.strstart & 32767] = this.head[num];
			this.head[num] = (short)this.strstart;
			this.ins_h = num;
			return (int)num2 & 65535;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004E80 File Offset: 0x00003080
		private void SlideWindow()
		{
			Array.Copy(this.window, 32768, this.window, 0, 32768);
			this.matchStart -= 32768;
			this.strstart -= 32768;
			this.blockStart -= 32768;
			for (int i = 0; i < 32768; i++)
			{
				int num = (int)this.head[i] & 65535;
				this.head[i] = (short)((num < 32768) ? 0 : (num - 32768));
			}
			for (int j = 0; j < 32768; j++)
			{
				int num2 = (int)this.prev[j] & 65535;
				this.prev[j] = (short)((num2 < 32768) ? 0 : (num2 - 32768));
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004F6C File Offset: 0x0000316C
		private bool FindLongestMatch(int curMatch)
		{
			int num = this.max_chain;
			int num2 = this.niceLength;
			short[] array = this.prev;
			int num3 = this.strstart;
			int num4 = this.strstart + this.matchLen;
			int num5 = Math.Max(this.matchLen, 2);
			int num6 = Math.Max(this.strstart - 32506, 0);
			int num7 = this.strstart + 258 - 1;
			byte b = this.window[num4 - 1];
			byte b2 = this.window[num4];
			if (num5 >= this.goodLength)
			{
				num >>= 2;
			}
			if (num2 > this.lookahead)
			{
				num2 = this.lookahead;
			}
			do
			{
				if (this.window[curMatch + num5] == b2 && this.window[curMatch + num5 - 1] == b && this.window[curMatch] == this.window[num3] && this.window[curMatch + 1] == this.window[num3 + 1])
				{
					int num8 = curMatch + 2;
					num3 += 2;
					while (this.window[++num3] == this.window[++num8] && this.window[++num3] == this.window[++num8] && this.window[++num3] == this.window[++num8] && this.window[++num3] == this.window[++num8] && this.window[++num3] == this.window[++num8] && this.window[++num3] == this.window[++num8] && this.window[++num3] == this.window[++num8] && this.window[++num3] == this.window[++num8] && num3 < num7)
					{
					}
					if (num3 > num4)
					{
						this.matchStart = curMatch;
						num4 = num3;
						num5 = num3 - this.strstart;
						if (num5 >= num2)
						{
							break;
						}
						b = this.window[num4 - 1];
						b2 = this.window[num4];
					}
					num3 = this.strstart;
				}
			}
			while ((curMatch = ((int)array[curMatch & 32767] & 65535)) > num6 && --num != 0);
			this.matchLen = Math.Min(num5, this.lookahead);
			return this.matchLen >= 3;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005204 File Offset: 0x00003404
		private bool DeflateStored(bool flush, bool finish)
		{
			if (!flush && this.lookahead == 0)
			{
				return false;
			}
			this.strstart += this.lookahead;
			this.lookahead = 0;
			int num = this.strstart - this.blockStart;
			if (num >= DeflaterConstants.MAX_BLOCK_SIZE || (this.blockStart < 32768 && num >= 32506) || flush)
			{
				bool flag = finish;
				if (num > DeflaterConstants.MAX_BLOCK_SIZE)
				{
					num = DeflaterConstants.MAX_BLOCK_SIZE;
					flag = false;
				}
				this.huffman.FlushStoredBlock(this.window, this.blockStart, num, flag);
				this.blockStart += num;
				return !flag;
			}
			return true;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000052BC File Offset: 0x000034BC
		private bool DeflateFast(bool flush, bool finish)
		{
			if (this.lookahead < 262 && !flush)
			{
				return false;
			}
			while (this.lookahead >= 262 || flush)
			{
				if (this.lookahead == 0)
				{
					this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, finish);
					this.blockStart = this.strstart;
					return false;
				}
				if (this.strstart > 65274)
				{
					this.SlideWindow();
				}
				int num;
				if (this.lookahead >= 3 && (num = this.InsertString()) != 0 && this.strategy != DeflateStrategy.HuffmanOnly && this.strstart - num <= 32506 && this.FindLongestMatch(num))
				{
					bool flag = this.huffman.TallyDist(this.strstart - this.matchStart, this.matchLen);
					this.lookahead -= this.matchLen;
					if (this.matchLen <= this.max_lazy && this.lookahead >= 3)
					{
						while (--this.matchLen > 0)
						{
							this.strstart++;
							this.InsertString();
						}
						this.strstart++;
					}
					else
					{
						this.strstart += this.matchLen;
						if (this.lookahead >= 2)
						{
							this.UpdateHash();
						}
					}
					this.matchLen = 2;
					if (!flag)
					{
						continue;
					}
				}
				else
				{
					this.huffman.TallyLit((int)(this.window[this.strstart] & byte.MaxValue));
					this.strstart++;
					this.lookahead--;
				}
				if (this.huffman.IsFull())
				{
					bool flag2 = finish && this.lookahead == 0;
					this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, flag2);
					this.blockStart = this.strstart;
					return !flag2;
				}
			}
			return true;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000054F8 File Offset: 0x000036F8
		private bool DeflateSlow(bool flush, bool finish)
		{
			if (this.lookahead < 262 && !flush)
			{
				return false;
			}
			while (this.lookahead >= 262 || flush)
			{
				if (this.lookahead == 0)
				{
					if (this.prevAvailable)
					{
						this.huffman.TallyLit((int)(this.window[this.strstart - 1] & byte.MaxValue));
					}
					this.prevAvailable = false;
					this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, finish);
					this.blockStart = this.strstart;
					return false;
				}
				if (this.strstart >= 65274)
				{
					this.SlideWindow();
				}
				int num = this.matchStart;
				int num2 = this.matchLen;
				if (this.lookahead >= 3)
				{
					int num3 = this.InsertString();
					if (this.strategy != DeflateStrategy.HuffmanOnly && num3 != 0 && this.strstart - num3 <= 32506 && this.FindLongestMatch(num3) && this.matchLen <= 5 && (this.strategy == DeflateStrategy.Filtered || (this.matchLen == 3 && this.strstart - this.matchStart > 4096)))
					{
						this.matchLen = 2;
					}
				}
				if (num2 >= 3 && this.matchLen <= num2)
				{
					this.huffman.TallyDist(this.strstart - 1 - num, num2);
					num2 -= 2;
					do
					{
						this.strstart++;
						this.lookahead--;
						if (this.lookahead >= 3)
						{
							this.InsertString();
						}
					}
					while (--num2 > 0);
					this.strstart++;
					this.lookahead--;
					this.prevAvailable = false;
					this.matchLen = 2;
				}
				else
				{
					if (this.prevAvailable)
					{
						this.huffman.TallyLit((int)(this.window[this.strstart - 1] & byte.MaxValue));
					}
					this.prevAvailable = true;
					this.strstart++;
					this.lookahead--;
				}
				if (this.huffman.IsFull())
				{
					int num4 = this.strstart - this.blockStart;
					if (this.prevAvailable)
					{
						num4--;
					}
					bool flag = finish && this.lookahead == 0 && !this.prevAvailable;
					this.huffman.FlushBlock(this.window, this.blockStart, num4, flag);
					this.blockStart += num4;
					return !flag;
				}
			}
			return true;
		}

		// Token: 0x04000047 RID: 71
		private const int TooFar = 4096;

		// Token: 0x04000048 RID: 72
		private int ins_h;

		// Token: 0x04000049 RID: 73
		private short[] head;

		// Token: 0x0400004A RID: 74
		private short[] prev;

		// Token: 0x0400004B RID: 75
		private int matchStart;

		// Token: 0x0400004C RID: 76
		private int matchLen;

		// Token: 0x0400004D RID: 77
		private bool prevAvailable;

		// Token: 0x0400004E RID: 78
		private int blockStart;

		// Token: 0x0400004F RID: 79
		private int strstart;

		// Token: 0x04000050 RID: 80
		private int lookahead;

		// Token: 0x04000051 RID: 81
		private byte[] window;

		// Token: 0x04000052 RID: 82
		private DeflateStrategy strategy;

		// Token: 0x04000053 RID: 83
		private int max_chain;

		// Token: 0x04000054 RID: 84
		private int max_lazy;

		// Token: 0x04000055 RID: 85
		private int niceLength;

		// Token: 0x04000056 RID: 86
		private int goodLength;

		// Token: 0x04000057 RID: 87
		private int compressionFunction;

		// Token: 0x04000058 RID: 88
		private byte[] inputBuf;

		// Token: 0x04000059 RID: 89
		private long totalIn;

		// Token: 0x0400005A RID: 90
		private int inputOff;

		// Token: 0x0400005B RID: 91
		private int inputEnd;

		// Token: 0x0400005C RID: 92
		private DeflaterPending pending;

		// Token: 0x0400005D RID: 93
		private DeflaterHuffman huffman;

		// Token: 0x0400005E RID: 94
		private Adler32 adler;
	}
}
