using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200009B RID: 155
	[DisallowMultipleComponent]
	public sealed class AsyncDrawGizmosTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x0600047B RID: 1147 RVA: 0x0000C19C File Offset: 0x0000A39C
		private void OnDrawGizmos()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000C1A9 File Offset: 0x0000A3A9
		public IAsyncOnDrawGizmosHandler GetOnDrawGizmosAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000C1B2 File Offset: 0x0000A3B2
		public IAsyncOnDrawGizmosHandler GetOnDrawGizmosAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000C1BC File Offset: 0x0000A3BC
		public UniTask OnDrawGizmosAsync()
		{
			return ((IAsyncOnDrawGizmosHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnDrawGizmosAsync();
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000C1CA File Offset: 0x0000A3CA
		public UniTask OnDrawGizmosAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnDrawGizmosHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnDrawGizmosAsync();
		}
	}
}
