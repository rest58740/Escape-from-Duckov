using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000073 RID: 115
	[DisallowMultipleComponent]
	public sealed class AsyncFixedUpdateTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060003EF RID: 1007 RVA: 0x0000BC5E File Offset: 0x00009E5E
		private void FixedUpdate()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000BC6B File Offset: 0x00009E6B
		public IAsyncFixedUpdateHandler GetFixedUpdateAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000BC74 File Offset: 0x00009E74
		public IAsyncFixedUpdateHandler GetFixedUpdateAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000BC7E File Offset: 0x00009E7E
		public UniTask FixedUpdateAsync()
		{
			return ((IAsyncFixedUpdateHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).FixedUpdateAsync();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000BC8C File Offset: 0x00009E8C
		public UniTask FixedUpdateAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncFixedUpdateHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).FixedUpdateAsync();
		}
	}
}
