using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000E3 RID: 227
	[DisallowMultipleComponent]
	public sealed class AsyncUpdateTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x0000CB2A File Offset: 0x0000AD2A
		private void Update()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0000CB37 File Offset: 0x0000AD37
		public IAsyncUpdateHandler GetUpdateAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0000CB40 File Offset: 0x0000AD40
		public IAsyncUpdateHandler GetUpdateAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0000CB4A File Offset: 0x0000AD4A
		public UniTask UpdateAsync()
		{
			return ((IAsyncUpdateHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).UpdateAsync();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0000CB58 File Offset: 0x0000AD58
		public UniTask UpdateAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncUpdateHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).UpdateAsync();
		}
	}
}
