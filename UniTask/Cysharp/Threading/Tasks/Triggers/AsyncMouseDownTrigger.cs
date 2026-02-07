using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000A7 RID: 167
	[DisallowMultipleComponent]
	public sealed class AsyncMouseDownTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x0000C332 File Offset: 0x0000A532
		private void OnMouseDown()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000C33F File Offset: 0x0000A53F
		public IAsyncOnMouseDownHandler GetOnMouseDownAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000C348 File Offset: 0x0000A548
		public IAsyncOnMouseDownHandler GetOnMouseDownAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000C352 File Offset: 0x0000A552
		public UniTask OnMouseDownAsync()
		{
			return ((IAsyncOnMouseDownHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnMouseDownAsync();
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000C360 File Offset: 0x0000A560
		public UniTask OnMouseDownAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnMouseDownHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnMouseDownAsync();
		}
	}
}
