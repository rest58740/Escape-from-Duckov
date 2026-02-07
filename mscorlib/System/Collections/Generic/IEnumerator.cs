using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A9A RID: 2714
	public interface IEnumerator<out T> : IDisposable, IEnumerator
	{
		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x06006128 RID: 24872
		T Current { get; }
	}
}
