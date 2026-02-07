using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x02000103 RID: 259
	[Serializable]
	public readonly struct Byte : IComparable, IConvertible, IFormattable, IComparable<byte>, IEquatable<byte>, ISpanFormattable
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x0002229C File Offset: 0x0002049C
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is byte))
			{
				throw new ArgumentException("Object must be of type Byte.");
			}
			return (int)(this - (byte)value);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x000222BF File Offset: 0x000204BF
		public int CompareTo(byte value)
		{
			return (int)(this - value);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x000222C5 File Offset: 0x000204C5
		public override bool Equals(object obj)
		{
			return obj is byte && this == (byte)obj;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0002205B File Offset: 0x0002025B
		[NonVersionable]
		public bool Equals(byte obj)
		{
			return this == obj;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x000221D9 File Offset: 0x000203D9
		public override int GetHashCode()
		{
			return (int)this;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x000222DB File Offset: 0x000204DB
		public static byte Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return byte.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x000222F8 File Offset: 0x000204F8
		public static byte Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return byte.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0002231B File Offset: 0x0002051B
		public static byte Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return byte.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00022339 File Offset: 0x00020539
		public static byte Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return byte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0002235D File Offset: 0x0002055D
		public static byte Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return byte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00022374 File Offset: 0x00020574
		private static byte Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
		{
			int num = 0;
			try
			{
				num = Number.ParseInt32(s, style, info);
			}
			catch (OverflowException innerException)
			{
				throw new OverflowException("Value was either too large or too small for an unsigned byte.", innerException);
			}
			if (num < 0 || num > 255)
			{
				throw new OverflowException("Value was either too large or too small for an unsigned byte.");
			}
			return (byte)num;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x000223C4 File Offset: 0x000205C4
		public static bool TryParse(string s, out byte result)
		{
			if (s == null)
			{
				result = 0;
				return false;
			}
			return byte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x000223E0 File Offset: 0x000205E0
		public static bool TryParse(ReadOnlySpan<char> s, out byte result)
		{
			return byte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000223EF File Offset: 0x000205EF
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out byte result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				result = 0;
				return false;
			}
			return byte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00022412 File Offset: 0x00020612
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out byte result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return byte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00022428 File Offset: 0x00020628
		private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info, out byte result)
		{
			result = 0;
			int num;
			if (!Number.TryParseInt32(s, style, info, out num))
			{
				return false;
			}
			if (num < 0 || num > 255)
			{
				return false;
			}
			result = (byte)num;
			return true;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00022459 File Offset: 0x00020659
		public override string ToString()
		{
			return Number.FormatInt32((int)this, null, null);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00022469 File Offset: 0x00020669
		public string ToString(string format)
		{
			return Number.FormatInt32((int)this, format, null);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00022479 File Offset: 0x00020679
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, null, provider);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00022489 File Offset: 0x00020689
		[SecuritySafeCritical]
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, format, provider);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00022499 File Offset: 0x00020699
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			return Number.TryFormatInt32((int)this, format, provider, destination, out charsWritten);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000224A7 File Offset: 0x000206A7
		public TypeCode GetTypeCode()
		{
			return TypeCode.Byte;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000224AA File Offset: 0x000206AA
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x000224B3 File Offset: 0x000206B3
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000224BC File Offset: 0x000206BC
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x000221D9 File Offset: 0x000203D9
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x000224C5 File Offset: 0x000206C5
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000224CE File Offset: 0x000206CE
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000224D7 File Offset: 0x000206D7
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x000224E0 File Offset: 0x000206E0
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000224E9 File Offset: 0x000206E9
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000224F2 File Offset: 0x000206F2
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000224FB File Offset: 0x000206FB
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00022504 File Offset: 0x00020704
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0002250D File Offset: 0x0002070D
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00022516 File Offset: 0x00020716
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Byte", "DateTime"));
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00022531 File Offset: 0x00020731
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04001074 RID: 4212
		private readonly byte m_value;

		// Token: 0x04001075 RID: 4213
		public const byte MaxValue = 255;

		// Token: 0x04001076 RID: 4214
		public const byte MinValue = 0;
	}
}
