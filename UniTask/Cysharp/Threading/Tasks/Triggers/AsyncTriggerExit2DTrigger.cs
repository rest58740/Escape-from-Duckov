using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000D7 RID: 215
	[DisallowMultipleComponent]
	public sealed class AsyncTriggerExit2DTrigger : AsyncTriggerBase<Collider2D>
	{
		// Token: 0x0600054D RID: 1357 RVA: 0x0000C998 File Offset: 0x0000AB98
		private void OnTriggerExit2D(Collider2D other)
		{
			base.RaiseEvent(other);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0000C9A1 File Offset: 0x0000ABA1
		public IAsyncOnTriggerExit2DHandler GetOnTriggerExit2DAsyncHandler()
		{
			return new AsyncTriggerHandler<Collider2D>(this, false);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0000C9AA File Offset: 0x0000ABAA
		public IAsyncOnTriggerExit2DHandler GetOnTriggerExit2DAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collider2D>(this, cancellationToken, false);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0000C9B4 File Offset: 0x0000ABB4
		public UniTask<Collider2D> OnTriggerExit2DAsync()
		{
			return ((IAsyncOnTriggerExit2DHandler)new AsyncTriggerHandler<Collider2D>(this, true)).OnTriggerExit2DAsync();
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0000C9C2 File Offset: 0x0000ABC2
		public UniTask<Collider2D> OnTriggerExit2DAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnTriggerExit2DHandler)new AsyncTriggerHandler<Collider2D>(this, cancellationToken, true)).OnTriggerExit2DAsync();
		}
	}
}
