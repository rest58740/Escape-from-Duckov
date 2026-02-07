using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000A9 RID: 169
	[DisallowMultipleComponent]
	public sealed class AsyncMouseDragTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004AC RID: 1196 RVA: 0x0000C377 File Offset: 0x0000A577
		private void OnMouseDrag()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000C384 File Offset: 0x0000A584
		public IAsyncOnMouseDragHandler GetOnMouseDragAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000C38D File Offset: 0x0000A58D
		public IAsyncOnMouseDragHandler GetOnMouseDragAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000C397 File Offset: 0x0000A597
		public UniTask OnMouseDragAsync()
		{
			return ((IAsyncOnMouseDragHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnMouseDragAsync();
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000C3A5 File Offset: 0x0000A5A5
		public UniTask OnMouseDragAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnMouseDragHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnMouseDragAsync();
		}
	}
}
