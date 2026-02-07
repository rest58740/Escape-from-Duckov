using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000AD RID: 173
	[DisallowMultipleComponent]
	public sealed class AsyncMouseExitTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x0000C401 File Offset: 0x0000A601
		private void OnMouseExit()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000C40E File Offset: 0x0000A60E
		public IAsyncOnMouseExitHandler GetOnMouseExitAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000C417 File Offset: 0x0000A617
		public IAsyncOnMouseExitHandler GetOnMouseExitAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000C421 File Offset: 0x0000A621
		public UniTask OnMouseExitAsync()
		{
			return ((IAsyncOnMouseExitHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnMouseExitAsync();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000C42F File Offset: 0x0000A62F
		public UniTask OnMouseExitAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnMouseExitHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnMouseExitAsync();
		}
	}
}
