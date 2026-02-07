using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Sirenix.Serialization
{
	// Token: 0x02000071 RID: 113
	public static class ProperBitConverter
	{
		// Token: 0x06000394 RID: 916 RVA: 0x0001955C File Offset: 0x0001775C
		private static uint[] CreateByteToHexLookup(bool upperCase)
		{
			uint[] array = new uint[256];
			if (upperCase)
			{
				for (int i = 0; i < 256; i++)
				{
					string text = i.ToString("X2", CultureInfo.InvariantCulture);
					array[i] = (uint)(text.get_Chars(0) + ((uint)text.get_Chars(1) << 16));
				}
			}
			else
			{
				for (int j = 0; j < 256; j++)
				{
					string text2 = j.ToString("x2", CultureInfo.InvariantCulture);
					array[j] = (uint)(text2.get_Chars(0) + ((uint)text2.get_Chars(1) << 16));
				}
			}
			return array;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000195EC File Offset: 0x000177EC
		public static string BytesToHexString(byte[] bytes, bool lowerCaseHexChars = true)
		{
			uint[] array = lowerCaseHexChars ? ProperBitConverter.ByteToHexCharLookupLowerCase : ProperBitConverter.ByteToHexCharLookupUpperCase;
			char[] array2 = new char[bytes.Length * 2];
			for (int i = 0; i < bytes.Length; i++)
			{
				int num = i * 2;
				uint num2 = array[(int)bytes[i]];
				array2[num] = (char)num2;
				array2[num + 1] = (char)(num2 >> 16);
			}
			return new string(array2);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00019644 File Offset: 0x00017844
		public static byte[] HexStringToBytes(string hex)
		{
			int length = hex.Length;
			int num = length / 2;
			if (length % 2 != 0)
			{
				throw new ArgumentException("Hex string must have an even length.");
			}
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = i * 2;
				byte b;
				try
				{
					b = ProperBitConverter.HexToByteLookup[(int)hex.get_Chars(num2)];
					if (b == 255)
					{
						throw new ArgumentException(string.Concat(new string[]
						{
							"Expected a hex character, got '",
							hex.get_Chars(num2).ToString(),
							"' at string index '",
							num2.ToString(),
							"'."
						}));
					}
				}
				catch (IndexOutOfRangeException)
				{
					throw new ArgumentException(string.Concat(new string[]
					{
						"Expected a hex character, got '",
						hex.get_Chars(num2).ToString(),
						"' at string index '",
						num2.ToString(),
						"'."
					}));
				}
				byte b2;
				try
				{
					b2 = ProperBitConverter.HexToByteLookup[(int)hex.get_Chars(num2 + 1)];
					if (b2 == 255)
					{
						throw new ArgumentException(string.Concat(new string[]
						{
							"Expected a hex character, got '",
							hex.get_Chars(num2 + 1).ToString(),
							"' at string index '",
							(num2 + 1).ToString(),
							"'."
						}));
					}
				}
				catch (IndexOutOfRangeException)
				{
					throw new ArgumentException(string.Concat(new string[]
					{
						"Expected a hex character, got '",
						hex.get_Chars(num2 + 1).ToString(),
						"' at string index '",
						(num2 + 1).ToString(),
						"'."
					}));
				}
				array[i] = (byte)((int)b << 4 | (int)b2);
			}
			return array;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001981C File Offset: 0x00017A1C
		public static short ToInt16(byte[] buffer, int index)
		{
			short num = 0;
			num |= (short)buffer[index + 1];
			num = (short)(num << 8);
			return num | (short)buffer[index];
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00019844 File Offset: 0x00017A44
		public static ushort ToUInt16(byte[] buffer, int index)
		{
			ushort num = 0;
			num |= (ushort)buffer[index + 1];
			num = (ushort)(num << 8);
			return num | (ushort)buffer[index];
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001986C File Offset: 0x00017A6C
		public static int ToInt32(byte[] buffer, int index)
		{
			int num = 0;
			num |= (int)buffer[index + 3];
			num <<= 8;
			num |= (int)buffer[index + 2];
			num <<= 8;
			num |= (int)buffer[index + 1];
			num <<= 8;
			return num | (int)buffer[index];
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000198A8 File Offset: 0x00017AA8
		public static uint ToUInt32(byte[] buffer, int index)
		{
			uint num = 0U;
			num |= (uint)buffer[index + 3];
			num <<= 8;
			num |= (uint)buffer[index + 2];
			num <<= 8;
			num |= (uint)buffer[index + 1];
			num <<= 8;
			return num | (uint)buffer[index];
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000198E4 File Offset: 0x00017AE4
		public static long ToInt64(byte[] buffer, int index)
		{
			long num = 0L;
			num |= (long)((ulong)buffer[index + 7]);
			num <<= 8;
			num |= (long)((ulong)buffer[index + 6]);
			num <<= 8;
			num |= (long)((ulong)buffer[index + 5]);
			num <<= 8;
			num |= (long)((ulong)buffer[index + 4]);
			num <<= 8;
			num |= (long)((ulong)buffer[index + 3]);
			num <<= 8;
			num |= (long)((ulong)buffer[index + 2]);
			num <<= 8;
			num |= (long)((ulong)buffer[index + 1]);
			num <<= 8;
			return num | (long)((ulong)buffer[index]);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00019958 File Offset: 0x00017B58
		public static ulong ToUInt64(byte[] buffer, int index)
		{
			ulong num = 0UL;
			num |= (ulong)buffer[index + 7];
			num <<= 8;
			num |= (ulong)buffer[index + 6];
			num <<= 8;
			num |= (ulong)buffer[index + 5];
			num <<= 8;
			num |= (ulong)buffer[index + 4];
			num <<= 8;
			num |= (ulong)buffer[index + 3];
			num <<= 8;
			num |= (ulong)buffer[index + 2];
			num <<= 8;
			num |= (ulong)buffer[index + 1];
			num <<= 8;
			return num | (ulong)buffer[index];
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000199CC File Offset: 0x00017BCC
		public static float ToSingle(byte[] buffer, int index)
		{
			ProperBitConverter.SingleByteUnion singleByteUnion = default(ProperBitConverter.SingleByteUnion);
			if (BitConverter.IsLittleEndian)
			{
				singleByteUnion.Byte0 = buffer[index];
				singleByteUnion.Byte1 = buffer[index + 1];
				singleByteUnion.Byte2 = buffer[index + 2];
				singleByteUnion.Byte3 = buffer[index + 3];
			}
			else
			{
				singleByteUnion.Byte3 = buffer[index];
				singleByteUnion.Byte2 = buffer[index + 1];
				singleByteUnion.Byte1 = buffer[index + 2];
				singleByteUnion.Byte0 = buffer[index + 3];
			}
			return singleByteUnion.Value;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00019A4C File Offset: 0x00017C4C
		public static double ToDouble(byte[] buffer, int index)
		{
			ProperBitConverter.DoubleByteUnion doubleByteUnion = default(ProperBitConverter.DoubleByteUnion);
			if (BitConverter.IsLittleEndian)
			{
				doubleByteUnion.Byte0 = buffer[index];
				doubleByteUnion.Byte1 = buffer[index + 1];
				doubleByteUnion.Byte2 = buffer[index + 2];
				doubleByteUnion.Byte3 = buffer[index + 3];
				doubleByteUnion.Byte4 = buffer[index + 4];
				doubleByteUnion.Byte5 = buffer[index + 5];
				doubleByteUnion.Byte6 = buffer[index + 6];
				doubleByteUnion.Byte7 = buffer[index + 7];
			}
			else
			{
				doubleByteUnion.Byte7 = buffer[index];
				doubleByteUnion.Byte6 = buffer[index + 1];
				doubleByteUnion.Byte5 = buffer[index + 2];
				doubleByteUnion.Byte4 = buffer[index + 3];
				doubleByteUnion.Byte3 = buffer[index + 4];
				doubleByteUnion.Byte2 = buffer[index + 5];
				doubleByteUnion.Byte1 = buffer[index + 6];
				doubleByteUnion.Byte0 = buffer[index + 7];
			}
			return doubleByteUnion.Value;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00019B2C File Offset: 0x00017D2C
		public static decimal ToDecimal(byte[] buffer, int index)
		{
			ProperBitConverter.DecimalByteUnion decimalByteUnion = default(ProperBitConverter.DecimalByteUnion);
			if (BitConverter.IsLittleEndian)
			{
				decimalByteUnion.Byte0 = buffer[index];
				decimalByteUnion.Byte1 = buffer[index + 1];
				decimalByteUnion.Byte2 = buffer[index + 2];
				decimalByteUnion.Byte3 = buffer[index + 3];
				decimalByteUnion.Byte4 = buffer[index + 4];
				decimalByteUnion.Byte5 = buffer[index + 5];
				decimalByteUnion.Byte6 = buffer[index + 6];
				decimalByteUnion.Byte7 = buffer[index + 7];
				decimalByteUnion.Byte8 = buffer[index + 8];
				decimalByteUnion.Byte9 = buffer[index + 9];
				decimalByteUnion.Byte10 = buffer[index + 10];
				decimalByteUnion.Byte11 = buffer[index + 11];
				decimalByteUnion.Byte12 = buffer[index + 12];
				decimalByteUnion.Byte13 = buffer[index + 13];
				decimalByteUnion.Byte14 = buffer[index + 14];
				decimalByteUnion.Byte15 = buffer[index + 15];
			}
			else
			{
				decimalByteUnion.Byte15 = buffer[index];
				decimalByteUnion.Byte14 = buffer[index + 1];
				decimalByteUnion.Byte13 = buffer[index + 2];
				decimalByteUnion.Byte12 = buffer[index + 3];
				decimalByteUnion.Byte11 = buffer[index + 4];
				decimalByteUnion.Byte10 = buffer[index + 5];
				decimalByteUnion.Byte9 = buffer[index + 6];
				decimalByteUnion.Byte8 = buffer[index + 7];
				decimalByteUnion.Byte7 = buffer[index + 8];
				decimalByteUnion.Byte6 = buffer[index + 9];
				decimalByteUnion.Byte5 = buffer[index + 10];
				decimalByteUnion.Byte4 = buffer[index + 11];
				decimalByteUnion.Byte3 = buffer[index + 12];
				decimalByteUnion.Byte2 = buffer[index + 13];
				decimalByteUnion.Byte1 = buffer[index + 14];
				decimalByteUnion.Byte0 = buffer[index + 15];
			}
			return decimalByteUnion.Value;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00019CE0 File Offset: 0x00017EE0
		public static Guid ToGuid(byte[] buffer, int index)
		{
			ProperBitConverter.GuidByteUnion guidByteUnion = default(ProperBitConverter.GuidByteUnion);
			guidByteUnion.Byte0 = buffer[index];
			guidByteUnion.Byte1 = buffer[index + 1];
			guidByteUnion.Byte2 = buffer[index + 2];
			guidByteUnion.Byte3 = buffer[index + 3];
			guidByteUnion.Byte4 = buffer[index + 4];
			guidByteUnion.Byte5 = buffer[index + 5];
			guidByteUnion.Byte6 = buffer[index + 6];
			guidByteUnion.Byte7 = buffer[index + 7];
			guidByteUnion.Byte8 = buffer[index + 8];
			guidByteUnion.Byte9 = buffer[index + 9];
			if (BitConverter.IsLittleEndian)
			{
				guidByteUnion.Byte10 = buffer[index + 10];
				guidByteUnion.Byte11 = buffer[index + 11];
				guidByteUnion.Byte12 = buffer[index + 12];
				guidByteUnion.Byte13 = buffer[index + 13];
				guidByteUnion.Byte14 = buffer[index + 14];
				guidByteUnion.Byte15 = buffer[index + 15];
			}
			else
			{
				guidByteUnion.Byte15 = buffer[index + 10];
				guidByteUnion.Byte14 = buffer[index + 11];
				guidByteUnion.Byte13 = buffer[index + 12];
				guidByteUnion.Byte12 = buffer[index + 13];
				guidByteUnion.Byte11 = buffer[index + 14];
				guidByteUnion.Byte10 = buffer[index + 15];
			}
			return guidByteUnion.Value;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00019E17 File Offset: 0x00018017
		public static void GetBytes(byte[] buffer, int index, short value)
		{
			if (BitConverter.IsLittleEndian)
			{
				buffer[index] = (byte)value;
				buffer[index + 1] = (byte)(value >> 8);
				return;
			}
			buffer[index] = (byte)(value >> 8);
			buffer[index + 1] = (byte)value;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00019E17 File Offset: 0x00018017
		public static void GetBytes(byte[] buffer, int index, ushort value)
		{
			if (BitConverter.IsLittleEndian)
			{
				buffer[index] = (byte)value;
				buffer[index + 1] = (byte)(value >> 8);
				return;
			}
			buffer[index] = (byte)(value >> 8);
			buffer[index + 1] = (byte)value;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00019E40 File Offset: 0x00018040
		public static void GetBytes(byte[] buffer, int index, int value)
		{
			if (BitConverter.IsLittleEndian)
			{
				buffer[index] = (byte)value;
				buffer[index + 1] = (byte)(value >> 8);
				buffer[index + 2] = (byte)(value >> 16);
				buffer[index + 3] = (byte)(value >> 24);
				return;
			}
			buffer[index] = (byte)(value >> 24);
			buffer[index + 1] = (byte)(value >> 16);
			buffer[index + 2] = (byte)(value >> 8);
			buffer[index + 3] = (byte)value;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00019E9C File Offset: 0x0001809C
		public static void GetBytes(byte[] buffer, int index, uint value)
		{
			if (BitConverter.IsLittleEndian)
			{
				buffer[index] = (byte)value;
				buffer[index + 1] = (byte)(value >> 8);
				buffer[index + 2] = (byte)(value >> 16);
				buffer[index + 3] = (byte)(value >> 24);
				return;
			}
			buffer[index] = (byte)(value >> 24);
			buffer[index + 1] = (byte)(value >> 16);
			buffer[index + 2] = (byte)(value >> 8);
			buffer[index + 3] = (byte)value;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00019EF8 File Offset: 0x000180F8
		public static void GetBytes(byte[] buffer, int index, long value)
		{
			if (BitConverter.IsLittleEndian)
			{
				buffer[index] = (byte)value;
				buffer[index + 1] = (byte)(value >> 8);
				buffer[index + 2] = (byte)(value >> 16);
				buffer[index + 3] = (byte)(value >> 24);
				buffer[index + 4] = (byte)(value >> 32);
				buffer[index + 5] = (byte)(value >> 40);
				buffer[index + 6] = (byte)(value >> 48);
				buffer[index + 7] = (byte)(value >> 56);
				return;
			}
			buffer[index] = (byte)(value >> 56);
			buffer[index + 1] = (byte)(value >> 48);
			buffer[index + 2] = (byte)(value >> 40);
			buffer[index + 3] = (byte)(value >> 32);
			buffer[index + 4] = (byte)(value >> 24);
			buffer[index + 5] = (byte)(value >> 16);
			buffer[index + 6] = (byte)(value >> 8);
			buffer[index + 7] = (byte)value;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00019FA4 File Offset: 0x000181A4
		public static void GetBytes(byte[] buffer, int index, ulong value)
		{
			if (BitConverter.IsLittleEndian)
			{
				buffer[index] = (byte)value;
				buffer[index + 1] = (byte)(value >> 8);
				buffer[index + 2] = (byte)(value >> 16);
				buffer[index + 3] = (byte)(value >> 24);
				buffer[index + 4] = (byte)(value >> 32);
				buffer[index + 5] = (byte)(value >> 40);
				buffer[index + 6] = (byte)(value >> 48);
				buffer[index + 7] = (byte)(value >> 56);
				return;
			}
			buffer[index] = (byte)(value >> 56);
			buffer[index + 1] = (byte)(value >> 48);
			buffer[index + 2] = (byte)(value >> 40);
			buffer[index + 3] = (byte)(value >> 32);
			buffer[index + 4] = (byte)(value >> 24);
			buffer[index + 5] = (byte)(value >> 16);
			buffer[index + 6] = (byte)(value >> 8);
			buffer[index + 7] = (byte)value;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001A050 File Offset: 0x00018250
		public static void GetBytes(byte[] buffer, int index, float value)
		{
			ProperBitConverter.SingleByteUnion singleByteUnion = default(ProperBitConverter.SingleByteUnion);
			singleByteUnion.Value = value;
			if (BitConverter.IsLittleEndian)
			{
				buffer[index] = singleByteUnion.Byte0;
				buffer[index + 1] = singleByteUnion.Byte1;
				buffer[index + 2] = singleByteUnion.Byte2;
				buffer[index + 3] = singleByteUnion.Byte3;
				return;
			}
			buffer[index] = singleByteUnion.Byte3;
			buffer[index + 1] = singleByteUnion.Byte2;
			buffer[index + 2] = singleByteUnion.Byte1;
			buffer[index + 3] = singleByteUnion.Byte0;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001A0CC File Offset: 0x000182CC
		public static void GetBytes(byte[] buffer, int index, double value)
		{
			ProperBitConverter.DoubleByteUnion doubleByteUnion = default(ProperBitConverter.DoubleByteUnion);
			doubleByteUnion.Value = value;
			if (BitConverter.IsLittleEndian)
			{
				buffer[index] = doubleByteUnion.Byte0;
				buffer[index + 1] = doubleByteUnion.Byte1;
				buffer[index + 2] = doubleByteUnion.Byte2;
				buffer[index + 3] = doubleByteUnion.Byte3;
				buffer[index + 4] = doubleByteUnion.Byte4;
				buffer[index + 5] = doubleByteUnion.Byte5;
				buffer[index + 6] = doubleByteUnion.Byte6;
				buffer[index + 7] = doubleByteUnion.Byte7;
				return;
			}
			buffer[index] = doubleByteUnion.Byte7;
			buffer[index + 1] = doubleByteUnion.Byte6;
			buffer[index + 2] = doubleByteUnion.Byte5;
			buffer[index + 3] = doubleByteUnion.Byte4;
			buffer[index + 4] = doubleByteUnion.Byte3;
			buffer[index + 5] = doubleByteUnion.Byte2;
			buffer[index + 6] = doubleByteUnion.Byte1;
			buffer[index + 7] = doubleByteUnion.Byte0;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001A1A0 File Offset: 0x000183A0
		public static void GetBytes(byte[] buffer, int index, decimal value)
		{
			ProperBitConverter.DecimalByteUnion decimalByteUnion = default(ProperBitConverter.DecimalByteUnion);
			decimalByteUnion.Value = value;
			if (BitConverter.IsLittleEndian)
			{
				buffer[index] = decimalByteUnion.Byte0;
				buffer[index + 1] = decimalByteUnion.Byte1;
				buffer[index + 2] = decimalByteUnion.Byte2;
				buffer[index + 3] = decimalByteUnion.Byte3;
				buffer[index + 4] = decimalByteUnion.Byte4;
				buffer[index + 5] = decimalByteUnion.Byte5;
				buffer[index + 6] = decimalByteUnion.Byte6;
				buffer[index + 7] = decimalByteUnion.Byte7;
				buffer[index + 8] = decimalByteUnion.Byte8;
				buffer[index + 9] = decimalByteUnion.Byte9;
				buffer[index + 10] = decimalByteUnion.Byte10;
				buffer[index + 11] = decimalByteUnion.Byte11;
				buffer[index + 12] = decimalByteUnion.Byte12;
				buffer[index + 13] = decimalByteUnion.Byte13;
				buffer[index + 14] = decimalByteUnion.Byte14;
				buffer[index + 15] = decimalByteUnion.Byte15;
				return;
			}
			buffer[index] = decimalByteUnion.Byte15;
			buffer[index + 1] = decimalByteUnion.Byte14;
			buffer[index + 2] = decimalByteUnion.Byte13;
			buffer[index + 3] = decimalByteUnion.Byte12;
			buffer[index + 4] = decimalByteUnion.Byte11;
			buffer[index + 5] = decimalByteUnion.Byte10;
			buffer[index + 6] = decimalByteUnion.Byte9;
			buffer[index + 7] = decimalByteUnion.Byte8;
			buffer[index + 8] = decimalByteUnion.Byte7;
			buffer[index + 9] = decimalByteUnion.Byte6;
			buffer[index + 10] = decimalByteUnion.Byte5;
			buffer[index + 11] = decimalByteUnion.Byte4;
			buffer[index + 12] = decimalByteUnion.Byte3;
			buffer[index + 13] = decimalByteUnion.Byte2;
			buffer[index + 14] = decimalByteUnion.Byte1;
			buffer[index + 15] = decimalByteUnion.Byte0;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001A334 File Offset: 0x00018534
		public static void GetBytes(byte[] buffer, int index, Guid value)
		{
			ProperBitConverter.GuidByteUnion guidByteUnion = new ProperBitConverter.GuidByteUnion
			{
				Value = value
			};
			buffer[index] = guidByteUnion.Byte0;
			buffer[index + 1] = guidByteUnion.Byte1;
			buffer[index + 2] = guidByteUnion.Byte2;
			buffer[index + 3] = guidByteUnion.Byte3;
			buffer[index + 4] = guidByteUnion.Byte4;
			buffer[index + 5] = guidByteUnion.Byte5;
			buffer[index + 6] = guidByteUnion.Byte6;
			buffer[index + 7] = guidByteUnion.Byte7;
			buffer[index + 8] = guidByteUnion.Byte8;
			buffer[index + 9] = guidByteUnion.Byte9;
			if (BitConverter.IsLittleEndian)
			{
				buffer[index + 10] = guidByteUnion.Byte10;
				buffer[index + 11] = guidByteUnion.Byte11;
				buffer[index + 12] = guidByteUnion.Byte12;
				buffer[index + 13] = guidByteUnion.Byte13;
				buffer[index + 14] = guidByteUnion.Byte14;
				buffer[index + 15] = guidByteUnion.Byte15;
				return;
			}
			buffer[index + 10] = guidByteUnion.Byte15;
			buffer[index + 11] = guidByteUnion.Byte14;
			buffer[index + 12] = guidByteUnion.Byte13;
			buffer[index + 13] = guidByteUnion.Byte12;
			buffer[index + 14] = guidByteUnion.Byte11;
			buffer[index + 15] = guidByteUnion.Byte10;
		}

		// Token: 0x04000148 RID: 328
		private static readonly uint[] ByteToHexCharLookupLowerCase = ProperBitConverter.CreateByteToHexLookup(false);

		// Token: 0x04000149 RID: 329
		private static readonly uint[] ByteToHexCharLookupUpperCase = ProperBitConverter.CreateByteToHexLookup(true);

		// Token: 0x0400014A RID: 330
		private static readonly byte[] HexToByteLookup = new byte[]
		{
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			10,
			11,
			12,
			13,
			14,
			15,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			10,
			11,
			12,
			13,
			14,
			15,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue
		};

		// Token: 0x02000102 RID: 258
		[StructLayout(2)]
		private struct SingleByteUnion
		{
			// Token: 0x040002A1 RID: 673
			[FieldOffset(0)]
			public byte Byte0;

			// Token: 0x040002A2 RID: 674
			[FieldOffset(1)]
			public byte Byte1;

			// Token: 0x040002A3 RID: 675
			[FieldOffset(2)]
			public byte Byte2;

			// Token: 0x040002A4 RID: 676
			[FieldOffset(3)]
			public byte Byte3;

			// Token: 0x040002A5 RID: 677
			[FieldOffset(0)]
			public float Value;
		}

		// Token: 0x02000103 RID: 259
		[StructLayout(2)]
		private struct DoubleByteUnion
		{
			// Token: 0x040002A6 RID: 678
			[FieldOffset(0)]
			public byte Byte0;

			// Token: 0x040002A7 RID: 679
			[FieldOffset(1)]
			public byte Byte1;

			// Token: 0x040002A8 RID: 680
			[FieldOffset(2)]
			public byte Byte2;

			// Token: 0x040002A9 RID: 681
			[FieldOffset(3)]
			public byte Byte3;

			// Token: 0x040002AA RID: 682
			[FieldOffset(4)]
			public byte Byte4;

			// Token: 0x040002AB RID: 683
			[FieldOffset(5)]
			public byte Byte5;

			// Token: 0x040002AC RID: 684
			[FieldOffset(6)]
			public byte Byte6;

			// Token: 0x040002AD RID: 685
			[FieldOffset(7)]
			public byte Byte7;

			// Token: 0x040002AE RID: 686
			[FieldOffset(0)]
			public double Value;
		}

		// Token: 0x02000104 RID: 260
		[StructLayout(2)]
		private struct DecimalByteUnion
		{
			// Token: 0x040002AF RID: 687
			[FieldOffset(0)]
			public byte Byte0;

			// Token: 0x040002B0 RID: 688
			[FieldOffset(1)]
			public byte Byte1;

			// Token: 0x040002B1 RID: 689
			[FieldOffset(2)]
			public byte Byte2;

			// Token: 0x040002B2 RID: 690
			[FieldOffset(3)]
			public byte Byte3;

			// Token: 0x040002B3 RID: 691
			[FieldOffset(4)]
			public byte Byte4;

			// Token: 0x040002B4 RID: 692
			[FieldOffset(5)]
			public byte Byte5;

			// Token: 0x040002B5 RID: 693
			[FieldOffset(6)]
			public byte Byte6;

			// Token: 0x040002B6 RID: 694
			[FieldOffset(7)]
			public byte Byte7;

			// Token: 0x040002B7 RID: 695
			[FieldOffset(8)]
			public byte Byte8;

			// Token: 0x040002B8 RID: 696
			[FieldOffset(9)]
			public byte Byte9;

			// Token: 0x040002B9 RID: 697
			[FieldOffset(10)]
			public byte Byte10;

			// Token: 0x040002BA RID: 698
			[FieldOffset(11)]
			public byte Byte11;

			// Token: 0x040002BB RID: 699
			[FieldOffset(12)]
			public byte Byte12;

			// Token: 0x040002BC RID: 700
			[FieldOffset(13)]
			public byte Byte13;

			// Token: 0x040002BD RID: 701
			[FieldOffset(14)]
			public byte Byte14;

			// Token: 0x040002BE RID: 702
			[FieldOffset(15)]
			public byte Byte15;

			// Token: 0x040002BF RID: 703
			[FieldOffset(0)]
			public decimal Value;
		}

		// Token: 0x02000105 RID: 261
		[StructLayout(2)]
		private struct GuidByteUnion
		{
			// Token: 0x040002C0 RID: 704
			[FieldOffset(0)]
			public byte Byte0;

			// Token: 0x040002C1 RID: 705
			[FieldOffset(1)]
			public byte Byte1;

			// Token: 0x040002C2 RID: 706
			[FieldOffset(2)]
			public byte Byte2;

			// Token: 0x040002C3 RID: 707
			[FieldOffset(3)]
			public byte Byte3;

			// Token: 0x040002C4 RID: 708
			[FieldOffset(4)]
			public byte Byte4;

			// Token: 0x040002C5 RID: 709
			[FieldOffset(5)]
			public byte Byte5;

			// Token: 0x040002C6 RID: 710
			[FieldOffset(6)]
			public byte Byte6;

			// Token: 0x040002C7 RID: 711
			[FieldOffset(7)]
			public byte Byte7;

			// Token: 0x040002C8 RID: 712
			[FieldOffset(8)]
			public byte Byte8;

			// Token: 0x040002C9 RID: 713
			[FieldOffset(9)]
			public byte Byte9;

			// Token: 0x040002CA RID: 714
			[FieldOffset(10)]
			public byte Byte10;

			// Token: 0x040002CB RID: 715
			[FieldOffset(11)]
			public byte Byte11;

			// Token: 0x040002CC RID: 716
			[FieldOffset(12)]
			public byte Byte12;

			// Token: 0x040002CD RID: 717
			[FieldOffset(13)]
			public byte Byte13;

			// Token: 0x040002CE RID: 718
			[FieldOffset(14)]
			public byte Byte14;

			// Token: 0x040002CF RID: 719
			[FieldOffset(15)]
			public byte Byte15;

			// Token: 0x040002D0 RID: 720
			[FieldOffset(0)]
			public Guid Value;
		}
	}
}
