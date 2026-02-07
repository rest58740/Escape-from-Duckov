using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000CD RID: 205
	[DisallowMultipleComponent]
	public sealed class AsyncTransformChildrenChangedTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x0600052A RID: 1322 RVA: 0x0000C84B File Offset: 0x0000AA4B
		private void OnTransformChildrenChanged()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000C858 File Offset: 0x0000AA58
		public IAsyncOnTransformChildrenChangedHandler GetOnTransformChildrenChangedAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000C861 File Offset: 0x0000AA61
		public IAsyncOnTransformChildrenChangedHandler GetOnTransformChildrenChangedAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000C86B File Offset: 0x0000AA6B
		public UniTask OnTransformChildrenChangedAsync()
		{
			return ((IAsyncOnTransformChildrenChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnTransformChildrenChangedAsync();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0000C879 File Offset: 0x0000AA79
		public UniTask OnTransformChildrenChangedAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnTransformChildrenChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnTransformChildrenChangedAsync();
		}
	}
}
