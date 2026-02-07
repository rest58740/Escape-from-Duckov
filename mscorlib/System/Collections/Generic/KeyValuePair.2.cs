using System;

namespace System.Collections.Generic
{
	// Token: 0x02000AA2 RID: 2722
	[Serializable]
	public readonly struct KeyValuePair<TKey, TValue>
	{
		// Token: 0x0600613D RID: 24893 RVA: 0x00145277 File Offset: 0x00143477
		public KeyValuePair(TKey key, TValue value)
		{
			this.key = key;
			this.value = value;
		}

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x0600613E RID: 24894 RVA: 0x00145287 File Offset: 0x00143487
		public TKey Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x0600613F RID: 24895 RVA: 0x0014528F File Offset: 0x0014348F
		public TValue Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x00145297 File Offset: 0x00143497
		public override string ToString()
		{
			return KeyValuePair.PairToString(this.Key, this.Value);
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x001452B4 File Offset: 0x001434B4
		public void Deconstruct(out TKey key, out TValue value)
		{
			key = this.Key;
			value = this.Value;
		}

		// Token: 0x040039E6 RID: 14822
		private readonly TKey key;

		// Token: 0x040039E7 RID: 14823
		private readonly TValue value;
	}
}
