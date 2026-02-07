using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A9E RID: 2718
	public interface IReadOnlyDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<!0, !1>>, IEnumerable
	{
		// Token: 0x06006131 RID: 24881
		bool ContainsKey(TKey key);

		// Token: 0x06006132 RID: 24882
		bool TryGetValue(TKey key, out TValue value);

		// Token: 0x17001148 RID: 4424
		TValue this[TKey key]
		{
			get;
		}

		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x06006134 RID: 24884
		IEnumerable<TKey> Keys { get; }

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x06006135 RID: 24885
		IEnumerable<TValue> Values { get; }
	}
}
