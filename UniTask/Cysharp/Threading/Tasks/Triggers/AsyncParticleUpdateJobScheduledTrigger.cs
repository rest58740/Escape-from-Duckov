using System;
using System.Threading;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000BB RID: 187
	[DisallowMultipleComponent]
	public sealed class AsyncParticleUpdateJobScheduledTrigger : AsyncTriggerBase<ParticleSystemJobData>
	{
		// Token: 0x060004EB RID: 1259 RVA: 0x0000C5E0 File Offset: 0x0000A7E0
		private void OnParticleUpdateJobScheduled(ParticleSystemJobData particles)
		{
			base.RaiseEvent(particles);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000C5E9 File Offset: 0x0000A7E9
		public IAsyncOnParticleUpdateJobScheduledHandler GetOnParticleUpdateJobScheduledAsyncHandler()
		{
			return new AsyncTriggerHandler<ParticleSystemJobData>(this, false);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0000C5F2 File Offset: 0x0000A7F2
		public IAsyncOnParticleUpdateJobScheduledHandler GetOnParticleUpdateJobScheduledAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<ParticleSystemJobData>(this, cancellationToken, false);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000C5FC File Offset: 0x0000A7FC
		public UniTask<ParticleSystemJobData> OnParticleUpdateJobScheduledAsync()
		{
			return ((IAsyncOnParticleUpdateJobScheduledHandler)new AsyncTriggerHandler<ParticleSystemJobData>(this, true)).OnParticleUpdateJobScheduledAsync();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000C60A File Offset: 0x0000A80A
		public UniTask<ParticleSystemJobData> OnParticleUpdateJobScheduledAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnParticleUpdateJobScheduledHandler)new AsyncTriggerHandler<ParticleSystemJobData>(this, cancellationToken, true)).OnParticleUpdateJobScheduledAsync();
		}
	}
}
