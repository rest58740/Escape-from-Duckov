using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000097 RID: 151
	[DisallowMultipleComponent]
	public sealed class AsyncControllerColliderHitTrigger : AsyncTriggerBase<ControllerColliderHit>
	{
		// Token: 0x0600046D RID: 1133 RVA: 0x0000C116 File Offset: 0x0000A316
		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			base.RaiseEvent(hit);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000C11F File Offset: 0x0000A31F
		public IAsyncOnControllerColliderHitHandler GetOnControllerColliderHitAsyncHandler()
		{
			return new AsyncTriggerHandler<ControllerColliderHit>(this, false);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000C128 File Offset: 0x0000A328
		public IAsyncOnControllerColliderHitHandler GetOnControllerColliderHitAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<ControllerColliderHit>(this, cancellationToken, false);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000C132 File Offset: 0x0000A332
		public UniTask<ControllerColliderHit> OnControllerColliderHitAsync()
		{
			return ((IAsyncOnControllerColliderHitHandler)new AsyncTriggerHandler<ControllerColliderHit>(this, true)).OnControllerColliderHitAsync();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000C140 File Offset: 0x0000A340
		public UniTask<ControllerColliderHit> OnControllerColliderHitAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnControllerColliderHitHandler)new AsyncTriggerHandler<ControllerColliderHit>(this, cancellationToken, true)).OnControllerColliderHitAsync();
		}
	}
}
