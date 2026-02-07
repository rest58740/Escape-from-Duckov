using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000DD RID: 221
	[DisallowMultipleComponent]
	public sealed class AsyncValidateTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x0000CA5B File Offset: 0x0000AC5B
		private void OnValidate()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0000CA68 File Offset: 0x0000AC68
		public IAsyncOnValidateHandler GetOnValidateAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0000CA71 File Offset: 0x0000AC71
		public IAsyncOnValidateHandler GetOnValidateAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0000CA7B File Offset: 0x0000AC7B
		public UniTask OnValidateAsync()
		{
			return ((IAsyncOnValidateHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnValidateAsync();
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0000CA89 File Offset: 0x0000AC89
		public UniTask OnValidateAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnValidateHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnValidateAsync();
		}
	}
}
