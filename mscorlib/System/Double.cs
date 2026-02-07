using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x02000112 RID: 274
	[Serializable]
	public readonly struct Double : IComparable, IConvertible, IFormattable, IComparable<double>, IEquatable<double>, ISpanFormattable
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x0002825C File Offset: 0x0002645C
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsFinite(double d)
		{
			return (BitConverter.DoubleToInt64Bits(d) & long.MaxValue) < 9218868437227405312L;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00028279 File Offset: 0x00026479
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInfinity(double d)
		{
			return (BitConverter.DoubleToInt64Bits(d) & long.MaxValue) == 9218868437227405312L;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00028296 File Offset: 0x00026496
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(double d)
		{
			return (BitConverter.DoubleToInt64Bits(d) & long.MaxValue) > 9218868437227405312L;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x000282B3 File Offset: 0x000264B3
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNegative(double d)
		{
			return (BitConverter.DoubleToInt64Bits(d) & long.MinValue) == long.MinValue;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x000282D0 File Offset: 0x000264D0
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNegativeInfinity(double d)
		{
			return d == double.NegativeInfinity;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x000282E0 File Offset: 0x000264E0
		[NonVersionable]
		public static bool IsNormal(double d)
		{
			long num = BitConverter.DoubleToInt64Bits(d);
			num &= long.MaxValue;
			return num < 9218868437227405312L && num != 0L && (num & 9218868437227405312L) != 0L;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00028320 File Offset: 0x00026520
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPositiveInfinity(double d)
		{
			return d == double.PositiveInfinity;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00028330 File Offset: 0x00026530
		[NonVersionable]
		public static bool IsSubnormal(double d)
		{
			long num = BitConverter.DoubleToInt64Bits(d);
			num &= long.MaxValue;
			return num < 9218868437227405312L && num != 0L && (num & 9218868437227405312L) == 0L;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00028370 File Offset: 0x00026570
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is double))
			{
				throw new ArgumentException("Object must be of type Double.");
			}
			double num = (double)value;
			if (this < num)
			{
				return -1;
			}
			if (this > num)
			{
				return 1;
			}
			if (this == num)
			{
				return 0;
			}
			if (!double.IsNaN(this))
			{
				return 1;
			}
			if (!double.IsNaN(num))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x000283C7 File Offset: 0x000265C7
		public int CompareTo(double value)
		{
			if (this < value)
			{
				return -1;
			}
			if (this > value)
			{
				return 1;
			}
			if (this == value)
			{
				return 0;
			}
			if (!double.IsNaN(this))
			{
				return 1;
			}
			if (!double.IsNaN(value))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000283F4 File Offset: 0x000265F4
		public override bool Equals(object obj)
		{
			if (!(obj is double))
			{
				return false;
			}
			double num = (double)obj;
			return num == this || (double.IsNaN(num) && double.IsNaN(this));
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0002842A File Offset: 0x0002662A
		[NonVersionable]
		public static bool operator ==(double left, double right)
		{
			return left == right;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00028430 File Offset: 0x00026630
		[NonVersionable]
		public static bool operator !=(double left, double right)
		{
			return left != right;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00028439 File Offset: 0x00026639
		[NonVersionable]
		public static bool operator <(double left, double right)
		{
			return left < right;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002843F File Offset: 0x0002663F
		[NonVersionable]
		public static bool operator >(double left, double right)
		{
			return left > right;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00028445 File Offset: 0x00026645
		[NonVersionable]
		public static bool operator <=(double left, double right)
		{
			return left <= right;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002844E File Offset: 0x0002664E
		[NonVersionable]
		public static bool operator >=(double left, double right)
		{
			return left >= right;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00028457 File Offset: 0x00026657
		public bool Equals(double obj)
		{
			return obj == this || (double.IsNaN(obj) && double.IsNaN(this));
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00028474 File Offset: 0x00026674
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			long num = BitConverter.DoubleToInt64Bits(this);
			if ((num - 1L & 9223372036854775807L) >= 9218868437227405312L)
			{
				num &= 9218868437227405312L;
			}
			return (int)num ^ (int)(num >> 32);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000284B6 File Offset: 0x000266B6
		public override string ToString()
		{
			return Number.FormatDouble(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x000284C5 File Offset: 0x000266C5
		public string ToString(string format)
		{
			return Number.FormatDouble(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x000284D4 File Offset: 0x000266D4
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatDouble(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000284E4 File Offset: 0x000266E4
		[SecuritySafeCritical]
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatDouble(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x000284F4 File Offset: 0x000266F4
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			return Number.TryFormatDouble(this, format, NumberFormatInfo.GetInstance(provider), destination, out charsWritten);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00028507 File Offset: 0x00026707
		public static double Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseDouble(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00028528 File Offset: 0x00026728
		public static double Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseDouble(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0002854B File Offset: 0x0002674B
		public static double Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseDouble(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0002856D File Offset: 0x0002676D
		public static double Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseDouble(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x00028591 File Offset: 0x00026791
		public static double Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return Number.ParseDouble(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x000285A6 File Offset: 0x000267A6
		public static bool TryParse(string s, out double result)
		{
			if (s == null)
			{
				result = 0.0;
				return false;
			}
			return double.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000285CE File Offset: 0x000267CE
		public static bool TryParse(ReadOnlySpan<char> s, out double result)
		{
			return double.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000285E1 File Offset: 0x000267E1
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out double result)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			if (s == null)
			{
				result = 0.0;
				return false;
			}
			return double.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0002860C File Offset: 0x0002680C
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out double result)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return double.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00028624 File Offset: 0x00026824
		private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info, out double result)
		{
			if (!Number.TryParseDouble(s, style, info, out result))
			{
				ReadOnlySpan<char> span = s.Trim();
				if (span.EqualsOrdinal(info.PositiveInfinitySymbol))
				{
					result = double.PositiveInfinity;
				}
				else if (span.EqualsOrdinal(info.NegativeInfinitySymbol))
				{
					result = double.NegativeInfinity;
				}
				else
				{
					if (!span.EqualsOrdinal(info.NaNSymbol))
					{
						return false;
					}
					result = double.NaN;
				}
			}
			return true;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000286A6 File Offset: 0x000268A6
		public TypeCode GetTypeCode()
		{
			return TypeCode.Double;
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000286AA File Offset: 0x000268AA
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000286B3 File Offset: 0x000268B3
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Double", "Char"));
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000286CE File Offset: 0x000268CE
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x000286D7 File Offset: 0x000268D7
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x000286E0 File Offset: 0x000268E0
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x000286E9 File Offset: 0x000268E9
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x000286F2 File Offset: 0x000268F2
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000286FB File Offset: 0x000268FB
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00028704 File Offset: 0x00026904
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0002870D File Offset: 0x0002690D
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00028716 File Offset: 0x00026916
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0002871F File Offset: 0x0002691F
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00028723 File Offset: 0x00026923
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0002872C File Offset: 0x0002692C
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Double", "DateTime"));
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00028747 File Offset: 0x00026947
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040010D9 RID: 4313
		private readonly double m_value;

		// Token: 0x040010DA RID: 4314
		public const double MinValue = -1.7976931348623157E+308;

		// Token: 0x040010DB RID: 4315
		public const double MaxValue = 1.7976931348623157E+308;

		// Token: 0x040010DC RID: 4316
		public const double Epsilon = 5E-324;

		// Token: 0x040010DD RID: 4317
		public const double NegativeInfinity = double.NegativeInfinity;

		// Token: 0x040010DE RID: 4318
		public const double PositiveInfinity = double.PositiveInfinity;

		// Token: 0x040010DF RID: 4319
		public const double NaN = double.NaN;

		// Token: 0x040010E0 RID: 4320
		internal const double NegativeZero = --0.0;
	}
}
