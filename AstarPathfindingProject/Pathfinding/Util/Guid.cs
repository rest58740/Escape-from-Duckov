using System;
using System.Text;

namespace Pathfinding.Util
{
	// Token: 0x02000292 RID: 658
	public struct Guid
	{
		// Token: 0x06000FBD RID: 4029 RVA: 0x00060610 File Offset: 0x0005E810
		public Guid(byte[] bytes)
		{
			ulong num = (ulong)bytes[0] | (ulong)bytes[1] << 8 | (ulong)bytes[2] << 16 | (ulong)bytes[3] << 24 | (ulong)bytes[4] << 32 | (ulong)bytes[5] << 40 | (ulong)bytes[6] << 48 | (ulong)bytes[7] << 56;
			ulong num2 = (ulong)bytes[8] | (ulong)bytes[9] << 8 | (ulong)bytes[10] << 16 | (ulong)bytes[11] << 24 | (ulong)bytes[12] << 32 | (ulong)bytes[13] << 40 | (ulong)bytes[14] << 48 | (ulong)bytes[15] << 56;
			this._a = (BitConverter.IsLittleEndian ? num : Guid.SwapEndianness(num));
			this._b = (BitConverter.IsLittleEndian ? num2 : Guid.SwapEndianness(num2));
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x000606C8 File Offset: 0x0005E8C8
		public Guid(string str)
		{
			this._a = 0UL;
			this._b = 0UL;
			if (str.Length < 32)
			{
				throw new FormatException("Invalid Guid format");
			}
			int i = 0;
			int num = 0;
			int num2 = 60;
			while (i < 16)
			{
				if (num >= str.Length)
				{
					throw new FormatException("Invalid Guid format. String too short");
				}
				char c = str[num];
				if (c != '-')
				{
					int num3 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c));
					if (num3 == -1)
					{
						throw new FormatException("Invalid Guid format : " + c.ToString() + " is not a hexadecimal character");
					}
					this._a |= (ulong)((ulong)((long)num3) << num2);
					num2 -= 4;
					i++;
				}
				num++;
			}
			num2 = 60;
			while (i < 32)
			{
				if (num >= str.Length)
				{
					throw new FormatException("Invalid Guid format. String too short");
				}
				char c2 = str[num];
				if (c2 != '-')
				{
					int num4 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c2));
					if (num4 == -1)
					{
						throw new FormatException("Invalid Guid format : " + c2.ToString() + " is not a hexadecimal character");
					}
					this._b |= (ulong)((ulong)((long)num4) << num2);
					num2 -= 4;
					i++;
				}
				num++;
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x000607FF File Offset: 0x0005E9FF
		public static Guid Parse(string input)
		{
			return new Guid(input);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00060808 File Offset: 0x0005EA08
		private static ulong SwapEndianness(ulong value)
		{
			ulong num = value & 255UL;
			ulong num2 = value >> 8 & 255UL;
			ulong num3 = value >> 16 & 255UL;
			ulong num4 = value >> 24 & 255UL;
			ulong num5 = value >> 32 & 255UL;
			ulong num6 = value >> 40 & 255UL;
			ulong num7 = value >> 48 & 255UL;
			ulong num8 = value >> 56 & 255UL;
			return num << 56 | num2 << 48 | num3 << 40 | num4 << 32 | num5 << 24 | num6 << 16 | num7 << 8 | num8;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00060898 File Offset: 0x0005EA98
		public static Guid NewGuid()
		{
			byte[] array = new byte[16];
			Guid.random.NextBytes(array);
			return new Guid(array);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x000608BE File Offset: 0x0005EABE
		public static bool operator ==(Guid lhs, Guid rhs)
		{
			return lhs._a == rhs._a && lhs._b == rhs._b;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x000608DE File Offset: 0x0005EADE
		public static bool operator !=(Guid lhs, Guid rhs)
		{
			return lhs._a != rhs._a || lhs._b != rhs._b;
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00060904 File Offset: 0x0005EB04
		public override bool Equals(object _rhs)
		{
			if (!(_rhs is Guid))
			{
				return false;
			}
			Guid guid = (Guid)_rhs;
			return this._a == guid._a && this._b == guid._b;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00060940 File Offset: 0x0005EB40
		public override int GetHashCode()
		{
			ulong num = this._a ^ this._b;
			return (int)(num >> 32) ^ (int)num;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00060964 File Offset: 0x0005EB64
		public override string ToString()
		{
			if (Guid.text == null)
			{
				Guid.text = new StringBuilder();
			}
			StringBuilder obj = Guid.text;
			string result;
			lock (obj)
			{
				Guid.text.Length = 0;
				Guid.text.Append(this._a.ToString("x16")).Append('-').Append(this._b.ToString("x16"));
				result = Guid.text.ToString();
			}
			return result;
		}

		// Token: 0x04000B8C RID: 2956
		private const string hex = "0123456789ABCDEF";

		// Token: 0x04000B8D RID: 2957
		public static readonly Guid zero = new Guid(new byte[16]);

		// Token: 0x04000B8E RID: 2958
		public static readonly string zeroString = new Guid(new byte[16]).ToString();

		// Token: 0x04000B8F RID: 2959
		private readonly ulong _a;

		// Token: 0x04000B90 RID: 2960
		private readonly ulong _b;

		// Token: 0x04000B91 RID: 2961
		private static Random random = new Random();

		// Token: 0x04000B92 RID: 2962
		private static StringBuilder text;
	}
}
