using System;

namespace Steamworks
{
	// Token: 0x020001C7 RID: 455
	[Serializable]
	public struct TimelineEventHandle_t : IEquatable<TimelineEventHandle_t>, IComparable<TimelineEventHandle_t>
	{
		// Token: 0x06000B34 RID: 2868 RVA: 0x00010A8D File Offset: 0x0000EC8D
		public TimelineEventHandle_t(ulong value)
		{
			this.m_TimelineEventHandle = value;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00010A96 File Offset: 0x0000EC96
		public override string ToString()
		{
			return this.m_TimelineEventHandle.ToString();
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00010AA3 File Offset: 0x0000ECA3
		public override bool Equals(object other)
		{
			return other is TimelineEventHandle_t && this == (TimelineEventHandle_t)other;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00010AC0 File Offset: 0x0000ECC0
		public override int GetHashCode()
		{
			return this.m_TimelineEventHandle.GetHashCode();
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00010ACD File Offset: 0x0000ECCD
		public static bool operator ==(TimelineEventHandle_t x, TimelineEventHandle_t y)
		{
			return x.m_TimelineEventHandle == y.m_TimelineEventHandle;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00010ADD File Offset: 0x0000ECDD
		public static bool operator !=(TimelineEventHandle_t x, TimelineEventHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00010AE9 File Offset: 0x0000ECE9
		public static explicit operator TimelineEventHandle_t(ulong value)
		{
			return new TimelineEventHandle_t(value);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00010AF1 File Offset: 0x0000ECF1
		public static explicit operator ulong(TimelineEventHandle_t that)
		{
			return that.m_TimelineEventHandle;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00010AF9 File Offset: 0x0000ECF9
		public bool Equals(TimelineEventHandle_t other)
		{
			return this.m_TimelineEventHandle == other.m_TimelineEventHandle;
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00010B09 File Offset: 0x0000ED09
		public int CompareTo(TimelineEventHandle_t other)
		{
			return this.m_TimelineEventHandle.CompareTo(other.m_TimelineEventHandle);
		}

		// Token: 0x04000B47 RID: 2887
		public ulong m_TimelineEventHandle;
	}
}
