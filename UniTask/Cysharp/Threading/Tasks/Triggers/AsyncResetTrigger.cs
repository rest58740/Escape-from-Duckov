using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000E1 RID: 225
	[DisallowMultipleComponent]
	public sealed class AsyncResetTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000570 RID: 1392 RVA: 0x0000CAE5 File Offset: 0x0000ACE5
		private void Reset()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0000CAF2 File Offset: 0x0000ACF2
		public IAsyncResetHandler GetResetAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000CAFB File Offset: 0x0000ACFB
		public IAsyncResetHandler GetResetAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0000CB05 File Offset: 0x0000AD05
		public UniTask ResetAsync()
		{
			return ((IAsyncResetHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).ResetAsync();
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0000CB13 File Offset: 0x0000AD13
		public UniTask ResetAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncResetHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).ResetAsync();
		}
	}
}
