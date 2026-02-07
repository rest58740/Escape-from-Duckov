using System;

namespace Steamworks
{
	// Token: 0x020001C9 RID: 457
	[Serializable]
	public struct AppId_t : IEquatable<AppId_t>, IComparable<AppId_t>
	{
		// Token: 0x06000B49 RID: 2889 RVA: 0x00010BB8 File Offset: 0x0000EDB8
		public AppId_t(uint value)
		{
			this.m_AppId = value;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00010BC1 File Offset: 0x0000EDC1
		public override string ToString()
		{
			return this.m_AppId.ToString();
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00010BCE File Offset: 0x0000EDCE
		public override bool Equals(object other)
		{
			return other is AppId_t && this == (AppId_t)other;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00010BEB File Offset: 0x0000EDEB
		public override int GetHashCode()
		{
			return this.m_AppId.GetHashCode();
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00010BF8 File Offset: 0x0000EDF8
		public static bool operator ==(AppId_t x, AppId_t y)
		{
			return x.m_AppId == y.m_AppId;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00010C08 File Offset: 0x0000EE08
		public static bool operator !=(AppId_t x, AppId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00010C14 File Offset: 0x0000EE14
		public static explicit operator AppId_t(uint value)
		{
			return new AppId_t(value);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00010C1C File Offset: 0x0000EE1C
		public static explicit operator uint(AppId_t that)
		{
			return that.m_AppId;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00010C24 File Offset: 0x0000EE24
		public bool Equals(AppId_t other)
		{
			return this.m_AppId == other.m_AppId;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00010C34 File Offset: 0x0000EE34
		public int CompareTo(AppId_t other)
		{
			return this.m_AppId.CompareTo(other.m_AppId);
		}

		// Token: 0x04000B4A RID: 2890
		public static readonly AppId_t Invalid = new AppId_t(0U);

		// Token: 0x04000B4B RID: 2891
		public uint m_AppId;
	}
}
