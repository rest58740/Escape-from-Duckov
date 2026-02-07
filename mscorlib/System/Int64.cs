using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x0200014A RID: 330
	[Serializable]
	public readonly struct Int64 : IComparable, IConvertible, IFormattable, IComparable<long>, IEquatable<long>, ISpanFormattable
	{
		// Token: 0x06000C66 RID: 3174 RVA: 0x00032574 File Offset: 0x00030774
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is long))
			{
				throw new ArgumentException("Object must be of type Int64.");
			}
			long num = (long)value;
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

		// Token: 0x06000C67 RID: 3175 RVA: 0x000325AF File Offset: 0x000307AF
		public int CompareTo(long value)
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

		// Token: 0x06000C68 RID: 3176 RVA: 0x000325C0 File Offset: 0x000307C0
		public override bool Equals(object obj)
		{
			return obj is long && this == (long)obj;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x000325D6 File Offset: 0x000307D6
		[NonVersionable]
		public bool Equals(long obj)
		{
			return this == obj;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x000325DD File Offset: 0x000307DD
		public override int GetHashCode()
		{
			return (int)this ^ (int)(this >> 32);
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x000325E9 File Offset: 0x000307E9
		public override string ToString()
		{
			return Number.FormatInt64(this, null, null);
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x000325F9 File Offset: 0x000307F9
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt64(this, null, provider);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00032609 File Offset: 0x00030809
		public string ToString(string format)
		{
			return Number.FormatInt64(this, format, null);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00032619 File Offset: 0x00030819
		[SecuritySafeCritical]
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatInt64(this, format, provider);
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00032629 File Offset: 0x00030829
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			return Number.TryFormatInt64(this, format, provider, destination, out charsWritten);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00032637 File Offset: 0x00030837
		public static long Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00032654 File Offset: 0x00030854
		public static long Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseInt64(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00032677 File Offset: 0x00030877
		public static long Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseInt64(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00032695 File Offset: 0x00030895
		public static long Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseInt64(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000326B9 File Offset: 0x000308B9
		public static long Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseInt64(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x000326CE File Offset: 0x000308CE
		public static bool TryParse(string s, out long result)
		{
			if (s == null)
			{
				result = 0L;
				return false;
			}
			return Number.TryParseInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x000326EB File Offset: 0x000308EB
		public static bool TryParse(ReadOnlySpan<char> s, out long result)
		{
			return Number.TryParseInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000326FA File Offset: 0x000308FA
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out long result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				result = 0L;
				return false;
			}
			return Number.TryParseInt64(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0003271E File Offset: 0x0003091E
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out long result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseInt64(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00032734 File Offset: 0x00030934
		public TypeCode GetTypeCode()
		{
			return TypeCode.Int64;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00032738 File Offset: 0x00030938
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00032741 File Offset: 0x00030941
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0003274A File Offset: 0x0003094A
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00032753 File Offset: 0x00030953
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0003275C File Offset: 0x0003095C
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00032765 File Offset: 0x00030965
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0003276E File Offset: 0x0003096E
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00032777 File Offset: 0x00030977
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00032780 File Offset: 0x00030980
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00032784 File Offset: 0x00030984
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0003278D File Offset: 0x0003098D
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00032796 File Offset: 0x00030996
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0003279F File Offset: 0x0003099F
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x000327A8 File Offset: 0x000309A8
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Int64", "DateTime"));
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000327C3 File Offset: 0x000309C3
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04001258 RID: 4696
		private readonly long m_value;

		// Token: 0x04001259 RID: 4697
		public const long MaxValue = 9223372036854775807L;

		// Token: 0x0400125A RID: 4698
		public const long MinValue = -9223372036854775808L;
	}
}
