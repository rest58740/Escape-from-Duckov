using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200010E RID: 270
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public readonly struct DateTimeOffset : IComparable, IFormattable, IComparable<DateTimeOffset>, IEquatable<DateTimeOffset>, ISerializable, IDeserializationCallback, ISpanFormattable
	{
		// Token: 0x06000A2D RID: 2605 RVA: 0x000271C8 File Offset: 0x000253C8
		public DateTimeOffset(long ticks, TimeSpan offset)
		{
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			DateTime dateTime = new DateTime(ticks);
			this._dateTime = DateTimeOffset.ValidateDate(dateTime, offset);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000271F8 File Offset: 0x000253F8
		public DateTimeOffset(DateTime dateTime)
		{
			TimeSpan localUtcOffset;
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				localUtcOffset = TimeZoneInfo.GetLocalUtcOffset(dateTime, TimeZoneInfoOptions.NoThrowOnInvalidTime);
			}
			else
			{
				localUtcOffset = new TimeSpan(0L);
			}
			this._offsetMinutes = DateTimeOffset.ValidateOffset(localUtcOffset);
			this._dateTime = DateTimeOffset.ValidateDate(dateTime, localUtcOffset);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0002723C File Offset: 0x0002543C
		public DateTimeOffset(DateTime dateTime, TimeSpan offset)
		{
			if (dateTime.Kind == DateTimeKind.Local)
			{
				if (offset != TimeZoneInfo.GetLocalUtcOffset(dateTime, TimeZoneInfoOptions.NoThrowOnInvalidTime))
				{
					throw new ArgumentException("The UTC Offset of the local dateTime parameter does not match the offset argument.", "offset");
				}
			}
			else if (dateTime.Kind == DateTimeKind.Utc && offset != TimeSpan.Zero)
			{
				throw new ArgumentException("The UTC Offset for Utc DateTime instances must be 0.", "offset");
			}
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this._dateTime = DateTimeOffset.ValidateDate(dateTime, offset);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x000272B2 File Offset: 0x000254B2
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, TimeSpan offset)
		{
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this._dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second), offset);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x000272DC File Offset: 0x000254DC
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, TimeSpan offset)
		{
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this._dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond), offset);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00027308 File Offset: 0x00025508
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, TimeSpan offset)
		{
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this._dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond, calendar), offset);
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x00027341 File Offset: 0x00025541
		public static DateTimeOffset Now
		{
			get
			{
				return new DateTimeOffset(DateTime.Now);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x0002734D File Offset: 0x0002554D
		public static DateTimeOffset UtcNow
		{
			get
			{
				return new DateTimeOffset(DateTime.UtcNow);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x00027359 File Offset: 0x00025559
		public DateTime DateTime
		{
			get
			{
				return this.ClockDateTime;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x00027361 File Offset: 0x00025561
		public DateTime UtcDateTime
		{
			get
			{
				return DateTime.SpecifyKind(this._dateTime, DateTimeKind.Utc);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00027370 File Offset: 0x00025570
		public DateTime LocalDateTime
		{
			get
			{
				return this.UtcDateTime.ToLocalTime();
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0002738C File Offset: 0x0002558C
		public DateTimeOffset ToOffset(TimeSpan offset)
		{
			return new DateTimeOffset((this._dateTime + offset).Ticks, offset);
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x000273B4 File Offset: 0x000255B4
		private DateTime ClockDateTime
		{
			get
			{
				return new DateTime((this._dateTime + this.Offset).Ticks, DateTimeKind.Unspecified);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x000273E0 File Offset: 0x000255E0
		public DateTime Date
		{
			get
			{
				return this.ClockDateTime.Date;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x000273FC File Offset: 0x000255FC
		public int Day
		{
			get
			{
				return this.ClockDateTime.Day;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00027418 File Offset: 0x00025618
		public DayOfWeek DayOfWeek
		{
			get
			{
				return this.ClockDateTime.DayOfWeek;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00027434 File Offset: 0x00025634
		public int DayOfYear
		{
			get
			{
				return this.ClockDateTime.DayOfYear;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00027450 File Offset: 0x00025650
		public int Hour
		{
			get
			{
				return this.ClockDateTime.Hour;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0002746C File Offset: 0x0002566C
		public int Millisecond
		{
			get
			{
				return this.ClockDateTime.Millisecond;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x00027488 File Offset: 0x00025688
		public int Minute
		{
			get
			{
				return this.ClockDateTime.Minute;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x000274A4 File Offset: 0x000256A4
		public int Month
		{
			get
			{
				return this.ClockDateTime.Month;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x000274BF File Offset: 0x000256BF
		public TimeSpan Offset
		{
			get
			{
				return new TimeSpan(0, (int)this._offsetMinutes, 0);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x000274D0 File Offset: 0x000256D0
		public int Second
		{
			get
			{
				return this.ClockDateTime.Second;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x000274EC File Offset: 0x000256EC
		public long Ticks
		{
			get
			{
				return this.ClockDateTime.Ticks;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x00027508 File Offset: 0x00025708
		public long UtcTicks
		{
			get
			{
				return this.UtcDateTime.Ticks;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x00027524 File Offset: 0x00025724
		public TimeSpan TimeOfDay
		{
			get
			{
				return this.ClockDateTime.TimeOfDay;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00027540 File Offset: 0x00025740
		public int Year
		{
			get
			{
				return this.ClockDateTime.Year;
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0002755C File Offset: 0x0002575C
		public DateTimeOffset Add(TimeSpan timeSpan)
		{
			return new DateTimeOffset(this.ClockDateTime.Add(timeSpan), this.Offset);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00027584 File Offset: 0x00025784
		public DateTimeOffset AddDays(double days)
		{
			return new DateTimeOffset(this.ClockDateTime.AddDays(days), this.Offset);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000275AC File Offset: 0x000257AC
		public DateTimeOffset AddHours(double hours)
		{
			return new DateTimeOffset(this.ClockDateTime.AddHours(hours), this.Offset);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000275D4 File Offset: 0x000257D4
		public DateTimeOffset AddMilliseconds(double milliseconds)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMilliseconds(milliseconds), this.Offset);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000275FC File Offset: 0x000257FC
		public DateTimeOffset AddMinutes(double minutes)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMinutes(minutes), this.Offset);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00027624 File Offset: 0x00025824
		public DateTimeOffset AddMonths(int months)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMonths(months), this.Offset);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0002764C File Offset: 0x0002584C
		public DateTimeOffset AddSeconds(double seconds)
		{
			return new DateTimeOffset(this.ClockDateTime.AddSeconds(seconds), this.Offset);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00027674 File Offset: 0x00025874
		public DateTimeOffset AddTicks(long ticks)
		{
			return new DateTimeOffset(this.ClockDateTime.AddTicks(ticks), this.Offset);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0002769C File Offset: 0x0002589C
		public DateTimeOffset AddYears(int years)
		{
			return new DateTimeOffset(this.ClockDateTime.AddYears(years), this.Offset);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000276C3 File Offset: 0x000258C3
		public static int Compare(DateTimeOffset first, DateTimeOffset second)
		{
			return DateTime.Compare(first.UtcDateTime, second.UtcDateTime);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000276D8 File Offset: 0x000258D8
		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is DateTimeOffset))
			{
				throw new ArgumentException("Object must be of type DateTimeOffset.");
			}
			DateTime utcDateTime = ((DateTimeOffset)obj).UtcDateTime;
			DateTime utcDateTime2 = this.UtcDateTime;
			if (utcDateTime2 > utcDateTime)
			{
				return 1;
			}
			if (utcDateTime2 < utcDateTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0002772C File Offset: 0x0002592C
		public int CompareTo(DateTimeOffset other)
		{
			DateTime utcDateTime = other.UtcDateTime;
			DateTime utcDateTime2 = this.UtcDateTime;
			if (utcDateTime2 > utcDateTime)
			{
				return 1;
			}
			if (utcDateTime2 < utcDateTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00027760 File Offset: 0x00025960
		public override bool Equals(object obj)
		{
			return obj is DateTimeOffset && this.UtcDateTime.Equals(((DateTimeOffset)obj).UtcDateTime);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00027794 File Offset: 0x00025994
		public bool Equals(DateTimeOffset other)
		{
			return this.UtcDateTime.Equals(other.UtcDateTime);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x000277B8 File Offset: 0x000259B8
		public bool EqualsExact(DateTimeOffset other)
		{
			return this.ClockDateTime == other.ClockDateTime && this.Offset == other.Offset && this.ClockDateTime.Kind == other.ClockDateTime.Kind;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0002780E File Offset: 0x00025A0E
		public static bool Equals(DateTimeOffset first, DateTimeOffset second)
		{
			return DateTime.Equals(first.UtcDateTime, second.UtcDateTime);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00027823 File Offset: 0x00025A23
		public static DateTimeOffset FromFileTime(long fileTime)
		{
			return new DateTimeOffset(DateTime.FromFileTime(fileTime));
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00027830 File Offset: 0x00025A30
		public static DateTimeOffset FromUnixTimeSeconds(long seconds)
		{
			if (seconds < -62135596800L || seconds > 253402300799L)
			{
				throw new ArgumentOutOfRangeException("seconds", SR.Format("Valid values are between {0} and {1}, inclusive.", -62135596800L, 253402300799L));
			}
			return new DateTimeOffset(seconds * 10000000L + 621355968000000000L, TimeSpan.Zero);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x000278A4 File Offset: 0x00025AA4
		public static DateTimeOffset FromUnixTimeMilliseconds(long milliseconds)
		{
			if (milliseconds < -62135596800000L || milliseconds > 253402300799999L)
			{
				throw new ArgumentOutOfRangeException("milliseconds", SR.Format("Valid values are between {0} and {1}, inclusive.", -62135596800000L, 253402300799999L));
			}
			return new DateTimeOffset(milliseconds * 10000L + 621355968000000000L, TimeSpan.Zero);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00027918 File Offset: 0x00025B18
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			try
			{
				DateTimeOffset.ValidateOffset(this.Offset);
				DateTimeOffset.ValidateDate(this.ClockDateTime, this.Offset);
			}
			catch (ArgumentException innerException)
			{
				throw new SerializationException("An error occurred while deserializing the object.  The serialized data is corrupt.", innerException);
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00027964 File Offset: 0x00025B64
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("DateTime", this._dateTime);
			info.AddValue("OffsetMinutes", this._offsetMinutes);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00027998 File Offset: 0x00025B98
		private DateTimeOffset(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this._dateTime = (DateTime)info.GetValue("DateTime", typeof(DateTime));
			this._offsetMinutes = (short)info.GetValue("OffsetMinutes", typeof(short));
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000279F4 File Offset: 0x00025BF4
		public override int GetHashCode()
		{
			return this.UtcDateTime.GetHashCode();
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00027A10 File Offset: 0x00025C10
		public static DateTimeOffset Parse(string input)
		{
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.Parse(input, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out offset).Ticks, offset);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00027A48 File Offset: 0x00025C48
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider)
		{
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			return DateTimeOffset.Parse(input, formatProvider, DateTimeStyles.None);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00027A5C File Offset: 0x00025C5C
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.Parse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles, out offset).Ticks, offset);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00027AA4 File Offset: 0x00025CA4
		public static DateTimeOffset Parse(ReadOnlySpan<char> input, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.Parse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles, out offset).Ticks, offset);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00027ADB File Offset: 0x00025CDB
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider)
		{
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			if (format == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
			}
			return DateTimeOffset.ParseExact(input, format, formatProvider, DateTimeStyles.None);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00027AFC File Offset: 0x00025CFC
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			if (format == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
			}
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.ParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles, out offset).Ticks, offset);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00027B54 File Offset: 0x00025D54
		public static DateTimeOffset ParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format, IFormatProvider formatProvider, DateTimeStyles styles = DateTimeStyles.None)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.ParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles, out offset).Ticks, offset);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00027B8C File Offset: 0x00025D8C
		public static DateTimeOffset ParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			if (input == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
			}
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.ParseExactMultiple(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out offset).Ticks, offset);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00027BD4 File Offset: 0x00025DD4
		public static DateTimeOffset ParseExact(ReadOnlySpan<char> input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles = DateTimeStyles.None)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			TimeSpan offset;
			return new DateTimeOffset(DateTimeParse.ParseExactMultiple(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out offset).Ticks, offset);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00027C0C File Offset: 0x00025E0C
		public TimeSpan Subtract(DateTimeOffset value)
		{
			return this.UtcDateTime.Subtract(value.UtcDateTime);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00027C30 File Offset: 0x00025E30
		public DateTimeOffset Subtract(TimeSpan value)
		{
			return new DateTimeOffset(this.ClockDateTime.Subtract(value), this.Offset);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00027C58 File Offset: 0x00025E58
		public long ToFileTime()
		{
			return this.UtcDateTime.ToFileTime();
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00027C74 File Offset: 0x00025E74
		public long ToUnixTimeSeconds()
		{
			return this.UtcDateTime.Ticks / 10000000L - 62135596800L;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00027CA0 File Offset: 0x00025EA0
		public long ToUnixTimeMilliseconds()
		{
			return this.UtcDateTime.Ticks / 10000L - 62135596800000L;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00027CCC File Offset: 0x00025ECC
		public DateTimeOffset ToLocalTime()
		{
			return this.ToLocalTime(false);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00027CD8 File Offset: 0x00025ED8
		internal DateTimeOffset ToLocalTime(bool throwOnOverflow)
		{
			return new DateTimeOffset(this.UtcDateTime.ToLocalTime(throwOnOverflow));
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00027CF9 File Offset: 0x00025EF9
		public override string ToString()
		{
			return DateTimeFormat.Format(this.ClockDateTime, null, null, this.Offset);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00027D0E File Offset: 0x00025F0E
		public string ToString(string format)
		{
			return DateTimeFormat.Format(this.ClockDateTime, format, null, this.Offset);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00027D23 File Offset: 0x00025F23
		public string ToString(IFormatProvider formatProvider)
		{
			return DateTimeFormat.Format(this.ClockDateTime, null, formatProvider, this.Offset);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00027D38 File Offset: 0x00025F38
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return DateTimeFormat.Format(this.ClockDateTime, format, formatProvider, this.Offset);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00027D4D File Offset: 0x00025F4D
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider formatProvider = null)
		{
			return DateTimeFormat.TryFormat(this.ClockDateTime, destination, out charsWritten, format, formatProvider, this.Offset);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00027D65 File Offset: 0x00025F65
		public DateTimeOffset ToUniversalTime()
		{
			return new DateTimeOffset(this.UtcDateTime);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00027D74 File Offset: 0x00025F74
		public static bool TryParse(string input, out DateTimeOffset result)
		{
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParse(input, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00027DAC File Offset: 0x00025FAC
		public static bool TryParse(ReadOnlySpan<char> input, out DateTimeOffset result)
		{
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParse(input, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00027DDC File Offset: 0x00025FDC
		public static bool TryParse(string input, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			if (input == null)
			{
				result = default(DateTimeOffset);
				return false;
			}
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00027E2C File Offset: 0x0002602C
		public static bool TryParse(ReadOnlySpan<char> input, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00027E6C File Offset: 0x0002606C
		public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			if (input == null || format == null)
			{
				result = default(DateTimeOffset);
				return false;
			}
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00027EC8 File Offset: 0x000260C8
		public static bool TryParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00027F08 File Offset: 0x00026108
		public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			if (input == null)
			{
				result = default(DateTimeOffset);
				return false;
			}
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParseExactMultiple(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00027F5C File Offset: 0x0002615C
		public static bool TryParseExact(ReadOnlySpan<char> input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			DateTime dateTime;
			TimeSpan offset;
			bool result2 = DateTimeParse.TryParseExactMultiple(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out offset);
			result = new DateTimeOffset(dateTime.Ticks, offset);
			return result2;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00027F9C File Offset: 0x0002619C
		private static short ValidateOffset(TimeSpan offset)
		{
			long ticks = offset.Ticks;
			if (ticks % 600000000L != 0L)
			{
				throw new ArgumentException("Offset must be specified in whole minutes.", "offset");
			}
			if (ticks < -504000000000L || ticks > 504000000000L)
			{
				throw new ArgumentOutOfRangeException("offset", "Offset must be within plus or minus 14 hours.");
			}
			return (short)(offset.Ticks / 600000000L);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00028004 File Offset: 0x00026204
		private static DateTime ValidateDate(DateTime dateTime, TimeSpan offset)
		{
			long num = dateTime.Ticks - offset.Ticks;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("offset", "The UTC time represented when the offset is applied must be between year 0 and 10,000.");
			}
			return new DateTime(num, DateTimeKind.Unspecified);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0002804C File Offset: 0x0002624C
		private static DateTimeStyles ValidateStyles(DateTimeStyles style, string parameterName)
		{
			if ((style & ~(DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal | DateTimeStyles.AssumeUniversal | DateTimeStyles.RoundtripKind)) != DateTimeStyles.None)
			{
				throw new ArgumentException("An undefined DateTimeStyles value is being used.", parameterName);
			}
			if ((style & DateTimeStyles.AssumeLocal) != DateTimeStyles.None && (style & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
			{
				throw new ArgumentException("The DateTimeStyles values AssumeLocal and AssumeUniversal cannot be used together.", parameterName);
			}
			if ((style & DateTimeStyles.NoCurrentDateDefault) != DateTimeStyles.None)
			{
				throw new ArgumentException("The DateTimeStyles value 'NoCurrentDateDefault' is not allowed when parsing DateTimeOffset.", parameterName);
			}
			style &= ~DateTimeStyles.RoundtripKind;
			style &= ~DateTimeStyles.AssumeLocal;
			return style;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x000280A7 File Offset: 0x000262A7
		public static implicit operator DateTimeOffset(DateTime dateTime)
		{
			return new DateTimeOffset(dateTime);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x000280AF File Offset: 0x000262AF
		public static DateTimeOffset operator +(DateTimeOffset dateTimeOffset, TimeSpan timeSpan)
		{
			return new DateTimeOffset(dateTimeOffset.ClockDateTime + timeSpan, dateTimeOffset.Offset);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x000280CA File Offset: 0x000262CA
		public static DateTimeOffset operator -(DateTimeOffset dateTimeOffset, TimeSpan timeSpan)
		{
			return new DateTimeOffset(dateTimeOffset.ClockDateTime - timeSpan, dateTimeOffset.Offset);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x000280E5 File Offset: 0x000262E5
		public static TimeSpan operator -(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime - right.UtcDateTime;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x000280FA File Offset: 0x000262FA
		public static bool operator ==(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime == right.UtcDateTime;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0002810F File Offset: 0x0002630F
		public static bool operator !=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime != right.UtcDateTime;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00028124 File Offset: 0x00026324
		public static bool operator <(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime < right.UtcDateTime;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00028139 File Offset: 0x00026339
		public static bool operator <=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime <= right.UtcDateTime;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0002814E File Offset: 0x0002634E
		public static bool operator >(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime > right.UtcDateTime;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00028163 File Offset: 0x00026363
		public static bool operator >=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime >= right.UtcDateTime;
		}

		// Token: 0x040010C6 RID: 4294
		internal const long MaxOffset = 504000000000L;

		// Token: 0x040010C7 RID: 4295
		internal const long MinOffset = -504000000000L;

		// Token: 0x040010C8 RID: 4296
		private const long UnixEpochSeconds = 62135596800L;

		// Token: 0x040010C9 RID: 4297
		private const long UnixEpochMilliseconds = 62135596800000L;

		// Token: 0x040010CA RID: 4298
		internal const long UnixMinSeconds = -62135596800L;

		// Token: 0x040010CB RID: 4299
		internal const long UnixMaxSeconds = 253402300799L;

		// Token: 0x040010CC RID: 4300
		public static readonly DateTimeOffset MinValue = new DateTimeOffset(0L, TimeSpan.Zero);

		// Token: 0x040010CD RID: 4301
		public static readonly DateTimeOffset MaxValue = new DateTimeOffset(3155378975999999999L, TimeSpan.Zero);

		// Token: 0x040010CE RID: 4302
		public static readonly DateTimeOffset UnixEpoch = new DateTimeOffset(621355968000000000L, TimeSpan.Zero);

		// Token: 0x040010CF RID: 4303
		private readonly DateTime _dateTime;

		// Token: 0x040010D0 RID: 4304
		private readonly short _offsetMinutes;
	}
}
