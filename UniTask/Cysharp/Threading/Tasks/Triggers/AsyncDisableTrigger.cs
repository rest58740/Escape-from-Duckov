using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000099 RID: 153
	[DisallowMultipleComponent]
	public sealed class AsyncDisableTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000474 RID: 1140 RVA: 0x0000C157 File Offset: 0x0000A357
		private void OnDisable()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000C164 File Offset: 0x0000A364
		public IAsyncOnDisableHandler GetOnDisableAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000C16D File Offset: 0x0000A36D
		public IAsyncOnDisableHandler GetOnDisableAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000C177 File Offset: 0x0000A377
		public UniTask OnDisableAsync()
		{
			return ((IAsyncOnDisableHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnDisableAsync();
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000C185 File Offset: 0x0000A385
		public UniTask OnDisableAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnDisableHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnDisableAsync();
		}
	}
}
