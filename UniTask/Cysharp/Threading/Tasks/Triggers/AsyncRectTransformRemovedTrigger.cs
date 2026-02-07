using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000C5 RID: 197
	[DisallowMultipleComponent]
	public sealed class AsyncRectTransformRemovedTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x0600050E RID: 1294 RVA: 0x0000C735 File Offset: 0x0000A935
		private void OnRectTransformRemoved()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0000C742 File Offset: 0x0000A942
		public IAsyncOnRectTransformRemovedHandler GetOnRectTransformRemovedAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0000C74B File Offset: 0x0000A94B
		public IAsyncOnRectTransformRemovedHandler GetOnRectTransformRemovedAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0000C755 File Offset: 0x0000A955
		public UniTask OnRectTransformRemovedAsync()
		{
			return ((IAsyncOnRectTransformRemovedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnRectTransformRemovedAsync();
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0000C763 File Offset: 0x0000A963
		public UniTask OnRectTransformRemovedAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnRectTransformRemovedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnRectTransformRemovedAsync();
		}
	}
}
