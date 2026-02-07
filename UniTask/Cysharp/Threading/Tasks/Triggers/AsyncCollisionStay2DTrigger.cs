using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000095 RID: 149
	[DisallowMultipleComponent]
	public sealed class AsyncCollisionStay2DTrigger : AsyncTriggerBase<Collision2D>
	{
		// Token: 0x06000466 RID: 1126 RVA: 0x0000C0D5 File Offset: 0x0000A2D5
		private void OnCollisionStay2D(Collision2D coll)
		{
			base.RaiseEvent(coll);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000C0DE File Offset: 0x0000A2DE
		public IAsyncOnCollisionStay2DHandler GetOnCollisionStay2DAsyncHandler()
		{
			return new AsyncTriggerHandler<Collision2D>(this, false);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000C0E7 File Offset: 0x0000A2E7
		public IAsyncOnCollisionStay2DHandler GetOnCollisionStay2DAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collision2D>(this, cancellationToken, false);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000C0F1 File Offset: 0x0000A2F1
		public UniTask<Collision2D> OnCollisionStay2DAsync()
		{
			return ((IAsyncOnCollisionStay2DHandler)new AsyncTriggerHandler<Collision2D>(this, true)).OnCollisionStay2DAsync();
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000C0FF File Offset: 0x0000A2FF
		public UniTask<Collision2D> OnCollisionStay2DAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnCollisionStay2DHandler)new AsyncTriggerHandler<Collision2D>(this, cancellationToken, true)).OnCollisionStay2DAsync();
		}
	}
}
