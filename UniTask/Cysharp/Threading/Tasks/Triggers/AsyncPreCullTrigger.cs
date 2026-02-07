using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000BF RID: 191
	[DisallowMultipleComponent]
	public sealed class AsyncPreCullTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x0000C666 File Offset: 0x0000A866
		private void OnPreCull()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000C673 File Offset: 0x0000A873
		public IAsyncOnPreCullHandler GetOnPreCullAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000C67C File Offset: 0x0000A87C
		public IAsyncOnPreCullHandler GetOnPreCullAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000C686 File Offset: 0x0000A886
		public UniTask OnPreCullAsync()
		{
			return ((IAsyncOnPreCullHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnPreCullAsync();
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000C694 File Offset: 0x0000A894
		public UniTask OnPreCullAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnPreCullHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnPreCullAsync();
		}
	}
}
