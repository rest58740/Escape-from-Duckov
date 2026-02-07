using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200008D RID: 141
	[DisallowMultipleComponent]
	public sealed class AsyncCollisionEnter2DTrigger : AsyncTriggerBase<Collision2D>
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x0000BFD1 File Offset: 0x0000A1D1
		private void OnCollisionEnter2D(Collision2D coll)
		{
			base.RaiseEvent(coll);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000BFDA File Offset: 0x0000A1DA
		public IAsyncOnCollisionEnter2DHandler GetOnCollisionEnter2DAsyncHandler()
		{
			return new AsyncTriggerHandler<Collision2D>(this, false);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000BFE3 File Offset: 0x0000A1E3
		public IAsyncOnCollisionEnter2DHandler GetOnCollisionEnter2DAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collision2D>(this, cancellationToken, false);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000BFED File Offset: 0x0000A1ED
		public UniTask<Collision2D> OnCollisionEnter2DAsync()
		{
			return ((IAsyncOnCollisionEnter2DHandler)new AsyncTriggerHandler<Collision2D>(this, true)).OnCollisionEnter2DAsync();
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000BFFB File Offset: 0x0000A1FB
		public UniTask<Collision2D> OnCollisionEnter2DAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnCollisionEnter2DHandler)new AsyncTriggerHandler<Collision2D>(this, cancellationToken, true)).OnCollisionEnter2DAsync();
		}
	}
}
