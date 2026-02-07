using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000996 RID: 2454
	[ComVisible(true)]
	[Serializable]
	public class TaiwanCalendar : Calendar
	{
		// Token: 0x060057EA RID: 22506 RVA: 0x00128B4F File Offset: 0x00126D4F
		internal static Calendar GetDefaultInstance()
		{
			if (TaiwanCalendar.s_defaultInstance == null)
			{
				TaiwanCalendar.s_defaultInstance = new TaiwanCalendar();
			}
			return TaiwanCalendar.s_defaultInstance;
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x060057EB RID: 22507 RVA: 0x00128B6D File Offset: 0x00126D6D
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return TaiwanCalendar.calendarMinValue;
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x060057EC RID: 22508 RVA: 0x0012274D File Offset: 0x0012094D
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x060057ED RID: 22509 RVA: 0x000040F7 File Offset: 0x000022F7
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x00128B74 File Offset: 0x00126D74
		public TaiwanCalendar()
		{
			try
			{
				new CultureInfo("zh-TW");
			}
			catch (ArgumentException innerException)
			{
				throw new TypeInitializationException(base.GetType().FullName, innerException);
			}
			this.helper = new GregorianCalendarHelper(this, TaiwanCalendar.taiwanEraInfo);
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x060057EF RID: 22511 RVA: 0x0002280B File Offset: 0x00020A0B
		internal override int ID
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060057F0 RID: 22512 RVA: 0x00128BC8 File Offset: 0x00126DC8
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x00128BD7 File Offset: 0x00126DD7
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x00128BE6 File Offset: 0x00126DE6
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x00128BF6 File Offset: 0x00126DF6
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x00128C05 File Offset: 0x00126E05
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x00128C13 File Offset: 0x00126E13
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x060057F6 RID: 22518 RVA: 0x00128C21 File Offset: 0x00126E21
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x00128C2F File Offset: 0x00126E2F
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x060057F8 RID: 22520 RVA: 0x00128C3E File Offset: 0x00126E3E
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x00128C4E File Offset: 0x00126E4E
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x060057FA RID: 22522 RVA: 0x00128C5C File Offset: 0x00126E5C
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x060057FB RID: 22523 RVA: 0x00128C6A File Offset: 0x00126E6A
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x060057FC RID: 22524 RVA: 0x00128C78 File Offset: 0x00126E78
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x060057FD RID: 22525 RVA: 0x00128C8A File Offset: 0x00126E8A
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x00128C99 File Offset: 0x00126E99
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x060057FF RID: 22527 RVA: 0x00128CA8 File Offset: 0x00126EA8
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x06005800 RID: 22528 RVA: 0x00128CB8 File Offset: 0x00126EB8
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005801 RID: 22529 RVA: 0x00128CDD File Offset: 0x00126EDD
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06005802 RID: 22530 RVA: 0x00126F93 File Offset: 0x00125193
		// (set) Token: 0x06005803 RID: 22531 RVA: 0x00128CEC File Offset: 0x00126EEC
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 99);
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

		// Token: 0x06005804 RID: 22532 RVA: 0x00128D50 File Offset: 0x00126F50
		public override int ToFourDigitYear(int year)
		{
			if (year <= 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("Positive number required."));
			}
			if (year > this.helper.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, this.helper.MaxYear));
			}
			return year;
		}

		// Token: 0x0400368E RID: 13966
		internal static EraInfo[] taiwanEraInfo = new EraInfo[]
		{
			new EraInfo(1, 1912, 1, 1, 1911, 1, 8088)
		};

		// Token: 0x0400368F RID: 13967
		internal static volatile Calendar s_defaultInstance;

		// Token: 0x04003690 RID: 13968
		internal GregorianCalendarHelper helper;

		// Token: 0x04003691 RID: 13969
		internal static readonly DateTime calendarMinValue = new DateTime(1912, 1, 1);

		// Token: 0x04003692 RID: 13970
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 99;
	}
}
