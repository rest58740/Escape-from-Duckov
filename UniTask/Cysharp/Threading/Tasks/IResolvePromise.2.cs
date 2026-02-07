using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000049 RID: 73
	public interface IResolvePromise<T>
	{
		// Token: 0x0600019B RID: 411
		bool TrySetResult(T value);
	}
}
