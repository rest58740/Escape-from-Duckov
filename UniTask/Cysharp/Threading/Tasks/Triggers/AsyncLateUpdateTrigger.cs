using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000075 RID: 117
	[DisallowMultipleComponent]
	public sealed class AsyncLateUpdateTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060003F6 RID: 1014 RVA: 0x0000BCA3 File Offset: 0x00009EA3
		private void LateUpdate()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000BCB0 File Offset: 0x00009EB0
		public IAsyncLateUpdateHandler GetLateUpdateAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000BCB9 File Offset: 0x00009EB9
		public IAsyncLateUpdateHandler GetLateUpdateAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000BCC3 File Offset: 0x00009EC3
		public UniTask LateUpdateAsync()
		{
			return ((IAsyncLateUpdateHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).LateUpdateAsync();
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000BCD1 File Offset: 0x00009ED1
		public UniTask LateUpdateAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncLateUpdateHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).LateUpdateAsync();
		}
	}
}
