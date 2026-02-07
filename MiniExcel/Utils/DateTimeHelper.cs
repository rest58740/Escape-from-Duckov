using System;

namespace MiniExcelLibs.Utils
{
	// Token: 0x0200002B RID: 43
	internal static class DateTimeHelper
	{
		// Token: 0x0600012E RID: 302 RVA: 0x000052C6 File Offset: 0x000034C6
		public static bool IsDateTimeFormat(string formatCode)
		{
			return new ExcelNumberFormat(formatCode).IsDateTimeFormat;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000052D3 File Offset: 0x000034D3
		public static DateTime FromOADate(double d)
		{
			return new DateTime(DateTimeHelper.DoubleDateToTicks(d), DateTimeKind.Unspecified);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000052E4 File Offset: 0x000034E4
		internal static long DoubleDateToTicks(double value)
		{
			if (value >= 2958466.0 || value <= -657435.0)
			{
				throw new ArgumentException("Invalid OA Date");
			}
			long num = (long)(value * 86400000.0 + ((value >= 0.0) ? 0.5 : -0.5));
			if (num < 0L)
			{
				num -= num % 86400000L * 2L;
			}
			num += 59926435200000L;
			if (num < 0L || num >= 315537897600000L)
			{
				throw new ArgumentException("OA Date out of range");
			}
			return num * 10000L;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005385 File Offset: 0x00003585
		public static double AdjustOADateTime(double value, bool date1904)
		{
			if (date1904)
			{
				return value + 1462.0;
			}
			if (value >= 0.0 && value < 60.0)
			{
				return value + 1.0;
			}
			return value;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000053BB File Offset: 0x000035BB
		public static bool IsValidOADateTime(double value)
		{
			return value > -657435.0 && value < 2958466.0;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000053D8 File Offset: 0x000035D8
		public static object ConvertFromOATime(double value, bool date1904)
		{
			double num = DateTimeHelper.AdjustOADateTime(value, date1904);
			if (DateTimeHelper.IsValidOADateTime(num))
			{
				return DateTimeHelper.FromOADate(num);
			}
			return value;
		}

		// Token: 0x04000053 RID: 83
		public const double OADateMinAsDouble = -657435.0;

		// Token: 0x04000054 RID: 84
		public const double OADateMaxAsDouble = 2958466.0;

		// Token: 0x04000055 RID: 85
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x04000056 RID: 86
		private const long TicksPerSecond = 10000000L;

		// Token: 0x04000057 RID: 87
		private const long TicksPerMinute = 600000000L;

		// Token: 0x04000058 RID: 88
		private const long TicksPerHour = 36000000000L;

		// Token: 0x04000059 RID: 89
		private const long TicksPerDay = 864000000000L;

		// Token: 0x0400005A RID: 90
		private const int MillisPerSecond = 1000;

		// Token: 0x0400005B RID: 91
		private const int MillisPerMinute = 60000;

		// Token: 0x0400005C RID: 92
		private const int MillisPerHour = 3600000;

		// Token: 0x0400005D RID: 93
		private const int MillisPerDay = 86400000;

		// Token: 0x0400005E RID: 94
		private const int DaysPerYear = 365;

		// Token: 0x0400005F RID: 95
		private const int DaysPer4Years = 1461;

		// Token: 0x04000060 RID: 96
		private const int DaysPer100Years = 36524;

		// Token: 0x04000061 RID: 97
		private const int DaysPer400Years = 146097;

		// Token: 0x04000062 RID: 98
		private const int DaysTo1899 = 693593;

		// Token: 0x04000063 RID: 99
		private const int DaysTo10000 = 3652059;

		// Token: 0x04000064 RID: 100
		private const long MaxMillis = 315537897600000L;

		// Token: 0x04000065 RID: 101
		private const long DoubleDateOffset = 599264352000000000L;
	}
}
