using System;
using System.Globalization;
using System.Runtime.Versioning;

namespace System
{
	// Token: 0x02000105 RID: 261
	[Serializable]
	public readonly struct Char : IComparable, IComparable<char>, IEquatable<char>, IConvertible
	{
		// Token: 0x060007DD RID: 2013 RVA: 0x00022558 File Offset: 0x00020758
		private static bool IsLatin1(char ch)
		{
			return ch <= 'ÿ';
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00022565 File Offset: 0x00020765
		private static bool IsAscii(char ch)
		{
			return ch <= '\u007f';
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0002256F File Offset: 0x0002076F
		private static UnicodeCategory GetLatin1UnicodeCategory(char ch)
		{
			return (UnicodeCategory)char.s_categoryForLatin1[(int)ch];
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00022578 File Offset: 0x00020778
		public override int GetHashCode()
		{
			return (int)(this | (int)this << 16);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00022582 File Offset: 0x00020782
		public override bool Equals(object obj)
		{
			return obj is char && this == (char)obj;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00022598 File Offset: 0x00020798
		[NonVersionable]
		public bool Equals(char obj)
		{
			return this == obj;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002259F File Offset: 0x0002079F
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is char))
			{
				throw new ArgumentException("Object must be of type Char.");
			}
			return (int)(this - (char)value);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000225C2 File Offset: 0x000207C2
		public int CompareTo(char value)
		{
			return (int)(this - value);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000225C8 File Offset: 0x000207C8
		public override string ToString()
		{
			return char.ToString(this);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x000225C8 File Offset: 0x000207C8
		public string ToString(IFormatProvider provider)
		{
			return char.ToString(this);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x000225D1 File Offset: 0x000207D1
		public static string ToString(char c)
		{
			return string.CreateFromChar(c);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000225D9 File Offset: 0x000207D9
		public static char Parse(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length != 1)
			{
				throw new FormatException("String must be exactly one character long.");
			}
			return s[0];
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00022604 File Offset: 0x00020804
		public static bool TryParse(string s, out char result)
		{
			result = '\0';
			if (s == null)
			{
				return false;
			}
			if (s.Length != 1)
			{
				return false;
			}
			result = s[0];
			return true;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00022623 File Offset: 0x00020823
		public static bool IsDigit(char c)
		{
			if (char.IsLatin1(c))
			{
				return c >= '0' && c <= '9';
			}
			return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.DecimalDigitNumber;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00022646 File Offset: 0x00020846
		internal static bool CheckLetter(UnicodeCategory uc)
		{
			return uc <= UnicodeCategory.OtherLetter;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0002264F File Offset: 0x0002084F
		public static bool IsLetter(char c)
		{
			if (!char.IsLatin1(c))
			{
				return char.CheckLetter(CharUnicodeInfo.GetUnicodeCategory(c));
			}
			if (char.IsAscii(c))
			{
				c |= ' ';
				return c >= 'a' && c <= 'z';
			}
			return char.CheckLetter(char.GetLatin1UnicodeCategory(c));
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0002268F File Offset: 0x0002088F
		private static bool IsWhiteSpaceLatin1(char c)
		{
			return c == ' ' || c - '\t' <= '\u0004' || c == '\u00a0' || c == '\u0085';
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000226AF File Offset: 0x000208AF
		public static bool IsWhiteSpace(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.IsWhiteSpaceLatin1(c);
			}
			return CharUnicodeInfo.IsWhiteSpace(c);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000226C6 File Offset: 0x000208C6
		public static bool IsUpper(char c)
		{
			if (!char.IsLatin1(c))
			{
				return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.UppercaseLetter;
			}
			if (char.IsAscii(c))
			{
				return c >= 'A' && c <= 'Z';
			}
			return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.UppercaseLetter;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000226FB File Offset: 0x000208FB
		public static bool IsLower(char c)
		{
			if (!char.IsLatin1(c))
			{
				return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter;
			}
			if (char.IsAscii(c))
			{
				return c >= 'a' && c <= 'z';
			}
			return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.LowercaseLetter;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00022730 File Offset: 0x00020930
		internal static bool CheckPunctuation(UnicodeCategory uc)
		{
			return uc - UnicodeCategory.ConnectorPunctuation <= 6;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0002273C File Offset: 0x0002093C
		public static bool IsPunctuation(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.CheckPunctuation(char.GetLatin1UnicodeCategory(c));
			}
			return char.CheckPunctuation(CharUnicodeInfo.GetUnicodeCategory(c));
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0002275D File Offset: 0x0002095D
		internal static bool CheckLetterOrDigit(UnicodeCategory uc)
		{
			return uc <= UnicodeCategory.OtherLetter || uc == UnicodeCategory.DecimalDigitNumber;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0002276A File Offset: 0x0002096A
		public static bool IsLetterOrDigit(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.CheckLetterOrDigit(char.GetLatin1UnicodeCategory(c));
			}
			return char.CheckLetterOrDigit(CharUnicodeInfo.GetUnicodeCategory(c));
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0002278B File Offset: 0x0002098B
		public static char ToUpper(char c, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToUpper(c);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000227A7 File Offset: 0x000209A7
		public static char ToUpper(char c)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToUpper(c);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000227B9 File Offset: 0x000209B9
		public static char ToUpperInvariant(char c)
		{
			return CultureInfo.InvariantCulture.TextInfo.ToUpper(c);
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000227CB File Offset: 0x000209CB
		public static char ToLower(char c, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToLower(c);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x000227E7 File Offset: 0x000209E7
		public static char ToLower(char c)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToLower(c);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000227F9 File Offset: 0x000209F9
		public static char ToLowerInvariant(char c)
		{
			return CultureInfo.InvariantCulture.TextInfo.ToLower(c);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002280B File Offset: 0x00020A0B
		public TypeCode GetTypeCode()
		{
			return TypeCode.Char;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0002280E File Offset: 0x00020A0E
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Char", "Boolean"));
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00022829 File Offset: 0x00020A29
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002282D File Offset: 0x00020A2D
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00022836 File Offset: 0x00020A36
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002283F File Offset: 0x00020A3F
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00022848 File Offset: 0x00020A48
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00022851 File Offset: 0x00020A51
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002285A File Offset: 0x00020A5A
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00022863 File Offset: 0x00020A63
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002286C File Offset: 0x00020A6C
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00022875 File Offset: 0x00020A75
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Char", "Single"));
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00022890 File Offset: 0x00020A90
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Char", "Double"));
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x000228AB File Offset: 0x00020AAB
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Char", "Decimal"));
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x000228C6 File Offset: 0x00020AC6
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Char", "DateTime"));
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x000228E1 File Offset: 0x00020AE1
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000228F1 File Offset: 0x00020AF1
		public static bool IsControl(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.Control;
			}
			return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.Control;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00022910 File Offset: 0x00020B10
		public static bool IsControl(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char ch = s[index];
			if (char.IsLatin1(ch))
			{
				return char.GetLatin1UnicodeCategory(ch) == UnicodeCategory.Control;
			}
			return CharUnicodeInfo.GetUnicodeCategory(s, index) == UnicodeCategory.Control;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00022968 File Offset: 0x00020B68
		public static bool IsDigit(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (char.IsLatin1(c))
			{
				return c >= '0' && c <= '9';
			}
			return CharUnicodeInfo.GetUnicodeCategory(s, index) == UnicodeCategory.DecimalDigitNumber;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000229C4 File Offset: 0x00020BC4
		public static bool IsLetter(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (!char.IsLatin1(c))
			{
				return char.CheckLetter(CharUnicodeInfo.GetUnicodeCategory(s, index));
			}
			if (char.IsAscii(c))
			{
				c |= ' ';
				return c >= 'a' && c <= 'z';
			}
			return char.CheckLetter(char.GetLatin1UnicodeCategory(c));
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00022A3C File Offset: 0x00020C3C
		public static bool IsLetterOrDigit(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char ch = s[index];
			if (char.IsLatin1(ch))
			{
				return char.CheckLetterOrDigit(char.GetLatin1UnicodeCategory(ch));
			}
			return char.CheckLetterOrDigit(CharUnicodeInfo.GetUnicodeCategory(s, index));
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00022A94 File Offset: 0x00020C94
		public static bool IsLower(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (!char.IsLatin1(c))
			{
				return CharUnicodeInfo.GetUnicodeCategory(s, index) == UnicodeCategory.LowercaseLetter;
			}
			if (char.IsAscii(c))
			{
				return c >= 'a' && c <= 'z';
			}
			return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.LowercaseLetter;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00022AFF File Offset: 0x00020CFF
		internal static bool CheckNumber(UnicodeCategory uc)
		{
			return uc - UnicodeCategory.DecimalDigitNumber <= 2;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00022B0A File Offset: 0x00020D0A
		public static bool IsNumber(char c)
		{
			if (!char.IsLatin1(c))
			{
				return char.CheckNumber(CharUnicodeInfo.GetUnicodeCategory(c));
			}
			if (char.IsAscii(c))
			{
				return c >= '0' && c <= '9';
			}
			return char.CheckNumber(char.GetLatin1UnicodeCategory(c));
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00022B44 File Offset: 0x00020D44
		public static bool IsNumber(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (!char.IsLatin1(c))
			{
				return char.CheckNumber(CharUnicodeInfo.GetUnicodeCategory(s, index));
			}
			if (char.IsAscii(c))
			{
				return c >= '0' && c <= '9';
			}
			return char.CheckNumber(char.GetLatin1UnicodeCategory(c));
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00022BB4 File Offset: 0x00020DB4
		public static bool IsPunctuation(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char ch = s[index];
			if (char.IsLatin1(ch))
			{
				return char.CheckPunctuation(char.GetLatin1UnicodeCategory(ch));
			}
			return char.CheckPunctuation(CharUnicodeInfo.GetUnicodeCategory(s, index));
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00022C0B File Offset: 0x00020E0B
		internal static bool CheckSeparator(UnicodeCategory uc)
		{
			return uc - UnicodeCategory.SpaceSeparator <= 2;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00022C17 File Offset: 0x00020E17
		private static bool IsSeparatorLatin1(char c)
		{
			return c == ' ' || c == '\u00a0';
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00022C28 File Offset: 0x00020E28
		public static bool IsSeparator(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.IsSeparatorLatin1(c);
			}
			return char.CheckSeparator(CharUnicodeInfo.GetUnicodeCategory(c));
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00022C44 File Offset: 0x00020E44
		public static bool IsSeparator(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (char.IsLatin1(c))
			{
				return char.IsSeparatorLatin1(c);
			}
			return char.CheckSeparator(CharUnicodeInfo.GetUnicodeCategory(s, index));
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00022C96 File Offset: 0x00020E96
		public static bool IsSurrogate(char c)
		{
			return c >= '\ud800' && c <= '\udfff';
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00022CAD File Offset: 0x00020EAD
		public static bool IsSurrogate(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return char.IsSurrogate(s[index]);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00022CDD File Offset: 0x00020EDD
		internal static bool CheckSymbol(UnicodeCategory uc)
		{
			return uc - UnicodeCategory.MathSymbol <= 3;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00022CE9 File Offset: 0x00020EE9
		public static bool IsSymbol(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.CheckSymbol(char.GetLatin1UnicodeCategory(c));
			}
			return char.CheckSymbol(CharUnicodeInfo.GetUnicodeCategory(c));
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00022D0C File Offset: 0x00020F0C
		public static bool IsSymbol(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char ch = s[index];
			if (char.IsLatin1(ch))
			{
				return char.CheckSymbol(char.GetLatin1UnicodeCategory(ch));
			}
			return char.CheckSymbol(CharUnicodeInfo.GetUnicodeCategory(s, index));
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00022D64 File Offset: 0x00020F64
		public static bool IsUpper(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (!char.IsLatin1(c))
			{
				return CharUnicodeInfo.GetUnicodeCategory(s, index) == UnicodeCategory.UppercaseLetter;
			}
			if (char.IsAscii(c))
			{
				return c >= 'A' && c <= 'Z';
			}
			return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.UppercaseLetter;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00022DD0 File Offset: 0x00020FD0
		public static bool IsWhiteSpace(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (char.IsLatin1(s[index]))
			{
				return char.IsWhiteSpaceLatin1(s[index]);
			}
			return CharUnicodeInfo.IsWhiteSpace(s, index);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00022E21 File Offset: 0x00021021
		public static UnicodeCategory GetUnicodeCategory(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.GetLatin1UnicodeCategory(c);
			}
			return CharUnicodeInfo.GetUnicodeCategory((int)c);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00022E38 File Offset: 0x00021038
		public static UnicodeCategory GetUnicodeCategory(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (char.IsLatin1(s[index]))
			{
				return char.GetLatin1UnicodeCategory(s[index]);
			}
			return CharUnicodeInfo.InternalGetUnicodeCategory(s, index);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00022E89 File Offset: 0x00021089
		public static double GetNumericValue(char c)
		{
			return CharUnicodeInfo.GetNumericValue(c);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00022E91 File Offset: 0x00021091
		public static double GetNumericValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return CharUnicodeInfo.GetNumericValue(s, index);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00022EBC File Offset: 0x000210BC
		public static bool IsHighSurrogate(char c)
		{
			return c >= '\ud800' && c <= '\udbff';
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00022ED3 File Offset: 0x000210D3
		public static bool IsHighSurrogate(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return char.IsHighSurrogate(s[index]);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00022F07 File Offset: 0x00021107
		public static bool IsLowSurrogate(char c)
		{
			return c >= '\udc00' && c <= '\udfff';
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00022F1E File Offset: 0x0002111E
		public static bool IsLowSurrogate(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return char.IsLowSurrogate(s[index]);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00022F54 File Offset: 0x00021154
		public static bool IsSurrogatePair(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return index + 1 < s.Length && char.IsSurrogatePair(s[index], s[index + 1]);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00022FA9 File Offset: 0x000211A9
		public static bool IsSurrogatePair(char highSurrogate, char lowSurrogate)
		{
			return highSurrogate >= '\ud800' && highSurrogate <= '\udbff' && lowSurrogate >= '\udc00' && lowSurrogate <= '\udfff';
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00022FD4 File Offset: 0x000211D4
		public unsafe static string ConvertFromUtf32(int utf32)
		{
			if (utf32 < 0 || utf32 > 1114111 || (utf32 >= 55296 && utf32 <= 57343))
			{
				throw new ArgumentOutOfRangeException("utf32", "A valid UTF32 value is between 0x000000 and 0x10ffff, inclusive, and should not include surrogate codepoint values (0x00d800 ~ 0x00dfff).");
			}
			if (utf32 < 65536)
			{
				return char.ToString((char)utf32);
			}
			utf32 -= 65536;
			uint num = 0U;
			char* ptr = (char*)(&num);
			*ptr = (char)(utf32 / 1024 + 55296);
			ptr[1] = (char)(utf32 % 1024 + 56320);
			return new string(ptr, 0, 2);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00023058 File Offset: 0x00021258
		public static int ConvertToUtf32(char highSurrogate, char lowSurrogate)
		{
			if (!char.IsHighSurrogate(highSurrogate))
			{
				throw new ArgumentOutOfRangeException("highSurrogate", "A valid high surrogate character is between 0xd800 and 0xdbff, inclusive.");
			}
			if (!char.IsLowSurrogate(lowSurrogate))
			{
				throw new ArgumentOutOfRangeException("lowSurrogate", "A valid low surrogate character is between 0xdc00 and 0xdfff, inclusive.");
			}
			return (int)((highSurrogate - '\ud800') * 'Ѐ' + (lowSurrogate - '\udc00')) + 65536;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000230B0 File Offset: 0x000212B0
		public static int ConvertToUtf32(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			int num = (int)(s[index] - '\ud800');
			if (num < 0 || num > 2047)
			{
				return (int)s[index];
			}
			if (num > 1023)
			{
				throw new ArgumentException(SR.Format("Found a low surrogate char without a preceding high surrogate at index: {0}. The input may not be in this encoding, or may not contain valid Unicode (UTF-16) characters.", index), "s");
			}
			if (index >= s.Length - 1)
			{
				throw new ArgumentException(SR.Format("Found a high surrogate char without a following low surrogate at index: {0}. The input may not be in this encoding, or may not contain valid Unicode (UTF-16) characters.", index), "s");
			}
			int num2 = (int)(s[index + 1] - '\udc00');
			if (num2 >= 0 && num2 <= 1023)
			{
				return num * 1024 + num2 + 65536;
			}
			throw new ArgumentException(SR.Format("Found a high surrogate char without a following low surrogate at index: {0}. The input may not be in this encoding, or may not contain valid Unicode (UTF-16) characters.", index), "s");
		}

		// Token: 0x04001078 RID: 4216
		private readonly char m_value;

		// Token: 0x04001079 RID: 4217
		public const char MaxValue = '￿';

		// Token: 0x0400107A RID: 4218
		public const char MinValue = '\0';

		// Token: 0x0400107B RID: 4219
		private static readonly byte[] s_categoryForLatin1 = new byte[]
		{
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			11,
			24,
			24,
			24,
			26,
			24,
			24,
			24,
			20,
			21,
			24,
			25,
			24,
			19,
			24,
			24,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			24,
			24,
			25,
			25,
			25,
			24,
			24,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			20,
			24,
			21,
			27,
			18,
			27,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			20,
			25,
			21,
			25,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			11,
			24,
			26,
			26,
			26,
			26,
			28,
			28,
			27,
			28,
			1,
			22,
			25,
			19,
			28,
			27,
			28,
			25,
			10,
			10,
			27,
			1,
			28,
			24,
			27,
			10,
			1,
			23,
			10,
			10,
			10,
			24,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			25,
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
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			25,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1
		};

		// Token: 0x0400107C RID: 4220
		internal const int UNICODE_PLANE00_END = 65535;

		// Token: 0x0400107D RID: 4221
		internal const int UNICODE_PLANE01_START = 65536;

		// Token: 0x0400107E RID: 4222
		internal const int UNICODE_PLANE16_END = 1114111;

		// Token: 0x0400107F RID: 4223
		internal const int HIGH_SURROGATE_START = 55296;

		// Token: 0x04001080 RID: 4224
		internal const int LOW_SURROGATE_END = 57343;
	}
}
