using System;
using UnityEngine.ParticleSystemJobs;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000BA RID: 186
	public interface IAsyncOnParticleUpdateJobScheduledHandler
	{
		// Token: 0x060004EA RID: 1258
		UniTask<ParticleSystemJobData> OnParticleUpdateJobScheduledAsync();
	}
}
