using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000B7 RID: 183
	[DisallowMultipleComponent]
	public sealed class AsyncParticleSystemStoppedTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004DD RID: 1245 RVA: 0x0000C556 File Offset: 0x0000A756
		private void OnParticleSystemStopped()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0000C563 File Offset: 0x0000A763
		public IAsyncOnParticleSystemStoppedHandler GetOnParticleSystemStoppedAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0000C56C File Offset: 0x0000A76C
		public IAsyncOnParticleSystemStoppedHandler GetOnParticleSystemStoppedAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0000C576 File Offset: 0x0000A776
		public UniTask OnParticleSystemStoppedAsync()
		{
			return ((IAsyncOnParticleSystemStoppedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnParticleSystemStoppedAsync();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000C584 File Offset: 0x0000A784
		public UniTask OnParticleSystemStoppedAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnParticleSystemStoppedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnParticleSystemStoppedAsync();
		}
	}
}
