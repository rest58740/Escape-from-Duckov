using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200008B RID: 139
	[DisallowMultipleComponent]
	public sealed class AsyncCollisionEnterTrigger : AsyncTriggerBase<Collision>
	{
		// Token: 0x06000443 RID: 1091 RVA: 0x0000BF90 File Offset: 0x0000A190
		private void OnCollisionEnter(Collision coll)
		{
			base.RaiseEvent(coll);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000BF99 File Offset: 0x0000A199
		public IAsyncOnCollisionEnterHandler GetOnCollisionEnterAsyncHandler()
		{
			return new AsyncTriggerHandler<Collision>(this, false);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000BFA2 File Offset: 0x0000A1A2
		public IAsyncOnCollisionEnterHandler GetOnCollisionEnterAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collision>(this, cancellationToken, false);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000BFAC File Offset: 0x0000A1AC
		public UniTask<Collision> OnCollisionEnterAsync()
		{
			return ((IAsyncOnCollisionEnterHandler)new AsyncTriggerHandler<Collision>(this, true)).OnCollisionEnterAsync();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000BFBA File Offset: 0x0000A1BA
		public UniTask<Collision> OnCollisionEnterAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnCollisionEnterHandler)new AsyncTriggerHandler<Collision>(this, cancellationToken, true)).OnCollisionEnterAsync();
		}
	}
}
