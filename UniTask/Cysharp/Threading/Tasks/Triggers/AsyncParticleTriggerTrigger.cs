using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000B9 RID: 185
	[DisallowMultipleComponent]
	public sealed class AsyncParticleTriggerTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004E4 RID: 1252 RVA: 0x0000C59B File Offset: 0x0000A79B
		private void OnParticleTrigger()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0000C5A8 File Offset: 0x0000A7A8
		public IAsyncOnParticleTriggerHandler GetOnParticleTriggerAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0000C5B1 File Offset: 0x0000A7B1
		public IAsyncOnParticleTriggerHandler GetOnParticleTriggerAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000C5BB File Offset: 0x0000A7BB
		public UniTask OnParticleTriggerAsync()
		{
			return ((IAsyncOnParticleTriggerHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnParticleTriggerAsync();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000C5C9 File Offset: 0x0000A7C9
		public UniTask OnParticleTriggerAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnParticleTriggerHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnParticleTriggerAsync();
		}
	}
}
