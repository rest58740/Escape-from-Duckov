using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200009F RID: 159
	[DisallowMultipleComponent]
	public sealed class AsyncEnableTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000489 RID: 1161 RVA: 0x0000C226 File Offset: 0x0000A426
		private void OnEnable()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000C233 File Offset: 0x0000A433
		public IAsyncOnEnableHandler GetOnEnableAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000C23C File Offset: 0x0000A43C
		public IAsyncOnEnableHandler GetOnEnableAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000C246 File Offset: 0x0000A446
		public UniTask OnEnableAsync()
		{
			return ((IAsyncOnEnableHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnEnableAsync();
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000C254 File Offset: 0x0000A454
		public UniTask OnEnableAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnEnableHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnEnableAsync();
		}
	}
}
