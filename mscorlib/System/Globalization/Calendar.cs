using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System.Globalization
{
	// Token: 0x02000983 RID: 2435
	[ComVisible(true)]
	[Serializable]
	public abstract class Calendar : ICloneable
	{
		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x060055F2 RID: 22002 RVA: 0x00122746 File Offset: 0x00120946
		[ComVisible(false)]
		public virtual DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x060055F3 RID: 22003 RVA: 0x0012274D File Offset: 0x0012094D
		[ComVisible(false)]
		public virtual DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x060055F5 RID: 22005 RVA: 0x0012276A File Offset: 0x0012096A
		internal virtual int ID
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x060055F6 RID: 22006 RVA: 0x0012276D File Offset: 0x0012096D
		internal virtual int BaseCalendarID
		{
			get
			{
				return this.ID;
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x060055F7 RID: 22007 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[ComVisible(false)]
		public virtual CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.Unknown;
			}
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x060055F8 RID: 22008 RVA: 0x00122775 File Offset: 0x00120975
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x060055F9 RID: 22009 RVA: 0x0012277D File Offset: 0x0012097D
		[ComVisible(false)]
		public virtual object Clone()
		{
			object obj = base.MemberwiseClone();
			((Calendar)obj).SetReadOnlyState(false);
			return obj;
		}

		// Token: 0x060055FA RID: 22010 RVA: 0x00122791 File Offset: 0x00120991
		[ComVisible(false)]
		public static Calendar ReadOnly(Calendar calendar)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			if (calendar.IsReadOnly)
			{
				return calendar;
			}
			Calendar calendar2 = (Calendar)calendar.MemberwiseClone();
			calendar2.SetReadOnlyState(true);
			return calendar2;
		}

		// Token: 0x060055FB RID: 22011 RVA: 0x001227BD File Offset: 0x001209BD
		internal void VerifyWritable()
		{
			if (this.m_isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Instance is read-only."));
			}
		}

		// Token: 0x060055FC RID: 22012 RVA: 0x001227D7 File Offset: 0x001209D7
		internal void SetReadOnlyState(bool readOnly)
		{
			this.m_isReadOnly = readOnly;
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x060055FD RID: 22013 RVA: 0x001227E0 File Offset: 0x001209E0
		internal virtual int CurrentEraValue
		{
			get
			{
				if (this.m_currentEraValue == -1)
				{
					this.m_currentEraValue = CalendarData.GetCalendarData(this.BaseCalendarID).iCurrentEra;
				}
				return this.m_currentEraValue;
			}
		}

		// Token: 0x060055FE RID: 22014 RVA: 0x00122807 File Offset: 0x00120A07
		internal static void CheckAddResult(long ticks, DateTime minValue, DateTime maxValue)
		{
			if (ticks < minValue.Ticks || ticks > maxValue.Ticks)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("The result is out of the supported range for this calendar. The result should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive."), minValue, maxValue));
			}
		}

		// Token: 0x060055FF RID: 22015 RVA: 0x00122844 File Offset: 0x00120A44
		internal DateTime Add(DateTime time, double value, int scale)
		{
			double num = value * (double)scale + ((value >= 0.0) ? 0.5 : -0.5);
			if (num <= -315537897600000.0 || num >= 315537897600000.0)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("Value to add was out of range."));
			}
			long num2 = (long)num;
			long ticks = time.Ticks + num2 * 10000L;
			Calendar.CheckAddResult(ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(ticks);
		}

		// Token: 0x06005600 RID: 22016 RVA: 0x001228CE File Offset: 0x00120ACE
		public virtual DateTime AddMilliseconds(DateTime time, double milliseconds)
		{
			return this.Add(time, milliseconds, 1);
		}

		// Token: 0x06005601 RID: 22017 RVA: 0x001228D9 File Offset: 0x00120AD9
		public virtual DateTime AddDays(DateTime time, int days)
		{
			return this.Add(time, (double)days, 86400000);
		}

		// Token: 0x06005602 RID: 22018 RVA: 0x001228E9 File Offset: 0x00120AE9
		public virtual DateTime AddHours(DateTime time, int hours)
		{
			return this.Add(time, (double)hours, 3600000);
		}

		// Token: 0x06005603 RID: 22019 RVA: 0x001228F9 File Offset: 0x00120AF9
		public virtual DateTime AddMinutes(DateTime time, int minutes)
		{
			return this.Add(time, (double)minutes, 60000);
		}

		// Token: 0x06005604 RID: 22020
		public abstract DateTime AddMonths(DateTime time, int months);

		// Token: 0x06005605 RID: 22021 RVA: 0x00122909 File Offset: 0x00120B09
		public virtual DateTime AddSeconds(DateTime time, int seconds)
		{
			return this.Add(time, (double)seconds, 1000);
		}

		// Token: 0x06005606 RID: 22022 RVA: 0x00122919 File Offset: 0x00120B19
		public virtual DateTime AddWeeks(DateTime time, int weeks)
		{
			return this.AddDays(time, weeks * 7);
		}

		// Token: 0x06005607 RID: 22023
		public abstract DateTime AddYears(DateTime time, int years);

		// Token: 0x06005608 RID: 22024
		public abstract int GetDayOfMonth(DateTime time);

		// Token: 0x06005609 RID: 22025
		public abstract DayOfWeek GetDayOfWeek(DateTime time);

		// Token: 0x0600560A RID: 22026
		public abstract int GetDayOfYear(DateTime time);

		// Token: 0x0600560B RID: 22027 RVA: 0x00122925 File Offset: 0x00120B25
		public virtual int GetDaysInMonth(int year, int month)
		{
			return this.GetDaysInMonth(year, month, 0);
		}

		// Token: 0x0600560C RID: 22028
		public abstract int GetDaysInMonth(int year, int month, int era);

		// Token: 0x0600560D RID: 22029 RVA: 0x00122930 File Offset: 0x00120B30
		public virtual int GetDaysInYear(int year)
		{
			return this.GetDaysInYear(year, 0);
		}

		// Token: 0x0600560E RID: 22030
		public abstract int GetDaysInYear(int year, int era);

		// Token: 0x0600560F RID: 22031
		public abstract int GetEra(DateTime time);

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06005610 RID: 22032
		public abstract int[] Eras { get; }

		// Token: 0x06005611 RID: 22033 RVA: 0x0012293A File Offset: 0x00120B3A
		public virtual int GetHour(DateTime time)
		{
			return (int)(time.Ticks / 36000000000L % 24L);
		}

		// Token: 0x06005612 RID: 22034 RVA: 0x00122952 File Offset: 0x00120B52
		public virtual double GetMilliseconds(DateTime time)
		{
			return (double)(time.Ticks / 10000L % 1000L);
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x0012296A File Offset: 0x00120B6A
		public virtual int GetMinute(DateTime time)
		{
			return (int)(time.Ticks / 600000000L % 60L);
		}

		// Token: 0x06005614 RID: 22036
		public abstract int GetMonth(DateTime time);

		// Token: 0x06005615 RID: 22037 RVA: 0x0012297F File Offset: 0x00120B7F
		public virtual int GetMonthsInYear(int year)
		{
			return this.GetMonthsInYear(year, 0);
		}

		// Token: 0x06005616 RID: 22038
		public abstract int GetMonthsInYear(int year, int era);

		// Token: 0x06005617 RID: 22039 RVA: 0x00122989 File Offset: 0x00120B89
		public virtual int GetSecond(DateTime time)
		{
			return (int)(time.Ticks / 10000000L % 60L);
		}

		// Token: 0x06005618 RID: 22040 RVA: 0x001229A0 File Offset: 0x00120BA0
		internal int GetFirstDayWeekOfYear(DateTime time, int firstDayOfWeek)
		{
			int num = this.GetDayOfYear(time) - 1;
			int num2 = (this.GetDayOfWeek(time) - (DayOfWeek)(num % 7) - firstDayOfWeek + 14) % 7;
			return (num + num2) / 7 + 1;
		}

		// Token: 0x06005619 RID: 22041 RVA: 0x001229D4 File Offset: 0x00120BD4
		private int GetWeekOfYearFullDays(DateTime time, int firstDayOfWeek, int fullDays)
		{
			int num = this.GetDayOfYear(time) - 1;
			int num2 = this.GetDayOfWeek(time) - (DayOfWeek)(num % 7);
			int num3 = (firstDayOfWeek - num2 + 14) % 7;
			if (num3 != 0 && num3 >= fullDays)
			{
				num3 -= 7;
			}
			int num4 = num - num3;
			if (num4 >= 0)
			{
				return num4 / 7 + 1;
			}
			if (time <= this.MinSupportedDateTime.AddDays((double)num))
			{
				return this.GetWeekOfYearOfMinSupportedDateTime(firstDayOfWeek, fullDays);
			}
			return this.GetWeekOfYearFullDays(time.AddDays((double)(-(double)(num + 1))), firstDayOfWeek, fullDays);
		}

		// Token: 0x0600561A RID: 22042 RVA: 0x00122A50 File Offset: 0x00120C50
		private int GetWeekOfYearOfMinSupportedDateTime(int firstDayOfWeek, int minimumDaysInFirstWeek)
		{
			int num = this.GetDayOfYear(this.MinSupportedDateTime) - 1;
			int num2 = this.GetDayOfWeek(this.MinSupportedDateTime) - (DayOfWeek)(num % 7);
			int num3 = (firstDayOfWeek + 7 - num2) % 7;
			if (num3 == 0 || num3 >= minimumDaysInFirstWeek)
			{
				return 1;
			}
			int num4 = this.DaysInYearBeforeMinSupportedYear - 1;
			int num5 = num2 - 1 - num4 % 7;
			int num6 = (firstDayOfWeek - num5 + 14) % 7;
			int num7 = num4 - num6;
			if (num6 >= minimumDaysInFirstWeek)
			{
				num7 += 7;
			}
			return num7 / 7 + 1;
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x0600561B RID: 22043 RVA: 0x00122AC2 File Offset: 0x00120CC2
		protected virtual int DaysInYearBeforeMinSupportedYear
		{
			get
			{
				return 365;
			}
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x00122ACC File Offset: 0x00120CCC
		public virtual int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			if (firstDayOfWeek < DayOfWeek.Sunday || firstDayOfWeek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException("firstDayOfWeek", Environment.GetResourceString("Valid values are between {0} and {1}, inclusive.", new object[]
				{
					DayOfWeek.Sunday,
					DayOfWeek.Saturday
				}));
			}
			switch (rule)
			{
			case CalendarWeekRule.FirstDay:
				return this.GetFirstDayWeekOfYear(time, (int)firstDayOfWeek);
			case CalendarWeekRule.FirstFullWeek:
				return this.GetWeekOfYearFullDays(time, (int)firstDayOfWeek, 7);
			case CalendarWeekRule.FirstFourDayWeek:
				return this.GetWeekOfYearFullDays(time, (int)firstDayOfWeek, 4);
			default:
				throw new ArgumentOutOfRangeException("rule", Environment.GetResourceString("Valid values are between {0} and {1}, inclusive.", new object[]
				{
					CalendarWeekRule.FirstDay,
					CalendarWeekRule.FirstFourDayWeek
				}));
			}
		}

		// Token: 0x0600561D RID: 22045
		public abstract int GetYear(DateTime time);

		// Token: 0x0600561E RID: 22046 RVA: 0x00122B6B File Offset: 0x00120D6B
		public virtual bool IsLeapDay(int year, int month, int day)
		{
			return this.IsLeapDay(year, month, day, 0);
		}

		// Token: 0x0600561F RID: 22047
		public abstract bool IsLeapDay(int year, int month, int day, int era);

		// Token: 0x06005620 RID: 22048 RVA: 0x00122B77 File Offset: 0x00120D77
		public virtual bool IsLeapMonth(int year, int month)
		{
			return this.IsLeapMonth(year, month, 0);
		}

		// Token: 0x06005621 RID: 22049
		public abstract bool IsLeapMonth(int year, int month, int era);

		// Token: 0x06005622 RID: 22050 RVA: 0x00122B82 File Offset: 0x00120D82
		[ComVisible(false)]
		public virtual int GetLeapMonth(int year)
		{
			return this.GetLeapMonth(year, 0);
		}

		// Token: 0x06005623 RID: 22051 RVA: 0x00122B8C File Offset: 0x00120D8C
		[ComVisible(false)]
		public virtual int GetLeapMonth(int year, int era)
		{
			if (!this.IsLeapYear(year, era))
			{
				return 0;
			}
			int monthsInYear = this.GetMonthsInYear(year, era);
			for (int i = 1; i <= monthsInYear; i++)
			{
				if (this.IsLeapMonth(year, i, era))
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x06005624 RID: 22052 RVA: 0x00122BC8 File Offset: 0x00120DC8
		public virtual bool IsLeapYear(int year)
		{
			return this.IsLeapYear(year, 0);
		}

		// Token: 0x06005625 RID: 22053
		public abstract bool IsLeapYear(int year, int era);

		// Token: 0x06005626 RID: 22054 RVA: 0x00122BD4 File Offset: 0x00120DD4
		public virtual DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			return this.ToDateTime(year, month, day, hour, minute, second, millisecond, 0);
		}

		// Token: 0x06005627 RID: 22055
		public abstract DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era);

		// Token: 0x06005628 RID: 22056 RVA: 0x00122BF4 File Offset: 0x00120DF4
		internal virtual bool TryToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era, out DateTime result)
		{
			result = DateTime.MinValue;
			bool result2;
			try
			{
				result = this.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
				result2 = true;
			}
			catch (ArgumentException)
			{
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x00017C70 File Offset: 0x00015E70
		internal virtual bool IsValidYear(int year, int era)
		{
			return year >= this.GetYear(this.MinSupportedDateTime) && year <= this.GetYear(this.MaxSupportedDateTime);
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x00017C50 File Offset: 0x00015E50
		internal virtual bool IsValidMonth(int year, int month, int era)
		{
			return this.IsValidYear(year, era) && month >= 1 && month <= this.GetMonthsInYear(year, era);
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x00017C2C File Offset: 0x00015E2C
		internal virtual bool IsValidDay(int year, int month, int day, int era)
		{
			return this.IsValidMonth(year, month, era) && day >= 1 && day <= this.GetDaysInMonth(year, month, era);
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x0600562C RID: 22060 RVA: 0x00122C44 File Offset: 0x00120E44
		// (set) Token: 0x0600562D RID: 22061 RVA: 0x00122C4C File Offset: 0x00120E4C
		public virtual int TwoDigitYearMax
		{
			get
			{
				return this.twoDigitYearMax;
			}
			set
			{
				this.VerifyWritable();
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x00122C5C File Offset: 0x00120E5C
		public virtual int ToFourDigitYear(int year)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("Non-negative number required."));
			}
			if (year < 100)
			{
				return (this.TwoDigitYearMax / 100 - ((year > this.TwoDigitYearMax % 100) ? 1 : 0)) * 100 + year;
			}
			return year;
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x00122CA8 File Offset: 0x00120EA8
		internal static long TimeToTicks(int hour, int minute, int second, int millisecond)
		{
			if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60 || second < 0 || second >= 60)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Hour, Minute, and Second parameters describe an un-representable DateTime."));
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 999));
			}
			return TimeSpan.TimeToTicks(hour, minute, second) + (long)millisecond * 10000L;
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x00122D30 File Offset: 0x00120F30
		[SecuritySafeCritical]
		internal static int GetSystemTwoDigitYearSetting(int CalID, int defaultYearValue)
		{
			int num = CalendarData.nativeGetTwoDigitYearMax(CalID);
			if (num < 0)
			{
				num = defaultYearValue;
			}
			return num;
		}

		// Token: 0x04003586 RID: 13702
		internal const long TicksPerMillisecond = 10000L;

		// Token: 0x04003587 RID: 13703
		internal const long TicksPerSecond = 10000000L;

		// Token: 0x04003588 RID: 13704
		internal const long TicksPerMinute = 600000000L;

		// Token: 0x04003589 RID: 13705
		internal const long TicksPerHour = 36000000000L;

		// Token: 0x0400358A RID: 13706
		internal const long TicksPerDay = 864000000000L;

		// Token: 0x0400358B RID: 13707
		internal const int MillisPerSecond = 1000;

		// Token: 0x0400358C RID: 13708
		internal const int MillisPerMinute = 60000;

		// Token: 0x0400358D RID: 13709
		internal const int MillisPerHour = 3600000;

		// Token: 0x0400358E RID: 13710
		internal const int MillisPerDay = 86400000;

		// Token: 0x0400358F RID: 13711
		internal const int DaysPerYear = 365;

		// Token: 0x04003590 RID: 13712
		internal const int DaysPer4Years = 1461;

		// Token: 0x04003591 RID: 13713
		internal const int DaysPer100Years = 36524;

		// Token: 0x04003592 RID: 13714
		internal const int DaysPer400Years = 146097;

		// Token: 0x04003593 RID: 13715
		internal const int DaysTo10000 = 3652059;

		// Token: 0x04003594 RID: 13716
		internal const long MaxMillis = 315537897600000L;

		// Token: 0x04003595 RID: 13717
		internal const int CAL_GREGORIAN = 1;

		// Token: 0x04003596 RID: 13718
		internal const int CAL_GREGORIAN_US = 2;

		// Token: 0x04003597 RID: 13719
		internal const int CAL_JAPAN = 3;

		// Token: 0x04003598 RID: 13720
		internal const int CAL_TAIWAN = 4;

		// Token: 0x04003599 RID: 13721
		internal const int CAL_KOREA = 5;

		// Token: 0x0400359A RID: 13722
		internal const int CAL_HIJRI = 6;

		// Token: 0x0400359B RID: 13723
		internal const int CAL_THAI = 7;

		// Token: 0x0400359C RID: 13724
		internal const int CAL_HEBREW = 8;

		// Token: 0x0400359D RID: 13725
		internal const int CAL_GREGORIAN_ME_FRENCH = 9;

		// Token: 0x0400359E RID: 13726
		internal const int CAL_GREGORIAN_ARABIC = 10;

		// Token: 0x0400359F RID: 13727
		internal const int CAL_GREGORIAN_XLIT_ENGLISH = 11;

		// Token: 0x040035A0 RID: 13728
		internal const int CAL_GREGORIAN_XLIT_FRENCH = 12;

		// Token: 0x040035A1 RID: 13729
		internal const int CAL_JULIAN = 13;

		// Token: 0x040035A2 RID: 13730
		internal const int CAL_JAPANESELUNISOLAR = 14;

		// Token: 0x040035A3 RID: 13731
		internal const int CAL_CHINESELUNISOLAR = 15;

		// Token: 0x040035A4 RID: 13732
		internal const int CAL_SAKA = 16;

		// Token: 0x040035A5 RID: 13733
		internal const int CAL_LUNAR_ETO_CHN = 17;

		// Token: 0x040035A6 RID: 13734
		internal const int CAL_LUNAR_ETO_KOR = 18;

		// Token: 0x040035A7 RID: 13735
		internal const int CAL_LUNAR_ETO_ROKUYOU = 19;

		// Token: 0x040035A8 RID: 13736
		internal const int CAL_KOREANLUNISOLAR = 20;

		// Token: 0x040035A9 RID: 13737
		internal const int CAL_TAIWANLUNISOLAR = 21;

		// Token: 0x040035AA RID: 13738
		internal const int CAL_PERSIAN = 22;

		// Token: 0x040035AB RID: 13739
		internal const int CAL_UMALQURA = 23;

		// Token: 0x040035AC RID: 13740
		internal int m_currentEraValue = -1;

		// Token: 0x040035AD RID: 13741
		[OptionalField(VersionAdded = 2)]
		private bool m_isReadOnly;

		// Token: 0x040035AE RID: 13742
		public const int CurrentEra = 0;

		// Token: 0x040035AF RID: 13743
		internal int twoDigitYearMax = -1;
	}
}
