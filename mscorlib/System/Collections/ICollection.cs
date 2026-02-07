using System;

namespace System.Collections
{
	// Token: 0x02000A12 RID: 2578
	public interface ICollection : IEnumerable
	{
		// Token: 0x06005B7E RID: 23422
		void CopyTo(Array array, int index);

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x06005B7F RID: 23423
		int Count { get; }

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06005B80 RID: 23424
		object SyncRoot { get; }

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06005B81 RID: 23425
		bool IsSynchronized { get; }
	}
}
