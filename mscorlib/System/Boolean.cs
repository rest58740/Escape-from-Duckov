using System;
using System.Runtime.Versioning;

namespace System
{
	// Token: 0x02000102 RID: 258
	[Serializable]
	public readonly struct Boolean : IComparable, IConvertible, IComparable<bool>, IEquatable<bool>
	{
		// Token: 0x06000798 RID: 1944 RVA: 0x00021FE3 File Offset: 0x000201E3
		public override int GetHashCode()
		{
			if (!this)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00021FEC File Offset: 0x000201EC
		public override string ToString()
		{
			if (!this)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00021FFD File Offset: 0x000201FD
		public string ToString(IFormatProvider provider)
		{
			return this.ToString();
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00022008 File Offset: 0x00020208
		public bool TryFormat(Span<char> destination, out int charsWritten)
		{
			string text = this ? "True" : "False";
			if (text.AsSpan().TryCopyTo(destination))
			{
				charsWritten = text.Length;
				return true;
			}
			charsWritten = 0;
			return false;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00022045 File Offset: 0x00020245
		public override bool Equals(object obj)
		{
			return obj is bool && this == (bool)obj;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0002205B File Offset: 0x0002025B
		[NonVersionable]
		public bool Equals(bool obj)
		{
			return this == obj;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00022062 File Offset: 0x00020262
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is bool))
			{
				throw new ArgumentException("Object must be of type Boolean.");
			}
			if (this == (bool)obj)
			{
				return 0;
			}
			if (!this)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002208F File Offset: 0x0002028F
		public int CompareTo(bool value)
		{
			if (this == value)
			{
				return 0;
			}
			if (!this)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0002209F File Offset: 0x0002029F
		public static bool Parse(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return bool.Parse(value.AsSpan());
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x000220BC File Offset: 0x000202BC
		public static bool Parse(ReadOnlySpan<char> value)
		{
			bool result;
			if (!bool.TryParse(value, out result))
			{
				throw new FormatException("String was not recognized as a valid Boolean.");
			}
			return result;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x000220DF File Offset: 0x000202DF
		public static bool TryParse(string value, out bool result)
		{
			if (value == null)
			{
				result = false;
				return false;
			}
			return bool.TryParse(value.AsSpan(), out result);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x000220F8 File Offset: 0x000202F8
		public static bool TryParse(ReadOnlySpan<char> value, out bool result)
		{
			ReadOnlySpan<char> span = "True".AsSpan();
			if (span.EqualsOrdinalIgnoreCase(value))
			{
				result = true;
				return true;
			}
			ReadOnlySpan<char> span2 = "False".AsSpan();
			if (span2.EqualsOrdinalIgnoreCase(value))
			{
				result = false;
				return true;
			}
			value = bool.TrimWhiteSpaceAndNull(value);
			if (span.EqualsOrdinalIgnoreCase(value))
			{
				result = true;
				return true;
			}
			if (span2.EqualsOrdinalIgnoreCase(value))
			{
				result = false;
				return true;
			}
			result = false;
			return false;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00022160 File Offset: 0x00020360
		private unsafe static ReadOnlySpan<char> TrimWhiteSpaceAndNull(ReadOnlySpan<char> value)
		{
			int num = 0;
			while (num < value.Length && (char.IsWhiteSpace((char)(*value[num])) || *value[num] == 0))
			{
				num++;
			}
			int num2 = value.Length - 1;
			while (num2 >= num && (char.IsWhiteSpace((char)(*value[num2])) || *value[num2] == 0))
			{
				num2--;
			}
			return value.Slice(num, num2 - num + 1);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000221D6 File Offset: 0x000203D6
		public TypeCode GetTypeCode()
		{
			return TypeCode.Boolean;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x000221D9 File Offset: 0x000203D9
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x000221DD File Offset: 0x000203DD
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Boolean", "Char"));
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000221F8 File Offset: 0x000203F8
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00022201 File Offset: 0x00020401
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0002220A File Offset: 0x0002040A
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00022213 File Offset: 0x00020413
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0002221C File Offset: 0x0002041C
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00022225 File Offset: 0x00020425
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0002222E File Offset: 0x0002042E
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00022237 File Offset: 0x00020437
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00022240 File Offset: 0x00020440
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00022249 File Offset: 0x00020449
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00022252 File Offset: 0x00020452
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0002225B File Offset: 0x0002045B
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Boolean", "DateTime"));
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00022276 File Offset: 0x00020476
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400106D RID: 4205
		private readonly bool m_value;

		// Token: 0x0400106E RID: 4206
		internal const int True = 1;

		// Token: 0x0400106F RID: 4207
		internal const int False = 0;

		// Token: 0x04001070 RID: 4208
		internal const string TrueLiteral = "True";

		// Token: 0x04001071 RID: 4209
		internal const string FalseLiteral = "False";

		// Token: 0x04001072 RID: 4210
		public static readonly string TrueString = "True";

		// Token: 0x04001073 RID: 4211
		public static readonly string FalseString = "False";
	}
}
