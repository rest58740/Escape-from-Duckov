using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000CF RID: 207
	[DisallowMultipleComponent]
	public sealed class AsyncTransformParentChangedTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000531 RID: 1329 RVA: 0x0000C890 File Offset: 0x0000AA90
		private void OnTransformParentChanged()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000C89D File Offset: 0x0000AA9D
		public IAsyncOnTransformParentChangedHandler GetOnTransformParentChangedAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000C8A6 File Offset: 0x0000AAA6
		public IAsyncOnTransformParentChangedHandler GetOnTransformParentChangedAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
		public UniTask OnTransformParentChangedAsync()
		{
			return ((IAsyncOnTransformParentChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnTransformParentChangedAsync();
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000C8BE File Offset: 0x0000AABE
		public UniTask OnTransformParentChangedAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnTransformParentChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnTransformParentChangedAsync();
		}
	}
}
