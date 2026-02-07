using System;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	// Token: 0x0200000F RID: 15
	public class Inflater
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00005C2C File Offset: 0x00003E2C
		public Inflater() : this(false)
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005C38 File Offset: 0x00003E38
		public Inflater(bool noHeader)
		{
			this.noHeader = noHeader;
			this.adler = new Adler32();
			this.input = new StreamManipulator();
			this.outputWindow = new OutputWindow();
			this.mode = ((!noHeader) ? 0 : 2);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public void Reset()
		{
			this.mode = ((!this.noHeader) ? 0 : 2);
			this.totalIn = 0L;
			this.totalOut = 0L;
			this.input.Reset();
			this.outputWindow.Reset();
			this.dynHeader = null;
			this.litlenTree = null;
			this.distTree = null;
			this.isLastBlock = false;
			this.adler.Reset();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005D68 File Offset: 0x00003F68
		private bool DecodeHeader()
		{
			int num = this.input.PeekBits(16);
			if (num < 0)
			{
				return false;
			}
			this.input.DropBits(16);
			num = ((num << 8 | num >> 8) & 65535);
			if (num % 31 != 0)
			{
				throw new SharpZipBaseException("Header checksum illegal");
			}
			if ((num & 3840) != 2048)
			{
				throw new SharpZipBaseException("Compression Method unknown");
			}
			if ((num & 32) == 0)
			{
				this.mode = 2;
			}
			else
			{
				this.mode = 1;
				this.neededBits = 32;
			}
			return true;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005DFC File Offset: 0x00003FFC
		private bool DecodeDict()
		{
			while (this.neededBits > 0)
			{
				int num = this.input.PeekBits(8);
				if (num < 0)
				{
					return false;
				}
				this.input.DropBits(8);
				this.readAdler = (this.readAdler << 8 | num);
				this.neededBits -= 8;
			}
			return false;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005E5C File Offset: 0x0000405C
		private bool DecodeHuffman()
		{
			int i = this.outputWindow.GetFreeSpace();
			while (i >= 258)
			{
				int symbol;
				switch (this.mode)
				{
				case 7:
					while (((symbol = this.litlenTree.GetSymbol(this.input)) & -256) == 0)
					{
						this.outputWindow.Write(symbol);
						if (--i < 258)
						{
							return true;
						}
					}
					if (symbol >= 257)
					{
						try
						{
							this.repLength = Inflater.CPLENS[symbol - 257];
							this.neededBits = Inflater.CPLEXT[symbol - 257];
						}
						catch (Exception)
						{
							throw new SharpZipBaseException("Illegal rep length code");
						}
						goto IL_E3;
					}
					if (symbol < 0)
					{
						return false;
					}
					this.distTree = null;
					this.litlenTree = null;
					this.mode = 2;
					return true;
				case 8:
					goto IL_E3;
				case 9:
					goto IL_13D;
				case 10:
					break;
				default:
					throw new SharpZipBaseException("Inflater unknown mode");
				}
				IL_18D:
				if (this.neededBits > 0)
				{
					this.mode = 10;
					int num = this.input.PeekBits(this.neededBits);
					if (num < 0)
					{
						return false;
					}
					this.input.DropBits(this.neededBits);
					this.repDist += num;
				}
				this.outputWindow.Repeat(this.repLength, this.repDist);
				i -= this.repLength;
				this.mode = 7;
				continue;
				IL_13D:
				symbol = this.distTree.GetSymbol(this.input);
				if (symbol < 0)
				{
					return false;
				}
				try
				{
					this.repDist = Inflater.CPDIST[symbol];
					this.neededBits = Inflater.CPDEXT[symbol];
				}
				catch (Exception)
				{
					throw new SharpZipBaseException("Illegal rep dist code");
				}
				goto IL_18D;
				IL_E3:
				if (this.neededBits > 0)
				{
					this.mode = 8;
					int num2 = this.input.PeekBits(this.neededBits);
					if (num2 < 0)
					{
						return false;
					}
					this.input.DropBits(this.neededBits);
					this.repLength += num2;
				}
				this.mode = 9;
				goto IL_13D;
			}
			return true;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000060BC File Offset: 0x000042BC
		private bool DecodeChksum()
		{
			while (this.neededBits > 0)
			{
				int num = this.input.PeekBits(8);
				if (num < 0)
				{
					return false;
				}
				this.input.DropBits(8);
				this.readAdler = (this.readAdler << 8 | num);
				this.neededBits -= 8;
			}
			if ((int)this.adler.Value != this.readAdler)
			{
				throw new SharpZipBaseException(string.Concat(new object[]
				{
					"Adler chksum doesn't match: ",
					(int)this.adler.Value,
					" vs. ",
					this.readAdler
				}));
			}
			this.mode = 12;
			return false;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00006180 File Offset: 0x00004380
		private bool Decode()
		{
			switch (this.mode)
			{
			case 0:
				return this.DecodeHeader();
			case 1:
				return this.DecodeDict();
			case 2:
				if (this.isLastBlock)
				{
					if (this.noHeader)
					{
						this.mode = 12;
						return false;
					}
					this.input.SkipToByteBoundary();
					this.neededBits = 32;
					this.mode = 11;
					return true;
				}
				else
				{
					int num = this.input.PeekBits(3);
					if (num < 0)
					{
						return false;
					}
					this.input.DropBits(3);
					if ((num & 1) != 0)
					{
						this.isLastBlock = true;
					}
					switch (num >> 1)
					{
					case 0:
						this.input.SkipToByteBoundary();
						this.mode = 3;
						break;
					case 1:
						this.litlenTree = InflaterHuffmanTree.defLitLenTree;
						this.distTree = InflaterHuffmanTree.defDistTree;
						this.mode = 7;
						break;
					case 2:
						this.dynHeader = new InflaterDynHeader();
						this.mode = 6;
						break;
					default:
						throw new SharpZipBaseException("Unknown block type " + num);
					}
					return true;
				}
				break;
			case 3:
				if ((this.uncomprLen = this.input.PeekBits(16)) < 0)
				{
					return false;
				}
				this.input.DropBits(16);
				this.mode = 4;
				break;
			case 4:
				break;
			case 5:
				goto IL_1D4;
			case 6:
				if (!this.dynHeader.Decode(this.input))
				{
					return false;
				}
				this.litlenTree = this.dynHeader.BuildLitLenTree();
				this.distTree = this.dynHeader.BuildDistTree();
				this.mode = 7;
				goto IL_263;
			case 7:
			case 8:
			case 9:
			case 10:
				goto IL_263;
			case 11:
				return this.DecodeChksum();
			case 12:
				return false;
			default:
				throw new SharpZipBaseException("Inflater.Decode unknown mode");
			}
			int num2 = this.input.PeekBits(16);
			if (num2 < 0)
			{
				return false;
			}
			this.input.DropBits(16);
			if (num2 != (this.uncomprLen ^ 65535))
			{
				throw new SharpZipBaseException("broken uncompressed block");
			}
			this.mode = 5;
			IL_1D4:
			int num3 = this.outputWindow.CopyStored(this.input, this.uncomprLen);
			this.uncomprLen -= num3;
			if (this.uncomprLen == 0)
			{
				this.mode = 2;
				return true;
			}
			return !this.input.IsNeedingInput;
			IL_263:
			return this.DecodeHuffman();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00006404 File Offset: 0x00004604
		public void SetDictionary(byte[] buffer)
		{
			this.SetDictionary(buffer, 0, buffer.Length);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00006414 File Offset: 0x00004614
		public void SetDictionary(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (!this.IsNeedingDictionary)
			{
				throw new InvalidOperationException("Dictionary is not needed");
			}
			this.adler.Update(buffer, index, count);
			if ((int)this.adler.Value != this.readAdler)
			{
				throw new SharpZipBaseException("Wrong adler checksum");
			}
			this.adler.Reset();
			this.outputWindow.CopyDict(buffer, index, count);
			this.mode = 2;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000064BC File Offset: 0x000046BC
		public void SetInput(byte[] buffer)
		{
			this.SetInput(buffer, 0, buffer.Length);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000064CC File Offset: 0x000046CC
		public void SetInput(byte[] buffer, int index, int count)
		{
			this.input.SetInput(buffer, index, count);
			this.totalIn += (long)count;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000064EC File Offset: 0x000046EC
		public int Inflate(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			return this.Inflate(buffer, 0, buffer.Length);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000650C File Offset: 0x0000470C
		public int Inflate(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "count cannot be negative");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "offset cannot be negative");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentException("count exceeds buffer bounds");
			}
			if (count == 0)
			{
				if (!this.IsFinished)
				{
					this.Decode();
				}
				return 0;
			}
			int num = 0;
			for (;;)
			{
				if (this.mode != 11)
				{
					int num2 = this.outputWindow.CopyOutput(buffer, offset, count);
					if (num2 > 0)
					{
						this.adler.Update(buffer, offset, num2);
						offset += num2;
						num += num2;
						this.totalOut += (long)num2;
						count -= num2;
						if (count == 0)
						{
							break;
						}
					}
				}
				if (!this.Decode() && (this.outputWindow.GetAvailable() <= 0 || this.mode == 11))
				{
					return num;
				}
			}
			return num;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AC RID: 172 RVA: 0x0000660C File Offset: 0x0000480C
		public bool IsNeedingInput
		{
			get
			{
				return this.input.IsNeedingInput;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000661C File Offset: 0x0000481C
		public bool IsNeedingDictionary
		{
			get
			{
				return this.mode == 1 && this.neededBits == 0;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00006638 File Offset: 0x00004838
		public bool IsFinished
		{
			get
			{
				return this.mode == 12 && this.outputWindow.GetAvailable() == 0;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00006658 File Offset: 0x00004858
		public int Adler
		{
			get
			{
				return (!this.IsNeedingDictionary) ? ((int)this.adler.Value) : this.readAdler;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00006688 File Offset: 0x00004888
		public long TotalOut
		{
			get
			{
				return this.totalOut;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00006690 File Offset: 0x00004890
		public long TotalIn
		{
			get
			{
				return this.totalIn - (long)this.RemainingInput;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000066A0 File Offset: 0x000048A0
		public int RemainingInput
		{
			get
			{
				return this.input.AvailableBytes;
			}
		}

		// Token: 0x0400007F RID: 127
		private const int DECODE_HEADER = 0;

		// Token: 0x04000080 RID: 128
		private const int DECODE_DICT = 1;

		// Token: 0x04000081 RID: 129
		private const int DECODE_BLOCKS = 2;

		// Token: 0x04000082 RID: 130
		private const int DECODE_STORED_LEN1 = 3;

		// Token: 0x04000083 RID: 131
		private const int DECODE_STORED_LEN2 = 4;

		// Token: 0x04000084 RID: 132
		private const int DECODE_STORED = 5;

		// Token: 0x04000085 RID: 133
		private const int DECODE_DYN_HEADER = 6;

		// Token: 0x04000086 RID: 134
		private const int DECODE_HUFFMAN = 7;

		// Token: 0x04000087 RID: 135
		private const int DECODE_HUFFMAN_LENBITS = 8;

		// Token: 0x04000088 RID: 136
		private const int DECODE_HUFFMAN_DIST = 9;

		// Token: 0x04000089 RID: 137
		private const int DECODE_HUFFMAN_DISTBITS = 10;

		// Token: 0x0400008A RID: 138
		private const int DECODE_CHKSUM = 11;

		// Token: 0x0400008B RID: 139
		private const int FINISHED = 12;

		// Token: 0x0400008C RID: 140
		private static readonly int[] CPLENS = new int[]
		{
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			13,
			15,
			17,
			19,
			23,
			27,
			31,
			35,
			43,
			51,
			59,
			67,
			83,
			99,
			115,
			131,
			163,
			195,
			227,
			258
		};

		// Token: 0x0400008D RID: 141
		private static readonly int[] CPLEXT = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			2,
			2,
			2,
			2,
			3,
			3,
			3,
			3,
			4,
			4,
			4,
			4,
			5,
			5,
			5,
			5,
			0
		};

		// Token: 0x0400008E RID: 142
		private static readonly int[] CPDIST = new int[]
		{
			1,
			2,
			3,
			4,
			5,
			7,
			9,
			13,
			17,
			25,
			33,
			49,
			65,
			97,
			129,
			193,
			257,
			385,
			513,
			769,
			1025,
			1537,
			2049,
			3073,
			4097,
			6145,
			8193,
			12289,
			16385,
			24577
		};

		// Token: 0x0400008F RID: 143
		private static readonly int[] CPDEXT = new int[]
		{
			0,
			0,
			0,
			0,
			1,
			1,
			2,
			2,
			3,
			3,
			4,
			4,
			5,
			5,
			6,
			6,
			7,
			7,
			8,
			8,
			9,
			9,
			10,
			10,
			11,
			11,
			12,
			12,
			13,
			13
		};

		// Token: 0x04000090 RID: 144
		private int mode;

		// Token: 0x04000091 RID: 145
		private int readAdler;

		// Token: 0x04000092 RID: 146
		private int neededBits;

		// Token: 0x04000093 RID: 147
		private int repLength;

		// Token: 0x04000094 RID: 148
		private int repDist;

		// Token: 0x04000095 RID: 149
		private int uncomprLen;

		// Token: 0x04000096 RID: 150
		private bool isLastBlock;

		// Token: 0x04000097 RID: 151
		private long totalOut;

		// Token: 0x04000098 RID: 152
		private long totalIn;

		// Token: 0x04000099 RID: 153
		private bool noHeader;

		// Token: 0x0400009A RID: 154
		private StreamManipulator input;

		// Token: 0x0400009B RID: 155
		private OutputWindow outputWindow;

		// Token: 0x0400009C RID: 156
		private InflaterDynHeader dynHeader;

		// Token: 0x0400009D RID: 157
		private InflaterHuffmanTree litlenTree;

		// Token: 0x0400009E RID: 158
		private InflaterHuffmanTree distTree;

		// Token: 0x0400009F RID: 159
		private Adler32 adler;
	}
}
