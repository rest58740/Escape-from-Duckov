using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000B1 RID: 177
	[DisallowMultipleComponent]
	public sealed class AsyncMouseUpTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004C8 RID: 1224 RVA: 0x0000C48B File Offset: 0x0000A68B
		private void OnMouseUp()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000C498 File Offset: 0x0000A698
		public IAsyncOnMouseUpHandler GetOnMouseUpAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000C4A1 File Offset: 0x0000A6A1
		public IAsyncOnMouseUpHandler GetOnMouseUpAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0000C4AB File Offset: 0x0000A6AB
		public UniTask OnMouseUpAsync()
		{
			return ((IAsyncOnMouseUpHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnMouseUpAsync();
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000C4B9 File Offset: 0x0000A6B9
		public UniTask OnMouseUpAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnMouseUpHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnMouseUpAsync();
		}
	}
}
