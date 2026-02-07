using System;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200001F RID: 31
	public interface IAsyncWriter<T>
	{
		// Token: 0x0600025E RID: 606
		UniTask YieldAsync(T value);
	}
}
