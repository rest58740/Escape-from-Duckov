using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Globalization
{
	// Token: 0x0200098E RID: 2446
	[ComVisible(true)]
	[Serializable]
	public class HijriCalendar : Calendar
	{
		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x060056E7 RID: 22247 RVA: 0x001263A8 File Offset: 0x001245A8
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return HijriCalendar.calendarMinValue;
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x060056E8 RID: 22248 RVA: 0x001263AF File Offset: 0x001245AF
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return HijriCalendar.calendarMaxValue;
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x060056E9 RID: 22249 RVA: 0x00015831 File Offset: 0x00013A31
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.LunarCalendar;
			}
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x060056EB RID: 22251 RVA: 0x000224A7 File Offset: 0x000206A7
		internal override int ID
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x060056EC RID: 22252 RVA: 0x001263C9 File Offset: 0x001245C9
		protected override int DaysInYearBeforeMinSupportedYear
		{
			get
			{
				return 354;
			}
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x001263D0 File Offset: 0x001245D0
		private long GetAbsoluteDateHijri(int y, int m, int d)
		{
			return this.DaysUpToHijriYear(y) + (long)HijriCalendar.HijriMonthDays[m - 1] + (long)d - 1L - (long)this.HijriAdjustment;
		}

		// Token: 0x060056EE RID: 22254 RVA: 0x001263F4 File Offset: 0x001245F4
		private long DaysUpToHijriYear(int HijriYear)
		{
			int num = (HijriYear - 1) / 30 * 30;
			int i = HijriYear - num - 1;
			long num2 = (long)num * 10631L / 30L + 227013L;
			while (i > 0)
			{
				num2 += (long)(354 + (this.IsLeapYear(i, 0) ? 1 : 0));
				i--;
			}
			return num2;
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x060056EF RID: 22255 RVA: 0x00126449 File Offset: 0x00124649
		// (set) Token: 0x060056F0 RID: 22256 RVA: 0x0012646C File Offset: 0x0012466C
		public int HijriAdjustment
		{
			[SecuritySafeCritical]
			get
			{
				if (this.m_HijriAdvance == -2147483648)
				{
					this.m_HijriAdvance = HijriCalendar.GetAdvanceHijriDate();
				}
				return this.m_HijriAdvance;
			}
			set
			{
				if (value < -2 || value > 2)
				{
					throw new ArgumentOutOfRangeException("HijriAdjustment", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument must be between {0} and {1}."), -2, 2));
				}
				base.VerifyWritable();
				this.m_HijriAdvance = value;
			}
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[SecurityCritical]
		private static int GetAdvanceHijriDate()
		{
			return 0;
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x001264BC File Offset: 0x001246BC
		internal static void CheckTicksRange(long ticks)
		{
			if (ticks < HijriCalendar.calendarMinValue.Ticks || ticks > HijriCalendar.calendarMaxValue.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Specified time is not supported in this calendar. It should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive."), HijriCalendar.calendarMinValue, HijriCalendar.calendarMaxValue));
			}
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x00126516 File Offset: 0x00124716
		internal static void CheckEraRange(int era)
		{
			if (era != 0 && era != HijriCalendar.HijriEra)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("Era value was not valid."));
			}
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x00126538 File Offset: 0x00124738
		internal static void CheckYearRange(int year, int era)
		{
			HijriCalendar.CheckEraRange(era);
			if (year < 1 || year > 9666)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, 9666));
			}
		}

		// Token: 0x060056F5 RID: 22261 RVA: 0x00126588 File Offset: 0x00124788
		internal static void CheckYearMonthRange(int year, int month, int era)
		{
			HijriCalendar.CheckYearRange(year, era);
			if (year == 9666 && month > 4)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, 4));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("Month must be between one and twelve."));
			}
		}

		// Token: 0x060056F6 RID: 22262 RVA: 0x001265F4 File Offset: 0x001247F4
		internal virtual int GetDatePart(long ticks, int part)
		{
			HijriCalendar.CheckTicksRange(ticks);
			long num = ticks / 864000000000L + 1L;
			num += (long)this.HijriAdjustment;
			int num2 = (int)((num - 227013L) * 30L / 10631L) + 1;
			long num3 = this.DaysUpToHijriYear(num2);
			long num4 = (long)this.GetDaysInYear(num2, 0);
			if (num < num3)
			{
				num3 -= num4;
				num2--;
			}
			else if (num == num3)
			{
				num2--;
				num3 -= (long)this.GetDaysInYear(num2, 0);
			}
			else if (num > num3 + num4)
			{
				num3 += num4;
				num2++;
			}
			if (part == 0)
			{
				return num2;
			}
			int num5 = 1;
			num -= num3;
			if (part == 1)
			{
				return (int)num;
			}
			while (num5 <= 12 && num > (long)HijriCalendar.HijriMonthDays[num5 - 1])
			{
				num5++;
			}
			num5--;
			if (part == 2)
			{
				return num5;
			}
			int result = (int)(num - (long)HijriCalendar.HijriMonthDays[num5 - 1]);
			if (part == 3)
			{
				return result;
			}
			throw new InvalidOperationException(Environment.GetResourceString("Internal Error in DateTime and Calendar operations."));
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x001266E0 File Offset: 0x001248E0
		public override DateTime AddMonths(DateTime time, int months)
		{
			if (months < -120000 || months > 120000)
			{
				throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), -120000, 120000));
			}
			int num = this.GetDatePart(time.Ticks, 0);
			int num2 = this.GetDatePart(time.Ticks, 2);
			int num3 = this.GetDatePart(time.Ticks, 3);
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
			int daysInMonth = this.GetDaysInMonth(num, num2);
			if (num3 > daysInMonth)
			{
				num3 = daysInMonth;
			}
			long ticks = this.GetAbsoluteDateHijri(num, num2, num3) * 864000000000L + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(ticks);
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x001223A1 File Offset: 0x001205A1
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x060056F9 RID: 22265 RVA: 0x001267D9 File Offset: 0x001249D9
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x060056FA RID: 22266 RVA: 0x001223BE File Offset: 0x001205BE
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x001267E9 File Offset: 0x001249E9
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x060056FC RID: 22268 RVA: 0x001267F9 File Offset: 0x001249F9
		public override int GetDaysInMonth(int year, int month, int era)
		{
			HijriCalendar.CheckYearMonthRange(year, month, era);
			if (month == 12)
			{
				if (!this.IsLeapYear(year, 0))
				{
					return 29;
				}
				return 30;
			}
			else
			{
				if (month % 2 != 1)
				{
					return 29;
				}
				return 30;
			}
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x00126823 File Offset: 0x00124A23
		public override int GetDaysInYear(int year, int era)
		{
			HijriCalendar.CheckYearRange(year, era);
			if (!this.IsLeapYear(year, 0))
			{
				return 354;
			}
			return 355;
		}

		// Token: 0x060056FE RID: 22270 RVA: 0x00126841 File Offset: 0x00124A41
		public override int GetEra(DateTime time)
		{
			HijriCalendar.CheckTicksRange(time.Ticks);
			return HijriCalendar.HijriEra;
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x060056FF RID: 22271 RVA: 0x00126854 File Offset: 0x00124A54
		public override int[] Eras
		{
			get
			{
				return new int[]
				{
					HijriCalendar.HijriEra
				};
			}
		}

		// Token: 0x06005700 RID: 22272 RVA: 0x00126864 File Offset: 0x00124A64
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x06005701 RID: 22273 RVA: 0x00126874 File Offset: 0x00124A74
		public override int GetMonthsInYear(int year, int era)
		{
			HijriCalendar.CheckYearRange(year, era);
			return 12;
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x0012687F File Offset: 0x00124A7F
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x00126890 File Offset: 0x00124A90
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Day must be between 1 and {0} for month {1}."), daysInMonth, month));
			}
			return this.IsLeapYear(year, era) && month == 12 && day == 30;
		}

		// Token: 0x06005704 RID: 22276 RVA: 0x001268F2 File Offset: 0x00124AF2
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			HijriCalendar.CheckYearRange(year, era);
			return 0;
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x001268FC File Offset: 0x00124AFC
		public override bool IsLeapMonth(int year, int month, int era)
		{
			HijriCalendar.CheckYearMonthRange(year, month, era);
			return false;
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x00126907 File Offset: 0x00124B07
		public override bool IsLeapYear(int year, int era)
		{
			HijriCalendar.CheckYearRange(year, era);
			return (year * 11 + 14) % 30 < 11;
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x00126920 File Offset: 0x00124B20
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Day must be between 1 and {0} for month {1}."), daysInMonth, month));
			}
			long absoluteDateHijri = this.GetAbsoluteDateHijri(year, month, day);
			if (absoluteDateHijri >= 0L)
			{
				return new DateTime(absoluteDateHijri * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Year, Month, and Day parameters describe an un-representable DateTime."));
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06005708 RID: 22280 RVA: 0x001269A9 File Offset: 0x00124BA9
		// (set) Token: 0x06005709 RID: 22281 RVA: 0x001269D0 File Offset: 0x00124BD0
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 1451);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > 9666)
				{
					throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 99, 9666));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x00126A28 File Offset: 0x00124C28
		public override int ToFourDigitYear(int year)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("Non-negative number required."));
			}
			if (year < 100)
			{
				return base.ToFourDigitYear(year);
			}
			if (year > 9666)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, 9666));
			}
			return year;
		}

		// Token: 0x0400362A RID: 13866
		public static readonly int HijriEra = 1;

		// Token: 0x0400362B RID: 13867
		internal const int DatePartYear = 0;

		// Token: 0x0400362C RID: 13868
		internal const int DatePartDayOfYear = 1;

		// Token: 0x0400362D RID: 13869
		internal const int DatePartMonth = 2;

		// Token: 0x0400362E RID: 13870
		internal const int DatePartDay = 3;

		// Token: 0x0400362F RID: 13871
		internal const int MinAdvancedHijri = -2;

		// Token: 0x04003630 RID: 13872
		internal const int MaxAdvancedHijri = 2;

		// Token: 0x04003631 RID: 13873
		internal static readonly int[] HijriMonthDays = new int[]
		{
			0,
			30,
			59,
			89,
			118,
			148,
			177,
			207,
			236,
			266,
			295,
			325,
			355
		};

		// Token: 0x04003632 RID: 13874
		private const string HijriAdvanceRegKeyEntry = "AddHijriDate";

		// Token: 0x04003633 RID: 13875
		private int m_HijriAdvance = int.MinValue;

		// Token: 0x04003634 RID: 13876
		internal const int MaxCalendarYear = 9666;

		// Token: 0x04003635 RID: 13877
		internal const int MaxCalendarMonth = 4;

		// Token: 0x04003636 RID: 13878
		internal const int MaxCalendarDay = 3;

		// Token: 0x04003637 RID: 13879
		internal static readonly DateTime calendarMinValue = new DateTime(622, 7, 18);

		// Token: 0x04003638 RID: 13880
		internal static readonly DateTime calendarMaxValue = DateTime.MaxValue;

		// Token: 0x04003639 RID: 13881
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 1451;
	}
}
