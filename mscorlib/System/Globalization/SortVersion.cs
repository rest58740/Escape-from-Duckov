using System;

namespace System.Globalization
{
	// Token: 0x0200096E RID: 2414
	[Serializable]
	public sealed class SortVersion : IEquatable<SortVersion>
	{
		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06005549 RID: 21833 RVA: 0x0011E385 File Offset: 0x0011C585
		public int FullVersion
		{
			get
			{
				return this.m_NlsVersion;
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x0600554A RID: 21834 RVA: 0x0011E38D File Offset: 0x0011C58D
		public Guid SortId
		{
			get
			{
				return this.m_SortId;
			}
		}

		// Token: 0x0600554B RID: 21835 RVA: 0x0011E395 File Offset: 0x0011C595
		public SortVersion(int fullVersion, Guid sortId)
		{
			this.m_SortId = sortId;
			this.m_NlsVersion = fullVersion;
		}

		// Token: 0x0600554C RID: 21836 RVA: 0x0011E3AC File Offset: 0x0011C5AC
		internal SortVersion(int nlsVersion, int effectiveId, Guid customVersion)
		{
			this.m_NlsVersion = nlsVersion;
			if (customVersion == Guid.Empty)
			{
				byte h = (byte)(effectiveId >> 24);
				byte i = (byte)((effectiveId & 16711680) >> 16);
				byte j = (byte)((effectiveId & 65280) >> 8);
				byte k = (byte)(effectiveId & 255);
				customVersion = new Guid(0, 0, 0, 0, 0, 0, 0, h, i, j, k);
			}
			this.m_SortId = customVersion;
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x0011E414 File Offset: 0x0011C614
		public override bool Equals(object obj)
		{
			SortVersion sortVersion = obj as SortVersion;
			return sortVersion != null && this.Equals(sortVersion);
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x0011E43A File Offset: 0x0011C63A
		public bool Equals(SortVersion other)
		{
			return !(other == null) && this.m_NlsVersion == other.m_NlsVersion && this.m_SortId == other.m_SortId;
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x0011E468 File Offset: 0x0011C668
		public override int GetHashCode()
		{
			return this.m_NlsVersion * 7 | this.m_SortId.GetHashCode();
		}

		// Token: 0x06005550 RID: 21840 RVA: 0x0011E484 File Offset: 0x0011C684
		public static bool operator ==(SortVersion left, SortVersion right)
		{
			if (left != null)
			{
				return left.Equals(right);
			}
			return right == null || right.Equals(left);
		}

		// Token: 0x06005551 RID: 21841 RVA: 0x0011E49D File Offset: 0x0011C69D
		public static bool operator !=(SortVersion left, SortVersion right)
		{
			return !(left == right);
		}

		// Token: 0x040034E2 RID: 13538
		private int m_NlsVersion;

		// Token: 0x040034E3 RID: 13539
		private Guid m_SortId;
	}
}
