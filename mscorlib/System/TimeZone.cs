using System;
using System.Globalization;
using System.Threading;

namespace System
{
	// Token: 0x02000193 RID: 403
	[Obsolete("System.TimeZone has been deprecated.  Please investigate the use of System.TimeZoneInfo instead.")]
	[Serializable]
	public abstract class TimeZone
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x0004233C File Offset: 0x0004053C
		private static object InternalSyncObject
		{
			get
			{
				if (TimeZone.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange<object>(ref TimeZone.s_InternalSyncObject, value, null);
				}
				return TimeZone.s_InternalSyncObject;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600101F RID: 4127 RVA: 0x00042368 File Offset: 0x00040568
		public static TimeZone CurrentTimeZone
		{
			get
			{
				TimeZone timeZone = TimeZone.currentTimeZone;
				if (timeZone == null)
				{
					object internalSyncObject = TimeZone.InternalSyncObject;
					lock (internalSyncObject)
					{
						if (TimeZone.currentTimeZone == null)
						{
							TimeZone.currentTimeZone = new CurrentSystemTimeZone();
						}
						timeZone = TimeZone.currentTimeZone;
					}
				}
				return timeZone;
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x000423CC File Offset: 0x000405CC
		internal static void ResetTimeZone()
		{
			if (TimeZone.currentTimeZone != null)
			{
				object internalSyncObject = TimeZone.InternalSyncObject;
				lock (internalSyncObject)
				{
					TimeZone.currentTimeZone = null;
				}
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06001021 RID: 4129
		public abstract string StandardName { get; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06001022 RID: 4130
		public abstract string DaylightName { get; }

		// Token: 0x06001023 RID: 4131
		public abstract TimeSpan GetUtcOffset(DateTime time);

		// Token: 0x06001024 RID: 4132 RVA: 0x00042418 File Offset: 0x00040618
		public virtual DateTime ToUniversalTime(DateTime time)
		{
			if (time.Kind == DateTimeKind.Utc)
			{
				return time;
			}
			long num = time.Ticks - this.GetUtcOffset(time).Ticks;
			if (num > 3155378975999999999L)
			{
				return new DateTime(3155378975999999999L, DateTimeKind.Utc);
			}
			if (num < 0L)
			{
				return new DateTime(0L, DateTimeKind.Utc);
			}
			return new DateTime(num, DateTimeKind.Utc);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0004247C File Offset: 0x0004067C
		public virtual DateTime ToLocalTime(DateTime time)
		{
			if (time.Kind == DateTimeKind.Local)
			{
				return time;
			}
			bool isAmbiguousDst = false;
			long utcOffsetFromUniversalTime = ((CurrentSystemTimeZone)TimeZone.CurrentTimeZone).GetUtcOffsetFromUniversalTime(time, ref isAmbiguousDst);
			return new DateTime(time.Ticks + utcOffsetFromUniversalTime, DateTimeKind.Local, isAmbiguousDst);
		}

		// Token: 0x06001026 RID: 4134
		public abstract DaylightTime GetDaylightChanges(int year);

		// Token: 0x06001027 RID: 4135 RVA: 0x000424BA File Offset: 0x000406BA
		public virtual bool IsDaylightSavingTime(DateTime time)
		{
			return TimeZone.IsDaylightSavingTime(time, this.GetDaylightChanges(time.Year));
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x000424CF File Offset: 0x000406CF
		public static bool IsDaylightSavingTime(DateTime time, DaylightTime daylightTimes)
		{
			return TimeZone.CalculateUtcOffset(time, daylightTimes) != TimeSpan.Zero;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x000424E4 File Offset: 0x000406E4
		internal static TimeSpan CalculateUtcOffset(DateTime time, DaylightTime daylightTimes)
		{
			if (daylightTimes == null)
			{
				return TimeSpan.Zero;
			}
			if (time.Kind == DateTimeKind.Utc)
			{
				return TimeSpan.Zero;
			}
			DateTime dateTime = daylightTimes.Start + daylightTimes.Delta;
			DateTime end = daylightTimes.End;
			DateTime t;
			DateTime t2;
			if (daylightTimes.Delta.Ticks > 0L)
			{
				t = end - daylightTimes.Delta;
				t2 = end;
			}
			else
			{
				t = dateTime;
				t2 = dateTime - daylightTimes.Delta;
			}
			bool flag = false;
			if (dateTime > end)
			{
				if (time >= dateTime || time < end)
				{
					flag = true;
				}
			}
			else if (time >= dateTime && time < end)
			{
				flag = true;
			}
			if (flag && time >= t && time < t2)
			{
				flag = time.IsAmbiguousDaylightSavingTime();
			}
			if (flag)
			{
				return daylightTimes.Delta;
			}
			return TimeSpan.Zero;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x000425BD File Offset: 0x000407BD
		internal static void ClearCachedData()
		{
			TimeZone.currentTimeZone = null;
		}

		// Token: 0x04001317 RID: 4887
		private static volatile TimeZone currentTimeZone;

		// Token: 0x04001318 RID: 4888
		private static object s_InternalSyncObject;
	}
}
