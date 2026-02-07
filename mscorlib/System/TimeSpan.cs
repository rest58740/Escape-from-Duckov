using System;
using System.Globalization;

namespace System
{
	// Token: 0x02000192 RID: 402
	[Serializable]
	public readonly struct TimeSpan : IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>, IFormattable, ISpanFormattable
	{
		// Token: 0x06000FCF RID: 4047 RVA: 0x00041A3F File Offset: 0x0003FC3F
		public TimeSpan(long ticks)
		{
			this._ticks = ticks;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00041A48 File Offset: 0x0003FC48
		public TimeSpan(int hours, int minutes, int seconds)
		{
			this._ticks = TimeSpan.TimeToTicks(hours, minutes, seconds);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00041A58 File Offset: 0x0003FC58
		public TimeSpan(int days, int hours, int minutes, int seconds)
		{
			this = new TimeSpan(days, hours, minutes, seconds, 0);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00041A68 File Offset: 0x0003FC68
		public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds)
		{
			long num = ((long)days * 3600L * 24L + (long)hours * 3600L + (long)minutes * 60L + (long)seconds) * 1000L + (long)milliseconds;
			if (num > 922337203685477L || num < -922337203685477L)
			{
				throw new ArgumentOutOfRangeException(null, "TimeSpan overflowed because the duration is too long.");
			}
			this._ticks = num * 10000L;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00041AD5 File Offset: 0x0003FCD5
		public long Ticks
		{
			get
			{
				return this._ticks;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x00041ADD File Offset: 0x0003FCDD
		public int Days
		{
			get
			{
				return (int)(this._ticks / 864000000000L);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00041AF0 File Offset: 0x0003FCF0
		public int Hours
		{
			get
			{
				return (int)(this._ticks / 36000000000L % 24L);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x00041B07 File Offset: 0x0003FD07
		public int Milliseconds
		{
			get
			{
				return (int)(this._ticks / 10000L % 1000L);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00041B1E File Offset: 0x0003FD1E
		public int Minutes
		{
			get
			{
				return (int)(this._ticks / 600000000L % 60L);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x00041B32 File Offset: 0x0003FD32
		public int Seconds
		{
			get
			{
				return (int)(this._ticks / 10000000L % 60L);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00041B46 File Offset: 0x0003FD46
		public double TotalDays
		{
			get
			{
				return (double)this._ticks * 1.1574074074074074E-12;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x00041B59 File Offset: 0x0003FD59
		public double TotalHours
		{
			get
			{
				return (double)this._ticks * 2.7777777777777777E-11;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x00041B6C File Offset: 0x0003FD6C
		public double TotalMilliseconds
		{
			get
			{
				double num = (double)this._ticks * 0.0001;
				if (num > 922337203685477.0)
				{
					return 922337203685477.0;
				}
				if (num < -922337203685477.0)
				{
					return -922337203685477.0;
				}
				return num;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x00041BB8 File Offset: 0x0003FDB8
		public double TotalMinutes
		{
			get
			{
				return (double)this._ticks * 1.6666666666666667E-09;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x00041BCB File Offset: 0x0003FDCB
		public double TotalSeconds
		{
			get
			{
				return (double)this._ticks * 1E-07;
			}
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00041BE0 File Offset: 0x0003FDE0
		public TimeSpan Add(TimeSpan ts)
		{
			long num = this._ticks + ts._ticks;
			if (this._ticks >> 63 == ts._ticks >> 63 && this._ticks >> 63 != num >> 63)
			{
				throw new OverflowException("TimeSpan overflowed because the duration is too long.");
			}
			return new TimeSpan(num);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00041C2F File Offset: 0x0003FE2F
		public static int Compare(TimeSpan t1, TimeSpan t2)
		{
			if (t1._ticks > t2._ticks)
			{
				return 1;
			}
			if (t1._ticks < t2._ticks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00041C54 File Offset: 0x0003FE54
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is TimeSpan))
			{
				throw new ArgumentException("Object must be of type TimeSpan.");
			}
			long ticks = ((TimeSpan)value)._ticks;
			if (this._ticks > ticks)
			{
				return 1;
			}
			if (this._ticks < ticks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00041C9C File Offset: 0x0003FE9C
		public int CompareTo(TimeSpan value)
		{
			long ticks = value._ticks;
			if (this._ticks > ticks)
			{
				return 1;
			}
			if (this._ticks < ticks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00041CC7 File Offset: 0x0003FEC7
		public static TimeSpan FromDays(double value)
		{
			return TimeSpan.Interval(value, 86400000);
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00041CD4 File Offset: 0x0003FED4
		public TimeSpan Duration()
		{
			if (this.Ticks == TimeSpan.MinValue.Ticks)
			{
				throw new OverflowException("The duration cannot be returned for TimeSpan.MinValue because the absolute value of TimeSpan.MinValue exceeds the value of TimeSpan.MaxValue.");
			}
			return new TimeSpan((this._ticks >= 0L) ? this._ticks : (-this._ticks));
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x00041D11 File Offset: 0x0003FF11
		public override bool Equals(object value)
		{
			return value is TimeSpan && this._ticks == ((TimeSpan)value)._ticks;
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x00041D30 File Offset: 0x0003FF30
		public bool Equals(TimeSpan obj)
		{
			return this._ticks == obj._ticks;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00041D30 File Offset: 0x0003FF30
		public static bool Equals(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks == t2._ticks;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00041D40 File Offset: 0x0003FF40
		public override int GetHashCode()
		{
			return (int)this._ticks ^ (int)(this._ticks >> 32);
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00041D54 File Offset: 0x0003FF54
		public static TimeSpan FromHours(double value)
		{
			return TimeSpan.Interval(value, 3600000);
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00041D64 File Offset: 0x0003FF64
		private static TimeSpan Interval(double value, int scale)
		{
			if (double.IsNaN(value))
			{
				throw new ArgumentException("TimeSpan does not accept floating point Not-a-Number values.");
			}
			double num = value * (double)scale + ((value >= 0.0) ? 0.5 : -0.5);
			if (num > 922337203685477.0 || num < -922337203685477.0)
			{
				throw new OverflowException("TimeSpan overflowed because the duration is too long.");
			}
			return new TimeSpan((long)num * 10000L);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00041DDB File Offset: 0x0003FFDB
		public static TimeSpan FromMilliseconds(double value)
		{
			return TimeSpan.Interval(value, 1);
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00041DE4 File Offset: 0x0003FFE4
		public static TimeSpan FromMinutes(double value)
		{
			return TimeSpan.Interval(value, 60000);
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00041DF1 File Offset: 0x0003FFF1
		public TimeSpan Negate()
		{
			if (this.Ticks == TimeSpan.MinValue.Ticks)
			{
				throw new OverflowException("Negating the minimum value of a twos complement number is invalid.");
			}
			return new TimeSpan(-this._ticks);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00041E1C File Offset: 0x0004001C
		public static TimeSpan FromSeconds(double value)
		{
			return TimeSpan.Interval(value, 1000);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00041E2C File Offset: 0x0004002C
		public TimeSpan Subtract(TimeSpan ts)
		{
			long num = this._ticks - ts._ticks;
			if (this._ticks >> 63 != ts._ticks >> 63 && this._ticks >> 63 != num >> 63)
			{
				throw new OverflowException("TimeSpan overflowed because the duration is too long.");
			}
			return new TimeSpan(num);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00041E7B File Offset: 0x0004007B
		public TimeSpan Multiply(double factor)
		{
			return this * factor;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00041E89 File Offset: 0x00040089
		public TimeSpan Divide(double divisor)
		{
			return this / divisor;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00041E97 File Offset: 0x00040097
		public double Divide(TimeSpan ts)
		{
			return this / ts;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00041EA5 File Offset: 0x000400A5
		public static TimeSpan FromTicks(long value)
		{
			return new TimeSpan(value);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00041EB0 File Offset: 0x000400B0
		internal static long TimeToTicks(int hour, int minute, int second)
		{
			long num = (long)hour * 3600L + (long)minute * 60L + (long)second;
			if (num > 922337203685L || num < -922337203685L)
			{
				throw new ArgumentOutOfRangeException(null, "TimeSpan overflowed because the duration is too long.");
			}
			return num * 10000000L;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x00041EFD File Offset: 0x000400FD
		private static void ValidateStyles(TimeSpanStyles style, string parameterName)
		{
			if (style != TimeSpanStyles.None && style != TimeSpanStyles.AssumeNegative)
			{
				throw new ArgumentException("An undefined TimeSpanStyles value is being used.", parameterName);
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00041F12 File Offset: 0x00040112
		public static TimeSpan Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			return TimeSpanParse.Parse(s, null);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00041F2A File Offset: 0x0004012A
		public static TimeSpan Parse(string input, IFormatProvider formatProvider)
		{
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			return TimeSpanParse.Parse(input, formatProvider);
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00041F42 File Offset: 0x00040142
		public static TimeSpan Parse(ReadOnlySpan<char> input, IFormatProvider formatProvider = null)
		{
			return TimeSpanParse.Parse(input, formatProvider);
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00041F4B File Offset: 0x0004014B
		public static TimeSpan ParseExact(string input, string format, IFormatProvider formatProvider)
		{
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			if (format == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
			}
			return TimeSpanParse.ParseExact(input, format, formatProvider, TimeSpanStyles.None);
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00041F74 File Offset: 0x00040174
		public static TimeSpan ParseExact(string input, string[] formats, IFormatProvider formatProvider)
		{
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			return TimeSpanParse.ParseExactMultiple(input, formats, formatProvider, TimeSpanStyles.None);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00041F8E File Offset: 0x0004018E
		public static TimeSpan ParseExact(string input, string format, IFormatProvider formatProvider, TimeSpanStyles styles)
		{
			TimeSpan.ValidateStyles(styles, "styles");
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			if (format == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
			}
			return TimeSpanParse.ParseExact(input, format, formatProvider, styles);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00041FC2 File Offset: 0x000401C2
		public static TimeSpan ParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format, IFormatProvider formatProvider, TimeSpanStyles styles = TimeSpanStyles.None)
		{
			TimeSpan.ValidateStyles(styles, "styles");
			return TimeSpanParse.ParseExact(input, format, formatProvider, styles);
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00041FD8 File Offset: 0x000401D8
		public static TimeSpan ParseExact(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles styles)
		{
			TimeSpan.ValidateStyles(styles, "styles");
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			return TimeSpanParse.ParseExactMultiple(input, formats, formatProvider, styles);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00041FFD File Offset: 0x000401FD
		public static TimeSpan ParseExact(ReadOnlySpan<char> input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles styles = TimeSpanStyles.None)
		{
			TimeSpan.ValidateStyles(styles, "styles");
			return TimeSpanParse.ParseExactMultiple(input, formats, formatProvider, styles);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00042013 File Offset: 0x00040213
		public static bool TryParse(string s, out TimeSpan result)
		{
			if (s == null)
			{
				result = default(TimeSpan);
				return false;
			}
			return TimeSpanParse.TryParse(s, null, out result);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0004202E File Offset: 0x0004022E
		public static bool TryParse(ReadOnlySpan<char> s, out TimeSpan result)
		{
			return TimeSpanParse.TryParse(s, null, out result);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00042038 File Offset: 0x00040238
		public static bool TryParse(string input, IFormatProvider formatProvider, out TimeSpan result)
		{
			if (input == null)
			{
				result = default(TimeSpan);
				return false;
			}
			return TimeSpanParse.TryParse(input, formatProvider, out result);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00042053 File Offset: 0x00040253
		public static bool TryParse(ReadOnlySpan<char> input, IFormatProvider formatProvider, out TimeSpan result)
		{
			return TimeSpanParse.TryParse(input, formatProvider, out result);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0004205D File Offset: 0x0004025D
		public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, out TimeSpan result)
		{
			if (input == null || format == null)
			{
				result = default(TimeSpan);
				return false;
			}
			return TimeSpanParse.TryParseExact(input, format, formatProvider, TimeSpanStyles.None, out result);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00042082 File Offset: 0x00040282
		public static bool TryParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format, IFormatProvider formatProvider, out TimeSpan result)
		{
			return TimeSpanParse.TryParseExact(input, format, formatProvider, TimeSpanStyles.None, out result);
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0004208E File Offset: 0x0004028E
		public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, out TimeSpan result)
		{
			if (input == null)
			{
				result = default(TimeSpan);
				return false;
			}
			return TimeSpanParse.TryParseExactMultiple(input, formats, formatProvider, TimeSpanStyles.None, out result);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x000420AB File Offset: 0x000402AB
		public static bool TryParseExact(ReadOnlySpan<char> input, string[] formats, IFormatProvider formatProvider, out TimeSpan result)
		{
			return TimeSpanParse.TryParseExactMultiple(input, formats, formatProvider, TimeSpanStyles.None, out result);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x000420B7 File Offset: 0x000402B7
		public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, TimeSpanStyles styles, out TimeSpan result)
		{
			TimeSpan.ValidateStyles(styles, "styles");
			if (input == null || format == null)
			{
				result = default(TimeSpan);
				return false;
			}
			return TimeSpanParse.TryParseExact(input, format, formatProvider, styles, out result);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x000420E9 File Offset: 0x000402E9
		public static bool TryParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format, IFormatProvider formatProvider, TimeSpanStyles styles, out TimeSpan result)
		{
			TimeSpan.ValidateStyles(styles, "styles");
			return TimeSpanParse.TryParseExact(input, format, formatProvider, styles, out result);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00042101 File Offset: 0x00040301
		public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles styles, out TimeSpan result)
		{
			TimeSpan.ValidateStyles(styles, "styles");
			if (input == null)
			{
				result = default(TimeSpan);
				return false;
			}
			return TimeSpanParse.TryParseExactMultiple(input, formats, formatProvider, styles, out result);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0004212B File Offset: 0x0004032B
		public static bool TryParseExact(ReadOnlySpan<char> input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles styles, out TimeSpan result)
		{
			TimeSpan.ValidateStyles(styles, "styles");
			return TimeSpanParse.TryParseExactMultiple(input, formats, formatProvider, styles, out result);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00042143 File Offset: 0x00040343
		public override string ToString()
		{
			return TimeSpanFormat.Format(this, null, null);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00042152 File Offset: 0x00040352
		public string ToString(string format)
		{
			return TimeSpanFormat.Format(this, format, null);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00042161 File Offset: 0x00040361
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return TimeSpanFormat.Format(this, format, formatProvider);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00042170 File Offset: 0x00040370
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider formatProvider = null)
		{
			return TimeSpanFormat.TryFormat(this, destination, out charsWritten, format, formatProvider);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00042182 File Offset: 0x00040382
		public static TimeSpan operator -(TimeSpan t)
		{
			if (t._ticks == TimeSpan.MinValue._ticks)
			{
				throw new OverflowException("Negating the minimum value of a twos complement number is invalid.");
			}
			return new TimeSpan(-t._ticks);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x000421AD File Offset: 0x000403AD
		public static TimeSpan operator -(TimeSpan t1, TimeSpan t2)
		{
			return t1.Subtract(t2);
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0000270D File Offset: 0x0000090D
		public static TimeSpan operator +(TimeSpan t)
		{
			return t;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x000421B7 File Offset: 0x000403B7
		public static TimeSpan operator +(TimeSpan t1, TimeSpan t2)
		{
			return t1.Add(t2);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x000421C4 File Offset: 0x000403C4
		public static TimeSpan operator *(TimeSpan timeSpan, double factor)
		{
			if (double.IsNaN(factor))
			{
				throw new ArgumentException("TimeSpan does not accept floating point Not-a-Number values.", "factor");
			}
			double num = Math.Round((double)timeSpan.Ticks * factor);
			if (num > 9.223372036854776E+18 | num < -9.223372036854776E+18)
			{
				throw new OverflowException("TimeSpan overflowed because the duration is too long.");
			}
			return TimeSpan.FromTicks((long)num);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00042226 File Offset: 0x00040426
		public static TimeSpan operator *(double factor, TimeSpan timeSpan)
		{
			return timeSpan * factor;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00042230 File Offset: 0x00040430
		public static TimeSpan operator /(TimeSpan timeSpan, double divisor)
		{
			if (double.IsNaN(divisor))
			{
				throw new ArgumentException("TimeSpan does not accept floating point Not-a-Number values.", "divisor");
			}
			double num = Math.Round((double)timeSpan.Ticks / divisor);
			if ((num > 9.223372036854776E+18 | num < -9.223372036854776E+18) || double.IsNaN(num))
			{
				throw new OverflowException("TimeSpan overflowed because the duration is too long.");
			}
			return TimeSpan.FromTicks((long)num);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0004229A File Offset: 0x0004049A
		public static double operator /(TimeSpan t1, TimeSpan t2)
		{
			return (double)t1.Ticks / (double)t2.Ticks;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00041D30 File Offset: 0x0003FF30
		public static bool operator ==(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks == t2._ticks;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x000422AD File Offset: 0x000404AD
		public static bool operator !=(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks != t2._ticks;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x000422C0 File Offset: 0x000404C0
		public static bool operator <(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks < t2._ticks;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x000422D0 File Offset: 0x000404D0
		public static bool operator <=(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks <= t2._ticks;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x000422E3 File Offset: 0x000404E3
		public static bool operator >(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks > t2._ticks;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x000422F3 File Offset: 0x000404F3
		public static bool operator >=(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks >= t2._ticks;
		}

		// Token: 0x04001300 RID: 4864
		public const long TicksPerMillisecond = 10000L;

		// Token: 0x04001301 RID: 4865
		private const double MillisecondsPerTick = 0.0001;

		// Token: 0x04001302 RID: 4866
		public const long TicksPerSecond = 10000000L;

		// Token: 0x04001303 RID: 4867
		private const double SecondsPerTick = 1E-07;

		// Token: 0x04001304 RID: 4868
		public const long TicksPerMinute = 600000000L;

		// Token: 0x04001305 RID: 4869
		private const double MinutesPerTick = 1.6666666666666667E-09;

		// Token: 0x04001306 RID: 4870
		public const long TicksPerHour = 36000000000L;

		// Token: 0x04001307 RID: 4871
		private const double HoursPerTick = 2.7777777777777777E-11;

		// Token: 0x04001308 RID: 4872
		public const long TicksPerDay = 864000000000L;

		// Token: 0x04001309 RID: 4873
		private const double DaysPerTick = 1.1574074074074074E-12;

		// Token: 0x0400130A RID: 4874
		private const int MillisPerSecond = 1000;

		// Token: 0x0400130B RID: 4875
		private const int MillisPerMinute = 60000;

		// Token: 0x0400130C RID: 4876
		private const int MillisPerHour = 3600000;

		// Token: 0x0400130D RID: 4877
		private const int MillisPerDay = 86400000;

		// Token: 0x0400130E RID: 4878
		internal const long MaxSeconds = 922337203685L;

		// Token: 0x0400130F RID: 4879
		internal const long MinSeconds = -922337203685L;

		// Token: 0x04001310 RID: 4880
		internal const long MaxMilliSeconds = 922337203685477L;

		// Token: 0x04001311 RID: 4881
		internal const long MinMilliSeconds = -922337203685477L;

		// Token: 0x04001312 RID: 4882
		internal const long TicksPerTenthSecond = 1000000L;

		// Token: 0x04001313 RID: 4883
		public static readonly TimeSpan Zero = new TimeSpan(0L);

		// Token: 0x04001314 RID: 4884
		public static readonly TimeSpan MaxValue = new TimeSpan(long.MaxValue);

		// Token: 0x04001315 RID: 4885
		public static readonly TimeSpan MinValue = new TimeSpan(long.MinValue);

		// Token: 0x04001316 RID: 4886
		internal readonly long _ticks;
	}
}
