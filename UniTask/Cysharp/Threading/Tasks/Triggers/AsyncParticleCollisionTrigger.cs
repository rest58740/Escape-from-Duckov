using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000B5 RID: 181
	[DisallowMultipleComponent]
	public sealed class AsyncParticleCollisionTrigger : AsyncTriggerBase<GameObject>
	{
		// Token: 0x060004D6 RID: 1238 RVA: 0x0000C515 File Offset: 0x0000A715
		private void OnParticleCollision(GameObject other)
		{
			base.RaiseEvent(other);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000C51E File Offset: 0x0000A71E
		public IAsyncOnParticleCollisionHandler GetOnParticleCollisionAsyncHandler()
		{
			return new AsyncTriggerHandler<GameObject>(this, false);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000C527 File Offset: 0x0000A727
		public IAsyncOnParticleCollisionHandler GetOnParticleCollisionAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<GameObject>(this, cancellationToken, false);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000C531 File Offset: 0x0000A731
		public UniTask<GameObject> OnParticleCollisionAsync()
		{
			return ((IAsyncOnParticleCollisionHandler)new AsyncTriggerHandler<GameObject>(this, true)).OnParticleCollisionAsync();
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0000C53F File Offset: 0x0000A73F
		public UniTask<GameObject> OnParticleCollisionAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnParticleCollisionHandler)new AsyncTriggerHandler<GameObject>(this, cancellationToken, true)).OnParticleCollisionAsync();
		}
	}
}
