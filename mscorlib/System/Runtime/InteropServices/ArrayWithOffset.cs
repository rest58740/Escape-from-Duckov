using System;
using System.Security;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006E1 RID: 1761
	[ComVisible(true)]
	[Serializable]
	public struct ArrayWithOffset
	{
		// Token: 0x06004052 RID: 16466 RVA: 0x000E0E52 File Offset: 0x000DF052
		[SecuritySafeCritical]
		public ArrayWithOffset(object array, int offset)
		{
			this.m_array = array;
			this.m_offset = offset;
			this.m_count = 0;
			this.m_count = this.CalculateCount();
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x000E0E75 File Offset: 0x000DF075
		public object GetArray()
		{
			return this.m_array;
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x000E0E7D File Offset: 0x000DF07D
		public int GetOffset()
		{
			return this.m_offset;
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x000E0E85 File Offset: 0x000DF085
		public override int GetHashCode()
		{
			return this.m_count + this.m_offset;
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x000E0E94 File Offset: 0x000DF094
		public override bool Equals(object obj)
		{
			return obj is ArrayWithOffset && this.Equals((ArrayWithOffset)obj);
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x000E0EAC File Offset: 0x000DF0AC
		public bool Equals(ArrayWithOffset obj)
		{
			return obj.m_array == this.m_array && obj.m_offset == this.m_offset && obj.m_count == this.m_count;
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x000E0EDA File Offset: 0x000DF0DA
		public static bool operator ==(ArrayWithOffset a, ArrayWithOffset b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x000E0EE4 File Offset: 0x000DF0E4
		public static bool operator !=(ArrayWithOffset a, ArrayWithOffset b)
		{
			return !(a == b);
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x000E0EF0 File Offset: 0x000DF0F0
		private int CalculateCount()
		{
			Array array = this.m_array as Array;
			if (array == null)
			{
				throw new ArgumentException();
			}
			return array.Rank * array.Length - this.m_offset;
		}

		// Token: 0x04002A23 RID: 10787
		private object m_array;

		// Token: 0x04002A24 RID: 10788
		private int m_offset;

		// Token: 0x04002A25 RID: 10789
		private int m_count;
	}
}
