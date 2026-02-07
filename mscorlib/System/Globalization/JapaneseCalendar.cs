using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Globalization
{
	// Token: 0x0200098F RID: 2447
	[ComVisible(true)]
	[Serializable]
	public class JapaneseCalendar : Calendar
	{
		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x0600570C RID: 22284 RVA: 0x00126ACE File Offset: 0x00124CCE
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return JapaneseCalendar.calendarMinValue;
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x0600570D RID: 22285 RVA: 0x0012274D File Offset: 0x0012094D
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x0600570E RID: 22286 RVA: 0x000040F7 File Offset: 0x000022F7
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x0600570F RID: 22287 RVA: 0x00126AD8 File Offset: 0x00124CD8
		internal static EraInfo[] GetEraInfo()
		{
			if (JapaneseCalendar.japaneseEraInfo == null)
			{
				JapaneseCalendar.japaneseEraInfo = JapaneseCalendar.GetErasFromRegistry();
				if (JapaneseCalendar.japaneseEraInfo == null)
				{
					JapaneseCalendar.japaneseEraInfo = new EraInfo[]
					{
						new EraInfo(5, 2019, 5, 1, 2018, 1, 7981, "令和", "令", "R"),
						new EraInfo(4, 1989, 1, 8, 1988, 1, 31, "平成", "平", "H"),
						new EraInfo(3, 1926, 12, 25, 1925, 1, 64, "昭和", "昭", "S"),
						new EraInfo(2, 1912, 7, 30, 1911, 1, 15, "大正", "大", "T"),
						new EraInfo(1, 1868, 1, 1, 1867, 1, 45, "明治", "明", "M")
					};
				}
			}
			return JapaneseCalendar.japaneseEraInfo;
		}

		// Token: 0x06005710 RID: 22288 RVA: 0x0000AF5E File Offset: 0x0000915E
		[SecuritySafeCritical]
		private static EraInfo[] GetErasFromRegistry()
		{
			return null;
		}

		// Token: 0x06005711 RID: 22289 RVA: 0x00126BE6 File Offset: 0x00124DE6
		private static int CompareEraRanges(EraInfo a, EraInfo b)
		{
			return b.ticks.CompareTo(a.ticks);
		}

		// Token: 0x06005712 RID: 22290 RVA: 0x00126BFC File Offset: 0x00124DFC
		private static EraInfo GetEraFromValue(string value, string data)
		{
			if (value == null || data == null)
			{
				return null;
			}
			if (value.Length != 10)
			{
				return null;
			}
			int num;
			int startMonth;
			int startDay;
			if (!Number.TryParseInt32(value.Substring(0, 4), NumberStyles.None, NumberFormatInfo.InvariantInfo, out num) || !Number.TryParseInt32(value.Substring(5, 2), NumberStyles.None, NumberFormatInfo.InvariantInfo, out startMonth) || !Number.TryParseInt32(value.Substring(8, 2), NumberStyles.None, NumberFormatInfo.InvariantInfo, out startDay))
			{
				return null;
			}
			string[] array = data.Split(new char[]
			{
				'_'
			});
			if (array.Length != 4)
			{
				return null;
			}
			if (array[0].Length == 0 || array[1].Length == 0 || array[2].Length == 0 || array[3].Length == 0)
			{
				return null;
			}
			return new EraInfo(0, num, startMonth, startDay, num - 1, 1, 0, array[0], array[1], array[3]);
		}

		// Token: 0x06005713 RID: 22291 RVA: 0x00126CCE File Offset: 0x00124ECE
		internal static Calendar GetDefaultInstance()
		{
			if (JapaneseCalendar.s_defaultInstance == null)
			{
				JapaneseCalendar.s_defaultInstance = new JapaneseCalendar();
			}
			return JapaneseCalendar.s_defaultInstance;
		}

		// Token: 0x06005714 RID: 22292 RVA: 0x00126CEC File Offset: 0x00124EEC
		public JapaneseCalendar()
		{
			try
			{
				new CultureInfo("ja-JP");
			}
			catch (ArgumentException innerException)
			{
				throw new TypeInitializationException(base.GetType().FullName, innerException);
			}
			this.helper = new GregorianCalendarHelper(this, JapaneseCalendar.GetEraInfo());
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06005715 RID: 22293 RVA: 0x000221D6 File Offset: 0x000203D6
		internal override int ID
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06005716 RID: 22294 RVA: 0x00126D40 File Offset: 0x00124F40
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x06005717 RID: 22295 RVA: 0x00126D4F File Offset: 0x00124F4F
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x06005718 RID: 22296 RVA: 0x00126D5E File Offset: 0x00124F5E
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x06005719 RID: 22297 RVA: 0x00126D6E File Offset: 0x00124F6E
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x00126D7D File Offset: 0x00124F7D
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x00126D8B File Offset: 0x00124F8B
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x00126D99 File Offset: 0x00124F99
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x00126DA7 File Offset: 0x00124FA7
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x00126DB6 File Offset: 0x00124FB6
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x0600571F RID: 22303 RVA: 0x00126DC6 File Offset: 0x00124FC6
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x00126DD4 File Offset: 0x00124FD4
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x00126DE2 File Offset: 0x00124FE2
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x00126DF0 File Offset: 0x00124FF0
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x00126E02 File Offset: 0x00125002
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x00126E11 File Offset: 0x00125011
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x06005725 RID: 22309 RVA: 0x00126E20 File Offset: 0x00125020
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x00126E30 File Offset: 0x00125030
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x00126E58 File Offset: 0x00125058
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

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06005728 RID: 22312 RVA: 0x00126EC2 File Offset: 0x001250C2
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x00126ED0 File Offset: 0x001250D0
		internal static string[] EraNames()
		{
			EraInfo[] eraInfo = JapaneseCalendar.GetEraInfo();
			string[] array = new string[eraInfo.Length];
			for (int i = 0; i < eraInfo.Length; i++)
			{
				array[i] = eraInfo[eraInfo.Length - i - 1].eraName;
			}
			return array;
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x00126F0C File Offset: 0x0012510C
		internal static string[] AbbrevEraNames()
		{
			EraInfo[] eraInfo = JapaneseCalendar.GetEraInfo();
			string[] array = new string[eraInfo.Length];
			for (int i = 0; i < eraInfo.Length; i++)
			{
				array[i] = eraInfo[eraInfo.Length - i - 1].abbrevEraName;
			}
			return array;
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x00126F48 File Offset: 0x00125148
		internal static string[] EnglishEraNames()
		{
			EraInfo[] eraInfo = JapaneseCalendar.GetEraInfo();
			string[] array = new string[eraInfo.Length];
			for (int i = 0; i < eraInfo.Length; i++)
			{
				array[i] = eraInfo[eraInfo.Length - i - 1].englishEraName;
			}
			return array;
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x00126F84 File Offset: 0x00125184
		internal override bool IsValidYear(int year, int era)
		{
			return this.helper.IsValidYear(year, era);
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x0600572D RID: 22317 RVA: 0x00126F93 File Offset: 0x00125193
		// (set) Token: 0x0600572E RID: 22318 RVA: 0x00126FB8 File Offset: 0x001251B8
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

		// Token: 0x0400363A RID: 13882
		internal static readonly DateTime calendarMinValue = new DateTime(1868, 9, 8);

		// Token: 0x0400363B RID: 13883
		internal static volatile EraInfo[] japaneseEraInfo;

		// Token: 0x0400363C RID: 13884
		private const string c_japaneseErasHive = "System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras";

		// Token: 0x0400363D RID: 13885
		private const string c_japaneseErasHivePermissionList = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras";

		// Token: 0x0400363E RID: 13886
		internal static volatile Calendar s_defaultInstance;

		// Token: 0x0400363F RID: 13887
		internal GregorianCalendarHelper helper;

		// Token: 0x04003640 RID: 13888
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 99;
	}
}
