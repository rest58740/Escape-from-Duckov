using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000AF RID: 175
	[DisallowMultipleComponent]
	public sealed class AsyncMouseOverTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004C1 RID: 1217 RVA: 0x0000C446 File Offset: 0x0000A646
		private void OnMouseOver()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000C453 File Offset: 0x0000A653
		public IAsyncOnMouseOverHandler GetOnMouseOverAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000C45C File Offset: 0x0000A65C
		public IAsyncOnMouseOverHandler GetOnMouseOverAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000C466 File Offset: 0x0000A666
		public UniTask OnMouseOverAsync()
		{
			return ((IAsyncOnMouseOverHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnMouseOverAsync();
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000C474 File Offset: 0x0000A674
		public UniTask OnMouseOverAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnMouseOverHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnMouseOverAsync();
		}
	}
}
