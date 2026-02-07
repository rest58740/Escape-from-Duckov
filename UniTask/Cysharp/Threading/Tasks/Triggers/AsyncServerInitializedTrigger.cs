using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000CB RID: 203
	[DisallowMultipleComponent]
	public sealed class AsyncServerInitializedTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x0000C806 File Offset: 0x0000AA06
		private void OnServerInitialized()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000C813 File Offset: 0x0000AA13
		public IAsyncOnServerInitializedHandler GetOnServerInitializedAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000C81C File Offset: 0x0000AA1C
		public IAsyncOnServerInitializedHandler GetOnServerInitializedAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000C826 File Offset: 0x0000AA26
		public UniTask OnServerInitializedAsync()
		{
			return ((IAsyncOnServerInitializedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnServerInitializedAsync();
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000C834 File Offset: 0x0000AA34
		public UniTask OnServerInitializedAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnServerInitializedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnServerInitializedAsync();
		}
	}
}
