using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000AB RID: 171
	[DisallowMultipleComponent]
	public sealed class AsyncMouseEnterTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x0000C3BC File Offset: 0x0000A5BC
		private void OnMouseEnter()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000C3C9 File Offset: 0x0000A5C9
		public IAsyncOnMouseEnterHandler GetOnMouseEnterAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000C3D2 File Offset: 0x0000A5D2
		public IAsyncOnMouseEnterHandler GetOnMouseEnterAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000C3DC File Offset: 0x0000A5DC
		public UniTask OnMouseEnterAsync()
		{
			return ((IAsyncOnMouseEnterHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnMouseEnterAsync();
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000C3EA File Offset: 0x0000A5EA
		public UniTask OnMouseEnterAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnMouseEnterHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnMouseEnterAsync();
		}
	}
}
