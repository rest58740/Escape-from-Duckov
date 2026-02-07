using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000992 RID: 2450
	[ComVisible(true)]
	[Serializable]
	public class KoreanCalendar : Calendar
	{
		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06005762 RID: 22370 RVA: 0x00122746 File Offset: 0x00120946
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06005763 RID: 22371 RVA: 0x0012274D File Offset: 0x0012094D
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06005764 RID: 22372 RVA: 0x000040F7 File Offset: 0x000022F7
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x001277C0 File Offset: 0x001259C0
		public KoreanCalendar()
		{
			try
			{
				new CultureInfo("ko-KR");
			}
			catch (ArgumentException innerException)
			{
				throw new TypeInitializationException(base.GetType().FullName, innerException);
			}
			this.helper = new GregorianCalendarHelper(this, KoreanCalendar.koreanEraInfo);
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06005766 RID: 22374 RVA: 0x0003CDA4 File Offset: 0x0003AFA4
		internal override int ID
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x00127814 File Offset: 0x00125A14
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x00127823 File Offset: 0x00125A23
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x00127832 File Offset: 0x00125A32
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x00127842 File Offset: 0x00125A42
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x00127851 File Offset: 0x00125A51
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x0012785F File Offset: 0x00125A5F
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x0012786D File Offset: 0x00125A6D
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x0012787B File Offset: 0x00125A7B
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x0012788A File Offset: 0x00125A8A
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x0012789A File Offset: 0x00125A9A
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x06005771 RID: 22385 RVA: 0x001278A8 File Offset: 0x00125AA8
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x001278B6 File Offset: 0x00125AB6
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x001278C4 File Offset: 0x00125AC4
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x001278D6 File Offset: 0x00125AD6
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x001278E5 File Offset: 0x00125AE5
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x001278F4 File Offset: 0x00125AF4
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x00127904 File Offset: 0x00125B04
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06005778 RID: 22392 RVA: 0x00127929 File Offset: 0x00125B29
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06005779 RID: 22393 RVA: 0x00127936 File Offset: 0x00125B36
		// (set) Token: 0x0600577A RID: 22394 RVA: 0x00127960 File Offset: 0x00125B60
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 4362);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > this.helper.MaxYear)
				{
					throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 99, this.helper.MaxYear));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x001279C3 File Offset: 0x00125BC3
		public override int ToFourDigitYear(int year)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("Non-negative number required."));
			}
			return this.helper.ToFourDigitYear(year, this.TwoDigitYearMax);
		}

		// Token: 0x04003658 RID: 13912
		public const int KoreanEra = 1;

		// Token: 0x04003659 RID: 13913
		internal static EraInfo[] koreanEraInfo = new EraInfo[]
		{
			new EraInfo(1, 1, 1, 1, -2333, 2334, 12332)
		};

		// Token: 0x0400365A RID: 13914
		internal GregorianCalendarHelper helper;

		// Token: 0x0400365B RID: 13915
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 4362;
	}
}
