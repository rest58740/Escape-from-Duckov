using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Pathfinding.Ionic.Crc
{
	// Token: 0x0200006A RID: 106
	[ComVisible(true)]
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000C")]
	[ClassInterface(1)]
	public class CRC32
	{
		// Token: 0x06000482 RID: 1154 RVA: 0x0001F3D0 File Offset: 0x0001D5D0
		public CRC32() : this(false)
		{
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001F3DC File Offset: 0x0001D5DC
		public CRC32(bool reverseBits) : this(-306674912, reverseBits)
		{
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001F3EC File Offset: 0x0001D5EC
		public CRC32(int polynomial, bool reverseBits)
		{
			this.reverseBits = reverseBits;
			this.dwPolynomial = (uint)polynomial;
			this.GenerateLookupTable();
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0001F410 File Offset: 0x0001D610
		public long TotalBytesRead
		{
			get
			{
				return this._TotalBytesRead;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0001F418 File Offset: 0x0001D618
		public int Crc32Result
		{
			get
			{
				return (int)(~(int)this._register);
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001F424 File Offset: 0x0001D624
		public int GetCrc32(Stream input)
		{
			return this.GetCrc32AndCopy(input, null);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001F430 File Offset: 0x0001D630
		public int GetCrc32AndCopy(Stream input, Stream output)
		{
			if (input == null)
			{
				throw new Exception("The input stream must not be null.");
			}
			byte[] array = new byte[8192];
			int num = 8192;
			this._TotalBytesRead = 0L;
			int i = input.Read(array, 0, num);
			if (output != null)
			{
				output.Write(array, 0, i);
			}
			this._TotalBytesRead += (long)i;
			while (i > 0)
			{
				this.SlurpBlock(array, 0, i);
				i = input.Read(array, 0, num);
				if (output != null)
				{
					output.Write(array, 0, i);
				}
				this._TotalBytesRead += (long)i;
			}
			return (int)(~(int)this._register);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001F4D4 File Offset: 0x0001D6D4
		public int ComputeCrc32(int W, byte B)
		{
			return this._InternalComputeCrc32((uint)W, B);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001F4E0 File Offset: 0x0001D6E0
		internal int _InternalComputeCrc32(uint W, byte B)
		{
			return (int)(this.crc32Table[(int)((UIntPtr)((W ^ (uint)B) & 255U))] ^ W >> 8);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001F4F8 File Offset: 0x0001D6F8
		public void SlurpBlock(byte[] block, int offset, int count)
		{
			if (block == null)
			{
				throw new Exception("The data buffer must not be null.");
			}
			for (int i = 0; i < count; i++)
			{
				int num = offset + i;
				byte b = block[num];
				if (this.reverseBits)
				{
					uint num2 = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)num2)]);
				}
				else
				{
					uint num3 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)num3)]);
				}
			}
			this._TotalBytesRead += (long)count;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001F59C File Offset: 0x0001D79C
		public void UpdateCRC(byte b)
		{
			if (this.reverseBits)
			{
				uint num = this._register >> 24 ^ (uint)b;
				this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)num)]);
			}
			else
			{
				uint num2 = (this._register & 255U) ^ (uint)b;
				this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)num2)]);
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001F604 File Offset: 0x0001D804
		public void UpdateCRC(byte b, int n)
		{
			while (n-- > 0)
			{
				if (this.reverseBits)
				{
					uint num = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((UIntPtr)((num < 0U) ? (num + 256U) : num))]);
				}
				else
				{
					uint num2 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((UIntPtr)((num2 < 0U) ? (num2 + 256U) : num2))]);
				}
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001F6A4 File Offset: 0x0001D8A4
		private static uint ReverseBits(uint data)
		{
			uint num = (data & 1431655765U) << 1 | (data >> 1 & 1431655765U);
			num = ((num & 858993459U) << 2 | (num >> 2 & 858993459U));
			num = ((num & 252645135U) << 4 | (num >> 4 & 252645135U));
			return num << 24 | (num & 65280U) << 8 | (num >> 8 & 65280U) | num >> 24;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001F710 File Offset: 0x0001D910
		private static byte ReverseBits(byte data)
		{
			uint num = (uint)data * 131586U;
			uint num2 = 17055760U;
			uint num3 = num & num2;
			uint num4 = num << 2 & num2 << 1;
			return (byte)(16781313U * (num3 + num4) >> 24);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001F744 File Offset: 0x0001D944
		private void GenerateLookupTable()
		{
			this.crc32Table = new uint[256];
			byte b = 0;
			do
			{
				uint num = (uint)b;
				for (byte b2 = 8; b2 > 0; b2 -= 1)
				{
					if ((num & 1U) == 1U)
					{
						num = (num >> 1 ^ this.dwPolynomial);
					}
					else
					{
						num >>= 1;
					}
				}
				if (this.reverseBits)
				{
					this.crc32Table[(int)CRC32.ReverseBits(b)] = CRC32.ReverseBits(num);
				}
				else
				{
					this.crc32Table[(int)b] = num;
				}
				b += 1;
			}
			while (b != 0);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001F7CC File Offset: 0x0001D9CC
		private uint gf2_matrix_times(uint[] matrix, uint vec)
		{
			uint num = 0U;
			int num2 = 0;
			while (vec != 0U)
			{
				if ((vec & 1U) == 1U)
				{
					num ^= matrix[num2];
				}
				vec >>= 1;
				num2++;
			}
			return num;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001F804 File Offset: 0x0001DA04
		private void gf2_matrix_square(uint[] square, uint[] mat)
		{
			for (int i = 0; i < 32; i++)
			{
				square[i] = this.gf2_matrix_times(mat, mat[i]);
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001F834 File Offset: 0x0001DA34
		public void Combine(int crc, int length)
		{
			uint[] array = new uint[32];
			uint[] array2 = new uint[32];
			if (length == 0)
			{
				return;
			}
			uint num = ~this._register;
			array2[0] = this.dwPolynomial;
			uint num2 = 1U;
			for (int i = 1; i < 32; i++)
			{
				array2[i] = num2;
				num2 <<= 1;
			}
			this.gf2_matrix_square(array, array2);
			this.gf2_matrix_square(array2, array);
			uint num3 = (uint)length;
			do
			{
				this.gf2_matrix_square(array, array2);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array, num);
				}
				num3 >>= 1;
				if (num3 == 0U)
				{
					break;
				}
				this.gf2_matrix_square(array2, array);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array2, num);
				}
				num3 >>= 1;
			}
			while (num3 != 0U);
			num ^= (uint)crc;
			this._register = ~num;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001F908 File Offset: 0x0001DB08
		public void Reset()
		{
			this._register = uint.MaxValue;
		}

		// Token: 0x04000398 RID: 920
		private const int BUFFER_SIZE = 8192;

		// Token: 0x04000399 RID: 921
		private uint dwPolynomial;

		// Token: 0x0400039A RID: 922
		private long _TotalBytesRead;

		// Token: 0x0400039B RID: 923
		private bool reverseBits;

		// Token: 0x0400039C RID: 924
		private uint[] crc32Table;

		// Token: 0x0400039D RID: 925
		private uint _register = uint.MaxValue;
	}
}
