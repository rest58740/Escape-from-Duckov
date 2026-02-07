using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200001F RID: 31
	public interface IConnectableUniTaskAsyncEnumerable<out T> : IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x06000080 RID: 128
		IDisposable Connect();
	}
}
