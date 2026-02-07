using System;

namespace System.Collections.Generic
{
	// Token: 0x02000A99 RID: 2713
	public interface IEnumerable<out T> : IEnumerable
	{
		// Token: 0x06006127 RID: 24871
		IEnumerator<T> GetEnumerator();
	}
}
