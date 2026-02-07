using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200008F RID: 143
	[DisallowMultipleComponent]
	public sealed class AsyncCollisionExitTrigger : AsyncTriggerBase<Collision>
	{
		// Token: 0x06000451 RID: 1105 RVA: 0x0000C012 File Offset: 0x0000A212
		private void OnCollisionExit(Collision coll)
		{
			base.RaiseEvent(coll);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000C01B File Offset: 0x0000A21B
		public IAsyncOnCollisionExitHandler GetOnCollisionExitAsyncHandler()
		{
			return new AsyncTriggerHandler<Collision>(this, false);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000C024 File Offset: 0x0000A224
		public IAsyncOnCollisionExitHandler GetOnCollisionExitAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collision>(this, cancellationToken, false);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000C02E File Offset: 0x0000A22E
		public UniTask<Collision> OnCollisionExitAsync()
		{
			return ((IAsyncOnCollisionExitHandler)new AsyncTriggerHandler<Collision>(this, true)).OnCollisionExitAsync();
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000C03C File Offset: 0x0000A23C
		public UniTask<Collision> OnCollisionExitAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnCollisionExitHandler)new AsyncTriggerHandler<Collision>(this, cancellationToken, true)).OnCollisionExitAsync();
		}
	}
}
