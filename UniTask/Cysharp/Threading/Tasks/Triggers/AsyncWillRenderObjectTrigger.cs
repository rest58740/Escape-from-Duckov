using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000DF RID: 223
	[DisallowMultipleComponent]
	public sealed class AsyncWillRenderObjectTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x0000CAA0 File Offset: 0x0000ACA0
		private void OnWillRenderObject()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0000CAAD File Offset: 0x0000ACAD
		public IAsyncOnWillRenderObjectHandler GetOnWillRenderObjectAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0000CAB6 File Offset: 0x0000ACB6
		public IAsyncOnWillRenderObjectHandler GetOnWillRenderObjectAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0000CAC0 File Offset: 0x0000ACC0
		public UniTask OnWillRenderObjectAsync()
		{
			return ((IAsyncOnWillRenderObjectHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnWillRenderObjectAsync();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0000CACE File Offset: 0x0000ACCE
		public UniTask OnWillRenderObjectAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnWillRenderObjectHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnWillRenderObjectAsync();
		}
	}
}
