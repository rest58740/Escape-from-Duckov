using System;
using System.Collections.Generic;

namespace Animancer.FSM
{
	// Token: 0x0200000E RID: 14
	public class ReverseComparer<T> : IComparer<T>
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00002708 File Offset: 0x00000908
		private ReverseComparer()
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002710 File Offset: 0x00000910
		public int Compare(T x, T y)
		{
			return Comparer<T>.Default.Compare(y, x);
		}

		// Token: 0x0400000F RID: 15
		public static readonly ReverseComparer<T> Instance = new ReverseComparer<T>();
	}
}
