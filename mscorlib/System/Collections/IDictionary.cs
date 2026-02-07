using System;

namespace System.Collections
{
	// Token: 0x02000A14 RID: 2580
	public interface IDictionary : ICollection, IEnumerable
	{
		// Token: 0x17000FC2 RID: 4034
		object this[object key]
		{
			get;
			set;
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06005B85 RID: 23429
		ICollection Keys { get; }

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06005B86 RID: 23430
		ICollection Values { get; }

		// Token: 0x06005B87 RID: 23431
		bool Contains(object key);

		// Token: 0x06005B88 RID: 23432
		void Add(object key, object value);

		// Token: 0x06005B89 RID: 23433
		void Clear();

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x06005B8A RID: 23434
		bool IsReadOnly { get; }

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06005B8B RID: 23435
		bool IsFixedSize { get; }

		// Token: 0x06005B8C RID: 23436
		IDictionaryEnumerator GetEnumerator();

		// Token: 0x06005B8D RID: 23437
		void Remove(object key);
	}
}
