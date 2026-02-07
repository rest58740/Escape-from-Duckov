using System;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000A1 RID: 161
	[DisallowMultipleComponent]
	public sealed class AsyncGUITrigger : AsyncTriggerBase<AsyncUnit>
	{
		// Token: 0x06000490 RID: 1168 RVA: 0x0000C26B File Offset: 0x0000A46B
		private void OnGUI()
		{
			base.RaiseEvent(AsyncUnit.Default);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000C278 File Offset: 0x0000A478
		public IAsyncOnGUIHandler GetOnGUIAsyncHandler()
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, false);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000C281 File Offset: 0x0000A481
		public IAsyncOnGUIHandler GetOnGUIAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000C28B File Offset: 0x0000A48B
		public UniTask OnGUIAsync()
		{
			return ((IAsyncOnGUIHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnGUIAsync();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000C299 File Offset: 0x0000A499
		public UniTask OnGUIAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnGUIHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnGUIAsync();
		}
	}
}
