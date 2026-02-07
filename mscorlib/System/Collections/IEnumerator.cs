using System;

namespace System.Collections
{
	// Token: 0x02000A17 RID: 2583
	public interface IEnumerator
	{
		// Token: 0x06005B92 RID: 23442
		bool MoveNext();

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06005B93 RID: 23443
		object Current { get; }

		// Token: 0x06005B94 RID: 23444
		void Reset();
	}
}
