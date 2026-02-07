using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000077 RID: 119
	[DisallowMultipleComponent]
	public sealed class AsyncAnimatorIKTrigger : AsyncTriggerBase<int>
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x0000BCE8 File Offset: 0x00009EE8
		private void OnAnimatorIK(int layerIndex)
		{
			base.RaiseEvent(layerIndex);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000BCF1 File Offset: 0x00009EF1
		public IAsyncOnAnimatorIKHandler GetOnAnimatorIKAsyncHandler()
		{
			return new AsyncTriggerHandler<int>(this, false);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000BCFA File Offset: 0x00009EFA
		public IAsyncOnAnimatorIKHandler GetOnAnimatorIKAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<int>(this, cancellationToken, false);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000BD04 File Offset: 0x00009F04
		public UniTask<int> OnAnimatorIKAsync()
		{
			return ((IAsyncOnAnimatorIKHandler)new AsyncTriggerHandler<int>(this, true)).OnAnimatorIKAsync();
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000BD12 File Offset: 0x00009F12
		public UniTask<int> OnAnimatorIKAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnAnimatorIKHandler)new AsyncTriggerHandler<int>(this, cancellationToken, true)).OnAnimatorIKAsync();
		}
	}
}
