using System;
using System.Collections;
using System.Collections.Generic;

namespace Sirenix.Utilities
{
	// Token: 0x02000022 RID: 34
	[Serializable]
	public class ImmutableHashSet<T> : IEnumerable<!0>, IEnumerable
	{
		// Token: 0x06000167 RID: 359 RVA: 0x0000B8A8 File Offset: 0x00009AA8
		public ImmutableHashSet(HashSet<T> hashSet)
		{
			this.hashSet = hashSet;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000B8B7 File Offset: 0x00009AB7
		public bool Contains(T item)
		{
			return this.hashSet.Contains(item);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000B8C5 File Offset: 0x00009AC5
		public IEnumerator<T> GetEnumerator()
		{
			return this.hashSet.GetEnumerator();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000B8C5 File Offset: 0x00009AC5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.hashSet.GetEnumerator();
		}

		// Token: 0x04000055 RID: 85
		private readonly HashSet<T> hashSet;
	}
}
