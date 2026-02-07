using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000D9 RID: 217
	[DisallowMultipleComponent]
	public sealed class AsyncTriggerStayTrigger : AsyncTriggerBase<Collider>
	{
		// Token: 0x06000554 RID: 1364 RVA: 0x0000C9D9 File Offset: 0x0000ABD9
		private void OnTriggerStay(Collider other)
		{
			base.RaiseEvent(other);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0000C9E2 File Offset: 0x0000ABE2
		public IAsyncOnTriggerStayHandler GetOnTriggerStayAsyncHandler()
		{
			return new AsyncTriggerHandler<Collider>(this, false);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0000C9EB File Offset: 0x0000ABEB
		public IAsyncOnTriggerStayHandler GetOnTriggerStayAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collider>(this, cancellationToken, false);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0000C9F5 File Offset: 0x0000ABF5
		public UniTask<Collider> OnTriggerStayAsync()
		{
			return ((IAsyncOnTriggerStayHandler)new AsyncTriggerHandler<Collider>(this, true)).OnTriggerStayAsync();
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0000CA03 File Offset: 0x0000AC03
		public UniTask<Collider> OnTriggerStayAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnTriggerStayHandler)new AsyncTriggerHandler<Collider>(this, cancellationToken, true)).OnTriggerStayAsync();
		}
	}
}
