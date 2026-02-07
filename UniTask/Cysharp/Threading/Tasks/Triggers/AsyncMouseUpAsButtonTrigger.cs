using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000B3 RID: 179
	[DisallowMultipleComponent]
	public sealed class AsyncMouseUpAsButtonTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x0000C4D0 File Offset: 0x0000A6D0
		private void OnMouseUpAsButton()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000C4DD File Offset: 0x0000A6DD
		public IAsyncOnMouseUpAsButtonHandler GetOnMouseUpAsButtonAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0000C4E6 File Offset: 0x0000A6E6
		public IAsyncOnMouseUpAsButtonHandler GetOnMouseUpAsButtonAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
		public UniTask OnMouseUpAsButtonAsync()
		{
			return ((IAsyncOnMouseUpAsButtonHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnMouseUpAsButtonAsync();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000C4FE File Offset: 0x0000A6FE
		public UniTask OnMouseUpAsButtonAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnMouseUpAsButtonHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnMouseUpAsButtonAsync();
		}
	}
}
