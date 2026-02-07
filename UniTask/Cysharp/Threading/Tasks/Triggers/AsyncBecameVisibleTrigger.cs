using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000085 RID: 133
	[DisallowMultipleComponent]
	public sealed class AsyncBecameVisibleTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x0600042E RID: 1070 RVA: 0x0000BEC1 File Offset: 0x0000A0C1
		private void OnBecameVisible()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000BECE File Offset: 0x0000A0CE
		public IAsyncOnBecameVisibleHandler GetOnBecameVisibleAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000BED7 File Offset: 0x0000A0D7
		public IAsyncOnBecameVisibleHandler GetOnBecameVisibleAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000BEE1 File Offset: 0x0000A0E1
		public UniTask OnBecameVisibleAsync()
		{
			return ((IAsyncOnBecameVisibleHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnBecameVisibleAsync();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000BEEF File Offset: 0x0000A0EF
		public UniTask OnBecameVisibleAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnBecameVisibleHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnBecameVisibleAsync();
		}
	}
}
