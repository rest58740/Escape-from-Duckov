using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000DB RID: 219
	[DisallowMultipleComponent]
	public sealed class AsyncTriggerStay2DTrigger : AsyncTriggerBase<Collider2D>
	{
		// Token: 0x0600055B RID: 1371 RVA: 0x0000CA1A File Offset: 0x0000AC1A
		private void OnTriggerStay2D(Collider2D other)
		{
			base.RaiseEvent(other);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0000CA23 File Offset: 0x0000AC23
		public IAsyncOnTriggerStay2DHandler GetOnTriggerStay2DAsyncHandler()
		{
			return new AsyncTriggerHandler<Collider2D>(this, false);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0000CA2C File Offset: 0x0000AC2C
		public IAsyncOnTriggerStay2DHandler GetOnTriggerStay2DAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collider2D>(this, cancellationToken, false);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0000CA36 File Offset: 0x0000AC36
		public UniTask<Collider2D> OnTriggerStay2DAsync()
		{
			return ((IAsyncOnTriggerStay2DHandler)new AsyncTriggerHandler<Collider2D>(this, true)).OnTriggerStay2DAsync();
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0000CA44 File Offset: 0x0000AC44
		public UniTask<Collider2D> OnTriggerStay2DAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnTriggerStay2DHandler)new AsyncTriggerHandler<Collider2D>(this, cancellationToken, true)).OnTriggerStay2DAsync();
		}
	}
}
