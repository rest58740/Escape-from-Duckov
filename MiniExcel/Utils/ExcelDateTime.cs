using System;
using System.Globalization;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000031 RID: 49
	internal class ExcelDateTime
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00005637 File Offset: 0x00003837
		public DateTime AdjustedDateTime { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000563F File Offset: 0x0000383F
		public int AdjustDaysPost { get; }

		// Token: 0x06000148 RID: 328 RVA: 0x00005648 File Offset: 0x00003848
		public ExcelDateTime(double numericDate, bool isDate1904)
		{
			if (isDate1904)
			{
				numericDate += 1462.0;
				this.AdjustedDateTime = new DateTime(ExcelDateTime.DoubleDateToTicks(numericDate), DateTimeKind.Unspecified);
				return;
			}
			DateTime dateTime = new DateTime(ExcelDateTime.DoubleDateToTicks(numericDate), DateTimeKind.Unspecified);
			if (dateTime < ExcelDateTime.Excel1900ZeroethMinDate)
			{
				this.AdjustDaysPost = 0;
				this.AdjustedDateTime = dateTime.AddDays(2.0);
				return;
			}
			if (dateTime < ExcelDateTime.Excel1900ZeroethMaxDate)
			{
				this.AdjustDaysPost = -1;
				this.AdjustedDateTime = dateTime.AddDays(2.0);
				return;
			}
			if (dateTime < ExcelDateTime.Excel1900LeapMinDate)
			{
				this.AdjustDaysPost = 0;
				this.AdjustedDateTime = dateTime.AddDays(1.0);
				return;
			}
			if (dateTime < ExcelDateTime.Excel1900LeapMaxDate)
			{
				this.AdjustDaysPost = 1;
				this.AdjustedDateTime = dateTime;
				return;
			}
			this.AdjustDaysPost = 0;
			this.AdjustedDateTime = dateTime;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005737 File Offset: 0x00003937
		public ExcelDateTime(DateTime value)
		{
			this.AdjustedDateTime = value;
			this.AdjustDaysPost = 0;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00005750 File Offset: 0x00003950
		public int Year
		{
			get
			{
				return this.AdjustedDateTime.Year;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000576C File Offset: 0x0000396C
		public int Month
		{
			get
			{
				return this.AdjustedDateTime.Month;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00005788 File Offset: 0x00003988
		public int Day
		{
			get
			{
				return this.AdjustedDateTime.Day + this.AdjustDaysPost;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000057AC File Offset: 0x000039AC
		public int Hour
		{
			get
			{
				return this.AdjustedDateTime.Hour;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000057C8 File Offset: 0x000039C8
		public int Minute
		{
			get
			{
				return this.AdjustedDateTime.Minute;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600014F RID: 335 RVA: 0x000057E4 File Offset: 0x000039E4
		public int Second
		{
			get
			{
				return this.AdjustedDateTime.Second;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00005800 File Offset: 0x00003A00
		public int Millisecond
		{
			get
			{
				return this.AdjustedDateTime.Millisecond;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000581C File Offset: 0x00003A1C
		public DayOfWeek DayOfWeek
		{
			get
			{
				return this.AdjustedDateTime.DayOfWeek;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005838 File Offset: 0x00003A38
		public string ToString(string numberFormat, CultureInfo culture)
		{
			return this.AdjustedDateTime.ToString(numberFormat, culture);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005858 File Offset: 0x00003A58
		public static bool TryConvert(object value, bool isDate1904, CultureInfo culture, out ExcelDateTime result)
		{
			if (value is double)
			{
				double numericDate = (double)value;
				result = new ExcelDateTime(numericDate, isDate1904);
				return true;
			}
			if (value is int)
			{
				int num = (int)value;
				result = new ExcelDateTime((double)num, isDate1904);
				return true;
			}
			if (value is short)
			{
				short num2 = (short)value;
				result = new ExcelDateTime((double)num2, isDate1904);
				return true;
			}
			if (value is DateTime)
			{
				DateTime value2 = (DateTime)value;
				result = new ExcelDateTime(value2);
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000058D4 File Offset: 0x00003AD4
		internal static long DoubleDateToTicks(double value)
		{
			long num = (long)(value * 86400000.0 + ((value >= 0.0) ? 0.5 : -0.5));
			if (num < 0L)
			{
				num -= num % 86400000L * 2L;
			}
			num += 59926435200000L;
			return num * 10000L;
		}

		// Token: 0x04000075 RID: 117
		private static DateTime Excel1900LeapMinDate = new DateTime(1900, 2, 28);

		// Token: 0x04000076 RID: 118
		private static DateTime Excel1900LeapMaxDate = new DateTime(1900, 3, 1);

		// Token: 0x04000077 RID: 119
		private static DateTime Excel1900ZeroethMinDate = new DateTime(1899, 12, 30);

		// Token: 0x04000078 RID: 120
		private static DateTime Excel1900ZeroethMaxDate = new DateTime(1899, 12, 31);

		// Token: 0x04000079 RID: 121
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x0400007A RID: 122
		private const long TicksPerSecond = 10000000L;

		// Token: 0x0400007B RID: 123
		private const long TicksPerMinute = 600000000L;

		// Token: 0x0400007C RID: 124
		private const long TicksPerHour = 36000000000L;

		// Token: 0x0400007D RID: 125
		private const long TicksPerDay = 864000000000L;

		// Token: 0x0400007E RID: 126
		private const int MillisPerSecond = 1000;

		// Token: 0x0400007F RID: 127
		private const int MillisPerMinute = 60000;

		// Token: 0x04000080 RID: 128
		private const int MillisPerHour = 3600000;

		// Token: 0x04000081 RID: 129
		private const int MillisPerDay = 86400000;

		// Token: 0x04000082 RID: 130
		private const int DaysPerYear = 365;

		// Token: 0x04000083 RID: 131
		private const int DaysPer4Years = 1461;

		// Token: 0x04000084 RID: 132
		private const int DaysPer100Years = 36524;

		// Token: 0x04000085 RID: 133
		private const int DaysPer400Years = 146097;

		// Token: 0x04000086 RID: 134
		private const int DaysTo1899 = 693593;

		// Token: 0x04000087 RID: 135
		private const long DoubleDateOffset = 599264352000000000L;
	}
}
