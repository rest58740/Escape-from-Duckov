using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x02000149 RID: 329
	[Serializable]
	public readonly struct Int32 : IComparable, IConvertible, IFormattable, IComparable<int>, IEquatable<int>, ISpanFormattable
	{
		// Token: 0x06000C43 RID: 3139 RVA: 0x00032320 File Offset: 0x00030520
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is int))
			{
				throw new ArgumentException("Object must be of type Int32.");
			}
			int num = (int)value;
			if (this < num)
			{
				return -1;
			}
			if (this > num)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0003235B File Offset: 0x0003055B
		public int CompareTo(int value)
		{
			if (this < value)
			{
				return -1;
			}
			if (this > value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0003236C File Offset: 0x0003056C
		public override bool Equals(object obj)
		{
			return obj is int && this == (int)obj;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00032382 File Offset: 0x00030582
		[NonVersionable]
		public bool Equals(int obj)
		{
			return this == obj;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00032389 File Offset: 0x00030589
		public override int GetHashCode()
		{
			return this;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0003238D File Offset: 0x0003058D
		public override string ToString()
		{
			return Number.FormatInt32(this, null, null);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0003239D File Offset: 0x0003059D
		public string ToString(string format)
		{
			return Number.FormatInt32(this, format, null);
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000323AD File Offset: 0x000305AD
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32(this, null, provider);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x000323BD File Offset: 0x000305BD
		[SecuritySafeCritical]
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatInt32(this, format, provider);
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x000323CD File Offset: 0x000305CD
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			return Number.TryFormatInt32(this, format, provider, destination, out charsWritten);
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x000323DB File Offset: 0x000305DB
		public static int Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x000323F8 File Offset: 0x000305F8
		public static int Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseInt32(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0003241B File Offset: 0x0003061B
		public static int Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00032439 File Offset: 0x00030639
		public static int Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseInt32(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0003245D File Offset: 0x0003065D
		public static int Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseInt32(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00032472 File Offset: 0x00030672
		public static bool TryParse(string s, out int result)
		{
			if (s == null)
			{
				result = 0;
				return false;
			}
			return Number.TryParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0003248E File Offset: 0x0003068E
		public static bool TryParse(ReadOnlySpan<char> s, out int result)
		{
			return Number.TryParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0003249D File Offset: 0x0003069D
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out int result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				result = 0;
				return false;
			}
			return Number.TryParseInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x000324C0 File Offset: 0x000306C0
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out int result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000324D6 File Offset: 0x000306D6
		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000324DA File Offset: 0x000306DA
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x000324E3 File Offset: 0x000306E3
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x000324EC File Offset: 0x000306EC
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x000324F5 File Offset: 0x000306F5
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x000324FE File Offset: 0x000306FE
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00032507 File Offset: 0x00030707
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00032389 File Offset: 0x00030589
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00032510 File Offset: 0x00030710
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00032519 File Offset: 0x00030719
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00032522 File Offset: 0x00030722
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0003252B File Offset: 0x0003072B
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00032534 File Offset: 0x00030734
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0003253D File Offset: 0x0003073D
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00032546 File Offset: 0x00030746
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Int32", "DateTime"));
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00032561 File Offset: 0x00030761
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04001255 RID: 4693
		private readonly int m_value;

		// Token: 0x04001256 RID: 4694
		public const int MaxValue = 2147483647;

		// Token: 0x04001257 RID: 4695
		public const int MinValue = -2147483648;
	}
}
