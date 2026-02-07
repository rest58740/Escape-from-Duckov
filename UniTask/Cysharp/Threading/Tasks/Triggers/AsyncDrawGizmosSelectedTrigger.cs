using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200009D RID: 157
	[DisallowMultipleComponent]
	public sealed class AsyncDrawGizmosSelectedTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000482 RID: 1154 RVA: 0x0000C1E1 File Offset: 0x0000A3E1
		private void OnDrawGizmosSelected()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000C1EE File Offset: 0x0000A3EE
		public IAsyncOnDrawGizmosSelectedHandler GetOnDrawGizmosSelectedAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000C1F7 File Offset: 0x0000A3F7
		public IAsyncOnDrawGizmosSelectedHandler GetOnDrawGizmosSelectedAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000C201 File Offset: 0x0000A401
		public UniTask OnDrawGizmosSelectedAsync()
		{
			return ((IAsyncOnDrawGizmosSelectedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnDrawGizmosSelectedAsync();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000C20F File Offset: 0x0000A40F
		public UniTask OnDrawGizmosSelectedAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnDrawGizmosSelectedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnDrawGizmosSelectedAsync();
		}
	}
}
