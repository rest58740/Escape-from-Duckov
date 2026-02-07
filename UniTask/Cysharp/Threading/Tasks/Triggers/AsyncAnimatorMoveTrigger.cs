using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000079 RID: 121
	[DisallowMultipleComponent]
	public sealed class AsyncAnimatorMoveTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000404 RID: 1028 RVA: 0x0000BD29 File Offset: 0x00009F29
		private void OnAnimatorMove()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000BD36 File Offset: 0x00009F36
		public IAsyncOnAnimatorMoveHandler GetOnAnimatorMoveAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000BD3F File Offset: 0x00009F3F
		public IAsyncOnAnimatorMoveHandler GetOnAnimatorMoveAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000BD49 File Offset: 0x00009F49
		public UniTask OnAnimatorMoveAsync()
		{
			return ((IAsyncOnAnimatorMoveHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnAnimatorMoveAsync();
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000BD57 File Offset: 0x00009F57
		public UniTask OnAnimatorMoveAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnAnimatorMoveHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnAnimatorMoveAsync();
		}
	}
}
