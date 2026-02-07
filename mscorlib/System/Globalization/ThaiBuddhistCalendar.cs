using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x0200099A RID: 2458
	[ComVisible(true)]
	[Serializable]
	public class ThaiBuddhistCalendar : Calendar
	{
		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06005857 RID: 22615 RVA: 0x00122746 File Offset: 0x00120946
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06005858 RID: 22616 RVA: 0x0012274D File Offset: 0x0012094D
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06005859 RID: 22617 RVA: 0x000040F7 File Offset: 0x000022F7
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x0012A0A2 File Offset: 0x001282A2
		public ThaiBuddhistCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, ThaiBuddhistCalendar.thaiBuddhistEraInfo);
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x0600585B RID: 22619 RVA: 0x00032282 File Offset: 0x00030482
		internal override int ID
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x0600585C RID: 22620 RVA: 0x0012A0BB File Offset: 0x001282BB
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x0600585D RID: 22621 RVA: 0x0012A0CA File Offset: 0x001282CA
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x0600585E RID: 22622 RVA: 0x0012A0D9 File Offset: 0x001282D9
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x0600585F RID: 22623 RVA: 0x0012A0E9 File Offset: 0x001282E9
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x06005860 RID: 22624 RVA: 0x0012A0F8 File Offset: 0x001282F8
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x06005861 RID: 22625 RVA: 0x0012A106 File Offset: 0x00128306
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x06005862 RID: 22626 RVA: 0x0012A114 File Offset: 0x00128314
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x06005863 RID: 22627 RVA: 0x0012A122 File Offset: 0x00128322
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x06005864 RID: 22628 RVA: 0x0012A131 File Offset: 0x00128331
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x06005865 RID: 22629 RVA: 0x0012A141 File Offset: 0x00128341
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x06005866 RID: 22630 RVA: 0x0012A14F File Offset: 0x0012834F
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x06005867 RID: 22631 RVA: 0x0012A15D File Offset: 0x0012835D
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x06005868 RID: 22632 RVA: 0x0012A16B File Offset: 0x0012836B
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x0012A17D File Offset: 0x0012837D
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x0012A18C File Offset: 0x0012838C
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x0012A19B File Offset: 0x0012839B
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x0012A1AC File Offset: 0x001283AC
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x0600586D RID: 22637 RVA: 0x0012A1D1 File Offset: 0x001283D1
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x0600586E RID: 22638 RVA: 0x0012A1DE File Offset: 0x001283DE
		// (set) Token: 0x0600586F RID: 22639 RVA: 0x0012A208 File Offset: 0x00128408
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 2572);
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

		// Token: 0x06005870 RID: 22640 RVA: 0x0012A26B File Offset: 0x0012846B
		public override int ToFourDigitYear(int year)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("Non-negative number required."));
			}
			return this.helper.ToFourDigitYear(year, this.TwoDigitYearMax);
		}

		// Token: 0x040036B5 RID: 14005
		internal static EraInfo[] thaiBuddhistEraInfo = new EraInfo[]
		{
			new EraInfo(1, 1, 1, 1, -543, 544, 10542)
		};

		// Token: 0x040036B6 RID: 14006
		public const int ThaiBuddhistEra = 1;

		// Token: 0x040036B7 RID: 14007
		internal GregorianCalendarHelper helper;

		// Token: 0x040036B8 RID: 14008
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 2572;
	}
}
