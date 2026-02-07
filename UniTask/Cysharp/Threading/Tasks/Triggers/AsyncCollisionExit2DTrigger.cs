using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000091 RID: 145
	[DisallowMultipleComponent]
	public sealed class AsyncCollisionExit2DTrigger : AsyncTriggerBase<Collision2D>
	{
		// Token: 0x06000458 RID: 1112 RVA: 0x0000C053 File Offset: 0x0000A253
		private void OnCollisionExit2D(Collision2D coll)
		{
			base.RaiseEvent(coll);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000C05C File Offset: 0x0000A25C
		public IAsyncOnCollisionExit2DHandler GetOnCollisionExit2DAsyncHandler()
		{
			return new AsyncTriggerHandler<Collision2D>(this, false);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000C065 File Offset: 0x0000A265
		public IAsyncOnCollisionExit2DHandler GetOnCollisionExit2DAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collision2D>(this, cancellationToken, false);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000C06F File Offset: 0x0000A26F
		public UniTask<Collision2D> OnCollisionExit2DAsync()
		{
			return ((IAsyncOnCollisionExit2DHandler)new AsyncTriggerHandler<Collision2D>(this, true)).OnCollisionExit2DAsync();
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000C07D File Offset: 0x0000A27D
		public UniTask<Collision2D> OnCollisionExit2DAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnCollisionExit2DHandler)new AsyncTriggerHandler<Collision2D>(this, cancellationToken, true)).OnCollisionExit2DAsync();
		}
	}
}
