using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000D1 RID: 209
	[DisallowMultipleComponent]
	public sealed class AsyncTriggerEnterTrigger : AsyncTriggerBase<Collider>
	{
		// Token: 0x06000538 RID: 1336 RVA: 0x0000C8D5 File Offset: 0x0000AAD5
		private void OnTriggerEnter(Collider other)
		{
			base.RaiseEvent(other);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000C8DE File Offset: 0x0000AADE
		public IAsyncOnTriggerEnterHandler GetOnTriggerEnterAsyncHandler()
		{
			return new AsyncTriggerHandler<Collider>(this, false);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000C8E7 File Offset: 0x0000AAE7
		public IAsyncOnTriggerEnterHandler GetOnTriggerEnterAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<Collider>(this, cancellationToken, false);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000C8F1 File Offset: 0x0000AAF1
		public UniTask<Collider> OnTriggerEnterAsync()
		{
			return ((IAsyncOnTriggerEnterHandler)new AsyncTriggerHandler<Collider>(this, true)).OnTriggerEnterAsync();
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000C8FF File Offset: 0x0000AAFF
		public UniTask<Collider> OnTriggerEnterAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnTriggerEnterHandler)new AsyncTriggerHandler<Collider>(this, cancellationToken, true)).OnTriggerEnterAsync();
		}
	}
}
