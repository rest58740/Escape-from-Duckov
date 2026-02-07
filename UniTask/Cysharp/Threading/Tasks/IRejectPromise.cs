using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200004A RID: 74
	public interface IRejectPromise
	{
		// Token: 0x0600019C RID: 412
		bool TrySetException(Exception exception);
	}
}
