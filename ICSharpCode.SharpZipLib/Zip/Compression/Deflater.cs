using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x02000010 RID: 16
	public class Deflater
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x000066B0 File Offset: 0x000048B0
		public Deflater() : this(-1, false)
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000066BC File Offset: 0x000048BC
		public Deflater(int level) : this(level, false)
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000066C8 File Offset: 0x000048C8
		public Deflater(int level, bool noZlibHeaderOrFooter)
		{
			if (level == -1)
			{
				level = 6;
			}
			else if (level < 0 || level > 9)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			this.pending = new DeflaterPending();
			this.engine = new DeflaterEngine(this.pending);
			this.noZlibHeaderOrFooter = noZlibHeaderOrFooter;
			this.SetStrategy(DeflateStrategy.Default);
			this.SetLevel(level);
			this.Reset();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000673C File Offset: 0x0000493C
		public void Reset()
		{
			this.state = ((!this.noZlibHeaderOrFooter) ? 0 : 16);
			this.totalOut = 0L;
			this.pending.Reset();
			this.engine.Reset();
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00006778 File Offset: 0x00004978
		public int Adler
		{
			get
			{
				return this.engine.Adler;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00006788 File Offset: 0x00004988
		public long TotalIn
		{
			get
			{
				return this.engine.TotalIn;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00006798 File Offset: 0x00004998
		public long TotalOut
		{
			get
			{
				return this.totalOut;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000067A0 File Offset: 0x000049A0
		public void Flush()
		{
			this.state |= 4;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000067B0 File Offset: 0x000049B0
		public void Finish()
		{
			this.state |= 12;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000067C4 File Offset: 0x000049C4
		public bool IsFinished
		{
			get
			{
				return this.state == 30 && this.pending.IsFlushed;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000067E4 File Offset: 0x000049E4
		public bool IsNeedingInput
		{
			get
			{
				return this.engine.NeedsInput();
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000067F4 File Offset: 0x000049F4
		public void SetInput(byte[] input)
		{
			this.SetInput(input, 0, input.Length);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006804 File Offset: 0x00004A04
		public void SetInput(byte[] input, int offset, int count)
		{
			if ((this.state & 8) != 0)
			{
				throw new InvalidOperationException("Finish() already called");
			}
			this.engine.SetInput(input, offset, count);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006838 File Offset: 0x00004A38
		public void SetLevel(int level)
		{
			if (level == -1)
			{
				level = 6;
			}
			else if (level < 0 || level > 9)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			if (this.level != level)
			{
				this.level = level;
				this.engine.SetLevel(level);
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006890 File Offset: 0x00004A90
		public int GetLevel()
		{
			return this.level;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006898 File Offset: 0x00004A98
		public void SetStrategy(DeflateStrategy strategy)
		{
			this.engine.Strategy = strategy;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000068A8 File Offset: 0x00004AA8
		public int Deflate(byte[] output)
		{
			return this.Deflate(output, 0, output.Length);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000068B8 File Offset: 0x00004AB8
		public int Deflate(byte[] output, int offset, int length)
		{
			int num = length;
			if (this.state == 127)
			{
				throw new InvalidOperationException("Deflater closed");
			}
			if (this.state < 16)
			{
				int num2 = 30720;
				int num3 = this.level - 1 >> 1;
				if (num3 < 0 || num3 > 3)
				{
					num3 = 3;
				}
				num2 |= num3 << 6;
				if ((this.state & 1) != 0)
				{
					num2 |= 32;
				}
				num2 += 31 - num2 % 31;
				this.pending.WriteShortMSB(num2);
				if ((this.state & 1) != 0)
				{
					int adler = this.engine.Adler;
					this.engine.ResetAdler();
					this.pending.WriteShortMSB(adler >> 16);
					this.pending.WriteShortMSB(adler & 65535);
				}
				this.state = (16 | (this.state & 12));
			}
			for (;;)
			{
				int num4 = this.pending.Flush(output, offset, length);
				offset += num4;
				this.totalOut += (long)num4;
				length -= num4;
				if (length == 0 || this.state == 30)
				{
					break;
				}
				if (!this.engine.Deflate((this.state & 4) != 0, (this.state & 8) != 0))
				{
					if (this.state == 16)
					{
						goto Block_8;
					}
					if (this.state == 20)
					{
						if (this.level != 0)
						{
							for (int i = 8 + (-this.pending.BitCount & 7); i > 0; i -= 10)
							{
								this.pending.WriteBits(2, 10);
							}
						}
						this.state = 16;
					}
					else if (this.state == 28)
					{
						this.pending.AlignToByte();
						if (!this.noZlibHeaderOrFooter)
						{
							int adler2 = this.engine.Adler;
							this.pending.WriteShortMSB(adler2 >> 16);
							this.pending.WriteShortMSB(adler2 & 65535);
						}
						this.state = 30;
					}
				}
			}
			return num - length;
			Block_8:
			return num - length;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006AD4 File Offset: 0x00004CD4
		public void SetDictionary(byte[] dictionary)
		{
			this.SetDictionary(dictionary, 0, dictionary.Length);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006AE4 File Offset: 0x00004CE4
		public void SetDictionary(byte[] dictionary, int index, int count)
		{
			if (this.state != 0)
			{
				throw new InvalidOperationException();
			}
			this.state = 1;
			this.engine.SetDictionary(dictionary, index, count);
		}

		// Token: 0x040000A0 RID: 160
		public const int BEST_COMPRESSION = 9;

		// Token: 0x040000A1 RID: 161
		public const int BEST_SPEED = 1;

		// Token: 0x040000A2 RID: 162
		public const int DEFAULT_COMPRESSION = -1;

		// Token: 0x040000A3 RID: 163
		public const int NO_COMPRESSION = 0;

		// Token: 0x040000A4 RID: 164
		public const int DEFLATED = 8;

		// Token: 0x040000A5 RID: 165
		private const int IS_SETDICT = 1;

		// Token: 0x040000A6 RID: 166
		private const int IS_FLUSHING = 4;

		// Token: 0x040000A7 RID: 167
		private const int IS_FINISHING = 8;

		// Token: 0x040000A8 RID: 168
		private const int INIT_STATE = 0;

		// Token: 0x040000A9 RID: 169
		private const int SETDICT_STATE = 1;

		// Token: 0x040000AA RID: 170
		private const int BUSY_STATE = 16;

		// Token: 0x040000AB RID: 171
		private const int FLUSHING_STATE = 20;

		// Token: 0x040000AC RID: 172
		private const int FINISHING_STATE = 28;

		// Token: 0x040000AD RID: 173
		private const int FINISHED_STATE = 30;

		// Token: 0x040000AE RID: 174
		private const int CLOSED_STATE = 127;

		// Token: 0x040000AF RID: 175
		private int level;

		// Token: 0x040000B0 RID: 176
		private bool noZlibHeaderOrFooter;

		// Token: 0x040000B1 RID: 177
		private int state;

		// Token: 0x040000B2 RID: 178
		private long totalOut;

		// Token: 0x040000B3 RID: 179
		private DeflaterPending pending;

		// Token: 0x040000B4 RID: 180
		private DeflaterEngine engine;
	}
}
