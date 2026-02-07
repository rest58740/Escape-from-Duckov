using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200010C RID: 268
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public readonly struct DateTime : IComparable, IFormattable, IConvertible, IComparable<DateTime>, IEquatable<DateTime>, ISerializable, ISpanFormattable
	{
		// Token: 0x060009A7 RID: 2471 RVA: 0x0002593F File Offset: 0x00023B3F
		public DateTime(long ticks)
		{
			if (ticks < 0L || ticks > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("ticks", "Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.");
			}
			this._dateData = (ulong)ticks;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00025969 File Offset: 0x00023B69
		private DateTime(ulong dateData)
		{
			this._dateData = dateData;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00025974 File Offset: 0x00023B74
		public DateTime(long ticks, DateTimeKind kind)
		{
			if (ticks < 0L || ticks > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("ticks", "Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.");
			}
			if (kind < DateTimeKind.Unspecified || kind > DateTimeKind.Local)
			{
				throw new ArgumentException("Invalid DateTimeKind value.", "kind");
			}
			this._dateData = (ulong)(ticks | (long)kind << 62);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x000259C8 File Offset: 0x00023BC8
		internal DateTime(long ticks, DateTimeKind kind, bool isAmbiguousDst)
		{
			if (ticks < 0L || ticks > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("ticks", "Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.");
			}
			this._dateData = (ulong)(ticks | (isAmbiguousDst ? -4611686018427387904L : long.MinValue));
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00025A15 File Offset: 0x00023C15
		public DateTime(int year, int month, int day)
		{
			this._dateData = (ulong)DateTime.DateToTicks(year, month, day);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00025A25 File Offset: 0x00023C25
		public DateTime(int year, int month, int day, Calendar calendar)
		{
			this = new DateTime(year, month, day, 0, 0, 0, calendar);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00025A35 File Offset: 0x00023C35
		public DateTime(int year, int month, int day, int hour, int minute, int second)
		{
			this._dateData = (ulong)(DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second));
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00025A54 File Offset: 0x00023C54
		public DateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
		{
			if (kind < DateTimeKind.Unspecified || kind > DateTimeKind.Local)
			{
				throw new ArgumentException("Invalid DateTimeKind value.", "kind");
			}
			long num = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second);
			this._dateData = (ulong)(num | (long)kind << 62);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00025AA0 File Offset: 0x00023CA0
		public DateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			this._dateData = (ulong)calendar.ToDateTime(year, month, day, hour, minute, second, 0).Ticks;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00025ADC File Offset: 0x00023CDC
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", SR.Format("Valid values are between {0} and {1}, inclusive.", 0, 999));
			}
			long num = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second);
			num += (long)millisecond * 10000L;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException("Combination of arguments to the DateTime constructor is out of the legal range.");
			}
			this._dateData = (ulong)num;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00025B64 File Offset: 0x00023D64
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
		{
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", SR.Format("Valid values are between {0} and {1}, inclusive.", 0, 999));
			}
			if (kind < DateTimeKind.Unspecified || kind > DateTimeKind.Local)
			{
				throw new ArgumentException("Invalid DateTimeKind value.", "kind");
			}
			long num = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second);
			num += (long)millisecond * 10000L;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException("Combination of arguments to the DateTime constructor is out of the legal range.");
			}
			this._dateData = (ulong)(num | (long)kind << 62);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00025C0C File Offset: 0x00023E0C
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", SR.Format("Valid values are between {0} and {1}, inclusive.", 0, 999));
			}
			long num = calendar.ToDateTime(year, month, day, hour, minute, second, 0).Ticks;
			num += (long)millisecond * 10000L;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException("Combination of arguments to the DateTime constructor is out of the legal range.");
			}
			this._dateData = (ulong)num;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00025CA8 File Offset: 0x00023EA8
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", SR.Format("Valid values are between {0} and {1}, inclusive.", 0, 999));
			}
			if (kind < DateTimeKind.Unspecified || kind > DateTimeKind.Local)
			{
				throw new ArgumentException("Invalid DateTimeKind value.", "kind");
			}
			long num = calendar.ToDateTime(year, month, day, hour, minute, second, 0).Ticks;
			num += (long)millisecond * 10000L;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException("Combination of arguments to the DateTime constructor is out of the legal range.");
			}
			this._dateData = (ulong)(num | (long)kind << 62);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00025D64 File Offset: 0x00023F64
		private DateTime(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			bool flag = false;
			bool flag2 = false;
			long dateData = 0L;
			ulong dateData2 = 0UL;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name = enumerator.Name;
				if (!(name == "ticks"))
				{
					if (name == "dateData")
					{
						dateData2 = Convert.ToUInt64(enumerator.Value, CultureInfo.InvariantCulture);
						flag2 = true;
					}
				}
				else
				{
					dateData = Convert.ToInt64(enumerator.Value, CultureInfo.InvariantCulture);
					flag = true;
				}
			}
			if (flag2)
			{
				this._dateData = dateData2;
			}
			else
			{
				if (!flag)
				{
					throw new SerializationException("Invalid serialized DateTime data. Unable to find 'ticks' or 'dateData'.");
				}
				this._dateData = (ulong)dateData;
			}
			long internalTicks = this.InternalTicks;
			if (internalTicks < 0L || internalTicks > 3155378975999999999L)
			{
				throw new SerializationException("Invalid serialized DateTime data. Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.");
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00025E36 File Offset: 0x00024036
		internal long InternalTicks
		{
			get
			{
				return (long)(this._dateData & 4611686018427387903UL);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x00025E48 File Offset: 0x00024048
		private ulong InternalKind
		{
			get
			{
				return this._dateData & 13835058055282163712UL;
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00025E5A File Offset: 0x0002405A
		public DateTime Add(TimeSpan value)
		{
			return this.AddTicks(value._ticks);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00025E68 File Offset: 0x00024068
		private DateTime Add(double value, int scale)
		{
			long num = (long)(value * (double)scale + ((value >= 0.0) ? 0.5 : -0.5));
			if (num <= -315537897600000L || num >= 315537897600000L)
			{
				throw new ArgumentOutOfRangeException("value", "Value to add was out of range.");
			}
			return this.AddTicks(num * 10000L);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x00025ED2 File Offset: 0x000240D2
		public DateTime AddDays(double value)
		{
			return this.Add(value, 86400000);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00025EE0 File Offset: 0x000240E0
		public DateTime AddHours(double value)
		{
			return this.Add(value, 3600000);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00025EEE File Offset: 0x000240EE
		public DateTime AddMilliseconds(double value)
		{
			return this.Add(value, 1);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00025EF8 File Offset: 0x000240F8
		public DateTime AddMinutes(double value)
		{
			return this.Add(value, 60000);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x00025F08 File Offset: 0x00024108
		public DateTime AddMonths(int months)
		{
			if (months < -120000 || months > 120000)
			{
				throw new ArgumentOutOfRangeException("months", "Months value must be between +/-120000.");
			}
			int num;
			int num2;
			int num3;
			this.GetDatePart(out num, out num2, out num3);
			int num4 = num2 - 1 + months;
			if (num4 >= 0)
			{
				num2 = num4 % 12 + 1;
				num += num4 / 12;
			}
			else
			{
				num2 = 12 + (num4 + 1) % 12;
				num += (num4 - 11) / 12;
			}
			if (num < 1 || num > 9999)
			{
				throw new ArgumentOutOfRangeException("months", "The added or subtracted value results in an un-representable DateTime.");
			}
			int num5 = DateTime.DaysInMonth(num, num2);
			if (num3 > num5)
			{
				num3 = num5;
			}
			return new DateTime((ulong)(DateTime.DateToTicks(num, num2, num3) + this.InternalTicks % 864000000000L | (long)this.InternalKind));
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x00025FC1 File Offset: 0x000241C1
		public DateTime AddSeconds(double value)
		{
			return this.Add(value, 1000);
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00025FD0 File Offset: 0x000241D0
		public DateTime AddTicks(long value)
		{
			long internalTicks = this.InternalTicks;
			if (value > 3155378975999999999L - internalTicks || value < 0L - internalTicks)
			{
				throw new ArgumentOutOfRangeException("value", "The added or subtracted value results in an un-representable DateTime.");
			}
			return new DateTime((ulong)(internalTicks + value | (long)this.InternalKind));
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00026018 File Offset: 0x00024218
		public DateTime AddYears(int value)
		{
			if (value < -10000 || value > 10000)
			{
				throw new ArgumentOutOfRangeException("years", "Years value must be between +/-10000.");
			}
			return this.AddMonths(value * 12);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00026044 File Offset: 0x00024244
		public static int Compare(DateTime t1, DateTime t2)
		{
			long internalTicks = t1.InternalTicks;
			long internalTicks2 = t2.InternalTicks;
			if (internalTicks > internalTicks2)
			{
				return 1;
			}
			if (internalTicks < internalTicks2)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0002606E File Offset: 0x0002426E
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is DateTime))
			{
				throw new ArgumentException("Object must be of type DateTime.");
			}
			return DateTime.Compare(this, (DateTime)value);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00026099 File Offset: 0x00024299
		public int CompareTo(DateTime value)
		{
			return DateTime.Compare(this, value);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x000260A8 File Offset: 0x000242A8
		private static long DateToTicks(int year, int month, int day)
		{
			if (year >= 1 && year <= 9999 && month >= 1 && month <= 12)
			{
				int[] array = DateTime.IsLeapYear(year) ? DateTime.s_daysToMonth366 : DateTime.s_daysToMonth365;
				if (day >= 1 && day <= array[month] - array[month - 1])
				{
					int num = year - 1;
					return (long)(num * 365 + num / 4 - num / 100 + num / 400 + array[month - 1] + day - 1) * 864000000000L;
				}
			}
			throw new ArgumentOutOfRangeException(null, "Year, Month, and Day parameters describe an un-representable DateTime.");
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0002612C File Offset: 0x0002432C
		private static long TimeToTicks(int hour, int minute, int second)
		{
			if (hour >= 0 && hour < 24 && minute >= 0 && minute < 60 && second >= 0 && second < 60)
			{
				return TimeSpan.TimeToTicks(hour, minute, second);
			}
			throw new ArgumentOutOfRangeException(null, "Hour, Minute, and Second parameters describe an un-representable DateTime.");
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00026160 File Offset: 0x00024360
		public static int DaysInMonth(int year, int month)
		{
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", "Month must be between one and twelve.");
			}
			int[] array = DateTime.IsLeapYear(year) ? DateTime.s_daysToMonth366 : DateTime.s_daysToMonth365;
			return array[month] - array[month - 1];
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000261A4 File Offset: 0x000243A4
		internal static long DoubleDateToTicks(double value)
		{
			if (value >= 2958466.0 || value <= -657435.0)
			{
				throw new ArgumentException(" Not a legal OleAut date.");
			}
			long num = (long)(value * 86400000.0 + ((value >= 0.0) ? 0.5 : -0.5));
			if (num < 0L)
			{
				num -= num % 86400000L * 2L;
			}
			num += 59926435200000L;
			if (num < 0L || num >= 315537897600000L)
			{
				throw new ArgumentException("OleAut date did not convert to a DateTime correctly.");
			}
			return num * 10000L;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00026248 File Offset: 0x00024448
		public override bool Equals(object value)
		{
			return value is DateTime && this.InternalTicks == ((DateTime)value).InternalTicks;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00026275 File Offset: 0x00024475
		public bool Equals(DateTime value)
		{
			return this.InternalTicks == value.InternalTicks;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00026286 File Offset: 0x00024486
		public static bool Equals(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks == t2.InternalTicks;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00026298 File Offset: 0x00024498
		public static DateTime FromBinary(long dateData)
		{
			if ((dateData & -9223372036854775808L) == 0L)
			{
				return DateTime.FromBinaryRaw(dateData);
			}
			long num = dateData & 4611686018427387903L;
			if (num > 4611685154427387904L)
			{
				num -= 4611686018427387904L;
			}
			bool isAmbiguousDst = false;
			long ticks;
			if (num < 0L)
			{
				ticks = TimeZoneInfo.GetLocalUtcOffset(DateTime.MinValue, TimeZoneInfoOptions.NoThrowOnInvalidTime).Ticks;
			}
			else if (num > 3155378975999999999L)
			{
				ticks = TimeZoneInfo.GetLocalUtcOffset(DateTime.MaxValue, TimeZoneInfoOptions.NoThrowOnInvalidTime).Ticks;
			}
			else
			{
				DateTime time = new DateTime(num, DateTimeKind.Utc);
				bool flag = false;
				ticks = TimeZoneInfo.GetUtcOffsetFromUtc(time, TimeZoneInfo.Local, out flag, out isAmbiguousDst).Ticks;
			}
			num += ticks;
			if (num < 0L)
			{
				num += 864000000000L;
			}
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException("The binary data must result in a DateTime with ticks between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.", "dateData");
			}
			return new DateTime(num, DateTimeKind.Local, isAmbiguousDst);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00026380 File Offset: 0x00024580
		internal static DateTime FromBinaryRaw(long dateData)
		{
			long num = dateData & 4611686018427387903L;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentException("The binary data must result in a DateTime with ticks between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.", "dateData");
			}
			return new DateTime((ulong)dateData);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x000263C0 File Offset: 0x000245C0
		public static DateTime FromFileTime(long fileTime)
		{
			return DateTime.FromFileTimeUtc(fileTime).ToLocalTime();
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x000263DB File Offset: 0x000245DB
		public static DateTime FromFileTimeUtc(long fileTime)
		{
			if (fileTime < 0L || fileTime > 2650467743999999999L)
			{
				throw new ArgumentOutOfRangeException("fileTime", "Not a valid Win32 FileTime.");
			}
			return new DateTime(fileTime + 504911232000000000L, DateTimeKind.Utc);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002640F File Offset: 0x0002460F
		public static DateTime FromOADate(double d)
		{
			return new DateTime(DateTime.DoubleDateToTicks(d), DateTimeKind.Unspecified);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0002641D File Offset: 0x0002461D
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("ticks", this.InternalTicks);
			info.AddValue("dateData", this._dateData);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002644F File Offset: 0x0002464F
		public bool IsDaylightSavingTime()
		{
			return this.Kind != DateTimeKind.Utc && TimeZoneInfo.Local.IsDaylightSavingTime(this, TimeZoneInfoOptions.NoThrowOnInvalidTime);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0002646D File Offset: 0x0002466D
		public static DateTime SpecifyKind(DateTime value, DateTimeKind kind)
		{
			return new DateTime(value.InternalTicks, kind);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0002647C File Offset: 0x0002467C
		public long ToBinary()
		{
			if (this.Kind == DateTimeKind.Local)
			{
				TimeSpan localUtcOffset = TimeZoneInfo.GetLocalUtcOffset(this, TimeZoneInfoOptions.NoThrowOnInvalidTime);
				long num = this.Ticks - localUtcOffset.Ticks;
				if (num < 0L)
				{
					num = 4611686018427387904L + num;
				}
				return num | long.MinValue;
			}
			return (long)this._dateData;
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x000264D1 File Offset: 0x000246D1
		public DateTime Date
		{
			get
			{
				long internalTicks = this.InternalTicks;
				return new DateTime((ulong)(internalTicks - internalTicks % 864000000000L | (long)this.InternalKind));
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x000264F4 File Offset: 0x000246F4
		private int GetDatePart(int part)
		{
			int i = (int)(this.InternalTicks / 864000000000L);
			int num = i / 146097;
			i -= num * 146097;
			int num2 = i / 36524;
			if (num2 == 4)
			{
				num2 = 3;
			}
			i -= num2 * 36524;
			int num3 = i / 1461;
			i -= num3 * 1461;
			int num4 = i / 365;
			if (num4 == 4)
			{
				num4 = 3;
			}
			if (part == 0)
			{
				return num * 400 + num2 * 100 + num3 * 4 + num4 + 1;
			}
			i -= num4 * 365;
			if (part == 1)
			{
				return i + 1;
			}
			int[] array = (num4 == 3 && (num3 != 24 || num2 == 3)) ? DateTime.s_daysToMonth366 : DateTime.s_daysToMonth365;
			int num5 = (i >> 5) + 1;
			while (i >= array[num5])
			{
				num5++;
			}
			if (part == 2)
			{
				return num5;
			}
			return i - array[num5 - 1] + 1;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x000265DC File Offset: 0x000247DC
		internal void GetDatePart(out int year, out int month, out int day)
		{
			int i = (int)(this.InternalTicks / 864000000000L);
			int num = i / 146097;
			i -= num * 146097;
			int num2 = i / 36524;
			if (num2 == 4)
			{
				num2 = 3;
			}
			i -= num2 * 36524;
			int num3 = i / 1461;
			i -= num3 * 1461;
			int num4 = i / 365;
			if (num4 == 4)
			{
				num4 = 3;
			}
			year = num * 400 + num2 * 100 + num3 * 4 + num4 + 1;
			i -= num4 * 365;
			int[] array = (num4 == 3 && (num3 != 24 || num2 == 3)) ? DateTime.s_daysToMonth366 : DateTime.s_daysToMonth365;
			int num5 = (i >> 5) + 1;
			while (i >= array[num5])
			{
				num5++;
			}
			month = num5;
			day = i - array[num5 - 1] + 1;
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x000266B6 File Offset: 0x000248B6
		public int Day
		{
			get
			{
				return this.GetDatePart(3);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x000266BF File Offset: 0x000248BF
		public DayOfWeek DayOfWeek
		{
			get
			{
				return (DayOfWeek)((this.InternalTicks / 864000000000L + 1L) % 7L);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x000266D8 File Offset: 0x000248D8
		public int DayOfYear
		{
			get
			{
				return this.GetDatePart(1);
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x000266E4 File Offset: 0x000248E4
		public override int GetHashCode()
		{
			long internalTicks = this.InternalTicks;
			return (int)internalTicks ^ (int)(internalTicks >> 32);
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x00026700 File Offset: 0x00024900
		public int Hour
		{
			get
			{
				return (int)(this.InternalTicks / 36000000000L % 24L);
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00026717 File Offset: 0x00024917
		internal bool IsAmbiguousDaylightSavingTime()
		{
			return this.InternalKind == 13835058055282163712UL;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0002672C File Offset: 0x0002492C
		public DateTimeKind Kind
		{
			get
			{
				ulong internalKind = this.InternalKind;
				if (internalKind == 0UL)
				{
					return DateTimeKind.Unspecified;
				}
				if (internalKind != 4611686018427387904UL)
				{
					return DateTimeKind.Local;
				}
				return DateTimeKind.Utc;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x00026756 File Offset: 0x00024956
		public int Millisecond
		{
			get
			{
				return (int)(this.InternalTicks / 10000L % 1000L);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0002676D File Offset: 0x0002496D
		public int Minute
		{
			get
			{
				return (int)(this.InternalTicks / 600000000L % 60L);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x00026781 File Offset: 0x00024981
		public int Month
		{
			get
			{
				return this.GetDatePart(2);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0002678C File Offset: 0x0002498C
		public static DateTime Now
		{
			get
			{
				DateTime utcNow = DateTime.UtcNow;
				bool isAmbiguousDst = false;
				long ticks = TimeZoneInfo.GetDateTimeNowUtcOffsetFromUtc(utcNow, out isAmbiguousDst).Ticks;
				long num = utcNow.Ticks + ticks;
				if (num > 3155378975999999999L)
				{
					return new DateTime(3155378975999999999L, DateTimeKind.Local);
				}
				if (num < 0L)
				{
					return new DateTime(0L, DateTimeKind.Local);
				}
				return new DateTime(num, DateTimeKind.Local, isAmbiguousDst);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x000267EF File Offset: 0x000249EF
		public int Second
		{
			get
			{
				return (int)(this.InternalTicks / 10000000L % 60L);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00026803 File Offset: 0x00024A03
		public long Ticks
		{
			get
			{
				return this.InternalTicks;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0002680B File Offset: 0x00024A0B
		public TimeSpan TimeOfDay
		{
			get
			{
				return new TimeSpan(this.InternalTicks % 864000000000L);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00026824 File Offset: 0x00024A24
		public static DateTime Today
		{
			get
			{
				return DateTime.Now.Date;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0002683E File Offset: 0x00024A3E
		public int Year
		{
			get
			{
				return this.GetDatePart(0);
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00026847 File Offset: 0x00024A47
		public static bool IsLeapYear(int year)
		{
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", "Year must be between 1 and 9999.");
			}
			return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0002687E File Offset: 0x00024A7E
		public static DateTime Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return DateTimeParse.Parse(s, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0002689B File Offset: 0x00024A9B
		public static DateTime Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return DateTimeParse.Parse(s, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x000268B9 File Offset: 0x00024AB9
		public static DateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles)
		{
			DateTimeFormatInfo.ValidateStyles(styles, "styles");
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return DateTimeParse.Parse(s, DateTimeFormatInfo.GetInstance(provider), styles);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x000268E2 File Offset: 0x00024AE2
		public static DateTime Parse(ReadOnlySpan<char> s, IFormatProvider provider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTimeFormatInfo.ValidateStyles(styles, "styles");
			return DateTimeParse.Parse(s, DateTimeFormatInfo.GetInstance(provider), styles);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x000268FC File Offset: 0x00024AFC
		public static DateTime ParseExact(string s, string format, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			if (format == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
			}
			return DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0002692A File Offset: 0x00024B2A
		public static DateTime ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			if (format == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
			}
			return DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00026963 File Offset: 0x00024B63
		public static DateTime ParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider provider, DateTimeStyles style = DateTimeStyles.None)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			return DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0002697E File Offset: 0x00024B7E
		public static DateTime ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return DateTimeParse.ParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x000269A8 File Offset: 0x00024BA8
		public static DateTime ParseExact(ReadOnlySpan<char> s, string[] formats, IFormatProvider provider, DateTimeStyles style = DateTimeStyles.None)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			return DateTimeParse.ParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x000269C3 File Offset: 0x00024BC3
		public TimeSpan Subtract(DateTime value)
		{
			return new TimeSpan(this.InternalTicks - value.InternalTicks);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x000269D8 File Offset: 0x00024BD8
		public DateTime Subtract(TimeSpan value)
		{
			long internalTicks = this.InternalTicks;
			long ticks = value._ticks;
			if (internalTicks < ticks || internalTicks - 3155378975999999999L > ticks)
			{
				throw new ArgumentOutOfRangeException("value", "The added or subtracted value results in an un-representable DateTime.");
			}
			return new DateTime((ulong)(internalTicks - ticks | (long)this.InternalKind));
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00026A24 File Offset: 0x00024C24
		private static double TicksToOADate(long value)
		{
			if (value == 0L)
			{
				return 0.0;
			}
			if (value < 864000000000L)
			{
				value += 599264352000000000L;
			}
			if (value < 31241376000000000L)
			{
				throw new OverflowException(" Not a legal OleAut date.");
			}
			long num = (value - 599264352000000000L) / 10000L;
			if (num < 0L)
			{
				long num2 = num % 86400000L;
				if (num2 != 0L)
				{
					num -= (86400000L + num2) * 2L;
				}
			}
			return (double)num / 86400000.0;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00026AAC File Offset: 0x00024CAC
		public double ToOADate()
		{
			return DateTime.TicksToOADate(this.InternalTicks);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00026ABC File Offset: 0x00024CBC
		public long ToFileTime()
		{
			return this.ToUniversalTime().ToFileTimeUtc();
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00026AD8 File Offset: 0x00024CD8
		public long ToFileTimeUtc()
		{
			long num = (((this.InternalKind & 9223372036854775808UL) != 0UL) ? this.ToUniversalTime().InternalTicks : this.InternalTicks) - 504911232000000000L;
			if (num < 0L)
			{
				throw new ArgumentOutOfRangeException(null, "Not a valid Win32 FileTime.");
			}
			return num;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00026B28 File Offset: 0x00024D28
		public DateTime ToLocalTime()
		{
			return this.ToLocalTime(false);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00026B34 File Offset: 0x00024D34
		internal DateTime ToLocalTime(bool throwOnOverflow)
		{
			if (this.Kind == DateTimeKind.Local)
			{
				return this;
			}
			bool flag = false;
			bool isAmbiguousDst = false;
			long ticks = TimeZoneInfo.GetUtcOffsetFromUtc(this, TimeZoneInfo.Local, out flag, out isAmbiguousDst).Ticks;
			long num = this.Ticks + ticks;
			if (num > 3155378975999999999L)
			{
				if (throwOnOverflow)
				{
					throw new ArgumentException("Specified argument was out of the range of valid values.");
				}
				return new DateTime(3155378975999999999L, DateTimeKind.Local);
			}
			else
			{
				if (num >= 0L)
				{
					return new DateTime(num, DateTimeKind.Local, isAmbiguousDst);
				}
				if (throwOnOverflow)
				{
					throw new ArgumentException("Specified argument was out of the range of valid values.");
				}
				return new DateTime(0L, DateTimeKind.Local);
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00026BCA File Offset: 0x00024DCA
		public string ToLongDateString()
		{
			return DateTimeFormat.Format(this, "D", null);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00026BDD File Offset: 0x00024DDD
		public string ToLongTimeString()
		{
			return DateTimeFormat.Format(this, "T", null);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00026BF0 File Offset: 0x00024DF0
		public string ToShortDateString()
		{
			return DateTimeFormat.Format(this, "d", null);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00026C03 File Offset: 0x00024E03
		public string ToShortTimeString()
		{
			return DateTimeFormat.Format(this, "t", null);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00026C16 File Offset: 0x00024E16
		public override string ToString()
		{
			return DateTimeFormat.Format(this, null, null);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00026C25 File Offset: 0x00024E25
		public string ToString(string format)
		{
			return DateTimeFormat.Format(this, format, null);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00026C34 File Offset: 0x00024E34
		public string ToString(IFormatProvider provider)
		{
			return DateTimeFormat.Format(this, null, provider);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00026C43 File Offset: 0x00024E43
		public string ToString(string format, IFormatProvider provider)
		{
			return DateTimeFormat.Format(this, format, provider);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00026C52 File Offset: 0x00024E52
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			return DateTimeFormat.TryFormat(this, destination, out charsWritten, format, provider);
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00026C64 File Offset: 0x00024E64
		public DateTime ToUniversalTime()
		{
			return TimeZoneInfo.ConvertTimeToUtc(this, TimeZoneInfoOptions.NoThrowOnInvalidTime);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00026C72 File Offset: 0x00024E72
		public static bool TryParse(string s, out DateTime result)
		{
			if (s == null)
			{
				result = default(DateTime);
				return false;
			}
			return DateTimeParse.TryParse(s, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out result);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00026C92 File Offset: 0x00024E92
		public static bool TryParse(ReadOnlySpan<char> s, out DateTime result)
		{
			return DateTimeParse.TryParse(s, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out result);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00026CA1 File Offset: 0x00024EA1
		public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
		{
			DateTimeFormatInfo.ValidateStyles(styles, "styles");
			if (s == null)
			{
				result = default(DateTime);
				return false;
			}
			return DateTimeParse.TryParse(s, DateTimeFormatInfo.GetInstance(provider), styles, out result);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00026CCD File Offset: 0x00024ECD
		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
		{
			DateTimeFormatInfo.ValidateStyles(styles, "styles");
			return DateTimeParse.TryParse(s, DateTimeFormatInfo.GetInstance(provider), styles, out result);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00026CE8 File Offset: 0x00024EE8
		public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style, out DateTime result)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			if (s == null || format == null)
			{
				result = default(DateTime);
				return false;
			}
			return DateTimeParse.TryParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style, out result);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00026D1F File Offset: 0x00024F1F
		public static bool TryParseExact(ReadOnlySpan<char> s, ReadOnlySpan<char> format, IFormatProvider provider, DateTimeStyles style, out DateTime result)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			return DateTimeParse.TryParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style, out result);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00026D3C File Offset: 0x00024F3C
		public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out DateTime result)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			if (s == null)
			{
				result = default(DateTime);
				return false;
			}
			return DateTimeParse.TryParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style, out result);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00026D6B File Offset: 0x00024F6B
		public static bool TryParseExact(ReadOnlySpan<char> s, string[] formats, IFormatProvider provider, DateTimeStyles style, out DateTime result)
		{
			DateTimeFormatInfo.ValidateStyles(style, "style");
			return DateTimeParse.TryParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style, out result);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00026D88 File Offset: 0x00024F88
		public static DateTime operator +(DateTime d, TimeSpan t)
		{
			long internalTicks = d.InternalTicks;
			long ticks = t._ticks;
			if (ticks > 3155378975999999999L - internalTicks || ticks < 0L - internalTicks)
			{
				throw new ArgumentOutOfRangeException("t", "The added or subtracted value results in an un-representable DateTime.");
			}
			return new DateTime((ulong)(internalTicks + ticks | (long)d.InternalKind));
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00026DDC File Offset: 0x00024FDC
		public static DateTime operator -(DateTime d, TimeSpan t)
		{
			long internalTicks = d.InternalTicks;
			long ticks = t._ticks;
			if (internalTicks < ticks || internalTicks - 3155378975999999999L > ticks)
			{
				throw new ArgumentOutOfRangeException("t", "The added or subtracted value results in an un-representable DateTime.");
			}
			return new DateTime((ulong)(internalTicks - ticks | (long)d.InternalKind));
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00026E2A File Offset: 0x0002502A
		public static TimeSpan operator -(DateTime d1, DateTime d2)
		{
			return new TimeSpan(d1.InternalTicks - d2.InternalTicks);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00026286 File Offset: 0x00024486
		public static bool operator ==(DateTime d1, DateTime d2)
		{
			return d1.InternalTicks == d2.InternalTicks;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00026E40 File Offset: 0x00025040
		public static bool operator !=(DateTime d1, DateTime d2)
		{
			return d1.InternalTicks != d2.InternalTicks;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00026E55 File Offset: 0x00025055
		public static bool operator <(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks < t2.InternalTicks;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00026E67 File Offset: 0x00025067
		public static bool operator <=(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks <= t2.InternalTicks;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00026E7C File Offset: 0x0002507C
		public static bool operator >(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks > t2.InternalTicks;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00026E8E File Offset: 0x0002508E
		public static bool operator >=(DateTime t1, DateTime t2)
		{
			return t1.InternalTicks >= t2.InternalTicks;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00026EA3 File Offset: 0x000250A3
		public string[] GetDateTimeFormats()
		{
			return this.GetDateTimeFormats(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x00026EB0 File Offset: 0x000250B0
		public string[] GetDateTimeFormats(IFormatProvider provider)
		{
			return DateTimeFormat.GetAllDateTimes(this, DateTimeFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00026EC3 File Offset: 0x000250C3
		public string[] GetDateTimeFormats(char format)
		{
			return this.GetDateTimeFormats(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00026ED1 File Offset: 0x000250D1
		public string[] GetDateTimeFormats(char format, IFormatProvider provider)
		{
			return DateTimeFormat.GetAllDateTimes(this, format, DateTimeFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00026EE5 File Offset: 0x000250E5
		public TypeCode GetTypeCode()
		{
			return TypeCode.DateTime;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00026EE9 File Offset: 0x000250E9
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "Boolean"));
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x00026F04 File Offset: 0x00025104
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "Char"));
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00026F1F File Offset: 0x0002511F
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "SByte"));
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00026F3A File Offset: 0x0002513A
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "Byte"));
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00026F55 File Offset: 0x00025155
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "Int16"));
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00026F70 File Offset: 0x00025170
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "UInt16"));
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00026F8B File Offset: 0x0002518B
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "Int32"));
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00026FA6 File Offset: 0x000251A6
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "UInt32"));
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00026FC1 File Offset: 0x000251C1
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "Int64"));
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00026FDC File Offset: 0x000251DC
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "UInt64"));
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00026FF7 File Offset: 0x000251F7
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "Single"));
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00027012 File Offset: 0x00025212
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "Double"));
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0002702D File Offset: 0x0002522D
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "DateTime", "Decimal"));
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00027048 File Offset: 0x00025248
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00027050 File Offset: 0x00025250
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00027064 File Offset: 0x00025264
		internal static bool TryCreate(int year, int month, int day, int hour, int minute, int second, int millisecond, out DateTime result)
		{
			result = DateTime.MinValue;
			if (year < 1 || year > 9999 || month < 1 || month > 12)
			{
				return false;
			}
			int[] array = DateTime.IsLeapYear(year) ? DateTime.s_daysToMonth366 : DateTime.s_daysToMonth365;
			if (day < 1 || day > array[month] - array[month - 1])
			{
				return false;
			}
			if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60 || second < 0 || second >= 60)
			{
				return false;
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				return false;
			}
			long num = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second);
			num += (long)millisecond * 10000L;
			if (num < 0L || num > 3155378975999999999L)
			{
				return false;
			}
			result = new DateTime(num, DateTimeKind.Unspecified);
			return true;
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0002712F File Offset: 0x0002532F
		public static DateTime UtcNow
		{
			get
			{
				return new DateTime((ulong)(DateTime.GetSystemTimeAsFileTime() + 504911232000000000L | 4611686018427387904L));
			}
		}

		// Token: 0x06000A2A RID: 2602
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern long GetSystemTimeAsFileTime();

		// Token: 0x06000A2B RID: 2603 RVA: 0x0002714F File Offset: 0x0002534F
		internal long ToBinaryRaw()
		{
			return (long)this._dateData;
		}

		// Token: 0x04001093 RID: 4243
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x04001094 RID: 4244
		private const long TicksPerSecond = 10000000L;

		// Token: 0x04001095 RID: 4245
		private const long TicksPerMinute = 600000000L;

		// Token: 0x04001096 RID: 4246
		private const long TicksPerHour = 36000000000L;

		// Token: 0x04001097 RID: 4247
		private const long TicksPerDay = 864000000000L;

		// Token: 0x04001098 RID: 4248
		private const int MillisPerSecond = 1000;

		// Token: 0x04001099 RID: 4249
		private const int MillisPerMinute = 60000;

		// Token: 0x0400109A RID: 4250
		private const int MillisPerHour = 3600000;

		// Token: 0x0400109B RID: 4251
		private const int MillisPerDay = 86400000;

		// Token: 0x0400109C RID: 4252
		private const int DaysPerYear = 365;

		// Token: 0x0400109D RID: 4253
		private const int DaysPer4Years = 1461;

		// Token: 0x0400109E RID: 4254
		private const int DaysPer100Years = 36524;

		// Token: 0x0400109F RID: 4255
		private const int DaysPer400Years = 146097;

		// Token: 0x040010A0 RID: 4256
		private const int DaysTo1601 = 584388;

		// Token: 0x040010A1 RID: 4257
		private const int DaysTo1899 = 693593;

		// Token: 0x040010A2 RID: 4258
		internal const int DaysTo1970 = 719162;

		// Token: 0x040010A3 RID: 4259
		private const int DaysTo10000 = 3652059;

		// Token: 0x040010A4 RID: 4260
		internal const long MinTicks = 0L;

		// Token: 0x040010A5 RID: 4261
		internal const long MaxTicks = 3155378975999999999L;

		// Token: 0x040010A6 RID: 4262
		private const long MaxMillis = 315537897600000L;

		// Token: 0x040010A7 RID: 4263
		internal const long UnixEpochTicks = 621355968000000000L;

		// Token: 0x040010A8 RID: 4264
		private const long FileTimeOffset = 504911232000000000L;

		// Token: 0x040010A9 RID: 4265
		private const long DoubleDateOffset = 599264352000000000L;

		// Token: 0x040010AA RID: 4266
		private const long OADateMinAsTicks = 31241376000000000L;

		// Token: 0x040010AB RID: 4267
		private const double OADateMinAsDouble = -657435.0;

		// Token: 0x040010AC RID: 4268
		private const double OADateMaxAsDouble = 2958466.0;

		// Token: 0x040010AD RID: 4269
		private const int DatePartYear = 0;

		// Token: 0x040010AE RID: 4270
		private const int DatePartDayOfYear = 1;

		// Token: 0x040010AF RID: 4271
		private const int DatePartMonth = 2;

		// Token: 0x040010B0 RID: 4272
		private const int DatePartDay = 3;

		// Token: 0x040010B1 RID: 4273
		private static readonly int[] s_daysToMonth365 = new int[]
		{
			0,
			31,
			59,
			90,
			120,
			151,
			181,
			212,
			243,
			273,
			304,
			334,
			365
		};

		// Token: 0x040010B2 RID: 4274
		private static readonly int[] s_daysToMonth366 = new int[]
		{
			0,
			31,
			60,
			91,
			121,
			152,
			182,
			213,
			244,
			274,
			305,
			335,
			366
		};

		// Token: 0x040010B3 RID: 4275
		public static readonly DateTime MinValue = new DateTime(0L, DateTimeKind.Unspecified);

		// Token: 0x040010B4 RID: 4276
		public static readonly DateTime MaxValue = new DateTime(3155378975999999999L, DateTimeKind.Unspecified);

		// Token: 0x040010B5 RID: 4277
		public static readonly DateTime UnixEpoch = new DateTime(621355968000000000L, DateTimeKind.Utc);

		// Token: 0x040010B6 RID: 4278
		private const ulong TicksMask = 4611686018427387903UL;

		// Token: 0x040010B7 RID: 4279
		private const ulong FlagsMask = 13835058055282163712UL;

		// Token: 0x040010B8 RID: 4280
		private const ulong LocalMask = 9223372036854775808UL;

		// Token: 0x040010B9 RID: 4281
		private const long TicksCeiling = 4611686018427387904L;

		// Token: 0x040010BA RID: 4282
		private const ulong KindUnspecified = 0UL;

		// Token: 0x040010BB RID: 4283
		private const ulong KindUtc = 4611686018427387904UL;

		// Token: 0x040010BC RID: 4284
		private const ulong KindLocal = 9223372036854775808UL;

		// Token: 0x040010BD RID: 4285
		private const ulong KindLocalAmbiguousDst = 13835058055282163712UL;

		// Token: 0x040010BE RID: 4286
		private const int KindShift = 62;

		// Token: 0x040010BF RID: 4287
		private const string TicksField = "ticks";

		// Token: 0x040010C0 RID: 4288
		private const string DateDataField = "dateData";

		// Token: 0x040010C1 RID: 4289
		private readonly ulong _dateData;
	}
}
