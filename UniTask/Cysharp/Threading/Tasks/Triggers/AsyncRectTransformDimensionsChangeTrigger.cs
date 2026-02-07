using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000C3 RID: 195
	[DisallowMultipleComponent]
	public sealed class AsyncRectTransformDimensionsChangeTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000507 RID: 1287 RVA: 0x0000C6F0 File Offset: 0x0000A8F0
		private void OnRectTransformDimensionsChange()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0000C6FD File Offset: 0x0000A8FD
		public IAsyncOnRectTransformDimensionsChangeHandler GetOnRectTransformDimensionsChangeAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000C706 File Offset: 0x0000A906
		public IAsyncOnRectTransformDimensionsChangeHandler GetOnRectTransformDimensionsChangeAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0000C710 File Offset: 0x0000A910
		public UniTask OnRectTransformDimensionsChangeAsync()
		{
			return ((IAsyncOnRectTransformDimensionsChangeHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnRectTransformDimensionsChangeAsync();
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0000C71E File Offset: 0x0000A91E
		public UniTask OnRectTransformDimensionsChangeAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnRectTransformDimensionsChangeHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnRectTransformDimensionsChangeAsync();
		}
	}
}
