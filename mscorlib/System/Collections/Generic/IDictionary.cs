using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A95 RID: 2709
	public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<!0, !1>>, IEnumerable
	{
		// Token: 0x1700113F RID: 4415
		TValue this[TKey key]
		{
			get;
			set;
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x0600611B RID: 24859
		ICollection<TKey> Keys { get; }

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x0600611C RID: 24860
		ICollection<TValue> Values { get; }

		// Token: 0x0600611D RID: 24861
		bool ContainsKey(TKey key);

		// Token: 0x0600611E RID: 24862
		void Add(TKey key, TValue value);

		// Token: 0x0600611F RID: 24863
		bool Remove(TKey key);

		// Token: 0x06006120 RID: 24864
		bool TryGetValue(TKey key, out TValue value);
	}
}
