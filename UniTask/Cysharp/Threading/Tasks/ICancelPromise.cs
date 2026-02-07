using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200004B RID: 75
	public interface ICancelPromise
	{
		// Token: 0x0600019D RID: 413
		bool TrySetCanceled(CancellationToken cancellationToken = default(CancellationToken));
	}
}
