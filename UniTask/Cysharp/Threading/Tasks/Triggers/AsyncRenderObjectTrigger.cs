using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000C9 RID: 201
	[DisallowMultipleComponent]
	public sealed class AsyncRenderObjectTrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x0600051C RID: 1308 RVA: 0x0000C7C1 File Offset: 0x0000A9C1
		private void OnRenderObject()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000C7CE File Offset: 0x0000A9CE
		public IAsyncOnRenderObjectHandler GetOnRenderObjectAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000C7D7 File Offset: 0x0000A9D7
		public IAsyncOnRenderObjectHandler GetOnRenderObjectAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000C7E1 File Offset: 0x0000A9E1
		public UniTask OnRenderObjectAsync()
		{
			return ((IAsyncOnRenderObjectHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnRenderObjectAsync();
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000C7EF File Offset: 0x0000A9EF
		public UniTask OnRenderObjectAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnRenderObjectHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnRenderObjectAsync();
		}
	}
}
