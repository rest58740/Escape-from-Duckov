using System;

namespace System.Collections
{
	// Token: 0x02000A19 RID: 2585
	public interface IList : ICollection, IEnumerable
	{
		// Token: 0x17000FCB RID: 4043
		object this[int index]
		{
			get;
			set;
		}

		// Token: 0x06005B99 RID: 23449
		int Add(object value);

		// Token: 0x06005B9A RID: 23450
		bool Contains(object value);

		// Token: 0x06005B9B RID: 23451
		void Clear();

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06005B9C RID: 23452
		bool IsReadOnly { get; }

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06005B9D RID: 23453
		bool IsFixedSize { get; }

		// Token: 0x06005B9E RID: 23454
		int IndexOf(object value);

		// Token: 0x06005B9F RID: 23455
		void Insert(int index, object value);

		// Token: 0x06005BA0 RID: 23456
		void Remove(object value);

		// Token: 0x06005BA1 RID: 23457
		void RemoveAt(int index);
	}
}
