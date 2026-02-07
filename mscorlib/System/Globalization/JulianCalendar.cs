using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000991 RID: 2449
	[ComVisible(true)]
	[Serializable]
	public class JulianCalendar : Calendar
	{
		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06005742 RID: 22338 RVA: 0x00122746 File Offset: 0x00120946
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06005743 RID: 22339 RVA: 0x0012274D File Offset: 0x0012094D
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06005744 RID: 22340 RVA: 0x000040F7 File Offset: 0x000022F7
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x001271E4 File Offset: 0x001253E4
		public JulianCalendar()
		{
			this.twoDigitYearMax = 2029;
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06005746 RID: 22342 RVA: 0x0003D1EA File Offset: 0x0003B3EA
		internal override int ID
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x00127202 File Offset: 0x00125402
		internal static void CheckEraRange(int era)
		{
			if (era != 0 && era != JulianCalendar.JulianEra)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("Era value was not valid."));
			}
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x00127224 File Offset: 0x00125424
		internal void CheckYearEraRange(int year, int era)
		{
			JulianCalendar.CheckEraRange(era);
			if (year <= 0 || year > this.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, this.MaxYear));
			}
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x00127274 File Offset: 0x00125474
		internal static void CheckMonthRange(int month)
		{
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("Month must be between one and twelve."));
			}
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x00127294 File Offset: 0x00125494
		internal static void CheckDayRange(int year, int month, int day)
		{
			if (year == 1 && month == 1 && day < 3)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Year, Month, and Day parameters describe an un-representable DateTime."));
			}
			int[] array = (year % 4 == 0) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			int num = array[month] - array[month - 1];
			if (day < 1 || day > num)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, num));
			}
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x00127310 File Offset: 0x00125510
		internal static int GetDatePart(long ticks, int part)
		{
			int i = (int)((ticks + 1728000000000L) / 864000000000L);
			int num = i / 1461;
			i -= num * 1461;
			int num2 = i / 365;
			if (num2 == 4)
			{
				num2 = 3;
			}
			if (part == 0)
			{
				return num * 4 + num2 + 1;
			}
			i -= num2 * 365;
			if (part == 1)
			{
				return i + 1;
			}
			int[] array = (num2 == 3) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			int num3 = i >> 6;
			while (i >= array[num3])
			{
				num3++;
			}
			if (part == 2)
			{
				return num3;
			}
			return i - array[num3 - 1] + 1;
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x001273AC File Offset: 0x001255AC
		internal static long DateToTicks(int year, int month, int day)
		{
			int[] array = (year % 4 == 0) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			int num = year - 1;
			return (long)(num * 365 + num / 4 + array[month - 1] + day - 1 - 2) * 864000000000L;
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x001273F4 File Offset: 0x001255F4
		public override DateTime AddMonths(DateTime time, int months)
		{
			if (months < -120000 || months > 120000)
			{
				throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), -120000, 120000));
			}
			int num = JulianCalendar.GetDatePart(time.Ticks, 0);
			int num2 = JulianCalendar.GetDatePart(time.Ticks, 2);
			int num3 = JulianCalendar.GetDatePart(time.Ticks, 3);
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
			int[] array = (num % 4 == 0 && (num % 100 != 0 || num % 400 == 0)) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			int num5 = array[num2] - array[num2 - 1];
			if (num3 > num5)
			{
				num3 = num5;
			}
			long ticks = JulianCalendar.DateToTicks(num, num2, num3) + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(ticks);
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x001223A1 File Offset: 0x001205A1
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x00127504 File Offset: 0x00125704
		public override int GetDayOfMonth(DateTime time)
		{
			return JulianCalendar.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x001223BE File Offset: 0x001205BE
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x00127513 File Offset: 0x00125713
		public override int GetDayOfYear(DateTime time)
		{
			return JulianCalendar.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x00127524 File Offset: 0x00125724
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckYearEraRange(year, era);
			JulianCalendar.CheckMonthRange(month);
			int[] array = (year % 4 == 0) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365;
			return array[month] - array[month - 1];
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x0012755A File Offset: 0x0012575A
		public override int GetDaysInYear(int year, int era)
		{
			if (!this.IsLeapYear(year, era))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x00127571 File Offset: 0x00125771
		public override int GetEra(DateTime time)
		{
			return JulianCalendar.JulianEra;
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x00127578 File Offset: 0x00125778
		public override int GetMonth(DateTime time)
		{
			return JulianCalendar.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06005756 RID: 22358 RVA: 0x00127587 File Offset: 0x00125787
		public override int[] Eras
		{
			get
			{
				return new int[]
				{
					JulianCalendar.JulianEra
				};
			}
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x00127597 File Offset: 0x00125797
		public override int GetMonthsInYear(int year, int era)
		{
			this.CheckYearEraRange(year, era);
			return 12;
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x001275A3 File Offset: 0x001257A3
		public override int GetYear(DateTime time)
		{
			return JulianCalendar.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x001275B2 File Offset: 0x001257B2
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			JulianCalendar.CheckMonthRange(month);
			if (this.IsLeapYear(year, era))
			{
				JulianCalendar.CheckDayRange(year, month, day);
				return month == 2 && day == 29;
			}
			JulianCalendar.CheckDayRange(year, month, day);
			return false;
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x001275E2 File Offset: 0x001257E2
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			this.CheckYearEraRange(year, era);
			return 0;
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x001275ED File Offset: 0x001257ED
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.CheckYearEraRange(year, era);
			JulianCalendar.CheckMonthRange(month);
			return false;
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x001275FE File Offset: 0x001257FE
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckYearEraRange(year, era);
			return year % 4 == 0;
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x00127610 File Offset: 0x00125810
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			this.CheckYearEraRange(year, era);
			JulianCalendar.CheckMonthRange(month);
			JulianCalendar.CheckDayRange(year, month, day);
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 0, 999));
			}
			if (hour >= 0 && hour < 24 && minute >= 0 && minute < 60 && second >= 0 && second < 60)
			{
				return new DateTime(JulianCalendar.DateToTicks(year, month, day) + new TimeSpan(0, hour, minute, second, millisecond).Ticks);
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Hour, Minute, and Second parameters describe an un-representable DateTime."));
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x0600575E RID: 22366 RVA: 0x00122C44 File Offset: 0x00120E44
		// (set) Token: 0x0600575F RID: 22367 RVA: 0x001276C8 File Offset: 0x001258C8
		public override int TwoDigitYearMax
		{
			get
			{
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > this.MaxYear)
				{
					throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 99, this.MaxYear));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x00127724 File Offset: 0x00125924
		public override int ToFourDigitYear(int year)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("Non-negative number required."));
			}
			if (year > this.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument must be between {0} and {1}."), 1, this.MaxYear));
			}
			return base.ToFourDigitYear(year);
		}

		// Token: 0x0400364E RID: 13902
		public static readonly int JulianEra = 1;

		// Token: 0x0400364F RID: 13903
		private const int DatePartYear = 0;

		// Token: 0x04003650 RID: 13904
		private const int DatePartDayOfYear = 1;

		// Token: 0x04003651 RID: 13905
		private const int DatePartMonth = 2;

		// Token: 0x04003652 RID: 13906
		private const int DatePartDay = 3;

		// Token: 0x04003653 RID: 13907
		private const int JulianDaysPerYear = 365;

		// Token: 0x04003654 RID: 13908
		private const int JulianDaysPer4Years = 1461;

		// Token: 0x04003655 RID: 13909
		private static readonly int[] DaysToMonth365 = new int[]
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

		// Token: 0x04003656 RID: 13910
		private static readonly int[] DaysToMonth366 = new int[]
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

		// Token: 0x04003657 RID: 13911
		internal int MaxYear = 9999;
	}
}
